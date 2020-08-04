using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace IntergeratedDocumentExplorer.Lib
{
    public class SettingUtils
    {
        /// <summary>
        /// Update the recent Files
        /// </summary>
        /// <param name="FileName">A string</param>
        public void  UpdateRecentFiles(string FileName)
        {
            if(Settings.Default.RecentFiles.Contains(FileName)) { return ; }
            Settings.Default.RecentFiles.Add(FileName);
            if(Settings.Default.RecentFiles.Count > 30)
            {
                Settings.Default.RecentFiles.RemoveAt(0);
            }
            Settings.Default.Save();
        }
        /// <summary>
        /// To Get Recent Files
        /// </summary>
        /// <returns>A StringCollection</returns>
        public StringCollection GetRecentFiles()
        {
            return Settings.Default.RecentFiles;
        }
    }
}
