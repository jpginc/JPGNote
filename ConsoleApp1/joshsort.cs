using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    public class JoshSort
    {
        private struct JoshScore
        {
            public string str;
            public int score;

            public JoshScore(string str, int score)
            {
                this.str = str;
                this.score = score;
            }
        }
        public static IEnumerable<SortableRowWithValue> Sort(string sortBy, IEnumerable<SortableRowWithValue> todo)
        {
            return todo.OrderByDescending(js => GetJoshScore(js.SortByText, sortBy));
        }

        /*
         * who knows what this does. I've used it for years and it works well
         */
        public static int GetJoshScore(String value, String compareTo)
        {
            int score = 0;
            String[] words = compareTo.Split(" ");
            String valueLower = value.ToLower();
            foreach (string word in words)
            {
                int bonus = word.Length * 2;
                string mutableWord = word;
                while (mutableWord.Length != 0)
                {
                    string mutableWordLower = word.ToLower();
                    
                    if (value.Contains(mutableWord))
                    {
                        score += bonus;
                    }
                    if (value.StartsWith(mutableWord))
                    {
                        score += bonus;
                    }

                    if (valueLower.Contains(mutableWordLower))
                    {
                        score += bonus;
                    }

                    if (valueLower.StartsWith(mutableWordLower))
                    {
                        score += bonus;
                    }

                    if (value.Equals(mutableWord))
                    {
                        score *= 10;
                    }

                    if (valueLower.Equals(mutableWordLower))
                    {
                        score *= 5;
                    }

                    mutableWord = mutableWord.Substring(1);
                }
            }
            return score;
        }
    }
}