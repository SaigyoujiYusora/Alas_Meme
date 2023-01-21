using System;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

namespace Alas_Meme
{
    class ImageConverter
    {
        public static void ConvertImages(string path)
        {
            Console.WriteLine("Staring image conversion");
            // 获取该文件夹下所有图片文件
            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                            || s.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
                            || s.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase)
                            || s.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)
                            || s.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                            || s.EndsWith(".ico", StringComparison.OrdinalIgnoreCase));

            // 遍历文件
            foreach (string file in files)
            {
                //获取文件
                // ConvertImages(file);
                // 读取文件头
                byte[] fileHeader = new byte[8];
                using (var stream = new FileStream(file, FileMode.Open))
                {
                    stream.Read(fileHeader, 0, 8);
                }

                // 判断文件头是否为PNG
                if (fileHeader[0] != 0x89 || fileHeader[1] != 0x50 || fileHeader[2] != 0x4E || fileHeader[3] != 0x47 || fileHeader[4] != 0x0D || fileHeader[5] != 0x0A || fileHeader[6] != 0x1A || fileHeader[7] != 0x0A)
                {
                    try
                    {
                        using (Image image = Image.Load(file))
                        {
                            image.Save(Path.ChangeExtension(file, "png"), new PngEncoder());
                        }

                        Console.WriteLine("Image converted finished");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error converting image: " + e.Message);
                    }
                }
            }
        }
    }
}
