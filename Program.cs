// PascalsTriangle, by Carl Walsh Sept 2010
using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing;

namespace Pascals
{
    class Program
    {
        static void Main(string[] args)
        {
            int ROWS = 0;
            int MOD = 0;
            bool MAKE_IMAGE = false;
            Console.WriteLine("How many rows?...");
            String reply = Console.ReadLine();
            try
            {
                ROWS = Int32.Parse(reply);
            }
            catch { }
            Console.WriteLine("Modded to?");
            reply = Console.ReadLine();
            try
            {
                MOD = Int32.Parse(reply);
            }
            catch { }
            if (ROWS < 1)
                ROWS = 1;
            if (MOD < 1)
                MOD = 1;
            Console.WriteLine("Creating a Pascal's Triangle with {0} rows, mod {1}", ROWS, MOD);
            Triangle tri = new Triangle(ROWS, MOD);
            Console.WriteLine("Display as image, or as text? (i/t)");
            reply = Console.ReadLine();
            if (reply.ToLower() == "i")
                MAKE_IMAGE = true;
            if (MAKE_IMAGE)
            {
                Form screen = new Form();
                screen.Text = "Pascal Triangle with " + ROWS + " rows, mod " + MOD;
                Bitmap pic = tri.Image();
                GDI32.SaveImage(pic);
                screen.SetBounds(30, 30, pic.Width + 16, pic.Height  + 38);
                screen.Show();
                screen.SetDesktopLocation(0, 0);
                Graphics g = Graphics.FromHwnd(screen.Handle);
                g.DrawImage(pic, 0, 0);
            }
            else
                tri.Print();
            Console.ReadLine();
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
