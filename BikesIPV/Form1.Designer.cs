using System.Drawing;
using System.Windows.Forms;

namespace BikesIPV
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            this.imageBox2 = new Emgu.CV.UI.ImageBox();
            this.leftRightCombo = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.problems = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_next = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btn_help = new System.Windows.Forms.Button();
            this.pb_tutorial = new System.Windows.Forms.PictureBox();
            this.button4 = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_tutorial)).BeginInit();
            this.SuspendLayout();
            // 
            // imageBox1
            // 
            this.imageBox1.Location = new System.Drawing.Point(12, 101);
            this.imageBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(593, 455);
            this.imageBox1.TabIndex = 2;
            this.imageBox1.TabStop = false;
            // 
            // imageBox2
            // 
            this.imageBox2.Location = new System.Drawing.Point(620, 101);
            this.imageBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.imageBox2.Name = "imageBox2";
            this.imageBox2.Size = new System.Drawing.Size(560, 455);
            this.imageBox2.TabIndex = 2;
            this.imageBox2.TabStop = false;
            // 
            // leftRightCombo
            // 
            this.leftRightCombo.FormattingEnabled = true;
            this.leftRightCombo.Items.AddRange(new object[] {
            "Left",
            "Right"});
            this.leftRightCombo.Location = new System.Drawing.Point(6, 19);
            this.leftRightCombo.Name = "leftRightCombo";
            this.leftRightCombo.Size = new System.Drawing.Size(235, 21);
            this.leftRightCombo.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 46);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(235, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Use the camera";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.comboBox2);
            this.groupBox1.Controls.Add(this.leftRightCombo);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1168, 84);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Get Started";
            // 
            // problems
            // 
            this.problems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.problems.FormattingEnabled = true;
            this.problems.Items.AddRange(new object[] {
            "Flat Front Tire",
            "Flat Back Tire",
            "Crank Replacement"});
            this.problems.Location = new System.Drawing.Point(126, 589);
            this.problems.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.problems.Name = "problems";
            this.problems.Size = new System.Drawing.Size(142, 21);
            this.problems.TabIndex = 8;
            this.problems.SelectedIndexChanged += new System.EventHandler(this.problems_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 591);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "What is the problem?";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_next);
            this.groupBox2.Controls.Add(this.richTextBox1);
            this.groupBox2.Location = new System.Drawing.Point(287, 576);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(518, 116);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Fix It Description";
            // 
            // btn_next
            // 
            this.btn_next.Location = new System.Drawing.Point(445, 20);
            this.btn_next.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_next.Name = "btn_next";
            this.btn_next.Size = new System.Drawing.Size(68, 25);
            this.btn_next.TabIndex = 1;
            this.btn_next.Text = "Next";
            this.btn_next.UseVisualStyleBackColor = true;
            this.btn_next.Click += new System.EventHandler(this.btn_next_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Enabled = false;
            this.richTextBox1.Location = new System.Drawing.Point(14, 20);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(428, 75);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // btn_help
            // 
            this.btn_help.Location = new System.Drawing.Point(126, 624);
            this.btn_help.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_help.Name = "btn_help";
            this.btn_help.Size = new System.Drawing.Size(93, 19);
            this.btn_help.TabIndex = 11;
            this.btn_help.Text = "Help Me!";
            this.btn_help.UseVisualStyleBackColor = true;
            this.btn_help.Click += new System.EventHandler(this.btn_help_Click);
            // 
            // pb_tutorial
            // 
            this.pb_tutorial.Location = new System.Drawing.Point(821, 585);
            this.pb_tutorial.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pb_tutorial.Name = "pb_tutorial";
            this.pb_tutorial.Size = new System.Drawing.Size(184, 107);
            this.pb_tutorial.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_tutorial.TabIndex = 12;
            this.pb_tutorial.TabStop = false;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(303, 46);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(199, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "Process Images";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(303, 19);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(199, 21);
            this.comboBox2.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Location = new System.Drawing.Point(247, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(49, 50);
            this.panel1.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1193, 715);
            this.Controls.Add(this.pb_tutorial);
            this.Controls.Add(this.btn_help);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.problems);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.imageBox2);
            this.Controls.Add(this.imageBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_tutorial)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox imageBox1;
        private Emgu.CV.UI.ImageBox imageBox2;
        private System.Windows.Forms.ComboBox leftRightCombo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox problems;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_next;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btn_help;
        private System.Windows.Forms.PictureBox pb_tutorial;
        private Panel panel1;
        private ComboBox comboBox2;
        private Button button4;
    }
}

