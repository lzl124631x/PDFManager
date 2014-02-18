using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace PDFManager_CSharp
{
    public partial class PDFManager_CSharp : Form
    {
        bool canSelectRow = false; //初次选定pdf、未选定情况下sort、删除一行时应为false
        bool noRowSelected = true;
        int propIndex = -1;

        bool canSelectRowTail = false;
        bool noRowSelectedTail = true;
        int propIndexTail = -1;

        public PDFManager_CSharp()
        {
            InitializeComponent();
        }

        private void PDFManager_CSharp_Load(object sender, EventArgs e)
        {
            lbCreationDate.Text = "";
            lbModDate.Text = "";
            lbCreator.Text = "";
            lbProducer.Text = "";
            lbVersion.Text = "";
            lbFileName.Text = "";
            lbDirectoryName.Text = "";
            btnRefresh.Enabled = false;
            btnCustom.Enabled = false;
            btnDelete.Enabled = false;
            dgvCustom.AllowUserToResizeColumns = false;
            dgvCustom.AllowUserToResizeRows = false;
            dgvCustom.AllowUserToAddRows = false;
            dgvCustom.BackgroundColor = Color.White;
            dgvCustom.BorderStyle = BorderStyle.Fixed3D;
            dgvCustom.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCustom.RowHeadersVisible = false;
            dgvCustom.MultiSelect = false;
            dgvCustom.ReadOnly = true;
            dgvCustom.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustom.CellBorderStyle = DataGridViewCellBorderStyle.None;

            dgvTails.AllowUserToResizeColumns = false;
            dgvTails.AllowUserToResizeRows = false;
            dgvTails.AllowUserToAddRows = false;
            dgvTails.BackgroundColor = Color.White;
            dgvTails.BorderStyle = BorderStyle.Fixed3D;
            dgvTails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvTails.RowHeadersVisible = false;
            dgvTails.MultiSelect = false;
            dgvTails.ReadOnly = true;
            dgvTails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTails.CellBorderStyle = DataGridViewCellBorderStyle.None;
        }

        private void scanFiles(string path)
        {
            string[] files = Directory.GetFiles(path);
            string[] dirs = Directory.GetDirectories(path);
            foreach (string file in files)
            {
                if (Path.GetExtension(file).ToUpper() == ".PDF")
                {
                    TreeNode node = new TreeNode(Path.GetFileName(file));
                    node.Tag = file;
                    tvPDF.Nodes.Add(node);
                }
            }
            foreach (string dir in dirs)
            {
                scanFiles(dir);
            }
        }

        private void scanPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnRefresh.Enabled = false;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = @"Z:\My Folder\Files\PDF相关\PDFs\Yes";
            fbd.Description = "Please select a folder:";
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tvPDF.Nodes.Clear();
                scanFiles(fbd.SelectedPath);
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            using (Stream stream = File.Open(tvPDF.SelectedNode.Tag.ToString(), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (tvPDF.SelectedNode == null) return;
                long start;
                if (!Int64.TryParse(tbFrom.Text.Trim(), out start)) return;
                if (start >= stream.Length) start = stream.Length;
                if (start < 0) start = 0;
                if (rbEnd.Checked == true) start = stream.Length - start;
                stream.Seek(start, SeekOrigin.Begin);
                long num = Int64.Parse(tbNum.Text);
                long length = stream.Length - start > num ? num : stream.Length - start;
                byte[] bytes = new byte[length];
                string show = "";
                stream.Read(bytes, 0, Convert.ToInt32(length));
                show += "Text:\n" + Encoding.Default.GetString(bytes).Replace("\0", "\\0") + "\n\n";
                string byteString = "";
                if (cbByte.Checked)
                {
                    byteString += "Binary:\n";
                    foreach (byte b in bytes)
                    {
                        byteString += b + " ";
                    }
                    show += byteString;
                }
                rtbBinaryViewer.Text = show;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<PDF.Attr> attrs = new List<PDF.Attr>();
            foreach (DataGridViewRow row in dgvCustom.Rows)
            {
                attrs.Add(new PDF.Attr(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString()));
            }
            string path = tvPDF.SelectedNode.Tag.ToString();
            PDF pdf = new PDF(path);
            pdf.SaveInfo(tbTitle.Text.Trim(), tbAuthor.Text.Trim(), tbSubject.Text.Trim(), tbKeywords.Text.Trim(), attrs);
            MessageBox.Show("Saved successfully！", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            Process.Start(tvPDF.SelectedNode.Tag.ToString());
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", "/select," + tvPDF.SelectedNode.Tag.ToString());
        }

        private void tvPDF_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ShowInfo();
        }

        private void ShowInfo()
        {
            canSelectRow = false;
            canSelectRowTail = false;
            btnRefresh.Enabled = true;
            PDF pdf = new PDF(tvPDF.SelectedNode.Tag.ToString());
            rtbShow.Text = pdf.ShowInfo();
            tbTitle.Text = pdf.Title;
            tbAuthor.Text = pdf.Author;
            tbSubject.Text = pdf.Subject;
            tbKeywords.Text = pdf.Keywords;
            lbCreationDate.Text = pdf.CreationDate.ToString();
            lbModDate.Text = pdf.ModDate.ToString();
            lbCreator.Text = pdf.Creator;
            lbProducer.Text = pdf.Producer;
            lbVersion.Text = pdf.Version;
            lbFileName.Text = pdf.FileName;
            lbDirectoryName.Text = pdf.DirectoryName;
            //更新dgvCustom
            dgvCustom.Rows.Clear();
            tbName.Text = "";
            rtbValue.Text = "";
            for (int i = 0; i < pdf.Attrs.Count; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgvCustom);
                row.Cells[0].Value = pdf.Attrs[i].Name;
                row.Cells[1].Value = pdf.Attrs[i].Value;
                dgvCustom.Rows.Add(row);
            }
            canSelectRow = false;
            dgvCustom.ClearSelection();
            if (pdf.Attrs.Count == 0)
            {
                btnCustom.Enabled = false;
                btnDelete.Enabled = false;
            }
            //更新dgvTails
            dgvTails.Rows.Clear();
            tbTailName.Text = "";
            rtbTailValue.Text = "";
            for (int i = 0; i < pdf.Tails.Count; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgvTails);
                row.Cells[0].Value = pdf.Tails[i].Name;
                row.Cells[1].Value = pdf.Tails[i].Value;
                dgvTails.Rows.Add(row);
            }
            canSelectRowTail = false;
            dgvTails.ClearSelection();
            if (pdf.Attrs.Count == 0)
            {
                btnTailAdd.Enabled = false;
                btnTailDelete.Enabled = false;
            }
            if (!pdf.readable)
            {
                btnSave.Enabled = false;
                return;
            }
            btnSave.Enabled = true;
            if (cbObj.Checked == true)
            {
                cbObjNum.Items.Clear();
                for (int i = 0; i < pdf.Size; i++)
                {
                    TreeNode node = new TreeNode(i.ToString());
                    node.Tag = Int32.Parse(pdf.Addrs[i].Substring(0, 10));
                    cbObjNum.Items.Add(node);
                }
                cbObjNum.ValueMember = "Text";
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ShowInfo();
        }

        private void cbObjNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            Stream stream = File.Open(tvPDF.SelectedNode.Tag.ToString(), FileMode.Open, FileAccess.Read, FileShare.Read);
            long start = Convert.ToInt32(((TreeNode)cbObjNum.SelectedItem).Tag.ToString());
            stream.Seek(start, SeekOrigin.Begin);
            int num = Int32.Parse(tbNum.Text);
            long length = stream.Length - start > num ? num : stream.Length - start;
            byte[] bytes = new byte[length];
            string show = "Location:\t" + ((TreeNode)cbObjNum.SelectedItem).Tag.ToString() + "\n";
            stream.Read(bytes, 0, Convert.ToInt32(length));
            show += "Text:\n" + Encoding.Default.GetString(bytes).Replace("\0", "\\0") + '\n';
            show += "Binary:\n";
            foreach (byte b in bytes)
            {
                show += b + " ";
            }
            rtbBinaryViewer.Text = show;
            stream.Close();
        }

        private void dgvCustom_SelectionChanged(object sender, EventArgs e)
        {
            if (!canSelectRow)
            {
                dgvCustom.ClearSelection();
            }
            else
            {
                tbName.Text = dgvCustom.SelectedRows[0].Cells[0].Value.ToString();
                rtbValue.Text = dgvCustom.SelectedRows[0].Cells[1].Value.ToString();
                btnCustom.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        public int GetRowIndexAt(DataGridView dgv, int mouseLocation_Y)
        {
            if (dgv.FirstDisplayedScrollingRowIndex < 0)
            {
                return -1;
            }
            if (dgv.ColumnHeadersVisible == true && mouseLocation_Y <= dgv.ColumnHeadersHeight)
            {
                return -1;
            }
            int index = dgv.FirstDisplayedScrollingRowIndex;
            int displayedCount = dgv.DisplayedRowCount(true);
            for (int k = 1; k <= displayedCount; )
            {
                if (dgv.Rows[index].Visible == true)
                {
                    Rectangle rect = dgv.GetRowDisplayRectangle(index, true);  // 取该区域的显示部分区域   
                    if (rect.Top <= mouseLocation_Y && mouseLocation_Y < rect.Bottom)
                    {
                        return index;
                    }
                    k++;
                }
                index++;
            }
            return -1;
        }

        private bool clickOnColumnHeader(DataGridView dgv, int mouseLocation_Y)
        {
            if (dgv.ColumnHeadersVisible == true && mouseLocation_Y <= dgvCustom.ColumnHeadersHeight)
            {
                return true;
            }
            return false;
        }

        private void dgvCustom_MouseClick(object sender, MouseEventArgs e)
        {
            int index = GetRowIndexAt(dgvCustom, e.Y);
            bool onHeader = clickOnColumnHeader(dgvCustom, e.Y);
            if (index == -1) //没点击某行
            {
                //选空白处时清空触发选定，此处取消选定；选表头时不用做任何修改
                if (!onHeader) //点击空白处
                {
                    canSelectRow = false;
                    dgvCustom.ClearSelection();
                    tbName.Text = "";
                    rtbValue.Text = "";
                    btnCustom.Enabled = false;
                    btnDelete.Enabled = false;
                    noRowSelected = true;
                }
            }
            else
            {
                canSelectRow = true;
                noRowSelected = false;
                //currentcell与是否被选中无关！！
                dgvCustom.Rows[index].Selected = true; ; //此处与dgvCustom.Rows[index].Selected = true;类似，但是后者方法无法改currentcell
            }
        }

        private void dgvCustom_Sorted(object sender, EventArgs e)
        {
            if (noRowSelected)
            {
                canSelectRow = false;
            }
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            btnCustom.Text = "Add";
            propIndex = -1;
            if (tbName.Text == "")
            {
                btnCustom.Enabled = false;
                btnDelete.Enabled = false;
                return;
            }
            foreach (DataGridViewRow row in dgvCustom.Rows)
            {
                if (tbName.Text == row.Cells[0].Value.ToString())
                {
                    propIndex = row.Index;
                    btnCustom.Text = "Modify";
                    break;
                }
            }
            if (propIndex != -1)
                btnDelete.Enabled = true;
            else
                btnDelete.Enabled = false;
            if (rtbValue.Text == "")
                btnCustom.Enabled = false;
            else
                btnCustom.Enabled = true;
        }

        private void rtbValue_TextChanged(object sender, EventArgs e)
        {
            if (tbName.Text == "")
            {
                btnCustom.Enabled = false;
                btnDelete.Enabled = false;
                return;
            }
            if (propIndex != -1)
                btnDelete.Enabled = true;
            else
                btnDelete.Enabled = false;
            if (rtbValue.Text == "")
                btnCustom.Enabled = false;
            else
                btnCustom.Enabled = true;
        }

        private void btnCustom_Click(object sender, EventArgs e)
        {
            if (btnCustom.Text == "Modify")
            {
                dgvCustom.Rows[propIndex].Cells[1].Value = rtbTailValue.Text;
            }
            else
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgvCustom);
                row.Cells[0].Value = tbName.Text;
                row.Cells[1].Value = rtbValue.Text;
                dgvCustom.Rows.Add(row);
                propIndex = row.Index;
                canSelectRow = true;
                row.Selected = true;
                btnCustom.Text = "Modify";
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            canSelectRow = false;
            dgvCustom.Rows.Remove(dgvCustom.SelectedRows[0]);
            btnCustom.Text = "Add";
            btnDelete.Enabled = false;
            propIndex = -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rtbBinaryViewer.Text = DeflateDecompress(""); return;
            byte[] bytes = new byte[] { 104, 222, 98, 98, 100, 16, 96, 98, 96, 154, 207, 196, 192, 152, 197, 196, 192, 112, 17, 72, 247, 0, 249, 189, 76, 12, 191, 10, 129, 108, 111, 128, 0, 3, 0, 62, 36, 4, 210 };
            MyZlib mz = new MyZlib();
            MyZlib.HuffmanTree tree = mz.GenerateHuffmanTree(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' }, new int[] { 3, 3, 3, 3, 3, 2, 4, 4 }, 4);
            string show = "";
            foreach (MyZlib.HuffmanTree.HuffmanTreeBranch htb in tree.Branches)
            {
                show += htb.Symbol + "\t" + htb.Length + "\t" + htb.CodeString + "\n";
            }
            MessageBox.Show(show);
        }

        public string DeflateDecompress(string strSource)
        {
            //byte[] buffer = Convert.FromBase64String(strSource);
            //byte[] buffer = new byte[] { 98, 98, 100, 16, 96, 98, 96, 154, 207, 196, 192, 152, 197, 196, 192, 112, 17, 72, 247, 0, 249, 189, 76, 12, 191, 10, 129, 108, 111, 128, 0, 3, 0, 62, 36, 4, 210 };
            byte[] buffer = new byte[] { 98, 98, 100, 16, 96, 96, 98, 96, 154, 9, 36, 24, 2, 129, 4, 227, 22, 16, 113, 4, 72, 40, 10, 2, 9, 133, 66, 32, 33, 31, 0, 36, 164, 191, 3, 9, 17, 45, 32, 33, 112, 22, 72, 8, 173, 3, 18, 222, 83, 24, 152, 24, 25, 14, 131, 244, 50, 48, 50, 253, 103, 92, 250, 31, 32, 192, 0, 80, 193, 10, 255 };
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                ms.Write(buffer, 0, buffer.Length);
                ms.Position = 0;
                using (System.IO.Compression.DeflateStream stream = new System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Decompress))
                {
                    stream.Flush();
                    int nSize = 16 * 1024 + 256;    //假设字符串不会超过16K
                    byte[] decompressBuffer = new byte[nSize];
                    int nSizeIncept = stream.Read(decompressBuffer, 0, nSize);
                    stream.Close();
                    string output = "";
                    for (int i = 0; i < 32; i++)
                    {
                        output += Convert.ToInt32(decompressBuffer[i]) + "\n";
                    }
                    return output;   //转换为普通的字符串
                }
            }
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

        private void btnTailAdd_Click(object sender, EventArgs e)
        {
            if (btnTailAdd.Text == "Modify")
            {
                dgvTails.Rows[propIndex].Cells[1].Value = rtbTailValue.Text;
            }
            else
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgvTails);
                row.Cells[0].Value = tbTailName.Text;
                row.Cells[1].Value = rtbTailValue.Text;
                dgvTails.Rows.Add(row);
                propIndexTail = row.Index;
                canSelectRowTail = true;
                row.Selected = true;
                btnTailAdd.Text = "Modify";
            }
        }

        private void btnTailDelete_Click(object sender, EventArgs e)
        {
            canSelectRowTail = false;
            dgvTails.Rows.Remove(dgvTails.SelectedRows[0]);
            btnTailAdd.Text = "Add";
            btnTailDelete.Enabled = false;
            propIndexTail = -1;
        }

        private void dgvTails_SelectionChanged(object sender, EventArgs e)
        {
            if (!canSelectRowTail)
            {
                dgvTails.ClearSelection();
            }
            else
            {
                tbTailName.Text = dgvTails.SelectedRows[0].Cells[0].Value.ToString();
                rtbTailValue.Text = dgvTails.SelectedRows[0].Cells[1].Value.ToString();
                btnTailAdd.Enabled = true;
                btnTailDelete.Enabled = true;
            }
        }

        private void dgvTails_MouseClick(object sender, MouseEventArgs e)
        {
            int index = GetRowIndexAt(dgvTails, e.Y);
            bool onHeader = clickOnColumnHeader(dgvTails, e.Y);
            if (index == -1) //没点击某行
            {
                //选空白处时清空触发选定，此处取消选定；选表头时不用做任何修改
                if (!onHeader) //点击空白处
                {
                    canSelectRowTail = false;
                    dgvTails.ClearSelection();
                    tbTailName.Text = "";
                    rtbTailValue.Text = "";
                    btnTailAdd.Enabled = false;
                    btnTailDelete.Enabled = false;
                    noRowSelectedTail = true;
                }
            }
            else
            {
                canSelectRowTail = true;
                noRowSelectedTail = false;
                //currentcell与是否被选中无关！！
                dgvTails.Rows[index].Selected = true; ; //此处与dgvCustom.Rows[index].Selected = true;类似，但是后者方法无法改currentcell
            }
        }

        private void tbTailName_TextChanged(object sender, EventArgs e)
        {
            btnTailAdd.Text = "Add";
            propIndexTail = -1;
            if (tbTailName.Text == "")
            {
                btnTailAdd.Enabled = false;
                btnTailDelete.Enabled = false;
                return;
            }
            foreach (DataGridViewRow row in dgvTails.Rows)
            {
                if (tbTailName.Text == row.Cells[0].Value.ToString())
                {
                    propIndexTail = row.Index;
                    btnTailAdd.Text = "Modify";
                    break;
                }
            }
            if (propIndexTail != -1)
                btnTailDelete.Enabled = true;
            else
                btnTailDelete.Enabled = false;
            if (rtbTailValue.Text == "")
                btnTailAdd.Enabled = false;
            else
                btnTailAdd.Enabled = true;
        }

        private void rtbTailValue_TextChanged(object sender, EventArgs e)
        {
            if (tbTailName.Text == "")
            {
                btnTailAdd.Enabled = false;
                btnTailDelete.Enabled = false;
                return;
            }
            if (propIndexTail != -1)
                btnTailDelete.Enabled = true;
            else
                btnTailDelete.Enabled = false;
            if (rtbTailValue.Text == "")
                btnTailAdd.Enabled = false;
            else
                btnTailAdd.Enabled = true;
        }

        private void dgvTails_Sorted(object sender, EventArgs e)
        {
            if (noRowSelectedTail)
            {
                canSelectRowTail = false;
            }
        }

        private void btnAppendTailNow_Click(object sender, EventArgs e)
        {
            List<PDF.Attr> tails = new List<PDF.Attr>();
            foreach (DataGridViewRow row in dgvTails.Rows)
            {
                tails.Add(new PDF.Attr(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString()));
            }
            string path = tvPDF.SelectedNode.Tag.ToString();
            PDF pdf = new PDF(path);
            pdf.AppendTail(tails);
            MessageBox.Show("Saved successfully！", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


    }
}
