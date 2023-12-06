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

        public async Task<ApiResponse> GetAllImagesById(int productId)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };
            var productImages = await _imageRepository.GetAllImagesById(productId);

            if (productImages.Any())
            {
                var images = new List<string>();

                foreach (var image in productImages)
                {
                    await foreach (var item in _blobContainerClient.GetBlobsAsync())
                    {
                        if (item.Name == image.Id)
                        {
                            images.Add(item.Name);
                        }
                    }
                }

                if(images.Any())
                {
                    response.IsSuccess = true;
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Result = images;
                    return response;
                }
            }

            response.Errors.Add("Error retrieving blobs.");
            return response;
        }


        public async Task<ApiResponse> GetAllImagesById2(int productId)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };
            var productImages = await _imageRepository.GetAllImagesById(productId);

            if (productImages.Any())
            {
                foreach (var item in productImages)
                {
                    var blobClient = _blobContainerClient.GetBlobClient(item.Id);
                    var blobDownloadInfo = await blobClient.DownloadAsync();

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        await blobDownloadInfo.Value.Content.CopyToAsync(memoryStream);
                        byte[] imageBytes = memoryStream.ToArray();

                        var contentType = blobDownloadInfo.Value.Details.ContentType; 

                        if (imageBytes != null && imageBytes.Length > 0)
                        {
                            var imageData = new ImageData
                            {
                                ImageBytes = imageBytes,
                                ContentType = contentType
                            };

                            response.Result = imageData; 
                            response.IsSuccess = true;
                            return response;
                        }
                    }
                }
                return response;
            }
            else
            {
                return response;
            }
        }

        public async Task<ApiResponse> DeleteImages(int productId) 
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };
            var productImages = await _imageRepository.GetAllImagesById(productId);

            if (productImages.Any())
            {
                foreach (var item in productImages)
                {
                    var blobClient = _blobContainerClient.GetBlobClient(item.Id);
                    await blobClient.DeleteAsync();
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
