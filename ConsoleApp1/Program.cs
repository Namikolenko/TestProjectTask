using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using YandexDisk.Client;
using YandexDisk.Client.Clients;
using YandexDisk.Client.Http;

namespace ConsoleApp1
{
    class Program
    {
        static async Task UploadFileAsynk(string file, IDiskApi diskApi, string destDirectory)
        {
            var name = file.Split(@"\")[file.Split(@"\").Length - 1];
            var diskPath = destDirectory + name;
            Console.WriteLine("Uploading file " + name);

            await diskApi.Files.UploadFileAsync(path: diskPath,
                overwrite: true,
                localFile: file,
                cancellationToken: CancellationToken.None);

            Console.WriteLine(name + " uploaded");
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine("Start project");
            Console.WriteLine(args.Length);

            var token = "AgAAAAAbypL7AADLWxjV5uI3qkwihLVZSj2-iLE"; // OAuth-token
            var testPath = args[0];
            var destDirectory = args[1];
            IDiskApi diskApi = new DiskHttpApi(token);

            string[] allFiles = Directory.GetFiles(testPath);
            Task[] tasks = new Task[allFiles.Length];
            int i = 0;
            foreach (var file in allFiles)
            {
                try
                {
                    tasks[i] = UploadFileAsynk(file, diskApi, destDirectory);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception with file " + file);
                    Console.WriteLine(e);
                    tasks[i] = null;
                }
                finally
                {
                    i++;
                }
            }

            foreach (var task in tasks)
            {
                if (null != task)
                    task.Wait();
            }
            
            Console.WriteLine("Files Uploaded");

        }
    }
}