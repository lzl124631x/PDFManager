using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;

namespace PDFManager_CSharp
{
    public class PDF
    {
        public bool readable = true;
        public string show;
        public int InfoAt;
        public int InfoEnd;
        public int TailAt;
        public int TailEnd;
        public bool hasTail = false;

        public string FileName;
        public string DirectoryName;
        public string PDFPath;
        private List<Xref> xrefs = new List<Xref>(); //按照从新到旧的顺序排列
        public string[] Addrs;
        private byte[] PDFInByte;
        public int startxref;
        public List<Attr> Tails = new List<Attr>();

        #region trailer信息
        public int Size;
        public int Root;
        public int Info;
        public int Encrypt;
        public int Tail;
        public string[] ID;
        #endregion

        #region Info信息
        public string Version;
        public string Title;
        public string Author;
        public string Subject;
        public string Keywords;
        public DateTimeValue CreationDate = new DateTimeValue("");
        public DateTimeValue ModDate = new DateTimeValue("");
        public string Creator;
        public string Producer;
        public string Comment;
        public List<Attr> Attrs = new List<Attr>();
        #endregion

        public struct Attr
        {
            public string Name;
            public string Value;
            public Attr(string name, string value)
            {
                Name = name;
                Value = value;
            }
        }

        //问题遗留！！ 
        public class DateTimeValue
        {
            public string prototype;
            public DateTime datetime;
            public int sign;
            public int offsetHour;
            public int offsetMinute;

            public DateTimeValue(string value)
            {
                prototype = value;
                if (value == "") return;
                try
                {
                    datetime = DateTime.ParseExact(value.Substring(2, 14), "yyyyMMddHHmmss", null);
                    value = value.Substring(16);
                    if (value.Length == 0) return;
                    switch (value[0])
                    {
                        case '+': sign = 1; break;
                        case '-': sign = -1; break;
                        case 'Z': return;
                    }
                    offsetHour = Int32.Parse(value.Substring(1, 2));
                    offsetMinute = Int32.Parse(value.Substring(4, 2));
                }
                catch
                { }
            }

            public override string ToString()
            {
                if (prototype == "")
                    return "";
                return datetime.ToString();
            }
        }

        private class Section
        {
            public int startID;
            public int count;
            public List<string> Addrs = new List<string>();
            public Section(int s, int c)
            {
                startID = s;
                count = c;
            }
        }

        private class Xref
        {
            public int xrefOffset;
            public List<Section> sections = new List<Section>();
            public Trailer trailer;

            public int newPrev;
            public int newXrefOffset;

            public Xref(int offset)
            {
                xrefOffset = offset;
            }
        }

        private class Trailer
        {
            public string Prototype;
            public int Size;
            public int Prev;
            public int Root;
            public int Info;
            public string[] ID = new string[2];
            public int Encrypt;
            public int XRefStm;
        }

        private class XrefComparer : IComparer<PDF.Xref>
        {
            public XrefComparer() { }

            public int Compare(PDF.Xref x, PDF.Xref y)
            {
                if (x.xrefOffset < y.xrefOffset)
                    return -1;
                else
                    return 1;
            }
        }

        public PDF(string path)
        {
            PDFPath = path;
            FileName = Path.GetFileName(path);
            DirectoryName = Path.GetDirectoryName(path);
            ReadPath(PDFPath);
        }

        private bool TestValid(string num)
        {
            foreach (char c in num)
            {
                if (c < 48 || c > 57 && c < 65 || c > 70 && c < 97 || c > 102)
                    return false;
            }
            return true;
        }

        private int GetDecimal(string num)
        {
            int dec = 0;
            foreach (char c in num)
            {
                dec *= 16;
                if (c >= 48 && c <= 57)
                {
                    dec += c - 48;
                }
                else
                {
                    switch (c)
                    {
                        case 'A':
                        case 'a': dec += 10; break;
                        case 'B':
                        case 'b': dec += 11; break;
                        case 'C':
                        case 'c': dec += 12; break;
                        case 'D':
                        case 'd': dec += 13; break;
                        case 'E':
                        case 'e': dec += 14; break;
                        case 'F':
                        case 'f': dec += 15; break;
                    }
                }
            }
            return dec;
        }

        private string ReadTagString(string tagString)
        {
            string output = "";
            int index = 0;
            int val = 0;
            List<byte> bytes = new List<byte>();
            while (index < tagString.Length)
            {
                if (tagString[index] != '#')
                {
                    if (bytes.Count != 0)
                    {
                        output += Encoding.UTF8.GetString(bytes.ToArray());
                        bytes.Clear();
                    }
                    output += tagString[index];
                    index++;
                }
                else
                {
                    if (tagString[index + 1] == '#')
                    {
                        if (bytes.Count != 0)
                        {
                            output += Encoding.UTF8.GetString(bytes.ToArray());
                            bytes.Clear();
                        }
                        output += "##";
                        index += 2;
                    }
                    else
                    {
                        if (!TestValid(tagString.Substring(index + 1, 2)))
                        {
                            if (bytes.Count != 0)
                            {
                                output += Encoding.UTF8.GetString(bytes.ToArray());
                                bytes.Clear();
                            }
                            output += "#" + tagString.Substring(index + 1, 2);
                            index += 3;
                        }
                        else
                        {
                            val = GetDecimal(tagString.Substring(index + 1, 2));
                            index += 3;
                            bytes.Add(Convert.ToByte(val));
                        }
                    }
                }
            }
            if (bytes.Count != 0)
            {
                output += Encoding.UTF8.GetString(bytes.ToArray());
                bytes.Clear();
            }
            return output;
        }

        private void ReadPath(string path)
        {
            using (Stream stream = File.Open(PDFPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                PDFInByte = new byte[stream.Length];
                stream.Read(PDFInByte, 0, Convert.ToInt32(stream.Length));                                      //涉及ing与long Read的count只能是int，但是Length是long
            }
            Version = (PDFInByte[5] - 48) + "." + (PDFInByte[7] - 48);
            int index = PDFInByte.Length - 1;
            index = PDFInByte.findStringUp(index, "%%EOF") - 5;
            while (PDFInByte[index] > 57 || PDFInByte[index] < 48) index--;
            int i = 1;
            startxref = 0;

            while (PDFInByte[index] <= 57 && PDFInByte[index] >= 48)
            {
                startxref += (PDFInByte[index--] - 48) * i;
                i *= 10;
            }

            ReadXref(startxref);

            Addrs = new string[Size];
            foreach (Xref xref in xrefs)
            {
                foreach (Section sec in xref.sections)
                {
                    for (int cursor = 0; cursor < sec.count; cursor++)
                    {
                        Addrs[sec.startID + cursor] = sec.Addrs[cursor]; //用新的覆盖旧的
                    }
                }
            }

            #region 读取Info
            if (Info == 0) return;
            index = InfoAt = Int32.Parse(Addrs[Info].Substring(0, 10));

            while (PDFInByte[index++] != 47) ; //找到第一个/,结束时index指向tag的第一个字符
            List<byte> tag = new List<byte>();
            string tagString, valueString;
            while (PDFInByte[index] != 62)//>
            {
                tag.Clear();
                while (PDFInByte[index] != 32 && PDFInByte[index] != 40) //找到空格或者(，结束时index指向空格或(
                {
                    tag.Add(PDFInByte[index]);
                    index++;
                }
                index = PDFInByte.omitBlank(index);
                tagString = Encoding.ASCII.GetString(tag.ToArray());
                tagString = ReadTagString(tagString);

                //开始读取value
                if (PDFInByte[index] != 40)                     //指向其他obj的情况
                {
                    int tagID = PDFInByte.getNumber(ref index);
                    index++;
                    while (PDFInByte[index] != 47 && PDFInByte[index] != 62) index++;             //找下一个/或者结束符>，结束时index指向下一个/或结束符>

                    string tagAddr = Addrs[tagID].Substring(0, 10);
                    int cursor = Int32.Parse(tagAddr);

                    while (PDFInByte[cursor++] != 40) ;          //找(，结束时指向属性的第一个字符

                    ReadValue(cursor, out valueString);
                }
                else  //跟随的括号中有属性的情况
                {
                    index = ReadValue(++index, out valueString) + 1;            //index此时指向)后面一个字符
                    while (PDFInByte[index] != 47 && PDFInByte[index] != 62) index++;             //找下一个/或者结束符>，结束时index指向下一个/或结束符>
                }
                index++;
                //index最终定位在下一个tag的第一个字符或结束符>

                switch (tagString)
                {
                    case "Title": Title = valueString; break;
                    case "Author": Author = valueString; break;
                    case "Subject": Subject = valueString; break;
                    case "Keywords": Keywords = valueString; break;
                    case "CreationDate": CreationDate = new DateTimeValue(valueString); break;
                    case "ModDate": ModDate = new DateTimeValue(valueString); break;
                    case "Creator": Creator = valueString; break;
                    case "Producer": Producer = valueString; break;
                    default: Attrs.Add(new Attr(tagString, valueString)); break;
                }
            }
            while (PDFInByte[index] != 106) index++;//将index指向endobj的j
            InfoEnd = index + 1;
            #endregion

            #region 读取Tail
            if (Tail == 0) return;
            index = TailAt = Int32.Parse(Addrs[Tail].Substring(0, 10));

            while (PDFInByte[index++] != 47) ; //找到第一个/,结束时index指向tag的第一个字符
            while (PDFInByte[index] != 62)//>
            {
                tag.Clear();
                while (PDFInByte[index] != 32 && PDFInByte[index] != 40) //找到空格或者(，结束时index指向空格或(
                {
                    tag.Add(PDFInByte[index]);
                    index++;
                }
                index = PDFInByte.omitBlank(index);
                tagString = Encoding.ASCII.GetString(tag.ToArray());
                tagString = ReadTagString(tagString);

                //开始读取value，默认Tail的属性只以空号的形式保存
                index = ReadValue(++index, out valueString) + 1;            //index此时指向)后面一个字符
                while (PDFInByte[index] != 47 && PDFInByte[index] != 62) index++;             //找下一个/或者结束符>，结束时index指向下一个/或结束符>

                index++;
                //index最终定位在下一个tag的第一个字符或结束符>

                Tails.Add(new Attr(tagString, valueString));
            }
            while (PDFInByte[index] != 106) index++;//将index指向endobj的j
            TailEnd = index + 1;
            #endregion
        }

        private int ReadValue(int start, out string valueString)         //start指向属性的第一个字符
        {
            List<byte> value = new List<byte>();
            int cursor = start;
            if (PDFInByte[cursor] == 254 && PDFInByte[cursor + 1] == 255)       //BigEndianUnicode的情况
            {
                cursor += 2;
                while (true)
                {
                    if (PDFInByte[cursor] == 41) break;           //读取到)，结束 //可以放到while（）里
                    if (PDFInByte[cursor] == 92) cursor++;             //PDF文件逢92要跳过一次
                    value.Add(PDFInByte[cursor++]);
                    if (PDFInByte[cursor] == 92) cursor++;
                    value.Add(PDFInByte[cursor++]);
                }
                valueString = Encoding.BigEndianUnicode.GetString(value.ToArray());
            }
            else
            {
                while (true)
                {
                    if (PDFInByte[cursor] == 41) break;       //读取到)，结束
                    if (PDFInByte[cursor] == 92) cursor++;      //需要转义的情况，跳一位
                    value.Add(PDFInByte[cursor++]);
                }
                valueString = Encoding.UTF7.GetString(value.ToArray());
            }
            return cursor;          //cursor最后指向)
        }

        private void ReadXref(int xrefOffset)
        {
            //暂时假设xref格式比较正规，空格只有一个，免过滤空格 while (PDFInByte[++index] == 32) ;
            int index = xrefOffset;
            Xref xref = new Xref(xrefOffset);
            //若前四个字节不是xref，特殊情况，直接返回；否则结束时index指向xref后面一个字节
            if (PDFInByte[index++] != 120 || PDFInByte[index++] != 114 || PDFInByte[index++] != 101 || PDFInByte[index++] != 102)
            {
                MessageBox.Show("Unknown PDF format!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                readable = false;
                return;
                //index = PDFInByte.findString(index, "stream");
                //index += 6;
                //int end = PDFInByte.findString(index, "endstream");
                //byte[] xrefbytes = new byte[end - index + 1];
                //for (int i = 0; i < xrefbytes.Length; i++)
                //{
                //    xrefbytes[i] = PDFInByte[index + i];
                //}
            }
            else
            {
                index = PDFInByte.omitBlank(index);
                //开始读取地址列表
                int startID;
                int count;
                int lineCount;
                byte[] newAddr;
                List<byte> tBytes;
                List<byte> tag = new List<byte>();
                List<byte> value = new List<byte>();
                string tagString;
                string valueString;
                //读地址列表
                do
                {
                    startID = PDFInByte.getNumber(ref index);
                    index++;                        //免过滤，index指向count第一个字节
                    count = PDFInByte.getNumber(ref index);

                    Section sec = new Section(startID, count);

                    while (PDFInByte[index] == 10 || PDFInByte[index] == 13 || PDFInByte[index] == 32) index++;   //滤过换行符和空格，结束时指向第一个有效字节
                    lineCount = count;
                    while (lineCount > 0)     //读count行结束，结束时index指向数字或者trailer的t
                    {
                        newAddr = new byte[18];
                        for (int i = 0; i < 18; i++)            //10个字节地址，1个空格，5个字节产生号，一个空格，一个字节标志位，2个字节换行，共20字节
                        {
                            newAddr[i] = PDFInByte[index + i];
                        }
                        sec.Addrs.Add(Encoding.ASCII.GetString(newAddr));
                        index += 20;
                        lineCount--;
                    }
                    xref.sections.Add(sec);
                } while (PDFInByte[index] != 116);          //当读到trailer的t时结束，index指向t

                //开始读取trailer，暂时认定xref必定跟随trailer且格式正规，免验证
                tBytes = new List<byte>();
                Trailer trailer = new Trailer();
                while (PDFInByte[++index] != 47) ;      //找第一个/，trailer至少要有Root属性，结束时指向第一个/
                while (PDFInByte[index] != 62 || PDFInByte[index + 1] != 62)            //一直读取到>>的前一个字符
                {
                    tBytes.Add(PDFInByte[index++]);
                }
                trailer.Prototype = Encoding.ASCII.GetString(tBytes.ToArray());

                index = 0;
                while (index < tBytes.Count)
                {
                    tag.Clear();
                    value.Clear();
                    index++;
                    while (tBytes[index] != 32 && tBytes[index] != 91)      //结束时index指向tag后的空格或[
                    {
                        tag.Add(tBytes[index]);
                        index++;
                    }
                    tagString = Encoding.ASCII.GetString(tag.ToArray());
                    while (tBytes[index] == 32) index++;            //滤过空格，找到空格后第一个有效字符

                    while (index < tBytes.Count && tBytes[index] != 47)       //读取value，结束时index指向下一个/或到达结尾
                    {
                        value.Add(tBytes[index++]);
                    }
                    valueString = Encoding.ASCII.GetString(value.ToArray());
                    int end = valueString.IndexOf(" ");
                    end = end == -1 ? valueString.Length : end;
                    valueString = valueString.Substring(0, end);
                    int val;
                    Int32.TryParse(valueString, out val);
                    switch (tagString)      //应该仅有以下几种情况
                    {
                        case "Size":
                            trailer.Size = val;
                            if (Size < val) Size = val;
                            break;
                        case "Info":
                            trailer.Info = val;
                            if (Info == 0) Info = val;
                            break;
                        case "Root":
                            trailer.Root = val;
                            if (Root == 0) Root = val;
                            break;
                        case "Prev":
                            trailer.Prev = val;
                            break;
                        case "Encrypt":
                            trailer.Encrypt = val;
                            if (Encrypt == 0) Encrypt = val;
                            break;
                        case "ID":
                            trailer.ID[0] = valueString.Substring(2, 32);
                            trailer.ID[1] = valueString.Substring(36, 32);
                            if (ID == null) ID = trailer.ID;
                            break;
                        case "XRefStm":
                            trailer.XRefStm = val;
                            break;
                        case "Tail":
                            hasTail = true;
                            if (Tail == 0) Tail = val;
                            break;
                    }
                }
                if (trailer.Prev != 0)
                    ReadXref(trailer.Prev);//递归应该放在此处，否则Info和Prev的顺序先后会导致Info的最终取值不同
                xref.trailer = trailer;
                xrefs.Add(xref);
            }
        }

        public string ShowInfo()
        {
            show = "";
            int i = 1;
            foreach (Xref x in xrefs)
            {
                show += i++ + "----\n";
                show += "XrefOffset:" + x.xrefOffset + "\n";
                show += "Prev:" + x.trailer.Prev + "\n";
                show += "XRefStm:" + x.trailer.XRefStm + "\n";
            }
            show += "----\n";
            show += "InfoAt:\t" + InfoAt + "\nTailAt:\t" + TailAt + "\nstartxref:\t" + startxref + "\n";
            show += xrefs.Count + " xrefs\n";
            foreach (Xref xref in xrefs)
            {
                show += xref.trailer.Prototype + '\n';
            }
            show += "\nAdditive Attributes:\n";
            foreach (Attr attr in Attrs)
            {
                show += attr.Name + "\t\t\t" + attr.Value + '\n';
            }
            show += "\nTail Attributes:\n";
            foreach (Attr tail in Tails)
            {
                show += tail.Name + "\t\t\t" + tail.Value + '\n';
            }
            return show;
        }

        public int GetObjAddr(int objID)
        {
            return Int32.Parse(Addrs[objID].Substring(0, 10));
        }

        //public int getObjEndAddr(int objID)
        //{
        //    int end = getObjAddr(objID);
        //    return end;
        //}

        public void AddASCII(List<byte> writer, string s)
        {
            byte[] bytes = new byte[s.Length];
            bytes = Encoding.ASCII.GetBytes(s);
            foreach (byte b in bytes) writer.Add(b);
        }

        public void AddBigEndianUnicode(List<byte> writer, string s)
        {
            if (s == null || s == "") return;
            List<byte> list = new List<byte>();
            list.Add(254);
            list.Add(255);
            byte[] bytes = new byte[s.Length];
            bytes = Encoding.BigEndianUnicode.GetBytes(s);
            foreach (byte b in bytes)
            {
                if (b == 92) list.Add(92);
                list.Add(b);
            }
            foreach (byte b in list) writer.Add(b);
        }

        private void SaveXrefs(Stream stream, ref int index, int gap, int from, int to, int newStartxref, int boundary)
        {
            List<byte> writer = new List<byte>();
            string str = "";
            for (int cursor = from; cursor < to; cursor++)
            {
                //xref之前
                while (index < xrefs[cursor].xrefOffset)
                {
                    stream.WriteByte(PDFInByte[index++]);
                }
                //xref地址部分
                while (PDFInByte[index] != 13 && PDFInByte[index] != 10) stream.WriteByte(PDFInByte[index++]);
                while (PDFInByte[index] == 13 || PDFInByte[index] == 10) stream.WriteByte(PDFInByte[index++]);
                foreach (PDF.Section sec in xrefs[cursor].sections)
                {
                    while (PDFInByte[index] != 13 && PDFInByte[index] != 10) stream.WriteByte(PDFInByte[index++]);
                    while (PDFInByte[index] == 13 || PDFInByte[index] == 10) stream.WriteByte(PDFInByte[index++]);
                    for (int count = 0; count < sec.count; count++)
                    {
                        byte[] Addr = new byte[10];
                        for (int i = 0; i < 10; i++)
                        {
                            Addr[i] = PDFInByte[index + i];
                        }
                        int num = Int32.Parse(Encoding.ASCII.GetString(Addr));
                        if (num > boundary)
                        {
                            num += gap;
                            for (int i = 0; i < 10; i++)
                            {
                                Addr[9 - i] = Convert.ToByte(num % 10 + 48);
                                num /= 10;
                            }
                        }
                        stream.Write(Addr, 0, 10);
                        index += 10;
                        stream.Write(PDFInByte, index, 10);
                        index += 10;
                    }
                }
                //trailer的处理
                while (PDFInByte[index] != 62 || PDFInByte[index + 1] != 62)
                {
                    if (PDFInByte[index] == 80 && PDFInByte[index + 1] == 114 && PDFInByte[index + 2] == 101 && PDFInByte[index + 3] == 118)//Prev
                    {
                        writer.Clear();
                        str = "Prev " + xrefs[cursor].newPrev.ToString();
                        AddASCII(writer, str);
                        foreach (byte b in writer)//stream.Write(writer.ToArray(), 0, writer.Count);哪个更高效？
                        {
                            stream.WriteByte(b);
                        }
                        index += str.Length;
                        continue;
                    }
                    if (PDFInByte[index] == 88 && PDFInByte[index + 1] == 82 && PDFInByte[index + 2] == 101 && PDFInByte[index + 3] == 102)//XRef
                    {
                        writer.Clear();
                        str = "XRefStm " + xrefs[cursor].trailer.XRefStm.ToString();
                        int len = str.Length;
                        index += len;
                        str = "DelTag ";
                        for (int i = 0; i < len - 7; i++)
                            str += "0";
                        AddASCII(writer, str);
                        foreach (byte b in writer)
                        {
                            stream.WriteByte(b);
                        }
                        continue;
                    }
                    stream.WriteByte(PDFInByte[index++]);
                }//结束时index指向>>的第一个>
                //看是否有startxref
                if (cursor == xrefs.Count - 1)
                {
                    int see = index + 2;
                    while (PDFInByte[see] == 13 || PDFInByte[see] == 10) see++; //结束时指向下一行第一个字符
                    //若有start
                    if (PDFInByte[see] == 115 && PDFInByte[see + 1] == 116 && PDFInByte[see + 2] == 97 && PDFInByte[see + 3] == 114 && PDFInByte[see + 4] == 116)
                    {
                        while (PDFInByte[see] < 48 || PDFInByte[see] > 57) see++;//see定位在第一个数字
                        while (index < see) stream.WriteByte(PDFInByte[index++]);//结束时index指向第一个数字

                        str = newStartxref.ToString();

                        writer.Clear();

                        AddASCII(writer, str);
                        foreach (byte b in writer)
                        {
                            stream.WriteByte(b);
                        }
                        index += str.Length;

                        if (cursor == xrefs.Count - 1)
                        {
                            while (index < PDFInByte.Length)
                            {
                                stream.WriteByte(PDFInByte[index++]);
                            }
                            break;
                        }
                    }
                }
            }
        }

        private string ProduceNameString(string tagName)
        {
            byte[] temp;
            int dec;
            List<char> value = new List<char>();
            string valueString = "";
            foreach (char c in tagName)
            {
                if (c < 48 || c > 57 && c < 65 || c > 90 && c < 97 || c > 122)
                {
                    temp = Encoding.UTF8.GetBytes(c.ToString());
                    foreach (byte b in temp)
                    {
                        dec = Convert.ToInt32(b);
                        value.Clear();
                        while (dec != 0)
                        {
                            if (dec % 16 < 10) value.Add((char)('0' + dec % 16));
                            else value.Add((char)('A' + dec % 16 - 10));
                            dec /= 16;
                        }
                        if (value.Count == 0)
                        {
                            value.Add('0');
                            value.Add('0');
                        }
                        if (value.Count == 1) value.Add('0');
                        value.Add('#');
                        value.Reverse();
                        foreach (char ch in value)
                        {
                            valueString += ch;
                        }
                    }
                }
                else
                {
                    valueString += c;
                }
            }
            return valueString;
        }

        public void SaveInfo(string title, string author, string subject, string keywords, List<Attr> attrs)
        {
            Stream stream = File.Open(PDFPath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            stream.SetLength(0);  //清空文件

            List<byte> writer = new List<byte>();
            string str;
            //生成新的Info
            str = Info.ToString();
            str += " 0 obj\r\n<</Title(";
            AddASCII(writer, str);
            AddBigEndianUnicode(writer, title);
            str = ")/Author(";
            AddASCII(writer, str);
            AddBigEndianUnicode(writer, author);
            str = ")/Subject(";
            AddASCII(writer, str);
            AddBigEndianUnicode(writer, subject);
            str = ")/Keywords(";
            AddASCII(writer, str);
            AddBigEndianUnicode(writer, keywords);
            str = ")/CreationDate(";
            AddASCII(writer, str);
            AddASCII(writer, CreationDate.prototype);
            str = ")/ModDate(";
            AddASCII(writer, str);
            AddASCII(writer, ModDate.prototype);
            str = ")/Creator(";
            AddASCII(writer, str);
            AddBigEndianUnicode(writer, Creator);
            str = ")/Producer(";
            AddASCII(writer, str);
            AddBigEndianUnicode(writer, Producer);
            foreach (Attr attr in attrs)
            {
                str = ")/" + ProduceNameString(attr.Name) + "(";
                AddASCII(writer, str);
                AddBigEndianUnicode(writer, attr.Value);
            }
            str = ")>>\r\nendobj";
            AddASCII(writer, str);
            byte[] InfoBytes = writer.ToArray();

            int index = 0;
            int pointA = GetObjAddr(Info);
            int pointB = InfoEnd;
            int gap = InfoBytes.Length - pointB + pointA;
            //更新xrefs,此处只更新Prev和XRefStm
            for (int i = 0; i < xrefs.Count; i++)
            {
                xrefs[i].newXrefOffset = xrefs[i].xrefOffset;
                xrefs[i].newPrev = xrefs[i].trailer.Prev;
                if (i > 0)
                {
                    xrefs[i].newPrev = xrefs[i - 1].newXrefOffset;
                }
                if (xrefs[i].xrefOffset > pointA)        //未检测位数变化
                {
                    xrefs[i].newXrefOffset += gap;
                }
                if (xrefs[i].trailer.XRefStm > pointA)
                {
                    xrefs[i].trailer.XRefStm += gap;
                }
            }
            int newStartxref = xrefs[xrefs.Count - 1].newXrefOffset;
            xrefs.Sort(new XrefComparer());
            int pos = 0;
            foreach (PDF.Xref x in xrefs)   //pos确定Info在Xrefs的排位,指示在哪一个xref之前
            {
                if (x.xrefOffset < pointA) pos++;
                else break;
            }

            SaveXrefs(stream, ref index, gap, 0, pos, newStartxref, InfoAt);
            //写到Info之前
            while (index < pointA)
            {
                stream.WriteByte(PDFInByte[index++]);
            }
            //重写Info
            foreach (byte b in InfoBytes)
            {
                stream.WriteByte(b);
            }
            //写Info之后，xref之前
            index = pointB;
            SaveXrefs(stream, ref index, gap, pos, xrefs.Count, newStartxref, InfoAt);

            stream.Close();
        }

        public void AppendTail(List<Attr> tails)
        {
            Stream stream = File.Open(PDFPath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            List<byte> writer = new List<byte>();
            string str;
            Tail = hasTail ? Tail : Size;
            str = Tail + " 0 obj\r\n<<";
            AddASCII(writer, str);
            foreach (Attr tail in tails)
            {
                str = "/" + ProduceNameString(tail.Name) + "(";
                AddASCII(writer, str);
                AddBigEndianUnicode(writer, tail.Value);
                writer.Add(41);//)的ASCII为41
            }
            str = ">>\r\nendobj";
            AddASCII(writer, str);
            if (hasTail)
            {
                stream.SetLength(TailAt + 1);  //从TailAt处开始重写
                stream.Seek(TailAt, SeekOrigin.Begin);

                int gap = writer.Count - TailEnd + TailAt;
                //更新xrefs
                for (int i = 0; i < xrefs.Count; i++)
                {
                    xrefs[i].newXrefOffset = xrefs[i].xrefOffset;
                    xrefs[i].newPrev = xrefs[i].trailer.Prev;
                    if (i > 0)
                    {
                        xrefs[i].newPrev = xrefs[i - 1].newXrefOffset;
                    }
                    if (xrefs[i].xrefOffset > TailAt)        //未检测位数变化
                    {
                        xrefs[i].newXrefOffset += gap;
                    }
                    if (xrefs[i].trailer.XRefStm > TailAt)
                    {
                        xrefs[i].trailer.XRefStm += gap;
                    }
                }
                int newStartxref = xrefs[xrefs.Count - 1].newXrefOffset;
                xrefs.Sort(new XrefComparer());
                int pos = 0;
                foreach (PDF.Xref x in xrefs)   //pos确定Tail在Xrefs的排位,指示在哪一个xref之前
                {
                    if (x.xrefOffset < TailAt) pos++;
                    else break;
                }

                //重写Tail
                stream.Write(writer.ToArray(), 0, writer.Count);
                //写Tail之后，xref之前
                int index = TailEnd;
                SaveXrefs(stream, ref index, gap, pos, xrefs.Count, newStartxref, TailAt);
            }
            else
            {
                stream.Seek(0, SeekOrigin.End);
                TailAt = (int)stream.Length + 2;//+2因为Tail对象前面补充了\r\n
                TailEnd = TailAt + writer.Count;
                Tail = Size;
                str = "\r\nxref\r\n" + Tail + " 1\r\n";
                for (int i = 0; i < 10 - TailAt.ToString().Length; i++)
                    str += "0";
                str += TailAt.ToString() + " 00000 n\r\n";
                str += "trailer\r\n<<";//假设Size什么的都有
                str += "/Size " + ++Size;
                str += "/Root " + Root + " 0 R";
                str += "/Info " + Info + " 0 R";
                str += "/Prev " + startxref;
                str += "/Tail " + Tail + " 0 R";
                str += ">>\r\nstartxref\r\n" + (TailEnd + 2) + "\r\n%%EOF";
                AddASCII(writer, str);//writer此前保存的是Tail的信息，没有首尾的\r\n
                //写Tail前的\r\n
                stream.WriteByte(13);//\r
                stream.WriteByte(10);//\n
                stream.Write(writer.ToArray(), 0, writer.Count);
            }
            stream.Close();
        }
    }
}
