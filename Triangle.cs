// PascalsTriangle, by Carl Walsh Sept 2010
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Pascals
{
    class Triangle
    {
        private List<int[]> rows;
        private int MOD;
        private int ROWS;
        public Triangle(int numRows, int mod)
        {
            ROWS = numRows;
            MOD = mod;
            rows = new List<int[]>();
            for(int i = 0; i < numRows; ++i)
            {
                rows.Add(new int[i+1]);
                if(i == 0)
                    rows[i][0] = 1;
                else
                    for(int n = 0; n <= i; n++)
                        rows[i][n] = getSum(i,n, mod);
            }
        }

        private int getSum(int i, int n, int mod)
        {
            int[] row = rows[i-1];
            int left = 0;
            if(n != 0)
                left = row[n-1];
            int right = 0;
            if(n < row.Count())
                right = row[n];
            return (right + left) % mod;
        }

        public Bitmap Image()
        {
            //There will be a square dot for each number, of dynamically chosen size
            int DOT_SIZE = 625 / ROWS; //the pixel dimension of a dot
            if (DOT_SIZE > 50)
                DOT_SIZE = 50;
            if (DOT_SIZE < 1)
                DOT_SIZE = 1;

            Bitmap ans = new Bitmap(ROWS * DOT_SIZE, ROWS * DOT_SIZE);
            for (int y = 0; y < ans.Height; ++y)
                for (int x = 0; x < ans.Width; ++x)
                    ans.SetPixel(x, y, Color.White);
            //0 is black, and the other colors are rainbow
            Color[] color = new Color[MOD];
            color[0] = Color.White;
            for (int i = 1; i < MOD; ++i)
                color[i] = ColorHandler.HSVtoColor(255 * (i - 1) / MOD, 255, 255);
            //TODO get rid of
            for (int r = 0; r < ROWS; ++r)
                for (int c = 0; c <= r; ++c)
                    changeDot(c, r, ROWS, color[rows[r][c]], DOT_SIZE, ans);
            return ans;
        }

        private void changeDot(int column, int row, int ROWS,
                               Color c, int dotSize, Bitmap img)
        {
            int offset = dotSize * (ROWS - row - 1) / 2;
            Point p = new Point(offset + column * dotSize, row * dotSize);
            int x = 0;
            while (x < dotSize)
            {
                p.Y = row * dotSize;
                int y = 0;
                while (y < dotSize)
                {
                    img.SetPixel(p.X, p.Y, c);
                    ++y;
                    ++p.Y;
                }
                ++x;
                ++p.X;
            }
        }

        public void Print()
        {
            for (int i = 0; i < rows.Count; ++i)
            {
                for (int j = 1; j < rows.Count - i; ++j)
                    Console.Write(' ');
                for (int n = 0; n < rows[i].Count(); ++n)
                {
                    Console.Write(rows[i][n]);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
        }
    }
}
