using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using C1.Win.C1Ribbon;
using C1.Framework;
using IntergeratedDocumentExplorer.Lib;

using System.IO;

namespace TextEditor
{
    public partial class TextEditor : UserControl , IC1RibbonHolder
    {
        #region control Declarations
        private RichTextBox rchTextBoxEditor;
        private C1Ribbon ribbonTextEditor;
        private RibbonTab ribbonTabHome;
        private RibbonGroup ribbonGroupFont;
        private RibbonFontComboBox ribbonFontComboBoxFamily;
        private RibbonToolBar ribbonToolFontStyle;
        private RibbonToggleButton ribbonbtnBold,ribbonbtnItalic,ribbonbtnUnderline,ribbonbtnStrikeout;
        private RibbonComboBox ribbonComboBoxSize;
        private RibbonToolBar ribbonToolColor;
        private RibbonColorPicker ribbonFontColorPicker,ribbonBackColorPicker;
        private RibbonGroup ribbonGroupEditOptions;
        private RibbonSplitButton ribbonSplitBtnPaste;
        private RibbonButton ribbonBtnPaste,ribbonBtnCut,ribbonBtnCopy,ribbonBtnFormatPainter;
        private RibbonGroup ribbonGroupParagraph;
        private RibbonToolBar ribbonToolTextAlign;
        private RibbonToggleButton ribbonBtnAlignLeft,ribbonBtnAlignCenter,ribbonBtnAlignRight;
        private RibbonToolBar ribbonToolIndent;
        private RibbonButton ribbonBtnLeftIndent,ribbonBtnRightIndent;
        private RibbonTab ribbonTabView;
        private RibbonGroup ribbonGroupZoom;
        private RibbonComboBox ribbonComboBoxZoom;
        private RibbonLabel ribbonLblZoom;
        private RibbonQat ribbonQat;
        private RibbonButton ribbonBtnUndo, ribbonBtnRedo, ribbonBtnPrintPreview,ribbonBtnBack;
        private Dictionary<RibbonToggleButton, FontStyle> DictFontStyle;
        private Dictionary<RibbonToggleButton, HorizontalAlignment> DictTextAlign;
        private Dictionary<string, RichTextBoxStreamType> streamType;
        private RibbonApplicationMenu ribbonAppMainMenu;
        private RibbonSplitButton ribbonBtnSaveAs;
        private RibbonButton ribbonBtnNew, ribbonBtnOpen, ribbonBtnSave, ribbonBtnExit;
        #endregion

        #region Library Declarations
        private FontUtils _fontUtils;
        private Paragraph _pharagraph;
        private Zoom _zoom;
        private FileInputOuput _fileInputOuput;
        private SettingUtils settingUtils;
        #endregion


        private bool _updations;
        private string _fileName= String.Empty;
        private bool NewFile = false;
        #region Constructors Intialization
        public TextEditor()
        {
            InitializeComponent();
            ComponentInitialization();
        }
        public TextEditor(string FileName)
        {
            _fileName = FileName;
            InitializeComponent();
            ComponentInitialization();

        }
        #endregion
        private void  ComponentInitialization()
        {
            #region Lib Initialization
            //fontUtils
            _fontUtils = new FontUtils();
            _fontUtils.FontChanged += Font_FontChanged;
            _fontUtils.ChangedFont += Font_ChangedFont;
            _fontUtils.ChangedBackColor += Font_ChangedBackColor;
            _fontUtils.ChangedForeColor += Font_ChangedForeColor;

            //ParagraphUtils
            _pharagraph = new Paragraph();
            _pharagraph.AlignmentChanged += Paragraph_AlignmentChanged; 
            _pharagraph.ChangedAlignment += Paragraph_ChangedAlignment;
            _pharagraph.IndentationChanged += Paragraph_IndentationChanged;

            //ZoomUtils
            _zoom = new Zoom();
            _zoom.ZoomChanged += Zoom_ZoomChanged;
            
            //SettingsUtils
            settingUtils = new SettingUtils();

            //fileInputOutputUitls
            _fileInputOuput = new FileInputOuput(ResourceRtfEditor.fileTypes.Split(','));
            _fileInputOuput.FileOpened += _fileInputOuput_FileOpened;
            _fileInputOuput.FileSaved += _fileInputOuput_FileSaved;
            _fileInputOuput.NewFileCreated += _fileInputOuput_NewFileCreated;
            _fileInputOuput.NewFileSaved += _fileInputOuput_NewFileSaved;
            _fileInputOuput.SavedFile += _fileInputOuput_SavedFile;

            #endregion

            #region Control Initialization
            //C1Ribbon
            ribbonTextEditor = new C1Ribbon();
            
            ribbonbtnBold = new RibbonToggleButton();
            ribbonbtnItalic = new RibbonToggleButton();
            ribbonbtnUnderline = new RibbonToggleButton();
            ribbonbtnStrikeout = new RibbonToggleButton();
            ribbonToolFontStyle = new RibbonToolBar();
            ribbonAppMainMenu = new RibbonApplicationMenu();
            ribbonTextEditor.ApplicationMenuHolder = ribbonAppMainMenu;

            ribbonToolFontStyle.Name = "RibbonToolFontStyle";

            DictFontStyle = new Dictionary<RibbonToggleButton, FontStyle>() {
                { ribbonbtnBold , FontStyle.Bold },
                { ribbonbtnItalic , FontStyle.Italic },
                { ribbonbtnUnderline , FontStyle.Underline },
                { ribbonbtnStrikeout , FontStyle.Strikeout }
            };

            streamType = new Dictionary<string, RichTextBoxStreamType>()
            {
                { ".rtf",RichTextBoxStreamType.RichText },
                { ".txt",RichTextBoxStreamType.PlainText }
            };

            foreach (KeyValuePair<RibbonToggleButton, FontStyle> value in DictFontStyle)
            {
                //ribbonButtonFontStyles
                value.Key.Name = value.Key.ToString();
                value.Key.SmallImage = ((Image)(ResourceRtfEditor.ResourceManager.GetObject(value.Value.ToString())));
                value.Key.PressedChanged += RibbonbtnFontStyle_PressedChanged;
                value.Key.KeyTip = KeyTipsResource.ResourceManager.GetString(value.Value.ToString());
                value.Key.ToolTip = ToolTipsResource.ResourceManager.GetString(value.Value.ToString());
                value.Key.ShortcutKeys = Keys.Control | (Keys)Enum.Parse((typeof(Keys)), Shorcutkeys.ResourceManager.GetString(value.Value.ToString()), true);
                ribbonToolFontStyle.Items.Add(value.Key);
            }

            //ribbonComboBox FontFamily
            ribbonFontComboBoxFamily = new RibbonFontComboBox();
            ribbonFontComboBoxFamily.Name = "FontFamilyComboBox";
            ribbonFontComboBoxFamily.SelectedIndex = 13;
            ribbonFontComboBoxFamily.ChangeCommitted += RibbonFontComboBoxFamily_IndexChanged;
            ribbonFontComboBoxFamily.KeyTip = KeyTipsResource.FontFamily;
            ribbonFontComboBoxFamily.ToolTip = ToolTipsResource.FontFamily;

            //ribbonComboBox FontSize
            ribbonComboBoxSize = new RibbonComboBox();
            ribbonComboBoxSize.Name = "SizeComboBox";
            foreach (string size in ResourceRtfEditor.Sizes.Split(','))
            {
                ribbonComboBoxSize.Items.Add(size);
            }
            ribbonComboBoxSize.SelectedIndex = 3;
            ribbonComboBoxSize.ChangeCommitted += RibbonComboBoxSize_SelectedIndexChanged;
            ribbonComboBoxSize.KeyTip = KeyTipsResource.FontSize;
            ribbonComboBoxSize.ToolTip = ToolTipsResource.FontSize;

            //ribbonColorPicker ForeColor
            ribbonFontColorPicker = new RibbonColorPicker();
            ribbonFontColorPicker.Name = "FontColorPicker";
            ribbonFontColorPicker.SmallImage = ((Image)(ResourceRtfEditor.FontColor));
            ribbonFontColorPicker.SelectedColorChanged += RibbonFontColorPicker_SelectedColorChanged;
            ribbonFontColorPicker.KeyTip = KeyTipsResource.ForeColor;
            ribbonFontColorPicker.ToolTip = ToolTipsResource.ForeColor;

            //ribbonColorPicker HighlightColor
            ribbonBackColorPicker = new RibbonColorPicker();
            ribbonBackColorPicker.Name = "BackColorPicker";
            ribbonBackColorPicker.SmallImage = ((Image)(ResourceRtfEditor.BackColor));
            ribbonBackColorPicker.SelectedColorChanged += RibbonBackColorPicker_SelectedColorChanged;
            ribbonBackColorPicker.KeyTip = KeyTipsResource.BackColor;
            ribbonBackColorPicker.ToolTip = ToolTipsResource.BackColor;

            //ribbon Color Toolbar
            ribbonToolColor = new RibbonToolBar();
            ribbonToolColor.Items.Add(ribbonFontColorPicker);
            ribbonToolColor.Items.Add(ribbonBackColorPicker);

            //ribbon Font Group
            ribbonGroupFont = new RibbonGroup();
            ribbonGroupFont.Items.Add(ribbonFontComboBoxFamily);
            ribbonGroupFont.Items.Add(ribbonToolFontStyle);
            ribbonGroupFont.Items.Add(ribbonComboBoxSize);
            ribbonGroupFont.Items.Add(ribbonToolColor);
            ribbonGroupFont.Name = "ribbonGroupFont";
            ribbonGroupFont.Text = "Font";
            ribbonGroupFont.Image = ((Image)(ResourceRtfEditor.Font));
            ribbonGroupFont.GroupKeyTip = KeyTipsResource.FontGroup;

            //ribbonButton Paste
            ribbonBtnPaste = new RibbonButton();
            ribbonBtnPaste.Name = "PasteRibbonButton";
            ribbonBtnPaste.Text = "Paste";
            ribbonBtnPaste.Click += RibbonBtnCut_Click;
            ribbonBtnPaste.KeyTip = KeyTipsResource.Paste;
            ribbonBtnPaste.ToolTip = ToolTipsResource.Paste;

            //ribbonButton Cut
            ribbonBtnCut = new RibbonButton();
            ribbonBtnCut.Name = "CutRibbonButton";
            ribbonBtnCut.Text = "Cut";
            ribbonBtnCut.Enabled = false;
            ribbonBtnCut.SmallImage = ((Image)(ResourceRtfEditor.Cut));
            ribbonBtnCut.Click += RibbonBtnCut_Click;
            ribbonBtnCut.KeyTip = KeyTipsResource.Cut;
            ribbonBtnCut.ToolTip = ToolTipsResource.Cut;

            //ribbonButton Copy
            ribbonBtnCopy = new RibbonButton();
            ribbonBtnCopy.Name = "CopyRibbonButton";
            ribbonBtnCopy.Text = "Copy";
            ribbonBtnCopy.Enabled = false;
            ribbonBtnCopy.SmallImage = ((Image)(ResourceRtfEditor.Copy));
            ribbonBtnCopy.Click += RibbonBtnCut_Click;
            ribbonBtnCopy.KeyTip = KeyTipsResource.Copy;
            ribbonBtnCopy.ToolTip = ToolTipsResource.Copy;

            //ribbonSplitButton Paste
            ribbonSplitBtnPaste = new RibbonSplitButton();
            ribbonSplitBtnPaste.Name = "PasteRibbonSplitButton";
            ribbonSplitBtnPaste.Text = "Paste";
            ribbonSplitBtnPaste.LargeImage = ((Image)(ResourceRtfEditor.Paste));
            ribbonSplitBtnPaste.Items.Add(ribbonBtnPaste);
            ribbonSplitBtnPaste.Click += RibbonSplitBtnPaste_Click;
            ribbonSplitBtnPaste.KeyTip = KeyTipsResource.Paste;
            ribbonSplitBtnPaste.ToolTip = ToolTipsResource.Paste;

            //ribbonButton FormatPainter
            ribbonBtnFormatPainter = new RibbonButton();
            ribbonBtnFormatPainter.Name = "FormatPainterRibbonButton";
            ribbonBtnFormatPainter.Text = "Format Painter";
            ribbonBtnFormatPainter.ShortcutKeys = Keys.Control | Keys.Shift | Keys.C;
            ribbonBtnFormatPainter.SmallImage = ((Image)(ResourceRtfEditor.FormatPainter));
            ribbonBtnFormatPainter.KeyTip = KeyTipsResource.FormatPainter;
            ribbonBtnFormatPainter.Click += RibbonBtnFormatPainter_Click;

            //ribbonGroup EditOptions
            ribbonGroupEditOptions = new RibbonGroup();
            ribbonGroupEditOptions.Items.Add(ribbonSplitBtnPaste);
            ribbonGroupEditOptions.Items.Add(ribbonBtnCut);
            ribbonGroupEditOptions.Items.Add(ribbonBtnCopy);
            ribbonGroupEditOptions.Items.Add(ribbonBtnFormatPainter);
            ribbonGroupEditOptions.Name = "ribbonGroupEditOptions";
            ribbonGroupEditOptions.Text = "Clipboard";
            ribbonGroupEditOptions.Image = ((Image)(ResourceRtfEditor.Paste));
            ribbonGroupEditOptions.GroupKeyTip = KeyTipsResource.EditingGroup;

            //ribbonButton LeftIndent
            ribbonBtnLeftIndent = new RibbonButton();
            ribbonBtnLeftIndent.Name = "LeftIndent";
            ribbonBtnLeftIndent.SmallImage = ((Image)(ResourceRtfEditor.LeftIndent));
            ribbonBtnLeftIndent.Click += RibbonBtnLeftIndent_Click;
            ribbonBtnLeftIndent.KeyTip = KeyTipsResource.LeftIndent;
            ribbonBtnLeftIndent.ToolTip = ToolTipsResource.LeftIndent;

            //ribbonButton RightIndent
            ribbonBtnRightIndent = new RibbonButton();
            ribbonBtnRightIndent.Name = "RightIndent";
            ribbonBtnRightIndent.SmallImage = ((Image)(ResourceRtfEditor.RightIndent));
            ribbonBtnRightIndent.Click += RibbonBtnRightIndent_Click;
            ribbonBtnRightIndent.KeyTip = KeyTipsResource.RightIndent;
            ribbonBtnRightIndent.ToolTip = ToolTipsResource.RightIndent;

            //ribbonToolbar Indentation
            ribbonToolIndent = new RibbonToolBar();
            ribbonToolIndent.Items.Add(ribbonBtnLeftIndent);
            ribbonToolIndent.Items.Add(ribbonBtnRightIndent);
            ribbonToolIndent.Name = "IndentRiibonTool";

            //ribbonButton AlignLeft
            ribbonBtnAlignLeft = new RibbonToggleButton();
            ribbonBtnAlignLeft.Name = "LeftAlign";
            ribbonBtnAlignLeft.SmallImage = ((Image)(ResourceRtfEditor.TextAlignLeft));
            ribbonBtnAlignLeft.ToggleGroupName = "AlignGroup";
            ribbonBtnAlignLeft.PressedChanged += RibbonBtnAlign_PressedChanged;
            ribbonBtnAlignLeft.KeyTip = KeyTipsResource.AlignLeft;
            ribbonBtnAlignLeft.ToolTip = ToolTipsResource.AlignLeft;

            //ribbonButton AlignCenter
            ribbonBtnAlignCenter = new RibbonToggleButton();
            ribbonBtnAlignCenter.Name = "MiddleAlign";
            ribbonBtnAlignCenter.SmallImage = ((Image)(ResourceRtfEditor.TextAlignMiddle));
            ribbonBtnAlignCenter.ToggleGroupName = "AlignGroup";
            ribbonBtnAlignCenter.PressedChanged += RibbonBtnAlign_PressedChanged;
            ribbonBtnAlignCenter.KeyTip = KeyTipsResource.AlignCenter;
            ribbonBtnAlignCenter.ToolTip = ToolTipsResource.AlignCenter;

            //ribbonButton AlignRight
            ribbonBtnAlignRight = new RibbonToggleButton();
            ribbonBtnAlignRight.Name = "RightAlign";
            ribbonBtnAlignRight.SmallImage = ((Image)(ResourceRtfEditor.TextAlignRight));
            ribbonBtnAlignRight.ToggleGroupName = "AlignGroup";
            ribbonBtnAlignRight.PressedChanged += RibbonBtnAlign_PressedChanged;
            ribbonBtnAlignRight.KeyTip = KeyTipsResource.AlignRight;
            ribbonBtnAlignRight.ToolTip = ToolTipsResource.AlignRight;

            //ribbon ToolBar TextAlign
            ribbonToolTextAlign = new RibbonToolBar();
            ribbonToolTextAlign.Name = "TextAlign";
            ribbonToolTextAlign.Items.Add(ribbonBtnAlignLeft);
            ribbonToolTextAlign.Items.Add(ribbonBtnAlignCenter);
            ribbonToolTextAlign.Items.Add(ribbonBtnAlignRight);

            //ribbonGroup Paragraph
            ribbonGroupParagraph = new RibbonGroup();
            ribbonGroupParagraph.Name = "ribbonGroupParagraph";
            ribbonGroupParagraph.Text = "Paragraph";
            ribbonGroupParagraph.Items.Add(ribbonToolIndent);
            ribbonGroupParagraph.Items.Add(ribbonToolTextAlign);
            ribbonGroupParagraph.GroupKeyTip = KeyTipsResource.PharagrapghGroup;

            //ribbonTab Home
            ribbonTabHome = new RibbonTab();
            ribbonTabHome.Groups.Add(ribbonGroupEditOptions);
            ribbonTabHome.Groups.Add(ribbonGroupFont);
            ribbonTabHome.Groups.Add(ribbonGroupParagraph);
            ribbonTabHome.Name = "ribbonTabHome";
            ribbonTabHome.Text = "&Home";

            //ribbonComboBox Zoom
            ribbonComboBoxZoom = new RibbonComboBox();
            ribbonComboBoxZoom.Name = "ZoomRibbonComboBox";
            foreach (string size in ResourceRtfEditor.Zoom.Split(','))
            {
                ribbonComboBoxZoom.Items.Add(size + ResourceRtfEditor.PerCent);
            }
            ribbonComboBoxZoom.SelectedIndex = 3;
            ribbonComboBoxZoom.ChangeCommitted += RibbonComboBoxZoom_ChangeCommitted;
            ribbonComboBoxZoom.KeyTip = KeyTipsResource.Zoom;
            ribbonComboBoxSize.ToolTip = ToolTipsResource.ZoomCombo;

            //ribbonLabel Zoom
            ribbonLblZoom = new RibbonLabel();
            ribbonLblZoom.Name = "ZoomRibbonLabel";
            ribbonLblZoom.Text = "100%";
            ribbonLblZoom.SmallImage = ((Image)(ResourceRtfEditor.NewDoc));
            ribbonLblZoom.ToolTip = ToolTipsResource.ZomLabel;

            //ribbonGroup Zoom
            ribbonGroupZoom = new RibbonGroup();
            ribbonGroupZoom.Name = "ribbonGroupZoom";
            ribbonGroupZoom.Text = "Zoom";
            ribbonGroupZoom.Items.Add(ribbonComboBoxZoom);
            ribbonGroupZoom.Items.Add(ribbonLblZoom);
            ribbonGroupZoom.GroupKeyTip = KeyTipsResource.ZoomGroup;
            
            //ribbonTab View
            ribbonTabView = new RibbonTab();
            ribbonTabView.Groups.Add(ribbonGroupZoom);
            ribbonTabView.Name = "ribbonTabZoom";
            ribbonTabView.Text = "&View";
            
            //ribbonButton Back
            ribbonBtnBack = new RibbonButton();
            ribbonBtnBack.Name = "UndoRibbonBtn";
            ribbonBtnBack.SmallImage = ((Image)(ResourceRtfEditor.Back));

            //ribbonButton Undo
            ribbonBtnUndo = new RibbonButton();
            ribbonBtnUndo.Name = "UndoRibbonBtn";
            ribbonBtnUndo.Enabled = false;
            ribbonBtnUndo.SmallImage = ((Image)(ResourceRtfEditor.Undo));
            ribbonBtnUndo.Click += RibbonBtnUndo_Click;
            ribbonBtnUndo.ToolTip = ToolTipsResource.Undo;

            //ribbonButton Redo
            ribbonBtnRedo = new RibbonButton();
            ribbonBtnRedo.Name = "UndoRibbonBtn";
            ribbonBtnRedo.Enabled = false;
            ribbonBtnRedo.SmallImage = ((Image)(ResourceRtfEditor.Redo));
            ribbonBtnRedo.Click += RibbonBtnRedo_Click;
            ribbonBtnRedo.ToolTip = ToolTipsResource.Redo;

            //ribbonButton PrintPreview
            ribbonBtnPrintPreview = new RibbonButton();
            ribbonBtnPrintPreview.Name = "PrintPreviewRibbonBtn";
            ribbonBtnPrintPreview.SmallImage = ((Image)(ResourceRtfEditor.PrintPreview));

            //ribbonQuickAccessToolBar
            ribbonQat = new RibbonQat();
            ribbonTextEditor.QatHolder = ribbonQat;
            ribbonQat.ItemLinks.Add(ribbonBtnBack);
            ribbonQat.ItemLinks.Add(ribbonBtnUndo);
            ribbonQat.ItemLinks.Add(ribbonBtnRedo);
            ribbonQat.ItemLinks.Add(ribbonBtnPrintPreview);

            //ribbonButton New
            ribbonBtnNew = new RibbonButton();
            ribbonBtnNew.Name = "NewRibbonBtn";
            ribbonBtnNew.Text = "&New";
            ribbonBtnNew.SmallImage = ((Image)(ResourceRtfEditor.NewDoc));
            ribbonBtnNew.ShortcutKeys = Keys.Control | Keys.N;
            ribbonBtnNew.Click += RibbonBtnNew_Click;
            ribbonBtnNew.ToolTip = ToolTipsResource.New;

            //ribbonButton Open
            ribbonBtnOpen = new RibbonButton();
            ribbonBtnOpen.Name = "OpenRibbonBtn";
            ribbonBtnOpen.Text = "&Open";
            ribbonBtnOpen.SmallImage = ((Image)(ResourceRtfEditor.Open));
            ribbonBtnOpen.ShortcutKeys = Keys.Control | Keys.O;
            ribbonBtnOpen.Click += RibbonBtnOpen_Click;
            ribbonBtnOpen.ToolTip = ToolTipsResource.Open;

            //ribonButton Save
            ribbonBtnSave = new RibbonButton();
            ribbonBtnSave.Name = "SaveRibbonBtn";
            ribbonBtnSave.Text = "&Save";
            ribbonBtnSave.SmallImage = ((Image)(ResourceRtfEditor.Save));
            ribbonBtnSave.ShortcutKeys = Keys.Control | Keys.S; 
            ribbonBtnSave.Click += RibbonBtnSave_Click;
            ribbonBtnSave.ToolTip = ToolTipsResource.Save;

            //ribbonButton SaveAs
            ribbonBtnSaveAs = new RibbonSplitButton();
            ribbonBtnSaveAs.Name = "SaveRibbonBtn";
            ribbonBtnSaveAs.Text = "Save&As";
            ribbonBtnSaveAs.ShortcutKeys = Keys.F12;
            ribbonBtnSaveAs.SmallImage = ((Image)(ResourceRtfEditor.SaveAs));
            ribbonBtnSaveAs.Click += RibbonBtnSaveAs_Click;
            ribbonBtnSaveAs.ToolTip = ToolTipsResource.SaveAs;

            //ribbonButton Exit
            ribbonBtnExit = new RibbonButton();
            ribbonBtnExit.Name = "ExitRibbonBtn";
            ribbonBtnExit.Text = "E&xit";
            ribbonBtnExit.SmallImage = ((Image)(ResourceRtfEditor.Exit));
            ribbonBtnExit.ShortcutKeys = Keys.Alt | Keys.X;
            ribbonBtnExit.Click += RibbonBtnExit_Click;
            ribbonBtnExit.ToolTip = ToolTipsResource.Exit;
            
            //ribbonApplicationMainMenu
            ribbonAppMainMenu.Name = "MainMenu";
            ribbonAppMainMenu.SmallImage = ((Image)(ResourceRtfEditor.MainMenu));
            ribbonAppMainMenu.KeyTip = KeyTipsResource.MainMenu;
            ribbonAppMainMenu.LeftPaneItems.Add(ribbonBtnNew);
            ribbonAppMainMenu.LeftPaneItems.Add(ribbonBtnOpen);
            ribbonAppMainMenu.LeftPaneItems.Add(ribbonBtnSave);
            ribbonAppMainMenu.LeftPaneItems.Add(ribbonBtnSaveAs);
            ribbonAppMainMenu.LeftPaneItems.Add(ribbonBtnExit);
           
            //C1Ribbon
            ribbonTextEditor.AutoSizeElement = AutoSizeElement.Width;
            ribbonTextEditor.Location = new Point(0, 0);
            ribbonTextEditor.Name = "RibbonTextEditor";
            ribbonTextEditor.Size = new Size(500, 130);
            ribbonTextEditor.Dock = DockStyle.Top;
            ribbonTextEditor.Tabs.Add(ribbonTabHome);
            ribbonTextEditor.Tabs.Add(ribbonTabView);
            
            //RichTextBox Editor
            rchTextBoxEditor = new RichTextBox();
            rchTextBoxEditor.Dock = DockStyle.Fill;
            rchTextBoxEditor.Location = new Point(0, 275);
            rchTextBoxEditor.AutoWordSelection = true;
            rchTextBoxEditor.Name = "richTextBox1";
            rchTextBoxEditor.Size = new Size(800, 295);
            rchTextBoxEditor.TabIndex = 3;
            rchTextBoxEditor.Text = String.Empty;
            rchTextBoxEditor.SelectionChanged += RchTextBoxEditor_SelectionChanged;
            rchTextBoxEditor.ScrollBars = RichTextBoxScrollBars.Both;
            rchTextBoxEditor.LinkClicked += RchTextBoxEditor_LinkClicked;
            rchTextBoxEditor.TextChanged += RchTextBoxEditor_TextChanged;
            rchTextBoxEditor.MouseUp += ApplyFormatPainter;
            rchTextBoxEditor.KeyDown += RchTextBoxEditor_KeyDown;

            this.Controls.Add(rchTextBoxEditor);
            this.Controls.Add(ribbonTextEditor);
            this.ResumeLayout(false);
            this.PerformLayout();
            #endregion

            DictTextAlign = new Dictionary<RibbonToggleButton, HorizontalAlignment>() {
                { ribbonBtnAlignLeft , HorizontalAlignment.Left },
                { ribbonBtnAlignCenter , HorizontalAlignment.Center },
                { ribbonBtnAlignRight , HorizontalAlignment.Right }
            };

        }
        public C1Ribbon GetRibbonControl()
        {
            return ribbonTextEditor;
        }
        /// <summary>
        /// This method called when Text Changed in rchTextBox
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void RchTextBoxEditor_TextChanged(object sender, EventArgs e)
        {
            RchTextBoxEditor_SelectionChanged(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">A FileEventArgs</param>
        private void _fileInputOuput_SavedFile(object sender, FileEventsArgs e)
        {
            RibbonBtnSave_Click(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">A FileEventArgs</param>
        private void _fileInputOuput_NewFileSaved(object sender, FileEventsArgs e)
        {
            NewFile = false;
            _fileName = e.FileName;
            settingUtils.UpdateRecentFiles(e.FileName);
        }
        /// <summary>
        /// Create a new File
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void RibbonBtnNew_Click(object sender, EventArgs e)
        {
            _fileInputOuput.NewFile(ResourceRtfEditor.fileTypes.Split(',')[0]);
        }
        /// <summary>
        /// New File Created Event Invoked
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">A FileEventArgs</param>
        private void _fileInputOuput_NewFileCreated(object sender, FileEventsArgs e)
        {
            Form frm = this.FindForm();
            dynamic instance = Activator.CreateInstance(frm.GetType());
            instance.Show();
        }
        /// <summary>
        /// File Saved Event Invoked 
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">A FileEventArgs</param>
        private void _fileInputOuput_FileSaved(object sender, FileEventsArgs e)
        {
            _fileName = e.FileName;
            SaveFile(e.FileName, e.FileType);
        }
        /// <summary>
        /// To save as a new file  when saveAs Button clicked
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void RibbonBtnSaveAs_Click(object sender, EventArgs e)
        {
            _fileInputOuput.SaveAsFile();
        }
        /// <summary>
        /// To save a file  when save Button clicked
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void RibbonBtnSave_Click(object sender, EventArgs e)
        {
            if(NewFile)
            {
                _fileInputOuput.SaveAsFile();
                NewFile = false;
            }
            if(rchTextBoxEditor.Modified)
            {
               SaveFile(_fileName, Path.GetExtension(_fileName));
            }
        }
        /// <summary>
        /// To save a file given <paramref name="fileName"/> fileNAme and <paramref name="fileType"/> fileType
        /// </summary>
        /// <param name="fileName">A string</param>
        /// <param name="fileType">A string</param>
        private void SaveFile(string fileName,string fileType) 
        {
            rchTextBoxEditor.SaveFile(fileName,streamType[Path.GetExtension(fileName)]);
            rchTextBoxEditor.Modified = false;
            settingUtils.UpdateRecentFiles(fileName);
            Preview p = new Preview();
            p.GenerateImage(fileName);
            ParentForm.Text = ParentForm.Text.Split(' ')[0]+ (" - " + Path.GetFileNameWithoutExtension(fileName));
        }
        /// <summary>
        /// Method to be called before Closing of Form to check whether a file saved or not.
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(rchTextBoxEditor.Modified || NewFile)
            {
                e.Cancel = _fileInputOuput.CheckForSaveFile(NewFile);  
            }
            if(e.Cancel)
            {
                ParentForm.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }
        /// <summary>
        /// Link Clicked 
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An LinkClickedEventArgs</param>
        private void RchTextBoxEditor_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if(MessageBox.Show(ResourceRtfEditor.LinkWarning,ResourceRtfEditor.IDE,MessageBoxButtons.OKCancel,MessageBoxIcon.Warning)==DialogResult.OK)
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
        }
        /// <summary>
        /// To open a new File in new Form
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void _fileInputOuput_FileOpened(object sender, FileEventsArgs e)
        {
            Form frm = this.FindForm();
            dynamic instance = Activator.CreateInstance(frm.GetType(),new object[] { e.FileName });
            instance.Show();
        }
        /// <summary>
        /// To open A file 
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void RibbonBtnOpen_Click(object sender, EventArgs e)
        {
            _fileInputOuput.OpenFile();
        }
        /// <summary>
        /// Close The Appliation 
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void RibbonBtnExit_Click(object sender, EventArgs e)
        {
            ParentForm.Close();
        }
        /// <summary>
        /// Apply Undo to RichTextBox
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void RibbonBtnUndo_Click(object sender, EventArgs e)
        {
            if(rchTextBoxEditor.CanUndo)
            {
                rchTextBoxEditor.Undo();
            }
        }
        /// <summary>
        /// Apply Redo  to RichTextBox
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void RibbonBtnRedo_Click(object sender, EventArgs e)
        {
            if(rchTextBoxEditor.CanRedo)
            {
                rchTextBoxEditor.Redo();
            }
        }
        /// <summary>
        /// Load the File in RichTextBox
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void RtfEditor_Load(object sender, EventArgs e)
        {
            if (_fileName != String.Empty)
            {
                try
                {
                    rchTextBoxEditor.LoadFile(_fileName,streamType[Path.GetExtension(_fileName)]);
                    settingUtils.UpdateRecentFiles(_fileName);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    SetForNewFile();
                }
            }
            else  SetForNewFile();
            rchTextBoxEditor.SelectionStart = 0;
            RchTextBoxEditor_SelectionChanged(rchTextBoxEditor, e);
            rchTextBoxEditor.Modified = false;
            ParentForm.FormClosing += ParentForm_FormClosing;
            ParentForm.Text += (" - " + Path.GetFileNameWithoutExtension(_fileName));
        }
        /// <summary>
        /// Initialize form for new File.
        /// </summary>
        private void SetForNewFile() {
            NewFile = true;
            rchTextBoxEditor.Font = new Font(ResourceRtfEditor.DefaultFontFamily, float.Parse( ResourceRtfEditor.DefaultFontSize));
            _fileName = Environment.SpecialFolder.MyDocuments + "\\document" + FileInputOuput.CountNewFileOpened.ToString() + ".rtf";
        }
        /// <summary>
        /// Commit The Change of Font Family to selected Text
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void RibbonFontComboBoxFamily_IndexChanged(object sender, EventArgs e)
        {
            if (_updations) return;
            ribbonFontComboBoxFamily.SelectedIndex = ribbonFontComboBoxFamily.Items.IndexOf(_fontUtils.ChangeFont(rchTextBoxEditor.SelectionFont, ribbonFontComboBoxFamily.Text));
            rchTextBoxEditor.Focus();
        }
        /// <summary>
        /// Commit The Change of Font Size to selected Text
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void RibbonComboBoxSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updations) return;
            float FontSize;
            if (!float.TryParse(ribbonComboBoxSize.Text, out FontSize))
            {
                FontSize = GetSelectedTextFont().Size;
            }
            ribbonComboBoxSize.Text = _fontUtils.ChangeFont(GetSelectedTextFont(), FontSize).ToString();
            rchTextBoxEditor.Focus();
        }
        /// <summary>
        /// Call The method to Change the FontStyle of Selected Text
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void RibbonbtnFontStyle_PressedChanged(object sender, EventArgs e)
        {
            if (_updations) return;
            var ApplyStyle = DictFontStyle[((RibbonToggleButton)sender)];
            _fontUtils.ChangeFont(GetSelectedTextFont(), ApplyStyle, ((RibbonToggleButton)sender).Pressed);
        }
        /// <summary>
        /// Calls the method to change the Fore Color of Selected Text.
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void RibbonFontColorPicker_SelectedColorChanged(object sender, EventArgs e)
        {
            _fontUtils.ChangeForeColor(ribbonFontColorPicker.Color);
        }
        /// <summary>
        /// Changes the Fore color of  Selected Text
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">A FontEventArgs</param>
        private void Font_ChangedForeColor(object sender, FontEventArgs e)
        {
            rchTextBoxEditor.SelectionColor = e.fontForeColor;
        }
        /// <summary>
        /// Calls the method to change the Highlight Color of Selected Text.
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void RibbonBackColorPicker_SelectedColorChanged(object sender, EventArgs e)
        {
            _fontUtils.ChangeBackColor(ribbonBackColorPicker.Color);
        }
        /// <summary>
        /// Changes the Highlight color of  Selected Text
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">A FontEventArgs</param>
        private void Font_ChangedBackColor(object sender, FontEventArgs e)
        {
            rchTextBoxEditor.SelectionBackColor = e.fontBackColor;
        }
        /// <summary>
        /// Changes the Font of SelectedText
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">A FontEventArgs</param>
        private void Font_ChangedFont(object sender, FontEventArgs e)
        {
            rchTextBoxEditor.SelectionFont = e.font;
        }
        /// <summary>
        /// When Selected Font Changes and Applied to Ribbon Items
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">A  FontEventArgs</param>
        private void Font_FontChanged(object sender, FontEventArgs e)
        {
            try
            {
                ribbonFontComboBoxFamily.SelectedIndex = ribbonFontComboBoxFamily.Items.IndexOf(e.font.FontFamily.Name);
                ribbonComboBoxSize.Text = e.font.Size.ToString();
                ApplyFontStyleToButtons(e.font);
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex);
                ribbonFontComboBoxFamily.Text = String.Empty;
                ribbonComboBoxSize.Text = String.Empty;
                ApplyFontStyleToButtons(new Font(ResourceRtfEditor.DefaultFontFamily, float.Parse(ResourceRtfEditor.DefaultFontSize), FontStyle.Regular));
            }
        }
        /// <summary>
        /// When the selectiion in richTextBox changes
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void RchTextBoxEditor_SelectionChanged(object sender, EventArgs e)
        {
            _updations = true;
            ApplyChangesToClipboardItems(rchTextBoxEditor.SelectionLength > 0);
            ApplyChangesToQat();
            _fontUtils.FontChange(rchTextBoxEditor.SelectionFont);
            _pharagraph.AlignmentChange(rchTextBoxEditor.SelectionAlignment);
            _updations = false;
        }
        /// <summary>
        /// Apply FontStyles to  FontStyleToolbar Buttons
        /// </summary>
        /// <param name="font">A Font</param>
        private void ApplyFontStyleToButtons(Font font)
        {
            ribbonbtnBold.Pressed = font.Bold;
            ribbonbtnItalic.Pressed = font.Italic;
            ribbonbtnStrikeout.Pressed = font.Strikeout;
            ribbonbtnUnderline.Pressed = font.Underline;
        }
        /// <summary>
        /// It returns the Font of selected Text.
        /// </summary>
        /// <returns>A Font</returns>
        private Font GetSelectedTextFont()
        {
            return rchTextBoxEditor.SelectionFont;
        }
        /// <summary>
        /// Apply Changes to Clipboard Item like Cut and Copy Enable or disable.
        /// </summary>
        /// <param name="EnableButton">A boolean</param>
        private void ApplyChangesToClipboardItems(bool EnableButton)
        {
            ribbonBtnCopy.Enabled = EnableButton;
            ribbonBtnCut.Enabled = EnableButton;
        }
        /// <summary>
        /// Apply Changes to Qat buttons Undo and Redo
        /// </summary>
        private void ApplyChangesToQat()
        {
            ribbonBtnUndo.Enabled = rchTextBoxEditor.CanUndo;
            ribbonBtnRedo.Enabled = rchTextBoxEditor.CanRedo;
        }
        /// <summary>
        /// Performs Basic Editing Operations like cut, copy and paste
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void RibbonBtnCut_Click(object sender, EventArgs e)
        {
            switch (((RibbonButton)sender).Name)
            {
                case "PasteRibbonButton":
                    rchTextBoxEditor.Paste();
                    break;
                case "CutRibbonButton":
                    rchTextBoxEditor.Cut();
                    break;
                case "CopyRibbonButton":
                    rchTextBoxEditor.Copy();
                    break;
            }
        }
        /// <summary>
        /// Paste the Text from memory to richTextBox.
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void RibbonSplitBtnPaste_Click(object sender, EventArgs e)
        {
            rchTextBoxEditor.Paste();
        }
        /// <summary>
        /// Apply Changed Alignment to richTextBox
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">A ParagraphEventArgs</param>
        private void Paragraph_AlignmentChanged(object sender, ParagraphEventArgs e)
        {
            rchTextBoxEditor.SelectionAlignment = e.TextAlign;
        }
        /// <summary>
        /// Trigers when button from ToolBarAlign Pressed
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void RibbonBtnAlign_PressedChanged(object sender, EventArgs e)
        {
            var button = ((RibbonToggleButton)sender);
            if (!button.Pressed) { return; }
            _pharagraph.ChangeTextAlign(DictTextAlign[button]);
            ApplyPressedToAlignButtons(button);
        }
        /// <summary>
        /// Apply Changed Alignment to AlignButton
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">ParagraphEventArgs</param>
        private void Paragraph_ChangedAlignment(object sender, ParagraphEventArgs e)
        {
            switch (e.TextAlign)
            {
                case HorizontalAlignment.Left:
                    ApplyPressedToAlignButtons(ribbonBtnAlignLeft);
                    break;
                case HorizontalAlignment.Center:
                    ApplyPressedToAlignButtons(ribbonBtnAlignCenter);
                    break;
                case HorizontalAlignment.Right:
                    ApplyPressedToAlignButtons(ribbonBtnAlignRight);
                    break;
            }
        }
        /// <summary>
        /// Change the Press to ToolBarAlign Buttons
        /// </summary>
        /// <param name="ribbonToggleButton">A RibbonToggleButton</param>
        private void ApplyPressedToAlignButtons(RibbonToggleButton ribbonToggleButton)
        {
            ribbonToggleButton.Pressed = true;
        }
        private void RibbonBtnLeftIndent_Click(object sender, EventArgs e)
        {
            _pharagraph.LeftIndent(rchTextBoxEditor.SelectionIndent);
        }
        /// <summary>
        /// Apply Right Indent to Selected Text.
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void RibbonBtnRightIndent_Click(object sender, EventArgs e)
        {
            _pharagraph.RightIndent(rchTextBoxEditor.SelectionIndent);
        }/// <summary>
        /// When Indentaion Changed in richTextBox.
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">A ParagraphEventArgs</param>
        private void Paragraph_IndentationChanged(object sender, ParagraphEventArgs e)
        {
            rchTextBoxEditor.SelectionIndent = e.Indentation;
        }
        /// <summary>
        /// The Change in value of  RibbonComboBoxZoom committed. 
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void RibbonComboBoxZoom_ChangeCommitted(object sender, EventArgs e)
        {
            string ZoomStr = ribbonComboBoxZoom.Text;
            ZoomStr = ZoomStr.Replace(ResourceRtfEditor.PerCent, String.Empty);
            float ZoomValue;
            if (!float.TryParse(ZoomStr, out ZoomValue))
            {
                ZoomValue = rchTextBoxEditor.ZoomFactor * 100;
            }
            ribbonComboBoxZoom.Text = _zoom.ChangeZoom(ZoomValue).ToString() + ResourceRtfEditor.PerCent;
            rchTextBoxEditor.Focus();
        }
        /// <summary>
        /// This method when Zoom Value Changed.
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">ZoomEventArgs</param>
        private void Zoom_ZoomChanged(object sender, ZoomEventsArgs e)
        {
            rchTextBoxEditor.ZoomFactor = e.getZoomFactor / 100;
            ribbonLblZoom.Text = e.getZoomFactor.ToString();
        }

        private void RchTextBoxEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift && e.KeyCode == Keys.V)
            {
                ApplyFormatPainter(sender, e);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Apply Format that copied to selected Text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplyFormatPainter(object sender, EventArgs e)
        {
            rchTextBoxEditor.SelectionFont = _fontUtils.font;
            rchTextBoxEditor.SelectionColor = _fontUtils.foreColor;
            rchTextBoxEditor.SelectionBackColor = _fontUtils.BackColor;
            rchTextBoxEditor.Cursor = Cursors.IBeam;
        }
        /// <summary>
        /// To copy format of selected Text
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">Event Args</param>
        private void RibbonBtnFormatPainter_Click(object sender, EventArgs e)
        {
            _fontUtils.PutFormatPainter(GetSelectedTextFont(), rchTextBoxEditor.SelectionColor, rchTextBoxEditor.SelectionBackColor);
            rchTextBoxEditor.Cursor = Cursors.Hand;
        }
        /// <summary>
        /// This method returns object of  C1ibbon
        /// </summary>
        /// <returns>C1Ribbon</returns>

    }
}