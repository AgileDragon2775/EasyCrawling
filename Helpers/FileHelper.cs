using EasyCrawling.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace EasyCrawling.Helpers
{
    public class FileHelper
    {
        public static string BaseDirectory
            = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EasyCrawling");
        private static string IndexDirectory
            = Path.Combine(BaseDirectory, "indexs");
        private static string CrawlingDirectory
            = Path.Combine(BaseDirectory, "crawlings");
        public static string TempDirectory
            = Path.Combine(BaseDirectory, "temp");

        public static void Serialize(Crawling crawling, string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                formatter.Serialize(fs, crawling);
            }
            catch (SerializationException e)
            {
                MessageBox.Show("Fail: " + e.Message);                
            }
            finally
            {
                fs.Close();
            }
        }

        public static Crawling Deserialize(string file_name)
        {
            FileStream fs = new FileStream(file_name, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            Crawling crawling = null;

            try
            {
                crawling = formatter.Deserialize(fs) as Crawling;
            }
            catch (SerializationException e)
            {
                MessageBox.Show("Fail: " + e.Message);
            }
            finally
            {
                fs.Close();
            }

            return crawling;
        }

        public static void CreateDirectoryIfNeed(string dir_name)
        {
            Directory.CreateDirectory(BaseDirectory);
            Directory.CreateDirectory(dir_name);
        }

        public static void SaveFile(Crawling crawling)
        {
            if (crawling == null) return;
            CreateDirectoryIfNeed(CrawlingDirectory);
            Serialize(crawling, CrawlingDirectory + "\\" + crawling.Name + ".cr");
        }

        public static List<Crawling> LoadFiles()
        {
            CreateDirectoryIfNeed(CrawlingDirectory);
            List<Crawling> crawlings = new List<Crawling>();
            DirectoryInfo di = new DirectoryInfo(CrawlingDirectory);            

            foreach (FileInfo file in di.GetFiles())
            {
                if (file.Extension.ToLower().CompareTo(".cr") == 0)
                {
                    Crawling now = Deserialize(file.FullName);
                    if (now != null) crawlings.Add(now);
                }
            }
            return crawlings;
        }

        public static void SaveIndexFile(Crawling crawling, string index)
        {
            CreateDirectoryIfNeed(IndexDirectory);

            string filePath = IndexDirectory + "\\" + crawling.Name + ".dat";
            BinaryWriter fbw;
            FileInfo fi = new FileInfo(filePath);

            if (!fi.Exists)
            {
                fbw = new BinaryWriter(File.Open(filePath, FileMode.Create));
            }
            else
            {
                fbw = new BinaryWriter(File.Open(filePath, FileMode.Append));
            }
            fbw.Write(index);
            fbw.Close();
        }

        public static List<string> LoadIndexFile(Crawling crawling)
        {
            CreateDirectoryIfNeed(IndexDirectory);

            string filePath = IndexDirectory + "\\" + crawling.Name + ".dat";
            List<string> indexs = new List<string>();
            FileInfo fi = new FileInfo(filePath);

            if (!fi.Exists) return indexs;

            BinaryReader fbr = new BinaryReader(File.Open(filePath, FileMode.Open));

            int pos = 0;
            int length = (int)fbr.BaseStream.Length;
            while (pos < length)
            {                
                try
                {
                    string index = fbr.ReadString();
                    indexs.Add(index);
                    pos += index.Length + 1;
                }
                catch
                {
                    break;
                }
            }

            fbr.Close();
            return indexs;
        }

        public static void DeleteFile(string name)
        {
            CreateDirectoryIfNeed(CrawlingDirectory);

            string crwlingFilePath = CrawlingDirectory + "\\" + name + ".cr";

            if (File.Exists(crwlingFilePath))
            {
                try
                {
                    File.Delete(crwlingFilePath);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public static void DeleteIndexFile(string name)
        {
            CreateDirectoryIfNeed(IndexDirectory);

            string indexFilePath = IndexDirectory + "\\" + name + ".dat";

            if (File.Exists(indexFilePath))
            {
                try
                {
                    File.Delete(indexFilePath);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public static void ChangeFileName(string oldName, string newName)
        {
            CreateDirectoryIfNeed(CrawlingDirectory);
            CreateDirectoryIfNeed(IndexDirectory);

            string oldCrwlingFilePath = CrawlingDirectory + "\\" + oldName + ".cr";
            string newCrwlingFilePath = CrawlingDirectory + "\\" + newName + ".cr";
            string oldIndexFilePath = IndexDirectory + "\\" + oldName + ".dat";
            string newIndexFilePath = IndexDirectory + "\\" + newName + ".dat";

            if (File.Exists(oldCrwlingFilePath))
            {
                try
                {
                    File.Move(oldCrwlingFilePath, newCrwlingFilePath);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            if (File.Exists(oldIndexFilePath))
            {
                try
                {
                    File.Move(oldIndexFilePath, newIndexFilePath);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public static bool IsExistFile(string name)
        {
            CreateDirectoryIfNeed(CrawlingDirectory);
            string filePath = CrawlingDirectory + "\\" + name + ".cr";

            return File.Exists(filePath);
        }
    }    
}
