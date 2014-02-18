using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDFManager_CSharp
{
    public static class MyHelper
    {
        public static int omitBlank(this byte[] array, int index)
        {
            while (array[index] == 32 || array[index] == 13 || array[index] == 10) index++;
            return index;
        }

        public static int findStringDown(this byte[] array, int index, string target)
        {
            if (index >= array.Length) return array.Length;
            if (index < 0) index = 0;
            byte[] tbyte = Encoding.ASCII.GetBytes(target);
            while (!array.matchDown(index, tbyte) && index < array.Length)
            {
                index++;
            }
            return index;
        }

        private static bool matchDown(this byte[] array, int index, byte[] tbyte)
        {
            bool m = true;
            for (int i = 0; i < tbyte.Length && index + i < array.Length && m == true; i++)
            {
                m = m && array[index + i] == tbyte[i];
            }
            return m;
        }

        public static int findStringUp(this byte[] array, int index, string target)
        {
            if (index < 0) return -1;
            if (index >= array.Length) index = array.Length - 1;
            byte[] tbyte = Encoding.ASCII.GetBytes(target);
            while (!array.matchUp(index, tbyte) && index >= 0)
            {
                index--;
            }
            return index;
        }

        private static bool matchUp(this byte[] array, int index, byte[] tbyte)
        {
            bool m = true;
            for (int i = 0; i < tbyte.Length && index - i >= 0 && m == true; i++)
            {
                m = m && array[index - i] == tbyte[tbyte.Length - 1 - i];
            }
            return m;
        }

        public static int getNumber(this byte[] array, ref int index)
        {
            int num = 0;
            while (array[index] != 32 && array[index] != 13 && array[index] != 10)
            {
                num *= 10;
                num += array[index++] - 48;
            }
            return num;
        }
    }
}
