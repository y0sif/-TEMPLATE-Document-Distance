using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DocumentDistance
{
    class DocDistance
    {
        // *****************************************
        // DON'T CHANGE CLASS OR FUNCTION NAME
        // YOU CAN ADD FUNCTIONS IF YOU NEED TO
        // *****************************************
        /// <summary>
        /// Write an efficient algorithm to calculate the distance between two documents
        /// </summary>
        /// <param name="doc1FilePath">File path of 1st document</param>
        /// <param name="doc2FilePath">File path of 2nd document</param>
        /// <returns>The angle (in degree) between the 2 documents</returns>
        public static double CalculateDistance(string doc1FilePath, string doc2FilePath)
        {
            // TODO comment the following line THEN fill your code here
            // throw new NotImplementedException();

            //variable initialization
            string docString1;
            string docString2;
            Dictionary<string, int> doc1hashMap = new Dictionary<string, int>();
            Dictionary<string, int> doc2hashMap = new Dictionary<string, int>();
            double distance;
            double d0 = 0;
            double d1 = 0;
            double d2 = 0;

            //reading file into a string
            using (StreamReader streamReader = new StreamReader(doc1FilePath, Encoding.UTF8))
            {
                docString1 = streamReader.ReadToEnd().ToLower();
            }
            using (StreamReader streamReader = new StreamReader(doc2FilePath, Encoding.UTF8))
            {
                docString2 = streamReader.ReadToEnd().ToLower();
            }

            //spliting the string to an array
            List<string> doc1 = ExtractWords(docString1);
            List<string> doc2 = ExtractWords(docString2);

            // Compare sets of words
            //HashSet<string> set1 = new HashSet<string>(doc1);
            //HashSet<string> set2 = new HashSet<string>(doc2);

            // Check for differences
            //var onlyInSet1 = set1.Except(set2);
            //var onlyInSet2 = set2.Except(set1);

            //Console.WriteLine("Words only in doc1:");
            //Console.WriteLine(string.Join(", ", onlyInSet1));

            //Console.WriteLine("Words only in doc2:");
            //Console.WriteLine(string.Join(", ", onlyInSet2));

            //use regex
            //string regex = @"[^a-zA-Z0-9]+";
            //string[] doc1 = Regex.Split(docString1, regex, RegexOptions.None).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            //string[] doc2 = Regex.Split(docString2, regex, RegexOptions.None).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

            //has to change this to a faster one
            //List<string> doc1 = doc1arr.Select(x => x.ToLower()).ToList();
            //List<string> doc2 = doc2arr.Select(x => x.ToLower()).ToList();

            //check accurance of the string
            foreach (string s in doc1)
            {
                if (doc1hashMap.ContainsKey(s)){
                    doc1hashMap[s] += 1;
                }
                //add it in hashmap if it is a new string
                else
                {
                    doc1hashMap.Add(s, 1);
                }
            }
            
            foreach (string s in doc2)
            {
                if (doc2hashMap.ContainsKey(s))
                {
                    doc2hashMap[s] += 1;
                }
                else
                {
                    doc2hashMap.Add(s, 1);
                }
            }
            /*
            foreach (string s in doc1hashMap.Keys)
            {
                Console.WriteLine($"{s}");
            }
            Console.WriteLine("second");
            foreach (string s in doc2hashMap.Keys)
            {
                Console.WriteLine($"{s}");
            }
            */
            //calculate d0, d1, d2
            foreach (int i in doc1hashMap.Values)
            {
                d1 += Math.Pow(i, 2);
            }
            foreach (int i in doc2hashMap.Values)
            {
                d2 += Math.Pow(i,2);
            }
            foreach (string s in doc1hashMap.Keys.Intersect(doc2hashMap.Keys))
            {
                d0 += Math.BigMul(doc1hashMap[s], doc2hashMap[s]) ;
            }

            //Console.WriteLine(d0);
            //Console.WriteLine(d1);
            //Console.WriteLine(d2);
            distance = Math.Acos(d0 / (Math.Sqrt(d1 * d2)));
            //Console.WriteLine(distance);
            distance = distance * (180 / Math.PI);
            //Console.WriteLine(distance);
            return distance;
        }

        private static List<string> ExtractWords(string document)
        {
            List<string> words = new List<string>();
            char[] characters = document.ToCharArray();
            int start = 0;

            for (int i = 0; i < characters.Length; i++)
            {
                if (!char.IsLetterOrDigit(characters[i]))
                {
                    if (start < i)
                    {
                        words.Add(new string(characters, start, i - start));
                    }
                    start = i + 1;
                }
            }

            if (start < characters.Length)
            {
                words.Add(new string(characters, start, characters.Length - start));
            }

            return words;
        }
    }
}
