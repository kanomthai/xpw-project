namespace OrderApp
{
    partial class OrderAddNewPalletForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderAddNewPalletForm));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.bbiAddPl = new DevExpress.XtraEditors.SimpleButton();
            this.bbiPlTotal = new DevExpress.XtraEditors.TextEdit();
            this.bbiPlSize = new DevExpress.XtraEditors.TextEdit();
            this.bbiPlNo = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.bbiPallet = new System.Windows.Forms.RadioButton();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.bbiBox = new System.Windows.Forms.RadioButton();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.label1 = new System.Windows.Forms.Label();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bbiPlTotal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bbiPlSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bbiPlNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.label1);
            this.layoutControl1.Controls.Add(this.bbiBox);
            this.layoutControl1.Controls.Add(this.bbiPallet);
            this.layoutControl1.Controls.Add(this.bbiAddPl);
            this.layoutControl1.Controls.Add(this.bbiPlTotal);
            this.layoutControl1.Controls.Add(this.bbiPlSize);
            this.layoutControl1.Controls.Add(this.bbiPlNo);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(327, 149);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // bbiAddPl
            // 
            this.bbiAddPl.Location = new System.Drawing.Point(165, 113);
            this.bbiAddPl.Name = "bbiAddPl";
            this.bbiAddPl.Size = new System.Drawing.Size(150, 22);
            this.bbiAddPl.StyleController = this.layoutControl1;
            this.bbiAddPl.TabIndex = 7;
            this.bbiAddPl.Text = "ADD";
            this.bbiAddPl.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // bbiPlTotal
            // 
            this.bbiPlTotal.Location = new System.Drawing.Point(53, 89);
            this.bbiPlTotal.Name = "bbiPlTotal";
            this.bbiPlTotal.Size = new System.Drawing.Size(262, 20);
            this.bbiPlTotal.StyleController = this.layoutControl1;
            this.bbiPlTotal.TabIndex = 6;
            // 
            // bbiPlSize
            // 
            this.bbiPlSize.Location = new System.Drawing.Point(81, 65);
            this.bbiPlSize.Name = "bbiPlSize";
            this.bbiPlSize.Size = new System.Drawing.Size(234, 20);
            this.bbiPlSize.StyleController = this.layoutControl1;
            this.bbiPlSize.TabIndex = 5;
            // 
            // bbiPlNo
            // 
            this.bbiPlNo.Location = new System.Drawing.Point(78, 41);
            this.bbiPlNo.Name = "bbiPlNo";
            this.bbiPlNo.Size = new System.Drawing.Size(237, 20);
            this.bbiPlNo.StyleController = this.layoutControl1;
            this.bbiPlNo.TabIndex = 4;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(327, 149);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.bbiPlNo;
            this.layoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 29);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(307, 24);
            this.layoutControlItem1.Text = "PALLET NO.:";
            this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(61, 13);
            this.layoutControlItem1.TextToControlDistance = 5;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 101);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(153, 28);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.bbiPlSize;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 53);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(307, 24);
            this.layoutControlItem2.Text = "PALLET SIZE:";
            this.layoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(64, 13);
            this.layoutControlItem2.TextToControlDistance = 5;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.bbiPlTotal;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 77);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(307, 24);
            this.layoutControlItem3.Text = "TOTAL:";
            this.layoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(36, 13);
            this.layoutControlItem3.TextToControlDistance = 5;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.bbiAddPl;
            this.layoutControlItem4.Location = new System.Drawing.Point(153, 101);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(154, 28);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // bbiPallet
            // 
            this.bbiPallet.Checked = true;
            this.bbiPallet.Location = new System.Drawing.Point(91, 12);
            this.bbiPallet.Name = "bbiPallet";
            this.bbiPallet.Size = new System.Drawing.Size(83, 25);
            this.bbiPallet.TabIndex = 8;
            this.bbiPallet.TabStop = true;
            this.bbiPallet.Text = "PALLET";
            this.bbiPallet.UseVisualStyleBackColor = true;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.bbiPallet;
            this.layoutControlItem5.Location = new System.Drawing.Point(79, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(87, 29);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // bbiBox
            // 
            this.bbiBox.Location = new System.Drawing.Point(178, 12);
            this.bbiBox.Name = "bbiBox";
            this.bbiBox.Size = new System.Drawing.Size(137, 25);
            this.bbiBox.TabIndex = 9;
            this.bbiBox.Text = "BOX";
            this.bbiBox.UseVisualStyleBackColor = true;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.bbiBox;
            this.layoutControlItem6.Location = new System.Drawing.Point(166, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(141, 29);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 25);
            this.label1.TabIndex = 10;
            this.label1.Text = "PALLET TYPE:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.label1;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(79, 29);
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // OrderAddNewPalletForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 149);
            this.Controls.Add(this.layoutControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "OrderAddNewPalletForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Pallet";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bbiPlTotal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bbiPlSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bbiPlNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraEditors.SimpleButton bbiAddPl;
        private DevExpress.XtraEditors.TextEdit bbiPlTotal;
        private DevExpress.XtraEditors.TextEdit bbiPlSize;
        private DevExpress.XtraEditors.TextEdit bbiPlNo;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton bbiBox;
        private System.Windows.Forms.RadioButton bbiPallet;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
    }
}