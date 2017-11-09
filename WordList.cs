using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Crozzle
{
    class WordList
    {
        public LookupWordTable Table { get; set; }

        public WordList(string url)
        {
            populateTable(url);
        }

        private void populateTable(string url)
        {
            Table = new LookupWordTable();
            List<string> uniqueWords = getUniqueWords(url);

            foreach (string uniqueWord in uniqueWords)
            {
                LookupLetterTable letterTable = new LookupLetterTable();

                foreach (char letter in uniqueWord)
                {
                    if (!letterTable.ContainsKey(letter))
                    {
                        List<string> intersectingWords = new List<string>();
                        foreach (string word in uniqueWords)
                            if (word.IndexOf(letter) > -1)
                                if (!intersectingWords.Contains(word))
                                    intersectingWords.Add(word);
                        letterTable.Add(letter, intersectingWords);
                    }
                }

                Table.Add(uniqueWord, letterTable);
            }
        }

        public List<string> GetIntersections(string word, char letter)
        {
            List<string> words = new List<string>();

            if (Table.ContainsKey(word))
            {
                LookupLetterTable letterTable = Table[word];
                if (letterTable.ContainsKey(letter))
                    words = letterTable[letter];
            }
            return (words);
        }

        private List<string> getUniqueWords(string path)
        {
            List<string> uniqueWords = new List<string>();
            WebClient webClient = new WebClient();

            try
            {
                // Open streams to this URL file.
                Stream aStream = webClient.OpenRead(path);
                StreamReader aStreamReader = new StreamReader(aStream);

                // Process each line of the file.
                while (!aStreamReader.EndOfStream)
                {
                    string line = aStreamReader.ReadLine();
                    string[] words = line.Split(new char[] { ',' });
                    foreach (string word in words)
                        if (!uniqueWords.Contains(word))
                            uniqueWords.Add(word);
                }

                // Close streams.
                aStreamReader.Close();
            }
            catch (WebException webEx)
            {
                Console.WriteLine(webEx.Message);
            }
            catch (ArgumentNullException argNullEx)
            {
                Console.WriteLine(argNullEx.Message);
            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine(argEx.Message);
            }

            return (uniqueWords);
        }
    }
}
