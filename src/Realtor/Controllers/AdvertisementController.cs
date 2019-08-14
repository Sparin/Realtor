using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Realtor.Model.DTO;
using Realtor.Model.DTO.Advertisement;
using Realtor.Model.Entities;
using Realtor.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Realtor.Controllers
{
    [Route("api/[controller]")]
    public class AdvertisementController : Controller
    {
        private readonly IAdvertisementService _advertisements;
        private readonly IMapper _mapper;
        private readonly ILogger<AdvertisementController> _logger;

        public AdvertisementController(
            IAdvertisementService advertisements,
            IMapper mapper,
            ILogger<AdvertisementController> logger)
        {
            _advertisements = advertisements;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Search advertisements by query
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/advertisement?page=0&amp;limit=50
        ///
        /// </remarks>
        /// <param name="request">Searching options</param>
        /// <param name="page">Offset. Depends on limit</param>
        /// <param name="limit">Count of advertisements per request (max 50)</param>
        /// <response code="200">Successful operation</response>
        [ProducesResponseType(200, Type = typeof(SearchResponse<AdvertisementDto>))]
        [HttpGet]
        public async Task<IActionResult> SearchAdvertisementsAsync(SearchRequest request, int page = Helpers.DEFAULT_PAGE, int limit = Helpers.MAX_LIMIT_ON_PAGE)
        {
            Helpers.CorrectPageLimitValues(ref page, ref limit);
            _logger.LogInformation($"Searching advertisements on page {page} with limit {limit}");

            var entities = await _advertisements.GetAdvertisementsAsync(request, page, limit);
            var totalItems = await _advertisements.GetAdvertisementsCountAsync(request);

            //TODO: Sanitize entities for avoid OWASP Top 10 A7:2017-Cross-Site Scripting (XSS)            
            var items = _mapper.Map<IEnumerable<AdvertisementDto>>(entities);
            var searchResponse = new SearchResponse<AdvertisementDto>(totalItems, page, limit, items);
            _logger.LogInformation($"User received {entities.Count()} advertisements");
            return Ok(searchResponse);
        }

        /// <summary>
        /// Retrieve advertisement by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/advertisement/5
        ///
        /// </remarks>
        /// <param name="id">Identificator of advertisement</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Advertisement is not found</response>        
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(AdvertisementDto))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdvertisementByIdAsync(int id)
        {
            _logger.LogInformation($"User trying to get advertisement with id {id}");
            var entity = await _advertisements.FindAdvertisementAsync(id);

            if (entity == default(Advertisement))
            {
                _logger.LogWarning($"User requested not existing advertisement by id {id}");
                return NotFound();
            }

            //TODO: Sanitize entities for avoid OWASP Top 10 A7:2017-Cross-Site Scripting (XSS)
            _logger.LogInformation($"User received advertisement with id {id}");
            var result = _mapper.Map<AdvertisementDto>(entity);
            return Ok(result);
        }

        /// <summary>
        /// Create new advertisement
        /// </summary>
        /// <param name="requestDto">Description of future advertisement</param>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Entity validation is not passed</response>
        [ProducesResponseType(400, Type = typeof(Dictionary<string, string[]>))]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(AdvertisementDto))]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAdvertisementAsync([FromBody]CreateAdvertisementDto requestDto)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation($"User trying to create new advertisement");
            var entity = _mapper.Map<Advertisement>(requestDto);
            entity.AuthorId = int.Parse(userId);

            //TODO: Sanitize entities for avoid OWASP Top 10 A7:2017-Cross-Site Scripting (XSS)
            _logger.LogInformation($"Validating new advertisement");

            if (ModelState.IsValid)
                TryValidateModel(entity);
            if (!ModelState.IsValid)
            {
                var errors = ModelState.FormatModelErrors();
                _logger.LogWarning($"New advertisement did not pass entity validation", errors);
                return BadRequest(errors);
            }


            entity = await _advertisements.CreateAdvertisementAsync(entity);
            _logger.LogInformation($"User created new advertisement with identificator {entity.Id}");

            var result = _mapper.Map<AdvertisementDto>(entity);
            return Ok(result);
        }

        /// <summary>
        /// Update existing advertisement by id
        /// </summary>
        /// <param name="id">Identificator of advertisement</param>
        /// <param name="requestDto">Description of updating advertisement</param>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Entity validation is not passed</response>
        [ProducesResponseType(400, Type = typeof(Dictionary<string, string[]>))]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(AdvertisementDto))]
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdvertisementAsync(int id, [FromBody]UpdateAdvertisementDto requestDto)
        {
            _logger.LogInformation($"User trying to update existing advertisement with id {id}");
            var entity = await _advertisements.FindAdvertisementAsync(id);
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (entity == default(Advertisement))
            {
                _logger.LogWarning($"User requested not existing advertisement");
                return NotFound();
            }

            if (entity.AuthorId.ToString() != userId)
            {
                _logger.LogWarning($"User is tried to update not his own advertisement");
                return Forbid();
            }

            entity = _mapper.Map(requestDto, entity);
            //TODO: Sanitize entities for avoid OWASP Top 10 A7:2017-Cross-Site Scripting (XSS)

            _logger.LogInformation($"Validating updated advertisement");


            if (ModelState.IsValid)
                TryValidateModel(entity);
            if (!ModelState.IsValid)
            {
                var errors = ModelState.FormatModelErrors();
                _logger.LogWarning($"Updated advertisement did not pass entity validation", errors);
                return BadRequest(errors);
            }

            entity = await _advertisements.UpdateAdvertisementAsync(entity);
            _logger.LogInformation($"Advertisement with identificator {entity.Id} updated");

            var result = _mapper.Map<AdvertisementDto>(entity);
            return Ok(result);
        }

        /// <summary>
        /// Delete advertisement by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/advertisement/5
        ///
        /// </remarks>
        /// <param name="id">Identificator of advertisement</param>
        /// <response code="204">Successful operation</response>
        /// <response code="404">Advertisement is not found</response>        
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(204)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"User trying to delete advertisement with identificator {id}");
            var entity = await _advertisements.FindAdvertisementAsync(id);
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (entity == default(Advertisement))
            {
                _logger.LogWarning($"User requested not existing advertisement");
                return NotFound();
            }

            if (entity.AuthorId.ToString() != userId)
            {
                _logger.LogWarning($"User is tried to delete not his own advertisement");
                return Forbid();
            }

            await _advertisements.DeleteAdvertisementAsync(entity);
            _logger.LogInformation($"Advertisement with identificator {entity.Id} was deleted");

            return NoContent();
        }
    }
}
