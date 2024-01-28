
namespace BmpGBDKConverter
{
    partial class frm1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnSelectFile = new Button();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            lblWhiteColor = new Label();
            lblLightGrey = new Label();
            lblDarkGrey = new Label();
            lblBlack = new Label();
            lblColorHeader = new Label();
            openFileDialog = new OpenFileDialog();
            txtFilePath = new TextBox();
            btnSelectOutputFolder = new Button();
            folderBrowserDialog1 = new FolderBrowserDialog();
            txtOutputFile = new TextBox();
            txtOutputName = new TextBox();
            lblOutputName = new Label();
            btnConvert = new Button();
            SuspendLayout();
            // 
            // btnSelectFile
            // 
            btnSelectFile.Location = new Point(480, 33);
            btnSelectFile.Name = "btnSelectFile";
            btnSelectFile.Size = new Size(129, 23);
            btnSelectFile.TabIndex = 0;
            btnSelectFile.Text = "Select File";
            btnSelectFile.UseVisualStyleBackColor = true;
            btnSelectFile.Click += btnSelectFile_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 33);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(12, 62);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 2;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(12, 91);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(100, 23);
            textBox3.TabIndex = 3;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(12, 120);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(100, 23);
            textBox4.TabIndex = 4;
            // 
            // lblWhiteColor
            // 
            lblWhiteColor.AutoSize = true;
            lblWhiteColor.Location = new Point(118, 36);
            lblWhiteColor.Name = "lblWhiteColor";
            lblWhiteColor.Size = new Size(81, 15);
            lblWhiteColor.TabIndex = 5;
            lblWhiteColor.Text = "Lightest Color";
            // 
            // lblLightGrey
            // 
            lblLightGrey.AutoSize = true;
            lblLightGrey.Location = new Point(118, 65);
            lblLightGrey.Name = "lblLightGrey";
            lblLightGrey.Size = new Size(101, 15);
            lblLightGrey.TabIndex = 6;
            lblLightGrey.Text = "Low Middle Color";
            // 
            // lblDarkGrey
            // 
            lblDarkGrey.AutoSize = true;
            lblDarkGrey.Location = new Point(118, 94);
            lblDarkGrey.Name = "lblDarkGrey";
            lblDarkGrey.Size = new Size(103, 15);
            lblDarkGrey.TabIndex = 7;
            lblDarkGrey.Text = "Dark Middle Color";
            // 
            // lblBlack
            // 
            lblBlack.AutoSize = true;
            lblBlack.Location = new Point(118, 123);
            lblBlack.Name = "lblBlack";
            lblBlack.Size = new Size(78, 15);
            lblBlack.TabIndex = 8;
            lblBlack.Text = "Darkest Color";
            // 
            // lblColorHeader
            // 
            lblColorHeader.AutoSize = true;
            lblColorHeader.Location = new Point(12, 9);
            lblColorHeader.Name = "lblColorHeader";
            lblColorHeader.Size = new Size(194, 15);
            lblColorHeader.TabIndex = 9;
            lblColorHeader.Text = "Specify Color Range (format #ffffff)";
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog1";
            // 
            // txtFilePath
            // 
            txtFilePath.Location = new Point(283, 33);
            txtFilePath.Name = "txtFilePath";
            txtFilePath.ReadOnly = true;
            txtFilePath.Size = new Size(177, 23);
            txtFilePath.TabIndex = 10;
            // 
            // btnSelectOutputFolder
            // 
            btnSelectOutputFolder.Location = new Point(480, 61);
            btnSelectOutputFolder.Name = "btnSelectOutputFolder";
            btnSelectOutputFolder.Size = new Size(129, 23);
            btnSelectOutputFolder.TabIndex = 11;
            btnSelectOutputFolder.Text = "Select Output Folder";
            btnSelectOutputFolder.UseVisualStyleBackColor = true;
            // 
            // txtOutputFile
            // 
            txtOutputFile.Location = new Point(283, 62);
            txtOutputFile.Name = "txtOutputFile";
            txtOutputFile.ReadOnly = true;
            txtOutputFile.Size = new Size(177, 23);
            txtOutputFile.TabIndex = 12;
            // 
            // txtOutputName
            // 
            txtOutputName.Location = new Point(283, 91);
            txtOutputName.Name = "txtOutputName";
            txtOutputName.Size = new Size(177, 23);
            txtOutputName.TabIndex = 13;
            // 
            // lblOutputName
            // 
            lblOutputName.AutoSize = true;
            lblOutputName.Location = new Point(480, 94);
            lblOutputName.Name = "lblOutputName";
            lblOutputName.Size = new Size(80, 15);
            lblOutputName.TabIndex = 14;
            lblOutputName.Text = "Output Name";
            // 
            // btnConvert
            // 
            btnConvert.Location = new Point(485, 123);
            btnConvert.Name = "btnConvert";
            btnConvert.Size = new Size(124, 23);
            btnConvert.TabIndex = 15;
            btnConvert.Text = "CONVERT";
            btnConvert.UseVisualStyleBackColor = true;
            btnConvert.Click += btnConvert_Click;
            // 
            // frm1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(656, 196);
            Controls.Add(btnConvert);
            Controls.Add(lblOutputName);
            Controls.Add(txtOutputName);
            Controls.Add(txtOutputFile);
            Controls.Add(btnSelectOutputFolder);
            Controls.Add(txtFilePath);
            Controls.Add(lblColorHeader);
            Controls.Add(lblBlack);
            Controls.Add(lblDarkGrey);
            Controls.Add(lblLightGrey);
            Controls.Add(lblWhiteColor);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(btnSelectFile);
            Name = "frm1";
            Text = "BMP Converter";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSelectFile;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private Label lblWhiteColor;
        private Label lblLightGrey;
        private Label lblDarkGrey;
        private Label lblBlack;
        private Label lblColorHeader;
        private OpenFileDialog openFileDialog;
        private TextBox txtFilePath;
        private Button btnSelectOutputFolder;
        private FolderBrowserDialog folderBrowserDialog1;
        private TextBox txtOutputFile;
        private TextBox txtOutputName;
        private Label lblOutputName;
        private Button btnConvert;
    }
}
