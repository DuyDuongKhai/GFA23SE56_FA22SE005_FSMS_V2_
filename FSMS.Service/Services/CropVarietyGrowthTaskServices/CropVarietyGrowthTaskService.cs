using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.CropVarietyGrowthTaskRepositories;
using FSMS.Entity.Repositories.CropVarietyStageRepositories;
using FSMS.Service.Enums;
using FSMS.Service.ViewModels.CropVarietyGrowthTasks;

namespace FSMS.Service.Services.CropVarietyGrowthTaskServices
{
    public class CropVarietyGrowthTaskService : ICropVarietyGrowthTaskService
    {
        private ICropVarietyStageRepository _cropVarietyStageRepository;
        private ICropVarietyGrowthTaskRepository _cropVarietyGrowthTaskRepository;
        private IMapper _mapper;
        public CropVarietyGrowthTaskService(ICropVarietyStageRepository cropVarietyStageRepository, IMapper mapper, ICropVarietyGrowthTaskRepository cropVarietyGrowthTaskRepository)
        {
            _cropVarietyStageRepository = cropVarietyStageRepository;
            _mapper = mapper;
            _cropVarietyGrowthTaskRepository = cropVarietyGrowthTaskRepository;
        }

        public async Task CreateCropVarietyGrowthTaskAsync(CreateCropVarietyGrowthTask createCropVarietyGrowthTask)
        {
            try
            {

                CropVarietyStage existedCropVarietyStage = (await _cropVarietyStageRepository.GetByIDAsync(createCropVarietyGrowthTask.CropVarietyStageId));
                if (existedCropVarietyStage == null)
                {
                    throw new Exception("CropVarietyStage Id does not exist in the system.");
                }

                int lastId = (await _cropVarietyGrowthTaskRepository.GetAsync()).Max(x => x.GrowthTaskId);
                CropVarietyGrowthTask cropVarietyGrowthTask = new CropVarietyGrowthTask()
                {

                    TaskName = createCropVarietyGrowthTask.TaskName,
                    Description = createCropVarietyGrowthTask.Description,
                    StartDate = createCropVarietyGrowthTask.StartDate,
                    EndDate = createCropVarietyGrowthTask.EndDate,
                    CropVarietyStageId = createCropVarietyGrowthTask.CropVarietyStageId,
                    Status = CropVarietyGrowthTaskEnum.Pending.ToString(),
                    CreatedDate = DateTime.Now,
                    GrowthTaskId = lastId + 1
                };

                await _cropVarietyGrowthTaskRepository.InsertAsync(cropVarietyGrowthTask);
                await _cropVarietyGrowthTaskRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task DeleteCropVarietyGrowthTaskAsync(int key)
        {
            try
            {
                CropVarietyGrowthTask existedCropVarietyGrowthTask = await _cropVarietyGrowthTaskRepository.GetByIDAsync(key);

                if (existedCropVarietyGrowthTask == null)
                {
                    throw new Exception("CropVarietyGrowthTask ID does not exist in the system.");
                }

                existedCropVarietyGrowthTask.Status = CropVarietyGrowthTaskEnum.Cancelled.ToString();

                await _cropVarietyGrowthTaskRepository.UpdateAsync(existedCropVarietyGrowthTask);
                await _cropVarietyGrowthTaskRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<GetCropVarietyGrowthTask> GetAsync(int key)
        {
            try
            {
                CropVarietyGrowthTask cropVarietyGrowthTask = await _cropVarietyGrowthTaskRepository.GetByIDAsync(key);

                if (cropVarietyGrowthTask == null)
                {
                    throw new Exception("CropVarietyGrowthTask ID does not exist in the system.");
                }
                if (cropVarietyGrowthTask.Status == CropVarietyGrowthTaskEnum.Cancelled.ToString())
                {
                    throw new Exception("CropVarietyGrowthTask is not active.");
                }
                List<GetCropVarietyGrowthTask> cropVarietyGrowthTasks = _mapper.Map<List<GetCropVarietyGrowthTask>>(
                  await _cropVarietyGrowthTaskRepository.GetAsync(includeProperties: "CropVarietyStage")
              );

                GetCropVarietyGrowthTask result = _mapper.Map<GetCropVarietyGrowthTask>(cropVarietyGrowthTask);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<GetCropVarietyGrowthTask>> GetAllAsync(string? taskName = null, DateTime? startDate = null, bool activeOnly = false, int? cropVarietyStageId = null)
        {
            try
            {
                List<GetCropVarietyGrowthTask> cropVarietyGrowthTasks = _mapper.Map<List<GetCropVarietyGrowthTask>>(
                    (await _cropVarietyGrowthTaskRepository.GetAsync(includeProperties: "CropVarietyStage"))
                    .Where(task => (string.IsNullOrEmpty(taskName) || task.TaskName.Contains(taskName)) &&
                                    (!startDate.HasValue || task.StartDate.Date == startDate.Value.Date) &&
                                    (!activeOnly || task.Status != CropVarietyGrowthTaskEnum.Cancelled.ToString()) &&
                                     (!cropVarietyStageId.HasValue || task.CropVarietyStageId == cropVarietyStageId.Value)

                    )
                );

                return cropVarietyGrowthTasks;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task UpdateCropVarietyGrowthTaskAsync(int key, UpdateCropVarietyGrowthTask updateCropVarietyGrowthTask)
        {
            try
            {
                CropVarietyGrowthTask existedCropVarietyGrowthTask = await _cropVarietyGrowthTaskRepository.GetByIDAsync(key);

                if (existedCropVarietyGrowthTask == null)
                {
                    throw new Exception("CropVarietyGrowthTask ID does not exist in the system.");
                }

                if (!string.IsNullOrEmpty(updateCropVarietyGrowthTask.TaskName))
                {
                    existedCropVarietyGrowthTask.TaskName = updateCropVarietyGrowthTask.TaskName;
                }

                if (!string.IsNullOrEmpty(updateCropVarietyGrowthTask.Description))
                {
                    existedCropVarietyGrowthTask.Description = updateCropVarietyGrowthTask.Description;
                }
                existedCropVarietyGrowthTask.StartDate = updateCropVarietyGrowthTask.StartDate;
                existedCropVarietyGrowthTask.EndDate = updateCropVarietyGrowthTask.EndDate;

                if (!string.IsNullOrEmpty(updateCropVarietyGrowthTask.Status))
                {
                    if (updateCropVarietyGrowthTask.Status != "Pending" && updateCropVarietyGrowthTask.Status != "InProgress" && updateCropVarietyGrowthTask.Status != "Completed" && updateCropVarietyGrowthTask.Status != "Cancelled")
                    {
                        throw new Exception("Status must be 'Pending' or 'InProgress'. or 'Completed' or 'Cancelled'");
                    }
                    existedCropVarietyGrowthTask.Status = updateCropVarietyGrowthTask.Status;
                }
                existedCropVarietyGrowthTask.UpdateDate = DateTime.Now;


                await _cropVarietyGrowthTaskRepository.UpdateAsync(existedCropVarietyGrowthTask);
                await _cropVarietyGrowthTaskRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
