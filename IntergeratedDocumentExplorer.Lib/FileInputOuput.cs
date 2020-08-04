using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IntergeratedDocumentExplorer.Lib
{
    public class FileInputOuput : IFileInputOutput
    {
        #region Private Variable declarations
        private OpenFileDialog openDialog;
        private SaveFileDialog saveDialog;
        private DialogResult dialogResult;
        private string[] _fileTypes;
        private string _fileName;
        #endregion

        public static int CountNewFileOpened;

        public event EventHandler<FileEventsArgs> NewFileCreated;
        public event System.EventHandler<FileEventsArgs> FileOpened;
        public event System.EventHandler<FileEventsArgs> FileSaved;
        public event EventHandler<FileEventsArgs> NewFileSaved;
        public event EventHandler<FileEventsArgs> SavedFile;
        public event EventHandler<FileEventsArgs> FileNotExisted;

        public FileInputOuput(string[] fileTypes)
        {
            Initialize();
            _fileTypes = fileTypes;
            string filterStr = String.Empty;
            foreach (string fileType in fileTypes)
            {
                string fileFilter = fileType + ResourceFileIO.file + fileType + ResourceFileIO.orAsterik + fileType + ResourceFileIO.or;
                filterStr += fileFilter;
            }
            filterStr += ResourceFileIO.allfiles;
            openDialog.Filter = filterStr;
            saveDialog.Filter = filterStr;
        }
        public FileInputOuput()
        {
            Initialize();
            string filterStr = ResourceFileIO.allfiles;
            openDialog.Filter = filterStr;
            saveDialog.Filter = filterStr;
        }
        /// <summary>
        /// Initilize the Private Variables
        /// </summary>
        private void Initialize()
        {
            openDialog = new OpenFileDialog();
            openDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openDialog.FilterIndex = 1;
            openDialog.RestoreDirectory = true;

            saveDialog = new SaveFileDialog();
            saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveDialog.RestoreDirectory = true;
           
        }
        /// <summary>
        /// To Create A new File
        /// </summary>
        /// <param name="fileType">A string</param>
        public void NewFile(string fileType)
        {
            CountNewFileOpened += 1;
            FileEventsArgs args = new FileEventsArgs();
            args.FileType = fileType;
            OnFileNew(args);
        }
        /// <summary>
        /// To open A file given File Name <paramref name="fileName"/>
        /// </summary>
        /// <param name="fileName">A string</param>
        public void OpenFile(string fileName)
        {
            FileEventsArgs args = new FileEventsArgs();
            args.FileName = fileName;
            args.FileType = Path.GetExtension(fileName);
            if (!File.Exists(fileName)) {
                Settings.Default.RecentFiles.Remove(fileName);
                Settings.Default.Save();
                OnFileNotExists(args);
                return;
            }
            OnFileOpen(args);
        }
        /// <summary>
        /// To Show File Not Found Messaage which takes parameter <paramref name="fileName"/> 
        /// </summary>
        /// <param name="fileName">A string</param>
        public static void ShowFileNotFound(string fileName)
        {
            MessageBox.Show(fileName + ResourceFileIO.NotFound, ResourceFileIO.caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// To Open A file using Dialog Box
        /// </summary>
        public void OpenFile()
        {
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                _fileName = openDialog.FileName;
                try
                {
                    if (checkForExtension(_fileName))
                    {
                        OpenFile(_fileName);
                    }
                    else
                        throw new ArgumentException();
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show(_fileName + ResourceFileIO.formatInvalid, ResourceFileIO.caption, MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                catch (IOException ex)
                {
                    System.Console.WriteLine(ex.Message);
                    MessageBox.Show(ResourceFileIO.errorFileLoad + _fileName, ResourceFileIO.caption, MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }
        /// <summary>
        /// Check For Extension
        /// </summary>
        /// <param name="fileName">A string</param>
        /// <returns></returns>
        private bool checkForExtension(string fileName)
        {
            foreach(string fileType in _fileTypes)
            {
                if(Path.GetExtension(fileName) == fileType) { return true; }
            }
            return false;
        }
        /// <summary>
        /// Check Whether a file is modified or not
        /// </summary>
        /// <param name="fileNew">A boolean</param>
        /// <returns></returns>
        public bool CheckForSaveFile(bool fileNew)
        {
            dialogResult = MessageBox.Show(ResourceFileIO.SavePrompt, ResourceFileIO.caption, MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                if(fileNew && SaveAsFile())
                 {
                        FileEventsArgs args = new FileEventsArgs();
                        args.FileName = saveDialog.FileName;
                        args.FileType = _fileTypes[saveDialog.FilterIndex - 1];
                        OnNewFileSave(args);
                }
                else
                {
                    FileEventsArgs args = new FileEventsArgs();
                    SavedFile(this, args);
                }
            }
            else if(dialogResult == DialogResult.No)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// To Save a File as new File.
        /// </summary>
        /// <returns> A boolean that file is saved or not.</returns>
        public bool SaveAsFile()
        {
            if (saveDialog.ShowDialog() == DialogResult.OK && saveDialog.FileName.Length > 0)
            {
                FileStream fs = (FileStream)saveDialog.OpenFile();
                fs.Close();
                FileEventsArgs args = new FileEventsArgs();
                args.FileName = saveDialog.FileName;
                try
                {
                    args.FileType = _fileTypes[saveDialog.FilterIndex - 1];
                    OnFileSaveAs(args);
                    return true;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    //args.FileType = Path.GetExtension(saveDialog.FileName);
                }
            }
            return false;
        }
        /// <summary>
        /// Invokes an event of FileSavedAsNewFile
        /// </summary>
        /// <param name="e">FileEventsArgs</param>
        protected virtual void OnFileSaveAs(FileEventsArgs e)
        {
            if (this.FileSaved != null)
            {
                this.FileSaved(this, e);
            }
        }
        /// <summary>
        /// Invokes an event of FileNotExists
        /// </summary>
        /// <param name="e">FileEventsArgs</param>
        protected virtual void OnFileNotExists(FileEventsArgs e)
        {
            if (this.FileNotExisted != null)
            {
                this.FileNotExisted(this, e);
            }
        }
        /// <summary>
        /// Invokes an event of NewFileSaved
        /// </summary>
        /// <param name="e">FileEventsArgs</param>
        protected virtual void OnNewFileSave(FileEventsArgs e)
        {
            if (this.NewFileSaved != null)
            {
                this.NewFileSaved(this, e);
            }
        }
        /// <summary>
        /// Invokes an event of FileNew
        /// </summary>
        /// <param name="e">FileEventsArgs</param>
        protected virtual void OnFileNew(FileEventsArgs e)
        {
            if (this.NewFileCreated != null)
            {
                this.NewFileCreated(this, e);
            }
        }
        /// <summary>
        /// Invokes an event of FileOpened
        /// </summary>
        /// <param name="e">FileEventsArgs</param>
        protected virtual void OnFileOpen(FileEventsArgs e)
        {
            if (this.FileOpened != null)
            {
                this.FileOpened(this, e);
            }
        }
    }
}