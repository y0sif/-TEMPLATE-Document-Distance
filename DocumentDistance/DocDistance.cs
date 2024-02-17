using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
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

            //config thread pool for better performance
            //values should change according to pc specs
            //will leave it as default
            /*
            int newMinWorkerThreads = 40;
            int newMinIocpThreads = 40;
            int newMaxWorkerThreads = 200;
            int newMaxIocpThreads = 200;

            ThreadPool.SetMinThreads(newMinWorkerThreads, newMinIocpThreads);
            ThreadPool.SetMaxThreads(newMaxWorkerThreads, newMaxIocpThreads);
            */

            //variable initialization
            string docString1;
            string docString2;
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

            //spliting the string into a dictionary in parallel
            Dictionary<string, int> doc1hashMap = new Dictionary<string, int>();
            Dictionary<string, int> doc2hashMap = new Dictionary<string, int>();

            Parallel.Invoke(
                () =>
                {
                    //had to use tempHashMap and move it to the main hashMap so it can be seen out side of parallel
                    Dictionary<string, int> tempHashMap = SplitStringToDictionary(docString1);
                    foreach (var tuple in tempHashMap)
                    {
                        doc1hashMap.Add(tuple.Key, tuple.Value);
                    }
                },
                () =>
                {
                    Dictionary<string, int> tempHashMap = SplitStringToDictionary(docString2);
                    foreach (var tuple in tempHashMap)
                    {
                        doc2hashMap.Add(tuple.Key, tuple.Value);
                    }
                }
            );

            //calculate d0, d1, d2 in parallel
            //had to use Math functions because regular operations are not accurate
            Parallel.Invoke(
                () =>
                {
                    //this way is faster than .Intersect()
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
                },
                () =>
                {
                    foreach (int i in doc1hashMap.Values)
                    {
                        d1 += Math.Pow(i, 2);
                    }
                },
                () =>
                {
                    foreach (int i in doc2hashMap.Values)
                    {
                        d2 += Math.Pow(i, 2);
                    }
                }
            );
            
            //calculate angle
            return Math.Acos(d0 / (Math.Sqrt(d1 * d2))) * (180 / Math.PI);
            
        }

        /// <summary>
        /// a function that does the splitting and forming the dictionary due to split function is not working right and not splitting when '?' occurs, and Regex.split being so slow. 
        /// </summary>
        /// <param name="document">document to be splitted</param>
        /// <returns>a dictionary of alphanumerical and the number of occurrences of each alphanumerical value</returns>
        private static Dictionary<string,int> SplitStringToDictionary(string docString)
        {
            Dictionary<string, int> doc = new Dictionary<string, int>();
            char[] characters = docString.ToCharArray();
            int start = 0;
            string s;

            //iterate over the document
            for (int i = 0; i < characters.Length; i++)
            {
                //if non alphanumerical 
                if (!char.IsLetterOrDigit(characters[i]))
                {
                    //found a separator add the alphanumerical to the document, or else it is only a separator so skip 
                    if (start < i)
                    {
                        s = new string(characters, start, i - start);
                        if (doc.ContainsKey(s))
                        {
                            doc[s] += 1;
                        }
                        else
                        {
                            doc.Add(s, 1);
                        }
                    }
                    start = i + 1;
                }
            }
            //add last alphanumerical in the document if exists
            if (start < characters.Length)
            {
                s = new string(characters, start, characters.Length - start);
                doc.Add(s, 1);
            }

            return doc;
        }
    }
}
