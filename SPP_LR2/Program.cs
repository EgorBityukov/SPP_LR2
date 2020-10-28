// Копирование файлов из директории в директорию с помощью ThreadPool, подсчёт количества скопированных файлов

using System;
using System.IO;
using System.Threading;

namespace SPP_LR2
{
    class Program
    {
        static void Main(string[] args)
        {
            string path1, path2;

            //Console.WriteLine("Input necessary path: ");
            //path1 = Console.ReadLine();
            //Console.WriteLine("Input target path: ");
            //path2 = Console.ReadLine();
            path1 = @"D:\Учёба\Test";
            path2 = @"D:\Учёба\Suda";

            CopyFiler cf = new CopyFiler();
            cf.copyFiles(path1, path2);

        }
    }

    class CopyFiler
    {
        int kol = 0;

        private struct ForAsyncCopy
        {
            public FileInfo fileInfo;
            public string path;
            public ForAsyncCopy(FileInfo fileInfo, string path)
            {
                this.fileInfo = fileInfo;
                this.path = path;
            }
        }

        public void copyFiles(string pathFrom, string pathWhere)
        {
            kol = 0;
            copyFilesThreadPool(pathFrom, pathWhere);
            Console.WriteLine("Count: " + kol + " files");
        }

        private void copyFilesThreadPool(string pathFrom, string pathWhere)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(pathFrom);
            ForAsyncCopy forAsyncCopy;

            foreach (FileInfo f in directoryInfo.GetFiles())
            {
                forAsyncCopy = new ForAsyncCopy(f, pathWhere);
                ThreadPool.QueueUserWorkItem(copyFilesAsync, forAsyncCopy);
                kol++;
            }

            foreach (DirectoryInfo d in directoryInfo.GetDirectories())
            {
                Directory.CreateDirectory(pathWhere + @"\" + d.Name);
                copyFiles1(pathFrom + @"\" + d.Name, pathWhere + @"\" + d.Name);
            }

        }

        private void copyFilesAsync(object state)
        {
            string pathWhere;

            try
            {
                ForAsyncCopy forAsync = (ForAsyncCopy)state;
                FileInfo f = forAsync.fileInfo;
                pathWhere = forAsync.path;
                f.CopyTo(pathWhere + @"\" + f.Name);
            }
            catch
            {
                Console.WriteLine("Fail path");
            }
        }
    }
}
