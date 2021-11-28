using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LocalWebBuilder
{
    public static class lwb
    {
        public static string addScripts = @"
<script id='xzY1'></script>
<script id='xzY2'></script>
<script id='log'>
    console.log('SCRIPTS', document.scripts.length);
    for (var i in document.scripts) {
        try {
            console.log(i, document.scripts[i].id, document.scripts[i].text.length);
        }
        catch (ex) {
            console.log('EXCEPTION', ex);
        }
    }
    console.log('all', document.scripts);
</script>
<script id='f5'>
    var xzY1 = '';
    if (xzY1.length > 0) {
        let sc = document.getElementById('xzY1');
        console.log('sc', sc);
        if (sc) {
            let vr = MD5(sc.innerHTML);
            if (vr.toUpperCase() != xzY1) {

                removeElement('mainContent');
            }
        }
    }
    removeElement('f5');
    removeElement('log');

</script>";


        public static int AddMinimized(this string[] inputFiles )
        {
            int result = 0;
            if (inputFiles.Length < 1)
                return -1;

            foreach (var f in inputFiles)
            {
                string path = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(f)));
                string pathMinimized = $"{path}\\{Path.GetFileNameWithoutExtension(f)}_Deploy\\2_Minimized";
                pathMinimized.ResetDir();


                string[] cssFiles = Directory.GetFiles(Path.GetDirectoryName(f), "*.css");

                StringBuilder sbCss = new StringBuilder();
                foreach (string cssFile in cssFiles)
                {
                    string text = File.ReadAllText(cssFile);
                    sbCss.AppendLine(text);
                }

                string minimizedJS = "";
                    string[] obfFiles = Directory.GetFiles(Path.GetDirectoryName(f), "*.js1");
                    foreach (string obfFile in obfFiles)
                    {
                    minimizedJS += File.ReadAllText(obfFile);

                    }

                string md5 = minimizedJS.CreateMD5Hash();
                string html = File.ReadAllText(f);

                html = html.Replace("</head>", $"<style>{sbCss}</style></head>");
                html = html.Replace("var xzY1 = '';", $"var xzY1 = '{md5}';");

                if (html.Contains("<script id='xzY1'></script>"))
                    html = html.Replace("<script id='xzY1'></script>", $@"<script id='xzY1'>{minimizedJS}</script>");
                else if ( html.Contains("<script id=xzY1></script>"))
                    html = html.Replace("<script id=xzY1></script>", $@"<script id='xzY1'>{minimizedJS}</script>");
    
                string newName = $"{pathMinimized}\\{Path.GetFileNameWithoutExtension(f)}.html";

                File.WriteAllText(newName, html);

                newName = $"{pathMinimized}\\{Path.GetFileNameWithoutExtension(f)}.md5";
                File.WriteAllText(newName, md5);

            }
            return result;
        }


        public static int AddObfuscated(this string[] inputFiles, string obfuscatedText = "", string ext = "*.js1")
        {
            int result = 0;
            if (inputFiles.Length < 1)
                return -1;

            foreach (var f in inputFiles)
            {
                string path = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(f)));
                string pathObfuscate = $"{path}\\{Path.GetFileNameWithoutExtension(f)}_Deploy\\3_Obfuscate";
                pathObfuscate.ResetDir();


                string[] cssFiles = Directory.GetFiles(Path.GetDirectoryName(f), "*.css");

                StringBuilder sbCss = new StringBuilder();
                foreach(string cssFile in cssFiles)
                {
                    string text = File.ReadAllText(cssFile);
                    sbCss.AppendLine(text);
                }

                if (obfuscatedText.Length == 0 )
                {
                    string[] obfFiles = Directory.GetFiles(path, "*.js1");
                    foreach (string obfFile in obfFiles)
                    {
                        obfuscatedText += File.ReadAllText(obfFile);

                    }
                }

                string md5 = obfuscatedText.CreateMD5Hash();
                string html = File.ReadAllText(f);

                html = html.Replace("</head>", $"<style>{sbCss}</style></head>");
                html = html.Replace("var xzY1 = '';", $"var xzY1 = '{md5}';");
                //html = html + $@"<script id='f5'>var zxyZ1 = '{md5}';</script>";
                html = html.Replace("<script id='xzY1'></script>", $@"<script id='xzY1'>{obfuscatedText}</script>");
                string newName = $"{pathObfuscate}\\{Path.GetFileNameWithoutExtension(f)}_Obfuscated.html";

                File.WriteAllText(newName, html);

                newName = $"{pathObfuscate}\\{Path.GetFileNameWithoutExtension(f)}.md5";
                File.WriteAllText(newName, md5);

            }
            return result;
        }

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
            while(index >=0 )
            {
                index = text2.ToLower().IndexOf(start,startIndex);
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

        public static int GenerateLocalApp(this string[] inputFiles,
     string excludedFiles)
        {
            int result = 0;
            excludedFiles = excludedFiles.ToLower();
            if (inputFiles.Length < 1)
                return -1;

            string fName = "";

            foreach (string f in inputFiles)
            {
                List<string> cssFiles = new List<string>();
                List<string> jsFiles = new List<string>();
                List<string> exCssFiles = new List<string>();
                List<string> exJsFiles = new List<string>();
                StringBuilder sbCSS = new StringBuilder();
                StringBuilder sbJS = new StringBuilder();
                List<string> excluded = new List<string>();
                StringBuilder sbJoined = new StringBuilder();

                string html = File.ReadAllText(f);
                html = html + addScripts;

                cssFiles = html.extractAllBetween("\"stylesheet\" href=\"", "\">");
                if ( cssFiles.Count == 0 )
                    cssFiles = html.extractAllBetween("stylesheet href=", ">");

                jsFiles = html.extractAllBetween("<script src=\"", "\">");
                if ( jsFiles.Count == 0 )
                    jsFiles = html.extractAllBetween("<script src=", ">");

                string path = Path.GetDirectoryName(Path.GetDirectoryName(f));
                string pathOut = $"{path}\\{Path.GetFileNameWithoutExtension(f)}_Deploy\\1_ToMinify";

                pathOut.ResetDir();


                foreach (string cssFile in cssFiles)
                {

                    fName = $"{path}\\{cssFile}";
                    if ( !File.Exists(fName))
                        fName = fName.Replace(Path.GetExtension(fName), $".min{Path.GetExtension(fName)}");

                    if ( File.Exists(fName))
                    {
                        string text = File.ReadAllText(fName);
                        if (excludedFiles.Contains(cssFile.ToLower()))
                            exCssFiles.Add(text);
                        else
                            sbCSS.AppendLine(text);

                        File.WriteAllText($"{pathOut}\\{Path.GetFileNameWithoutExtension(cssFile)}.css",text);
                    }
                }

                foreach (string jsFile in jsFiles)
                {

                    fName = $"{path}\\{jsFile}";
                    if (File.Exists(fName))
                    {
                        string text = File.ReadAllText(fName);
                        if (excludedFiles.Contains(jsFile.ToLower()))
                            exJsFiles.Add(text);
                        else
                        {
                            sbJoined.AppendLine(jsFile);
                            sbJS.AppendLine(text);
                        }
                        File.WriteAllText($"{pathOut}\\{Path.GetFileNameWithoutExtension(jsFile)}.js", text);
                    }
                }

//                html = html.Replace("</head>", $@"<style>
//{sbCSS}
//</style>
//</head>
//");
                foreach(string ex in exJsFiles)
                {
                    html = html + $@"<script>
{ex}
</script>
";

                }

//                html = html.Replace("</head>", $@"<script>
//{sbJS}
//</script>
//</head>
//");



                fName = $"{pathOut}\\{Path.GetFileNameWithoutExtension(f)}.html";
                File.WriteAllText(fName, html);

                fName = $"{pathOut}\\JoinedJS_{Path.GetFileNameWithoutExtension(f)}.js1";
                File.WriteAllText(fName, sbJS.ToString());

                fName = $"{pathOut}\\JoinedJSFiles_{Path.GetFileNameWithoutExtension(f)}.txt";
                File.WriteAllText(fName, sbJoined.ToString());

            }

            return result;
        }
    }

}

