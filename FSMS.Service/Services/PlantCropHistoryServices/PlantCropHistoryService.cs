using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.PlantCropHistoryRepositories;
using FSMS.Entity.Repositories.SeasonRepositories;
using FSMS.Service.ViewModels.PlantCropHistory;

namespace FSMS.Service.Services.PlantCropHistoryServices
{
    public class PlantCropHistoryService : IPlantCropHistoryService
    {
        private IPlantCropHistoryRepository _plantCropHistoryRepository;
        private ISeasonRepository _seasonRepository;
        private IMapper _mapper;
        public PlantCropHistoryService(IPlantCropHistoryRepository plantCropHistoryRepository, IMapper mapper, ISeasonRepository seasonRepository)
        {
            _plantCropHistoryRepository = plantCropHistoryRepository;
            _seasonRepository = seasonRepository;
            _mapper = mapper;

        }

        public async Task<string> GetSeasonNameAsync(int seasonId)
        {
            try
            {
                Season season = await _seasonRepository.GetByIDAsync(seasonId);
                return season.SeasonName;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while fetching season with Id {seasonId}", ex);
            }
        }
        public async Task<List<GetPlantCropHistory>> GetAllAsync(string? gardenName = null, string? plantName = null, string? seasonName = null)
        {
            try
            {
                IEnumerable<PlantCropHistory> plantCropHistories = await _plantCropHistoryRepository.GetAsync(includeProperties: "Plant,Plant.Garden");

                // Lọc dữ liệu theo các tham số tìm kiếm
                plantCropHistories = plantCropHistories
                    .Where(pch =>
                        (gardenName == null || pch.Plant.Garden.GardenName == gardenName) &&
                        (plantName == null || pch.Plant.PlantName == plantName) &&
                        (seasonName == null || GetSeasonNameAsync(pch.SeasonId).Result == seasonName));

                List<GetPlantCropHistory> result = plantCropHistories.Select(async plantCropHistory =>
                {
                    var plantCropHistoryDto = _mapper.Map<GetPlantCropHistory>(plantCropHistory);
                    plantCropHistoryDto.SeasonName = await GetSeasonNameAsync(plantCropHistory.SeasonId);
                    return plantCropHistoryDto;
                }).Select(t => t.Result).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching plant crop history", ex);
            }
        }




        public async Task CreatePlantCropHistoryAsync(CreatePlantCropHistory createPlantCropHistory)
        {
            try
            {
                PlantCropHistory plantCropHistory = new PlantCropHistory()
                {
                    PlantId = createPlantCropHistory.PlantId,
                    SeasonId = createPlantCropHistory.SeasonId,
                    GardenId = createPlantCropHistory.GardenId,
                    Quantity = createPlantCropHistory.Quantity,
                    CreatedDate = DateTime.Now,
                };

                await _plantCropHistoryRepository.InsertAsync(plantCropHistory);
                await _plantCropHistoryRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
