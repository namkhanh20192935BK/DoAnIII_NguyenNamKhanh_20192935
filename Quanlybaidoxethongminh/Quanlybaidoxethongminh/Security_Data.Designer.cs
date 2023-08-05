namespace Quanlybaidoxethongminh
{
    partial class Security_Data
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
            this.textBox_inputPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button_checkCode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_inputPassword
            // 
            this.textBox_inputPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_inputPassword.Location = new System.Drawing.Point(46, 75);
            this.textBox_inputPassword.Name = "textBox_inputPassword";
            this.textBox_inputPassword.PasswordChar = '*';
            this.textBox_inputPassword.Size = new System.Drawing.Size(217, 22);
            this.textBox_inputPassword.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(43, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mật khẩu xác thực:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(43, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(233, 30);
            this.label2.TabIndex = 2;
            this.label2.Text = "Vui lòng nhập mật khẩu xác thực mà bạn \r\nđã được cấp để chỉnh sửa thông tin!";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // button_checkCode
            // 
            this.button_checkCode.Location = new System.Drawing.Point(46, 162);
            this.button_checkCode.Name = "button_checkCode";
            this.button_checkCode.Size = new System.Drawing.Size(104, 36);
            this.button_checkCode.TabIndex = 3;
            this.button_checkCode.Text = "Xác thực";
            this.button_checkCode.UseVisualStyleBackColor = true;
            this.button_checkCode.Click += new System.EventHandler(this.button_checkCode_Click);
            // 
            // Security_Data
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 223);
            this.Controls.Add(this.button_checkCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_inputPassword);
            this.Name = "Security_Data";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Security_Data";
            this.Load += new System.EventHandler(this.Security_Data_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_inputPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_checkCode;
    }
}