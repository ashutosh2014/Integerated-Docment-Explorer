using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace IntergeratedDocumentExplorer.Lib
{
    public class Preview
    {

        private string _folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + ResourceFileIO.folderLocation;
        /// <summary>
        /// This returns the Last Modified Date of a file.
        /// </summary>
        /// <param name="fileName">A string</param>
        /// <returns>A string i.e. Modified Date</returns>
        public string GetModifiedDate(string fileName)
        {
            DateTime dateTime = File.GetLastWriteTime(fileName);
            return ResourceFileIO.Modified + dateTime.ToShortDateString();

        }
        /// <summary>
        /// Retuns the image related to file Type
        /// </summary>
        /// <param name="fileName">A string</param>
        /// <returns>An Image</returns>
        public Image GetImage(string fileName)
        {
            Image image;
            switch (Path.GetExtension(fileName))
            {
                case ".rtf":
                    image = GetImageforRTF(fileName);
                    break;
                case ".txt":
                    image = GetImageforRTF(fileName);
                    break;
                default:
                    image = ResourceFileIO.warning;
                    break;
            }
            return image;
        }
        /// <summary>
        /// This Check which Type of file is to generate an image
        /// </summary>
        /// <param name="fileName">A string</param>
        public void GenerateImage(string fileName)
        {
            switch (Path.GetExtension(fileName))
            {
                case ".rtf":
                    GenerateImageforRTF(fileName,RichTextBoxStreamType.RichText);
                    break;
                case ".txt":
                    GenerateImageforRTF(fileName, RichTextBoxStreamType.PlainText);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// It retunrs logo related to type of a File
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Image</returns>
        public Image GetLogo(string fileName)
        {
            Image image;
            switch (Path.GetExtension(fileName))
            {
                case ".rtf":
                    image = ResourceFileIO.RTElogo;
                    break;
                case ".txt":
                    image = ResourceFileIO.RTElogo;
                    break;
                default:
                    image = ResourceFileIO.warning;
                    break;
            }
            return image;
        }
        /// <summary>
        /// Generate image and save it to local system of a given File <paramref name="fileName"/> 
        /// </summary>
        /// <param name="fileName">A string</param>
        /// <param name="streamType">/a RichTextBoxStreamType</param>
        private void GenerateImageforRTF(string fileName, RichTextBoxStreamType streamType)
        {
            RichTextBox rtf = new RichTextBox();
            rtf.BorderStyle = BorderStyle.None;
            rtf.Width = 700;
            rtf.Height = 800;
            rtf.LoadFile(fileName, streamType);
            rtf.Select(0, 0);
            Bitmap bitmap = new Bitmap(500, rtf.Height);
            rtf.DrawToBitmap(bitmap, new Rectangle(0, 0, 500, rtf.Height));
            if (!Directory.Exists(_folder)) { Directory.CreateDirectory(_folder); }
            string imageName = _folder + ResourceFileIO.slash + Path.GetFileNameWithoutExtension(fileName)  + ResourceFileIO.png;
            bitmap.Save(imageName, System.Drawing.Imaging.ImageFormat.Png);
            bitmap.Dispose();
            rtf.Dispose();
        }
        /// <summary>
        /// It return the image of a related File 
        /// </summary>
        /// <param name="fileName">A string</param>
        /// <returns>Image of a file</returns>
        private Image GetImageforRTF(string fileName)
        {
            string imageName = _folder + ResourceFileIO.slash + Path.GetFileNameWithoutExtension(fileName) + ResourceFileIO.png;
            return Image.FromFile(imageName).GetThumbnailImage(500, 500, null, IntPtr.Zero);
        }
    }
}
