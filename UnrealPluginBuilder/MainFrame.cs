using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Text;

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

        private readonly string resourceDirName = "Resources";
        private readonly string sourceDirName = "Source";
        private readonly string contentDirName = "Content";

        private Process buildProcess = null;

        private bool isInBuildProcess = false;
        private bool IsInBuildProcess
        {
            get => isInBuildProcess;
            set
            {
                isInBuildProcess = value;
                UpdateFormControlState();
            }
        }

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
            get => Path.Combine(OutputDir, "BuiltPlugins");
        }

        private string PackageDir
        {
            get => Path.Combine(OutputDir, "PackagedPlugins");
        }

        private string TempDir
        {
            get => Path.Combine(OutputDir, "Temp");
        }

        private string LogDir
        {
            get => Path.Combine(OutputDir, "Logs");
        }

        private string OutputLog
        {
            get => tb_OutputLog.Text;
            set => SafeSetOutputLog(value);
        }
        private void SafeSetOutputLog(string text)
        {
            if (tb_OutputLog.InvokeRequired)
            {
                Invoke((MethodInvoker)delegate () { SafeSetOutputLog(text); });
            }
            else
            {
                tb_OutputLog.Text = text;
                tb_OutputLog.SelectionStart = text.Length;
                tb_OutputLog.SelectionLength = 0;
                tb_OutputLog.ScrollToCaret();
            }
        }

        private bool CreatePackage
        {
            get => cb_CreatePackage.Checked;
        }

        private bool StrictIncludes
        {
            get => cb_StrictIncludes.Checked;
        }

        private void SafeSetBuildButtonState(bool state)
        {
            if (btn_Build.InvokeRequired)
            {
                Invoke((MethodInvoker)delegate () { SafeSetBuildButtonState(state); });
            }
            else
            {
                btn_Build.Enabled = state;
            }
        }

        private void SafeSetFormControlState(bool state)
        {
            if (tb_ProjectPath.InvokeRequired ||
                btn_PickProjectPath.InvokeRequired ||
                tb_PluginPath.InvokeRequired ||
                btn_PickPluginPath.InvokeRequired ||
                btn_AddBatchFile.InvokeRequired ||
                btn_RemoveBatchFile.InvokeRequired ||
                clb_BuildBatchFiles.InvokeRequired ||
                tb_OutputDir.InvokeRequired ||
                btn_PickOutputDir.InvokeRequired ||
                btn_Build.InvokeRequired ||
                cb_CreatePackage.InvokeRequired ||
                cb_StrictIncludes.InvokeRequired)
            {
                Invoke((MethodInvoker)delegate () { SafeSetFormControlState(state); });
            }
            else
            {
                tb_ProjectPath.ReadOnly = !state;
                btn_PickProjectPath.Enabled = state;
                tb_PluginPath.ReadOnly = !state;
                btn_PickPluginPath.Enabled = state;
                btn_AddBatchFile.Enabled = state;
                btn_RemoveBatchFile.Enabled = state;
                clb_BuildBatchFiles.Enabled = state;
                tb_OutputDir.ReadOnly = !state;
                btn_PickOutputDir.Enabled = state;
                btn_Build.Enabled = state;
                cb_CreatePackage.Enabled = state;
                cb_StrictIncludes.Enabled = state;
            }
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
            if (buildProcess != null)
            {
                if (!buildProcess.HasExited)
                {
                    buildProcess.Kill();
                }
            }

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
                OutputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            }

            UpdateBuildButtonState();
        }

        private void UpdateBuildButtonState()
        {
            var newState = (
                UprojectPath != string.Empty &&
                UpluginPath != string.Empty &&
                OutputDir != string.Empty &&
                GetBatchFilePaths().Count > 0 &&
                !IsInBuildProcess
            );

            SafeSetBuildButtonState(newState);
        }

        private void UpdateFormControlState()
        {
            SafeSetFormControlState(!IsInBuildProcess);
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

        private string GetEngineVersioinPath(string batchFilePath)
        {
            var buildDirPath = Directory.GetParent(batchFilePath).Parent.FullName;
            var versionFilePath = Path.GetFullPath(Path.Combine(buildDirPath, "Build.version"));

            return versionFilePath;
        }

        private EngineVersion GetEngineVersion(string batchFilePath)
        {
            var versionFilePath = GetEngineVersioinPath(batchFilePath);

            if (!File.Exists(versionFilePath))
            {
                return null;
            }

            var jsonString = File.ReadAllText(versionFilePath);
            return JsonSerializer.Deserialize<EngineVersion>(jsonString);
        }

        private string GetEngineVersionString(EngineVersion engineVersion)
        {
            if (engineVersion == null)
            {
                return "UnknownVersion";
            }

            return $"{engineVersion.MajorVersion}.{engineVersion.MinorVersion}";
        }

        private PluginInfo GetPluginInfo()
        {
            if (!File.Exists(UpluginPath))
            {
                return null;
            }

            var jsonString = File.ReadAllText(UpluginPath);
            return JsonSerializer.Deserialize<PluginInfo>(jsonString);
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

        private void CopyFileToTemp(string sourceDirPath, string filename)
        {
            var sourceFilePath = Path.Combine(sourceDirPath, filename);
            var destFilePath = Path.Combine(TempDir, filename);
            File.Copy(sourceFilePath, destFilePath);
            OutputLog += $"Copy \"{sourceFilePath}\" to \"{destFilePath}\"\r\n";
        }

        private void CopyDirectoryToTemp(string sourceRootDirPath, string dirName)
        {
            var sourceDirPath = Path.Combine(sourceRootDirPath, dirName);
            var destDirPath = Path.Combine(TempDir, dirName);
            DirectoryCopy(sourceDirPath, destDirPath);
            OutputLog += $"Copy \"{sourceDirPath}\" to \"{destDirPath}\"\r\n";
        }

        private void DirectoryCopy(string sourceDirName, string destDirName)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                return;
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
    
            Directory.CreateDirectory(destDirName);

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, tempPath);
            }
        }

        private string CreateTitleMessage(string message)
        {
            string frame = string.Empty;
            int length = message.Length + message.Length / 2;
            for (int Index = 0; Index <= length; Index++)
            {
                frame += '=';
            }

            return $"{frame}\r\n{message}\r\n{frame}\r\n\r\n";
        }

        private struct BuildResult
        {
            public string outputPath;
            public bool wasSuccessful;

            public BuildResult(string outputPath, bool wasSuccessful)
            {
                this.outputPath = outputPath;
                this.wasSuccessful = wasSuccessful;
            }

            static public BuildResult Faild()
            {
                return new BuildResult(string.Empty, false);
            }
        };
        private BuildResult RunBuildProcess(string batchFilePath)
        {
            var pluginInfo = GetPluginInfo();
            var engineVersion = GetEngineVersion(batchFilePath);
            if (pluginInfo == null)
            {
                OutputLog += CreateTitleMessage($"Error : Not found uplugin file. ({UpluginPath})");
                return BuildResult.Faild();
            }
            if (engineVersion == null)
            {
                OutputLog += CreateTitleMessage($"Error : Not found version file. ({GetEngineVersioinPath(batchFilePath)})");
                return BuildResult.Faild();
            }
            var engineVersionString = GetEngineVersionString(engineVersion);

            var header = $"[Plugin Name] {pluginInfo.PluginName} / [Plugin Version] {pluginInfo.VersionName} / [Engine Version] {engineVersionString}";
            OutputLog += CreateTitleMessage(header);

            buildProcess = new Process();
            buildProcess.StartInfo.FileName = batchFilePath;
            buildProcess.StartInfo.CreateNoWindow = true;
            buildProcess.StartInfo.UseShellExecute = false;

            var OutputDirName = $"{pluginInfo.PluginName}_{engineVersionString}";
            var OutputDirPath = Path.Combine(BuildDir, OutputDirName);
            buildProcess.StartInfo.Arguments = $"BuildPlugin -Plugin=\"{UpluginPath}\" -Package=\"{OutputDirPath}\" -Rocket";
            if (StrictIncludes)
            {
                buildProcess.StartInfo.Arguments += " -strictincludes";
            }

            buildProcess.StartInfo.RedirectStandardOutput = true;

            buildProcess.Start();
            while (!buildProcess.HasExited)
            {
                var line = buildProcess.StandardOutput.ReadLine() + "\r\n";
                OutputLog += line;
            }
            var output = buildProcess.StandardOutput.ReadToEnd();
            output = output.Replace("\r\r\n", "\r\n");
            OutputLog += output;

            var wasSuccessful = (buildProcess.ExitCode == 0);
            if (wasSuccessful)
            {
                OutputLog += $"Output to \"{OutputDirPath}\"\r\n\r\n";
            }

            return new BuildResult(OutputDirPath, wasSuccessful);
        }

        private bool RunPackageProcess(string batchFilePath, string builtPluginPath)
        {
            var pluginInfo = GetPluginInfo();
            var engineVersion = GetEngineVersion(batchFilePath);
            if (pluginInfo == null)
            {
                OutputLog += CreateTitleMessage($"Error : Not found uplugin file. ({UpluginPath})");
                return false;
            }
            if (engineVersion == null)
            {
                OutputLog += CreateTitleMessage($"Error : Not found version file. ({GetEngineVersioinPath(batchFilePath)})");
                return false;
            }
            var engineVersionString = GetEngineVersionString(engineVersion);

            try
            {
                if (pluginInfo.CanContainContent)
                {
                    CopyDirectoryToTemp(builtPluginPath, contentDirName);
                }
                CopyDirectoryToTemp(builtPluginPath, resourceDirName);
                CopyDirectoryToTemp(builtPluginPath, sourceDirName);
                CopyFileToTemp(builtPluginPath, Path.GetFileName(UpluginPath));

                var OutputFilename = $"{pluginInfo.PluginName}{pluginInfo.VersionName}.zip";
                var OutputDirName = $"{pluginInfo.PluginName}_{engineVersionString}";
                var OutputDirPath = Path.Combine(PackageDir, OutputDirName);
                var OutputFilePath = Path.Combine(OutputDirPath, OutputFilename);

                if (!Directory.Exists(OutputDirPath))
                {
                    Directory.CreateDirectory(OutputDirPath);
                }
                else
                {
                    if (File.Exists(OutputFilePath))
                    {
                        File.Delete(OutputFilePath);
                    }
                }

                OutputLog += $"\r\nCreating {OutputFilename}\r\n\r\n";
                ZipFile.CreateFromDirectory(TempDir, OutputFilePath, CompressionLevel.Optimal, false, Encoding.UTF8);

                OutputLog += $"Output to \"{OutputFilePath}\"\r\n\r\n";

                Directory.Delete(TempDir, true);
                return true;
            }
            catch
            {
                if (Directory.Exists(TempDir))
                {
                    Directory.Delete(TempDir, true);
                }
                return false;
            }
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

        private void btn_PickOutputDir_Click(object sender, EventArgs e)
        {
            OutputDir = PickDirectoryPath(
                "Please specify the directory to output the package of the built plug-in."
            );

            ValidateParameters();
        }

        private void btn_Build_Click(object sender, EventArgs e)
        {
            OutputLog = string.Empty;
            tb_OutputDir.Focus();
            IsInBuildProcess = true;
           
            Task.Factory.StartNew(() =>
            {
                var BatchFilePaths = GetBatchFilePaths();
                foreach (var BatchFilePath in BatchFilePaths)
                {
                    BuildResult buildResult = RunBuildProcess(BatchFilePath);
                    if (buildResult.wasSuccessful && CreatePackage)
                    {
                        RunPackageProcess(BatchFilePath, buildResult.outputPath);
                    }
                }

                IsInBuildProcess = false;

                if (!Directory.Exists(LogDir))
                {
                    Directory.CreateDirectory(LogDir);
                }

                var logFilename = $"OutputLog_{DateTime.Now.Year}.{DateTime.Now.Month}.{DateTime.Now.Day}.{DateTime.Now.Hour}.{DateTime.Now.Minute}.{DateTime.Now.Second}.{DateTime.Now.Millisecond}.txt";
                var logFilePath = Path.Combine(LogDir, logFilename);
                var streamWriter = new StreamWriter(logFilePath, false, Encoding.UTF8);
                streamWriter.Write(OutputLog);
                streamWriter.Close();
            });
        }

        private void clb_BuildBatchFiles_MouseDown(object sender, MouseEventArgs e)
        {
            UpdateFormControlState();
        }
    }
}
