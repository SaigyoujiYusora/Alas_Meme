using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using MarkdownDeep;
using System.Windows.Forms;

namespace Alas_Meme
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            //Read the markdown file
            // string markdown = File.ReadAllText(args[0]);
            string markdown = File.ReadAllText("../../../Alas_Meme_N.md");

            //Initialize the MarkdownDeep engine
            Markdown md = new Markdown();

            //Convert the markdown to HTML
            string html = md.Transform(markdown);

            //Save the HTML to a file
            File.WriteAllText("../../../temp/output.html", html);
            Console.WriteLine("Markdown converted to HTML successfully!");

            //Convert the HTML to image
            var browser = new WebBrowser();
            Console.WriteLine("Web Open Successfully!");
            browser.ScrollBarsEnabled = false;
            browser.AllowNavigation = false;
            browser.DocumentText = html;
            browser.Width = 800;
            browser.Height = 2000;
            browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
            Application.Run();
        }

        static void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var browser = sender as WebBrowser;
            using (Bitmap bitmap = new Bitmap(browser.Width, browser.Height))
            {
                browser.DrawToBitmap(bitmap, new Rectangle(0, 0, browser.Width, browser.Height));
                bitmap.Save("../../../temp/output.png", ImageFormat.Png);
                Console.WriteLine("HTML convert to image successfully!");
                Application.Exit();
            }
        }
    }
}