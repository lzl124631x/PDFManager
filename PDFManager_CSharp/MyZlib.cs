using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDFManager_CSharp
{
    public class MyZlib
    {
        public class HuffmanTree
        {
            public class HuffmanTreeBranch
            {
                public char Symbol;
                public int Length;
                public int CodeValue;
                public string CodeString;

                public HuffmanTreeBranch(char ch, int len)
                {
                    Symbol = ch;
                    Length = len;
                }
            }

            public List<HuffmanTreeBranch> Branches;

            public HuffmanTree(char[] symbols, int[] lengths)
            {
                Branches = new List<HuffmanTreeBranch>();
                for (int i = 0; i < symbols.Length; i++)
                {
                    Branches.Add(new HuffmanTreeBranch(symbols[i], lengths[i]));
                }
            }
        }

        public MyZlib()
        {

        }

        private string DecToBinary(int dec, int length)
        {
            List<char> value = new List<char>();
            while (dec != 0)
            {
                switch (dec % 2)
                {
                    case 0: value.Add('0'); break;
                    case 1: value.Add('1'); break;
                }
                dec /= 2;
            }
            value.Reverse();
            string valueString = "";
            for (int i = 0; i < length - value.Count; i++)
            {
                valueString += '0';
            }
            foreach (char c in value)
            {
                valueString += c;
            }
            return valueString;
        }

        public HuffmanTree GenerateHuffmanTree(char[] symbols, int[] lengths, int maxBitLength)
        {
            HuffmanTree tree = new HuffmanTree(symbols, lengths);

            //Step 1. Count the number of codes for each code length.
            int[] codeBLCount = new int[maxBitLength + 1];
            for (int i = 0; i < tree.Branches.Count; i++)
            {
                codeBLCount[tree.Branches[i].Length]++;
            }

            //Step 2. Find the numerical value of the smallest code for each code length.
            int code = 0;
            codeBLCount[0] = 0;
            int[] next_code = new int[maxBitLength + 1];
            int len;
            for (len = 1; len <= maxBitLength; len++)
            {
                code = (code + codeBLCount[len - 1]) << 1;
                next_code[len] = code;
            }

            //Step 3. Assign numerical values to all codes.
            for (int n = 0; n < tree.Branches.Count; n++)
            {
                len = tree.Branches[n].Length;
                if (len != 0)
                {
                    tree.Branches[n].CodeValue = next_code[len];
                    tree.Branches[n].CodeString = DecToBinary(next_code[len], len);
                    next_code[len]++;
                }
            }
            return tree;
        }
    }
}
