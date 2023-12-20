using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.GardenRepositories;
using FSMS.Entity.Repositories.SeasonRepositories;
using FSMS.Service.Enums;
using FSMS.Service.Services.FileServices;
using FSMS.Service.ViewModels.Seasons;

namespace FSMS.Service.Services.SeasonServices
{
    public class SeasonService : ISeasonService
    {
        private ISeasonRepository _seasonRepository;
        private IGardenRepository _gardenRepository;
        private readonly IFileService _fileService;
        private IMapper _mapper;
        public SeasonService(ISeasonRepository seasonRepository, IMapper mapper, IGardenRepository gardenRepository, IFileService fileService)
        {
            _seasonRepository = seasonRepository;
            _mapper = mapper;
            _gardenRepository = gardenRepository;
            _fileService = fileService;
        }

        public async Task CreateSeasonAsync(CreateSeason createSeason)
        {
            try
            {

                Garden existedGarden = (await _gardenRepository.GetByIDAsync(createSeason.GardenId));
                if (existedGarden == null)
                {
                    throw new Exception("Garden Id does not exist in the system.");
                }
                int lastId = (await _seasonRepository.GetAsync()).Max(x => x.SeasonId);
                Season season = new Season()
                {
                    SeasonName = createSeason.SeasonName,
                    Description = createSeason.Description,
                    /*                    Image = createSeason.Image,
                    */
                    StartDate = createSeason.StartDate,
                    EndDate = createSeason.EndDate,
                    GardenId = createSeason.GardenId,
                    Status = StatusEnums.Active.ToString(),
                    CreatedDate = DateTime.Now,
                    SeasonId = lastId + 1
                };
                if (createSeason.UploadFile == null)
                {
                    season.Image = "";
                }
                else if (createSeason.UploadFile != null) season.Image = await _fileService.UploadFile(createSeason.UploadFile);

                await _seasonRepository.InsertAsync(season);
                await _seasonRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task DeleteSeasonAsync(int key)
        {
            try
            {
                Season existedSeason = await _seasonRepository.GetByIDAsync(key);

                if (existedSeason == null)
                {
                    throw new Exception("SeasonId does not exist in the system.");
                }
                if (existedSeason.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("Season is not active.");
                }

                existedSeason.Status = StatusEnums.InActive.ToString();

                await _seasonRepository.UpdateAsync(existedSeason);
                await _seasonRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<GetSeason> GetAsync(int key)
        {
            try
            {
                Season season = await _seasonRepository.GetByIDAsync(key);

                if (season == null)
                {
                    throw new Exception("Season ID does not exist in the system.");
                }
                if (season.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("Season is not active.");
                }
                List<GetSeason> seasons = _mapper.Map<List<GetSeason>>(
                  await _seasonRepository.GetAsync(includeProperties: "Garden")
              );

                GetSeason result = _mapper.Map<GetSeason>(season);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<GetSeason>> GetAllSeasonsAsync(string? seasonName = null, DateTime? startDate = null, bool activeOnly = false, int? gardenId = null)
        {
            try
            {
                var seasons = (await _seasonRepository.GetAsync(includeProperties: "Garden"))
                    .Where(season =>
                        (string.IsNullOrEmpty(seasonName) || season.SeasonName.Contains(seasonName)) &&
                        (!startDate.HasValue || season.StartDate >= startDate) &&
                        (!gardenId.HasValue || season.GardenId == gardenId) &&
                        (!activeOnly || season.Status == StatusEnums.Active.ToString())
                    );

                List<GetSeason> mappedSeasons = _mapper.Map<List<GetSeason>>(seasons);

                return mappedSeasons;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task UpdateSeasonAsync(int key, UpdateSeason updateSeason)
        {
            try
            {
                Season existedSeason = await _seasonRepository.GetByIDAsync(key);

                if (existedSeason == null)
                {
                    throw new Exception("SeasonId does not exist in the system.");
                }

                if (!string.IsNullOrEmpty(updateSeason.SeasonName))
                {
                    existedSeason.SeasonName = updateSeason.SeasonName;
                }

                /* if (!string.IsNullOrEmpty(updateSeason.Image))
                 {
                     existedSeason.Image = updateSeason.Image;
                 }*/

                if (!string.IsNullOrEmpty(updateSeason.Description))
                {
                    existedSeason.Description = updateSeason.Description;
                }

                if (updateSeason.GardenId > 0)
                {
                    existedSeason.GardenId = updateSeason.GardenId;
                }

                existedSeason.StartDate = DateTime.Now;
                existedSeason.EndDate = DateTime.Now;


                if (updateSeason.UploadFile != null)
                {
                    existedSeason.Image = await _fileService.UploadFile(updateSeason.UploadFile);
                }

                if (!string.IsNullOrEmpty(updateSeason.Status))
                {
                    if (updateSeason.Status != "Active" && updateSeason.Status != "InActive")
                    {
                        throw new Exception("Status must be 'Active' or 'InActive'.");
                    }
                    existedSeason.Status = updateSeason.Status;
                }
                existedSeason.UpdateDate = DateTime.Now;


                await _seasonRepository.UpdateAsync(existedSeason);
                await _seasonRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
