using System;
using System.IO;

namespace Alas_Meme
{
    
    public class FileSearch
    {
        public string  SearchForDefaultMd(string directory)
        {
            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                if (Path.GetFileName(file) == "Alas_Meme_N.md")
                {
                    Console.WriteLine("Found Alas_Meme_N.md in: " + file);
                    return file;
                }
            }

            string[] subdirectories = Directory.GetDirectories(directory);
            foreach (string subdirectory in subdirectories)
            {
                string path = SearchForDefaultMd(subdirectory);

                if (path != null)
                    return path;
            }
            return null;
        }
    }
}