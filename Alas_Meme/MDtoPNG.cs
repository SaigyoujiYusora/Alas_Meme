using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MarkdownDeep;

namespace Alas_Meme
{
    internal class MDtoPng
    {
        [STAThread]
        public static void Main(string[] args)
        {
            //Read the markdown file
            string markdown;
            try
            {
                markdown = File.ReadAllText(args[0]);
            }
            catch (IndexOutOfRangeException)
            {
                // markdown = File.ReadAllText("../../../Alas_Meme_N_Debug.md");
                markdown = File.ReadAllText("../../../Alas_Meme_N.md");
            }

            //Initialize the MarkdownDeep engine
            Markdown md = new Markdown();
            
            //Convert the markdown to HTML
            string html = md.Transform(markdown);

            //Process image width
            html = Regex.Replace(html, @"(<img.*?)(/?>)", "$1 width=\"400\"$2");

            //Save the HTML to a file
            try
            {
                File.WriteAllText("../../../temp/output.html", html);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory("../../../temp");
                File.WriteAllText("../../../temp/output.html", html);
            }
            
            Console.WriteLine("Markdown converted to HTML successfully!");

            //Convert the HTML to image
            var browser = new WebBrowser();
            Console.WriteLine("Web Open Successfully!");
            browser.ScrollBarsEnabled = false;
            browser.AllowNavigation = false;
            browser.DocumentText = html;
            
            browser.DocumentCompleted += WebBrowser_DocumentCompleted;
            Application.Run();
        }

        static void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var browser = sender as WebBrowser;
            
            //scale browser to 200%
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
                browser.DrawToBitmap(bitmap, new Rectangle(0, 0, browser.Width, browser.Height));
                bitmap.Save("../../../temp/output.png", ImageFormat.Png);
                Console.WriteLine("HTML convert to image successfully!");
                
                //del output.html
                File.Delete("../../../temp/output.html");
                
                //get running path
                string runPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                
                //set runPath to ../../../temp/
                for (int i = 0; i <= 3; i++)
                {
                    runPath = runPath.Substring(0, runPath.LastIndexOf('\\'));
                }
                runPath += "\\temp\\";
                
                
                //open temp folder in explorer
                System.Diagnostics.Process.Start("explorer.exe", runPath);

                Application.Exit();
            }
        }
    }
}