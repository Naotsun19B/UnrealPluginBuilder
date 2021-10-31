using System;
using System.Windows.Forms;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;

namespace UnrealPluginBuilder
{
    public partial class MainFrame : Form
    {
        private readonly string iniFilename = Path.GetFullPath("Settings.ini");

        private readonly string uprojectExtension = ".uproject";
        private readonly string upluginExtension = ".uplugin";
        private readonly string batchExtension = ".bat";
        private readonly string batchFilename = "RunUAT";

        private readonly string uprojectSection = "UprojectFile";
        private readonly string uprojectKey = "Path";
        private readonly string upluginSection = "UpluginFile";
        private readonly string upluginKey = "Path";
        private readonly string batchFilesSection = "BatchFiles";
        private readonly string batchFilesCountKey = "Count";
        private readonly string outputDirSection = "OutputDir";
        private readonly string outputDirKey = "Path";

        private string UprojectPath
        {
            get => tb_ProjectPath.Text;
            set => tb_ProjectPath.Text = value;
        }

        private string UpluginPath
        {
            get => tb_PluginPath.Text;
            set => tb_PluginPath.Text = value;
        }

        private string OutputDir
        {
            get => tb_OutputDir.Text;
            set => tb_OutputDir.Text = value;
        }

        private string BuildDir
        {
            get => Path.Combine(OutputDir, "BuiltPlugin");
        }

        private string PackageDir
        {
            get => Path.Combine(OutputDir, "PackagedPlugin");
        }

        private string OutputLog
        {
            get => tb_OutputLog.Text;
            set => tb_OutputLog.Text = value;
        }

        private bool StrictIncludes
        {
            get => cb_StrictIncludes.Checked;
        }

        public MainFrame()
        {
            InitializeComponent();
        }

        private void MainFrame_Load(object sender, EventArgs e)
        {
            if (!File.Exists(iniFilename))
            {
                File.Create(iniFilename);
            }

            Deserialize();
            ValidateParameters();
        }

        private void MainFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            Serialize();
        }

        void Serialize()
        {
            var ini = new IniFile(iniFilename);
            ini.SetValue(uprojectSection, uprojectKey, UprojectPath);
            ini.SetValue(upluginSection, upluginKey, UpluginPath);

            ini.SetValue(batchFilesSection, batchFilesCountKey, clb_BuildBatchFiles.Items.Count);
            int Index = 0;
            foreach (var item in clb_BuildBatchFiles.Items)
            {
                var filenameKey = $"BatchFilename_{Index}";
                ini.SetValue(batchFilesSection, filenameKey, item);

                var checkStateKey = $"CheckState_{Index}";
                ini.SetValue(batchFilesSection, checkStateKey, clb_BuildBatchFiles.GetItemChecked(Index));

                Index++;
            }

            ini.SetValue(outputDirSection, outputDirKey, OutputDir);
        }

        void Deserialize()
        {
            var ini = new IniFile(iniFilename);
            UprojectPath = ini.GetValueOrDefault(uprojectSection, uprojectKey, string.Empty);
            UpluginPath = ini.GetValueOrDefault(upluginSection, upluginKey, string.Empty);

            int batchFilesCount = ini.GetValueOrDefault(batchFilesSection, batchFilesCountKey, 0);
            for (int Index = 0; Index < batchFilesCount; Index++)
            {
                var filenameKey = $"BatchFilename_{Index}";
                string filename = ini.GetValueOrDefault(batchFilesSection, filenameKey, string.Empty);

                var checkStateKey = $"CheckState_{Index}";
                var checkState = ini.GetValueOrDefault(batchFilesSection, checkStateKey, true);

                clb_BuildBatchFiles.Items.Add(filename, checkState);
            }

            OutputDir = ini.GetValueOrDefault(outputDirSection, outputDirKey, string.Empty);
        }

        private void ValidateParameters()
        {
            if (Path.GetExtension(UprojectPath) != uprojectExtension ||
                !File.Exists(UprojectPath))
            {
                UprojectPath = string.Empty;
            }

            if (Path.GetExtension(UpluginPath) != upluginExtension ||
                !File.Exists(UpluginPath))
            {
                UpluginPath = string.Empty;
            }

            List<object> invalidItems = new List<object>();
            foreach (var item in clb_BuildBatchFiles.Items)
            {
                if (Path.GetFileNameWithoutExtension(item.ToString()) != batchFilename ||
                    Path.GetExtension(item.ToString()) != batchExtension)
                {
                    invalidItems.Add(item);
                }
            }
            foreach (var invalidItem in invalidItems)
            {
                clb_BuildBatchFiles.Items.Remove(invalidItem);
            }

            if (!Directory.Exists(OutputDir))
            {
                OutputDir = string.Empty;
            }

            UpdateBuildButtonState();
        }

        private void UpdateBuildButtonState()
        {
            btn_Build.Enabled = (
                UprojectPath != string.Empty &&
                UpluginPath != string.Empty &&
                OutputDir != string.Empty &&
                GetBatchFilePaths().Count > 0
            );
        }

        private List<string> GetBatchFilePaths()
        {
            List<string> Result = new List<string>();

            for (int Index = 0; Index < clb_BuildBatchFiles.Items.Count; Index++)
            {
                if (clb_BuildBatchFiles.GetItemChecked(Index))
                {
                    Result.Add(clb_BuildBatchFiles.Items[Index].ToString());
                }
            }

            return Result;
        }

        private string PickFilePath(string title, string filter)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = title;
            openFileDialog.Filter = filter;
            openFileDialog.FilterIndex = 1;

            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }

            return string.Empty;
        }

        private string PickDirectoryPath(string title)
        {
            var folderSelectDialog = new FolderSelectDialog();
            folderSelectDialog.Title = title;

            DialogResult result = folderSelectDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                return folderSelectDialog.Path;
            }

            return string.Empty;
        }

        private void RunBuildProcess(string BatchFilePath)
        {
            var process = new Process();
            process.StartInfo.FileName = BatchFilePath;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            
            process.StartInfo.Arguments = $"BuildPlugin -Plugin=\"{UpluginPath}\" -Package=\"{BuildDir}\" -Rocket";
            if (StrictIncludes)
            {
                process.StartInfo.Arguments += " -strictincludes";
            }

            process.StartInfo.RedirectStandardOutput = true;

            process.Start();

            var output = process.StandardOutput.ReadToEnd();
            output = output.Replace("\r\r\n", "\n");
            OutputLog += output;

            process.WaitForExit();
        }

        private void btn_PickProjectPath_Click(object sender, EventArgs e)
        {
            UprojectPath = PickFilePath(
                "Please specify the project file you want to build.",
                "Unreal Engine Project File(*" + uprojectExtension + ")|*" + uprojectExtension
            );

            ValidateParameters();
        }

        private void btn_PickPluginPath_Click(object sender, EventArgs e)
        {
           UpluginPath = PickFilePath(
               "Please specify the plugin file you want to build.",
               "UPLUGIN File(*" + upluginExtension + ")|*" + upluginExtension
           );

            ValidateParameters();
        }

        private void btn_AddBatchFile_Click(object sender, EventArgs e)
        {
            var batchFilePath = PickFilePath(
                "Please specify the build batch file in the engine folder. ([EngineRoot]/Engine/Build/BatchFiles/" + batchFilename + batchExtension + ")",
                "Windows Batch File(*" + batchExtension + ")|*" + batchExtension
            );

            clb_BuildBatchFiles.Items.Add(batchFilePath, true);

            ValidateParameters();
        }

        private void btn_RemoveBatchFile_Click(object sender, EventArgs e)
        {
            clb_BuildBatchFiles.Items.Remove(clb_BuildBatchFiles.SelectedItem);
        }

        private void btn_OutputDir_Click(object sender, EventArgs e)
        {
            OutputDir = PickDirectoryPath(
                "Please specify the directory to output the package of the built plug-in."
            );

            ValidateParameters();
        }

        private void btn_Build_Click(object sender, EventArgs e)
        {
            OutputLog = string.Empty;

            var BatchFilePaths = GetBatchFilePaths();
            foreach (var BatchFilePath in BatchFilePaths)
            {
                RunBuildProcess(BatchFilePath);
            }
        }

        private void clb_BuildBatchFiles_MouseDown(object sender, MouseEventArgs e)
        {
            UpdateBuildButtonState();
        }
    }
}
