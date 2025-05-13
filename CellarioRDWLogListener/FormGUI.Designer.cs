namespace CellarioRDWLogListener
{
    partial class FormGUI
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
            components = new System.ComponentModel.Container();
            txtLogFile = new TextBox();
            btnBrowseLog = new Button();
            txtResultsFolder = new TextBox();
            btnBrowseResults = new Button();
            textConsole = new RichTextBox();
            contextMenuConsole = new ContextMenuStrip(components);
            clearConsole = new ToolStripMenuItem();
            btnStart = new Button();
            btnStop = new Button();
            lblStatus = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            contextMenuConsole.SuspendLayout();
            SuspendLayout();
            // 
            // txtLogFile
            // 
            txtLogFile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtLogFile.Location = new Point(201, 12);
            txtLogFile.Name = "txtLogFile";
            txtLogFile.Size = new Size(373, 27);
            txtLogFile.TabIndex = 1;
            // 
            // btnBrowseLog
            // 
            btnBrowseLog.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBrowseLog.Location = new Point(581, 10);
            btnBrowseLog.Name = "btnBrowseLog";
            btnBrowseLog.Size = new Size(94, 32);
            btnBrowseLog.TabIndex = 2;
            btnBrowseLog.Text = "Browse";
            btnBrowseLog.UseVisualStyleBackColor = true;
            btnBrowseLog.Click += btnBrowseLogFile;
            // 
            // txtResultsFolder
            // 
            txtResultsFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtResultsFolder.Location = new Point(201, 49);
            txtResultsFolder.Name = "txtResultsFolder";
            txtResultsFolder.Size = new Size(373, 27);
            txtResultsFolder.TabIndex = 4;
            // 
            // btnBrowseResults
            // 
            btnBrowseResults.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBrowseResults.Location = new Point(581, 48);
            btnBrowseResults.Name = "btnBrowseResults";
            btnBrowseResults.Size = new Size(94, 32);
            btnBrowseResults.TabIndex = 5;
            btnBrowseResults.Text = "Browse";
            btnBrowseResults.UseVisualStyleBackColor = true;
            btnBrowseResults.Click += btnBrowseResultsFolder;
            // 
            // textConsole
            // 
            textConsole.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textConsole.BackColor = Color.FromArgb(202, 235, 235);
            textConsole.ContextMenuStrip = contextMenuConsole;
            textConsole.ForeColor = Color.Gray;
            textConsole.Location = new Point(12, 86);
            textConsole.Name = "textConsole";
            textConsole.ReadOnly = true;
            textConsole.ScrollBars = RichTextBoxScrollBars.Vertical;
            textConsole.Size = new Size(913, 344);
            textConsole.TabIndex = 6;
            textConsole.Text = "";
            // 
            // contextMenuConsole
            // 
            contextMenuConsole.ImageScalingSize = new Size(20, 20);
            contextMenuConsole.Items.AddRange(new ToolStripItem[] { clearConsole });
            contextMenuConsole.Name = "contextMenuConsole";
            contextMenuConsole.Size = new Size(203, 28);
            // 
            // clearConsole
            // 
            clearConsole.Name = "clearConsole";
            clearConsole.Size = new Size(202, 24);
            clearConsole.Text = "Clear All Messages";
            clearConsole.Click += clearConsole_Click;
            // 
            // btnStart
            // 
            btnStart.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnStart.BackColor = Color.FromArgb(0, 192, 0);
            btnStart.Location = new Point(681, 10);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(120, 70);
            btnStart.TabIndex = 7;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStartListening;
            // 
            // btnStop
            // 
            btnStop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnStop.BackColor = Color.Red;
            btnStop.Location = new Point(807, 10);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(118, 70);
            btnStop.TabIndex = 8;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += btnStopListening;
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblStatus.Location = new Point(861, 433);
            lblStatus.Name = "lblStatus";
            lblStatus.RightToLeft = RightToLeft.Yes;
            lblStatus.Size = new Size(67, 20);
            lblStatus.TabIndex = 9;
            lblStatus.Text = "Idle        ";
            lblStatus.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(184, 20);
            label1.TabIndex = 10;
            label1.Text = "RDW Log File (to listen to)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 51);
            label2.Name = "label2";
            label2.Size = new Size(133, 20);
            label2.TabIndex = 11;
            label2.Text = "Results Folder Path";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Location = new Point(7, 433);
            label3.Name = "label3";
            label3.Size = new Size(169, 20);
            label3.TabIndex = 12;
            label3.Text = "LAF, D-BSSE, ETH Zürich";
            // 
            // FormGUI
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(937, 459);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lblStatus);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(textConsole);
            Controls.Add(btnBrowseResults);
            Controls.Add(txtResultsFolder);
            Controls.Add(btnBrowseLog);
            Controls.Add(txtLogFile);
            Name = "FormGUI";
            Text = "Remote Driver Wrapper Log Listener";
            contextMenuConsole.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtLogFile;
        private Button btnBrowseLog;
        private TextBox txtResultsFolder;
        private Button btnBrowseResults;
        private RichTextBox textConsole;
        private Button btnStart;
        private Button btnStop;
        private Label lblStatus;
        private Label label1;
        private Label label2;
        private ContextMenuStrip contextMenuConsole;
        private ToolStripMenuItem clearConsole;
        private Label label3;
    }
}