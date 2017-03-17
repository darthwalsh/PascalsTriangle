// PascalsTriangle, by Carl Walsh Sept 2010
using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing;
using System.Diagnostics;

namespace Pascals
{
    class Config
    {
        public int Rows;
        public int Mod;
        public bool Text;
        public string SaveFile;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Config config = GetConfig(args);

            Triangle tri = new Triangle(config.Rows, config.Mod);

            if (config.Text)
            {
                tri.Print();
            }
            else
            {
                Bitmap image = tri.Image();

                if (config.SaveFile != null)
                {
                    image.Save(config.SaveFile, ImageFormat.Bmp);
                }
                else
                {
                    Form screen = new Form
                    {
                        Text = "Pascal Triangle with " + config.Rows + " rows, mod " + config.Mod
                    };
                    screen.SetBounds(30, 30, image.Width + 16, image.Height + 38);
                    screen.Show();
                    screen.SetDesktopLocation(0, 0);
                    Graphics g = Graphics.FromHwnd(screen.Handle);
                    g.DrawImage(image, 0, 0);
                    Console.ReadLine();
                }
            }
        }

        static Config GetConfig(string[] args)
        {
            if (args.Length == 0)
            {
                return ReadConfig();
            }

            if (args.Length == 2 || args.Length == 3)
            {
                var config = new Config
                {
                    Rows = Int32.Parse(args[0]),
                    Mod = Int32.Parse(args[1]),
                    Text = true,
                };
                if (args.Length == 3)
                {
                    config.Text = false;
                    config.SaveFile = args[2];
                }
                return config;
            }

            PrintUsage();
            Environment.Exit(1);
            throw null;
        }

        static void PrintUsage()
        {
            Console.WriteLine("Usage: PascalsTriangle [rows  mod [fileName]]");
            Console.WriteLine("");
            Console.WriteLine("Displays Pascal's Triangle, modulus a custom parameter. Try 2, 4, 6, or 12.");
            Console.WriteLine("Optionally pass row count and modulus. Optionally pass a file name to save image.");
        }

        static Config ReadConfig()
        {
            var config = new Config();

            Console.WriteLine("How many rows?...");
            String reply = Console.ReadLine();
            config.Rows = Int32.Parse(reply);
            Console.WriteLine("Modded to?");
            reply = Console.ReadLine();
            config.Mod = Int32.Parse(reply);
            if (config.Rows < 1)
                config.Rows = 1;
            if (config.Mod < 1)
                config.Mod = 1;
            Console.WriteLine("Creating a Pascal's Triangle with {0} rows, mod {1}", config.Rows, config.Mod);
            Console.WriteLine("Display as image, or as text? (i/t)");
            reply = Console.ReadLine();

            if (reply.ToLower() != "i")
                config.Text = true;

            return config;
        }
    }
}
