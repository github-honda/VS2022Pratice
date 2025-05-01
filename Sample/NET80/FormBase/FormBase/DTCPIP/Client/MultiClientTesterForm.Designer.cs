namespace FormBase.DTCPIP.Client
{
    partial class MultiClientTesterForm
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
            btnStart = new Button();
            txtServerIp = new TextBox();
            txtServerPort = new TextBox();
            txtClientCount = new TextBox();
            listBoxLog = new ListBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtMessagesPerClient = new TextBox();
            label4 = new Label();
            lblStatus = new Label();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Location = new Point(319, 79);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(94, 29);
            btnStart.TabIndex = 0;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // txtServerIp
            // 
            txtServerIp.Location = new Point(69, 15);
            txtServerIp.Name = "txtServerIp";
            txtServerIp.Size = new Size(125, 27);
            txtServerIp.TabIndex = 1;
            // 
            // txtServerPort
            // 
            txtServerPort.Location = new Point(288, 15);
            txtServerPort.Name = "txtServerPort";
            txtServerPort.Size = new Size(125, 27);
            txtServerPort.TabIndex = 2;
            // 
            // txtClientCount
            // 
            txtClientCount.Location = new Point(115, 48);
            txtClientCount.Name = "txtClientCount";
            txtClientCount.Size = new Size(125, 27);
            txtClientCount.TabIndex = 3;
            // 
            // listBoxLog
            // 
            listBoxLog.FormattingEnabled = true;
            listBoxLog.ItemHeight = 19;
            listBoxLog.Location = new Point(12, 114);
            listBoxLog.Name = "listBoxLog";
            listBoxLog.Size = new Size(776, 270);
            listBoxLog.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 19);
            label1.Name = "label1";
            label1.Size = new Size(22, 19);
            label1.TabIndex = 5;
            label1.Text = "IP";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(231, 19);
            label2.Name = "label2";
            label2.Size = new Size(38, 19);
            label2.TabIndex = 6;
            label2.Text = "Port";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 51);
            label3.Name = "label3";
            label3.Size = new Size(95, 19);
            label3.TabIndex = 7;
            label3.Text = "Client Count";
            // 
            // txtMessagesPerClient
            // 
            txtMessagesPerClient.Location = new Point(115, 81);
            txtMessagesPerClient.Name = "txtMessagesPerClient";
            txtMessagesPerClient.Size = new Size(125, 27);
            txtMessagesPerClient.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 85);
            label4.Name = "label4";
            label4.Size = new Size(108, 19);
            label4.TabIndex = 9;
            label4.Text = "Msg per client";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(442, 84);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(108, 19);
            lblStatus.TabIndex = 10;
            lblStatus.Text = "Msg per client";
            // 
            // MultiClientTesterForm
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 400);
            Controls.Add(lblStatus);
            Controls.Add(label4);
            Controls.Add(txtMessagesPerClient);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(listBoxLog);
            Controls.Add(txtClientCount);
            Controls.Add(txtServerPort);
            Controls.Add(txtServerIp);
            Controls.Add(btnStart);
            Name = "MultiClientTesterForm";
            Text = "MultiClientTesterForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStart;
        private TextBox txtServerIp;
        private TextBox txtServerPort;
        private TextBox txtClientCount;
        private ListBox listBoxLog;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtMessagesPerClient;
        private Label label4;
        private Label lblStatus;
    }
}