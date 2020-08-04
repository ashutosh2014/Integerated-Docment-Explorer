using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IntergeratedDocumentExplorer.Lib;
using C1.Win.C1Tile;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Collections.Specialized;

namespace IntegeratedDocumentExplorer
{
    public partial class Main : Form
    {
        #region Control declaration
        private SplitContainer splitVertically;
        private FlowLayoutPanel sideBarFlowLayout;
        private FlowLayoutPanel logoFlowLayout;
        private PictureBox pictureBoxLogo;
        private Label lblLogo;
        private Button btnRecents;
        private Button btnRTF;
        private FlowLayoutPanel topbarFlowLayout;
        private Label lblNew;
        private ContextMenuStrip newMenuStrip;
        private ToolStripLabel lblRTFDoc;
        private C1TileControl tileControlRecentFiles;
        private Group tileGroupRecentFiles;
        private Template templateRecentFiles;
        private Font fontCalibri;
        #endregion

        #region Lib declaration
        private FileInputOuput fileInputOuput;
        private Preview preview;
        private SettingUtils settingUtils;
        #endregion 
        public Main()
        {
            InitializeComponent();
            #region Control and Lib Initialization
            fontCalibri = new Font("Calibiri", 12);
            fileInputOuput = new FileInputOuput();
            fileInputOuput.NewFileCreated += FileInputOuput_NewFileCreated;
            fileInputOuput.FileOpened += FileInputOuput_FileOpened;
            fileInputOuput.FileNotExisted += FileInputOuput_FileNotExisted;

            preview = new Preview();
            settingUtils = new SettingUtils();

            splitVertically = new SplitContainer();
            splitVertically.Dock = DockStyle.Fill;
            splitVertically.Location = new Point(0, 0);
            splitVertically.Name = "splitVertical";
            splitVertically.SplitterDistance = 1000;

            sideBarFlowLayout = new FlowLayoutPanel();
            sideBarFlowLayout.Dock = DockStyle.Fill;
            sideBarFlowLayout.FlowDirection = FlowDirection.TopDown;
            sideBarFlowLayout.Name = "sideBarFlowLayout";
            sideBarFlowLayout.BackColor = Color.FromArgb(50, 74, 178);
            sideBarFlowLayout.Size = new Size(300, 300);
            splitVertically.Panel1.Controls.Add(sideBarFlowLayout);

            logoFlowLayout = new FlowLayoutPanel();
            logoFlowLayout.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            logoFlowLayout.AutoSize = true;
            logoFlowLayout.Name = "logoFlowLayout";
            logoFlowLayout.Padding = new Padding(8);
            sideBarFlowLayout.Controls.Add(logoFlowLayout);

            pictureBoxLogo = new PictureBox();
            pictureBoxLogo.Name = "LogoPicture";
            pictureBoxLogo.Size = new Size(30, 30);
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxLogo.Image = ((Image)(MainResource.LogoIDE));
            logoFlowLayout.Controls.Add(pictureBoxLogo);

            lblLogo = new Label();
            lblLogo.Location = new Point(49, 0);
            lblLogo.Name = "label1";
            lblLogo.Size = new Size(193, 30);
            lblLogo.Text = "Documents";
            lblLogo.Font = new Font("Arial", 16, FontStyle.Bold);
            logoFlowLayout.Controls.Add(lblLogo);
            lblLogo.TextAlign = ContentAlignment.MiddleLeft;
            lblLogo.Padding = new Padding(0, 8, 8, 0);
            lblLogo.ForeColor = Color.White;

            btnRecents = new Button();
            btnRecents.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnRecents.AutoSize = true;
            btnRecents.FlatAppearance.BorderSize = 0;
            btnRecents.FlatStyle = FlatStyle.Flat;
            btnRecents.Location = new Point(3, 65);
            btnRecents.Name = "recentbtn";
            btnRecents.Text = "Recent";
            btnRecents.ForeColor = Color.White;
            btnRecents.Margin = new Padding(0);
            btnRecents.Font = fontCalibri;
            btnRecents.TextAlign = ContentAlignment.MiddleLeft;
            btnRecents.Padding = new Padding(8);
            btnRecents.BackColor = Color.Green;
            btnRecents.Click += BtnRecents_Click;
            btnRecents.MouseHover += ShowToolTipsButtons;
            sideBarFlowLayout.Controls.Add(btnRecents);

            btnRTF = new Button();
            btnRTF.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnRTF.AutoSize = true;
            btnRTF.FlatAppearance.BorderSize = 0;
            btnRTF.FlatStyle = FlatStyle.Flat;
            btnRTF.Location = new Point(0, 97);
            btnRTF.Margin = new Padding(0);
            btnRTF.Padding = new Padding(8);
            btnRTF.TextAlign = ContentAlignment.MiddleLeft;
            btnRTF.Name = "rtfbtn";
            btnRTF.Text = "RTF Docs";
            btnRTF.Font = fontCalibri;
            btnRTF.ForeColor = Color.White;
            btnRTF.Click += BtnRecents_Click;
            btnRTF.UseVisualStyleBackColor = true;
            btnRTF.MouseHover += ShowToolTipsButtons;
            sideBarFlowLayout.Controls.Add(btnRTF);

            topbarFlowLayout = new FlowLayoutPanel();
            topbarFlowLayout.Left = -20;
            topbarFlowLayout.Dock = DockStyle.Top;
            topbarFlowLayout.Padding = new Padding(8);
            topbarFlowLayout.BackColor = Color.FromArgb(224, 224, 224);
            topbarFlowLayout.Name = "TopBar";
            topbarFlowLayout.Height = 52;

            lblNew = new Label();
            lblNew.Text = "New  " + '\u25BC';
            lblNew.Name = "NewLbl";
            lblNew.Image = ((Image)(MainResource.add));
            lblNew.ImageAlign = ContentAlignment.MiddleLeft;
            lblNew.TextAlign = ContentAlignment.MiddleRight;
            lblNew.Font = fontCalibri;
            lblNew.Size = new Size(90, 35);
            lblNew.Click += LblNew_Click;
            topbarFlowLayout.Controls.Add(lblNew);

            splitVertically.Panel1.BackColor = Color.FromArgb(50, 74, 178);
            splitVertically.Size = new Size(800, 450);
            splitVertically.SplitterDistance = 220;
            splitVertically.FixedPanel = FixedPanel.Panel1;
            splitVertically.BorderStyle = BorderStyle.None;
            splitVertically.SplitterWidth = 1;

            lblRTFDoc = new ToolStripLabel();
            lblRTFDoc.Name = "RTF Document";
            lblRTFDoc.Text = "Rich Text Document";
            lblRTFDoc.Image = ((Image)(MainResource.RTElogo));
            lblRTFDoc.Size = new Size(140, 55);
            lblRTFDoc.Padding = new Padding(7);
            lblRTFDoc.Font = new Font("Calibiri", 10);
            lblRTFDoc.ToolTipText = MainResource.NewRTfDoc;
            lblRTFDoc.Click += LblRTFDoc_Click;

            newMenuStrip = new ContextMenuStrip();
            newMenuStrip.Name = "NewContextMenuStrip";
            newMenuStrip.Items.Add(lblRTFDoc);
            newMenuStrip.ShowImageMargin = false;

            tileControlRecentFiles = new C1TileControl();
            tileControlRecentFiles.Name = "RecentFilesTileControl";
            tileControlRecentFiles.Orientation = LayoutOrientation.Vertical;
            tileControlRecentFiles.Dock = DockStyle.Fill;
            tileControlRecentFiles.TileBorderColor = Color.Black;
            tileControlRecentFiles.GroupPadding = new Padding(20, 40, 20, 20);
            tileControlRecentFiles.Padding = new Padding(0);
            tileControlRecentFiles.Location = new Point(0, 51);
            tileControlRecentFiles.CellSpacing = 20;
            tileControlRecentFiles.CellHeight = 200;
            tileControlRecentFiles.CellWidth = 200;

            tileGroupRecentFiles = new Group();
            tileGroupRecentFiles.Name = "RecentFilesGroup";
            tileControlRecentFiles.Groups.Add(tileGroupRecentFiles);

            splitVertically.Panel2.Controls.Add(tileControlRecentFiles);
            splitVertically.Panel2.Controls.Add(topbarFlowLayout);
            splitVertically.Panel2.BackColor = Color.White;

            this.Controls.Add(splitVertically);
            #endregion
            InitializeTemplate();
            this.MaximizeBox = true;
            GenerateThumbnails();
            AddTiles();

            //AddTiles(new string[] { ".rtf", ".txt" });
        }
        private void ShowToolTipsButtons(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip((Control)sender, MainResource.ResourceManager.GetString(((Control)sender).Name));

        }
        /// <summary>
        /// File Not Exised Event Invoked
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">A FileEventArgs</param>
        private void FileInputOuput_FileNotExisted(object sender, FileEventsArgs e)
        {
            tileGroupRecentFiles.Tiles.Remove(e.FileName);
            FileInputOuput.ShowFileNotFound(e.FileName);
        }
        /// <summary>
        /// To Open a File
        /// </summary>
        /// <param name="sender">An Object</param>
        /// <param name="e">A FileEventArgs</param>
        private void FileInputOuput_FileOpened(object sender, FileEventsArgs e)
        {
            RtfEditor rtfEditor;
            switch (e.FileType)
            {
                case ".rtf":
                    rtfEditor = new RtfEditor(e.FileName);
                    rtfEditor.Show();
                    break;
                case ".txt":
                    rtfEditor = new RtfEditor(e.FileName);
                    rtfEditor.Show();
                    break;
            }
        }
        /// <summary>
        /// Generate Thumbnail of all Recent Files
        /// </summary>
        public void GenerateThumbnails()
        {
            foreach (string s in settingUtils.GetRecentFiles())
            {
                preview.GenerateImage(s);
            }
        }
        /// <summary>
        /// Add Tiles of all file Types
        /// </summary>
        public void AddTiles()
        {
            StringCollection recentFiles = settingUtils.GetRecentFiles();
            for (int i = recentFiles.Count - 1 ; i >= 0; i--)
            {
                AddTile(recentFiles[i]);
            }
        }
        /// <summary>
        /// Add Tiles of a specicfic File types <paramref name="fileTypes"/>
        /// </summary>
        /// <param name="fileTypes">An arry of sting</param>
        public void AddTiles(string[] fileTypes)
        {
            StringCollection recentFiles = settingUtils.GetRecentFiles();
            for (int i = recentFiles.Count - 1; i >= 0; i--)
            {
                if (fileTypes.Contains(Path.GetExtension(recentFiles[i])))
                {
                    AddTile(recentFiles[i]);
                }
            }
        }
        /// <summary>
        /// Add a tile to Tile Contol with file <paramref name="fileName"/>
        /// </summary>
        /// <param name="fileName">A string</param>
        public void AddTile(string fileName)
        {
            #region Tile declaration and Initialization
            Tile tile = new Tile();
            tile.BackColor = Color.White;
            tile.BackColor1 = Color.White;
            tile.BackColor2 = Color.Black;
            tile.BackColor3 = Color.White;
            tile.BackColor4 = Color.White;
            tile.Image = preview.GetImage(fileName);
            tile.Image2 = preview.GetLogo(fileName);
            tile.Name = fileName;
            tile.Template = templateRecentFiles;
            tile.Text4 = Path.GetFileName(fileName);
            tile.Text5 = preview.GetModifiedDate(fileName);
            tile.ForeColor4 = Color.Black;
            tile.ForeColor5 = Color.Gray;
            tile.Click += Tile_Click;
            tileGroupRecentFiles.Tiles.Add(tile);
            #endregion
        }
        /// <summary>
        /// To open a File
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void Tile_Click(object sender, EventArgs e)
        {
            fileInputOuput.OpenFile(((Tile)sender).Name);
        }
        /// <summary>
        /// New File Created Method Invoked
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">A FileEventArgs</param>

        private void FileInputOuput_NewFileCreated(object sender, FileEventsArgs e)
        {
            switch (e.FileType)
            {
                case ".rtf":
                    CreateNewRTFFile();
                    break;
                default:
                    MessageBox.Show(MainResource.ErrorNewFile, MainResource.caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }
        /// <summary>
        /// Create a new RTF File.
        /// </summary>
        private void CreateNewRTFFile()
        {
            RtfEditor rtfEditor = new RtfEditor();
            rtfEditor.Show();
        }
        /// <summary>
        /// new File of rtf Type
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void LblRTFDoc_Click(object sender, EventArgs e)
        {
            fileInputOuput.NewFile(MainResource.rtf);
        }
        /// <summary>
        /// To Open Contenxt Menu
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An Event Args</param>
        private void LblNew_Click(object sender, EventArgs e)
        {
            newMenuStrip.Show(lblNew, new Point(8, 40));
        }
        /// <summary>
        /// Call the method to change color of buttons and content of tileContol.
        /// </summary>
        /// <param name="sender">An object</param>
        /// <param name="e">An EventArgs</param>
        private void BtnRecents_Click(object sender, EventArgs e)
        {
            ChangeColor((Button)sender);
        }
        /// <summary>
        /// Change The Back Color of Buttons
        /// </summary>
        /// <param name="btnsender">Button</param>
        private void ChangeColor(Button btnsender)
        {
            btnRecents.BackColor = Color.FromArgb(50, 74, 178);
            btnRTF.BackColor = Color.FromArgb(50, 74, 178);
            if (btnsender.BackColor == Color.FromArgb(255, 50, 74, 178))
            {
                btnsender.BackColor = Color.Green;
                ChangeContent(btnsender);
            }
            else btnsender.BackColor = Color.FromArgb(255, 50, 74, 178);
        }
        /// <summary>
        /// Change the contenet of TileControl
        /// </summary>
        /// <param name="btnsender">Button</param>
        private void ChangeContent(Button btnsender)
        {
            tileGroupRecentFiles.Tiles.Clear();
            if (btnsender.Text == MainResource.Recent)
            {
                AddTiles();
            }
            else AddTiles(MainResource.ResourceManager.GetString(btnsender.Text).Split(','));
        }
        /// <summary>
        /// Initialize the template.
        /// </summary>
        private void InitializeTemplate()
        {
            #region template initialization
            templateRecentFiles = new Template();
            templateRecentFiles.Name = "ShowRecentFiles";
            ImageElement PreviewImageElemnt = new ImageElement();
            PanelElement pnlBottom = new PanelElement();
            PanelElement fileLogoMargin = new PanelElement();
            PanelElement fileDetailsLogo = new PanelElement();
            ImageElement fileTypeimg = new ImageElement();
            PanelElement fileDetailspnl = new PanelElement();
            TextElement fileNametxt = new TextElement();
            TextElement fileModifiedtxt = new TextElement();
            PreviewImageElemnt.Margin = new Padding(10);
            pnlBottom.BackColor = Color.White;
            pnlBottom.BackColorSelector = BackColorSelector.BackColor2;
            fileLogoMargin.Alignment = ContentAlignment.MiddleLeft;
            fileLogoMargin.AlignmentOfContents = ContentAlignment.TopRight;
            fileDetailsLogo.Alignment = ContentAlignment.MiddleLeft;
            fileDetailsLogo.BackColor = Color.White;
            fileDetailsLogo.BackColorSelector = BackColorSelector.BackColor1;
            fileTypeimg.Alignment = ContentAlignment.MiddleLeft;
            fileTypeimg.FixedHeight = 45;
            fileTypeimg.FixedWidth = 40;
            fileTypeimg.ImageLayout = ForeImageLayout.Stretch;
            fileTypeimg.ImageSelector = ImageSelector.Image2;
            fileTypeimg.Margin = new Padding(0, -7, 0, 0);
            fileNametxt.BackColor = Color.White;
            fileNametxt.ForeColor = Color.Black;
            fileNametxt.TextSelector = TextSelector.Text4;
            fileNametxt.Font = new Font("Arial", 12);
            fileNametxt.ForeColorSelector = ForeColorSelector.ForeColor4;
            fileNametxt.Alignment = ContentAlignment.TopLeft;
            fileModifiedtxt.BackColor = Color.White;
            fileModifiedtxt.ForeColor = Color.Black;
            fileModifiedtxt.ForeColorSelector = ForeColorSelector.ForeColor5;
            fileModifiedtxt.TextSelector = TextSelector.Text5;
            fileModifiedtxt.Font = new Font("Arial", 8);
            fileModifiedtxt.Alignment = ContentAlignment.BottomLeft;
            fileDetailspnl.Children.Add(fileNametxt);
            fileDetailspnl.Children.Add(fileModifiedtxt);
            fileDetailspnl.FixedWidth = 200;
            fileDetailspnl.Orientation = LayoutOrientation.Vertical;
            fileDetailsLogo.Children.Add(fileTypeimg);
            fileDetailsLogo.Children.Add(fileDetailspnl);
            fileLogoMargin.Children.Add(fileDetailsLogo);
            pnlBottom.Children.Add(fileLogoMargin);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.FixedHeight = 40;
            pnlBottom.Padding = new Padding(0, 1, 0, 0);
            templateRecentFiles.Elements.Add(PreviewImageElemnt);
            templateRecentFiles.Elements.Add(pnlBottom);
            #endregion
        }
    }
}
