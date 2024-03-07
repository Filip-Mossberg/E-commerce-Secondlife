using E_commerce.Models;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.BLL.IService
{
    public interface IImageService
    {
        public Task<ApiResponse> UploadMultipleImages(List<ImageUploadRequest> imageUploadRequests, int productId);
        public Task<ApiResponse> DeleteImages(int productId);
        //public Task<byte[]> imgToByteArray(ImageGetRequest image);
    }
}
