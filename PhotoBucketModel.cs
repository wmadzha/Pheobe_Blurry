using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Table;
namespace Pheobe_Blurry
{
    public class PhotoBucketModel
    {
        public string ImageID { get; set; }
        public string ImageExtension { get; set; }
        public FileStream ImageStream { get; set; }
        public string ImageContainer { get; set; }
        public string ImageType { get; set; }
        public PhotoBucketModel(string ContainerName,string ImageType,string ImageExtension)
        {
            this.ImageID = Guid.NewGuid().ToString();
            this.ImageContainer = ContainerName.ToLower();
            this.ImageExtension = ImageExtension;
            this.ImageType = ImageType;
        }
    }
    internal class AzurePhotoBucket :TableEntity
    {
        public AzurePhotoBucket()
        {
        }
        public string ImageID { get; set; }
        public string ImageExtension { get; set; }
        public string ImageType { get; set; }
        public string ImageFullName { get; set; }
        public AzurePhotoBucket(PhotoBucketModel Data)
        {
            this.Timestamp = DateTime.UtcNow; 
            this.PartitionKey = "PhotoBucket";
            this.RowKey = Data.ImageID.ToString();
            this.ImageID = Data.ImageID.ToString();
            this.ImageExtension = Data.ImageExtension;
            this.ImageType = Data.ImageType;
            this.ImageFullName = Data.ImageID + "." + Data.ImageExtension;
        }
    }
}
