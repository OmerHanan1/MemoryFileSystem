using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyMemFS
{
    internal class File
    {
        private string _fileName { get; set; }
        private DateTime _created { get; set; }
        private List<byte> _data { get; set; }
        private int _fileSize { get; set; }
        private string _formattedFileSize { get; set; }

        public File(string fileName)
        {
            this._fileName = fileName;
            this._created = DateTime.Now;
            this._data = new List<byte>();
            this._fileSize = 0;
            this._formattedFileSize = getFormattedFileSize(_fileSize);
        }

        public File(string fileName, List<byte> data)
        {
            this._fileName = fileName;
            this._data = data;
            this._created = DateTime.Now;
            this._fileSize = data.Count;
            this._formattedFileSize = getFormattedFileSize(_fileSize);
        }

        public File(File file)
        {
            this._fileName = file._fileName;
            this._created = file._created;
            this._data = file._data;
            this._fileSize = file._fileSize;
            this._formattedFileSize= file._formattedFileSize;
        }

        public string getFormattedFileSize(int size) 
        {
            string[] orders = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (order < orders.Length - 1 && size >= 1024) 
            {
                order++;
                size /= 1024;
            }
            string result = "";
            result = $"{size}{orders[order]}";
            return result;
        }

        public override string ToString()
        {
            string result = "";
            result = $"{this._fileName}, {this._formattedFileSize}, {this._created}";
            return result;
        }
    }
}
