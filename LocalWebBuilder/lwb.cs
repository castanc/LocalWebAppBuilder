using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LocalWebBuilder
{
    public static class lwb
    {
        public static string path = @"C:\MyWorks\_LocalWebAppsDeploy";
        public static Exception Ex;
        public static string Message = "";
        public static string FileName = "";
        public static string FinalFolder = "";
            public static int Result = 0;
        public static string FinalHTML = "";

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


        public static int AddMinified(this string[] inputFiles )
        {
            if (inputFiles.Length < 1)
                return -1;

            foreach (var f in inputFiles)
            {
                string pathMinified = $"{path}\\{Path.GetFileNameWithoutExtension(f)}_Deploy\\2_Minified";
                pathMinified.ResetDir();


                string[] cssFiles = Directory.GetFiles(Path.GetDirectoryName(f), "*.css");

                StringBuilder sbCss = new StringBuilder();
                foreach (string cssFile in cssFiles)
                {
                    string text = File.ReadAllText(cssFile);
                    sbCss.AppendLine(text);
                }

                string minifiedJS = "";
                    string[] minifiedJSFiles = Directory.GetFiles(Path.GetDirectoryName(f), "*.js1");
                    foreach (string minifiedJSFile in minifiedJSFiles)
                    {
                    try
                    {
                        minifiedJS += File.ReadAllText(minifiedJSFile);
                    }
                    catch(Exception ex)
                    {
                        Ex = ex;
                        Result = -1;
                        Message = "Error reading joined JS Files.";
                        return Result;
                    }

                    }

                string md5 = minifiedJS.CreateMD5Hash();
                string html = File.ReadAllText(f);

                html = html.RemoveAllBetween("<script src=", ">");
                html = html.RemoveAllBetween("<link rel =", ">");
                

                 html = html.Replace("</head>", $"<style>{sbCss}</style></head>");
                html = html.Replace("var xzY1 = '';", $"var xzY1 = '{md5}';");

                if (html.Contains("<script id='xzY1'></script>"))
                    html = html.Replace("<script id='xzY1'></script>", $@"<script id='xzY1'>{minifiedJS}</script>");
                else if ( html.Contains("<script id=xzY1></script>"))
                    html = html.Replace("<script id=xzY1></script>", $@"<script id='xzY1'>{minifiedJS}</script>");
    
                string newName = $"{pathMinified}\\{Path.GetFileNameWithoutExtension(f)}.html";

                File.WriteAllText(newName, html);

                newName = $"{pathMinified}\\{Path.GetFileNameWithoutExtension(f)}.md5";
                File.WriteAllText(newName, md5);

            }
            return Result;
        }

        public static int AddMinified3(this string[] inputFiles)
        {
            if (inputFiles.Length < 1)
                return -1;

            foreach (var f in inputFiles)
            {
                string pathMinified = $"{path}\\{Path.GetFileNameWithoutExtension(f).Replace(".min","")}_Deploy\\2_Minified";
                string outPath = $"{path}\\{Path.GetFileNameWithoutExtension(f).Replace(".min", "")}_Deploy\\3_Output";

                outPath.ResetDir();


                string[] cssFiles = Directory.GetFiles(pathMinified, "*.min.cs1");

                StringBuilder sbCss = new StringBuilder();
                foreach (string cssFile in cssFiles)
                {
                    string text = File.ReadAllText(cssFile);
                    sbCss.AppendLine(text);
                }

                string minifiedJS = "";
                string[] minifiedJSFiles = Directory.GetFiles(Path.GetDirectoryName(f), "*.js1");
                foreach (string minifiedJSFile in minifiedJSFiles)
                {
                    try
                    {
                        minifiedJS += File.ReadAllText(minifiedJSFile);
                    }
                    catch (Exception ex)
                    {
                        Ex = ex;
                        Result = -1;
                        Message = "Error reading joined JS Files.";
                        return Result;
                    }

                }

                string md5 = minifiedJS.CreateMD5Hash();
                string html = File.ReadAllText(f);

                html = html.RemoveAllBetween("<link rel=", ">");
                html = html.RemoveAllBetween("<script src=", "</script>");

                html = html.Replace("</head>", $"<style>{sbCss}</style></head>");
                html = html.Replace("var xzY1 = '';", $"var xzY1 = '{md5}';");

                if (html.Contains("<script id='xzY1'></script>"))
                    html = html.Replace("<script id='xzY1'></script>", $@"<script id='xzY1'>{minifiedJS}</script>");
                else if (html.Contains("<script id=xzY1></script>"))
                    html = html.Replace("<script id=xzY1></script>", $@"<script id='xzY1'>{minifiedJS}</script>");
                FinalHTML = html;

                FileName = $"{pathMinified}\\{Path.GetFileNameWithoutExtension(f)}.html";

                File.WriteAllText($"{pathMinified}\\{Path.GetFileNameWithoutExtension(f)}.html", html);
                File.WriteAllText($"{outPath}\\{Path.GetFileNameWithoutExtension(f)}.html", html);

                string newName = $"{outPath}\\{Path.GetFileNameWithoutExtension(f)}.md5";
                File.WriteAllText(newName, md5);
                FinalFolder = outPath;


            }
            return Result;
        }


        public static int AddObfuscated(this string[] inputFiles, string obfuscatedText = "", string ext = "*.js1")
        {
            int result = 0;
            if (inputFiles.Length < 1)
                return -1;

            foreach (var f in inputFiles)
            {
                string pathObfuscate = $"{path}\\{Path.GetFileNameWithoutExtension(f)}_Deploy\\3_ToObfuscate";
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
                int index = html.ToLower().IndexOf("</html");
                if (index >= 0)
                    html = html.Substring(0, index + 6);

                html = html + addScripts;

                cssFiles = html.extractAllBetween("\"stylesheet\" href=\"", "\">");
                if ( cssFiles.Count == 0 )
                    cssFiles = html.extractAllBetween("stylesheet href=", ">");

                jsFiles = html.extractAllBetween("<script src=\"", "\">");
                if ( jsFiles.Count == 0 )
                    jsFiles = html.extractAllBetween("<script src=", ">");

                string pathOut = $"{path}\\{Path.GetFileNameWithoutExtension(f)}_Deploy\\1_ToMinify";
                string pathToObfuscate = $"{path}\\{Path.GetFileNameWithoutExtension(f)}_Deploy\\3_ToObfuscate";


                pathOut.ResetDir();
                pathToObfuscate.ResetDir();


                foreach (string cssFile in cssFiles)
                {

                    fName = $"{Path.GetDirectoryName(f)}\\{cssFile}";
                    if ( File.Exists(fName))
                    {
                        string text = File.ReadAllText(fName);
                        sbCSS.AppendLine(text);
                        File.WriteAllText($"{pathOut}\\{Path.GetFileNameWithoutExtension(cssFile)}.css",text);
                    }
                }

                foreach (string jsFile in jsFiles)
                {

                    fName = $"{Path.GetDirectoryName(f)}\\{jsFile}";
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

                foreach(string ex in exJsFiles)
                {
                    html = html + $@"<script>
{ex}
</script>
";
                }

                fName = $"{pathOut}\\{Path.GetFileNameWithoutExtension(f)}.html";
                File.WriteAllText(fName, html);
                fName = $"{pathToObfuscate}\\{Path.GetFileNameWithoutExtension(f)}.html";
                File.WriteAllText(fName, html);

                fName = $"{pathOut}\\JoinedJS_{Path.GetFileNameWithoutExtension(f)}.js1";
                File.WriteAllText(fName, sbJS.ToString());
                fName = $"{pathToObfuscate}\\JoinedJS_{Path.GetFileNameWithoutExtension(f)}.js1";
                File.WriteAllText(fName, sbJS.ToString());



                fName = $"{pathOut}\\JoinedJSFiles_{Path.GetFileNameWithoutExtension(f)}.txt";
                File.WriteAllText(fName, sbJoined.ToString());

            }

            return result;
        }


        public static int GenerateLocalApp2(this string[] inputFiles,
string excludedFiles, bool minifyJS = true, bool obfuscateJS = true)
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
                int index = html.ToLower().IndexOf("</html>");
                if (index >= 0)
                    html = html.Substring(0, index + 7);

                html = html + addScripts;

                cssFiles = html.extractAllBetween("\"stylesheet\" href=\"", "\">");
                if (cssFiles.Count == 0)
                    cssFiles = html.extractAllBetween("stylesheet href=", ">");

                jsFiles = html.extractAllBetween("<script src=\"", "\">");
                if (jsFiles.Count == 0)
                    jsFiles = html.extractAllBetween("<script src=", ">");

                string pathOut = $"{path}\\{Path.GetFileNameWithoutExtension(f)}_Deploy\\1_ToMinify";
                string pathToObfuscate = $"{path}\\{Path.GetFileNameWithoutExtension(f)}_Deploy\\3_ToObfuscate";


                pathOut.ResetDir();
                pathToObfuscate.ResetDir();


                foreach (string cssFile in cssFiles)
                {

                    fName = $"{Path.GetDirectoryName(f)}\\{cssFile}";
                    if (File.Exists(fName))
                    {
                        string text = File.ReadAllText(fName);
                        sbCSS.AppendLine(text);
                        File.WriteAllText($"{pathOut}\\{Path.GetFileNameWithoutExtension(cssFile)}.css", text);
                    }
                }

                foreach (string jsFile in jsFiles)
                {

                    fName = $"{Path.GetDirectoryName(f)}\\{jsFile}";
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

                foreach (string ex in exJsFiles)
                {
                    html = html + $@"<script>
{ex}
</script>
";
                }

                File.WriteAllText($"{pathOut}\\{Path.GetFileNameWithoutExtension(f)}.html", html);
                File.WriteAllText($"{pathToObfuscate}\\{Path.GetFileNameWithoutExtension(f)}.html", html);

                File.WriteAllText($"{pathOut}\\JoinedCSS_{Path.GetFileNameWithoutExtension(f)}.cs1", sbCSS.ToString());
                File.WriteAllText($"{pathToObfuscate}\\JoinedCSS_{Path.GetFileNameWithoutExtension(f)}.cs1", sbCSS.ToString());

                File.WriteAllText($"{pathOut}\\JoinedJS_{Path.GetFileNameWithoutExtension(f)}.js1", sbJS.ToString());
                File.WriteAllText($"{pathToObfuscate}\\JoinedJS_{Path.GetFileNameWithoutExtension(f)}.js1", sbJS.ToString());

                File.WriteAllText($"{pathOut}\\JoinedJSFiles_{Path.GetFileNameWithoutExtension(f)}.txt", sbJoined.ToString());

            }

            return result;
        }


        public static async Task<int> GenerateLocalAppAutoMinify(this string[] inputFiles,
string excludedFiles, bool minifyJS = true, bool obfuscateJS = true)
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
                int index = html.ToLower().IndexOf("</html>");
                if (index >= 0)
                    html = html.Substring(0, index + 7);


                cssFiles = html.extractAllBetween("\"stylesheet\" href=\"", "\">");
                if (cssFiles.Count == 0)
                    cssFiles = html.extractAllBetween("stylesheet href=", ">");

                jsFiles = html.extractAllBetween("<script src=\"", "\">");
                if (jsFiles.Count == 0)
                    jsFiles = html.extractAllBetween("<script src=", ">");

                string pathToMinify = $"{path}\\{Path.GetFileNameWithoutExtension(f)}_Deploy\\1_ToMinify";
                pathToMinify.ResetDir();

                File.WriteAllText($"{pathToMinify}\\{Path.GetFileNameWithoutExtension(f)}.html",html);

                string html2 = html;    // await "https://www.toptal.com/developers/html-minifier/raw".Minify(html);
                if (html2.Length > 0)
                    html = html2 + addScripts;


                string pathOut = $"{path}\\{Path.GetFileNameWithoutExtension(f)}_Deploy\\2_Minified";
                string pathToObfuscate = $"{path}\\{Path.GetFileNameWithoutExtension(f)}_Deploy\\3_ToObfuscate";


                pathOut.ResetDir();
                pathToObfuscate.ResetDir();


                foreach (string cssFile in cssFiles)
                {
                    fName = $"{Path.GetDirectoryName(f)}\\{cssFile}";
                    if (File.Exists(fName))
                    {
                        string text = File.ReadAllText(fName);
                        sbCSS.AppendLine(text);
                        File.WriteAllText($"{pathOut}\\{Path.GetFileNameWithoutExtension(cssFile)}.css", text);
                        File.WriteAllText($"{pathToMinify}\\{Path.GetFileNameWithoutExtension(cssFile)}.css", text);
                    }
                }

                string cssMinified = sbCSS.ToString();
                if ( sbCSS.ToString().Length>0)
                {
                    cssMinified = await "https://www.toptal.com/developers/cssminifier/raw".Minify(sbCSS.ToString());
                    if (cssMinified.Length == 0)
                        cssMinified = sbCSS.ToString();

                    File.WriteAllText($"{pathOut}\\{Path.GetFileNameWithoutExtension(f)}.min.cs1", cssMinified);
                    //File.WriteAllText($"{pathOut}\\{Path.GetFileNameWithoutExtension(f)}.css", sbCSS.ToString());
                    File.WriteAllText($"{pathToMinify}\\{Path.GetFileNameWithoutExtension(f)}.css", sbCSS.ToString());
                }

                foreach (string jsFile in jsFiles)
                {

                    fName = $"{Path.GetDirectoryName(f)}\\{jsFile}";
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
                        File.WriteAllText($"{pathToMinify}\\{Path.GetFileNameWithoutExtension(jsFile)}.js", text);
                    }
                }

                foreach (string exJSFile in exJsFiles)
                {
                    html = html + $@"<script>
{exJSFile}
</script>
";
                }

                FileName = $"{pathOut}\\{Path.GetFileNameWithoutExtension(f)}.html";
                File.WriteAllText(FileName, html);
                File.WriteAllText($"{pathToObfuscate}\\{Path.GetFileNameWithoutExtension(f)}.html", html);

                File.WriteAllText($"{pathOut}\\JoinedJS_{Path.GetFileNameWithoutExtension(f)}.js", sbJS.ToString());
                File.WriteAllText($"{pathToObfuscate}\\JoinedJS_{Path.GetFileNameWithoutExtension(f)}.js", sbJS.ToString());

                string jsMinified = await "https://www.toptal.com/developers/javascript-minifier/raw".Minify(sbJS.ToString());
                if (jsMinified.Length == 0)
                    jsMinified = sbJS.ToString();

                File.WriteAllText($"{pathOut}\\JoinedJS_{Path.GetFileNameWithoutExtension(f)}.min.js1", jsMinified);
                File.WriteAllText($"{pathToObfuscate}\\JoinedJS_{Path.GetFileNameWithoutExtension(f)}.min.js1", jsMinified);

                File.WriteAllText($"{pathOut}\\JoinedJSFiles_{Path.GetFileNameWithoutExtension(f)}.txt", sbJoined.ToString());

                string[] files = FileName.Split("*");
                files.AddMinified3();

            }

            return result;
        }

    }

}
