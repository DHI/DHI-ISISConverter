namespace ISISConverterUI
{
    partial class ISISConvertForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ISISConvertForm));
            this.InputDataFileTextBox = new System.Windows.Forms.TextBox();
            this.Browsebutton = new System.Windows.Forms.Button();
            this.Inputlabel = new System.Windows.Forms.Label();
            this.Outputlabel = new System.Windows.Forms.Label();
            this.ConvertButton = new System.Windows.Forms.Button();
            this.outputNameTEXTBox = new System.Windows.Forms.TextBox();
            this.openISISFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.buttonbrowsegxyfile = new System.Windows.Forms.Button();
            this.textBoxgxyfile = new System.Windows.Forms.TextBox();
            this.labelgxyfile = new System.Windows.Forms.Label();
            this.openFileDialoggxyfile = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePickerDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxTimeStep = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonIef = new System.Windows.Forms.Button();
            this.textBoxIeffile = new System.Windows.Forms.TextBox();
            this.openFileDialogIeffile = new System.Windows.Forms.OpenFileDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxDuration = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.buttonFolder = new System.Windows.Forms.Button();
            this.folderBrowserDialogOutput = new System.Windows.Forms.FolderBrowserDialog();
            this.labelProgress = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // InputDataFileTextBox
            // 
            this.InputDataFileTextBox.Location = new System.Drawing.Point(118, 51);
            this.InputDataFileTextBox.Name = "InputDataFileTextBox";
            this.InputDataFileTextBox.Size = new System.Drawing.Size(255, 20);
            this.InputDataFileTextBox.TabIndex = 0;
            this.InputDataFileTextBox.TextChanged += new System.EventHandler(this.InputDataFileTextBox_TextChanged);
            // 
            // Browsebutton
            // 
            this.Browsebutton.Location = new System.Drawing.Point(380, 49);
            this.Browsebutton.Name = "Browsebutton";
            this.Browsebutton.Size = new System.Drawing.Size(61, 23);
            this.Browsebutton.TabIndex = 1;
            this.Browsebutton.Text = "...";
            this.Browsebutton.UseVisualStyleBackColor = true;
            this.Browsebutton.Click += new System.EventHandler(this.Browsebutton_Click);
            // 
            // Inputlabel
            // 
            this.Inputlabel.AutoSize = true;
            this.Inputlabel.Location = new System.Drawing.Point(12, 54);
            this.Inputlabel.Name = "Inputlabel";
            this.Inputlabel.Size = new System.Drawing.Size(72, 13);
            this.Inputlabel.TabIndex = 2;
            this.Inputlabel.Text = "Input *.dat file";
            // 
            // Outputlabel
            // 
            this.Outputlabel.AutoSize = true;
            this.Outputlabel.Location = new System.Drawing.Point(12, 107);
            this.Outputlabel.Name = "Outputlabel";
            this.Outputlabel.Size = new System.Drawing.Size(92, 13);
            this.Outputlabel.TabIndex = 3;
            this.Outputlabel.Text = "Output folder path";
            // 
            // ConvertButton
            // 
            this.ConvertButton.Location = new System.Drawing.Point(380, 194);
            this.ConvertButton.Name = "ConvertButton";
            this.ConvertButton.Size = new System.Drawing.Size(61, 26);
            this.ConvertButton.TabIndex = 4;
            this.ConvertButton.Text = "Convert";
            this.ConvertButton.UseVisualStyleBackColor = true;
            this.ConvertButton.Click += new System.EventHandler(this.ConvertButton_Click);
            // 
            // outputNameTEXTBox
            // 
            this.outputNameTEXTBox.Location = new System.Drawing.Point(118, 107);
            this.outputNameTEXTBox.Name = "outputNameTEXTBox";
            this.outputNameTEXTBox.Size = new System.Drawing.Size(255, 20);
            this.outputNameTEXTBox.TabIndex = 5;
            this.outputNameTEXTBox.TextChanged += new System.EventHandler(this.outputNameTEXTBox_TextChanged);
            // 
            // openISISFileDialog
            // 
            this.openISISFileDialog.FileName = "ISIS file";
            this.openISISFileDialog.Filter = "ISIS data files (*.dat)|*.dat";
            this.openISISFileDialog.FilterIndex = 0;
            this.openISISFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openISISFileDialog_FileOk);
            // 
            // buttonbrowsegxyfile
            // 
            this.buttonbrowsegxyfile.Location = new System.Drawing.Point(380, 21);
            this.buttonbrowsegxyfile.Name = "buttonbrowsegxyfile";
            this.buttonbrowsegxyfile.Size = new System.Drawing.Size(61, 23);
            this.buttonbrowsegxyfile.TabIndex = 6;
            this.buttonbrowsegxyfile.Text = "...";
            this.buttonbrowsegxyfile.UseVisualStyleBackColor = true;
            this.buttonbrowsegxyfile.Click += new System.EventHandler(this.buttonbrowsegxyfile_Click);
            // 
            // textBoxgxyfile
            // 
            this.textBoxgxyfile.Location = new System.Drawing.Point(118, 24);
            this.textBoxgxyfile.Name = "textBoxgxyfile";
            this.textBoxgxyfile.Size = new System.Drawing.Size(255, 20);
            this.textBoxgxyfile.TabIndex = 7;
            this.textBoxgxyfile.TextChanged += new System.EventHandler(this.textBoxgxyfile_TextChanged);
            // 
            // labelgxyfile
            // 
            this.labelgxyfile.AutoSize = true;
            this.labelgxyfile.Location = new System.Drawing.Point(12, 27);
            this.labelgxyfile.Name = "labelgxyfile";
            this.labelgxyfile.Size = new System.Drawing.Size(71, 13);
            this.labelgxyfile.TabIndex = 8;
            this.labelgxyfile.Text = "Input *.*xy file";
            // 
            // openFileDialoggxyfile
            // 
            this.openFileDialoggxyfile.Filter = "ISIS geo files (*.*xy)|*.*xy";
            this.openFileDialoggxyfile.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialoggxyfile_FileOk);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 141);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Simulation start time";
            // 
            // dateTimePickerDate
            // 
            this.dateTimePickerDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerDate.Location = new System.Drawing.Point(118, 141);
            this.dateTimePickerDate.Name = "dateTimePickerDate";
            this.dateTimePickerDate.Size = new System.Drawing.Size(255, 20);
            this.dateTimePickerDate.TabIndex = 10;
            this.dateTimePickerDate.Value = new System.DateTime(2014, 12, 17, 0, 0, 0, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 198);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Simulation time step";
            // 
            // textBoxTimeStep
            // 
            this.textBoxTimeStep.Location = new System.Drawing.Point(118, 198);
            this.textBoxTimeStep.Name = "textBoxTimeStep";
            this.textBoxTimeStep.Size = new System.Drawing.Size(100, 20);
            this.textBoxTimeStep.TabIndex = 12;
            this.textBoxTimeStep.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(218, 201);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "s";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Input *.ief file";
            // 
            // buttonIef
            // 
            this.buttonIef.Location = new System.Drawing.Point(380, 76);
            this.buttonIef.Name = "buttonIef";
            this.buttonIef.Size = new System.Drawing.Size(61, 23);
            this.buttonIef.TabIndex = 16;
            this.buttonIef.Text = "...";
            this.buttonIef.UseVisualStyleBackColor = true;
            this.buttonIef.Click += new System.EventHandler(this.buttonIef_Click);
            // 
            // textBoxIeffile
            // 
            this.textBoxIeffile.Location = new System.Drawing.Point(118, 78);
            this.textBoxIeffile.Name = "textBoxIeffile";
            this.textBoxIeffile.Size = new System.Drawing.Size(255, 20);
            this.textBoxIeffile.TabIndex = 15;
            this.textBoxIeffile.TextChanged += new System.EventHandler(this.textBoxIeffile_TextChanged);
            // 
            // openFileDialogIeffile
            // 
            this.openFileDialogIeffile.FileName = "ISIS file";
            this.openFileDialogIeffile.Filter = "ISIS ief files (*.ief)|*.ief";
            this.openFileDialogIeffile.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialogIeffile_FileOk);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(218, 174);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(13, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "h";
            // 
            // textBoxDuration
            // 
            this.textBoxDuration.Location = new System.Drawing.Point(118, 171);
            this.textBoxDuration.Name = "textBoxDuration";
            this.textBoxDuration.Size = new System.Drawing.Size(100, 20);
            this.textBoxDuration.TabIndex = 19;
            this.textBoxDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 174);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Simulation duration ";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(15, 227);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(358, 23);
            this.progressBar1.TabIndex = 21;
            // 
            // buttonFolder
            // 
            this.buttonFolder.Location = new System.Drawing.Point(380, 105);
            this.buttonFolder.Name = "buttonFolder";
            this.buttonFolder.Size = new System.Drawing.Size(61, 23);
            this.buttonFolder.TabIndex = 22;
            this.buttonFolder.Text = "...";
            this.buttonFolder.UseVisualStyleBackColor = true;
            this.buttonFolder.Click += new System.EventHandler(this.buttonFolder_Click);
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Location = new System.Drawing.Point(394, 237);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(47, 13);
            this.labelProgress.TabIndex = 23;
            this.labelProgress.Text = "0%Done";
            // 
            // ISISConvertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(462, 262);
            this.Controls.Add(this.labelProgress);
            this.Controls.Add(this.buttonFolder);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxDuration);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonIef);
            this.Controls.Add(this.textBoxIeffile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxTimeStep);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePickerDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelgxyfile);
            this.Controls.Add(this.textBoxgxyfile);
            this.Controls.Add(this.buttonbrowsegxyfile);
            this.Controls.Add(this.outputNameTEXTBox);
            this.Controls.Add(this.ConvertButton);
            this.Controls.Add(this.Outputlabel);
            this.Controls.Add(this.Inputlabel);
            this.Controls.Add(this.Browsebutton);
            this.Controls.Add(this.InputDataFileTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ISISConvertForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ISIS to MIKE HYDRO converter";
            this.Load += new System.EventHandler(this.IsisConvertForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox InputDataFileTextBox;
        private System.Windows.Forms.Button Browsebutton;
        private System.Windows.Forms.Label Inputlabel;
        private System.Windows.Forms.Label Outputlabel;
        private System.Windows.Forms.Button ConvertButton;
        private System.Windows.Forms.TextBox outputNameTEXTBox;
        private System.Windows.Forms.OpenFileDialog openISISFileDialog;
        private System.Windows.Forms.Button buttonbrowsegxyfile;
        private System.Windows.Forms.TextBox textBoxgxyfile;
        private System.Windows.Forms.Label labelgxyfile;
        private System.Windows.Forms.OpenFileDialog openFileDialoggxyfile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePickerDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxTimeStep;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonIef;
        private System.Windows.Forms.TextBox textBoxIeffile;
        private System.Windows.Forms.OpenFileDialog openFileDialogIeffile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxDuration;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button buttonFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogOutput;
        private System.Windows.Forms.Label labelProgress;
    }
}

