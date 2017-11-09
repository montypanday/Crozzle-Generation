using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Crozzle
{
    class Configuration
    {
        public Dictionary<string, Int32> INTERSECTING_POINTS_PER_LETTER;// holds cost metrics for intersecting words

        public Dictionary<string, Int32> NON_INTERSECTING_POINTS_PER_LETTER;// holds cost metrics for non intersecting words

        #region Properties
        public string LOGFILE_NAME { get; set; }
        public int Rows { get; set; }
        public int RUNTIME_LIMIT { get; set; }
        public string WORD_REGEX_PATTERN { get; set; }
        public int MINIMUM_NUMBER_OF_UNIQUE_WORDS { get; set; }
        public int MAXIMUM_NUMBER_OF_UNIQUE_WORDS { get; set; }
        public string INVALID_CROZZLE_SCORE { get; set; }
        public bool UPPERCASE { get; set; }
        public string BGCOLOUR_EMPTY_TD { get; set; }
        public string BGCOLOUR_NON_EMPTY_TD { get; set; }
        public int MINIMUM_NUMBER_OF_ROWS { get; set; }
        public int MAXIMUM_NUMBER_OF_ROWS { get; set; }
        public int MINIMUM_NUMBER_OF_COLUMNS { get; set; }
        public int MAXIMUM_NUMBER_OF_COLUMNS { get; set; }
        public int MINIMUM_HORIZONTAL_WORDS { get; set; }
        public int MAXIMUM_HORIZONTAL_WORDS { get; set; }
        public int MINIMUM_VERTICAL_WORDS { get; set; }
        public int MAXIMUM_VERTICAL_WORDS { get; set; }
        public int MINIMUM_INTERSECTIONS_IN_HORIZONTAL_WORDS { get; set; }
        public int MAXIMUM_INTERSECTIONS_IN_HORIZONTAL_WORDS { get; set; }
        public int MINIMUM_INTERSECTIONS_IN_VERTICAL_WORDS { get; set; }
        public int MAXIMUM_INTERSECTIONS_IN_VERTICAL_WORDS { get; set; }
        public int MINIMUM_NUMBER_OF_THE_SAME_WORD { get; set; }
        public int MAXIMUM_NUMBER_OF_THE_SAME_WORD { get; set; }
        public int MINIMUM_NUMBER_OF_GROUPS { get; set; }
        public int MAXIMUM_NUMBER_OF_GROUPS { get; set; }
        public int POINTS_PER_WORD { get; set; }


        #endregion

        public string Style { get; set; }
        public Configuration(string url)
        {
            populateConfiguration(url);
        }

        void populateConfiguration(string url)
        {
            WebClient webClient = new WebClient();
            try
            {
                // Open streams to this URL file.
                Stream aStream = webClient.OpenRead(url);
                StreamReader aStreamReader = new StreamReader(aStream);

                // Process each line of the file.
                while (!aStreamReader.EndOfStream)
                {
                    string line = aStreamReader.ReadLine();
                    //line = line.Trim();
                    if (line.StartsWith("//"))
                    {
                        continue;
                    }
                    if (line.IndexOf('/') > 0)
                    {
                        var startOfComment = line.IndexOf('/');
                        line = line.Remove(startOfComment);
                        line = line.Trim();
                    }
                    if (line.Trim().StartsWith("INTERSECTING_POINTS_PER_LETTER"))
                    {

                        int i = line.IndexOf("\"");
                        int j = line.LastIndexOf("\"");
                        line = line.Substring(i + 1);
                        line = line.Remove(line.Length - 1);
                        INTERSECTING_POINTS_PER_LETTER = line.Split(new char[] { ',' }).Select(x => x.Split('=')).ToDictionary(y => y[0], y => Convert.ToInt32(y[1]));
                        continue;
                    }
                    if (line.Trim().StartsWith("NON_INTERSECTING_POINTS_PER_LETTER"))
                    {
                        int i = line.IndexOf("\"");
                        int j = line.LastIndexOf("\"");
                        line = line.Substring(i + 1);
                        line = line.Remove(line.Length - 1);
                        NON_INTERSECTING_POINTS_PER_LETTER = line.Split(new char[] { ',' }).Select(x => x.Split('=')).ToDictionary(y => y[0], y => Convert.ToInt32(y[1]));
                        continue;
                    }
                    if (line.StartsWith("STYLE"))
                    {
                        int a = line.IndexOf("<");
                        string b = line.Substring(a);
                        int lastindex = b.LastIndexOf("<");
                        b = b.Substring(0, lastindex);
                        Style = b+"</style>";
                        continue;
                    }
                    string[] keyAndValue = line.Split(new char[] { '=', ',' });
                    if (keyAndValue.Length == 2)
                    {
                        switch (keyAndValue[0])
                        {
                            case "LOGFILE_NAME": { LOGFILE_NAME = keyAndValue[1]; break; }
                            case "RUNTIME_LIMIT": { RUNTIME_LIMIT = Convert.ToInt32(keyAndValue[1]); break; }
                            case "WORD_REGEX_PATTERN": { WORD_REGEX_PATTERN = keyAndValue[1]; break; }
                            case "MINIMUM_NUMBER_OF_UNIQUE_WORDS": { MINIMUM_NUMBER_OF_UNIQUE_WORDS = Convert.ToInt32(keyAndValue[1]); break; }
                            case "MAXIMUM_NUMBER_OF_UNIQUE_WORDS": { MAXIMUM_NUMBER_OF_UNIQUE_WORDS = Convert.ToInt32(keyAndValue[1]); break; }
                            case "INVALID_CROZZLE_SCORE": { INVALID_CROZZLE_SCORE = keyAndValue[1]; break; }
                            case "UPPERCASE": { UPPERCASE = Convert.ToBoolean(keyAndValue[1]); break; }
                            case "BGCOLOUR_EMPTY_TD": { BGCOLOUR_EMPTY_TD = keyAndValue[1]; break; }
                            case "BGCOLOUR_NON_EMPTY_TD": { BGCOLOUR_NON_EMPTY_TD = keyAndValue[1]; break; }
                            case "MINIMUM_NUMBER_OF_ROWS": { MINIMUM_NUMBER_OF_ROWS = Convert.ToInt32(keyAndValue[1]); break; }
                            case "MAXIMUM_NUMBER_OF_ROWS": { MAXIMUM_NUMBER_OF_ROWS = Convert.ToInt32(keyAndValue[1]); break; }
                            case "MINIMUM_NUMBER_OF_COLUMNS": { MINIMUM_NUMBER_OF_COLUMNS = Convert.ToInt32(keyAndValue[1]); break; }
                            case "MAXIMUM_NUMBER_OF_COLUMNS": { MAXIMUM_NUMBER_OF_COLUMNS = Convert.ToInt32(keyAndValue[1]); break; }
                            case "MINIMUM_HORIZONTAL_WORDS": { MINIMUM_HORIZONTAL_WORDS = Convert.ToInt32(keyAndValue[1]); break; }
                            case "MAXIMUM_HORIZONTAL_WORDS": { MAXIMUM_HORIZONTAL_WORDS = Convert.ToInt32(keyAndValue[1]); break; }
                            case "MINIMUM_VERTICAL_WORDS": { MINIMUM_VERTICAL_WORDS = Convert.ToInt32(keyAndValue[1]); break; }
                            case "MAXIMUM_VERTICAL_WORDS": { MAXIMUM_VERTICAL_WORDS = Convert.ToInt32(keyAndValue[1]); break; }
                            case "MINIMUM_INTERSECTIONS_IN_HORIZONTAL_WORDS": { MINIMUM_INTERSECTIONS_IN_HORIZONTAL_WORDS = Convert.ToInt32(keyAndValue[1]); break; }
                            case "MAXIMUM_INTERSECTIONS_IN_HORIZONTAL_WORDS": { MAXIMUM_INTERSECTIONS_IN_HORIZONTAL_WORDS = Convert.ToInt32(keyAndValue[1]); break; }
                            case "MINIMUM_INTERSECTIONS_IN_VERTICAL_WORDS": { MINIMUM_INTERSECTIONS_IN_VERTICAL_WORDS = Convert.ToInt32(keyAndValue[1]); break; }
                            case "MAXIMUM_INTERSECTIONS_IN_VERTICAL_WORDS": { MAXIMUM_INTERSECTIONS_IN_VERTICAL_WORDS = Convert.ToInt32(keyAndValue[1]); break; }
                            case "MINIMUM_NUMBER_OF_THE_SAME_WORD": { MINIMUM_NUMBER_OF_THE_SAME_WORD = Convert.ToInt32(keyAndValue[1]); break; }
                            case "MAXIMUM_NUMBER_OF_THE_SAME_WORD": { MAXIMUM_NUMBER_OF_THE_SAME_WORD = Convert.ToInt32(keyAndValue[1]); break; }
                            case "MINIMUM_NUMBER_OF_GROUPS": { MINIMUM_NUMBER_OF_GROUPS = Convert.ToInt32(keyAndValue[1]); break; }
                            case "MAXIMUM_NUMBER_OF_GROUPS": { MAXIMUM_NUMBER_OF_GROUPS = Convert.ToInt32(keyAndValue[1]); break; }
                            case "POINTS_PER_WORD": { POINTS_PER_WORD = Convert.ToInt32(keyAndValue[1]); break; }
                        }
                    }

                    
                }
                // Close streams.
                aStreamReader.Close();
            }
            catch (Exception)
            {
                
            }
            
        }
    }
}
