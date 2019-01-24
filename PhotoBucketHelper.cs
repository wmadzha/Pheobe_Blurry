using System;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Pheobe_Blurry
{
    public class PhotoBucketHelper
    {
        private string _StorageAccountName { get; set; }
        private string _StorageAccountKey { get; set; }
        public PhotoBucketHelper(string StorageAccountName , string StorageAccountKey )
        {
            this._StorageAccountKey = StorageAccountKey;
            this._StorageAccountName = StorageAccountName;
        }
        public async Task<bool> AddImage(PhotoBucketModel Data)
        {
            if (Data.ImageStream == null)
                return false;
            try
            {
                bool ResultImage = await AddBlobImage(Data.ImageContainer, Data.ImageID, Data.ImageExtension, Data.ImageStream);
                if (ResultImage)
                {
                    return await AddNewImageData(new AzurePhotoBucket(Data), Data.ImageContainer);
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        internal async Task<bool> AddBlobImage(string ContainerName, string FileName, string FileExtension, FileStream FileStream)
        {
            
            try
            {
                CloudBlobClient Client = SetupAccount().CreateCloudBlobClient();
                CloudBlobContainer Container = Client.GetContainerReference(ContainerName);
                await Container.CreateIfNotExistsAsync();
                CloudBlockBlob BlockBlob = Container.GetBlockBlobReference(FileName + "." + FileExtension);
                await BlockBlob.UploadFromStreamAsync(FileStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return await Task.FromResult(true);
        }
        internal  CloudStorageAccount SetupAccount()
        {
            StorageCredentials cred = new StorageCredentials(this._StorageAccountName, this._StorageAccountKey);
            CloudStorageAccount acc = new CloudStorageAccount(cred, this._StorageAccountName, "core.windows.net", true);
            return acc;
        }
        internal async Task <bool> AddNewImageData(ITableEntity DataModel , string ContainerName)
        {
            CloudStorageAccount sa = SetupAccount();
            CloudTableClient tc = sa.CreateCloudTableClient();
            CloudTable ct = tc.GetTableReference(ContainerName);
            await ct.CreateIfNotExistsAsync();
            TableOperation b = TableOperation.InsertOrReplace(DataModel);
            try
            {
                var result = await ct.ExecuteAsync(b);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
