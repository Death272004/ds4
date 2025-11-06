namespace Parcial_2
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.texboxF1 = new System.Windows.Forms.TextBox();
            this.TextBoxC2 = new System.Windows.Forms.TextBox();
            this.TextBoxK3 = new System.Windows.Forms.TextBox();
            this.Fahrenheit_A = new System.Windows.Forms.Button();
            this.Celsius_A = new System.Windows.Forms.Button();
            this.Kelvin_A = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.texboxF1_1 = new System.Windows.Forms.TextBox();
            this.texboxF1_2 = new System.Windows.Forms.TextBox();
            this.texboxF1_3 = new System.Windows.Forms.TextBox();
            this.TextBoxC2_1 = new System.Windows.Forms.TextBox();
            this.TextBoxC2_2 = new System.Windows.Forms.TextBox();
            this.TextBoxC2_3 = new System.Windows.Forms.TextBox();
            this.TextBoxK3_1 = new System.Windows.Forms.TextBox();
            this.TextBoxK3_2 = new System.Windows.Forms.TextBox();
            this.TextBoxK3_3 = new System.Windows.Forms.TextBox();
            this.Historial = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fahrenheit:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Celsius:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Kelvin:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(300, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Fahrenheit";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(413, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Celsius";
            // 
            // texboxF1
            // 
            this.texboxF1.Location = new System.Drawing.Point(90, 47);
            this.texboxF1.Name = "texboxF1";
            this.texboxF1.Size = new System.Drawing.Size(100, 20);
            this.texboxF1.TabIndex = 5;
            // 
            // TextBoxC2
            // 
            this.TextBoxC2.Location = new System.Drawing.Point(90, 74);
            this.TextBoxC2.Name = "TextBoxC2";
            this.TextBoxC2.Size = new System.Drawing.Size(100, 20);
            this.TextBoxC2.TabIndex = 6;
            // 
            // TextBoxK3
            // 
            this.TextBoxK3.Location = new System.Drawing.Point(90, 102);
            this.TextBoxK3.Name = "TextBoxK3";
            this.TextBoxK3.Size = new System.Drawing.Size(100, 20);
            this.TextBoxK3.TabIndex = 7;
            // 
            // Fahrenheit_A
            // 
            this.Fahrenheit_A.Location = new System.Drawing.Point(197, 44);
            this.Fahrenheit_A.Name = "Fahrenheit_A";
            this.Fahrenheit_A.Size = new System.Drawing.Size(75, 23);
            this.Fahrenheit_A.TabIndex = 8;
            this.Fahrenheit_A.Text = "->";
            this.Fahrenheit_A.UseVisualStyleBackColor = true;
            this.Fahrenheit_A.Click += new System.EventHandler(this.Fahrenheit_A_Click);
            // 
            // Celsius_A
            // 
            this.Celsius_A.Location = new System.Drawing.Point(197, 74);
            this.Celsius_A.Name = "Celsius_A";
            this.Celsius_A.Size = new System.Drawing.Size(75, 23);
            this.Celsius_A.TabIndex = 9;
            this.Celsius_A.Text = "->";
            this.Celsius_A.UseVisualStyleBackColor = true;
            this.Celsius_A.Click += new System.EventHandler(this.Celsius_A_Click);
            // 
            // Kelvin_A
            // 
            this.Kelvin_A.Location = new System.Drawing.Point(197, 103);
            this.Kelvin_A.Name = "Kelvin_A";
            this.Kelvin_A.Size = new System.Drawing.Size(75, 23);
            this.Kelvin_A.TabIndex = 10;
            this.Kelvin_A.Text = "->";
            this.Kelvin_A.UseVisualStyleBackColor = true;
            this.Kelvin_A.Click += new System.EventHandler(this.Kelvin_A_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(518, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Kelvin";
            // 
            // texboxF1_1
            // 
            this.texboxF1_1.Location = new System.Drawing.Point(278, 46);
            this.texboxF1_1.Name = "texboxF1_1";
            this.texboxF1_1.Size = new System.Drawing.Size(100, 20);
            this.texboxF1_1.TabIndex = 12;
            // 
            // texboxF1_2
            // 
            this.texboxF1_2.Location = new System.Drawing.Point(384, 46);
            this.texboxF1_2.Name = "texboxF1_2";
            this.texboxF1_2.Size = new System.Drawing.Size(100, 20);
            this.texboxF1_2.TabIndex = 13;
            // 
            // texboxF1_3
            // 
            this.texboxF1_3.Location = new System.Drawing.Point(490, 46);
            this.texboxF1_3.Name = "texboxF1_3";
            this.texboxF1_3.Size = new System.Drawing.Size(100, 20);
            this.texboxF1_3.TabIndex = 14;
            // 
            // TextBoxC2_1
            // 
            this.TextBoxC2_1.Location = new System.Drawing.Point(279, 76);
            this.TextBoxC2_1.Name = "TextBoxC2_1";
            this.TextBoxC2_1.Size = new System.Drawing.Size(100, 20);
            this.TextBoxC2_1.TabIndex = 15;
            // 
            // TextBoxC2_2
            // 
            this.TextBoxC2_2.Location = new System.Drawing.Point(384, 76);
            this.TextBoxC2_2.Name = "TextBoxC2_2";
            this.TextBoxC2_2.Size = new System.Drawing.Size(100, 20);
            this.TextBoxC2_2.TabIndex = 16;
            // 
            // TextBoxC2_3
            // 
            this.TextBoxC2_3.Location = new System.Drawing.Point(490, 76);
            this.TextBoxC2_3.Name = "TextBoxC2_3";
            this.TextBoxC2_3.Size = new System.Drawing.Size(100, 20);
            this.TextBoxC2_3.TabIndex = 17;
            // 
            // TextBoxK3_1
            // 
            this.TextBoxK3_1.Location = new System.Drawing.Point(278, 105);
            this.TextBoxK3_1.Name = "TextBoxK3_1";
            this.TextBoxK3_1.Size = new System.Drawing.Size(100, 20);
            this.TextBoxK3_1.TabIndex = 18;
            // 
            // TextBoxK3_2
            // 
            this.TextBoxK3_2.Location = new System.Drawing.Point(385, 105);
            this.TextBoxK3_2.Name = "TextBoxK3_2";
            this.TextBoxK3_2.Size = new System.Drawing.Size(100, 20);
            this.TextBoxK3_2.TabIndex = 19;
            // 
            // TextBoxK3_3
            // 
            this.TextBoxK3_3.Location = new System.Drawing.Point(490, 105);
            this.TextBoxK3_3.Name = "TextBoxK3_3";
            this.TextBoxK3_3.Size = new System.Drawing.Size(100, 20);
            this.TextBoxK3_3.TabIndex = 20;
            // 
            // Historial
            // 
            this.Historial.Location = new System.Drawing.Point(90, 140);
            this.Historial.Name = "Historial";
            this.Historial.Size = new System.Drawing.Size(75, 23);
            this.Historial.TabIndex = 21;
            this.Historial.Text = "Historial";
            this.Historial.UseVisualStyleBackColor = true;
            this.Historial.Click += new System.EventHandler(this.Historial_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(800, 321);
            this.Controls.Add(this.Historial);
            this.Controls.Add(this.TextBoxK3_3);
            this.Controls.Add(this.TextBoxK3_2);
            this.Controls.Add(this.TextBoxK3_1);
            this.Controls.Add(this.TextBoxC2_3);
            this.Controls.Add(this.TextBoxC2_2);
            this.Controls.Add(this.TextBoxC2_1);
            this.Controls.Add(this.texboxF1_3);
            this.Controls.Add(this.texboxF1_2);
            this.Controls.Add(this.texboxF1_1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Kelvin_A);
            this.Controls.Add(this.Celsius_A);
            this.Controls.Add(this.Fahrenheit_A);
            this.Controls.Add(this.TextBoxK3);
            this.Controls.Add(this.TextBoxC2);
            this.Controls.Add(this.texboxF1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Conversor de Dinero";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox texboxF1;
        private System.Windows.Forms.TextBox TextBoxC2;
        private System.Windows.Forms.TextBox TextBoxK3;
        private System.Windows.Forms.Button Fahrenheit_A;
        private System.Windows.Forms.Button Celsius_A;
        private System.Windows.Forms.Button Kelvin_A;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox texboxF1_1;
        private System.Windows.Forms.TextBox texboxF1_2;
        private System.Windows.Forms.TextBox texboxF1_3;
        private System.Windows.Forms.TextBox TextBoxC2_1;
        private System.Windows.Forms.TextBox TextBoxC2_2;
        private System.Windows.Forms.TextBox TextBoxC2_3;
        private System.Windows.Forms.TextBox TextBoxK3_1;
        private System.Windows.Forms.TextBox TextBoxK3_2;
        private System.Windows.Forms.TextBox TextBoxK3_3;
        private System.Windows.Forms.Button Historial;
    }
}

