using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.FruitImageRepositories;
using FSMS.Entity.Repositories.FruitRepositories;
using FSMS.Service.Enums;
using FSMS.Service.Services.FileServices;
using FSMS.Service.ViewModels.FruitImages;

namespace FSMS.Service.Services.FruitImageServices
{
    public class FruitImageService : IFruitImageService
    {
        private IFruitImageRepository _fruitImageRepository;
        private IFruitRepository _fruitRepository;
        private IMapper _mapper;
        private readonly IFileService _fileService;

        public FruitImageService(IFruitImageRepository fruitImageRepository, IMapper mapper,
            IFruitRepository fruitRepository, IFileService fileService)
        {
            _fruitImageRepository = fruitImageRepository;
            _mapper = mapper;
            _fruitRepository = fruitRepository;
            _fileService = fileService;
        }

        public async Task/*<List<GetProductImage>>*/ CreateFruitImageAsync(CreateFruitImage requestBody)
        {
            /* ProductImage? albumImg = await _productImageRepository.GetFirstOrDefaultAsync(alu => alu.ImageId == requestBody.ImageId);
             if (albumImg != null)
             {
                 throw new Exception("Please enter the correct information!!!, Id Invalid ");
             }*/
            Fruit existedFruit = (await _fruitRepository.GetByIDAsync(requestBody.FruitId));
            if (existedFruit == null)
            {
                throw new Exception("Fruit ID does not exist in the system.");
            }

            /*List<ProductImage> result = new List<ProductImage>();*/
            if (requestBody.UploadFiles != null)
            {
                List<string> listUrl = await _fileService.UploadFiles(requestBody.UploadFiles);
                foreach (var url in listUrl)
                {
                    int lastId = (await _fruitImageRepository.GetAsync()).Max(x => x.FruitImageId);
                    FruitImage albumImage = _mapper.Map<FruitImage>(requestBody);
                    if (albumImage == null)
                    {
                        throw new Exception("Please enter the correct information!!! ");
                    }
                    albumImage.FruitImageId = lastId + 1;
                    albumImage.ImageUrl = url;
                    albumImage.CreatedDate = DateTime.Now;
                    albumImage.Status = StatusEnums.Active.ToString();
                    await _fruitImageRepository.InsertAsync(albumImage);
                    await _fruitImageRepository.SaveChangesAsync();
                    /*  result.Add(albumImage);*/
                }
            }
            else
            {
                int lastId = (await _fruitImageRepository.GetAsync()).Max(x => x.FruitImageId);
                FruitImage albumImage = _mapper.Map<FruitImage>(requestBody);
                albumImage.FruitImageId = lastId + 1;
                albumImage.ImageUrl = "";
                albumImage.CreatedDate = DateTime.Now;
                albumImage.Status = StatusEnums.Active.ToString();
                await _fruitImageRepository.InsertAsync(albumImage);
                await _fruitImageRepository.SaveChangesAsync();
                /*result.Add(albumImage);*/
            }
            /*List<GetProductImage> listResult = _mapper.ProjectTo<GetProductImage>(result.AsQueryable()).ToList();
            return listResult;*/
        }


        public async Task UpdateFruitImageAsync(int id, UpdateFruitImage requestBody)
        {

            FruitImage albumImage = await _fruitImageRepository.GetFirstOrDefaultAsync(alu => alu.FruitImageId == id);
            if (albumImage == null)
            {
                throw new Exception("Please enter the correct information!!! ");
            }

            _mapper.Map(requestBody, albumImage);

            if (requestBody.UploadFile != null)
            {
                albumImage.ImageUrl = await _fileService.UploadFile(requestBody.UploadFile);
            }

            await _fruitImageRepository.UpdateAsync(albumImage);
            await _fruitImageRepository.SaveChangesAsync();
        }



        public async Task DeleteFruitImageAsync(int id)
        {
            FruitImage? albumImage = await _fruitImageRepository.GetFirstOrDefaultAsync(alu => alu.FruitImageId == id);
            if (albumImage == null)
            {
                throw new Exception("Please enter the correct information!!! ");
            }
            _fruitImageRepository.DeleteAsync(albumImage);
            await _fruitImageRepository.SaveChangesAsync();
        }
        public async Task<GetFruitImage> GetAsync(int key)
        {
            try
            {
                FruitImage fruitImage = await _fruitImageRepository.GetByIDAsync(key);

                if (fruitImage == null)
                {
                    throw new Exception("FruitImage ID does not exist in the system.");
                }
                if (fruitImage.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("ProductImage is not active.");
                }

                List<GetFruitImage> fruitImages = _mapper.Map<List<GetFruitImage>>(
                  await _fruitImageRepository.GetAsync(includeProperties: "Fruit")
              );

                GetFruitImage result = _mapper.Map<GetFruitImage>(fruitImage);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<GetFruitImage>> GetAllAsync()
        {
            try
            {
                List<GetFruitImage> fruitImages = _mapper.Map<List<GetFruitImage>>(
                    await _fruitImageRepository.GetAsync(includeProperties: "Fruit")
                );

                return fruitImages;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
