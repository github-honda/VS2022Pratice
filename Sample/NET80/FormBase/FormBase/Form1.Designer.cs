namespace FormBase
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
            textBox1 = new TextBox();
            button1 = new Button();
            btnMarshal = new Button();
            button2 = new Button();
            menuStrip1 = new MenuStrip();
            statusStrip1 = new StatusStrip();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(115, 185);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(420, 27);
            textBox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(83, 218);
            button1.Name = "button1";
            button1.Size = new Size(177, 39);
            button1.TabIndex = 1;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // btnMarshal
            // 
            btnMarshal.Location = new Point(294, 231);
            btnMarshal.Name = "btnMarshal";
            btnMarshal.Size = new Size(177, 39);
            btnMarshal.TabIndex = 2;
            btnMarshal.Text = "Marshal";
            btnMarshal.UseVisualStyleBackColor = true;
            btnMarshal.Click += btnMarshal_Click;
            // 
            // button2
            // 
            button2.Location = new Point(111, 263);
            button2.Name = "button2";
            button2.Size = new Size(177, 39);
            button2.TabIndex = 3;
            button2.Text = "GetBytes";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 4;
            menuStrip1.Text = "menuStrip1";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Location = new Point(0, 428);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(800, 22);
            statusStrip1.TabIndex = 5;
            statusStrip1.Text = "statusStrip1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(statusStrip1);
            Controls.Add(button2);
            Controls.Add(btnMarshal);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Button button1;
        private Button btnMarshal;
        private Button button2;
        private MenuStrip menuStrip1;
        private StatusStrip statusStrip1;
    }
}
