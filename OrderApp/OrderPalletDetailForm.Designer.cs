﻿namespace OrderApp
{
    partial class OrderPalletDetailForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderPalletDetailForm));
            this.ribbonControl = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.bbiPrintPreview = new DevExpress.XtraBars.BarButtonItem();
            this.bsiRecordsCount = new DevExpress.XtraBars.BarStaticItem();
            this.bbiNew = new DevExpress.XtraBars.BarButtonItem();
            this.bbiEdit = new DevExpress.XtraBars.BarButtonItem();
            this.bbiDelete = new DevExpress.XtraBars.BarButtonItem();
            this.bbiRefresh = new DevExpress.XtraBars.BarButtonItem();
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
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl
            // 
            resources.ApplyResources(this.ribbonControl, "ribbonControl");
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.ExpandCollapseItem.ImageOptions.ImageIndex = ((int)(resources.GetObject("ribbonControl.ExpandCollapseItem.ImageOptions.ImageIndex")));
            this.ribbonControl.ExpandCollapseItem.ImageOptions.LargeImageIndex = ((int)(resources.GetObject("ribbonControl.ExpandCollapseItem.ImageOptions.LargeImageIndex")));
            this.ribbonControl.ExpandCollapseItem.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("ribbonControl.ExpandCollapseItem.ImageOptions.SvgImage")));
            this.ribbonControl.ExpandCollapseItem.SearchTags = resources.GetString("ribbonControl.ExpandCollapseItem.SearchTags");
            this.ribbonControl.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl.ExpandCollapseItem,
            this.ribbonControl.SearchEditItem,
            this.bbiPrintPreview,
            this.bsiRecordsCount,
            this.bbiNew,
            this.bbiEdit,
            this.bbiDelete,
            this.bbiRefresh});
            this.ribbonControl.MaxItemId = 20;
            this.ribbonControl.Name = "ribbonControl";
            this.ribbonControl.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.OfficeUniversal;
            this.ribbonControl.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl.StatusBar = this.ribbonStatusBar;
            this.ribbonControl.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // bbiPrintPreview
            // 
            resources.ApplyResources(this.bbiPrintPreview, "bbiPrintPreview");
            this.bbiPrintPreview.Id = 14;
            this.bbiPrintPreview.ImageOptions.ImageIndex = ((int)(resources.GetObject("bbiPrintPreview.ImageOptions.ImageIndex")));
            this.bbiPrintPreview.ImageOptions.ImageUri.Uri = "Preview";
            this.bbiPrintPreview.ImageOptions.LargeImageIndex = ((int)(resources.GetObject("bbiPrintPreview.ImageOptions.LargeImageIndex")));
            this.bbiPrintPreview.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("bbiPrintPreview.ImageOptions.SvgImage")));
            this.bbiPrintPreview.Name = "bbiPrintPreview";
            this.bbiPrintPreview.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiPrintPreview_ItemClick);
            // 
            // bsiRecordsCount
            // 
            resources.ApplyResources(this.bsiRecordsCount, "bsiRecordsCount");
            this.bsiRecordsCount.Id = 15;
            this.bsiRecordsCount.ImageOptions.ImageIndex = ((int)(resources.GetObject("bsiRecordsCount.ImageOptions.ImageIndex")));
            this.bsiRecordsCount.ImageOptions.LargeImageIndex = ((int)(resources.GetObject("bsiRecordsCount.ImageOptions.LargeImageIndex")));
            this.bsiRecordsCount.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("bsiRecordsCount.ImageOptions.SvgImage")));
            this.bsiRecordsCount.Name = "bsiRecordsCount";
            // 
            // bbiNew
            // 
            resources.ApplyResources(this.bbiNew, "bbiNew");
            this.bbiNew.Id = 16;
            this.bbiNew.ImageOptions.ImageIndex = ((int)(resources.GetObject("bbiNew.ImageOptions.ImageIndex")));
            this.bbiNew.ImageOptions.ImageUri.Uri = "New";
            this.bbiNew.ImageOptions.LargeImageIndex = ((int)(resources.GetObject("bbiNew.ImageOptions.LargeImageIndex")));
            this.bbiNew.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("bbiNew.ImageOptions.SvgImage")));
            this.bbiNew.Name = "bbiNew";
            // 
            // bbiEdit
            // 
            resources.ApplyResources(this.bbiEdit, "bbiEdit");
            this.bbiEdit.Id = 17;
            this.bbiEdit.ImageOptions.ImageIndex = ((int)(resources.GetObject("bbiEdit.ImageOptions.ImageIndex")));
            this.bbiEdit.ImageOptions.ImageUri.Uri = "Edit";
            this.bbiEdit.ImageOptions.LargeImageIndex = ((int)(resources.GetObject("bbiEdit.ImageOptions.LargeImageIndex")));
            this.bbiEdit.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("bbiEdit.ImageOptions.SvgImage")));
            this.bbiEdit.Name = "bbiEdit";
            // 
            // bbiDelete
            // 
            resources.ApplyResources(this.bbiDelete, "bbiDelete");
            this.bbiDelete.Id = 18;
            this.bbiDelete.ImageOptions.ImageIndex = ((int)(resources.GetObject("bbiDelete.ImageOptions.ImageIndex")));
            this.bbiDelete.ImageOptions.ImageUri.Uri = "Delete";
            this.bbiDelete.ImageOptions.LargeImageIndex = ((int)(resources.GetObject("bbiDelete.ImageOptions.LargeImageIndex")));
            this.bbiDelete.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("bbiDelete.ImageOptions.SvgImage")));
            this.bbiDelete.Name = "bbiDelete";
            // 
            // bbiRefresh
            // 
            resources.ApplyResources(this.bbiRefresh, "bbiRefresh");
            this.bbiRefresh.Id = 19;
            this.bbiRefresh.ImageOptions.ImageIndex = ((int)(resources.GetObject("bbiRefresh.ImageOptions.ImageIndex")));
            this.bbiRefresh.ImageOptions.ImageUri.Uri = "Refresh";
            this.bbiRefresh.ImageOptions.LargeImageIndex = ((int)(resources.GetObject("bbiRefresh.ImageOptions.LargeImageIndex")));
            this.bbiRefresh.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("bbiRefresh.ImageOptions.SvgImage")));
            this.bbiRefresh.Name = "bbiRefresh";
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup2});
            this.ribbonPage1.MergeOrder = 0;
            this.ribbonPage1.Name = "ribbonPage1";
            resources.ApplyResources(this.ribbonPage1, "ribbonPage1");
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
            resources.ApplyResources(this.ribbonPageGroup1, "ribbonPageGroup1");
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.AllowTextClipping = false;
            this.ribbonPageGroup2.ItemLinks.Add(this.bbiPrintPreview);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.ShowCaptionButton = false;
            resources.ApplyResources(this.ribbonPageGroup2, "ribbonPageGroup2");
            // 
            // ribbonStatusBar
            // 
            resources.ApplyResources(this.ribbonStatusBar, "ribbonStatusBar");
            this.ribbonStatusBar.ItemLinks.Add(this.bsiRecordsCount);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbonControl;
            // 
            // layoutControl1
            // 
            resources.ApplyResources(this.layoutControl1, "layoutControl1");
            this.layoutControl1.Controls.Add(this.gridControl);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            // 
            // gridControl
            // 
            resources.ApplyResources(this.gridControl, "gridControl");
            this.gridControl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gridControl.EmbeddedNavigator.AccessibleDescription = resources.GetString("gridControl.EmbeddedNavigator.AccessibleDescription");
            this.gridControl.EmbeddedNavigator.AccessibleName = resources.GetString("gridControl.EmbeddedNavigator.AccessibleName");
            this.gridControl.EmbeddedNavigator.AllowHtmlTextInToolTip = ((DevExpress.Utils.DefaultBoolean)(resources.GetObject("gridControl.EmbeddedNavigator.AllowHtmlTextInToolTip")));
            this.gridControl.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("gridControl.EmbeddedNavigator.Anchor")));
            this.gridControl.EmbeddedNavigator.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gridControl.EmbeddedNavigator.BackgroundImage")));
            this.gridControl.EmbeddedNavigator.BackgroundImageLayout = ((System.Windows.Forms.ImageLayout)(resources.GetObject("gridControl.EmbeddedNavigator.BackgroundImageLayout")));
            this.gridControl.EmbeddedNavigator.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("gridControl.EmbeddedNavigator.ImeMode")));
            this.gridControl.EmbeddedNavigator.MaximumSize = ((System.Drawing.Size)(resources.GetObject("gridControl.EmbeddedNavigator.MaximumSize")));
            this.gridControl.EmbeddedNavigator.TextLocation = ((DevExpress.XtraEditors.NavigatorButtonsTextLocation)(resources.GetObject("gridControl.EmbeddedNavigator.TextLocation")));
            this.gridControl.EmbeddedNavigator.ToolTip = resources.GetString("gridControl.EmbeddedNavigator.ToolTip");
            this.gridControl.EmbeddedNavigator.ToolTipIconType = ((DevExpress.Utils.ToolTipIconType)(resources.GetObject("gridControl.EmbeddedNavigator.ToolTipIconType")));
            this.gridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("gridControl.EmbeddedNavigator.ToolTipTitle");
            this.gridControl.MainView = this.gridView;
            this.gridControl.MenuManager = this.ribbonControl;
            this.gridControl.Name = "gridControl";
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            resources.ApplyResources(this.gridView, "gridView");
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colSeq,
            this.colPlNo,
            this.colPlOutNo,
            this.colPlType,
            this.colPlStatus,
            this.colPlSize,
            this.colTotal,
            this.gridColumn1});
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
            // 
            // colSeq
            // 
            resources.ApplyResources(this.colSeq, "colSeq");
            this.colSeq.FieldName = "Id";
            this.colSeq.Name = "colSeq";
            this.colSeq.OptionsEditForm.Caption = resources.GetString("colSeq.OptionsEditForm.Caption");
            this.colSeq.OptionsEditForm.UseEditorColRowSpan = false;
            this.colSeq.OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
            // 
            // colPlNo
            // 
            this.colPlNo.AppearanceCell.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("colPlNo.AppearanceCell.FontStyleDelta")));
            this.colPlNo.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.colPlNo, "colPlNo");
            this.colPlNo.FieldName = "PlNo";
            this.colPlNo.Name = "colPlNo";
            this.colPlNo.OptionsEditForm.Caption = resources.GetString("colPlNo.OptionsEditForm.Caption");
            this.colPlNo.OptionsEditForm.StartNewRow = true;
            // 
            // colPlOutNo
            // 
            this.colPlOutNo.AppearanceCell.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("colPlOutNo.AppearanceCell.FontStyleDelta")));
            this.colPlOutNo.AppearanceCell.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Critical;
            this.colPlOutNo.AppearanceCell.Options.UseFont = true;
            this.colPlOutNo.AppearanceCell.Options.UseForeColor = true;
            resources.ApplyResources(this.colPlOutNo, "colPlOutNo");
            this.colPlOutNo.FieldName = "PlOut";
            this.colPlOutNo.Name = "colPlOutNo";
            this.colPlOutNo.OptionsEditForm.Caption = resources.GetString("colPlOutNo.OptionsEditForm.Caption");
            this.colPlOutNo.OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
            // 
            // colPlType
            // 
            this.colPlType.AppearanceCell.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("colPlType.AppearanceCell.FontStyleDelta")));
            this.colPlType.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.colPlType, "colPlType");
            this.colPlType.FieldName = "PlType";
            this.colPlType.Name = "colPlType";
            // 
            // colPlStatus
            // 
            this.colPlStatus.AppearanceCell.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("colPlStatus.AppearanceCell.FontStyleDelta")));
            this.colPlStatus.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.colPlStatus, "colPlStatus");
            this.colPlStatus.FieldName = "PlStatus";
            this.colPlStatus.Name = "colPlStatus";
            this.colPlStatus.OptionsEditForm.Caption = resources.GetString("colPlStatus.OptionsEditForm.Caption");
            this.colPlStatus.OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
            // 
            // colPlSize
            // 
            this.colPlSize.AppearanceCell.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Critical;
            this.colPlSize.AppearanceCell.Options.UseForeColor = true;
            resources.ApplyResources(this.colPlSize, "colPlSize");
            this.colPlSize.FieldName = "PlSize";
            this.colPlSize.Name = "colPlSize";
            this.colPlSize.OptionsEditForm.Caption = resources.GetString("colPlSize.OptionsEditForm.Caption");
            this.colPlSize.OptionsEditForm.StartNewRow = true;
            // 
            // colTotal
            // 
            resources.ApplyResources(this.colTotal, "colTotal");
            this.colTotal.FieldName = "PlTotal";
            this.colTotal.Name = "colTotal";
            // 
            // gridColumn1
            // 
            resources.ApplyResources(this.gridColumn1, "gridColumn1");
            this.gridColumn1.FieldName = "ContainerNo";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1024, 583);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            resources.ApplyResources(this.layoutControlItem1, "layoutControlItem1");
            this.layoutControlItem1.Control = this.gridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1004, 563);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // OrderPalletDetailForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbonControl);
            this.Name = "OrderPalletDetailForm";
            this.Ribbon = this.ribbonControl;
            this.StatusBar = this.ribbonStatusBar;
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
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
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
    }
}