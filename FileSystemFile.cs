using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyMemFS
{
    public class FileSystemFile
    {
        public string _fileName { get; set; }
        public DateTime _created { get; set; }
        public byte[] _data { get; set; }
        public int _fileSize { get; set; }
        public string _formattedFileSize { get; set; }
        public int _cryptoCounter { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileName">name for file</param>
        /// <param name="created">created time</param>
        /// <param name="data">file data in byte array</param>
        public FileSystemFile(string fileName, DateTime created, byte[] data)
        {
            _fileName = fileName;
            _created = created;
            _data = data;
            _fileSize = data.Length;
            _formattedFileSize = getFormattedFileSize(_fileSize);
            _cryptoCounter = 0;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="file">source file to copy from</param>
        public FileSystemFile(FileSystemFile file)
        {
            this._fileName = file._fileName;
            this._created = file._created;
            this._data = file._data;
            this._fileSize = file._fileSize;
            this._formattedFileSize= file._formattedFileSize;
            _cryptoCounter = 0;
        }

        /// <summary>
        /// formatted data size correctly with B/KB/M/MB/GB/TB suffix
        /// </summary>
        /// <param name="size">data byte array length</param>
        /// <returns>returns the file size with size format</returns>
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

        /// <summary>
        /// Overrided ToString function
        /// </summary>
        /// <returns>all needed file data in string format</returns>
        public override string ToString()
        {
            string result = "";
            result = $"{this._fileName}, {this._formattedFileSize}, {this._created}, {byteArrayToString(this._data)} ";
            return result;
        }

        public string byteArrayToString(byte[] ba)
        {
            string str = "";
            for (int i = 0; i < ba.Length; i++)
            {
                str += ba[i].ToString();
            }
            return str;
        }

        public byte[] getFileData()
        {
            return _data;
        }
    }
}
