using FSMS.Service.Services.SeasonServices;
using FSMS.Service.Utility;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.Validations.Season;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.Seasons;
using FSMS.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/seasons")]
    [ApiController]
    public class SeasonsController : ControllerBase
    {
        private ISeasonService _seasonService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public SeasonsController(ISeasonService seasonService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _seasonService = seasonService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpGet]
        [Cache(1000)]
        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> GetAllSeasons(string? seasonName = null, DateTime? startDate = null, bool activeOnly = false)
        {
            try
            {
                List<GetSeason> seasons = await _seasonService.GetAllSeasonsAsync(seasonName, startDate, activeOnly);
                return Ok(new
                {
                    Data = seasons
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }


        [HttpGet("{id}")]
        [Cache(1000)]

        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> GetSeasonById(int id)
        {
            try
            {
                GetSeason season = await _seasonService.GetAsync(id);
                return Ok(new
                {
                    Data = season
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }


        [HttpPost]

        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> CreateSeason([FromForm] CreateSeason createSeason)
        {
            var validator = new SeasonValidator();
            var validationResult = validator.Validate(createSeason);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }

                await _seasonService.CreateSeasonAsync(createSeason);

                return Ok();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }


        [HttpPut("{id}")]

        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> UpdateSeason(int id, [FromForm] UpdateSeason updateSeason)
        {
            var validator = new UpdateSeasonValidator();
            var validationResult = validator.Validate(updateSeason);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }
                await _seasonService.UpdateSeasonAsync(id, updateSeason);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]

        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> DeleteSeason(int id)
        {
            try
            {
                await _seasonService.DeleteSeasonAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }
    }
}
