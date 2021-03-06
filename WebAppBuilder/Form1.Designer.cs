
namespace WebAppBuilder
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.txMainFiles = new System.Windows.Forms.TextBox();
            this.btnLoadMainFiles = new System.Windows.Forms.Button();
            this.btnGenerateLocalWebApp = new System.Windows.Forms.Button();
            this.btnObfuscated = new System.Windows.Forms.Button();
            this.txExckudeJS = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExcludeFromObfuscate = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txObfuscatedJS = new System.Windows.Forms.TextBox();
            this.btnMinimized = new System.Windows.Forms.Button();
            this.chbAutoMinify = new System.Windows.Forms.CheckBox();
            this.btnBase64 = new System.Windows.Forms.Button();
            this.btnLauncher = new System.Windows.Forms.Button();
            this.btnSafeWords = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Main Files";
            // 
            // txMainFiles
            // 
            this.txMainFiles.Location = new System.Drawing.Point(80, 105);
            this.txMainFiles.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txMainFiles.Multiline = true;
            this.txMainFiles.Name = "txMainFiles";
            this.txMainFiles.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txMainFiles.Size = new System.Drawing.Size(743, 85);
            this.txMainFiles.TabIndex = 3;
            // 
            // btnLoadMainFiles
            // 
            this.btnLoadMainFiles.Location = new System.Drawing.Point(747, 65);
            this.btnLoadMainFiles.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLoadMainFiles.Name = "btnLoadMainFiles";
            this.btnLoadMainFiles.Size = new System.Drawing.Size(86, 31);
            this.btnLoadMainFiles.TabIndex = 6;
            this.btnLoadMainFiles.Text = "Load";
            this.btnLoadMainFiles.UseVisualStyleBackColor = true;
            this.btnLoadMainFiles.Click += new System.EventHandler(this.btnLoadMainFiles_Click);
            // 
            // btnGenerateLocalWebApp
            // 
            this.btnGenerateLocalWebApp.Location = new System.Drawing.Point(73, 833);
            this.btnGenerateLocalWebApp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnGenerateLocalWebApp.Name = "btnGenerateLocalWebApp";
            this.btnGenerateLocalWebApp.Size = new System.Drawing.Size(166, 31);
            this.btnGenerateLocalWebApp.TabIndex = 8;
            this.btnGenerateLocalWebApp.Text = "Generate Local App";
            this.btnGenerateLocalWebApp.UseVisualStyleBackColor = true;
            this.btnGenerateLocalWebApp.Click += new System.EventHandler(this.btnGenerateLocalWebApp_Click);
            // 
            // btnObfuscated
            // 
            this.btnObfuscated.Location = new System.Drawing.Point(646, 833);
            this.btnObfuscated.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnObfuscated.Name = "btnObfuscated";
            this.btnObfuscated.Size = new System.Drawing.Size(160, 31);
            this.btnObfuscated.TabIndex = 9;
            this.btnObfuscated.Text = "Add Obfuscated JS";
            this.btnObfuscated.UseVisualStyleBackColor = true;
            this.btnObfuscated.Click += new System.EventHandler(this.btnObfuscated_Click);
            // 
            // txExckudeJS
            // 
            this.txExckudeJS.Location = new System.Drawing.Point(80, 276);
            this.txExckudeJS.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txExckudeJS.Multiline = true;
            this.txExckudeJS.Name = "txExckudeJS";
            this.txExckudeJS.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txExckudeJS.Size = new System.Drawing.Size(743, 192);
            this.txExckudeJS.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(80, 239);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "Exclude JS From obfuscate";
            // 
            // btnExcludeFromObfuscate
            // 
            this.btnExcludeFromObfuscate.Location = new System.Drawing.Point(719, 233);
            this.btnExcludeFromObfuscate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExcludeFromObfuscate.Name = "btnExcludeFromObfuscate";
            this.btnExcludeFromObfuscate.Size = new System.Drawing.Size(114, 31);
            this.btnExcludeFromObfuscate.TabIndex = 12;
            this.btnExcludeFromObfuscate.Text = "Excluded JS";
            this.btnExcludeFromObfuscate.UseVisualStyleBackColor = true;
            this.btnExcludeFromObfuscate.Click += new System.EventHandler(this.btnExcludeFromObfuscate_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(73, 503);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 20);
            this.label3.TabIndex = 14;
            this.label3.Text = "Obfuscated JS";
            // 
            // txObfuscatedJS
            // 
            this.txObfuscatedJS.Location = new System.Drawing.Point(73, 540);
            this.txObfuscatedJS.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txObfuscatedJS.Multiline = true;
            this.txObfuscatedJS.Name = "txObfuscatedJS";
            this.txObfuscatedJS.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txObfuscatedJS.Size = new System.Drawing.Size(743, 192);
            this.txObfuscatedJS.TabIndex = 13;
            // 
            // btnMinimized
            // 
            this.btnMinimized.Location = new System.Drawing.Point(272, 833);
            this.btnMinimized.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnMinimized.Name = "btnMinimized";
            this.btnMinimized.Size = new System.Drawing.Size(127, 31);
            this.btnMinimized.TabIndex = 15;
            this.btnMinimized.Text = "Minimized Step";
            this.btnMinimized.UseVisualStyleBackColor = true;
            this.btnMinimized.Click += new System.EventHandler(this.btnMinimized_Click);
            // 
            // chbAutoMinify
            // 
            this.chbAutoMinify.AutoSize = true;
            this.chbAutoMinify.Location = new System.Drawing.Point(80, 756);
            this.chbAutoMinify.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chbAutoMinify.Name = "chbAutoMinify";
            this.chbAutoMinify.Size = new System.Drawing.Size(108, 24);
            this.chbAutoMinify.TabIndex = 16;
            this.chbAutoMinify.Text = "Auto Minify";
            this.chbAutoMinify.UseVisualStyleBackColor = true;
            // 
            // btnBase64
            // 
            this.btnBase64.Location = new System.Drawing.Point(439, 832);
            this.btnBase64.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBase64.Name = "btnBase64";
            this.btnBase64.Size = new System.Drawing.Size(86, 31);
            this.btnBase64.TabIndex = 17;
            this.btnBase64.Text = "Base64";
            this.btnBase64.UseVisualStyleBackColor = true;
            this.btnBase64.Click += new System.EventHandler(this.btnBase64_Click);
            // 
            // btnLauncher
            // 
            this.btnLauncher.Location = new System.Drawing.Point(493, 67);
            this.btnLauncher.Name = "btnLauncher";
            this.btnLauncher.Size = new System.Drawing.Size(94, 29);
            this.btnLauncher.TabIndex = 18;
            this.btnLauncher.Text = "Launcher";
            this.btnLauncher.UseVisualStyleBackColor = true;
            this.btnLauncher.Click += new System.EventHandler(this.btnLauncher_Click);
            // 
            // btnSafeWords
            // 
            this.btnSafeWords.Location = new System.Drawing.Point(611, 69);
            this.btnSafeWords.Name = "btnSafeWords";
            this.btnSafeWords.Size = new System.Drawing.Size(130, 29);
            this.btnSafeWords.TabIndex = 19;
            this.btnSafeWords.Text = "SafeWords";
            this.btnSafeWords.UseVisualStyleBackColor = true;
            this.btnSafeWords.Click += new System.EventHandler(this.btnSafeWords_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 917);
            this.Controls.Add(this.btnSafeWords);
            this.Controls.Add(this.btnLauncher);
            this.Controls.Add(this.btnBase64);
            this.Controls.Add(this.chbAutoMinify);
            this.Controls.Add(this.btnMinimized);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txObfuscatedJS);
            this.Controls.Add(this.btnExcludeFromObfuscate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txExckudeJS);
            this.Controls.Add(this.btnObfuscated);
            this.Controls.Add(this.btnGenerateLocalWebApp);
            this.Controls.Add(this.btnLoadMainFiles);
            this.Controls.Add(this.txMainFiles);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Web App Packer Vsn 1.3";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txMainFiles;
        private System.Windows.Forms.Button btnLoadMainFiles;
        private System.Windows.Forms.Button btnGenerateLocalWebApp;
        private System.Windows.Forms.Button btnObfuscated;
        private System.Windows.Forms.TextBox txExckudeJS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnExcludeFromObfuscate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txObfuscatedJS;
        private System.Windows.Forms.Button btnMinimized;
        private System.Windows.Forms.CheckBox chbAutoMinify;
        private System.Windows.Forms.Button btnBase64;
        private System.Windows.Forms.Button btnLauncher;
        private System.Windows.Forms.Button btnSafeWords;
    }
}

