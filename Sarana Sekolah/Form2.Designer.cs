namespace Sarana_Sekolah
{
    partial class Form2
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
            this.btnTestKoneksi_Click = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTestKoneksi_Click
            // 
            this.btnTestKoneksi_Click.Location = new System.Drawing.Point(327, 172);
            this.btnTestKoneksi_Click.Name = "btnTestKoneksi_Click";
            this.btnTestKoneksi_Click.Size = new System.Drawing.Size(96, 44);
            this.btnTestKoneksi_Click.TabIndex = 0;
            this.btnTestKoneksi_Click.Text = "button1";
            this.btnTestKoneksi_Click.UseVisualStyleBackColor = true;
            this.btnTestKoneksi_Click.Click += new System.EventHandler(this.btnTestKoneksi_Click_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnTestKoneksi_Click);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTestKoneksi_Click;
    }
}