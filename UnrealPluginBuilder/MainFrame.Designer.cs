
namespace UnrealPluginBuilder
{
    partial class MainFrame
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
            this.btn_Build = new System.Windows.Forms.Button();
            this.tb_ProjectPath = new System.Windows.Forms.TextBox();
            this.lbl_ProjectPath = new System.Windows.Forms.Label();
            this.btn_PickProjectPath = new System.Windows.Forms.Button();
            this.btn_PickPluginPath = new System.Windows.Forms.Button();
            this.lbl_PluginPath = new System.Windows.Forms.Label();
            this.tb_PluginPath = new System.Windows.Forms.TextBox();
            this.clb_BuildBatchFiles = new System.Windows.Forms.CheckedListBox();
            this.btn_AddBatchFile = new System.Windows.Forms.Button();
            this.lbl_BuildBatchFiles = new System.Windows.Forms.Label();
            this.btn_RemoveBatchFile = new System.Windows.Forms.Button();
            this.tb_OutputLog = new System.Windows.Forms.TextBox();
            this.cb_StrictIncludes = new System.Windows.Forms.CheckBox();
            this.lbl_OutputDir = new System.Windows.Forms.Label();
            this.tb_OutputDir = new System.Windows.Forms.TextBox();
            this.btn_OutputDir = new System.Windows.Forms.Button();
            this.cb_CreatePackage = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btn_Build
            // 
            this.btn_Build.Location = new System.Drawing.Point(8, 257);
            this.btn_Build.Name = "btn_Build";
            this.btn_Build.Size = new System.Drawing.Size(75, 23);
            this.btn_Build.TabIndex = 0;
            this.btn_Build.Text = "Build";
            this.btn_Build.UseVisualStyleBackColor = true;
            this.btn_Build.Click += new System.EventHandler(this.btn_Build_Click);
            // 
            // tb_ProjectPath
            // 
            this.tb_ProjectPath.Location = new System.Drawing.Point(89, 12);
            this.tb_ProjectPath.Name = "tb_ProjectPath";
            this.tb_ProjectPath.Size = new System.Drawing.Size(665, 23);
            this.tb_ProjectPath.TabIndex = 1;
            // 
            // lbl_ProjectPath
            // 
            this.lbl_ProjectPath.AutoSize = true;
            this.lbl_ProjectPath.Location = new System.Drawing.Point(12, 15);
            this.lbl_ProjectPath.Name = "lbl_ProjectPath";
            this.lbl_ProjectPath.Size = new System.Drawing.Size(71, 15);
            this.lbl_ProjectPath.TabIndex = 2;
            this.lbl_ProjectPath.Text = "Project Path";
            // 
            // btn_PickProjectPath
            // 
            this.btn_PickProjectPath.Location = new System.Drawing.Point(760, 11);
            this.btn_PickProjectPath.Name = "btn_PickProjectPath";
            this.btn_PickProjectPath.Size = new System.Drawing.Size(28, 23);
            this.btn_PickProjectPath.TabIndex = 3;
            this.btn_PickProjectPath.Text = "...";
            this.btn_PickProjectPath.UseVisualStyleBackColor = true;
            this.btn_PickProjectPath.Click += new System.EventHandler(this.btn_PickProjectPath_Click);
            // 
            // btn_PickPluginPath
            // 
            this.btn_PickPluginPath.Location = new System.Drawing.Point(760, 40);
            this.btn_PickPluginPath.Name = "btn_PickPluginPath";
            this.btn_PickPluginPath.Size = new System.Drawing.Size(28, 23);
            this.btn_PickPluginPath.TabIndex = 6;
            this.btn_PickPluginPath.Text = "...";
            this.btn_PickPluginPath.UseVisualStyleBackColor = true;
            this.btn_PickPluginPath.Click += new System.EventHandler(this.btn_PickPluginPath_Click);
            // 
            // lbl_PluginPath
            // 
            this.lbl_PluginPath.AutoSize = true;
            this.lbl_PluginPath.Location = new System.Drawing.Point(12, 44);
            this.lbl_PluginPath.Name = "lbl_PluginPath";
            this.lbl_PluginPath.Size = new System.Drawing.Size(68, 15);
            this.lbl_PluginPath.TabIndex = 5;
            this.lbl_PluginPath.Text = "Plugin Path";
            // 
            // tb_PluginPath
            // 
            this.tb_PluginPath.Location = new System.Drawing.Point(89, 41);
            this.tb_PluginPath.Name = "tb_PluginPath";
            this.tb_PluginPath.Size = new System.Drawing.Size(665, 23);
            this.tb_PluginPath.TabIndex = 4;
            // 
            // clb_BuildBatchFiles
            // 
            this.clb_BuildBatchFiles.FormattingEnabled = true;
            this.clb_BuildBatchFiles.Location = new System.Drawing.Point(89, 70);
            this.clb_BuildBatchFiles.Name = "clb_BuildBatchFiles";
            this.clb_BuildBatchFiles.Size = new System.Drawing.Size(699, 148);
            this.clb_BuildBatchFiles.TabIndex = 7;
            this.clb_BuildBatchFiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.clb_BuildBatchFiles_MouseDown);
            // 
            // btn_AddBatchFile
            // 
            this.btn_AddBatchFile.Location = new System.Drawing.Point(8, 110);
            this.btn_AddBatchFile.Name = "btn_AddBatchFile";
            this.btn_AddBatchFile.Size = new System.Drawing.Size(75, 23);
            this.btn_AddBatchFile.TabIndex = 8;
            this.btn_AddBatchFile.Text = "Add";
            this.btn_AddBatchFile.UseVisualStyleBackColor = true;
            this.btn_AddBatchFile.Click += new System.EventHandler(this.btn_AddBatchFile_Click);
            // 
            // lbl_BuildBatchFiles
            // 
            this.lbl_BuildBatchFiles.AutoSize = true;
            this.lbl_BuildBatchFiles.Location = new System.Drawing.Point(12, 80);
            this.lbl_BuildBatchFiles.Name = "lbl_BuildBatchFiles";
            this.lbl_BuildBatchFiles.Size = new System.Drawing.Size(63, 15);
            this.lbl_BuildBatchFiles.TabIndex = 9;
            this.lbl_BuildBatchFiles.Text = "Batch Files";
            // 
            // btn_RemoveBatchFile
            // 
            this.btn_RemoveBatchFile.Location = new System.Drawing.Point(8, 139);
            this.btn_RemoveBatchFile.Name = "btn_RemoveBatchFile";
            this.btn_RemoveBatchFile.Size = new System.Drawing.Size(75, 23);
            this.btn_RemoveBatchFile.TabIndex = 10;
            this.btn_RemoveBatchFile.Text = "Remove";
            this.btn_RemoveBatchFile.UseVisualStyleBackColor = true;
            this.btn_RemoveBatchFile.Click += new System.EventHandler(this.btn_RemoveBatchFile_Click);
            // 
            // tb_OutputLog
            // 
            this.tb_OutputLog.Location = new System.Drawing.Point(8, 285);
            this.tb_OutputLog.Multiline = true;
            this.tb_OutputLog.Name = "tb_OutputLog";
            this.tb_OutputLog.ReadOnly = true;
            this.tb_OutputLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tb_OutputLog.Size = new System.Drawing.Size(780, 264);
            this.tb_OutputLog.TabIndex = 11;
            this.tb_OutputLog.WordWrap = false;
            // 
            // cb_StrictIncludes
            // 
            this.cb_StrictIncludes.AutoSize = true;
            this.cb_StrictIncludes.Checked = true;
            this.cb_StrictIncludes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_StrictIncludes.Location = new System.Drawing.Point(202, 260);
            this.cb_StrictIncludes.Name = "cb_StrictIncludes";
            this.cb_StrictIncludes.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cb_StrictIncludes.Size = new System.Drawing.Size(100, 19);
            this.cb_StrictIncludes.TabIndex = 12;
            this.cb_StrictIncludes.Text = "Strict Includes";
            this.cb_StrictIncludes.UseVisualStyleBackColor = true;
            // 
            // lbl_OutputDir
            // 
            this.lbl_OutputDir.AutoSize = true;
            this.lbl_OutputDir.Location = new System.Drawing.Point(12, 227);
            this.lbl_OutputDir.Name = "lbl_OutputDir";
            this.lbl_OutputDir.Size = new System.Drawing.Size(63, 15);
            this.lbl_OutputDir.TabIndex = 13;
            this.lbl_OutputDir.Text = "Output Dir";
            // 
            // tb_OutputDir
            // 
            this.tb_OutputDir.Location = new System.Drawing.Point(89, 224);
            this.tb_OutputDir.Name = "tb_OutputDir";
            this.tb_OutputDir.Size = new System.Drawing.Size(665, 23);
            this.tb_OutputDir.TabIndex = 14;
            // 
            // btn_OutputDir
            // 
            this.btn_OutputDir.Location = new System.Drawing.Point(760, 223);
            this.btn_OutputDir.Name = "btn_OutputDir";
            this.btn_OutputDir.Size = new System.Drawing.Size(28, 23);
            this.btn_OutputDir.TabIndex = 15;
            this.btn_OutputDir.Text = "...";
            this.btn_OutputDir.UseVisualStyleBackColor = true;
            this.btn_OutputDir.Click += new System.EventHandler(this.btn_OutputDir_Click);
            // 
            // cb_CreatePackage
            // 
            this.cb_CreatePackage.AutoSize = true;
            this.cb_CreatePackage.Checked = true;
            this.cb_CreatePackage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_CreatePackage.Location = new System.Drawing.Point(89, 260);
            this.cb_CreatePackage.Name = "cb_CreatePackage";
            this.cb_CreatePackage.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cb_CreatePackage.Size = new System.Drawing.Size(107, 19);
            this.cb_CreatePackage.TabIndex = 16;
            this.cb_CreatePackage.Text = "Create Package";
            this.cb_CreatePackage.UseVisualStyleBackColor = true;
            // 
            // MainFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 561);
            this.Controls.Add(this.cb_CreatePackage);
            this.Controls.Add(this.btn_OutputDir);
            this.Controls.Add(this.tb_OutputDir);
            this.Controls.Add(this.lbl_OutputDir);
            this.Controls.Add(this.cb_StrictIncludes);
            this.Controls.Add(this.tb_OutputLog);
            this.Controls.Add(this.btn_RemoveBatchFile);
            this.Controls.Add(this.lbl_BuildBatchFiles);
            this.Controls.Add(this.btn_AddBatchFile);
            this.Controls.Add(this.clb_BuildBatchFiles);
            this.Controls.Add(this.btn_PickPluginPath);
            this.Controls.Add(this.lbl_PluginPath);
            this.Controls.Add(this.tb_PluginPath);
            this.Controls.Add(this.btn_PickProjectPath);
            this.Controls.Add(this.lbl_ProjectPath);
            this.Controls.Add(this.tb_ProjectPath);
            this.Controls.Add(this.btn_Build);
            this.Name = "MainFrame";
            this.Text = "Unreal Plugin Builder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFrame_FormClosing);
            this.Load += new System.EventHandler(this.MainFrame_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Build;
        private System.Windows.Forms.TextBox tb_ProjectPath;
        private System.Windows.Forms.Label lbl_ProjectPath;
        private System.Windows.Forms.Button btn_PickProjectPath;
        private System.Windows.Forms.Button btn_PickPluginPath;
        private System.Windows.Forms.Label lbl_PluginPath;
        private System.Windows.Forms.TextBox tb_PluginPath;
        private System.Windows.Forms.CheckedListBox clb_BuildBatchFiles;
        private System.Windows.Forms.Button btn_AddBatchFile;
        private System.Windows.Forms.Label lbl_BuildBatchFiles;
        private System.Windows.Forms.Button btn_RemoveBatchFile;
        private System.Windows.Forms.TextBox tb_OutputLog;
        private System.Windows.Forms.CheckBox cb_StrictIncludes;
        private System.Windows.Forms.Label lbl_OutputDir;
        private System.Windows.Forms.TextBox tb_OutputDir;
        private System.Windows.Forms.Button btn_OutputDir;
        private System.Windows.Forms.CheckBox cb_CreatePackage;
    }
}