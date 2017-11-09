namespace Crozzle
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
            this.addressesCB = new System.Windows.Forms.ComboBox();
            this.getCrozzleBTN = new System.Windows.Forms.Button();
            this.newAddressTB = new System.Windows.Forms.TextBox();
            this.addCrozzleBTN = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // addressesCB
            // 
            this.addressesCB.FormattingEnabled = true;
            this.addressesCB.Location = new System.Drawing.Point(12, 33);
            this.addressesCB.Name = "addressesCB";
            this.addressesCB.Size = new System.Drawing.Size(422, 21);
            this.addressesCB.TabIndex = 1;
            // 
            // getCrozzleBTN
            // 
            this.getCrozzleBTN.Location = new System.Drawing.Point(458, 33);
            this.getCrozzleBTN.Name = "getCrozzleBTN";
            this.getCrozzleBTN.Size = new System.Drawing.Size(127, 23);
            this.getCrozzleBTN.TabIndex = 2;
            this.getCrozzleBTN.Text = "Compute  Crozzle";
            this.getCrozzleBTN.UseVisualStyleBackColor = true;
            this.getCrozzleBTN.Click += new System.EventHandler(this.getCrozzleBTN_Click);
            // 
            // newAddressTB
            // 
            this.newAddressTB.Location = new System.Drawing.Point(12, 72);
            this.newAddressTB.Name = "newAddressTB";
            this.newAddressTB.Size = new System.Drawing.Size(422, 20);
            this.newAddressTB.TabIndex = 4;
            // 
            // addCrozzleBTN
            // 
            this.addCrozzleBTN.Location = new System.Drawing.Point(458, 72);
            this.addCrozzleBTN.Name = "addCrozzleBTN";
            this.addCrozzleBTN.Size = new System.Drawing.Size(127, 23);
            this.addCrozzleBTN.TabIndex = 5;
            this.addCrozzleBTN.Text = "Add new Crozzle";
            this.addCrozzleBTN.UseVisualStyleBackColor = true;
            this.addCrozzleBTN.Click += new System.EventHandler(this.addCrozzleBTN_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(12, 109);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(1101, 434);
            this.webBrowser1.TabIndex = 6;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "new Crozzle";
            this.saveFileDialog1.Title = "Save Crozzle";
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(612, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Save Crozzle";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(612, 71);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(109, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Open Crozzle file";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 555);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.addCrozzleBTN);
            this.Controls.Add(this.newAddressTB);
            this.Controls.Add(this.getCrozzleBTN);
            this.Controls.Add(this.addressesCB);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox addressesCB;
        private System.Windows.Forms.Button getCrozzleBTN;
        private System.Windows.Forms.TextBox newAddressTB;
        private System.Windows.Forms.Button addCrozzleBTN;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button2;
    }
}

