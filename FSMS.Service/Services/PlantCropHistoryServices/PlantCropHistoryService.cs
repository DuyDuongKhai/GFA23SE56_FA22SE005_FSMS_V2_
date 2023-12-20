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
        public async Task<List<GetPlantCropHistory>> GetAllAsync(string? gardenName = null, string? plantName = null, string? seasonName = null, int? gardenId = null, int? plantId = null, int? seasonId = null)
        {
            try
            {
                IEnumerable<PlantCropHistory> plantCropHistories = await _plantCropHistoryRepository.GetAsync(includeProperties: "Plant,Plant.Garden");

                // Lọc dữ liệu theo các tham số tìm kiếm
                plantCropHistories = plantCropHistories
                    .Where(pch =>
                        (gardenName == null || pch.Plant.Garden.GardenName == gardenName) &&
                        (plantName == null || pch.Plant.PlantName == plantName) &&
                        (seasonName == null || GetSeasonNameAsync(pch.SeasonId).Result == seasonName) &&
                        (!gardenId.HasValue || pch.Plant.GardenId == gardenId) &&
                        (!plantId.HasValue || pch.PlantId == plantId) &&
                        (!seasonId.HasValue || pch.SeasonId == seasonId));

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

        public async Task<List<PlantHarvestStats>> GetPlantHarvestStatsAsync(string? plantName, int plantId)
        {
            try
            {
                IEnumerable<PlantCropHistory> plantCropHistories = await _plantCropHistoryRepository.GetAsync(includeProperties: "Plant,Plant.Garden");

                // Lọc dữ liệu theo tên cây
                plantCropHistories = plantCropHistories
                     .Where(pch => (plantName == null || pch.Plant.PlantName == plantName) && (plantId == 0 || pch.PlantId == plantId));

                // Tạo một danh sách để lưu trữ kết quả thống kê
                List<PlantHarvestStats> statsList = new List<PlantHarvestStats>();

                // Nhóm cây theo mùa và tính tổng số lượng cây thu hoạch trong mỗi mùa
                var groupedBySeason = plantCropHistories.GroupBy(pch => new { pch.SeasonId, pch.PlantId });
                foreach (var group in groupedBySeason)
                {
                    int seasonId = group.Key.SeasonId;
                    int currentPlantId = group.Key.PlantId;
                    string seasonName = await GetSeasonNameAsync(seasonId);

                    // Lấy tổng quantity từ các PlantCropHistory trong group
                    double totalHarvested = group.Sum(pch => pch.Quantity);

                    // Tổng tổng số lượng thu hoạch qua tất cả các mùa

                    // Tạo đối tượng PlantHarvestStats và thêm vào danh sách kết quả
                    PlantHarvestStats stats = new PlantHarvestStats
                    {
                        PlantId = currentPlantId,
                        PlantName = group.First().Plant.PlantName, // Lấy tên cây từ bất kỳ đối tượng nào trong group
                        SeasonId = seasonId,
                        SeasonName = seasonName,
                        Quantity = group.Sum(pch => pch.Quantity),
                    };

                    statsList.Add(stats);
                }

                return statsList;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching plant harvest statistics", ex);
            }
        }



    }
}
