using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dacster {
    public class Dacpac {
        public static string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
        public static string outputFolder;
        List<string> instanceList = new List<string>();
        List<string> databaseList = new List<string>();
        LogWriter Log = new LogWriter("----- Running Dacpac Class -----");

        public Dacpac() {
            string outputFolder = CreateOutputDirectory();
        }

        public Dacpac(string instance, string database) {
            Log.LogWrite("Log file created");
            instanceList.Add(instance);
            Log.LogWrite("Instance list after add operation:");
            foreach (string singleInstance in instanceList) {
                Log.LogWrite(singleInstance);
            }
            databaseList.Add(database);
            Log.LogWrite("Database list after add operation:");
            foreach (string singleDatabase in databaseList) {
                Log.LogWrite(singleDatabase);
            }
            outputFolder = CreateOutputDirectory();
            Log.LogWrite($"Output file created at {outputFolder}");
        }

        public Dacpac(string instance, List<string> databases) {
            instanceList.Add(instance);
            foreach (string database in databases) {
                databaseList.Add(database);
            }
            outputFolder = CreateOutputDirectory();
        }

        public Dacpac(List<string> instances, string database) {
            foreach (string instance in instances) {
                instanceList.Add(instance);
            }
            databaseList.Add(database);
            outputFolder = CreateOutputDirectory();
        }

        public Dacpac(List<string> instances, List<string> databases) {
            foreach (string instance in instances) {
                instanceList.Add(instance);
            }
            foreach (string database in databases) {
                databaseList.Add(database);
            }
            outputFolder = CreateOutputDirectory();
        }

        public void AddInstance(string instance) {
            instanceList.Add(instance);
        }

        public void AddDatabase(string database) {
            databaseList.Add(database);
        }

        public void AddMultipleInstances(List<string> instances) {
            foreach (string instance in instances) {
                instanceList.Add(instance);
            }
        }

        public void AddMultipleDatabases(List<string> databases) {
            foreach (string database in databases) {
                databaseList.Add(database);
            }
        }

        private string CreateOutputDirectory() {
            string outputPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            outputPath = System.IO.Path.Combine(outputPath, "DACPACs");
            outputPath = System.IO.Path.Combine(outputPath, currentDateTime);
            System.IO.Directory.CreateDirectory(outputPath);
            return outputPath;
        }

        public static string GenerateFilename(string instance, string database, string outputFolder, string currentDateTime) {
            string filename = $"{instance}-{database}-{currentDateTime}.dacpac";
            string filenamePath = System.IO.Path.Combine(outputFolder, filename);
            return filenamePath;
        }

        public static System.Diagnostics.Process GenerateShellProcess() {
            System.Diagnostics.Process cmd = new System.Diagnostics.Process();
            cmd.StartInfo.FileName = "powershell.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            return cmd;
        }

        public static void EndShellProcess(System.Diagnostics.Process process) {
            process.StandardInput.Flush();
            process.StandardInput.Close();
            process.WaitForExit();
        }

        public static void DisplayMessageBox(string problem) {
            string messageBoxText = "";
            string caption = "";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            switch (problem) {
                case "LocateSQLPackage":
                    messageBoxText = "Not able to locate sqlpackage.exe are you use you have it installed on this machine?";
                    caption = problem;
                    MessageBox.Show(messageBoxText, caption, button, icon);
                    break;
                default:
                    messageBoxText = problem;
                    caption = "Unknown Error";
                    MessageBox.Show(messageBoxText, caption, button, icon);
                    break;
            }
        }
    }
}