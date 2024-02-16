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

            //calculate angle
            distance = Math.Acos(d0 / (Math.Sqrt(d1 * d2)));
            distance = distance * (180 / Math.PI);
            return distance;
        }

        /// <summary>
        /// split function is not working right and does not split when '?' occurs, and Regex.split is slow, so write a function that does the splitting 
        /// </summary>
        /// <param name="document">document to be splitted</param>
        /// <returns>list of alphanumerical strings from the document</returns>
        private static List<string> ExtractWords(string document)
        {
            List<string> alphanumerical = new List<string>();
            char[] characters = document.ToCharArray();
            int start = 0;

            //iterate over the document
            for (int i = 0; i < characters.Length; i++)
            {
                //if non alphanumerical 
                if (!char.IsLetterOrDigit(characters[i]))
                {
                    //add to alphanumerical, or else it is just a separator so skip 
                    if (start < i)
                    {
                        alphanumerical.Add(new string(characters, start, i - start));
                    }
                    start = i + 1;
                }
            }
            //add last alphanumerical in the document if exists
            if (start < characters.Length)
            {
                alphanumerical.Add(new string(characters, start, characters.Length - start));
            }

            return alphanumerical;
        }
    }
}
