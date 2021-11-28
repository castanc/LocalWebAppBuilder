using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace LocalWebBuilder
{
    public static class Utils
    {
        public static void ResetDir(this string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            else
            {
                var oldFiles = Directory.GetFiles(path, "*.*");
                foreach (var oldFile in oldFiles)
                {
                    try
                    {
                        File.Delete(oldFile);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

        }

        //http://csharpexamples.com/c-create-md5-hash-string/
        public static string CreateMD5Hash(this string input)
        {
            // Step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }


        public static List<string> extractAllBetween(this string text, string start,
            string end)
        {
            int startIndex = 0;
            string result = "XX";
            start = start.ToLower();
            end = end.ToLower();
            string text2 = text.ToLower();
            List<string> results = new List<string>();

            int index = 0;
            while (index >= 0)
            {
                index = text2.ToLower().IndexOf(start, startIndex);
                if (index >= startIndex)
                {
                    index += start.Length;
                    int index2 = text2.IndexOf(end, index);
                    if (index2 > index)
                    {
                        result = text.Substring(index, index2 - index);
                        results.Add(result);
                        startIndex = index2 + 1;
                    }
                }
            }

            return results;
        }
    }
}
