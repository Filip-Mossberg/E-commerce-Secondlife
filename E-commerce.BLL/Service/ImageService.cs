using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using E_commerce.BLL.IService;
using E_commerce.DAL.IRepository;
using E_commerce.Models;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Image;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace E_commerce.BLL.Service
{
    public class ImageService : IImageService
    {
        private readonly BlobContainerClient _blobContainerClient;
        private readonly IValidator<ImageUploadRequest> _imageValidator;
        private readonly IImageRepository _imageRepository;
        public ImageService(BlobContainerClient blobContainerClient, IValidator<ImageUploadRequest> validator,
            IImageRepository imageRepository)
        {
            _blobContainerClient = blobContainerClient;
            _imageValidator = validator; 
            _imageRepository = imageRepository;
        }

        public async Task<ApiResponse> UploadMultipleImages(List<ImageUploadRequest> imageUploadRequest, int productId)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };

            foreach(var image in imageUploadRequest)
            {
                var validationResult = await _imageValidator.ValidateAsync(image);

                if(validationResult.IsValid)
                {
                    var Name = Guid.NewGuid().ToString();

                    var blobClient = _blobContainerClient.GetBlobClient(Name);
                    var result = await blobClient.UploadAsync(image.FilePath, new BlobHttpHeaders { ContentType = image.FilePath.GetContentType() });

                    Image imageUpload = new Image()
                    {
                        Id = Name,
                        Url = blobClient.Uri.ToString(),
                        IsDisplayImage = image.IsDisplayImage,
                        ProductId = productId
                    };

                    await _imageRepository.UploadImage(imageUpload);
                }
                else
                {
                    foreach (var error in validationResult.Errors)
                    {
                        response.Errors.Add(error.ErrorMessage);
                    }
                    return response;
                }
            }

            response.IsSuccess = true;
            response.StatusCode = StatusCodes.Status200OK;
            return response;
        }

        public async Task<ApiResponse> DeleteImages(int productId) 
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };
            var productImages = await _imageRepository.GetAllImagesByProductId(productId);

            if (productImages.Any())
            {
                foreach (var item in productImages)
                {
                    var blobClient = _blobContainerClient.GetBlobClient(item.Id);
                    await blobClient.DeleteAsync();

                    foreach (var image in await _imageRepository.GetAllImagesByProductId(productId))
                    {
                        await _imageRepository.DeleteImage(image);
                    } 
                }

                response.IsSuccess = true;
                response.StatusCode = StatusCodes.Status200OK;
                return response;
            }
            else
            {
                response.Errors.Add("Error removing blobs.");
                return response;
            }
        }
    }
}
