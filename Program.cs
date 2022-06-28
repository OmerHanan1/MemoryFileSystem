using System;
using System.Collections.Generic;

namespace TinyMemFS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region arrange
            TinyMemFS tinyMemFS = new TinyMemFS();
            byte[] list = new byte[10];
            for (int i = 0; i < 10; i++)
                list[i] = (byte)i;

            for (int i = 0; i < 20; i++)
            {
                var file = new FileSystemFile($"file #{i}", DateTime.Now, list);
                tinyMemFS.AddFileToFS(file);
            }
            #endregion

            Test1(tinyMemFS);
            Test2(tinyMemFS);
            Test3(tinyMemFS);
            Test4(tinyMemFS);


            static void Test1(TinyMemFS tinyMemFS) 
            {
                // First scenario - simetric encryption & decryption
                Console.WriteLine(tinyMemFS.ToString());
                tinyMemFS.encrypt("firstEncryption");
                Console.WriteLine(tinyMemFS.ToString());
                tinyMemFS.encrypt("secondEncryption");
                Console.WriteLine(tinyMemFS.ToString());
                tinyMemFS.decrypt("secondEncryption");
                Console.WriteLine(tinyMemFS.ToString());
                tinyMemFS.decrypt("firstEncryption");
                Console.WriteLine(tinyMemFS.ToString());
            }

            static void Test2(TinyMemFS tinyMemFS)
            {
                // Second scenario - Isimetric encryption & decription
                Console.WriteLine(tinyMemFS.ToString());
                tinyMemFS.encrypt("password");
                Console.WriteLine(tinyMemFS.ToString());
                tinyMemFS.decrypt("wrong");
                Console.WriteLine(tinyMemFS.ToString());
            }

            static void Test3(TinyMemFS tinyMemFS)
            {
                //Third scenario -adding file after encryption, then decryption
                Console.WriteLine(tinyMemFS.ToString());
                tinyMemFS.encrypt("password");
                Console.WriteLine(tinyMemFS.ToString());
                tinyMemFS.AddFileToFS(new FileSystemFile("TestFile", DateTime.Now, new byte[] {5,5,5,5,5,5,5,5,5,5}));
                Console.WriteLine("New file has been added...");
                Console.WriteLine();
                Console.WriteLine(tinyMemFS.ToString());
                tinyMemFS.decrypt("password");
                Console.WriteLine(tinyMemFS.ToString());
            }

            static void Test4(TinyMemFS tinyMemFS)
            {
                // Fourth scenario - trying to decrypt files that never encrypted

            }

        }
    }
}
