using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

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
            this._formattedFileSize = file._formattedFileSize;
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

    public class TinyMemFS
    {
        private ConcurrentDictionary<string,FileSystemFile> fileSystem;
        private static byte[] iv = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        /// <summary>
        /// Default constructor
        /// </summary>
        public TinyMemFS()
        {
            this.fileSystem = new ConcurrentDictionary<string,FileSystemFile>();
        }

        /// <summary>
        /// Add file to the file system
        /// </summary>
        /// <param name="fsf">File system file instance</param>
        public void AddFileToFS(FileSystemFile fsf)
        {
            if (fsf == null)
                return;
            this.fileSystem.TryAdd(fsf._fileName, fsf);
        }

        /// <summary>
        /// string representation of File system
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = "";
            foreach (FileSystemFile fsf in this.fileSystem.Values)
            {
                str += " ";
                str += fsf.ToString();
                //string tmp = fsf.byteArrayToString(fsf.getFileData());
                //str += Convert.ToBase64String(fsf.getFileData());
                str += "\n";
            }
            return str;
        }

        /// <summary>
        /// Example:
        /// add("name1.pdf", "C:\\Users\\user\Desktop\\report.pdf")
        /// note that fileToAdd isn't the same as the fileName
        /// </summary>
        /// <param name="fileName">The name of the file to be added to the file system</param>
        /// <param name="fileToAdd">The file path on the computer that we add to the system</param>
        /// <returns>return false if operation failed for any reason</returns>
        public bool add(String fileName, String fileToAdd)
        {
            bool isAdded = false;
            try
            {
                FileInfo fileInfo = new FileInfo(fileToAdd);
                FileSystemFile file = new FileSystemFile(fileName, fileInfo.CreationTime, File.ReadAllBytes(fileToAdd));
                isAdded = this.fileSystem.TryAdd(fileName, file);
                return isAdded;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Caught exception: {ex}");
                return isAdded;
            }
        }

        /// <summary>
        /// this operation releases all allocated memory for this file
        /// Example:
        /// remove("name1.pdf")        
        /// </summary>
        /// <param name="fileName">remove fileName from the system</param>
        /// <returns>return false if operation failed for any reason</returns>
        public bool remove(String fileName)
        {
            bool isRemoved = false;
            try
            {
                if (this.fileSystem.ContainsKey(fileName))
                    isRemoved = this.fileSystem.Remove(fileName, out FileSystemFile value);
                return isRemoved;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Caught exception: {ex}");
                return false;
            }
        }

        /// <summary>
        /// Each string holds details of one file as following: "fileName,size,creation time"
        /// Example:{
        /// "report.pdf,630KB,Friday, ‎May ‎13, ‎2022, ‏‎12:16:32 PM",
        /// "table1.csv,220KB,Monday, ‎February ‎14, ‎2022, ‏‎8:38:24 PM" }
        /// You can use any format for the creation time and date
        /// </summary>
        /// <returns>The function returns a list of strings with the file information in the system</returns>
        public List<String> listFiles()
        {
            List<String> files = new List<string>();
            foreach (FileSystemFile file in fileSystem.Values) 
            {
                files.Add(file.ToString());
            }
            return files;
        }

        /// <summary>
        /// this function saves file from the TinyMemFS file system into a file in the physical disk
        /// Example:
        /// save("name1.pdf", "C:\\tmp\\fileName.pdf")
        /// </summary>
        /// <param name="fileName">file name from TinyMemFS to save in the computer</param>
        /// <param name="fileToAdd">The file path to be saved on the computer</param>
        /// <returns>return false if operation failed for any reason</returns>
        public bool save(String fileName, String fileToAdd)
        {
            try 
            {
                File.WriteAllBytes(fileToAdd, this.fileSystem[fileName].getFileData());
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Caught exception: {ex}");
                return false;
            }
        }

        /// <summary>
        /// You can use an encryption algorithm of your choice
        /// Example:
        /// encrypt("myFSpassword")
        /// </summary>
        /// <param name="key">Encryption key to encrypt the contents of all files in the system</param>
        /// <returns>return false if operation failed for any reason</returns>
        public bool encrypt(String key)
        {
            if (key == null)
                throw new ArgumentException($"Argument 'key' cannot be null");
            if (key.Length == 0)
                throw new ArgumentException($"Key must contain at least 1 char: {key}");
            if (key.Length > 16)
                throw new ArgumentException($"Key length must be less or equal to 16: {key}");

            byte[] resultKey = new byte[16];
            try
            {
                byte[] byteKey = Encoding.UTF8.GetBytes(key);
                for (int i = 0; i < byteKey.Length; i++)
                    resultKey[i] = byteKey[i];

                AesManaged aes = new AesManaged();
                aes.Key = resultKey;
                aes.IV = iv;

                foreach (FileSystemFile fileSystemFile in fileSystem.Values)
                {
                    try
                    {
                        MemoryStream ms = new MemoryStream();
                        CryptoStream cryptoStream =
                            new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);

                        byte[] InputBytes = fileSystemFile.getFileData();
                        cryptoStream.Write(InputBytes, 0, InputBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        fileSystemFile._data = ms.ToArray();
                        this.fileSystem[fileSystemFile._fileName]._cryptoCounter++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Something went wrong while trying to encrypt the File system.");
                        Console.WriteLine();
                        Console.WriteLine($"Caught exception: {ex}");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Caught exception: {ex}");
                return false; 
            }
        }

        /// <summary>
        /// Example:
        /// decrypt("myFSpassword")
        /// </summary>
        /// <param name="key">Decryption key to decrypt the contents of all files in the system</param>
        /// <returns>return false if operation failed for any reason</returns>
        public bool decrypt(String key)
        {
            if (key == null)
                throw new ArgumentException($"Argument 'key' cannot be null");
            if (key.Length == 0)
                throw new ArgumentException($"Key must contain at least 1 char: {key}");
            if (key.Length > 16)
                throw new ArgumentException($"Key length must be less or equal to 16: {key}");
            byte[] resultKey = new byte[16];

            try
            {
                byte[] byteKey = Encoding.UTF8.GetBytes(key);
                for (int i = 0; i < byteKey.Length; i++)
                    resultKey[i] = byteKey[i];

                AesManaged aes = new AesManaged();
                aes.Key = resultKey;
                aes.IV = iv;


                foreach (FileSystemFile fileSystemFile in fileSystem.Values)
                {
                    try
                    {
                        if (this.fileSystem[fileSystemFile._fileName]._cryptoCounter > 0) {
                            MemoryStream ms = new MemoryStream();
                            CryptoStream cryptoStream = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);

                            byte[] InputBytes = fileSystemFile.getFileData();
                            cryptoStream.Write(InputBytes, 0, InputBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            fileSystemFile._data = ms.ToArray();
                            this.fileSystem[fileSystemFile._fileName]._cryptoCounter--;
                        }
                    }
                    catch 
                    {
                        Console.WriteLine("Cannot decrypt this file, wrong password");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Caught exception: {ex}");
                Console.WriteLine();
                return false;
            }
        }

        
        // ************** NOT MANDATORY ********************************************
        // ********** Extended features of TinyMemFS ********************************
        public bool saveToDisk(String fileName)
        {
            /*
             * Save the FS to a single file in disk
             * return false if operation failed for any reason
             * You should store the entire FS (metadata and files) from memory to a single file.
             * You can decide how to save the FS in a single file (format, etc.) 
             * Example:
             * SaveToDisk("C:\\tmp\\MYTINYFS.DAT")
             */
            return false;
        }


        public bool loadFromDisk(String fileName)
        {
            /*
             * Load a saved FS from a file  
             * return false if operation failed for any reason
             * You should clear all the files in the current TinyMemFS if exist, before loading the filenName
             * Example:
             * LoadFromDisk("C:\\tmp\\MYTINYFS.DAT")
             */
            return false;
        }

        public bool compressFile(String fileName)
        {
            /* Compress file fileName
             * return false if operation failed for any reason
             * You can use an compression/uncompression algorithm of your choice
             * Note that the file size might be changed due to this operation, update it accordingly
             * Example:
             * compressFile ("name1.pdf");
             */
            return false;
        }

        public bool uncompressFile(String fileName)
        {
            /* uncompress file fileName
             * return false if operation failed for any reason
             * You can use an compression/uncompression algorithm of your choice
             * Note that the file size might be changed due to this operation, update it accordingly
             * Example:
             * uncompressFile ("name1.pdf");
             */
            return false;
        }

        public bool setHidden(String fileName, bool hidden)
        {
            /* set the hidden property of fileName
             * If file is hidden, it will not appear in the listFiles() results
             * return false if operation failed for any reason
             * Example:
             * setHidden ("name1.pdf", true);
             */
            return false;
        }

        public bool rename(String fileName, String newFileName)
        {
            /* Rename filename to newFileName
             * Return false if operation failed for any reason (E.g., newFileName already exists)
             * Example:
             * rename ("name1.pdf", "name2.pdf");
             */
            return false;
        }

        public bool copy(String fileName1, String fileName2)
        {
            /* Copy the content,size and creation date of one file to another.
             * The file name will not change
             * Return false if operation failed for any reason (E.g., fileName1 doesn't exist or filename2 already exists)
             * Example:
             * copy("name1.pdf", "name2.pdf");
             */
            return true;
        }


        public void sortByName()
        {
            /* Sort the files in the FS by their names (alphabetical order)
             * This should affect the order the files appear in the listFiles 
             * if two names are equal you can sort them arbitrarily
             */
            return;
        }

        public void sortByDate()
        {
            /* Sort the files in the FS by their date (new to old)
             * This should affect the order the files appear in the listFiles  
             * if two dates are equal you can sort them arbitrarily
             */
            return;
        }

        public void sortBySize()
        {
            /* Sort the files in the FS by their sizes (large to small)
             * This should affect the order the files appear in the listFiles  
             * if two sizes are equal you can sort them arbitrarily
             */
            return;
        }


        public bool compare(String fileName1, String fileName2)
        {
            /* compare fileName1 and fileName2
             * files considered equal if their content is equal 
             * Return false if the two files are not equal, or if operation failed for any reason (E.g., fileName1 or fileName2 not exist)
             * Example:
             * compare ("name1.pdf", "name2.pdf");
             */
            return false;
        }

        public Int64 getSize()
        {
            /* return the size of all files in the FS (sum of all sizes)
             */
            return 0;
        }
    }
}
