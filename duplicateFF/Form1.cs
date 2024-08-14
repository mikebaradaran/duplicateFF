using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Windows.Forms;

namespace duplicateFF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //string oneDrivePath1 = @"C:\Users\mikeb\OneDrive\Family photos all\";
        string oneDrivePath = @"C:\Users\mikeb\OneDrive\";

        List<string> listItems = new List<string>();

        private void display(string msg)
        {
            listItems.Add(msg.Replace(@"C:\Users\mikeb\OneDrive\",""));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            start();
            foreach (var item in listItems)
            {
                listBox1.Items.Add(item);
            }
        }


        private void start()
        {
            var fileNames = new Dictionary<string, List<string>>();

            try
            {
                var files = Directory.GetFiles(oneDrivePath, "*.MP4", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    try
                    {
                        string fileName = Path.GetFileName(file);
                        if (fileNames.ContainsKey(fileName))
                        {
                            fileNames[fileName].Add(file);
                        }
                        else
                        {
                            fileNames[fileName] = new List<string> { file };
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        display($"Access denied to file: {file}");
                    }
                    catch (Exception ex)
                    {
                        display($"Error processing file {file}: {ex.Message}");
                    }
                }

                // Display duplicate file names only
                foreach (var entry in fileNames.Where(e => e.Value.Count > 1))
                {
                    display(" ");
                    display($"Duplicate file name found: {entry.Key}");
                    foreach (var duplicate in entry.Value)
                    {
                        display($"- {duplicate}");
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                display("Access denied to a directory or file.");
            }
        }
    }
}

