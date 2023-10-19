using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using System.Reflection;
using ISISConverterEngine;

namespace ISISConverterUI
{
    public partial class ISISConvertForm : Form
    {
        string IsisDataFileName;
        string IsisgeometryFileName;
        string IsisIefFileName;
        string Outputdirectory;
        public IsisDataClass IsisData;
        string xsecXNS11outputfile;
        string logfile;

        public string OutPutDirectory
        {
            get
            {
                return Outputdirectory;
            }
        }


        public ISISConvertForm()
        {
            InitializeComponent();
            this.progressBar1.Maximum = 100;
            this.progressBar1.Value = 0;
            Outputdirectory = this.outputNameTEXTBox.Text;
        }

        private void openISISFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            IsisDataFileName = openISISFileDialog.FileName;
            InputDataFileTextBox.Text = IsisDataFileName;

            Outputdirectory = System.IO.Path.GetDirectoryName(IsisDataFileName) + "\\MIKE_HYDRO_model";
            outputNameTEXTBox.Text = Outputdirectory;
        }

        private void Browsebutton_Click(object sender, EventArgs e)
        {
            openISISFileDialog.ShowDialog();
        }

        delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[] { oControl, propName, propValue });
            }
            else
            {
                Type t = oControl.GetType();
                PropertyInfo[] props = t.GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if (p.Name.ToUpper() == propName.ToUpper())
                    {
                        p.SetValue(oControl, propValue, null);
                    }
                }
            }
        }

        private void DoConvert(object o)
        {
            ISISConvertForm form = o as ISISConvertForm;
            //validate the input files
            if (!System.IO.File.Exists(form.IsisgeometryFileName))
            {
                MessageBox.Show("Input geometry file location not valid. Please check file name and location.");
                return;
            }

            if (!System.IO.File.Exists(form.IsisDataFileName))
            {
                MessageBox.Show("Input data file location not valid. Please check file name and location.");
                return;
            }

            if (!string.IsNullOrEmpty(form.outputNameTEXTBox.Text))
            {
                {
                    string test = form.Outputdirectory;

                    try
                    {
                        System.IO.Directory.CreateDirectory(test);
                        form.Outputdirectory = test;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("The output name contains invalid characters. Please reconsider the output name.");
                        throw;
                    }
                }
            }
            else
            {
                MessageBox.Show("The output folder name empty. Please select output name folder.");
            }

            SetControlPropertyValue(form.progressBar1, "value", 2);
            SetControlPropertyValue(form.labelProgress, "TEXT", "2%Done");

            try
            {
                IsisDataClass IsisData = new IsisDataClass();
                IsisData.SetStatus = new Action<int, string>((progressVal, statusText) =>
                {
                    SetControlPropertyValue(form.progressBar1, "value", progressVal);
                    if (string.IsNullOrEmpty(statusText))
                        statusText = $"{progressVal}%Done";
                    SetControlPropertyValue(form.labelProgress, "TEXT", statusText);
                });
                //simulation period and time step
                IsisDataClass.datetimeStart = form.dateTimePickerDate.Value;
                if (string.IsNullOrEmpty(form.textBoxDuration.Text))
                {

                }
                else
                {
                    IsisDataClass.duration = double.Parse(form.textBoxDuration.Text);
                }
                if (string.IsNullOrEmpty(form.textBoxTimeStep.Text))
                {

                }
                else
                {
                    IsisData.timestep = double.Parse(form.textBoxTimeStep.Text);
                }
                //simulation period and time step
                if (System.IO.File.Exists(form.IsisgeometryFileName))
                {
                    IsisData.ReadGeometryFile(form.IsisgeometryFileName);
                }
                else if (form.IsisgeometryFileName != null)
                {
                    MessageBox.Show("Input geometry file location not valid. Please check file name and location.");
                }
                else
                {
                    IsisData.HydraulicElementsCollection = new HydraulicElementGeoCollectionClass();
                }

                bool readRes = false;
                string errMsg = string.Empty;
                if (System.IO.File.Exists(form.IsisDataFileName))
                {
                    try
                    {
                        readRes = IsisData.ReadDataFile(form.IsisDataFileName, out errMsg);
                        if (!readRes)
                        {
                            MessageBox.Show(errMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Input data file is invalid.");
                    }
                }
                else
                    MessageBox.Show("Input data file location not valid. Please check file name and location.");

                if (System.IO.File.Exists(form.IsisDataFileName) && readRes)
                {
                    if (IsisData.HydraulicElementsCollection.ElementList.Count > 0)
                    {
                        IsisData.CreateNetwork();
                    }

                    form.xsecXNS11outputfile = form.Outputdirectory + "\\" + System.IO.Path.GetFileNameWithoutExtension(form.IsisDataFileName) + ".xns11";
                    IsisData.WriteXNS11(form.xsecXNS11outputfile);

                    if (IsisData.HydraulicElementsCollection.ElementList.Count > 0)
                    {
                        string hydroFilename = form.Outputdirectory + "\\" + System.IO.Path.GetFileNameWithoutExtension(form.IsisDataFileName) + ".mhydro";
                        string Dfs0Directory = form.Outputdirectory + "\\TimeSeries\\";
                        IsisData.WriteMIKEHydroFile(hydroFilename, form.xsecXNS11outputfile, Dfs0Directory);
                    }
                    form.logfile = System.IO.Path.GetDirectoryName(form.IsisDataFileName);
                    form.logfile = form.Outputdirectory + "\\log.txt";

                    IsisData.WriteLog(form.logfile);

                    form.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ConvertButton_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ParameterizedThreadStart(this.DoConvert));
            t.IsBackground = true;
            t.Start(this);
        }

        private void buttonbrowsegxyfile_Click(object sender, EventArgs e)
        {
            openFileDialoggxyfile.ShowDialog();
        }

        private void openFileDialoggxyfile_FileOk(object sender, CancelEventArgs e)
        {
            IsisgeometryFileName = openFileDialoggxyfile.FileName;
            textBoxgxyfile.Text = IsisgeometryFileName;

            Outputdirectory = System.IO.Path.GetDirectoryName(IsisgeometryFileName) + "\\MIKE_HYDRO_model";
            outputNameTEXTBox.Text = Outputdirectory;
        }

        private void IsisConvertForm_Load(object sender, EventArgs e)
        {
            textBoxTimeStep.Text = "60";

            this.dateTimePickerDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerDate.CustomFormat = "yyyy-MM-dd HH:mm:ss";

            dateTimePickerDate.Value = new DateTime(2014, 1, 1, 12, 0, 0);

            this.BackColor = Color.FromArgb(241, 241, 241);
        }

        private void buttonIef_Click(object sender, EventArgs e)
        {
            openFileDialogIeffile.ShowDialog();
        }

        private void openFileDialogIeffile_FileOk(object sender, CancelEventArgs e)
        {
            IsisIefFileName = openFileDialogIeffile.FileName;
            textBoxIeffile.Text = IsisIefFileName;
            if (System.IO.File.Exists(IsisIefFileName))
            {
                int a = -1;
                int b = -1;
                int c = -1;
                GetIefInfo(IsisIefFileName, ref a, ref b, ref c);
                if (c != -1)
                {
                    textBoxTimeStep.Text = c.ToString();
                }
                else
                {
                    textBoxTimeStep.Text = "60";
                }
                if (a != -1 && b != -1)
                {
                    textBoxDuration.Text = (b - a).ToString();
                }
                else
                {
                    textBoxDuration.Text = "";
                }
            }
        }

        public void GetIefInfo(string IsisIefFileName, ref int start, ref int finish, ref int timestep)
        {
            string[] iefFileStringArray;
            iefFileStringArray = System.IO.File.ReadAllLines(IsisIefFileName);
            for (int i = 0; i < iefFileStringArray.Count(); i++)
            {
                if (iefFileStringArray[i].Contains("Start="))
                {
                    string startValue = iefFileStringArray[i].Replace("Start=", "");
                    if (IsNumeric(startValue))
                    {
                        start = int.Parse(startValue.Trim());
                    }
                }
                if (iefFileStringArray[i].Contains("Finish="))
                {
                    string finishValue = iefFileStringArray[i].Replace("Finish=", "");
                    if (IsNumeric(finishValue))
                    {
                        finish = int.Parse(finishValue.Trim());
                    }
                }
                if (iefFileStringArray[i].Contains("Timestep="))
                {
                    string timestepValue = iefFileStringArray[i].Replace("Timestep=", "");
                    if (IsNumeric(timestepValue))
                    {
                        timestep = int.Parse(timestepValue.Trim());
                    }
                }
            }
        }

        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        private void buttonFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogOutput.ShowDialog() == DialogResult.OK)
            {
                Outputdirectory = folderBrowserDialogOutput.SelectedPath;
                outputNameTEXTBox.Text = Outputdirectory;
            }
        }

        private void textBoxgxyfile_TextChanged(object sender, EventArgs e)
        {
            IsisgeometryFileName = textBoxgxyfile.Text;

            Outputdirectory = System.IO.Path.GetDirectoryName(IsisgeometryFileName) + "\\MIKE_HYDRO_model";
            outputNameTEXTBox.Text = Outputdirectory;
        }

        private void InputDataFileTextBox_TextChanged(object sender, EventArgs e)
        {
            IsisDataFileName = InputDataFileTextBox.Text;

            Outputdirectory = System.IO.Path.GetDirectoryName(IsisDataFileName) + "\\MIKE_HYDRO_model";
            outputNameTEXTBox.Text = Outputdirectory;
        }

        private void textBoxIeffile_TextChanged(object sender, EventArgs e)
        {
            IsisIefFileName = textBoxIeffile.Text;
        }

        private void outputNameTEXTBox_TextChanged(object sender, EventArgs e)
        {
            Outputdirectory = outputNameTEXTBox.Text;
        }
    }
}
