using System;
using System.Collections.Generic;

namespace TinyMemFS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\omerm\\filesystem\\test";

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
            Test5(tinyMemFS);
            Test8_listfiles(tinyMemFS);
            Test6_add(tinyMemFS, path, "filename");
            Test8_listfiles(tinyMemFS);
            Test7_remove(tinyMemFS, "file #1");
            Test8_listfiles(tinyMemFS);
            Test9_save(tinyMemFS, "file #2", path);
            Test8_listfiles(tinyMemFS);


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
                Console.WriteLine(tinyMemFS.ToString());
                tinyMemFS.decrypt("SomePassword");
                Console.WriteLine(tinyMemFS.ToString());
            }

            static void Test5(TinyMemFS tinyMemFS)
            {
                // Fifth scenario - adding files without order, encrypt and decrypt several times
                Console.WriteLine("-------------------------Encryption----------------------------");

                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine($"{i+1} Iteration:");
                    Console.WriteLine(tinyMemFS.ToString());
                    tinyMemFS.encrypt($"password{i}");
                    tinyMemFS.AddFileToFS(new FileSystemFile($"TestFile #{i+1}", DateTime.Now, new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }));
                    Console.WriteLine(tinyMemFS.ToString());
                }
                Console.WriteLine("-------------------------Decryption----------------------------");
                for (int i = 4; i >= 0; i--)
                {
                    Console.WriteLine($"{5-i+1} Iteration:");
                    Console.WriteLine(tinyMemFS.ToString());
                    tinyMemFS.decrypt($"password{i}");
                    Console.WriteLine(tinyMemFS.ToString());
                }
            }

            static void Test6_add(TinyMemFS tinyMemFS, string path, string fileName)
            {
                // Add function:
                try
                {
                    tinyMemFS.add(fileName, path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Caught exception: {ex}");
                }
            }

            static void Test7_remove(TinyMemFS tinyMemFS, string fileName)
            {
                // Remove function:
                tinyMemFS.remove(fileName);
            }

            static void Test8_listfiles(TinyMemFS tinyMemFS)
            {
                // List files:
                var list = tinyMemFS.listFiles();
                for (int i = 0; i < list.Count; i++)
                {
                    Console.WriteLine(list[i]);
                }
                Console.WriteLine();

            }

            static void Test9_save(TinyMemFS tinyMemFS,string fileName, string folderpath)
            {
                // save:
                try
                {
                    tinyMemFS.save(fileName, folderpath);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"{ex}");
                }
            }
        }
    }
}
