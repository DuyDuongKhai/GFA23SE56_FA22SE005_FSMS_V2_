using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.CropVarietyRepositories;
using FSMS.Entity.Repositories.GardenRepositories;
using FSMS.Entity.Repositories.PlantRepositories;
using FSMS.Service.Enums;
using FSMS.Service.Services.FileServices;
using FSMS.Service.ViewModels.Plants;
using System.Security.Cryptography;
using System.Text;

namespace FSMS.Service.Services.PlantServices
{
    public class PlantService : IPlantService
    {
        private ICropVarietyRepository _cropVarietyRepository;
        private IPlantRepository _plantRepository;
        private IGardenRepository _gardenRepository;
        private readonly IFileService _fileService;
        private IMapper _mapper;
        public PlantService(IPlantRepository plantRepository, IMapper mapper, IGardenRepository gardenRepository,
            ICropVarietyRepository cropVarietyRepository, IFileService fileService)
        {
            _plantRepository = plantRepository;
            _mapper = mapper;
            _gardenRepository = gardenRepository;
            _cropVarietyRepository = cropVarietyRepository;
            _fileService = fileService;
        }
        public async Task<GetPlant> CreatePlantAsync(CreatePlant createPlant)
        {
            try
            {
                Garden existedGarden = (await _gardenRepository.GetByIDAsync(createPlant.GardenId));
                if (existedGarden == null)
                {
                    throw new Exception("Garden Id does not exist in the system.");
                }
                CropVariety existedCropVariety = (await _cropVarietyRepository.GetByIDAsync(createPlant.CropVarietyId));
                if (existedGarden == null)
                {
                    throw new Exception("Variety Id does not exist in the system.");
                }

                int lastId = (await _plantRepository.GetAsync()).Max(x => x.PlantId);
                Plant plant = new Plant
                {
                    PlantName = createPlant.PlantName,
                    Description = createPlant.Description,
                    /*                    Image = createPlant.Image,
                    */
                    PlantingDate = createPlant.PlantingDate,
                    HarvestingDate = createPlant.HarvestingDate,
                    GardenId = createPlant.GardenId,
                    CropVarietyId = createPlant.CropVarietyId,
                    QuantityPlanted = createPlant.QuantityPlanted,
                    EstimatedHarvestQuantity = 0,
                    Status = createPlant.Status,
                    CreatedDate = DateTime.Now,
                    PlantId = lastId + 1
                };
                if (createPlant.UploadFile == null)
                {
                    plant.Image = "";
                }
                else if (createPlant.UploadFile != null) plant.Image = await _fileService.UploadFile(createPlant.UploadFile);

                string plantInfo = $"{plant.PlantName}{plant.Description}{plant.PlantingDate}{plant.HarvestingDate}{plant.Image}{plant.GardenId}{plant.CropVarietyId}{plant.QuantityPlanted}{plant.EstimatedHarvestQuantity}{plant.Status}{plant.CreatedDate}{plant.PlantId}";

                using (MD5 md5 = MD5.Create())
                {
                    byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(plantInfo));
                    string md5Hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                    var getPlant = new GetPlant
                    {
                        PlantId = plant.PlantId,
                        PlantName = plant.PlantName,
                        Description = plant.Description,
                        PlantingDate = plant.PlantingDate,
                        HarvestingDate = plant.HarvestingDate ?? DateTime.MinValue,
                        Status = plant.Status?.ToString(),
                        Image = plant.Image?.ToString(),
                        GardenName = plant.GardenId.ToString(),
                        CropVarietyName = plant.CropVarietyId.ToString(),
                        CreatedDate = plant.CreatedDate,
                        UpdateDate = plant.UpdateDate ?? DateTime.MinValue,
                        QuantityPlanted = plant.QuantityPlanted ?? 0,
                        EstimatedHarvestQuantity = plant.EstimatedHarvestQuantity ?? 0,
                        md5Hash = md5Hash
                    };

                    await _plantRepository.InsertAsync(plant);
                    await _plantRepository.CommitAsync();

                    existedGarden.QuantityPlanted += createPlant.QuantityPlanted;
                    await _gardenRepository.UpdateAsync(existedGarden);
                    await _gardenRepository.CommitAsync();

                    return getPlant;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<GetPlant> GetAsync(int key)
        {
            try
            {
                Plant plantt = await _plantRepository.GetByIDAsync(key);

                if (plantt == null)
                {
                    throw new Exception("Plant ID does not exist in the system.");
                }
                if (plantt.Status == PlantEnum.Dead.ToString())
                {
                    throw new Exception("Plant is not active.");
                }

                List<GetPlant> plants = _mapper.Map<List<GetPlant>>(
                    (await _plantRepository.GetAsync(includeProperties: "Garden,Variety")));

                foreach (var plant in plants)
                {
                    // Convert plant.CropId to an integer before comparison
                    if (plant.PlantId == key)
                    {
                        string plantInfo = $"{plant.PlantName}{plant.Description}{plant.PlantingDate}{plant.HarvestingDate}{plant.Image}{plant.GardenName}{plant.CropVarietyName}{plant.QuantityPlanted}{plant.EstimatedHarvestQuantity}{plant.Status}{plant.CreatedDate}{plant.PlantId}";
                        using (MD5 md5 = MD5.Create())
                        {
                            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(plantInfo));
                            string md5Hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                            plant.md5Hash = md5Hash;
                            GetPlant result = _mapper.Map<GetPlant>(plant);
                            return result;
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }





        public async Task<List<GetPlant>> GetAllAsync(string? cropName = null, DateTime? plantingDate = null, bool activeOnly = false, int gardenId = 0, int userId = 0, int CropVarietyId = 0)
        {
            try
            {
                List<GetPlant> plants = _mapper.Map<List<GetPlant>>(
                    (await _plantRepository.GetAsync(includeProperties: "Garden,CropVariety"))
                    .Where(plant =>
                        (string.IsNullOrEmpty(cropName) || plant.PlantName.Contains(cropName)) &&
                        (!plantingDate.HasValue || plant.PlantingDate.Date == plantingDate.Value.Date) &&
                        (!activeOnly || plant.Status != PlantEnum.Dead.ToString()) &&
                        (gardenId == 0 || plant.GardenId == gardenId) &&
                        (userId == 0 || plant.Garden.UserId == userId) &&
                        (CropVarietyId == 0 || plant.CropVarietyId == CropVarietyId)

                    )
                );

                foreach (var plant in plants)
                {
                    string plantInfo = $"{plant.PlantName}{plant.Description}{plant.PlantingDate}{plant.HarvestingDate}{plant.Image}{plant.GardenName}{plant.CropVarietyName}{plant.QuantityPlanted}{plant.EstimatedHarvestQuantity}{plant.Status}{plant.CreatedDate}{plant.PlantId}";
                    using (MD5 md5 = MD5.Create())
                    {
                        byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(plantInfo));
                        string md5Hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                        plant.md5Hash = md5Hash;
                    }
                }
                return plants;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




        public async Task DeletePlantAsync(int key)
        {
            try
            {
                Plant existedPlant = await _plantRepository.GetByIDAsync(key);

                if (existedPlant == null)
                {
                    throw new Exception("PlantId does not exist in the system.");
                }
                if (existedPlant.Status == PlantEnum.Dead.ToString())
                {
                    throw new Exception("Plant is not active.");
                }

                existedPlant.Status = PlantEnum.Dead.ToString();

                await _plantRepository.UpdateAsync(existedPlant);
                await _plantRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task UpdatePlantAsync(int key, UpdatePlant updatePlant)
        {
            try
            {
                Plant existedPlant = await _plantRepository.GetByIDAsync(key);

                if (existedPlant == null)
                {
                    throw new Exception("PlantID does not exist in the system.");
                }


                if (!string.IsNullOrEmpty(updatePlant.PlantName))
                {
                    existedPlant.PlantName = updatePlant.PlantName;
                }

                if (!string.IsNullOrEmpty(updatePlant.Description))
                {
                    existedPlant.Description = updatePlant.Description;
                }
                if (updatePlant.UploadFile != null)
                {
                    existedPlant.Image = await _fileService.UploadFile(updatePlant.UploadFile);
                }

                existedPlant.PlantingDate = updatePlant.PlantingDate;
                existedPlant.HarvestingDate = updatePlant.HarvestingDate;
                existedPlant.QuantityPlanted = updatePlant.QuantityPlanted;

                if (!string.IsNullOrEmpty(updatePlant.Status))
                {
                    if (updatePlant.Status == PlantEnum.Harvestable.ToString())
                    {
                        existedPlant.EstimatedHarvestQuantity = updatePlant.EstimatedHarvestQuantity;
                    }
                    else
                    {
                        existedPlant.EstimatedHarvestQuantity = 0;
                    }

                    existedPlant.Status = updatePlant.Status;
                }
                existedPlant.UpdateDate = DateTime.Now;


                await _plantRepository.UpdateAsync(existedPlant);
                await _plantRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
