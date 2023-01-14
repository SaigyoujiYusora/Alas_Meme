using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using MarkdownDeep;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Alas_Meme
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            //Read the markdown file
            string markdown = null;
            try
            {
                markdown = File.ReadAllText(args[0]);
            }
            catch (System.IndexOutOfRangeException)
            {
                markdown = File.ReadAllText("../../../Alas_Meme_N_Debug.md");
                // markdown = File.ReadAllText("../../../Alas_Meme_N.md");
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
            catch (System.IO.DirectoryNotFoundException)
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
            // browser.Width = 800;
            // browser.Height = 2000;
            
            browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
            Application.Run();
        }

        static void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var browser = sender as WebBrowser;
            
            //scale browser to 200%
            browser.Document.Body.Style = "zoom: 300%;";
            browser.Width = browser.Document.Body.ScrollRectangle.Width;
            browser.Width = browser.Width * 3;
            browser.Height = browser.Document.Body.ScrollRectangle.Height;
            browser.Height = browser.Height * 3;
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
                
                Application.Exit();
            }
        }
    }
}