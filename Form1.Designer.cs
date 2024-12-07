namespace AIAppBuilder_v0
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
            btnLoad = new Button();
            btnGenerate = new Button();
            txtPrompt = new TextBox();
            rtbOutput = new RichTextBox();
            bntTestCode = new Button();
            SuspendLayout();
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(108, 44);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(191, 47);
            btnLoad.TabIndex = 0;
            btnLoad.Text = "Load Document";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(825, 130);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(205, 86);
            btnGenerate.TabIndex = 2;
            btnGenerate.Text = "Generate Code";
            btnGenerate.UseVisualStyleBackColor = true;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // txtPrompt
            // 
            txtPrompt.Location = new Point(94, 137);
            txtPrompt.Multiline = true;
            txtPrompt.Name = "txtPrompt";
            txtPrompt.PlaceholderText = "Enter your prompt here...";
            txtPrompt.ScrollBars = ScrollBars.Vertical;
            txtPrompt.Size = new Size(618, 160);
            txtPrompt.TabIndex = 1;
            // 
            // rtbOutput
            // 
            rtbOutput.Location = new Point(142, 445);
            rtbOutput.Name = "rtbOutput";
            rtbOutput.ReadOnly = true;
            rtbOutput.Size = new Size(1181, 345);
            rtbOutput.TabIndex = 3;
            rtbOutput.Text = "";
            // 
            // bntTestCode
            // 
            bntTestCode.Location = new Point(825, 260);
            bntTestCode.Name = "bntTestCode";
            bntTestCode.Size = new Size(202, 80);
            bntTestCode.TabIndex = 4;
            bntTestCode.Text = "Test Code";
            bntTestCode.UseVisualStyleBackColor = true;
            bntTestCode.Click += bntTestCode_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1380, 938);
            Controls.Add(bntTestCode);
            Controls.Add(rtbOutput);
            Controls.Add(btnGenerate);
            Controls.Add(txtPrompt);
            Controls.Add(btnLoad);
            Name = "Form1";
            Text = "OpenAI RAG Desktop App";
            FormClosing += Form1_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnLoad;
        private Button btnGenerate;
        private TextBox txtPrompt;
        private RichTextBox rtbOutput;
        private Button bntTestCode;
    }
}
