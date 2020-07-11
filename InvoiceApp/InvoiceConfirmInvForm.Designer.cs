namespace InvoiceApp
{
    partial class InvoiceConfirmInvForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InvoiceConfirmInvForm));
            this.ribbonControl = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.bbiPrintPreview = new DevExpress.XtraBars.BarButtonItem();
            this.bsiRecordsCount = new DevExpress.XtraBars.BarStaticItem();
            this.bbiNew = new DevExpress.XtraBars.BarButtonItem();
            this.bbiEdit = new DevExpress.XtraBars.BarButtonItem();
            this.bbiDelete = new DevExpress.XtraBars.BarButtonItem();
            this.bbiRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.bbiConfirm = new DevExpress.XtraBars.BarButtonItem();
            this.bbiRebuildPallet = new DevExpress.XtraBars.BarButtonItem();
            this.bbiPlDetail = new DevExpress.XtraBars.BarButtonItem();
            this.bbiContainerDetail = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.bbiPrintShippingMark = new DevExpress.XtraBars.BarButtonItem();
            this.bbiShipingSelect = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colSeq = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPlNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPlOutNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPlType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPlStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPlSize = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.ppMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.bbiPalletReport = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ppMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl.ExpandCollapseItem,
            this.ribbonControl.SearchEditItem,
            this.bbiPrintPreview,
            this.bsiRecordsCount,
            this.bbiNew,
            this.bbiEdit,
            this.bbiDelete,
            this.bbiRefresh,
            this.bbiConfirm,
            this.bbiRebuildPallet,
            this.bbiPlDetail,
            this.bbiContainerDetail,
            this.barButtonItem1,
            this.bbiPrintShippingMark,
            this.bbiShipingSelect,
            this.bbiPalletReport});
            this.ribbonControl.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl.MaxItemId = 30;
            this.ribbonControl.Name = "ribbonControl";
            this.ribbonControl.PageHeaderItemLinks.Add(this.barButtonItem1, "RE PRINT PALLET");
            this.ribbonControl.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.OfficeUniversal;
            this.ribbonControl.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl.Size = new System.Drawing.Size(960, 93);
            this.ribbonControl.StatusBar = this.ribbonStatusBar;
            this.ribbonControl.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // bbiPrintPreview
            // 
            this.bbiPrintPreview.Caption = "Print Preview";
            this.bbiPrintPreview.Id = 14;
            this.bbiPrintPreview.ImageOptions.ImageUri.Uri = "Preview";
            this.bbiPrintPreview.Name = "bbiPrintPreview";
            this.bbiPrintPreview.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiPrintPreview_ItemClick);
            // 
            // bsiRecordsCount
            // 
            this.bsiRecordsCount.Caption = "RECORDS : 0";
            this.bsiRecordsCount.Id = 15;
            this.bsiRecordsCount.Name = "bsiRecordsCount";
            // 
            // bbiNew
            // 
            this.bbiNew.Caption = "New Pallet";
            this.bbiNew.Id = 16;
            this.bbiNew.ImageOptions.ImageUri.Uri = "New";
            this.bbiNew.Name = "bbiNew";
            this.bbiNew.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiNew_ItemClick);
            // 
            // bbiEdit
            // 
            this.bbiEdit.Caption = "Edit";
            this.bbiEdit.Id = 17;
            this.bbiEdit.ImageOptions.ImageUri.Uri = "Save";
            this.bbiEdit.Name = "bbiEdit";
            this.bbiEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiEdit_ItemClick);
            // 
            // bbiDelete
            // 
            this.bbiDelete.Caption = "Delete";
            this.bbiDelete.Id = 18;
            this.bbiDelete.ImageOptions.ImageUri.Uri = "Delete";
            this.bbiDelete.Name = "bbiDelete";
            this.bbiDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiDelete_ItemClick);
            // 
            // bbiRefresh
            // 
            this.bbiRefresh.Caption = "Refresh";
            this.bbiRefresh.Id = 19;
            this.bbiRefresh.ImageOptions.Image = global::InvoiceApp.Properties.Resources.recurrence_16x16;
            this.bbiRefresh.ImageOptions.LargeImage = global::InvoiceApp.Properties.Resources.recurrence_32x32;
            this.bbiRefresh.Name = "bbiRefresh";
            this.bbiRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiRefresh_ItemClick);
            // 
            // bbiConfirm
            // 
            this.bbiConfirm.Caption = "Confirm Invoice";
            this.bbiConfirm.Id = 20;
            this.bbiConfirm.ImageOptions.Image = global::InvoiceApp.Properties.Resources.clear_16x161;
            this.bbiConfirm.ImageOptions.LargeImage = global::InvoiceApp.Properties.Resources.clear_32x321;
            this.bbiConfirm.Name = "bbiConfirm";
            this.bbiConfirm.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiConfirm_ItemClick);
            // 
            // bbiRebuildPallet
            // 
            this.bbiRebuildPallet.Caption = "Rebuild Pallet";
            this.bbiRebuildPallet.Id = 21;
            this.bbiRebuildPallet.ImageOptions.Image = global::InvoiceApp.Properties.Resources.build_16x16;
            this.bbiRebuildPallet.ImageOptions.LargeImage = global::InvoiceApp.Properties.Resources.build_32x32;
            this.bbiRebuildPallet.Name = "bbiRebuildPallet";
            this.bbiRebuildPallet.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiRebuildPallet_ItemClick);
            // 
            // bbiPlDetail
            // 
            this.bbiPlDetail.Caption = "Pallet Detail";
            this.bbiPlDetail.Id = 22;
            this.bbiPlDetail.ImageOptions.Image = global::InvoiceApp.Properties.Resources.article_16x162;
            this.bbiPlDetail.ImageOptions.LargeImage = global::InvoiceApp.Properties.Resources.article_32x322;
            this.bbiPlDetail.Name = "bbiPlDetail";
            this.bbiPlDetail.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiPlDetail_ItemClick);
            // 
            // bbiContainerDetail
            // 
            this.bbiContainerDetail.Caption = "Container Detail";
            this.bbiContainerDetail.Id = 23;
            this.bbiContainerDetail.ImageOptions.Image = global::InvoiceApp.Properties.Resources.article_16x163;
            this.bbiContainerDetail.ImageOptions.LargeImage = global::InvoiceApp.Properties.Resources.article_32x323;
            this.bbiContainerDetail.Name = "bbiContainerDetail";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "RE-PRINT PALLET";
            this.barButtonItem1.Id = 25;
            this.barButtonItem1.ImageOptions.Image = global::InvoiceApp.Properties.Resources.viewsetting_16x16;
            this.barButtonItem1.ImageOptions.LargeImage = global::InvoiceApp.Properties.Resources.viewsetting_32x32;
            this.barButtonItem1.ItemShortcut = new DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                | System.Windows.Forms.Keys.R));
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // bbiPrintShippingMark
            // 
            this.bbiPrintShippingMark.Caption = "Print Shipping Mark";
            this.bbiPrintShippingMark.Id = 27;
            this.bbiPrintShippingMark.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("bbiPrintShippingMark.ImageOptions.Image")));
            this.bbiPrintShippingMark.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bbiPrintShippingMark.ImageOptions.LargeImage")));
            this.bbiPrintShippingMark.Name = "bbiPrintShippingMark";
            this.bbiPrintShippingMark.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiPrintShippingMark_ItemClick);
            // 
            // bbiShipingSelect
            // 
            this.bbiShipingSelect.Caption = "Print";
            this.bbiShipingSelect.Id = 28;
            this.bbiShipingSelect.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("bbiShipingSelect.ImageOptions.Image")));
            this.bbiShipingSelect.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bbiShipingSelect.ImageOptions.LargeImage")));
            this.bbiShipingSelect.Name = "bbiShipingSelect";
            this.bbiShipingSelect.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiShipingSelect_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup2});
            this.ribbonPage1.MergeOrder = 0;
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Home";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.AllowTextClipping = false;
            this.ribbonPageGroup1.ItemLinks.Add(this.bbiNew);
            this.ribbonPageGroup1.ItemLinks.Add(this.bbiEdit);
            this.ribbonPageGroup1.ItemLinks.Add(this.bbiDelete);
            this.ribbonPageGroup1.ItemLinks.Add(this.bbiRefresh);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.ShowCaptionButton = false;
            this.ribbonPageGroup1.Text = "Tasks";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.AllowTextClipping = false;
            this.ribbonPageGroup2.ItemLinks.Add(this.bbiPrintShippingMark);
            this.ribbonPageGroup2.ItemLinks.Add(this.bbiPalletReport);
            this.ribbonPageGroup2.ItemLinks.Add(this.bbiPrintPreview);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.ShowCaptionButton = false;
            this.ribbonPageGroup2.Text = "Print and Export";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.ItemLinks.Add(this.bsiRecordsCount);
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 568);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbonControl;
            this.ribbonStatusBar.Size = new System.Drawing.Size(960, 31);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.gridControl);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 93);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(960, 475);
            this.layoutControl1.TabIndex = 4;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // gridControl
            // 
            this.gridControl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gridControl.Location = new System.Drawing.Point(12, 12);
            this.gridControl.MainView = this.gridView;
            this.gridControl.MenuManager = this.ribbonControl;
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(936, 451);
            this.gridControl.TabIndex = 8;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colSeq,
            this.colPlNo,
            this.colPlOutNo,
            this.colPlType,
            this.colPlStatus,
            this.colPlSize,
            this.colTotal,
            this.gridColumn1,
            this.gridColumn2});
            this.gridView.CustomizationFormBounds = new System.Drawing.Rectangle(153, 240, 250, 280);
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsBehavior.Editable = false;
            this.gridView.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditForm;
            this.gridView.OptionsBehavior.ReadOnly = true;
            this.gridView.OptionsEditForm.EditFormColumnCount = 2;
            this.gridView.OptionsEditForm.ShowUpdateCancelPanel = DevExpress.Utils.DefaultBoolean.True;
            this.gridView.OptionsSelection.CheckBoxSelectorField = "Spliter";
            this.gridView.OptionsSelection.ResetSelectionClickOutsideCheckboxSelector = true;
            this.gridView.OptionsView.ShowFooter = true;
            this.gridView.OptionsView.ShowGroupPanel = false;
            this.gridView.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gridView_CustomColumnDisplayText);
            this.gridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridView_MouseUp);
            this.gridView.DoubleClick += new System.EventHandler(this.gridView_DoubleClick);
            // 
            // colSeq
            // 
            this.colSeq.Caption = "#";
            this.colSeq.FieldName = "Id";
            this.colSeq.Name = "colSeq";
            this.colSeq.OptionsEditForm.UseEditorColRowSpan = false;
            this.colSeq.OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
            this.colSeq.Width = 62;
            // 
            // colPlNo
            // 
            this.colPlNo.AppearanceCell.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.colPlNo.AppearanceCell.Options.UseFont = true;
            this.colPlNo.Caption = "PALLET NO.";
            this.colPlNo.FieldName = "PlNo";
            this.colPlNo.Name = "colPlNo";
            this.colPlNo.OptionsEditForm.StartNewRow = true;
            this.colPlNo.Visible = true;
            this.colPlNo.VisibleIndex = 0;
            this.colPlNo.Width = 105;
            // 
            // colPlOutNo
            // 
            this.colPlOutNo.AppearanceCell.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.colPlOutNo.AppearanceCell.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Critical;
            this.colPlOutNo.AppearanceCell.Options.UseFont = true;
            this.colPlOutNo.AppearanceCell.Options.UseForeColor = true;
            this.colPlOutNo.Caption = "PALLET OUT";
            this.colPlOutNo.FieldName = "PlOut";
            this.colPlOutNo.Name = "colPlOutNo";
            this.colPlOutNo.OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
            this.colPlOutNo.Visible = true;
            this.colPlOutNo.VisibleIndex = 1;
            this.colPlOutNo.Width = 162;
            // 
            // colPlType
            // 
            this.colPlType.AppearanceCell.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.colPlType.AppearanceCell.Options.UseFont = true;
            this.colPlType.Caption = "PALLET TYPE";
            this.colPlType.FieldName = "PlType";
            this.colPlType.Name = "colPlType";
            this.colPlType.Visible = true;
            this.colPlType.VisibleIndex = 2;
            this.colPlType.Width = 88;
            // 
            // colPlStatus
            // 
            this.colPlStatus.AppearanceCell.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.colPlStatus.AppearanceCell.Options.UseFont = true;
            this.colPlStatus.Caption = "STATUS";
            this.colPlStatus.FieldName = "PlStatus";
            this.colPlStatus.Name = "colPlStatus";
            this.colPlStatus.OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
            this.colPlStatus.Visible = true;
            this.colPlStatus.VisibleIndex = 7;
            this.colPlStatus.Width = 178;
            // 
            // colPlSize
            // 
            this.colPlSize.AppearanceCell.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Critical;
            this.colPlSize.AppearanceCell.Options.UseForeColor = true;
            this.colPlSize.Caption = "SIZE";
            this.colPlSize.FieldName = "PlSize";
            this.colPlSize.Name = "colPlSize";
            this.colPlSize.OptionsEditForm.StartNewRow = true;
            this.colPlSize.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "PlSize", "{0:0.##}")});
            this.colPlSize.Visible = true;
            this.colPlSize.VisibleIndex = 5;
            this.colPlSize.Width = 62;
            // 
            // colTotal
            // 
            this.colTotal.Caption = "TOTAL";
            this.colTotal.FieldName = "PlTotal";
            this.colTotal.Name = "colTotal";
            this.colTotal.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "PlTotal", "{0:0.##}")});
            this.colTotal.Visible = true;
            this.colTotal.VisibleIndex = 6;
            this.colTotal.Width = 72;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "CONTAINER";
            this.gridColumn1.FieldName = "ContainerNo";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 4;
            this.gridColumn1.Width = 128;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "PALLET SIZE";
            this.gridColumn2.FieldName = "PalletSize";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 3;
            this.gridColumn2.Width = 116;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(960, 475);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(940, 455);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // ppMenu
            // 
            this.ppMenu.ItemLinks.Add(this.bbiShipingSelect, true);
            this.ppMenu.ItemLinks.Add(this.bbiRefresh, true);
            this.ppMenu.Name = "ppMenu";
            this.ppMenu.Ribbon = this.ribbonControl;
            // 
            // bbiPalletReport
            // 
            this.bbiPalletReport.Caption = "Pallet Report";
            this.bbiPalletReport.Id = 29;
            this.bbiPalletReport.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem2.ImageOptions.Image")));
            this.bbiPalletReport.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem2.ImageOptions.LargeImage")));
            this.bbiPalletReport.Name = "bbiPalletReport";
            this.bbiPalletReport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiPalletReport_ItemClick);
            // 
            // InvoiceConfirmInvForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 599);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbonControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InvoiceConfirmInvForm";
            this.Ribbon = this.ribbonControl;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar;
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ppMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem bbiPrintPreview;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.BarStaticItem bsiRecordsCount;
        private DevExpress.XtraBars.BarButtonItem bbiNew;
        private DevExpress.XtraBars.BarButtonItem bbiEdit;
        private DevExpress.XtraBars.BarButtonItem bbiDelete;
        private DevExpress.XtraBars.BarButtonItem bbiRefresh;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn colSeq;
        private DevExpress.XtraGrid.Columns.GridColumn colPlNo;
        private DevExpress.XtraGrid.Columns.GridColumn colPlOutNo;
        private DevExpress.XtraGrid.Columns.GridColumn colPlType;
        private DevExpress.XtraGrid.Columns.GridColumn colPlStatus;
        private DevExpress.XtraGrid.Columns.GridColumn colPlSize;
        private DevExpress.XtraGrid.Columns.GridColumn colTotal;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraBars.BarButtonItem bbiConfirm;
        private DevExpress.XtraBars.BarButtonItem bbiRebuildPallet;
        private DevExpress.XtraBars.PopupMenu ppMenu;
        private DevExpress.XtraBars.BarButtonItem bbiPlDetail;
        private DevExpress.XtraBars.BarButtonItem bbiContainerDetail;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraBars.BarButtonItem bbiPrintShippingMark;
        private DevExpress.XtraBars.BarButtonItem bbiShipingSelect;
        private DevExpress.XtraBars.BarButtonItem bbiPalletReport;
    }
}