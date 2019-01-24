using System;
using System.IO;
using System.Threading.Tasks;
using Pheobe_Blurry;
namespace ConsoleTester
{
    class Program
    {
       public static void Main(string[] args)
        {
            Task t = DoWork();
            t.Wait();
        }
        public static async Task DoWork()
        {
            try
            {
                // Assuming We Are Saving Sales Item Images
                PhotoBucketModel data = new PhotoBucketModel("SalesItemPhotoBucket","Sales", "jpg");
                data.ImageStream = new FileStream(@"c:\users\mypc\desktop\sample.jpg", FileMode.Open, FileAccess.Read);
                PhotoBucketHelper bucket = new PhotoBucketHelper("StorageName", "StorageKey");
                var result = await bucket.AddImage(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
