namespace PDFManager_CSharp
{
    partial class PDFManager_CSharp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PDFManager_CSharp));
            this.tvPDF = new System.Windows.Forms.TreeView();
            this.msMenu = new System.Windows.Forms.MenuStrip();
            this.scanPDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnShow = new System.Windows.Forms.Button();
            this.tbFrom = new System.Windows.Forms.TextBox();
            this.tbNum = new System.Windows.Forms.TextBox();
            this.cbObj = new System.Windows.Forms.CheckBox();
            this.btnAppendTailNow = new System.Windows.Forms.Button();
            this.tcFunction = new System.Windows.Forms.TabControl();
            this.tpDescription = new System.Windows.Forms.TabPage();
            this.gpDescription = new System.Windows.Forms.GroupBox();
            this.lbDirectoryName = new System.Windows.Forms.Label();
            this.lbLocation = new System.Windows.Forms.Label();
            this.lbFileName = new System.Windows.Forms.Label();
            this.lbFile = new System.Windows.Forms.Label();
            this.lbVersion = new System.Windows.Forms.Label();
            this.lbProducer = new System.Windows.Forms.Label();
            this.lbCreator = new System.Windows.Forms.Label();
            this.lbModDate = new System.Windows.Forms.Label();
            this.lbCreationDate = new System.Windows.Forms.Label();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.lbPDFVersion = new System.Windows.Forms.Label();
            this.lbTitle = new System.Windows.Forms.Label();
            this.lbApplication = new System.Windows.Forms.Label();
            this.lbAuthor = new System.Windows.Forms.Label();
            this.lbPDFProducer = new System.Windows.Forms.Label();
            this.tbAuthor = new System.Windows.Forms.TextBox();
            this.lbModified = new System.Windows.Forms.Label();
            this.lbSubject = new System.Windows.Forms.Label();
            this.lbCreated = new System.Windows.Forms.Label();
            this.tbSubject = new System.Windows.Forms.TextBox();
            this.tbKeywords = new System.Windows.Forms.TextBox();
            this.lbKeywords = new System.Windows.Forms.Label();
            this.tpCustom = new System.Windows.Forms.TabPage();
            this.gbInfo = new System.Windows.Forms.GroupBox();
            this.pbBulb = new System.Windows.Forms.PictureBox();
            this.lbInfo = new System.Windows.Forms.Label();
            this.gbCustom = new System.Windows.Forms.GroupBox();
            this.dgvCustom = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCustom = new System.Windows.Forms.Button();
            this.rtbValue = new System.Windows.Forms.RichTextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lbValue = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.tpBinaryViewer = new System.Windows.Forms.TabPage();
            this.rtbBinaryViewer = new System.Windows.Forms.RichTextBox();
            this.gbBinaryViewer = new System.Windows.Forms.GroupBox();
            this.cbByte = new System.Windows.Forms.CheckBox();
            this.gbObjects = new System.Windows.Forms.GroupBox();
            this.cbObjNum = new System.Windows.Forms.ComboBox();
            this.lbObjNum = new System.Windows.Forms.Label();
            this.gbParameter = new System.Windows.Forms.GroupBox();
            this.lbLength = new System.Windows.Forms.Label();
            this.lbBeginAt = new System.Windows.Forms.Label();
            this.gbFrom = new System.Windows.Forms.GroupBox();
            this.rbStart = new System.Windows.Forms.RadioButton();
            this.rbEnd = new System.Windows.Forms.RadioButton();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.rtbShow = new System.Windows.Forms.RichTextBox();
            this.tpTail = new System.Windows.Forms.TabPage();
            this.gpTail = new System.Windows.Forms.GroupBox();
            this.dgvTails = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnTailDelete = new System.Windows.Forms.Button();
            this.btnTailAdd = new System.Windows.Forms.Button();
            this.rtbTailValue = new System.Windows.Forms.RichTextBox();
            this.tbTailName = new System.Windows.Forms.TextBox();
            this.lbTailValue = new System.Windows.Forms.Label();
            this.lbTailName = new System.Windows.Forms.Label();
            this.btnFolder = new System.Windows.Forms.Button();
            this.btnFile = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.msMenu.SuspendLayout();
            this.tcFunction.SuspendLayout();
            this.tpDescription.SuspendLayout();
            this.gpDescription.SuspendLayout();
            this.tpCustom.SuspendLayout();
            this.gbInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBulb)).BeginInit();
            this.gbCustom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustom)).BeginInit();
            this.tpBinaryViewer.SuspendLayout();
            this.gbBinaryViewer.SuspendLayout();
            this.gbObjects.SuspendLayout();
            this.gbParameter.SuspendLayout();
            this.gbFrom.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tpTail.SuspendLayout();
            this.gpTail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTails)).BeginInit();
            this.SuspendLayout();
            // 
            // tvPDF
            // 
            this.tvPDF.Location = new System.Drawing.Point(13, 27);
            this.tvPDF.Name = "tvPDF";
            this.tvPDF.Size = new System.Drawing.Size(258, 482);
            this.tvPDF.TabIndex = 0;
            this.tvPDF.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvPDF_AfterSelect);
            // 
            // msMenu
            // 
            this.msMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scanPDFToolStripMenuItem});
            this.msMenu.Location = new System.Drawing.Point(0, 0);
            this.msMenu.Name = "msMenu";
            this.msMenu.Size = new System.Drawing.Size(916, 24);
            this.msMenu.TabIndex = 3;
            this.msMenu.Text = "menuStrip1";
            // 
            // scanPDFToolStripMenuItem
            // 
            this.scanPDFToolStripMenuItem.Name = "scanPDFToolStripMenuItem";
            this.scanPDFToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.scanPDFToolStripMenuItem.Text = "Scan PDF";
            this.scanPDFToolStripMenuItem.Click += new System.EventHandler(this.scanPDFToolStripMenuItem_Click);
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(6, 150);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(217, 48);
            this.btnShow.TabIndex = 4;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // tbFrom
            // 
            this.tbFrom.Location = new System.Drawing.Point(82, 81);
            this.tbFrom.Name = "tbFrom";
            this.tbFrom.Size = new System.Drawing.Size(170, 20);
            this.tbFrom.TabIndex = 6;
            this.tbFrom.Text = "0";
            this.tbFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbNum
            // 
            this.tbNum.Location = new System.Drawing.Point(82, 107);
            this.tbNum.Name = "tbNum";
            this.tbNum.Size = new System.Drawing.Size(170, 20);
            this.tbNum.TabIndex = 22;
            this.tbNum.Text = "200";
            this.tbNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cbObj
            // 
            this.cbObj.AutoSize = true;
            this.cbObj.Location = new System.Drawing.Point(12, 23);
            this.cbObj.Name = "cbObj";
            this.cbObj.Size = new System.Drawing.Size(88, 17);
            this.cbObj.TabIndex = 24;
            this.cbObj.Text = "View Objects";
            this.cbObj.UseVisualStyleBackColor = true;
            // 
            // btnAppendTailNow
            // 
            this.btnAppendTailNow.Location = new System.Drawing.Point(6, 428);
            this.btnAppendTailNow.Name = "btnAppendTailNow";
            this.btnAppendTailNow.Size = new System.Drawing.Size(507, 61);
            this.btnAppendTailNow.TabIndex = 26;
            this.btnAppendTailNow.Text = "Append Tail Now";
            this.btnAppendTailNow.UseVisualStyleBackColor = true;
            this.btnAppendTailNow.Click += new System.EventHandler(this.btnAppendTailNow_Click);
            // 
            // tcFunction
            // 
            this.tcFunction.Controls.Add(this.tpDescription);
            this.tcFunction.Controls.Add(this.tpCustom);
            this.tcFunction.Controls.Add(this.tpBinaryViewer);
            this.tcFunction.Controls.Add(this.tabPage1);
            this.tcFunction.Controls.Add(this.tpTail);
            this.tcFunction.Location = new System.Drawing.Point(376, 29);
            this.tcFunction.Name = "tcFunction";
            this.tcFunction.SelectedIndex = 0;
            this.tcFunction.Size = new System.Drawing.Size(533, 532);
            this.tcFunction.TabIndex = 37;
            // 
            // tpDescription
            // 
            this.tpDescription.Controls.Add(this.gpDescription);
            this.tpDescription.Location = new System.Drawing.Point(4, 22);
            this.tpDescription.Name = "tpDescription";
            this.tpDescription.Padding = new System.Windows.Forms.Padding(3);
            this.tpDescription.Size = new System.Drawing.Size(525, 506);
            this.tpDescription.TabIndex = 0;
            this.tpDescription.Text = "Description";
            this.tpDescription.UseVisualStyleBackColor = true;
            // 
            // gpDescription
            // 
            this.gpDescription.Controls.Add(this.lbDirectoryName);
            this.gpDescription.Controls.Add(this.lbLocation);
            this.gpDescription.Controls.Add(this.lbFileName);
            this.gpDescription.Controls.Add(this.lbFile);
            this.gpDescription.Controls.Add(this.lbVersion);
            this.gpDescription.Controls.Add(this.lbProducer);
            this.gpDescription.Controls.Add(this.lbCreator);
            this.gpDescription.Controls.Add(this.lbModDate);
            this.gpDescription.Controls.Add(this.lbCreationDate);
            this.gpDescription.Controls.Add(this.tbTitle);
            this.gpDescription.Controls.Add(this.lbPDFVersion);
            this.gpDescription.Controls.Add(this.lbTitle);
            this.gpDescription.Controls.Add(this.lbApplication);
            this.gpDescription.Controls.Add(this.lbAuthor);
            this.gpDescription.Controls.Add(this.lbPDFProducer);
            this.gpDescription.Controls.Add(this.tbAuthor);
            this.gpDescription.Controls.Add(this.lbModified);
            this.gpDescription.Controls.Add(this.lbSubject);
            this.gpDescription.Controls.Add(this.lbCreated);
            this.gpDescription.Controls.Add(this.tbSubject);
            this.gpDescription.Controls.Add(this.tbKeywords);
            this.gpDescription.Controls.Add(this.lbKeywords);
            this.gpDescription.Location = new System.Drawing.Point(6, 6);
            this.gpDescription.Name = "gpDescription";
            this.gpDescription.Size = new System.Drawing.Size(411, 494);
            this.gpDescription.TabIndex = 29;
            this.gpDescription.TabStop = false;
            this.gpDescription.Text = "Description";
            // 
            // lbDirectoryName
            // 
            this.lbDirectoryName.AutoSize = true;
            this.lbDirectoryName.Location = new System.Drawing.Point(92, 278);
            this.lbDirectoryName.Name = "lbDirectoryName";
            this.lbDirectoryName.Size = new System.Drawing.Size(80, 13);
            this.lbDirectoryName.TabIndex = 34;
            this.lbDirectoryName.Text = "Directory Name";
            // 
            // lbLocation
            // 
            this.lbLocation.AutoSize = true;
            this.lbLocation.Location = new System.Drawing.Point(35, 278);
            this.lbLocation.Name = "lbLocation";
            this.lbLocation.Size = new System.Drawing.Size(51, 13);
            this.lbLocation.TabIndex = 33;
            this.lbLocation.Text = "Location:";
            // 
            // lbFileName
            // 
            this.lbFileName.AutoSize = true;
            this.lbFileName.Location = new System.Drawing.Point(93, 254);
            this.lbFileName.Name = "lbFileName";
            this.lbFileName.Size = new System.Drawing.Size(54, 13);
            this.lbFileName.TabIndex = 32;
            this.lbFileName.Text = "File Name";
            // 
            // lbFile
            // 
            this.lbFile.AutoSize = true;
            this.lbFile.Location = new System.Drawing.Point(61, 254);
            this.lbFile.Name = "lbFile";
            this.lbFile.Size = new System.Drawing.Size(26, 13);
            this.lbFile.TabIndex = 31;
            this.lbFile.Text = "File:";
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.Location = new System.Drawing.Point(93, 230);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(42, 13);
            this.lbVersion.TabIndex = 30;
            this.lbVersion.Text = "Version";
            // 
            // lbProducer
            // 
            this.lbProducer.AutoSize = true;
            this.lbProducer.Location = new System.Drawing.Point(93, 204);
            this.lbProducer.Name = "lbProducer";
            this.lbProducer.Size = new System.Drawing.Size(50, 13);
            this.lbProducer.TabIndex = 28;
            this.lbProducer.Text = "Producer";
            // 
            // lbCreator
            // 
            this.lbCreator.AutoSize = true;
            this.lbCreator.Location = new System.Drawing.Point(93, 182);
            this.lbCreator.Name = "lbCreator";
            this.lbCreator.Size = new System.Drawing.Size(41, 13);
            this.lbCreator.TabIndex = 27;
            this.lbCreator.Text = "Creator";
            // 
            // lbModDate
            // 
            this.lbModDate.AutoSize = true;
            this.lbModDate.Location = new System.Drawing.Point(93, 159);
            this.lbModDate.Name = "lbModDate";
            this.lbModDate.Size = new System.Drawing.Size(90, 13);
            this.lbModDate.TabIndex = 24;
            this.lbModDate.Text = "Modification Date";
            // 
            // lbCreationDate
            // 
            this.lbCreationDate.AutoSize = true;
            this.lbCreationDate.Location = new System.Drawing.Point(93, 133);
            this.lbCreationDate.Name = "lbCreationDate";
            this.lbCreationDate.Size = new System.Drawing.Size(72, 13);
            this.lbCreationDate.TabIndex = 21;
            this.lbCreationDate.Text = "Creation Date";
            // 
            // tbTitle
            // 
            this.tbTitle.Location = new System.Drawing.Point(96, 26);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(300, 20);
            this.tbTitle.TabIndex = 9;
            // 
            // lbPDFVersion
            // 
            this.lbPDFVersion.AutoSize = true;
            this.lbPDFVersion.Location = new System.Drawing.Point(19, 230);
            this.lbPDFVersion.Name = "lbPDFVersion";
            this.lbPDFVersion.Size = new System.Drawing.Size(69, 13);
            this.lbPDFVersion.TabIndex = 20;
            this.lbPDFVersion.Text = "PDF Version:";
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Location = new System.Drawing.Point(58, 29);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(30, 13);
            this.lbTitle.TabIndex = 8;
            this.lbTitle.Text = "Title:";
            // 
            // lbApplication
            // 
            this.lbApplication.AutoSize = true;
            this.lbApplication.Location = new System.Drawing.Point(26, 182);
            this.lbApplication.Name = "lbApplication";
            this.lbApplication.Size = new System.Drawing.Size(62, 13);
            this.lbApplication.TabIndex = 19;
            this.lbApplication.Text = "Application:";
            // 
            // lbAuthor
            // 
            this.lbAuthor.AutoSize = true;
            this.lbAuthor.Location = new System.Drawing.Point(47, 55);
            this.lbAuthor.Name = "lbAuthor";
            this.lbAuthor.Size = new System.Drawing.Size(41, 13);
            this.lbAuthor.TabIndex = 10;
            this.lbAuthor.Text = "Author:";
            // 
            // lbPDFProducer
            // 
            this.lbPDFProducer.AutoSize = true;
            this.lbPDFProducer.Location = new System.Drawing.Point(11, 204);
            this.lbPDFProducer.Name = "lbPDFProducer";
            this.lbPDFProducer.Size = new System.Drawing.Size(77, 13);
            this.lbPDFProducer.TabIndex = 18;
            this.lbPDFProducer.Text = "PDF Producer:";
            // 
            // tbAuthor
            // 
            this.tbAuthor.Location = new System.Drawing.Point(96, 52);
            this.tbAuthor.Name = "tbAuthor";
            this.tbAuthor.Size = new System.Drawing.Size(300, 20);
            this.tbAuthor.TabIndex = 11;
            // 
            // lbModified
            // 
            this.lbModified.AutoSize = true;
            this.lbModified.Location = new System.Drawing.Point(38, 159);
            this.lbModified.Name = "lbModified";
            this.lbModified.Size = new System.Drawing.Size(50, 13);
            this.lbModified.TabIndex = 17;
            this.lbModified.Text = "Modified:";
            // 
            // lbSubject
            // 
            this.lbSubject.AutoSize = true;
            this.lbSubject.Location = new System.Drawing.Point(42, 81);
            this.lbSubject.Name = "lbSubject";
            this.lbSubject.Size = new System.Drawing.Size(46, 13);
            this.lbSubject.TabIndex = 12;
            this.lbSubject.Text = "Subject:";
            // 
            // lbCreated
            // 
            this.lbCreated.AutoSize = true;
            this.lbCreated.Location = new System.Drawing.Point(41, 133);
            this.lbCreated.Name = "lbCreated";
            this.lbCreated.Size = new System.Drawing.Size(47, 13);
            this.lbCreated.TabIndex = 16;
            this.lbCreated.Text = "Created:";
            // 
            // tbSubject
            // 
            this.tbSubject.Location = new System.Drawing.Point(96, 78);
            this.tbSubject.Name = "tbSubject";
            this.tbSubject.Size = new System.Drawing.Size(300, 20);
            this.tbSubject.TabIndex = 13;
            // 
            // tbKeywords
            // 
            this.tbKeywords.Location = new System.Drawing.Point(96, 104);
            this.tbKeywords.Name = "tbKeywords";
            this.tbKeywords.Size = new System.Drawing.Size(300, 20);
            this.tbKeywords.TabIndex = 15;
            // 
            // lbKeywords
            // 
            this.lbKeywords.AutoSize = true;
            this.lbKeywords.Location = new System.Drawing.Point(32, 107);
            this.lbKeywords.Name = "lbKeywords";
            this.lbKeywords.Size = new System.Drawing.Size(56, 13);
            this.lbKeywords.TabIndex = 14;
            this.lbKeywords.Text = "Keywords:";
            // 
            // tpCustom
            // 
            this.tpCustom.Controls.Add(this.gbInfo);
            this.tpCustom.Controls.Add(this.gbCustom);
            this.tpCustom.Location = new System.Drawing.Point(4, 22);
            this.tpCustom.Name = "tpCustom";
            this.tpCustom.Padding = new System.Windows.Forms.Padding(3);
            this.tpCustom.Size = new System.Drawing.Size(525, 506);
            this.tpCustom.TabIndex = 2;
            this.tpCustom.Text = "Custom";
            this.tpCustom.UseVisualStyleBackColor = true;
            // 
            // gbInfo
            // 
            this.gbInfo.Controls.Add(this.pbBulb);
            this.gbInfo.Controls.Add(this.lbInfo);
            this.gbInfo.Location = new System.Drawing.Point(6, 386);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Size = new System.Drawing.Size(513, 94);
            this.gbInfo.TabIndex = 7;
            this.gbInfo.TabStop = false;
            // 
            // pbBulb
            // 
            this.pbBulb.Image = ((System.Drawing.Image)(resources.GetObject("pbBulb.Image")));
            this.pbBulb.Location = new System.Drawing.Point(25, 33);
            this.pbBulb.Name = "pbBulb";
            this.pbBulb.Size = new System.Drawing.Size(30, 38);
            this.pbBulb.TabIndex = 1;
            this.pbBulb.TabStop = false;
            // 
            // lbInfo
            // 
            this.lbInfo.AutoSize = true;
            this.lbInfo.Location = new System.Drawing.Point(76, 32);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(396, 39);
            this.lbInfo.TabIndex = 0;
            this.lbInfo.Text = resources.GetString("lbInfo.Text");
            // 
            // gbCustom
            // 
            this.gbCustom.Controls.Add(this.dgvCustom);
            this.gbCustom.Controls.Add(this.btnDelete);
            this.gbCustom.Controls.Add(this.btnCustom);
            this.gbCustom.Controls.Add(this.rtbValue);
            this.gbCustom.Controls.Add(this.tbName);
            this.gbCustom.Controls.Add(this.lbValue);
            this.gbCustom.Controls.Add(this.lbName);
            this.gbCustom.Location = new System.Drawing.Point(6, 6);
            this.gbCustom.Name = "gbCustom";
            this.gbCustom.Size = new System.Drawing.Size(513, 359);
            this.gbCustom.TabIndex = 2;
            this.gbCustom.TabStop = false;
            this.gbCustom.Text = "Custom Properties";
            // 
            // dgvCustom
            // 
            this.dgvCustom.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colValue});
            this.dgvCustom.Location = new System.Drawing.Point(7, 155);
            this.dgvCustom.Name = "dgvCustom";
            this.dgvCustom.Size = new System.Drawing.Size(500, 196);
            this.dgvCustom.TabIndex = 6;
            this.dgvCustom.SelectionChanged += new System.EventHandler(this.dgvCustom_SelectionChanged);
            this.dgvCustom.Sorted += new System.EventHandler(this.dgvCustom_Sorted);
            this.dgvCustom.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvCustom_MouseClick);
            // 
            // colName
            // 
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 150;
            // 
            // colValue
            // 
            this.colValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colValue.HeaderText = "Value";
            this.colValue.Name = "colValue";
            this.colValue.ReadOnly = true;
            this.colValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(406, 89);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(101, 60);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCustom
            // 
            this.btnCustom.Location = new System.Drawing.Point(406, 26);
            this.btnCustom.Name = "btnCustom";
            this.btnCustom.Size = new System.Drawing.Size(101, 60);
            this.btnCustom.TabIndex = 4;
            this.btnCustom.Text = "Add";
            this.btnCustom.UseVisualStyleBackColor = true;
            this.btnCustom.Click += new System.EventHandler(this.btnCustom_Click);
            // 
            // rtbValue
            // 
            this.rtbValue.Location = new System.Drawing.Point(51, 53);
            this.rtbValue.Name = "rtbValue";
            this.rtbValue.Size = new System.Drawing.Size(349, 96);
            this.rtbValue.TabIndex = 3;
            this.rtbValue.Text = "";
            this.rtbValue.TextChanged += new System.EventHandler(this.rtbValue_TextChanged);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(51, 26);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(349, 20);
            this.tbName.TabIndex = 2;
            this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
            // 
            // lbValue
            // 
            this.lbValue.AutoSize = true;
            this.lbValue.Location = new System.Drawing.Point(6, 52);
            this.lbValue.Name = "lbValue";
            this.lbValue.Size = new System.Drawing.Size(37, 13);
            this.lbValue.TabIndex = 1;
            this.lbValue.Text = "Value:";
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(6, 26);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(38, 13);
            this.lbName.TabIndex = 0;
            this.lbName.Text = "Name:";
            // 
            // tpBinaryViewer
            // 
            this.tpBinaryViewer.Controls.Add(this.rtbBinaryViewer);
            this.tpBinaryViewer.Controls.Add(this.gbBinaryViewer);
            this.tpBinaryViewer.Location = new System.Drawing.Point(4, 22);
            this.tpBinaryViewer.Name = "tpBinaryViewer";
            this.tpBinaryViewer.Padding = new System.Windows.Forms.Padding(3);
            this.tpBinaryViewer.Size = new System.Drawing.Size(525, 506);
            this.tpBinaryViewer.TabIndex = 3;
            this.tpBinaryViewer.Text = "Binary Viewer";
            this.tpBinaryViewer.UseVisualStyleBackColor = true;
            // 
            // rtbBinaryViewer
            // 
            this.rtbBinaryViewer.Location = new System.Drawing.Point(6, 6);
            this.rtbBinaryViewer.Name = "rtbBinaryViewer";
            this.rtbBinaryViewer.Size = new System.Drawing.Size(507, 268);
            this.rtbBinaryViewer.TabIndex = 42;
            this.rtbBinaryViewer.Text = "";
            // 
            // gbBinaryViewer
            // 
            this.gbBinaryViewer.Controls.Add(this.cbByte);
            this.gbBinaryViewer.Controls.Add(this.gbObjects);
            this.gbBinaryViewer.Controls.Add(this.gbParameter);
            this.gbBinaryViewer.Controls.Add(this.btnShow);
            this.gbBinaryViewer.Location = new System.Drawing.Point(6, 280);
            this.gbBinaryViewer.Name = "gbBinaryViewer";
            this.gbBinaryViewer.Size = new System.Drawing.Size(513, 220);
            this.gbBinaryViewer.TabIndex = 41;
            this.gbBinaryViewer.TabStop = false;
            this.gbBinaryViewer.Text = "Binary Viewer";
            // 
            // cbByte
            // 
            this.cbByte.AutoSize = true;
            this.cbByte.Location = new System.Drawing.Point(18, 28);
            this.cbByte.Name = "cbByte";
            this.cbByte.Size = new System.Drawing.Size(82, 17);
            this.cbByte.TabIndex = 48;
            this.cbByte.Text = "Show Bytes";
            this.cbByte.UseVisualStyleBackColor = true;
            // 
            // gbObjects
            // 
            this.gbObjects.Controls.Add(this.cbObjNum);
            this.gbObjects.Controls.Add(this.lbObjNum);
            this.gbObjects.Controls.Add(this.cbObj);
            this.gbObjects.Location = new System.Drawing.Point(7, 60);
            this.gbObjects.Name = "gbObjects";
            this.gbObjects.Size = new System.Drawing.Size(217, 84);
            this.gbObjects.TabIndex = 47;
            this.gbObjects.TabStop = false;
            this.gbObjects.Text = "Objects";
            // 
            // cbObjNum
            // 
            this.cbObjNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObjNum.FormattingEnabled = true;
            this.cbObjNum.Location = new System.Drawing.Point(97, 48);
            this.cbObjNum.Name = "cbObjNum";
            this.cbObjNum.Size = new System.Drawing.Size(100, 21);
            this.cbObjNum.TabIndex = 45;
            this.cbObjNum.SelectedIndexChanged += new System.EventHandler(this.cbObjNum_SelectedIndexChanged);
            // 
            // lbObjNum
            // 
            this.lbObjNum.AutoSize = true;
            this.lbObjNum.Location = new System.Drawing.Point(11, 51);
            this.lbObjNum.Name = "lbObjNum";
            this.lbObjNum.Size = new System.Drawing.Size(81, 13);
            this.lbObjNum.TabIndex = 46;
            this.lbObjNum.Text = "Object Number:";
            // 
            // gbParameter
            // 
            this.gbParameter.Controls.Add(this.lbLength);
            this.gbParameter.Controls.Add(this.tbFrom);
            this.gbParameter.Controls.Add(this.lbBeginAt);
            this.gbParameter.Controls.Add(this.tbNum);
            this.gbParameter.Controls.Add(this.gbFrom);
            this.gbParameter.Location = new System.Drawing.Point(230, 60);
            this.gbParameter.Name = "gbParameter";
            this.gbParameter.Size = new System.Drawing.Size(277, 141);
            this.gbParameter.TabIndex = 44;
            this.gbParameter.TabStop = false;
            this.gbParameter.Text = "Parameter:";
            // 
            // lbLength
            // 
            this.lbLength.AutoSize = true;
            this.lbLength.Location = new System.Drawing.Point(11, 110);
            this.lbLength.Name = "lbLength";
            this.lbLength.Size = new System.Drawing.Size(43, 13);
            this.lbLength.TabIndex = 43;
            this.lbLength.Text = "Length:";
            // 
            // lbBeginAt
            // 
            this.lbBeginAt.AutoSize = true;
            this.lbBeginAt.Location = new System.Drawing.Point(11, 84);
            this.lbBeginAt.Name = "lbBeginAt";
            this.lbBeginAt.Size = new System.Drawing.Size(50, 13);
            this.lbBeginAt.TabIndex = 42;
            this.lbBeginAt.Text = "Begin At:";
            // 
            // gbFrom
            // 
            this.gbFrom.Controls.Add(this.rbStart);
            this.gbFrom.Controls.Add(this.rbEnd);
            this.gbFrom.Location = new System.Drawing.Point(28, 19);
            this.gbFrom.Name = "gbFrom";
            this.gbFrom.Size = new System.Drawing.Size(204, 50);
            this.gbFrom.TabIndex = 41;
            this.gbFrom.TabStop = false;
            this.gbFrom.Text = "From:";
            // 
            // rbStart
            // 
            this.rbStart.AutoSize = true;
            this.rbStart.Checked = true;
            this.rbStart.Location = new System.Drawing.Point(40, 20);
            this.rbStart.Name = "rbStart";
            this.rbStart.Size = new System.Drawing.Size(47, 17);
            this.rbStart.TabIndex = 39;
            this.rbStart.TabStop = true;
            this.rbStart.Text = "Start";
            this.rbStart.UseVisualStyleBackColor = true;
            // 
            // rbEnd
            // 
            this.rbEnd.AutoSize = true;
            this.rbEnd.Location = new System.Drawing.Point(117, 20);
            this.rbEnd.Name = "rbEnd";
            this.rbEnd.Size = new System.Drawing.Size(44, 17);
            this.rbEnd.TabIndex = 40;
            this.rbEnd.Text = "End";
            this.rbEnd.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.rtbShow);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(525, 506);
            this.tabPage1.TabIndex = 4;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // rtbShow
            // 
            this.rtbShow.Location = new System.Drawing.Point(6, 6);
            this.rtbShow.Name = "rtbShow";
            this.rtbShow.Size = new System.Drawing.Size(513, 494);
            this.rtbShow.TabIndex = 26;
            this.rtbShow.Text = "";
            // 
            // tpTail
            // 
            this.tpTail.Controls.Add(this.gpTail);
            this.tpTail.Controls.Add(this.btnAppendTailNow);
            this.tpTail.Location = new System.Drawing.Point(4, 22);
            this.tpTail.Name = "tpTail";
            this.tpTail.Padding = new System.Windows.Forms.Padding(3);
            this.tpTail.Size = new System.Drawing.Size(525, 506);
            this.tpTail.TabIndex = 5;
            this.tpTail.Text = "Tail";
            this.tpTail.UseVisualStyleBackColor = true;
            // 
            // gpTail
            // 
            this.gpTail.Controls.Add(this.dgvTails);
            this.gpTail.Controls.Add(this.btnTailDelete);
            this.gpTail.Controls.Add(this.btnTailAdd);
            this.gpTail.Controls.Add(this.rtbTailValue);
            this.gpTail.Controls.Add(this.tbTailName);
            this.gpTail.Controls.Add(this.lbTailValue);
            this.gpTail.Controls.Add(this.lbTailName);
            this.gpTail.Location = new System.Drawing.Point(6, 7);
            this.gpTail.Name = "gpTail";
            this.gpTail.Size = new System.Drawing.Size(513, 415);
            this.gpTail.TabIndex = 8;
            this.gpTail.TabStop = false;
            this.gpTail.Text = "Custom Properties";
            // 
            // dgvTails
            // 
            this.dgvTails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dgvTails.Location = new System.Drawing.Point(7, 155);
            this.dgvTails.Name = "dgvTails";
            this.dgvTails.Size = new System.Drawing.Size(500, 254);
            this.dgvTails.TabIndex = 6;
            this.dgvTails.SelectionChanged += new System.EventHandler(this.dgvTails_SelectionChanged);
            this.dgvTails.Sorted += new System.EventHandler(this.dgvTails_Sorted);
            this.dgvTails.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvTails_MouseClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 150;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Value";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // btnTailDelete
            // 
            this.btnTailDelete.Location = new System.Drawing.Point(406, 89);
            this.btnTailDelete.Name = "btnTailDelete";
            this.btnTailDelete.Size = new System.Drawing.Size(101, 60);
            this.btnTailDelete.TabIndex = 5;
            this.btnTailDelete.Text = "Delete";
            this.btnTailDelete.UseVisualStyleBackColor = true;
            this.btnTailDelete.Click += new System.EventHandler(this.btnTailDelete_Click);
            // 
            // btnTailAdd
            // 
            this.btnTailAdd.Location = new System.Drawing.Point(406, 26);
            this.btnTailAdd.Name = "btnTailAdd";
            this.btnTailAdd.Size = new System.Drawing.Size(101, 60);
            this.btnTailAdd.TabIndex = 4;
            this.btnTailAdd.Text = "Add";
            this.btnTailAdd.UseVisualStyleBackColor = true;
            this.btnTailAdd.Click += new System.EventHandler(this.btnTailAdd_Click);
            // 
            // rtbTailValue
            // 
            this.rtbTailValue.Location = new System.Drawing.Point(51, 53);
            this.rtbTailValue.Name = "rtbTailValue";
            this.rtbTailValue.Size = new System.Drawing.Size(349, 96);
            this.rtbTailValue.TabIndex = 3;
            this.rtbTailValue.Text = "";
            this.rtbTailValue.TextChanged += new System.EventHandler(this.rtbTailValue_TextChanged);
            // 
            // tbTailName
            // 
            this.tbTailName.Location = new System.Drawing.Point(51, 26);
            this.tbTailName.Name = "tbTailName";
            this.tbTailName.Size = new System.Drawing.Size(349, 20);
            this.tbTailName.TabIndex = 2;
            this.tbTailName.TextChanged += new System.EventHandler(this.tbTailName_TextChanged);
            // 
            // lbTailValue
            // 
            this.lbTailValue.AutoSize = true;
            this.lbTailValue.Location = new System.Drawing.Point(6, 52);
            this.lbTailValue.Name = "lbTailValue";
            this.lbTailValue.Size = new System.Drawing.Size(37, 13);
            this.lbTailValue.TabIndex = 1;
            this.lbTailValue.Text = "Value:";
            // 
            // lbTailName
            // 
            this.lbTailName.AutoSize = true;
            this.lbTailName.Location = new System.Drawing.Point(6, 26);
            this.lbTailName.Name = "lbTailName";
            this.lbTailName.Size = new System.Drawing.Size(38, 13);
            this.lbTailName.TabIndex = 0;
            this.lbTailName.Text = "Name:";
            // 
            // btnFolder
            // 
            this.btnFolder.Location = new System.Drawing.Point(145, 515);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(126, 46);
            this.btnFolder.TabIndex = 36;
            this.btnFolder.Text = "Browse Folder";
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(13, 515);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(125, 46);
            this.btnFile.TabIndex = 35;
            this.btnFile.Text = "View File";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(282, 299);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(81, 262);
            this.btnSave.TabIndex = 39;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(282, 27);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(81, 266);
            this.btnRefresh.TabIndex = 38;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // PDFManager_CSharp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 573);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.tcFunction);
            this.Controls.Add(this.btnFolder);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.tvPDF);
            this.Controls.Add(this.msMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.msMenu;
            this.Name = "PDFManager_CSharp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PDFManager_CSharp";
            this.Load += new System.EventHandler(this.PDFManager_CSharp_Load);
            this.msMenu.ResumeLayout(false);
            this.msMenu.PerformLayout();
            this.tcFunction.ResumeLayout(false);
            this.tpDescription.ResumeLayout(false);
            this.gpDescription.ResumeLayout(false);
            this.gpDescription.PerformLayout();
            this.tpCustom.ResumeLayout(false);
            this.gbInfo.ResumeLayout(false);
            this.gbInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBulb)).EndInit();
            this.gbCustom.ResumeLayout(false);
            this.gbCustom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustom)).EndInit();
            this.tpBinaryViewer.ResumeLayout(false);
            this.gbBinaryViewer.ResumeLayout(false);
            this.gbBinaryViewer.PerformLayout();
            this.gbObjects.ResumeLayout(false);
            this.gbObjects.PerformLayout();
            this.gbParameter.ResumeLayout(false);
            this.gbParameter.PerformLayout();
            this.gbFrom.ResumeLayout(false);
            this.gbFrom.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tpTail.ResumeLayout(false);
            this.gpTail.ResumeLayout(false);
            this.gpTail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTails)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvPDF;
        private System.Windows.Forms.MenuStrip msMenu;
        private System.Windows.Forms.ToolStripMenuItem scanPDFToolStripMenuItem;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.TextBox tbFrom;
        private System.Windows.Forms.TextBox tbNum;
        private System.Windows.Forms.CheckBox cbObj;
        private System.Windows.Forms.Button btnAppendTailNow;
        private System.Windows.Forms.TabControl tcFunction;
        private System.Windows.Forms.TabPage tpDescription;
        private System.Windows.Forms.GroupBox gpDescription;
        private System.Windows.Forms.Button btnFolder;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.Label lbDirectoryName;
        private System.Windows.Forms.Label lbLocation;
        private System.Windows.Forms.Label lbFileName;
        private System.Windows.Forms.Label lbFile;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.Label lbProducer;
        private System.Windows.Forms.Label lbCreator;
        private System.Windows.Forms.Label lbModDate;
        private System.Windows.Forms.Label lbCreationDate;
        private System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.Label lbPDFVersion;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Label lbApplication;
        private System.Windows.Forms.Label lbAuthor;
        private System.Windows.Forms.Label lbPDFProducer;
        private System.Windows.Forms.TextBox tbAuthor;
        private System.Windows.Forms.Label lbModified;
        private System.Windows.Forms.Label lbSubject;
        private System.Windows.Forms.Label lbCreated;
        private System.Windows.Forms.TextBox tbSubject;
        private System.Windows.Forms.TextBox tbKeywords;
        private System.Windows.Forms.Label lbKeywords;
        private System.Windows.Forms.RichTextBox rtbShow;
        private System.Windows.Forms.RadioButton rbStart;
        private System.Windows.Forms.RadioButton rbEnd;
        private System.Windows.Forms.GroupBox gbBinaryViewer;
        private System.Windows.Forms.Label lbLength;
        private System.Windows.Forms.Label lbBeginAt;
        private System.Windows.Forms.GroupBox gbFrom;
        private System.Windows.Forms.TabPage tpCustom;
        private System.Windows.Forms.GroupBox gbCustom;
        private System.Windows.Forms.DataGridView dgvCustom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCustom;
        private System.Windows.Forms.RichTextBox rtbValue;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lbValue;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.GroupBox gbInfo;
        private System.Windows.Forms.PictureBox pbBulb;
        private System.Windows.Forms.Label lbInfo;
        private System.Windows.Forms.TabPage tpBinaryViewer;
        private System.Windows.Forms.RichTextBox rtbBinaryViewer;
        private System.Windows.Forms.GroupBox gbParameter;
        private System.Windows.Forms.GroupBox gbObjects;
        private System.Windows.Forms.Label lbObjNum;
        private System.Windows.Forms.ComboBox cbObjNum;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.CheckBox cbByte;
        private System.Windows.Forms.TabPage tpTail;
        private System.Windows.Forms.GroupBox gpTail;
        private System.Windows.Forms.DataGridView dgvTails;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.Button btnTailDelete;
        private System.Windows.Forms.Button btnTailAdd;
        private System.Windows.Forms.RichTextBox rtbTailValue;
        private System.Windows.Forms.TextBox tbTailName;
        private System.Windows.Forms.Label lbTailValue;
        private System.Windows.Forms.Label lbTailName;
    }
}