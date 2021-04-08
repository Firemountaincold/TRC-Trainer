
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttondata = new System.Windows.Forms.Button();
            this.buttontrain = new System.Windows.Forms.Button();
            this.buttonmodpath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxpath = new System.Windows.Forms.TextBox();
            this.textBoxinfo = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // buttondata
            // 
            this.buttondata.Location = new System.Drawing.Point(13, 13);
            this.buttondata.Name = "buttondata";
            this.buttondata.Size = new System.Drawing.Size(94, 29);
            this.buttondata.TabIndex = 1;
            this.buttondata.Text = "选择数据";
            this.buttondata.UseVisualStyleBackColor = true;
            this.buttondata.Click += new System.EventHandler(this.buttondata_Click);
            // 
            // buttontrain
            // 
            this.buttontrain.Location = new System.Drawing.Point(113, 13);
            this.buttontrain.Name = "buttontrain";
            this.buttontrain.Size = new System.Drawing.Size(94, 29);
            this.buttontrain.TabIndex = 2;
            this.buttontrain.Text = "训练模型";
            this.buttontrain.UseVisualStyleBackColor = true;
            this.buttontrain.Click += new System.EventHandler(this.buttontrain_Click);
            // 
            // buttonmodpath
            // 
            this.buttonmodpath.Location = new System.Drawing.Point(730, 13);
            this.buttonmodpath.Name = "buttonmodpath";
            this.buttonmodpath.Size = new System.Drawing.Size(58, 29);
            this.buttonmodpath.TabIndex = 3;
            this.buttonmodpath.Text = "浏览";
            this.buttonmodpath.UseVisualStyleBackColor = true;
            this.buttonmodpath.Click += new System.EventHandler(this.buttonmodpath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(213, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "模型保存位置：";
            // 
            // textBoxpath
            // 
            this.textBoxpath.Location = new System.Drawing.Point(333, 14);
            this.textBoxpath.Name = "textBoxpath";
            this.textBoxpath.Size = new System.Drawing.Size(391, 27);
            this.textBoxpath.TabIndex = 5;
            // 
            // textBoxinfo
            // 
            this.textBoxinfo.Location = new System.Drawing.Point(13, 49);
            this.textBoxinfo.Name = "textBoxinfo";
            this.textBoxinfo.Size = new System.Drawing.Size(775, 389);
            this.textBoxinfo.TabIndex = 6;
            this.textBoxinfo.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBoxinfo);
            this.Controls.Add(this.textBoxpath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonmodpath);
            this.Controls.Add(this.buttontrain);
            this.Controls.Add(this.buttondata);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "TRC Trainer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
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
    }
}

