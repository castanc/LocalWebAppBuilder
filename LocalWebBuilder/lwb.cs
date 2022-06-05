﻿using System;
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
        public static string pathMinified = "";
        public static string pathOut = "";
        public static string pathToObfuscate = "";
        public static string pathFullHtml = "";
        public static string pathToMinify = "";
        public static string mainFileName = "";
        public static string pathOut2 = "";
        public static string version = "6.0";


        public static string addScripts = @"
<script id='xzY1'></script>
<script id='xzY2'></script>
<script id='log'>
</script>
<script id='f5'>
function removeElement(divId) {
	let div = document.getElementById(divId);
	if (div)
		div.remove();
}

    var xzY1 = '';
    if (xzY1.length > 0) {
        let sc = document.getElementById('xzY1');
        console.log('sc', sc);
        if (sc) {
            let vr = MD5(sc.innerHTML);
            if (vr.toUpperCase() != xzY1) {

	            let div = document.getElementById('mainContent');
	            if (div)
		            div.remove();

            }
        }
    }
    removeElement('f5');
    removeElement('log');

</script>";


        public static int AddMinified(this string[] inputFiles)
        {
            if (inputFiles.Length < 1)
                return -1;

            foreach (var f in inputFiles)
            {
                pathMinified = $"{path}\\{Path.GetFileNameWithoutExtension(f)}_Deploy\\2_Minified";
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

                html = html.RemoveAllBetween("<script src=", ">");
                html = html.RemoveAllBetween("<link rel =", ">");


                html = html.Replace("</head>", $"<style>{sbCss}</style></head>");
                html = html.Replace("var xzY1 = '';", $"var xzY1 = '{md5}';");

                if (html.Contains("<script id='xzY1'></script>"))
                    html = html.Replace("<script id='xzY1'></script>", $@"<script id='xzY1'>{minifiedJS}</script>");
                else if (html.Contains("<script id=xzY1></script>"))
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

                File.WriteAllText($"{pathOut}\\{Path.GetFileNameWithoutExtension(f)}.html", html);

                string newName = $"{pathOut}\\{Path.GetFileNameWithoutExtension(f)}.md5";
                File.WriteAllText(newName, md5);
                FinalFolder = pathOut;


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
                foreach (string cssFile in cssFiles)
                {
                    string text = File.ReadAllText(cssFile);
                    sbCss.AppendLine(text);
                }

                if (obfuscatedText.Length == 0)
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



        //deprectated not used any more,. with t he new split methods
        public static int GenerateLocalApp(this string[] inputFiles,
     string excludedFiles)
        {
            int result = 0;
            string ffName = "";
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

                //html = html ;

                cssFiles = html.extractAllBetween("\"stylesheet\" href=\"", "\">");
                if (cssFiles.Count == 0)
                    cssFiles = html.extractAllBetween("stylesheet href=", ">");

                jsFiles = html.extractAllBetween("<script src=\"", "\">");
                if (jsFiles.Count == 0)
                    jsFiles = html.extractAllBetween("<script src=", ">");

                path = Path.GetDirectoryName(f);
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
                        File.WriteAllText($"{pathOut}\\{ffName}.js", text);
                    }
                }

                foreach (string ex in exJsFiles)
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


        //deprectated
        public static int GenerateLocalApp2(this string[] inputFiles,
string excludedFiles, bool minifyJS = true, bool obfuscateJS = true)
        {
            int result = 0;
            string ffName = "";
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


                html = $"{html}\n{addScripts}";

                cssFiles = html.extractAllBetween("\"stylesheet\" href=\"", "\">");
                if (cssFiles.Count == 0)
                    cssFiles = html.extractAllBetween("stylesheet href=", ">");

                jsFiles = html.extractAllBetween("<script src=\"", "\">");
                if (jsFiles.Count == 0)
                    jsFiles = html.extractAllBetween("<script src=", ">");

                path = Path.GetDirectoryName(f);
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
                        File.WriteAllText($"{pathOut}\\{ffName}.js", text);
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


        //aqui es
        public static async Task<int> GenerateLocalAppAutoMinify(this string[] inputFiles,
string excludedFiles, bool minifyJS = true, bool obfuscateJS = true)
        {
            int result = 0;
            string ffName = "";
            excludedFiles = excludedFiles.ToLower();
            string html = "";
            string htmlMin = "";
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

                html = File.ReadAllText(f);
                int index = html.ToLower().IndexOf("</html>");
                if (index >= 0)
                    html = html.Substring(0, index + 7);


                cssFiles = html.extractAllBetween("\"stylesheet\" href=\"", "\">");
                if (cssFiles.Count == 0)
                    cssFiles = html.extractAllBetween("stylesheet href=", ">");

                jsFiles = html.extractAllBetween("<script src=\"", "\">");
                if (jsFiles.Count == 0)
                    jsFiles = html.extractAllBetween("<script src=", ">");


                //path = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(f)));
                path = Path.GetPathRoot(f);
                pathToMinify = $"{path}\\_Deploy\\1_ToMinify";
                pathToMinify.ResetDir();

                File.WriteAllText($"{pathToMinify}\\{Path.GetFileNameWithoutExtension(f)}.html", html);

                string html2 = html;
                //string html2 =  await "https://www.toptal.com/developers/html-minifier/raw".Minify(html);
                if (html2.Length > 0)
                    html = html2 + addScripts;


                pathOut = $"{path}\\_Deploy\\2_Minified";
                pathMinified = $"{path}\\_Deploy\\2_Minified";
                pathToObfuscate = $"{path}\\_Deploy\\3_ToObfuscate";


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

                pathFullHtml = $"{path}\\FullHtml";
                pathFullHtml.ResetDir();

                string cssMinified = sbCSS.ToString();
                if (sbCSS.ToString().Length > 0)
                {
                    cssMinified = await "https://www.toptal.com/developers/cssminifier/raw".Minify(sbCSS.ToString());
                    if (cssMinified.Length == 0)
                        cssMinified = sbCSS.ToString();

                    File.WriteAllText($"{pathMinified}\\{Path.GetFileNameWithoutExtension(f)}.min.cs1", cssMinified);
                    //File.WriteAllText($"{pathOut}\\{Path.GetFileNameWithoutExtension(f)}.css", sbCSS.ToString());
                    File.WriteAllText($"{pathToMinify}\\{Path.GetFileNameWithoutExtension(f)}.css", sbCSS.ToString());
                }

                //string alljsFileName = $"{Path.GetDirectoryName(f)}\\{Path.GetFileNameWithoutExtension(f)}.js";
                //File.WriteAllText(alljsFileName, "");

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
                        File.WriteAllText($"{pathOut}\\{ffName}.js", text);
                        File.WriteAllText($"{pathFullHtml}\\{ffName}.html", $"<script id='{ffName}'>{text}</script>\n");
                        File.WriteAllText($"{pathToMinify}\\{ffName}.js", text);
                        //File.AppendAllText(alljsFileName, text);
                    }
                }

                string nojsName = $"{pathFullHtml}\\{Path.GetFileNameWithoutExtension(f)}.nojs.html";
                File.WriteAllText(nojsName, html);

                foreach (string exJSFile in exJsFiles)
                {
                    html = html + $@"<script>
{exJSFile}
</script>
";
                }

                FileName = $"{pathOut}\\{Path.GetFileNameWithoutExtension(f)}.html";

                //string fName2 = $"{Path.GetDirectoryName(f)}\\${Path.GetFileNameWithoutExtension(f)}_Final.html";
                File.WriteAllText(FileName, html);
                File.WriteAllText($"{pathToObfuscate}\\{Path.GetFileNameWithoutExtension(f)}.html", html);
                //File.WriteAllText(fName2, html);


                string nPath = $"{pathOut}\\JoinedJS";
                nPath.ResetDir();
                File.WriteAllText($"{nPath}\\{Path.GetFileNameWithoutExtension(f)}.js", sbJS.ToString());


                string jsMinified = await "https://www.toptal.com/developers/javascript-minifier/raw".Minify(sbJS.ToString());
                if (jsMinified.Length == 0)
                    jsMinified = sbJS.ToString();

                File.WriteAllText($"{nPath}\\{Path.GetFileNameWithoutExtension(f)}.min.js1", jsMinified);
                File.WriteAllText($"{pathFullHtml}\\{Path.GetFileNameWithoutExtension(f)}.min.html", $"<script id='allJsMin'>{jsMinified}</script>");


                nPath = $"{pathToObfuscate}\\JoinedJSFiles";
                nPath.ResetDir();
                File.WriteAllText($"{pathMinified}\\{Path.GetFileNameWithoutExtension(f)}.min.js1", jsMinified);

                File.WriteAllText($"{nPath}\\{Path.GetFileNameWithoutExtension(f)}.txt", sbJoined.ToString());

                File.WriteAllText($"{nPath}\\{Path.GetFileNameWithoutExtension(f)}.js", sbJS.ToString());

                //aqui es
                string[] files = FileName.Split("*");
                files.AddMinified3();

            }

            return result;
        }

        //aqui es nuevo
        public static async Task<int> GenerateLocalAppAutoMinifySplit(this string[] inputFiles,
string excludedFiles, bool minifyJS = true, bool obfuscateJS = true)
        {
            if (inputFiles.Length < 1)
                return -1;

            int result = 0;
            excludedFiles = excludedFiles.ToLower();
            string html = "";
            string htmlMin = "";
            string ffName = "";

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
                StringBuilder sbCSSMinified = new StringBuilder();
                StringBuilder sbJSMinified = new StringBuilder();
                List<string> finalCSS = new List<string>();
                List<string> finalJS = new List<string>();
                List<string> localCSS = new List<string>();
                List<string> localJS = new List<string>();


                mainFileName = "_" + Path.GetFileNameWithoutExtension(f);
                path = $"{Path.GetPathRoot(f)}\\_Deploy{mainFileName}";
                path.ResetDir();

                string pathOut2NoMin = $"{path}\\0_Out2_NoMin";
                pathOut2NoMin.ResetDir();

                string pathOut2Min = $"{path}\\0_Out2_Min";
                pathOut2Min.ResetDir();


                pathToMinify = $"{path}\\1_Original";
                pathToMinify.ResetDir();

                pathOut = $"{path}\\4_Out";
                pathOut.ResetDir();

                pathMinified = $"{path}\\2_Minified";
                pathMinified.ResetDir();

                pathToObfuscate = $"{path}\\3_ToObfuscate";
                pathToObfuscate.ResetDir();

                pathFullHtml = $"{path}\\5_FullHtml";
                pathFullHtml.ResetDir();

                html = File.ReadAllText(f);
                int index = html.ToLower().IndexOf("</html>");
                if (index >= 0)
                    html = html.Substring(0, index + 7);


                cssFiles = html.extractAllBetween("\"stylesheet\" href=\"", "\">");
                if (cssFiles.Count == 0)
                    cssFiles = html.extractAllBetween("stylesheet href=", ">");

                jsFiles = html.extractAllBetween("<script src=\"", "\">");
                if (jsFiles.Count == 0)
                    jsFiles = html.extractAllBetween("<script src=", ">");

                localCSS = html.extractAllBetweenInclusive("<link rel=\"stylesheet\" href=\"", ".css\">");
                localJS = html.extractAllBetweenInclusive("<script src=\"", "\"></script>");

                File.WriteAllText($"{pathToMinify}\\{mainFileName}.html", html);

                string html2 = html;
                //string html2 =  await "https://www.toptal.com/developers/html-minifier/raw".Minify(html);
                if (html2.Length > 0)
                    html = html2 + addScripts;


                string cssMinified = "";
                sbCSS = new StringBuilder();
                bool ok = false;
                pathMinified = $"{path}\\2_Minified\\css";
                pathMinified.ResetDir();
                pathFullHtml = $"{path}\\5_FullHtml\\css";
                pathFullHtml.ResetDir();

                string text = "";
                string fNameOut = "";
                foreach (string cssFile in cssFiles)
                {
                    ok = false;
                    fName = $"{Path.GetDirectoryName(f)}\\{cssFile}";
                    if (File.Exists(fName))
                    {
                        text = File.ReadAllText(fName);
                        sbCSS.AppendLine(text);
                        File.WriteAllText($"{pathToMinify}\\{ffName}.css", text);
                        //text = text.RemoveAllBetween("//", "\n");
                        //text = text.removeSpaces();
                        if (!fName.ToLower().EndsWith(".min.css") &&
                            !text.Contains("//nominify"))
                        {
                            cssMinified = await "https://www.toptal.com/developers/cssminifier/raw".Minify(text);
                            ok = cssMinified.Length > 0;
                        }
                        ffName = Path.GetFileNameWithoutExtension(cssFile);
                        if (ok)
                        {
                            sbCSSMinified.AppendLine(cssMinified);
                            File.WriteAllText($"{pathMinified}\\{ffName}.min.css", cssMinified);
                            File.WriteAllText($"{pathFullHtml}\\{ffName}.min.html",
                                $"<style>{cssMinified}</style>\n");
                        }
                        else
                        {
                            sbCSSMinified.AppendLine(text);
                            File.WriteAllText($"{pathMinified}\\{ffName}.nomin.css", text);
                            File.WriteAllText($"{pathFullHtml}\\{ffName}.nomin.html",
                                $"<style>{text}</style>\n");
                        }

                    }
                }

                File.WriteAllText($"{pathMinified}\\{mainFileName}.full.min.css", $"<style>${sbCSSMinified}</style>");
                File.WriteAllText($"{pathOut}\\{mainFileName}.full.min.css", $"<style>${sbCSSMinified}</style>");
                File.WriteAllText($"{pathFullHtml}\\{mainFileName}.full.min.css", $"<style>${sbCSSMinified}</style>");
                File.WriteAllText($"{pathToMinify}\\{mainFileName}.full.css", $"<style>${sbCSS}</style>");



                sbJS = new StringBuilder();
                sbJSMinified = new StringBuilder();
                sbJoined = new StringBuilder();
                string nPath = $"{pathOut}\\JoinedJS";
                nPath.ResetDir();
                string jsMinified = "";
                pathMinified = $"{path}\\2_Minified\\js";
                pathMinified.ResetDir();
                pathFullHtml = $"{path}\\5_FullHtml\\js";
                pathFullHtml.ResetDir();


                foreach (string jsFile in jsFiles)
                {
                    ok = false;
                    fName = $"{Path.GetDirectoryName(f)}\\{jsFile}";
                    if (File.Exists(fName))
                    {
                        ffName = Path.GetFileNameWithoutExtension(jsFile);
                        text = File.ReadAllText(fName);
                        File.WriteAllText($"{pathToMinify}\\{ffName}.js", text);
                        //text = text.RemoveAllBetween("//", System.Environment.NewLine);
                        //text = text.removeSpaces();
                        sbJS.AppendLine(text);

                        if (!fName.ToLower().EndsWith("min.js") &&
                            !text.Contains("//nominify"))
                        {
                            jsMinified = await "https://www.toptal.com/developers/javascript-minifier/raw".Minify(text);
                            ok = jsMinified.Length > 0;
                        }
                        ffName = Path.GetFileNameWithoutExtension(jsFile);
                        if (ok)
                        {
                            sbJSMinified.AppendLine(jsMinified);
                            File.WriteAllText($"{pathFullHtml}\\{ffName}.min.js.html", $"<script id='{ffName}'>{jsMinified}</script>\r\n");
                            File.WriteAllText($"{pathMinified}\\{ffName}.min.js", jsMinified);
                        }
                        else
                        {
                            sbJSMinified.AppendLine(text);
                            File.WriteAllText($"{pathFullHtml}\\{ffName}.nomin.js.html", $"<script id='{ffName}'>{text}</script>\r\n");
                            File.WriteAllText($"{pathMinified}\\{ffName}.nomin.js", text);
                        }
                    }
                }


                File.WriteAllText($"{pathOut}\\{mainFileName}.MD5Check.js.html", addScripts);
                addScripts = addScripts.removeSpaces();
                File.WriteAllText($"{pathMinified}\\{mainFileName}.MD5Check.js.html", addScripts);
                File.WriteAllText($"{pathFullHtml}\\{mainFileName}.MD5Check.js.html", addScripts);


                File.WriteAllText($"{pathOut}\\{mainFileName}.full.js", sbJS.ToString());
                File.WriteAllText($"{pathFullHtml}\\{mainFileName}.full.js.html", $"<script id='{mainFileName}'>{sbJS}</script>");
                File.WriteAllText($"{pathMinified}\\{mainFileName}.full.min.js", sbJSMinified.ToString());

                //remove links to local css an jscript
                foreach (string s in localCSS)
                    html = html.Replace(s, "");

                foreach (string s in localJS)
                    html = html.Replace(s, "");


                string nojsName = $"{pathFullHtml}\\{mainFileName}.nojs.html";
                File.WriteAllText(nojsName, html);


                File.WriteAllText($"{pathToObfuscate}\\{mainFileName}.html", html);
                File.WriteAllText($"{pathFullHtml}\\{mainFileName}.html", html);

                File.WriteAllText($"{nPath}\\{mainFileName}.js", sbJS.ToString());

                html = html.Replace("</head>", $"<script id='allJS'>{sbJSMinified}</script>{System.Environment.NewLine}<style>{sbCSSMinified}</style>\r\n</head>");


                lwb.FileName = $"{pathFullHtml}\\{mainFileName}.final.full.html";
                File.WriteAllText(lwb.FileName, html);
                File.WriteAllText($"{nPath}\\{mainFileName}.final.full.html", html);
                File.WriteAllText($"{pathToObfuscate}\\{mainFileName}final.full.html", html);
                File.WriteAllText($"{pathOut}\\{mainFileName}.final.full.html", html);

                lwb.FinalHTML = html;
                lwb.FileName = $"{nPath}\\{mainFileName}.final.full.html";


            }

            return result;
        }


        //aqui es new simplified
        public static async Task<int> GenerateLocalAppNew(this string[] inputFiles,string noMinify = "",
 bool minifyJS = true, bool obfuscateJS = true)
        {
            if (inputFiles.Length < 1)
                return -1;

            int result = 0;
            string html = "";
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
                StringBuilder sbCSSMinified = new StringBuilder();
                StringBuilder sbJSMinified = new StringBuilder();
                List<string> finalCSS = new List<string>();
                List<string> finalJS = new List<string>();
                List<string> localCSS = new List<string>();
                List<string> localJS = new List<string>();


                mainFileName = Path.GetFileNameWithoutExtension(f);
                path = $"{Path.GetPathRoot(f)}\\_Deploy_{mainFileName}";
                path.ResetDir();

                string pathOut = $"{path}\\0_Output";
                pathOut.ResetDir();

                html = await File.ReadAllTextAsync(f);
                int index = html.ToLower().IndexOf("</html>");
                if (index >= 0)
                    html = html.Substring(0, index + 7);


                cssFiles = html.extractAllBetween("\"stylesheet\" href=\"", "\">");
                if (cssFiles.Count == 0)
                    cssFiles = html.extractAllBetween("stylesheet href=", ">");

                jsFiles = html.extractAllBetween("<script src=\"", "\">");
                if (jsFiles.Count == 0)
                    jsFiles = html.extractAllBetween("<script src=", ">");

                localCSS = html.extractAllBetweenInclusive("<link rel=\"stylesheet\" href=\"", ".css\">");
                localJS = html.extractAllBetweenInclusive("<script src=\"", "\"></script>");

                await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.orig.html", html);

                html += addScripts;
                sbCSS = new StringBuilder();

                string text = "";
                foreach (string cssFile in cssFiles)
                {
                    fName = $"{Path.GetDirectoryName(f)}\\{cssFile}";
                    if (File.Exists(fName))
                    {
                        text = await File.ReadAllTextAsync(fName);
                        sbCSS.AppendLine(text);
                    }
                }
                string cssMinified = await "https://www.toptal.com/developers/cssminifier/raw".Minify(sbCSS.ToString());

                await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.css.html", $"<style>{sbCSS}</style>");
                await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.css", $"{sbCSS}");
                if ( cssMinified.Length > 0 )
                {
                    await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.min.css.html", $"<style>{cssMinified}</style>");
                    await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.min.css", cssMinified);

                }

                sbJS = new StringBuilder();
                Dictionary<string, IsolatedItem> isolated = new Dictionary<string, IsolatedItem>();
                StringBuilder sbJSNoMin = new StringBuilder();
                StringBuilder sbFilesNoMin = new StringBuilder();
                foreach (string jsFile in jsFiles)
                {
                    fName = $"{Path.GetDirectoryName(f)}\\{jsFile}";
                    if (File.Exists(fName))
                    {
                        text = await File.ReadAllTextAsync(fName);
                        sbJSNoMin.AppendLine($"//JS START {jsFile}\r\n{text}\r\n//JS END {jsFile}\r\n");
                        sbFilesNoMin.AppendLine(fName);

                        //if (fName.Contains("/isolated/"))
                        //{
                        //    IsolatedItem item = new IsolatedItem() { Id = Path.GetFileNameWithoutExtension(jsFile), Raw = text, Minified = "" };
                        //    isolated.Add(item.Id, item);
                        //}
                        //else if (text.Contains("`") || fName.Contains("/lib/"))
                        //{
                        //    sbJSNoMin.AppendLine(text);
                        //    sbFilesNoMin.AppendLine(fName);
                        //}
                        //else
                        //    sbJS.AppendLine(text);
                    }
                }
                string jsMinified = ""; // await "https://www.toptal.com/developers/javascript-minifier/raw".Minify($"{sbJS}");
                string jsMinIsolated = "";

                foreach(var kvp in isolated)
                {
                    //kvp.Value.Minified = await "https://www.toptal.com/developers/javascript-minifier/raw".Minify($"{kvp.Value.Raw}");
                    await File.WriteAllTextAsync($"{pathOut}\\{kvp.Key}.js.html", $"<script id='{kvp.Key}'>{kvp.Value.Raw}</script>");
                    await File.WriteAllTextAsync($"{pathOut}\\{kvp.Key}.js", kvp.Value.Raw);
                    sbJSNoMin.AppendLine(kvp.Value.Raw);
                    if (kvp.Value.Minified.Length > 0)
                    {
                        string finalIsolated = $"<script id='{kvp.Key}'>{kvp.Value.Minified}\r\n</script>\r\n";
                        jsMinIsolated += finalIsolated;
                        await File.WriteAllTextAsync($"{pathOut}\\{kvp.Key}.min.js.html", finalIsolated);
                        await File.WriteAllTextAsync($"{pathOut}\\{kvp.Key}.min.js", kvp.Value.Minified);
                    }
                }

                int sbjslen = sbJS.ToString().Length;
                if (sbjslen > 0)
                {
                    await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.js.html", $"<script id='jsraw'>{sbJS}</script>");
                    await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.js", $"{sbJS}");
                }
                if ( sbJSNoMin.ToString().Length > 0 )
                {
                    await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.jsnomin.html", $"<script id='nomin'>{sbJSNoMin}</script>");
                    await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.jsnomin.js", $"{sbJSNoMin}");

                }
                if (jsMinified.Length > 0)
                {
                    await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.min.js.html", $"<script id='jsmin'>{jsMinified}</script>\r\n<script id='nomin'>{sbJSNoMin}\r\n</script>\r\n");
                    await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.min.js", jsMinified);

                }



                if (html.Contains("<!--LOCAL FILES START-->") &&
                    html.Contains("<!--LOCAL FILES END-->"))
                    html = html.removeAllBetween("<!--LOCAL FILES START-->", "<!--LOCAL FILES END-->");
                else
                {
                    //remove links to local css an jscript
                    foreach (string s in localCSS)
                        html = html.Replace(s, "");

                    foreach (string s in localJS)
                        html = html.Replace(s, "");
                }

                mainFileName = $"_Final_{mainFileName}";
                await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.nojs.html", html);

                string html2 = html;
                if (sbjslen > 0)
                {
                    html = html.Replace("</head>", $"<style>{cssMinified}</style>\r\n<script id='jsraw'>{sbJS}\r\n</script>\r\n<script id='nomin'>{sbJSNoMin}\r\n</script>\r\n</head>");
                    await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.nomin.html", html);
                }
                else
                {
                    html = html.Replace("</head>", $"<style>{cssMinified}</style>\r\n<script id='nomin'>{sbJSNoMin}\r\n</script>\r\n</head>\r\n");
                    await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.nomin.html", html);
                }

                if (jsMinified.Length > 0)
                {
                    html = html2.Replace("</head>", $"<style>{cssMinified}</style>\r\n<script id='jsmin'>{jsMinified}\r\n</script>\r\n<script id='nomin'>{sbJSNoMin}\r\n</script>\r\n{jsMinIsolated}</head>\r\n");
                    await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.min.html", html);
                }

                if (sbjslen > 0)
                    html = html2.Replace("</head>", $"<style>{cssMinified}</style>\r\n<script id='{mainFileName}'>\r\n{sbJS}\r\n</script>\r\n<script id='nomin'>{sbJSNoMin}\r\n</script>\r\n</head>\r\n");
                else
                    html = html2.Replace("</head>", $"<style>{cssMinified}</style>\r\n<script id='nomin'>{sbJSNoMin}\r\n</script>\r\n</head>\r\n");

                await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.jsnomin.html", html);

                await File.WriteAllTextAsync($"{pathOut}\\NoMinimized.txt", sbFilesNoMin.ToString());

                lwb.FileName = $"{pathOut}\\{mainFileName}.nomin.html";
                lwb.FinalHTML = html;
            }

            return result;
        }



        //aqui es new simplest no minify
        public static async Task<int> GenerateLocalAppNoMin(this string[] inputFiles)
        {
            if (inputFiles.Length < 1)
                return -1;

            int result = 0;
            string html = "";
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
                StringBuilder sbCSSMinified = new StringBuilder();
                StringBuilder sbJSMinified = new StringBuilder();
                List<string> finalCSS = new List<string>();
                List<string> finalJS = new List<string>();
                List<string> localCSS = new List<string>();
                List<string> localJS = new List<string>();


                mainFileName = Path.GetFileNameWithoutExtension(f);
                path = $"{Path.GetPathRoot(f)}\\_Deploy_{mainFileName}";
                path.ResetDir();

                string pathOut = $"{path}\\0_Output";
                pathOut.ResetDir();

                html = await File.ReadAllTextAsync(f);
                int index = html.ToLower().IndexOf("</html>");
                if (index >= 0)
                    html = html.Substring(0, index + 7);


                cssFiles = html.extractAllBetween("\"stylesheet\" href=\"", "\">");
                if (cssFiles.Count == 0)
                    cssFiles = html.extractAllBetween("stylesheet href=", ">");

                jsFiles = html.extractAllBetween("<script src=\"", "\">");
                if (jsFiles.Count == 0)
                    jsFiles = html.extractAllBetween("<script src=", ">");

                localCSS = html.extractAllBetweenInclusive("<link rel=\"stylesheet\" href=\"", ".css\">");
                localJS = html.extractAllBetweenInclusive("<script src=\"", "\"></script>");

                await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.orig.html", html);

                html += addScripts;
                sbCSS = new StringBuilder();

                string text = "";
                foreach (string cssFile in cssFiles)
                {
                    fName = $"{Path.GetDirectoryName(f)}\\{cssFile}";
                    if (File.Exists(fName))
                    {
                        text = await File.ReadAllTextAsync(fName);
                        sbCSS.AppendLine(text);
                    }
                }
                string cssMinified = await "https://www.toptal.com/developers/cssminifier/raw".Minify(sbCSS.ToString());

                await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.css.html", $"<style>{sbCSS}</style>");
                await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.css", $"{sbCSS}");
                if (cssMinified.Length > 0)
                {
                    await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.min.css.html", $"<style>{cssMinified}</style>");
                    await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.min.css", cssMinified);

                }

                StringBuilder sbJSNoMin = new StringBuilder();
                StringBuilder sbFilesNoMin = new StringBuilder();
                foreach (string jsFile in jsFiles)
                {
                    fName = $"{Path.GetDirectoryName(f)}\\{jsFile}";
                    if (File.Exists(fName))
                    {
                        text = await File.ReadAllTextAsync(fName);
                        sbJSNoMin.AppendLine($"//JS START {jsFile}\r\n{text}\r\n//JS END {jsFile}\r\n");
                        sbFilesNoMin.AppendLine(fName);
                    }
                }
                if (sbJSNoMin.ToString().Length > 0)
                {
                    await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.jsnomin.html", $"<script id='nomin'>{sbJSNoMin}</script>");
                    await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.jsnomin.js", $"{sbJSNoMin}");

                }

                if (html.Contains("<!--LOCAL FILES START-->") &&
                    html.Contains("<!--LOCAL FILES END-->"))
                    html = html.removeAllBetween("<!--LOCAL FILES START-->", "<!--LOCAL FILES END-->");
                else
                {
                    //remove links to local css an jscript
                    foreach (string s in localCSS)
                        html = html.Replace(s, "");

                    foreach (string s in localJS)
                        html = html.Replace(s, "");
                }

                mainFileName = $"_Final_{mainFileName}";
                await File.WriteAllTextAsync($"{pathOut}\\{mainFileName}.nojs.html", html);
                lwb.FileName = $"{pathOut}\\{mainFileName}.nomin.html";
                html = html.Replace("</head>", 
                    $"<style>{cssMinified}</style>\r\n<script id='nomin'>{sbJSNoMin}\r\n</script>\r\n</head>\r\n");
                await File.WriteAllTextAsync(lwb.FileName, html);
                lwb.FinalHTML = html;
            }

            return result;
        }



    }

}
