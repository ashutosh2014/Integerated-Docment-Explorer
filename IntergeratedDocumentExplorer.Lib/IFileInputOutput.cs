using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntergeratedDocumentExplorer.Lib
{
    public interface IFileInputOutput
    {
        event EventHandler<FileEventsArgs> NewFileCreated;
        event System.EventHandler<FileEventsArgs> FileOpened;
        event System.EventHandler<FileEventsArgs> FileSaved;
        event EventHandler<FileEventsArgs> NewFileSaved;
        event EventHandler<FileEventsArgs> SavedFile;
        event EventHandler<FileEventsArgs> FileNotExisted;
        void NewFile(string fileType);
        void OpenFile(string fileName);
        void OpenFile();
        bool CheckForSaveFile(bool fileNew);
        bool SaveAsFile();
    }
    
}