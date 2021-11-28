using System;
using System.Collections.Generic;
using System.IO;
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
                    string[] minimizedJSFiles = Directory.GetFiles(Path.GetDirectoryName(f), "*.js1");
                    foreach (string minimizedJSFile in minimizedJSFiles)
                    {
                    minimizedJS += File.ReadAllText(minimizedJSFile);

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

                string path = Path.GetDirectoryName(f);
                string pathOut = $"{path}\\{Path.GetFileNameWithoutExtension(f)}_Deploy\\1_ToMinify";
                string pathToObfuscate = $"{path}\\{Path.GetFileNameWithoutExtension(f)}_Deploy\\3_ToObfuscate";


                pathOut.ResetDir();
                pathToObfuscate.ResetDir();


                foreach (string cssFile in cssFiles)
                {

                    fName = $"{path}\\{cssFile}";
                    if (!File.Exists(fName))
                    {
                        fName = fName.ToLower().Replace(".min", "");
                        fName = fName.Replace(Path.GetExtension(fName), $".min{Path.GetExtension(fName)}");
                    }

                    if ( File.Exists(fName))
                    {
                        string text = File.ReadAllText(fName);
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
    }

}

