using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Google.Protobuf.Reflection;
using System.Security.Principal;


//image upload using cloudinary 
public class ImageUploadService
{
    private readonly Cloudinary _cloudinary;

    public ImageUploadService(IConfiguration configuration)
    {
        Account account = new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]
        );

        _cloudinary = new Cloudinary(account);
    }

    public string UploadImage(IFormFile file)
    {
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            Transformation = new Transformation().Width(500).Height(500).Crop("fill") 
        };

        var uploadResult = _cloudinary.Upload(uploadParams);
        return uploadResult.SecureUrl.ToString();
    }
}
