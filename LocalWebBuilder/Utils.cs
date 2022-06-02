using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Net.Http;

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


        public static List<string> extractAllBetweenInclusive(this string text, string start,
    string end, string exclude = "https://")
        {
            int startIndex = 0;
            string result = "XX";
            start = start.ToLower();
            end = end.ToLower();
            string text2 = text.ToLower();
            List<string> results = new List<string>();

            int index = 0;
            int origIndex = 0;
            while (index >= 0)
            {
                index = text2.IndexOf(start, startIndex);
                if (index >= startIndex)
                {
                    origIndex = index;
                    index += start.Length;
                    int index2 = text2.IndexOf(end, index);
                    if (index2 > index)
                    {
                        result = text.Substring(origIndex, index2 - origIndex + end.Length);
                        if ( !result.ToLower().Contains(exclude))
                            results.Add(result);

                        startIndex = index2 + 1;
                    }
                }
            }

            return results;
        }


        public static string RemoveAllBetween(this string text, string start,
            string end, string exception = ":/")
        {
            int startIndex = 0;
            start = start.ToLower();
            end = end.ToLower();
            string text2 = text.ToLower();

            int index = text2.IndexOf(start);
            List<string> results = new List<string>();
            string result = "";
            while (index >= 0)
            {
                if (index >= startIndex)
                {
                    int index2 = text2.IndexOf(end, index);
                    if (index2 > index)
                    {
                        result = text.Substring(index, index2 - index+end.Length);

                        if (!result.ToLower().Contains(exception))
                        {
                            if (text.Contains(result))
                            {
                                text = text.Replace(result, "");
                                results.Add(result);
                                text2 = text.ToLower();
                                index = text2.ToLower().IndexOf(start, startIndex);
                                //text = text.Substring(0, index) + text.Substring(index2 + end.Length);\
                            }
                        }
                        else
                        {
                            startIndex = index2 + end.Length;
                            index = text2.ToLower().IndexOf(start, startIndex);
                        }
                    }
                    
                }
            }

            return text;
        }

        public static async Task<List<string>> FileToBase64(this string[] files)
        {
            List<string> result = new List<string>();
            if (files.Length < 0)
                return result;

            foreach(string file in files)
            {
                byte[] imageArray = await System.IO.File.ReadAllBytesAsync(file);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                string fName = $"{Path.GetDirectoryName(file)}\\{Path.GetFileNameWithoutExtension(file)}.base64.txt";
                await File.WriteAllTextAsync(fName, base64ImageRepresentation);
                result.Add(fName);
            }
            return result;
        }

        public static async Task<String> Minify(this string url, string inputText)
        {
            const string URL_CSS_MINIFIER =  "https://www.toptal.com/developers/cssminifier/raw";
            const string POST_PAREMETER_NAME = "input";

        List<KeyValuePair<String, String>> contentData = new List<KeyValuePair<String, String>>
        {
            new KeyValuePair<String, String>(POST_PAREMETER_NAME, inputText)
        };

            using (HttpClient httpClient = new HttpClient())
            {
                using (FormUrlEncodedContent content = new FormUrlEncodedContent(contentData))
                {
                    try
                    {
                        using (HttpResponseMessage response = await httpClient.PostAsync(url, content))
                        {
                            response.EnsureSuccessStatusCode();
                            return await response.Content.ReadAsStringAsync();
                        }
                    }
                    catch(Exception ex)
                    {
                        return "";
                    }
                }
            }
        }
    }

    }

    



