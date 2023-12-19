using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.CropVarietyRepositories;
using FSMS.Entity.Repositories.PlantRepositories;
using FSMS.Service.Enums;
using FSMS.Service.Services.FileServices;
using FSMS.Service.ViewModels.CropVariety;
using FSMS.Service.ViewModels.Plants;

namespace FSMS.Service.Services.CropVarietyServices
{
    public class CropVarietyService : ICropVarietyService
    {
        private IPlantRepository _plantRepository;
        private ICropVarietyRepository _cropVarietyRepository;
        private IFileService _fileService;
        private IMapper _mapper;
        public CropVarietyService(IPlantRepository plantRepository, IMapper mapper,
            ICropVarietyRepository cropVarietyRepository, IFileService fileService)
        {
            _plantRepository = plantRepository;
            _mapper = mapper;
            _cropVarietyRepository = cropVarietyRepository;
            _fileService = fileService;
        }

        public async Task CreateCropVarietyAsync(CreateCropVariety createCropVariety)
        {
            try
            {
                int lastId = (await _cropVarietyRepository.GetAsync()).Max(x => x.CropVarietyId);
                CropVariety cropVariety = new CropVariety()
                {
                    CropVarietyName = createCropVariety.CropVarietyName,
                    Description = createCropVariety.Description,
                    /*Image = createCropVariety.Image,*/
                    Status = StatusEnums.Active.ToString(),
                    CreatedDate = DateTime.Now,
                    CropVarietyId = lastId + 1
                };
                if (createCropVariety.UploadFile == null)
                {
                    cropVariety.Image = "";
                }
                else if (createCropVariety.UploadFile != null) cropVariety.Image = await _fileService.UploadFile(createCropVariety.UploadFile);

                await _cropVarietyRepository.InsertAsync(cropVariety);
                await _cropVarietyRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteCropVarietyAsync(int key)
        {
            try
            {
                CropVariety existedCropVariety = await _cropVarietyRepository.GetByIDAsync(key);

                if (existedCropVariety == null)
                {
                    throw new Exception("CropVarietyId does not exist in the system.");
                }
                if (existedCropVariety.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("CropVariety is not active.");
                }
                existedCropVariety.Status = StatusEnums.InActive.ToString();

                await _cropVarietyRepository.UpdateAsync(existedCropVariety);
                await _cropVarietyRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




        public async Task<GetCropVariety> GetAsync(int key)
        {
            try
            {
                CropVariety cropVariety = await _cropVarietyRepository.GetProductByIDAsync(key);

                if (cropVariety == null)
                {
                    throw new Exception("Crop Variety ID does not exist in the system.");
                }
                if (cropVariety.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("CropVariety is not active.");
                }

                GetCropVariety result = _mapper.Map<GetCropVariety>(cropVariety);

                // Include the list of plants in the result
                result.Plants = _mapper.Map<List<GetPlant>>(cropVariety.Plants);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<List<GetCropVariety>> GetAllWithPlantsAsync(string? varietyName = null, bool activeOnly = false, int? plantId = null)
        {
            try
            {
                IEnumerable<CropVariety> cropVarieties = await _cropVarietyRepository.GetAllProductWithPlantsAsync();

                if (activeOnly)
                {
                    cropVarieties = cropVarieties.Where(cropVariety => cropVariety.Status == StatusEnums.Active.ToString());
                }

                if (!string.IsNullOrEmpty(varietyName))
                {
                    cropVarieties = cropVarieties.Where(cropVariety => cropVariety.CropVarietyName == varietyName);
                }
                if (plantId.HasValue)
                {
                    cropVarieties = cropVarieties.Where(cropVariety => cropVariety.Plants.Any(plant => plant.PlantId == plantId));
                }
                List<GetCropVariety> result = cropVarieties.Select(cropVariety => _mapper.Map<GetCropVariety>(cropVariety)).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching crop varieties.", ex);
            }
        }







        public async Task UpdateCropVarietyAsync(int key, UpdateCropVariety updateCropVariety)
        {
            try
            {
                CropVariety existedCropVariety = await _cropVarietyRepository.GetByIDAsync(key);

                if (existedCropVariety == null)
                {
                    throw new Exception("CropVariety ID does not exist in the system.");
                }

                if (!string.IsNullOrEmpty(updateCropVariety.CropVarietyName))
                {
                    existedCropVariety.CropVarietyName = updateCropVariety.CropVarietyName;
                }

                if (!string.IsNullOrEmpty(updateCropVariety.Description))
                {
                    existedCropVariety.Description = updateCropVariety.Description;
                }

                if (updateCropVariety.UploadFile != null)
                {
                    existedCropVariety.Image = await _fileService.UploadFile(updateCropVariety.UploadFile);
                }

                if (!string.IsNullOrEmpty(updateCropVariety.Status))
                {
                    if (updateCropVariety.Status != "Active" && updateCropVariety.Status != "InActive")
                    {
                        throw new Exception("Status must be 'Active' or 'InActive'.");
                    }
                    existedCropVariety.Status = updateCropVariety.Status;
                }

                existedCropVariety.UpdateDate = DateTime.Now;

                await _cropVarietyRepository.UpdateAsync(existedCropVariety);
                await _cropVarietyRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
