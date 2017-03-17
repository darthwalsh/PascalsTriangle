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
                Form screen = new Form();
                screen.Text = "Pascal Triangle with " + config.Rows + " rows, mod " + config.Mod;
                Bitmap pic = tri.Image();
                GDI32.SaveImage(pic);
                screen.SetBounds(30, 30, pic.Width + 16, pic.Height + 38);
                screen.Show();
                screen.SetDesktopLocation(0, 0);
                Graphics g = Graphics.FromHwnd(screen.Handle);
                g.DrawImage(pic, 0, 0);
            }

            if (Debugger.IsAttached)
            {
                Console.ReadLine();
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

    class GDI32
    { //http://www.c-sharpcorner.com/UploadFile/perrylee/ScreenCapture11142005234547PM/ScreenCapture.aspx
        [DllImport("GDI32.dll")]
        public static extern bool BitBlt(int hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, int hdcSrc, int nXSrc, int nYSrc, int dwRop);
        [DllImport("GDI32.dll")]
        public static extern int CreateCompatibleBitmap(int hdc, int nWidth, int nHeight);
        [DllImport("GDI32.dll")]
        public static extern int CreateCompatibleDC(int hdc);
        [DllImport("GDI32.dll")]
        public static extern bool DeleteDC(int hdc);
        [DllImport("GDI32.dll")]
        public static extern bool DeleteObject(int hObject);
        [DllImport("GDI32.dll")]
        public static extern int GetDeviceCaps(int hdc, int nIndex);
        [DllImport("GDI32.dll")]
        public static extern int SelectObject(int hdc, int hgdiobj);
        [DllImport("User32.dll")]
        public static extern int GetDesktopWindow();
        [DllImport("User32.dll")]
        public static extern int GetWindowDC(int hWnd);
        [DllImport("User32.dll")]
        public static extern int ReleaseDC(int hWnd, int hDC);

        static int pictNumber = 0;

        public static Bitmap CaptureScreen()
        {
            int hdcSrc = GetWindowDC(GetDesktopWindow()),
            hdcDest = GDI32.CreateCompatibleDC(hdcSrc),
            hBitmap = GDI32.CreateCompatibleBitmap(
                            hdcSrc,
                            GDI32.GetDeviceCaps(hdcSrc, 8),
                            GDI32.GetDeviceCaps(hdcSrc, 10));
            GDI32.SelectObject(hdcDest, hBitmap);
            GDI32.BitBlt(hdcDest, 0, 0, GDI32.GetDeviceCaps(hdcSrc, 8),
            GDI32.GetDeviceCaps(hdcSrc, 10), hdcSrc, 0, 0, 0x00CC0020);
            Bitmap screen = Image.FromHbitmap(new IntPtr(hBitmap));
            Cleanup(hBitmap, hdcSrc, hdcDest);
            return screen;
        }
        private static void Cleanup(int hBitmap, int hdcSrc, int hdcDest)
        {
            ReleaseDC(GetDesktopWindow(), hdcSrc);
            GDI32.DeleteDC(hdcDest);
            GDI32.DeleteObject(hBitmap);
        }
        public static void SaveImage(Bitmap image)
        {
            // Puts the file into "debugging output"
            pictNumber++;
            image.Save("todelete" + pictNumber + ".bmp", ImageFormat.Bmp);
        }
    }
}
