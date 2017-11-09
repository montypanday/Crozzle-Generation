using System.Data;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Crozzle
{
    class CrozzleGrid
    {
        private readonly string bestWord;
        #region Properties

        private WordList WordList { get; set; }
        private List<string> Words { get; set; }
        private List<string> TemporaryWords { get; set; }
        private List<string> TemporaryWordsAlreadyInCrozzle { get; set; }
        private Node BestNode { get; set; }
        private Node RootNode { get; set; }
        private List<Node> ListOfNodes { get; set; }
        public Configuration Config { get; set; }
        private List<char> costSortedAlphabets { get; set; }
        private List<char> AllSortedAlphabets { get; set; }
        private List<char> nonintersectingcostSortedAlphabets { get; set; }
        private string URL { get; set; }
        public string Path { get; set; }
        private string ConfigurationURL { get; set; }
        private string WordlistURL { get; set; }
        private int Rows { get; set; }
        private int Columns { get; set; }
        private List<Word> CrozzleWords { get; set; }
        private List<Word> TemporaryCrozzleWords { get; set; }
        private char[,] Array { get; set; }
        private int Score { get; set; }
        private char[,] TemporaryArray { get; set; }
        private char[,] TemporaryPaddedArray { get; set; }
        #endregion
        public CrozzleGrid() { }
        public CrozzleGrid(string str)
        {
            URL = str;
        }

        /// <summary>
        /// This method is used to read the czl file.
        /// </summary>
        public void ReadFile()
        {
            FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.StartsWith("//")) { continue; }
                else
                {
                    if (line.Length > 0)
                    {
                        string[] keyAndValue = line.Split(new char[] { '=', ',' });
                        if (keyAndValue.Length == 2)
                        {
                            if (keyAndValue[1].StartsWith("\""))
                            {
                                keyAndValue[1] = keyAndValue[1].Substring(1, keyAndValue[1].Length - 2);
                            }
                            try
                            {
                                switch (keyAndValue[0])
                                {
                                    case "ROWS": { Rows = Convert.ToInt32(keyAndValue[1]); break; }
                                    case "COLUMNS": { Columns = Convert.ToInt32(keyAndValue[1]); break; }
                                    case "CONFIGURATION_FILE": { ConfigurationURL = keyAndValue[1]; break; }
                                    case "WORDLIST_FILE": { WordlistURL = keyAndValue[1]; break; }
                                }
                            }
                            catch (FormatException) { MessageBox.Show("Wrong Format: Files Cannot be read!"); }
                        }
                    }
                }
            }
            fs.Close();
            sr.Close();
        }

        /// <summary>
        /// This method is used to read a .czl file from a local machine.
        /// </summary>
        public void ReadWordsFromLocalFile()
        {
            FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.StartsWith("//")) { continue; }
                else
                {
                    if (line.Length > 0)
                    {
                        string[] keyAndValue = line.Split(new char[] { '=', ',' });
                        if (keyAndValue.Length == 4)
                        {

                            if (keyAndValue[0] == "COLUMN")
                            {
                                CrozzleWords.Add(new Word(keyAndValue[2], "Vertical", Convert.ToInt32(keyAndValue[3]), Convert.ToInt32(keyAndValue[1]), false));
                                Insert(keyAndValue[2], "Vertical", Convert.ToInt32(keyAndValue[3]), Convert.ToInt32(keyAndValue[1]));
                            }
                            else
                            {
                                CrozzleWords.Add(new Word(keyAndValue[2], "Horizontal", Convert.ToInt32(keyAndValue[1]), Convert.ToInt32(keyAndValue[3]), false));
                                Insert(keyAndValue[2], "Horizontal", Convert.ToInt32(keyAndValue[1]), Convert.ToInt32(keyAndValue[3]));
                            }
                        }
                    }
                }
            }
            fs.Close();
            sr.Close();
            Array = TemporaryArray;

        }

        /// <summary>
        /// This method is used to read the remote Crozzle file.
        /// </summary>
        public void ReadCrozzle()
        {
            WebClient webClient = new WebClient();
            Stream aStream = webClient.OpenRead(URL);
            StreamReader aStreamReader = new StreamReader(aStream);

            // Process each line.
            string line;
            while ((line = aStreamReader.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.StartsWith("//"))
                {
                    continue;
                }
                else
                {
                    if (line.Length > 0)
                    {
                        string[] keyAndValue = line.Split(new char[] { '=', ',' });
                        if (keyAndValue.Length == 2)
                        {
                            if (keyAndValue[1].StartsWith("\""))
                            {
                                keyAndValue[1] = keyAndValue[1].Substring(1, keyAndValue[1].Length - 2);
                            }
                            try
                            {
                                switch (keyAndValue[0])
                                {
                                    case "ROWS": { Rows = Convert.ToInt32(keyAndValue[1]); break; }
                                    case "COLUMNS": { Columns = Convert.ToInt32(keyAndValue[1]); break; }
                                    case "CONFIGURATION_FILE": { ConfigurationURL = keyAndValue[1]; break; }
                                    case "WORDLIST_FILE": { WordlistURL = keyAndValue[1]; break; }
                                }
                            }
                            catch (FormatException) { }
                        }

                    }
                }
            }
            // Close both streams.
            aStreamReader.Close();
            Load();
        }

        /// <summary>
        /// This method is used to initialize essential variables and load Configuration.
        /// </summary>
        public void Load()
        {
            WordList = new WordList(WordlistURL);
            Config = new Configuration(ConfigurationURL);
            costSortedAlphabets = new List<char>();
            nonintersectingcostSortedAlphabets = new List<char>();

            Words = new List<string>();
            CrozzleWords = new List<Word>();
            Array = new char[Rows, Columns];

            TemporaryArray = new char[Rows, Columns];
            TemporaryCrozzleWords = new List<Word>();
            TemporaryWords = new List<string>();
            TemporaryPaddedArray = GetPaddedArray(Array);
            ListOfNodes = new List<Node>();
            BestNode = new Node();
            AllSortedAlphabets = new List<char>();

            foreach (KeyValuePair<string, int> item in Config.INTERSECTING_POINTS_PER_LETTER.OrderByDescending(key => key.Value))
            {
                if (item.Value > Config.NON_INTERSECTING_POINTS_PER_LETTER[item.Key])
                {
                    costSortedAlphabets.Add(Convert.ToChar(item.Key));
                }
                AllSortedAlphabets.Add(Convert.ToChar(item.Key));
            }
            if (costSortedAlphabets.Count == 0)
            {
                foreach (KeyValuePair<string, int> item in Config.NON_INTERSECTING_POINTS_PER_LETTER)
                {
                    if (item.Value < 10)
                    {
                        costSortedAlphabets.Add(Convert.ToChar(item.Key));
                    }
                }
            }
            foreach (KeyValuePair<string, int> item in Config.NON_INTERSECTING_POINTS_PER_LETTER.OrderByDescending(key => key.Value))
            {
                nonintersectingcostSortedAlphabets.Add(Convert.ToChar(item.Key));
            }
        }

        /// <summary>
        /// This method fills our Crozzle with words.
        /// </summary>
        public void Fill()
        {
            foreach (var a in WordList.Table)
            {
                Words.Add(a.Key);
            }
            TemporaryArray = new char[Rows, Columns];
            TemporaryCrozzleWords = new List<Word>();
            TemporaryWords = new List<string>();
            TemporaryPaddedArray = GetPaddedArray(TemporaryArray);
            TemporaryWordsAlreadyInCrozzle = new List<string>();
            ListOfNodes = new List<Node>();

            Random rndm = new Random();
            var myfavWords = costSortedAlphabets;
            var alphabet = myfavWords[0];
            TemporaryWords = Words.OrderByDescending(xm => int.Parse(xm.Split(alphabet).Count().ToString())).ToList();
            var list = new List<string>();
            foreach (string str in Words)
            {
                if (str.ToUpper().Contains("Z") || str.ToUpper().Contains("Y") || str.ToUpper().Contains("X") || str.ToUpper().Contains("W") || str.ToUpper().Contains("V"))
                {
                    TemporaryWords.Add(str);

                }
                var indexofZ = FindIndexes(str, 'Z');
                if (indexofZ.Count() >= 2)
                {
                    list.Add(str);
                }
            }
            if (list.Count == 0)
            {
                list = TemporaryWords;
            }


            bool successful = false;
            while (successful == false)
            {
                int x = rndm.Next(1, Rows);
                int y = rndm.Next(1, Columns);
                int index = rndm.Next(0, list.Count - 1);


                if (TryFitFirstHorizontalWord(x, y, 0, list[index]))
                {
                    if (TemporaryWordsAlreadyInCrozzle.Contains(list[index]) == false)
                    {
                        Insert(list[index], "Horizontal", x, y);
                        TemporaryCrozzleWords.Add(new Word(list[index], "Horizontal", x, y, false));
                        TemporaryWords.Remove(list[index]);
                        TemporaryWordsAlreadyInCrozzle.Add(list[index]);
                        if (TemporaryCrozzleWords.Count == Config.MAXIMUM_NUMBER_OF_GROUPS)
                        {
                            successful = true;
                        }
                    }
                }
            }
            if (Config.INTERSECTING_POINTS_PER_LETTER["Z"] == 0)
            {
                RootNode = new Node(TemporaryArray, TemporaryCrozzleWords);
                BestNode = RootNode;
                InsertChildren(RootNode);
                if (BestNode.GetScore(Config) > Score)
                {
                    Array = BestNode.Array;
                    CrozzleWords = BestNode.CrozzleWords;
                    Score = BestNode.Score;
                }
            }
            else
            {
                for (int i = 0; i < 10; i++) { DoFillingWork(alphabet); }
                char fav = 'Z';
                if (Config.INTERSECTING_POINTS_PER_LETTER[fav.ToString()] != 0)
                {
                    for (int i = 0; i < 10; i++) { DoSimpleFilling(); }
                }

                int TemporaryArrayScore = GetScore(TemporaryArray);
                Score = GetScore(Array);
                if (Score < TemporaryArrayScore)
                {
                    Array = TemporaryArray;
                    CrozzleWords = TemporaryCrozzleWords.ToList();
                    Score = TemporaryArrayScore;
                }
            }
        }

        /// <summary>
        /// This method is use to find all indexes or a character in the given word.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private static IEnumerable<int> FindIndexes(string word, char c)
        {
            var query = c.ToString();
            return Enumerable.Range(0, word.Length - query.Length)
                .Where(i => query.Equals(word.Substring(i, query.Length)));
        }

        /// <summary>
        /// This method does the filling work, but only for those characters which are our favourite for forming Intersections.
        /// </summary>
        /// <param name="alphabet"></param>
        private void DoFillingWork(char alphabet)
        {
            foreach (Word w in TemporaryCrozzleWords.ToList())
            {
                if (w.IsProcessed == false && w.Type == "Horizontal")
                {
                    int x = w.Startx;
                    int y = w.Starty;
                    foreach (Char c in costSortedAlphabets)
                    {
                        if (w.Value.Contains(c))
                        {
                            var allindexes = FindIndexes(w.Value, c);
                            foreach (int index in allindexes)
                            {
                                if (CheckIfIntersection(x, y + index) == false)
                                {
                                    var listOfIntersections = WordList.GetIntersections(w.Value, c);
                                    listOfIntersections = listOfIntersections.Intersect(TemporaryWords).ToList();
                                    listOfIntersections = listOfIntersections.OrderByDescending(s => int.Parse(s.Split(alphabet).Count().ToString())).ToList();

                                    string bestWord = "";
                                    int bestScore = 0;
                                    int chosenIndex = 0;
                                    foreach (string word in listOfIntersections)
                                    {
                                        var allIndexesInChosenWord = FindIndexes(word, c);
                                        foreach (int chosenindex in allIndexesInChosenWord)
                                        {
                                            if (TryFitVerticalWord(x, y + index, chosenindex, word) && TemporaryWordsAlreadyInCrozzle.Contains(word) == false)
                                            {
                                                int wordCost = GetCost(word);
                                                if (wordCost > bestScore)
                                                {
                                                    bestWord = word;
                                                    chosenIndex = chosenindex;
                                                    bestScore = wordCost;
                                                }
                                            }
                                        }
                                    }
                                    if (bestScore != 0 || bestWord != "")
                                    {
                                        TemporaryCrozzleWords.Add(new Word(bestWord, "Vertical", x - chosenIndex, y + index, false));
                                        Insert(bestWord, "Vertical", x - chosenIndex, y + index);
                                        TemporaryWords.Remove(bestWord);
                                        TemporaryWordsAlreadyInCrozzle.Add(bestWord);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (Word w in TemporaryCrozzleWords.ToList())
            {
                if (w.IsProcessed == false && w.Type == "Vertical")
                {
                    int x = w.Startx;
                    int y = w.Starty;
                    foreach (Char c in costSortedAlphabets)
                    {
                        if (w.Value.Contains(c))
                        {
                            var allindexes = FindIndexes(w.Value, c);
                            foreach (int index in allindexes)
                            {
                                if (CheckIfIntersection(x + index, y) == false)
                                {
                                    var listOfIntersections = WordList.GetIntersections(w.Value, c);
                                    listOfIntersections = listOfIntersections.Intersect(TemporaryWords).ToList();
                                    listOfIntersections = listOfIntersections.OrderByDescending(s => int.Parse(s.Split(alphabet).Count().ToString())).ToList();

                                    string bestWord = "";
                                    int bestScore = 0;
                                    int chosenIndex = 0;

                                    foreach (string word in listOfIntersections)
                                    {
                                        var allIndexesInChosenWord = FindIndexes(word, c);
                                        foreach (int chosenindex in allIndexesInChosenWord)
                                        {
                                            if (TryFitHorizontalWord(x + index, y, chosenindex, word) && TemporaryWordsAlreadyInCrozzle.Contains(word) == false)
                                            {
                                                int wordCost = GetCost(word);
                                                if (wordCost > bestScore)
                                                {
                                                    bestWord = word;
                                                    bestScore = wordCost;
                                                    chosenIndex = chosenindex;
                                                }
                                            }
                                        }
                                    }
                                    if (bestScore != 0 || bestWord != "")
                                    {
                                        TemporaryCrozzleWords.Add(new Word(bestWord, "Horizontal", x + index, y - chosenIndex, false));
                                        Insert(bestWord, "Horizontal", x + index, y - chosenIndex);
                                        TemporaryWords.Remove(bestWord);
                                        TemporaryWordsAlreadyInCrozzle.Add(bestWord);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// For each character, it takes the higher value from Non Intersecting or Intersecting Costs and returns sum of those costs.
        /// </summary>
        /// <param name="word"></param>
        /// <returns>int</returns>
        private int GetCost(string word)
        {
            int score = 0;
            foreach (char c in word)
            {
                if (Config.INTERSECTING_POINTS_PER_LETTER[c.ToString()] > Config.NON_INTERSECTING_POINTS_PER_LETTER[c.ToString()])
                {
                    score += Config.INTERSECTING_POINTS_PER_LETTER[c.ToString()];
                }
                else
                {
                    score += Config.NON_INTERSECTING_POINTS_PER_LETTER[c.ToString()];
                }
            }
            return score;
        }

        /// <summary>
        /// This method is used to check whether the given Horizontal word can fit. Unlike another similar method, it also checks the upper and lower cell
        /// of the intersecting cell.
        /// </summary>
        /// <param name="intersectX"></param>
        /// <param name="intersectY"></param>
        /// <param name="chosenIndex"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool TryFitFirstHorizontalWord(int intersectX, int intersectY, int chosenIndex, string word)
        {
            int position = chosenIndex + 1;
            int lengthAfterIntersection = word.Length - position;
            int lengthBeforeIntersection = word.Remove(chosenIndex).Count();
            int actualintX = intersectX - 1;
            int actualintY = intersectY - 1;
            char[] wordCharArray = word.ToCharArray();

            if (lengthBeforeIntersection > actualintY)
            {
                return false;
            }
            if ((actualintY + lengthAfterIntersection) >= Columns)
            {
                return false;
            }

            int index = 0;
            for (int i = (actualintY - lengthBeforeIntersection); index < word.Length; i++)
            {
                if (i == actualintY - lengthBeforeIntersection)
                {
                    if (i >= 1)
                        if (TemporaryArray[actualintX, i - 1] != '\0') { return false; }
                }
                if (index == word.Length - 1)
                {
                    if (i < Columns && i != Columns - 1)
                    {
                        if (TemporaryArray[actualintX, i + 1] != '\0') { return false; }
                    }
                }
                if (i == actualintY)
                {
                    if (TemporaryArray[actualintX, actualintY] == '\0' || TemporaryArray[actualintX, actualintY] == wordCharArray[index]) { }
                    else { return false; }
                }

                if (actualintX != 0)
                {
                    if (TemporaryArray[actualintX - 1, i] != '\0') { return false; }
                }

                if (TemporaryArray[actualintX, i] == '\0' || TemporaryArray[actualintX, i] == wordCharArray[index]) { }
                else { return false; }
                if (actualintX != Rows - 1)
                {
                    if (TemporaryArray[actualintX + 1, i] != '\0') { return false; }
                }

                index++;
            }
            return true;
        }

        /// <summary>
        /// This method helps decide whether the given work can be put Vertically at that place and successfully intersect at the chosenIndex.
        /// </summary>
        /// <param name="intersectX"></param>
        /// <param name="intersectY"></param>
        /// <param name="chosenIndex"></param>
        /// <param name="word"></param>
        /// <returns>bool</returns>
        private bool TryFitVerticalWord(int intersectX, int intersectY, int chosenIndex, string word)
        {
            int position = chosenIndex + 1;
            int lengthBelowIntersection = word.Length - position;
            int lengthAboveIntersection = word.Remove(chosenIndex).Count();
            int actualintX = intersectX - 1;
            int actualintY = intersectY - 1;
            char[] wordCharArray = word.ToCharArray();

            if (lengthAboveIntersection >= intersectX || (intersectX + lengthBelowIntersection) > Rows)
            {
                return false;
            }
            int index = 0;
            for (int i = (actualintX - lengthAboveIntersection); index < word.Length; i++)
            {
                if (i == (actualintX - lengthAboveIntersection))
                {
                    if (i != 0) { if (TemporaryArray[i - 1, actualintY] != '\0') { return false; } }
                }
                if (index == word.Length - 1)
                {
                    if (i < Rows && i != Rows - 1)
                    {
                        if (TemporaryArray[i + 1, actualintY] != '\0') { return false; }
                    }
                }
                if (i != actualintX)
                {
                    if (actualintY != 0)
                    {
                        if (TemporaryArray[i, actualintY - 1] != '\0') { return false; }
                    }
                    if (TemporaryArray[i, actualintY] == '\0' || TemporaryArray[i, actualintY] == wordCharArray[index]) { }
                    else { return false; }
                    if (actualintY + 1 < Columns) { if (TemporaryArray[i, actualintY + 1] != '\0') { return false; } }
                }
                index++;
            }
            return true;
            // }
            //catch (Exception) { return false; }
        }

        /// <summary>
        /// This method helps decide whether the given work can be put Horizontally at that place and successfully intersect at the chosenIndex.
        /// </summary>
        /// <param name="intersectX"></param>
        /// <param name="intersectY"></param>
        /// <param name="chosenIndex"></param>
        /// <param name="word"></param>
        /// <returns>bool</returns>
        private bool TryFitHorizontalWord(int intersectX, int intersectY, int chosenIndex, string word)
        {
            int position = chosenIndex + 1;
            int lengthAfterIntersection = word.Length - position;
            int lengthBeforeIntersection = word.Remove(chosenIndex).Count();
            int actualintX = intersectX - 1;
            int actualintY = intersectY - 1;
            char[] wordCharArray = word.ToCharArray();

            if (lengthBeforeIntersection > actualintY)
            {
                return false;
            }
            if ((actualintY + lengthAfterIntersection) >= Columns)
            {
                return false;
            }

            int index = 0;
            for (int i = (actualintY - lengthBeforeIntersection); index < word.Length; i++)
            {
                if (i == actualintY - lengthBeforeIntersection)
                {
                    if (i >= 1)
                        if (TemporaryArray[actualintX, i - 1] != '\0') { return false; }
                }
                if (index == word.Length - 1)
                {
                    if (i < Columns && i != Columns - 1)
                    {
                        if (TemporaryArray[actualintX, i + 1] != '\0') { return false; }
                    }
                }
                if (i == actualintY)
                {
                    if (TemporaryArray[actualintX, actualintY] == '\0' || TemporaryArray[actualintX, actualintY] == wordCharArray[index]) { }
                    else { return false; }
                }
                if (i != actualintY)
                {
                    if (actualintX != 0)
                    {
                        if (TemporaryArray[actualintX - 1, i] != '\0') { return false; }
                    }

                    if (TemporaryArray[actualintX, i] == '\0' || TemporaryArray[actualintX, i] == wordCharArray[index]) { }
                    else { return false; }
                    if (actualintX != Rows - 1)
                    {
                        if (TemporaryArray[actualintX + 1, i] != '\0') { return false; }
                    }
                }
                index++;
            }
            return true;
        }

        /// <summary>
        /// This method inserts the value in both Array and PaddedArray.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="startx"></param>
        /// <param name="starty"></param>
        private void Insert(string value, string type, int startx, int starty)
        {
            //  try
            //{
            int x = startx;
            int y = starty;
            if (type == "Horizontal")
            {
                foreach (char c in value)
                {
                    TemporaryArray[x - 1, y - 1] = c;
                    TemporaryPaddedArray[x, y] = c;
                    y++;
                }
            }
            if (type == "Vertical")
            {
                foreach (char c in value)
                {
                    TemporaryArray[x - 1, y - 1] = c;
                    TemporaryPaddedArray[x, y] = c;
                    x++;
                }
            }
            // }
            // catch (Exception)
            // { }

        }

        /// <summary>
        /// This method returns a string to be displayed to Web Browser
        /// </summary>
        /// <returns>string</returns>
        public string GetHtmlTable()
        {
            string html = "<html><head>";
            html += Config.Style;
            html += "</head><body>";
            html += GetTableContents();
            html += "</body></html>";
            return html;
        }

        /// <summary>
        /// This method returns the string which is effectively the html we want to display in the webBrowser
        /// </summary>
        /// <returns>string</returns>
        private string GetTableContents()
        {

            var contents = "<table>";
            for (int r = 0; r < Rows; r++)
            {
                contents += "<tr>";
                for (int c = 0; c < Columns; c++)
                {

                    if (Array[r, c] != '\0')
                    {
                        contents += "<td bgcolor='" + Config.BGCOLOUR_NON_EMPTY_TD + "'>" + Array[r, c] + "</td>";
                    }
                    else
                    {
                        contents += "<td bgcolor='" + Config.BGCOLOUR_EMPTY_TD + "'>" + Array[r, c] + "</td>";
                    }
                }
                contents += "</tr>";
            }
            contents += "</table>";
            return contents;
        }

        /// <summary>
        /// This method accepts a char[,] and returns a new one after inserting Padding to it. This padding is necessary.
        /// </summary>
        /// <param name="array"></param>
        /// <returns>char[,]</returns>
        private char[,] GetPaddedArray(char[,] array)
        {
            char[,] paddedArray = new char[array.GetLength(0) + 2, array.GetLength(1) + 2];
            for (int x = 1; x <= paddedArray.GetLength(0) - 2; x++)
            {
                for (int y = 1; y <= paddedArray.GetLength(1) - 2; y++)
                {
                    paddedArray[x, y] = array[x - 1, y - 1];
                }
            }
            return paddedArray;
        }

        /// <summary>
        /// This method returns the Score of the Array. 
        /// </summary>
        /// <param name="thisarray"></param>
        /// <returns>int</returns>
        private int GetScore(char[,] thisarray)
        {
            int score = 0;
            int number_of_words = CrozzleWords.Count;

            if (number_of_words > 0)
            {
                score = number_of_words * Config.POINTS_PER_WORD;
            }

            char[,] array = thisarray;

            // this array will be used to form another array, with bigger dimension.
            // Because we need to search each array[x,y] if it is an intersection, 
            //so we need to add some sort of padding to prevent index out of range exception.
            char[,] paddedArray = GetPaddedArray(array);

            String intersections = "";
            String non_intersections = "";

            for (int r = 1; r <= paddedArray.GetLength(0) - 2; r++)
            {
                for (int c = 1; c <= paddedArray.GetLength(1) - 2; c++)
                {
                    if (paddedArray[r, c] != '\0')
                    {
                        if ((paddedArray[r, c - 1] != '\0' && paddedArray[r - 1, c] != '\0') ||
                          (paddedArray[r, c - 1] != '\0' && paddedArray[r + 1, c] != '\0') ||
                          (paddedArray[r, c + 1] != '\0' && paddedArray[r - 1, c] != '\0') ||
                          (paddedArray[r, c + 1] != '\0' && paddedArray[r + 1, c] != '\0'))
                        {
                            intersections += paddedArray[r, c];
                        }
                        else
                        {
                            non_intersections += paddedArray[r, c];
                        }
                    }
                }
            }

            foreach (char ch in intersections)
            {
                score += Config.INTERSECTING_POINTS_PER_LETTER[ch.ToString()];
            }

            foreach (char ch in non_intersections)
            {
                score += Config.NON_INTERSECTING_POINTS_PER_LETTER[ch.ToString()];
            }
            return score;
        }

        /// <summary>
        /// This method calculates the score according to our given Configuration.
        /// </summary>
        /// <returns>int</returns>
        public int CalculateScore()
        {
            int score = 0;
            int number_of_words = CrozzleWords.Count;

            if (number_of_words > 0)
            {
                score = number_of_words * Config.POINTS_PER_WORD;
            }

            char[,] array = Array;

            // this array will be used to form another array, with bigger dimension.
            // Because we need to search each array[x,y] if it is an intersection, 
            //so we need to add some sort of padding to prevent index out of range exception.
            char[,] paddedArray = GetPaddedArray(array);

            String intersections = "";
            String non_intersections = "";

            for (int r = 1; r <= paddedArray.GetLength(0) - 2; r++)
            {
                for (int c = 1; c <= paddedArray.GetLength(1) - 2; c++)
                {
                    if (paddedArray[r, c] != '\0')
                    {
                        if ((paddedArray[r, c - 1] != '\0' && paddedArray[r - 1, c] != '\0') ||
                          (paddedArray[r, c - 1] != '\0' && paddedArray[r + 1, c] != '\0') ||
                          (paddedArray[r, c + 1] != '\0' && paddedArray[r - 1, c] != '\0') ||
                          (paddedArray[r, c + 1] != '\0' && paddedArray[r + 1, c] != '\0'))
                        {
                            intersections += paddedArray[r, c];
                        }
                        else
                        {
                            non_intersections += paddedArray[r, c];
                        }
                    }
                }
            }

            foreach (char ch in intersections)
            {
                score += Config.INTERSECTING_POINTS_PER_LETTER[ch.ToString()];
            }

            foreach (char ch in non_intersections)
            {
                score += Config.NON_INTERSECTING_POINTS_PER_LETTER[ch.ToString()];
            }
            return score;
        }

        /// <summary>
        /// This method checks if cell is an existing intersection by using the Padded Array.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool CheckIfIntersection(int r, int c)
        {
            var valid = true;
            if ((TemporaryPaddedArray[r, c - 1] != '\0' && TemporaryPaddedArray[r - 1, c] != '\0') ||
                          (TemporaryPaddedArray[r, c - 1] != '\0' && TemporaryPaddedArray[r + 1, c] != '\0') ||
                          (TemporaryPaddedArray[r, c + 1] != '\0' && TemporaryPaddedArray[r - 1, c] != '\0') ||
                          (TemporaryPaddedArray[r, c + 1] != '\0' && TemporaryPaddedArray[r + 1, c] != '\0'))
            {

                valid = true;
            }
            else { valid = false; }
            return valid;
        }

        /// <summary>
        /// This method is called to do filling work for all characters. It will first form intersections at characters with highest cost.
        /// </summary>
        /// <returns></returns>
        private void DoSimpleFilling()
        {
            foreach (Word w in TemporaryCrozzleWords.ToList())
            {
                if (w.IsProcessed == false)
                {
                    if (w.Type == "Horizontal")
                    {
                        int x = w.Startx;
                        int y = w.Starty;

                        foreach (Char c in w.Value)
                        {
                            if (CheckIfIntersection(x, y) == false)
                            {
                                var listOfIntersections = WordList.GetIntersections(w.Value, c);

                                listOfIntersections = listOfIntersections.Intersect(TemporaryWords).ToList();
                                foreach (string word in listOfIntersections)
                                {
                                    if (TryFitVerticalWord(x, y, word.IndexOf(c), word))
                                    {
                                        if (TemporaryWordsAlreadyInCrozzle.Contains(word) == false)
                                        {
                                            TemporaryCrozzleWords.Add(new Word(word, "Vertical", x - word.IndexOf(c), y, false));
                                            Insert(word, "Vertical", x - word.IndexOf(c), y);
                                            TemporaryWords.Remove(word);
                                            TemporaryWordsAlreadyInCrozzle.Add(word);
                                            break;
                                        }
                                    }
                                }
                            }
                            // move to next column
                            y++;
                        }
                    }
                }
            }

            foreach (Word w in TemporaryCrozzleWords.ToList())
            {
                if (w.IsProcessed == false)
                {
                    if (w.Type == "Vertical")
                    {
                        int x = w.Startx;
                        int y = w.Starty;

                        foreach (Char c in w.Value)
                        {
                            if (CheckIfIntersection(x, y) == false)
                            {
                                var listOfIntersections = WordList.GetIntersections(w.Value, c);

                                listOfIntersections = listOfIntersections.Intersect(TemporaryWords).ToList();
                                foreach (string word in listOfIntersections)
                                {
                                    if (TryFitHorizontalWord(x, y, word.IndexOf(c), word))
                                    {
                                        if (TemporaryWordsAlreadyInCrozzle.Contains(word) == false)
                                        {
                                            TemporaryCrozzleWords.Add(new Word(word, "Horizontal", x, y - word.IndexOf(c), false));
                                            Insert(word, "Horizontal", x, y - word.IndexOf(c));
                                            TemporaryWords.Remove(word);
                                            TemporaryWordsAlreadyInCrozzle.Add(word);
                                            break;
                                        }
                                    }
                                }
                            }
                            // move to next column
                            x++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to return a string which will be written to the .czl file we want to save.
        /// </summary>
        /// <returns>string</returns>
        public string GetFileContents()
        {
            string str = "";

            str += "// File dependencies.\r\n" +
                "// Configuration file.\r\n" +
                "CONFIGURATION_FILE=\"" + ConfigurationURL + "\"\r\n\r\n// Word list file. \r\nWORDLIST_FILE=\"" + WordlistURL + "\"\r\n\r\n\r\n// Crozzle Size.\r\n// The number of rows and columns.\r\nROWS=" + Rows + "\r\nCOLUMNS=" + Columns + "\r\n\r\n// Word Data.\r\n// The horizontal rows containing words.\r\n";
            foreach (Word crozzleword in CrozzleWords)
            {
                if (crozzleword.Type == "Horizontal")
                {
                    str += "ROW=" + crozzleword.Startx + "," + crozzleword.Value + "," + crozzleword.Starty + "\r\n";
                }
            }

            str += "\r\n// The vertical rows containing words.\r\n";
            foreach (Word crozzleword in CrozzleWords)
            {
                if (crozzleword.Type == "Vertical")
                {
                    str += "COLUMN=" + crozzleword.Starty + "," + crozzleword.Value + "," + crozzleword.Startx + "\r\n";
                }
            }
            return str;
        }

        /// <summary>
        /// This method computes a array of strings from our two dimensional character array. Each row
        /// represents a string. The string[] length will be equal to the number of Columns.
        /// </summary>
        /// <returns>string[]</returns>
        public string[] GetGrid()
        {
            string[] grid = new string[Rows];
            for (int i = 0; i < Rows; i++)
            {
                string sentence = "";
                for (int j = 0; j < Columns; j++)
                {
                    if (Array[i, j] == '\0') { sentence += " "; }
                    else { sentence += Array[i, j]; }
                }
                grid[i] = sentence;
            }
            return grid;
        }

        /// <summary>
        /// This method accepts a node and tries to forms its children by inserting new words one at a time.
        /// This is a recursive depth first built algorithm. The memory impact is very low because local variables are used.
        /// </summary>
        /// <param name="node"></param>
        private void InsertChildren(Node node)
        {
            foreach (Word w in node.CrozzleWords)
            {
                if (w.IsProcessed == false && w.Type == "Horizontal")
                {
                    int x = w.Startx;
                    int y = w.Starty;
                    foreach (Char c in costSortedAlphabets)
                    {
                        if (w.Value.Contains(c))
                        {
                            var allindexes = FindIndexes(w.Value, c);
                            foreach (int index in allindexes)
                            {
                                if (node.CheckIfIntersection(x, y + index) == false)
                                {
                                    var listOfIntersections = WordList.GetIntersections(w.Value, c).Except(node.Words).ToList();

                                    foreach (string word in listOfIntersections)
                                    {
                                        var allIndexesInChosenWord = FindIndexes(word, c);
                                        foreach (int chosenindex in allIndexesInChosenWord)
                                        {
                                            if (node.TryFitVerticalWord(x, y + index, chosenindex, word, Rows, Columns) && TemporaryWordsAlreadyInCrozzle.Contains(word) == false)
                                            {
                                                node.SubSolutions.Add(new Word(word, "Vertical", x - chosenindex, y + index, false), GetCost(word));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (Word w in node.CrozzleWords)
            {
                if (w.IsProcessed == false && w.Type == "Vertical")
                {
                    int x = w.Startx;
                    int y = w.Starty;
                    foreach (Char c in costSortedAlphabets)
                    {
                        if (w.Value.Contains(c))
                        {
                            var allindexes = FindIndexes(w.Value, c);
                            foreach (int index in allindexes)
                            {
                                if (node.CheckIfIntersection(x + index, y) == false)
                                {
                                    var listOfIntersections = WordList.GetIntersections(w.Value, c).Except(node.Words).ToList();

                                    foreach (string word in listOfIntersections)
                                    {
                                        var allIndexesInChosenWord = FindIndexes(word, c);
                                        foreach (int chosenindex in allIndexesInChosenWord)
                                        {
                                            if (node.TryFitHorizontalWord(x + index, y, chosenindex, word, Rows, Columns) && TemporaryWordsAlreadyInCrozzle.Contains(word) == false)
                                            {
                                                node.SubSolutions.Add(new Word(word, "Horizontal", x + index, y - chosenindex, false), GetCost(word));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (node.SubSolutions.Count == 0)
            {
                if (node.GetScore(Config) > BestNode.GetScore(Config))
                {
                    BestNode = node.Copy();
                    BestNode.Score = GetScore(node.Array);
                }
            }
            else
            {
                var sorted = node.SubSolutions.OrderByDescending(k => k.Value).ToList();
                var first10sorted = sorted.Take(2);
                foreach (KeyValuePair<Word, Int32> pair in first10sorted)
                {
                    Node childnode = new Node();
                    //childnode.SubSolutions = new List<Word>();
                    childnode.SubSolutions = new Dictionary<Word, int>();
                    childnode.ListOfChildren = new List<Node>();
                    childnode.CrozzleWords = node.CrozzleWords.ToList();
                    childnode.Words = node.Words.ToList();
                    childnode.PaddedArray = new char[Rows + 2, Columns + 2];
                    childnode.Array = new char[Rows, Columns];
                    foreach (Word w in childnode.CrozzleWords)
                    {
                        childnode.Insert(w.Value, w.Type, w.Startx, w.Starty);
                    }

                    childnode.GetPaddedArray();

                    childnode.Insert(pair.Key.Value, pair.Key.Type, pair.Key.Startx, pair.Key.Starty);
                    //childnode.SubSolutions.Remove(word);
                    childnode.CrozzleWords.Add(pair.Key);
                    childnode.Words.Add(pair.Key.Value);
                    //node.addChild(childnode);
                    InsertChildren(childnode);
                }
            }

        }
    }
}
