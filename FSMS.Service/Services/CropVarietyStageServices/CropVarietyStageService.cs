using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.CropVarietyRepositories;
using FSMS.Entity.Repositories.CropVarietyStageRepositories;
using FSMS.Service.Enums;
using FSMS.Service.ViewModels.CropVarietyStages;

namespace FSMS.Service.Services.CropVarietyStageServices
{
    public class CropVarietyStageService : ICropVarietyStageService
    {
        private ICropVarietyStageRepository _cropVarietyStageRepository;
        private ICropVarietyRepository _cropVarietyRepository;
        private IMapper _mapper;
        public CropVarietyStageService(ICropVarietyStageRepository cropVarietyStageRepository, IMapper mapper, ICropVarietyRepository cropVarietyRepository)
        {
            _cropVarietyStageRepository = cropVarietyStageRepository;
            _mapper = mapper;
            _cropVarietyRepository = cropVarietyRepository;
        }

        public async Task CreateCropVarietyStageAsync(CreateCropVarietyStage createCropVarietyStage)
        {
            try
            {

                CropVariety existedCropVariety = (await _cropVarietyRepository.GetByIDAsync(createCropVarietyStage.CropVarietyId));
                if (existedCropVariety == null)
                {
                    throw new Exception("CropVariety Id does not exist in the system.");
                }

                int lastId = (await _cropVarietyStageRepository.GetAsync()).Max(x => x.CropVarietyStageId);
                CropVarietyStage cropVarietyStage = new CropVarietyStage()
                {

                    StageName = createCropVarietyStage.StageName,
                    Description = createCropVarietyStage.Description,
                    StartDate = createCropVarietyStage.StartDate,
                    EndDate = createCropVarietyStage.EndDate,
                    CropVarietyId = createCropVarietyStage.CropVarietyId,
                    Status = CropVarietyGrowthTaskEnum.Pending.ToString(),
                    CreatedDate = DateTime.Now,
                    CropVarietyStageId = lastId + 1
                };

                await _cropVarietyStageRepository.InsertAsync(cropVarietyStage);
                await _cropVarietyStageRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task DeleteCropVarietyStageAsync(int key)
        {
            try
            {
                CropVarietyStage existedCropVarietyStage = await _cropVarietyStageRepository.GetByIDAsync(key);

                if (existedCropVarietyStage == null)
                {
                    throw new Exception("CropVarietyStage ID does not exist in the system.");
                }

                existedCropVarietyStage.Status = StatusEnums.InActive.ToString();

                await _cropVarietyStageRepository.UpdateAsync(existedCropVarietyStage);
                await _cropVarietyStageRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<GetCropVarietyStage> GetAsync(int key)
        {
            try
            {
                CropVarietyStage cropVarietyStage = await _cropVarietyStageRepository.GetByIDAsync(key);

                if (cropVarietyStage == null)
                {
                    throw new Exception("CropVarietyStage ID does not exist in the system.");
                }
                if (cropVarietyStage.Status == StatusEnums.InActive.ToString())
                {
                    throw new Exception("CropVarietyStage is not active.");
                }
                List<GetCropVarietyStage> cropVarietyStages = _mapper.Map<List<GetCropVarietyStage>>(
                  await _cropVarietyStageRepository.GetAsync(includeProperties: "CropVariety")
              );

                GetCropVarietyStage result = _mapper.Map<GetCropVarietyStage>(cropVarietyStage);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<GetCropVarietyStage>> GetAllAsync(string? stageName = null, DateTime? startDate = null, bool activeOnly = false, int? CropVarietyId = null)
        {
            try
            {
                List<GetCropVarietyStage> cropVarietyStages = _mapper.Map<List<GetCropVarietyStage>>(
                    (await _cropVarietyStageRepository.GetAsync(includeProperties: "CropVariety"))
                    .Where(task => (string.IsNullOrEmpty(stageName) || task.StageName.Contains(stageName)) &&
                                    (!startDate.HasValue || task.StartDate.Date == startDate.Value.Date) &&
                                    (!activeOnly || task.Status != StatusEnums.InActive.ToString()) &&
                                    (!CropVarietyId.HasValue || task.CropVarietyId == CropVarietyId.Value)


                    )
                );

                return cropVarietyStages;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task UpdateCropVarietyStageAsync(int key, UpdateCropVarietyStage updateCropVarietyStage)
        {
            try
            {
                CropVarietyStage existedCropVarietyStage = await _cropVarietyStageRepository.GetByIDAsync(key);

                if (existedCropVarietyStage == null)
                {
                    throw new Exception("CropVarietyStage ID does not exist in the system.");
                }

                if (!string.IsNullOrEmpty(updateCropVarietyStage.StageName))
                {
                    existedCropVarietyStage.StageName = updateCropVarietyStage.StageName;
                }

                if (!string.IsNullOrEmpty(updateCropVarietyStage.Description))
                {
                    existedCropVarietyStage.Description = updateCropVarietyStage.Description;
                }
                existedCropVarietyStage.StartDate = updateCropVarietyStage.StartDate;
                existedCropVarietyStage.EndDate = updateCropVarietyStage.EndDate;

                if (!string.IsNullOrEmpty(updateCropVarietyStage.Status))
                {
                    if (updateCropVarietyStage.Status != "Active" && updateCropVarietyStage.Status != "InActive")
                    {
                        throw new Exception("Status must be 'Active' or 'InActive'.");
                    }
                    existedCropVarietyStage.Status = updateCropVarietyStage.Status;
                }

                existedCropVarietyStage.UpdateDate = DateTime.Now;


                await _cropVarietyStageRepository.UpdateAsync(existedCropVarietyStage);
                await _cropVarietyStageRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
