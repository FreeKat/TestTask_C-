using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace TaskManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog open = new FolderBrowserDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                txtSource.Text = open.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog target = new FolderBrowserDialog();
            if (target.ShowDialog() == DialogResult.OK)
            {
                txtReplica.Text = target.SelectedPath;
            }
        }
        
        static public void CopyDirectory(string SourceDirectory, string ReplicaDirectory)
        {
            
            DirectoryInfo source = new DirectoryInfo(SourceDirectory);
            DirectoryInfo replica = new DirectoryInfo(ReplicaDirectory); 

            //Copy files.
            FileInfo[] sourceFiles = source.GetFiles();
            for (int i = 0; i < sourceFiles.Length; ++i)
                File.Copy(sourceFiles[i].FullName, replica.FullName + "\\" + sourceFiles[i].Name, true);

            //Copy directories.
            DirectoryInfo[] sourceDirectories = source.GetDirectories();
            for (int j = 0; j < sourceDirectories.Length; ++j)
                CopyDirectory(sourceDirectories[j].FullName, replica.FullName + "\\" + sourceDirectories[j].Name);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txtSource.Text == string.Empty)
            {
                MessageBox.Show("Source File does not exist!", "Error");
                return;
            }
            
            DirectoryInfo source = new DirectoryInfo(txtSource.Text);
            DirectoryInfo replica = new DirectoryInfo(txtReplica.Text);

            //Determine whether the source directory exists.
            if (!source.Exists)
                return;
            //Determine whether the replica directory exists.
            if (!replica.Exists)
                replica.Create();


            replica.Delete(true);
            replica.Create();

            FileInfo[] sourceFiles = source.GetFiles();
            for (int i = 0; i < sourceFiles.Length; ++i)
                File.Copy(sourceFiles[i].FullName, replica.FullName + "\\" + sourceFiles[i].Name, true);

            //Copy directories.
            DirectoryInfo[] sourceDirectories = source.GetDirectories();
            for (int j = 0; j < sourceDirectories.Length; ++j)
                CopyDirectory(sourceDirectories[j].FullName, replica.FullName + "\\" + sourceDirectories[j].Name);

            MessageBox.Show("Replica Folder has been updated successfully", "Job Finished!");

            WriteToProgress(" #Replica Folder has been updated successfully \n");
            WriteToProgress(" Detailed report \n");
            for (int i = 0; i < sourceFiles.Length; ++i)
            WriteToProgress(sourceFiles[i].FullName + " copied successfully.");            
            var progress = WriteToProgress("\n \n End of report.");

        }

        public List<string> WriteToProgress(string step)
        {
            if (!string.IsNullOrEmpty(step))
                listBox1.Items.Add(step);
            return listBox1.Items.OfType<Object>().Where(o => o != null).Select(o => o.ToString()).ToList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var saveFile = new SaveFileDialog();
            saveFile.Filter = "Text (*.txt)|*.txt";
            saveFile.FileName = "Raport";
            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (var sw = new System.IO.StreamWriter(saveFile.FileName, false))
                    foreach (var item in listBox1.Items)
                        sw.Write(item.ToString() + Environment.NewLine); // Environment.CurrentDirectory);
                MessageBox.Show("The file has been saved successfully");
            }
        }

        
    }
}
