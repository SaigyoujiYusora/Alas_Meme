using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using MarkdownDeep;
using static Alas_Meme.ImageConverter;

namespace Alas_Meme
{
    internal class MDtoPng
    {
        [STAThread]
        public static void Main(string[] args)
        {
            //Read the markdown file
            string markdown;
            bool runInBuildPath;
            if (File.Exists("System.Buffers.xml"))
            {
                runInBuildPath = true;
                Console.WriteLine("Detected Debug mode ON");
            }
            else
            {
                runInBuildPath = false;
            }

            if (runInBuildPath)
            {
                string imgPath = Assembly.GetExecutingAssembly().Location;
                for (int i = 0; i <= 3; i++)
                {
                    imgPath = imgPath.Substring(0, imgPath.LastIndexOf('\\'));
                }
                imgPath += "\\Lme\\";
            
            
                ConvertImages(imgPath);
            }
            try
            {
                markdown = File.ReadAllText(args[0]);
            }
            catch (IndexOutOfRangeException)
            {
                //Initialize variables
                // markdown = File.ReadAllText("../../../Alas_Meme_N.md");
                // markdown = File.ReadAllText("../../../Alas_Meme_N_Debug.md");
                if (File.Exists("System.Buffers.xml"))
                {
                    markdown = File.ReadAllText("../../../Alas_Meme_N_Fun.md");
                }
                else
                {
                    FileSearch fileSearch = new FileSearch();
                    markdown = File.ReadAllText(fileSearch.SearchForDefaultMd(Directory.GetCurrentDirectory()));
                }

            }

            //Initialize the MarkdownDeep engine
            Markdown md = new Markdown();
            
            //Convert the markdown to HTML
            string html = md.Transform(markdown);

            //Process image width
            html = Regex.Replace(html, @"(<img.*?)(/?>)", "$1 width=\"400\"$2");

            //Save the HTML to a file
            if (runInBuildPath)
            {
                try
                {
                    File.WriteAllText("../../../temp/output.html", html);
                }
                catch (DirectoryNotFoundException)
                {
                    Directory.CreateDirectory("../../../temp");
                    File.WriteAllText("../../../temp/output.html", html);
                }
            }
            else
            {
                try
                {
                    File.WriteAllText("./temp/output.html", html);
                }
                catch (DirectoryNotFoundException)
                {
                    Directory.CreateDirectory("./temp");
                    File.WriteAllText("./temp/output.html", html);
                }
            }
            
            Console.WriteLine("Markdown converted to HTML successfully!");

            //Convert the HTML to image
            var browser = new WebBrowser();
            Console.WriteLine("Web Open Successfully!");
            browser.ScrollBarsEnabled = false;
            browser.AllowNavigation = false;
            browser.DocumentText = html;
            
            
            browser.DocumentCompleted += WebBrowser_DocumentCompleted;
            // browser.DocumentCompleted += (sender, e) => 
            // {
            //     
            // };
            Application.Run();
        }

        static void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var browser = sender as WebBrowser;
            bool inBuildPath;

            if (File.Exists("System.Buffers.xml"))
            {
                inBuildPath = true;
            }
            else
            {
                inBuildPath = false;
            }
            
            // scale browser to 200%
            browser.Document.Body.Style = "zoom: 300%;";
            browser.Width = browser.Document.Body.ScrollRectangle.Width;
            browser.Width *= 3;
            browser.Height = browser.Document.Body.ScrollRectangle.Height;
            browser.Height *= 3;
            if (browser.Width < 920)
            {
                browser.Width = 920;
            }
            
            using (Bitmap bitmap = new Bitmap(browser.Width, browser.Height))
            {
                //sleep 10seconds
                Console.WriteLine("Sleep 2s");
                Thread.Sleep(2000);
                
                
                browser.DrawToBitmap(bitmap, new Rectangle(0, 0, browser.Width, browser.Height));
                if (inBuildPath)
                {
                    bitmap.Save("../../../temp/output.png", ImageFormat.Png);
                }
                else
                {
                    bitmap.Save("./temp/output.png", ImageFormat.Png);
                }
                Console.WriteLine("HTML convert to image successfully!");
                
                //del output.html
                if (inBuildPath)
                {
                    File.Delete("../../../temp/output.html");
                }else
                {
                    File.Delete("./temp/output.html");
                }
                
                //get running path
                string runPath = Assembly.GetExecutingAssembly().Location;
                
                //set runPath to ../../../temp/
                if (inBuildPath)
                {
                    for (int i = 0; i <= 3; i++)
                    {
                        runPath = runPath.Substring(0, runPath.LastIndexOf('\\'));
                    }
                }
                else
                {
                    runPath = Directory.GetCurrentDirectory();
                }
                runPath += "\\temp\\";

                //open temp folder in explorer
                Process.Start("explorer.exe", runPath);

                Application.Exit();
            }
        }
    }
}