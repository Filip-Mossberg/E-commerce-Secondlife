using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using E_commerce.BLL.IService;
using E_commerce.DAL.IRepository;
using E_commerce.Models;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Image;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace E_commerce.BLL.Service
{
    public class ImageService : IImageService
    {
        private readonly BlobContainerClient _blobContainerClient;
        private readonly IValidator<ImageUploadRequest> _validator;
        private readonly IImageRepository _imageRepository;
        public ImageService(BlobContainerClient blobContainerClient, IValidator<ImageUploadRequest> validator,
            IImageRepository imageRepository)
        {
            _blobContainerClient = blobContainerClient; 
            _validator = validator; 
            _imageRepository = imageRepository;
        }

        public async Task<ApiResponse> UploadImage(ImageUploadRequest imageUploadRequest)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };
            var validationResult = await _validator.ValidateAsync(imageUploadRequest);

            if (validationResult.IsValid)
            {
                string Name = Guid.NewGuid().ToString();

                Image image = new Image()
                {
                    ImageName = Name,
                    IsDisplayImage = imageUploadRequest.IsDisplayImage,
                    //ProductId = imageUploadRequest.ProductId
                };
                await _imageRepository.UploadImage(image);

                var blobClient =_blobContainerClient.GetBlobClient(Name);
                await blobClient.UploadAsync(imageUploadRequest.FilePath, new BlobHttpHeaders { ContentType = imageUploadRequest.FilePath.GetContentType() });

                response.IsSuccess = true;
                response.StatusCode = StatusCodes.Status200OK;
                return response;
            }
            else
            {
                foreach (var error in validationResult.Errors)
                {
                    response.Errors.Add(error.ToString());
                }

                return response;
            }
        }
    }
}
