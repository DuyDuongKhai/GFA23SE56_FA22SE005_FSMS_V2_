using Firebase.Auth;
using Firebase.Storage;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.FruitImageRepositories;
using FSMS.Entity.Repositories.GardenRepositories;
using FSMS.Entity.Repositories.GardenTaskRepositories;
using FSMS.Entity.Repositories.PlantRepositories;
using FSMS.Entity.Repositories.PostRepositories;
using FSMS.Entity.Repositories.ReviewFruitRepositories;
using FSMS.Entity.Repositories.SeasonRepositories;
using FSMS.Entity.Repositories.UserRepositories;
using Microsoft.AspNetCore.Http;

namespace FSMS.Service.Services.FileServices
{
    public class FileService : IFileService
    {
        private static string ApiKey = "AIzaSyC6E0ovmbqlUYosOzwras-w5SP1bSrSfOU";
        private static string Bucket = "capstonep-30015.appspot.com";
        private static string AuthEmail = "minhvdse150355@fpt.edu.vn";
        private static string AuthPassword = "123456";

        private readonly IGardenRepository _gardenRepository;
        private readonly IPlantRepository _plantRepository;
        private readonly IGardenTaskRepository _gardenTaskRepository;
        private readonly IPostRepository _postRepository;
        private readonly IFruitImageRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IReviewFruitRepository _reviewProductTaskRepository;
        private readonly ISeasonRepository _seasonRepository;

        private static readonly List<string> AllowedExtensions = new List<string> { ".jpeg", ".png", ".bmp", ".webp", ".jpg" };

        public FileService(IGardenTaskRepository gardenTaskRepository, IPostRepository postRepository
            , IFruitImageRepository productRepository, IUserRepository userRepository,
            IGardenRepository gardenRepository)
        {
            _gardenTaskRepository = gardenTaskRepository;
            _postRepository = postRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _gardenRepository = gardenRepository;
        }

        private bool IsFileExtensionValid(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            return AllowedExtensions.Contains(extension);
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            if (!IsFileExtensionValid(file.FileName))
            {
                throw new ArgumentException("Invalid file extension");
            }

            string fileName = DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds + Path.GetExtension(file.FileName);

            var stream = file.OpenReadStream();

            var task = new FirebaseStorage(Bucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
            })
            .Child("images")
            .Child(fileName)
            .PutAsync(stream);

            String urlImage = await task;
            return urlImage;
        }

        public async Task<List<string>> UploadFiles(List<IFormFile> files)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            List<string> result = new List<string>();

            foreach (var fromFile in files)
            {
                if (fromFile.Length > 0)
                {
                    if (!IsFileExtensionValid(fromFile.FileName))
                    {
                        throw new ArgumentException("Invalid file extension");
                    }

                    string fileName = DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds + Path.GetExtension(fromFile.FileName);

                    var stream = fromFile.OpenReadStream();

                    var task = new FirebaseStorage(Bucket, new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    })
                    .Child("images")
                    .Child(fileName)
                    .PutAsync(stream);

                    string urlImage = await task;
                    result.Add(urlImage);
                }
            }

            Console.WriteLine("result: " + result.Count);
            return result;
        }

        public async Task DeleteProductFile(string[] urlImages)
        {
            foreach (var urlImage in urlImages)
            {
                if (urlImage.Length > 0)
                {
                    FruitImage albumImage = await _productRepository.GetFirstOrDefaultAsync(a => a.ImageUrl == urlImage);

                    if (albumImage == null)
                    {
                        throw new Exception("wrong syntax");
                    }

                    _productRepository.DeleteAsync(albumImage);
                    await _productRepository.SaveChangesAsync();
                }
            }
        }
    }
}
