using System;
using System.Collections.Generic;
using System.Linq;

namespace Crozzle
{
    class Node
    {
        public int Score { get; set; }
        public List<Word> CrozzleWords { get; set; }
        public Dictionary<Word,Int32> SubSolutions { get; set; }
        public List<string> Words { get; set; }
        public List<Node> ListOfChildren { get; set; }
        public char[,] Array { get; set; }
        public char[,] PaddedArray { get; set; }
        public Node() { }
        public Node(char[,] array, List<Word> crozzlewords)
        {
            Array = array;
            CrozzleWords = crozzlewords;
            LoadWords();
            GetPaddedArray();
            ListOfChildren = new List<Node>();
            SubSolutions = new Dictionary<Word, int>();
        }
        public void LoadWords()
        {
            Words = new List<string>();
            foreach (Word word in CrozzleWords)
            {
                
                Words.Add(word.Value);
            }
        }
        public void GetPaddedArray()
        {
            PaddedArray = new char[Array.GetLength(0) + 2, Array.GetLength(1) + 2];
            for (int x = 1; x <= PaddedArray.GetLength(0) - 2; x++)
            {
                for (int y = 1; y <= PaddedArray.GetLength(1) - 2; y++)
                {
                    PaddedArray[x, y] = Array[x - 1, y - 1];
                }
            }
        }
        public void Insert(string value, string type, int startx, int starty)
        {
            int x = startx;
            int y = starty;
            if (type == "Horizontal")
            {
                foreach (char c in value)
                {
                    Array[x - 1, y - 1] = c;
                    PaddedArray[x, y] = c;
                    y++;
                }
            }
            if (type == "Vertical")
            {
                foreach (char c in value)
                {
                    Array[x - 1, y - 1] = c;
                    PaddedArray[x, y] = c;
                    x++;
                }
            }
        }
        public bool TryFitVerticalWord(int intersectX, int intersectY, int chosenIndex, string word, int Rows, int Columns)
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
                    if (i != 0) { if (Array[i - 1, actualintY] != '\0') { return false; } }
                }
                if (index == word.Length - 1)
                {
                    if (i < Rows && i != Rows - 1)
                    {
                        if (Array[i + 1, actualintY] != '\0') { return false; }
                    }
                }
                if (i != actualintX)
                {
                    if (actualintY != 0)
                    {
                        if (Array[i, actualintY - 1] != '\0') { return false; }
                    }
                    if (Array[i, actualintY] == '\0' || Array[i, actualintY] == wordCharArray[index]) { }
                    else { return false; }
                    if (actualintY + 1 < Columns) { if (Array[i, actualintY + 1] != '\0') { return false; } }
                }
                index++;
            }
            return true;
        }
        public bool TryFitHorizontalWord(int intersectX, int intersectY, int chosenIndex, string word, int Rows, int Columns)
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
                        if (Array[actualintX, i - 1] != '\0') { return false; }
                }
                if (index == word.Length - 1)
                {
                    if (i < Columns && i != Columns - 1)
                    {
                        if (Array[actualintX, i + 1] != '\0') { return false; }
                    }
                }
                if (i == actualintY)
                {
                    if (Array[actualintX, actualintY] == '\0' || Array[actualintX, actualintY] == wordCharArray[index]) { }
                    else { return false; }
                }
                if (i != actualintY)
                {
                    if (actualintX != 0)
                    {
                        if (Array[actualintX - 1, i] != '\0') { return false; }
                    }

                    if (Array[actualintX, i] == '\0' || Array[actualintX, i] == wordCharArray[index]) { }
                    else { return false; }
                    if (actualintX != Rows - 1)
                    {
                        if (Array[actualintX + 1, i] != '\0') { return false; }
                    }
                }
                index++;
            }
            return true;
        }
        public bool CheckIfIntersection(int r, int c)
        {
            var valid = true;
            if ((PaddedArray[r, c - 1] != '\0' && PaddedArray[r - 1, c] != '\0') ||
                          (PaddedArray[r, c - 1] != '\0' && PaddedArray[r + 1, c] != '\0') ||
                          (PaddedArray[r, c + 1] != '\0' && PaddedArray[r - 1, c] != '\0') ||
                          (PaddedArray[r, c + 1] != '\0' && PaddedArray[r + 1, c] != '\0'))
            {

                valid = true;
            }
            else { valid = false; }
            return valid;
        }
        public Node Copy()
        {
            return this;
        }
        public void addChild(Node node)
        {
            ListOfChildren.Add(node);
        }
        public int GetScore(Configuration Config)
        {
            int score = 0;
            int number_of_words = CrozzleWords.Count;

            if (number_of_words > 0)
            {
                score = number_of_words * Config.POINTS_PER_WORD;
            }

            var array = Array;

            // this array will be used to form another array, with bigger dimension.
            // Because we need to search each array[x,y] if it is an intersection, 
            //so we need to add some sort of padding to prevent index out of range exception.
            char[,] paddedArray = PaddedArray;

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
            Score = score;
            return Score;
        }
    }
}
