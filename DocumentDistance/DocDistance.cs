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

            //spliting the string into a dictionary
            Dictionary<string, int> doc1hashMap = SplitString(docString1);
            Dictionary<string, int> doc2hashMap = SplitString(docString2);

            //calculate d0, d1, d2
            //had to use Math functions because regular operations are not accurate
            foreach (int i in doc1hashMap.Values)
            {
                d1 += Math.Pow(i, 2);
            }
            foreach (int i in doc2hashMap.Values)
            {
                d2 += Math.Pow(i,2);
            }

            //checking this way is faster than using .Intersect() 
            if (doc1hashMap.Count > doc2hashMap.Count)
            {
                foreach (string s in doc2hashMap.Keys)
                {
                    if (doc1hashMap.ContainsKey(s))
                    {
                        d0 += Math.BigMul(doc1hashMap[s], doc2hashMap[s]);

                    }
                }
            }
            else
            {
                foreach (string s in doc1hashMap.Keys)
                {
                    if (doc2hashMap.ContainsKey(s))
                    {
                        d0 += Math.BigMul(doc1hashMap[s], doc2hashMap[s]);

                    }
                }
            }

            //calculate angle
            distance = Math.Acos(d0 / (Math.Sqrt(d1 * d2)));
            distance = distance * (180 / Math.PI);
            return distance;
        }

        /// <summary>
        /// a function that does the splitting due to split function is not working right and does not split when '?' occurs, and Regex.split is slow. 
        /// </summary>
        /// <param name="document">document to be splitted</param>
        /// <returns>dictionary of alphanumerical and number of occurrences from the document</returns>
        private static Dictionary<string,int> SplitString(string document)
        {
            Dictionary<string, int> alphanumerical = new Dictionary<string, int>();
            char[] characters = document.ToCharArray();
            int start = 0;
            string s;

            //iterate over the document
            for (int i = 0; i < characters.Length; i++)
            {
                //if non alphanumerical 
                if (!char.IsLetterOrDigit(characters[i]))
                {
                    //add to alphanumerical, or else it is just a separator so skip 
                    if (start < i)
                    {
                        s = new string(characters, start, i - start);
                        if (alphanumerical.ContainsKey(s))
                        {
                            alphanumerical[s]++;
                        }
                        else
                        {
                            alphanumerical.Add(s, 1);
                        }
                    }
                    start = i + 1;
                }
            }
            //add last alphanumerical in the document if exists
            if (start < characters.Length)
            {
                s = new string(characters, start, characters.Length - start);
                alphanumerical.Add(s, 1);
            }

            return alphanumerical;
        }
    }
}
