
namespace TRC_Trainer
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
            this.buttondata = new System.Windows.Forms.Button();
            this.buttontrain = new System.Windows.Forms.Button();
            this.buttonmodpath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxpath = new System.Windows.Forms.TextBox();
            this.textBoxinfo = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxcuda = new System.Windows.Forms.TextBox();
            this.buttonexit = new System.Windows.Forms.Button();
            this.radioButtonnet = new System.Windows.Forms.RadioButton();
            this.radioButtonpy = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxtrainiter = new System.Windows.Forms.TextBox();
            this.textBoxtestiter = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxlr = new System.Windows.Forms.TextBox();
            this.buttonstopsave = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxepoch = new System.Windows.Forms.TextBox();
            this.checkBoxupsample = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttondata
            // 
            this.buttondata.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.buttondata.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttondata.Location = new System.Drawing.Point(9, 14);
            this.buttondata.Margin = new System.Windows.Forms.Padding(2);
            this.buttondata.Name = "buttondata";
            this.buttondata.Size = new System.Drawing.Size(90, 25);
            this.buttondata.TabIndex = 1;
            this.buttondata.Text = "选择数据";
            this.buttondata.UseVisualStyleBackColor = false;
            this.buttondata.Click += new System.EventHandler(this.buttondata_Click);
            // 
            // buttontrain
            // 
            this.buttontrain.BackColor = System.Drawing.Color.OldLace;
            this.buttontrain.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttontrain.Location = new System.Drawing.Point(379, 55);
            this.buttontrain.Margin = new System.Windows.Forms.Padding(2);
            this.buttontrain.Name = "buttontrain";
            this.buttontrain.Size = new System.Drawing.Size(177, 25);
            this.buttontrain.TabIndex = 2;
            this.buttontrain.Text = "训练模型";
            this.buttontrain.UseVisualStyleBackColor = false;
            this.buttontrain.Click += new System.EventHandler(this.buttontrain_Click);
            // 
            // buttonmodpath
            // 
            this.buttonmodpath.BackColor = System.Drawing.SystemColors.HighlightText;
            this.buttonmodpath.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonmodpath.Location = new System.Drawing.Point(517, 120);
            this.buttonmodpath.Margin = new System.Windows.Forms.Padding(2);
            this.buttonmodpath.Name = "buttonmodpath";
            this.buttonmodpath.Size = new System.Drawing.Size(39, 25);
            this.buttonmodpath.TabIndex = 3;
            this.buttonmodpath.Text = "浏览";
            this.buttonmodpath.UseVisualStyleBackColor = false;
            this.buttonmodpath.Click += new System.EventHandler(this.buttonmodpath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 126);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "模型保存位置：";
            // 
            // textBoxpath
            // 
            this.textBoxpath.Location = new System.Drawing.Point(104, 122);
            this.textBoxpath.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxpath.Name = "textBoxpath";
            this.textBoxpath.Size = new System.Drawing.Size(409, 21);
            this.textBoxpath.TabIndex = 5;
            // 
            // textBoxinfo
            // 
            this.textBoxinfo.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxinfo.Location = new System.Drawing.Point(9, 155);
            this.textBoxinfo.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxinfo.Name = "textBoxinfo";
            this.textBoxinfo.ReadOnly = true;
            this.textBoxinfo.Size = new System.Drawing.Size(547, 291);
            this.textBoxinfo.TabIndex = 6;
            this.textBoxinfo.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(242, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "CUDA:";
            // 
            // textBoxcuda
            // 
            this.textBoxcuda.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.textBoxcuda.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxcuda.Location = new System.Drawing.Point(285, 20);
            this.textBoxcuda.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxcuda.Name = "textBoxcuda";
            this.textBoxcuda.ReadOnly = true;
            this.textBoxcuda.Size = new System.Drawing.Size(45, 14);
            this.textBoxcuda.TabIndex = 8;
            this.textBoxcuda.Text = "不可用";
            // 
            // buttonexit
            // 
            this.buttonexit.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.buttonexit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonexit.Location = new System.Drawing.Point(466, 14);
            this.buttonexit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonexit.Name = "buttonexit";
            this.buttonexit.Size = new System.Drawing.Size(90, 25);
            this.buttonexit.TabIndex = 9;
            this.buttonexit.Text = "退出程序";
            this.buttonexit.UseVisualStyleBackColor = false;
            this.buttonexit.Click += new System.EventHandler(this.buttonexit_Click);
            // 
            // radioButtonnet
            // 
            this.radioButtonnet.AutoSize = true;
            this.radioButtonnet.Checked = true;
            this.radioButtonnet.Location = new System.Drawing.Point(116, 18);
            this.radioButtonnet.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButtonnet.Name = "radioButtonnet";
            this.radioButtonnet.Size = new System.Drawing.Size(47, 16);
            this.radioButtonnet.TabIndex = 10;
            this.radioButtonnet.TabStop = true;
            this.radioButtonnet.Text = ".NET";
            this.radioButtonnet.UseVisualStyleBackColor = true;
            // 
            // radioButtonpy
            // 
            this.radioButtonpy.AutoSize = true;
            this.radioButtonpy.Location = new System.Drawing.Point(167, 18);
            this.radioButtonpy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButtonpy.Name = "radioButtonpy";
            this.radioButtonpy.Size = new System.Drawing.Size(59, 16);
            this.radioButtonpy.TabIndex = 10;
            this.radioButtonpy.Text = "Python";
            this.radioButtonpy.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(169, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "测试迭代次数：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "训练迭代次数：";
            // 
            // textBoxtrainiter
            // 
            this.textBoxtrainiter.Location = new System.Drawing.Point(104, 57);
            this.textBoxtrainiter.Name = "textBoxtrainiter";
            this.textBoxtrainiter.Size = new System.Drawing.Size(59, 21);
            this.textBoxtrainiter.TabIndex = 12;
            this.textBoxtrainiter.Text = "1000";
            // 
            // textBoxtestiter
            // 
            this.textBoxtestiter.Location = new System.Drawing.Point(264, 57);
            this.textBoxtestiter.Name = "textBoxtestiter";
            this.textBoxtestiter.Size = new System.Drawing.Size(59, 21);
            this.textBoxtestiter.TabIndex = 12;
            this.textBoxtestiter.Text = "300";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "学习率：";
            // 
            // textBoxlr
            // 
            this.textBoxlr.Location = new System.Drawing.Point(104, 89);
            this.textBoxlr.Name = "textBoxlr";
            this.textBoxlr.Size = new System.Drawing.Size(56, 21);
            this.textBoxlr.TabIndex = 13;
            this.textBoxlr.Text = "0.0001";
            // 
            // buttonstopsave
            // 
            this.buttonstopsave.BackColor = System.Drawing.Color.SeaShell;
            this.buttonstopsave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonstopsave.Location = new System.Drawing.Point(379, 86);
            this.buttonstopsave.Name = "buttonstopsave";
            this.buttonstopsave.Size = new System.Drawing.Size(176, 24);
            this.buttonstopsave.TabIndex = 14;
            this.buttonstopsave.Text = "停止训练并保存模型";
            this.buttonstopsave.UseVisualStyleBackColor = false;
            this.buttonstopsave.Click += new System.EventHandler(this.buttonstopsave_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(169, 92);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "评估频率：";
            // 
            // textBoxepoch
            // 
            this.textBoxepoch.Location = new System.Drawing.Point(264, 89);
            this.textBoxepoch.Name = "textBoxepoch";
            this.textBoxepoch.Size = new System.Drawing.Size(59, 21);
            this.textBoxepoch.TabIndex = 16;
            this.textBoxepoch.Text = "10";
            // 
            // checkBoxupsample
            // 
            this.checkBoxupsample.AutoSize = true;
            this.checkBoxupsample.Location = new System.Drawing.Point(349, 18);
            this.checkBoxupsample.Name = "checkBoxupsample";
            this.checkBoxupsample.Size = new System.Drawing.Size(84, 16);
            this.checkBoxupsample.TabIndex = 17;
            this.checkBoxupsample.Text = "上采样评估";
            this.checkBoxupsample.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(567, 457);
            this.Controls.Add(this.checkBoxupsample);
            this.Controls.Add(this.textBoxepoch);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.buttonstopsave);
            this.Controls.Add(this.textBoxlr);
            this.Controls.Add(this.textBoxtestiter);
            this.Controls.Add(this.textBoxtrainiter);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.radioButtonpy);
            this.Controls.Add(this.radioButtonnet);
            this.Controls.Add(this.buttonexit);
            this.Controls.Add(this.textBoxcuda);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxinfo);
            this.Controls.Add(this.textBoxpath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonmodpath);
            this.Controls.Add(this.buttontrain);
            this.Controls.Add(this.buttondata);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "TRC Trainer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttondata;
        private System.Windows.Forms.Button buttontrain;
        private System.Windows.Forms.Button buttonmodpath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxpath;
        private System.Windows.Forms.RichTextBox textBoxinfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxcuda;
        private System.Windows.Forms.Button buttonexit;
        private System.Windows.Forms.RadioButton radioButtonnet;
        private System.Windows.Forms.RadioButton radioButtonpy;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxtrainiter;
        private System.Windows.Forms.TextBox textBoxtestiter;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxlr;
        private System.Windows.Forms.Button buttonstopsave;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxepoch;
        private System.Windows.Forms.CheckBox checkBoxupsample;
    }
}

