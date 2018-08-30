namespace CRC_Generate
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAddr = new System.Windows.Forms.TextBox();
            this.numCount = new System.Windows.Forms.NumericUpDown();
            this.txtData = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFunCode = new System.Windows.Forms.TextBox();
            this.txtList = new System.Windows.Forms.TextBox();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnPatch = new System.Windows.Forms.Button();
            this.btnCrc32 = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numCount)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "地址码起始";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(117, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "数量";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(289, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "数据位";
            // 
            // txtAddr
            // 
            this.txtAddr.Location = new System.Drawing.Point(83, 22);
            this.txtAddr.Name = "txtAddr";
            this.txtAddr.Size = new System.Drawing.Size(28, 21);
            this.txtAddr.TabIndex = 3;
            this.txtAddr.Text = "01";
            // 
            // numCount
            // 
            this.numCount.Location = new System.Drawing.Point(152, 22);
            this.numCount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numCount.Name = "numCount";
            this.numCount.Size = new System.Drawing.Size(50, 21);
            this.numCount.TabIndex = 4;
            this.numCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(336, 21);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(80, 21);
            this.txtData.TabIndex = 5;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(422, 19);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(70, 23);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "Generate";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(208, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "功能码";
            // 
            // txtFunCode
            // 
            this.txtFunCode.Location = new System.Drawing.Point(255, 22);
            this.txtFunCode.Name = "txtFunCode";
            this.txtFunCode.Size = new System.Drawing.Size(28, 21);
            this.txtFunCode.TabIndex = 9;
            // 
            // txtList
            // 
            this.txtList.Location = new System.Drawing.Point(12, 258);
            this.txtList.Multiline = true;
            this.txtList.Name = "txtList";
            this.txtList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtList.Size = new System.Drawing.Size(475, 205);
            this.txtList.TabIndex = 10;
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(12, 49);
            this.txtInput.Multiline = true;
            this.txtInput.Name = "txtInput";
            this.txtInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInput.Size = new System.Drawing.Size(475, 174);
            this.txtInput.TabIndex = 11;
            // 
            // btnPatch
            // 
            this.btnPatch.Location = new System.Drawing.Point(152, 229);
            this.btnPatch.Name = "btnPatch";
            this.btnPatch.Size = new System.Drawing.Size(70, 23);
            this.btnPatch.TabIndex = 12;
            this.btnPatch.Text = "Generate";
            this.btnPatch.UseVisualStyleBackColor = true;
            this.btnPatch.Click += new System.EventHandler(this.btnPatch_Click);
            // 
            // btnCrc32
            // 
            this.btnCrc32.Location = new System.Drawing.Point(277, 229);
            this.btnCrc32.Name = "btnCrc32";
            this.btnCrc32.Size = new System.Drawing.Size(75, 23);
            this.btnCrc32.TabIndex = 13;
            this.btnCrc32.Text = "CRC32 Test";
            this.btnCrc32.UseVisualStyleBackColor = true;
            this.btnCrc32.Click += new System.EventHandler(this.btnCrc32_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(374, 229);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 14;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 475);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnCrc32);
            this.Controls.Add(this.btnPatch);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.txtList);
            this.Controls.Add(this.txtFunCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtData);
            this.Controls.Add(this.numCount);
            this.Controls.Add(this.txtAddr);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CRC Generate";
            ((System.ComponentModel.ISupportInitialize)(this.numCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAddr;
        private System.Windows.Forms.NumericUpDown numCount;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFunCode;
        private System.Windows.Forms.TextBox txtList;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button btnPatch;
        private System.Windows.Forms.Button btnCrc32;
        private System.Windows.Forms.Button btnTest;
    }
}

