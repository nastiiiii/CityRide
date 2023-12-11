using CityRide.BillingService.API.Requests;
using Microsoft.AspNetCore.Mvc;
using CityRide.BillingService.API.Responses;
using CityRide.BillingService.Application.Services.Interfaces;
using CityRide.BillingService.Domain.Dtos;
using AutoMapper;
using CityRide.BillingService.API.Filters;

namespace CityRide.BillingService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExceptionFilter]
    public class RidePricesController : ControllerBase
    {
        private readonly IRidePriceService _ridePriceService;
        private readonly ICostService _costService;
        private readonly IMapper _mapper;

        public RidePricesController(IRidePriceService ridePriceService, ICostService costService, IMapper mapper)
        {
            _ridePriceService = ridePriceService;
            _costService = costService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<CalculateRidePriceResponse>> CalculateRidePrice(
            CalculateRidePriceRequest calculateRidePriceRequest)
        {
            var ridePriceDto = await _ridePriceService.GetRidePriceByCarClass(calculateRidePriceRequest.CarClass);
            var source = calculateRidePriceRequest.Source;
            var destination = calculateRidePriceRequest.Destination;
            
            var totalCost = _costService.CalculateRideCost(ridePriceDto, source, destination);

            var calculateRidePriceResponse = new CalculateRidePriceResponse
            {
                // If ridePriceDto is null _costService would throw an exception by now
                CarClass = ridePriceDto!.Name,
                TotalCost = totalCost
            };

            return Ok(calculateRidePriceResponse);
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RidePriceDto?>>> GetAllRidePrices()
        {
            var ridePrices = await _ridePriceService.GetAllRidePrices();

            return Ok(ridePrices);
        }

        // CRUD
        [HttpGet]
        public async Task<ActionResult<RidePriceResponse?>> GetRidePrice(int ridePriceId)
        {
            var ridePriceDto = await _ridePriceService.GetRidePriceById(ridePriceId);
            
            return Ok(_mapper.Map<RidePriceResponse>(ridePriceDto));
        }

        [HttpPost]
        public async Task<ActionResult<RidePriceDto?>> CreateRidePrice(CreateRidePriceRequest request)
        {
            var createdRidePriceDto = await _ridePriceService.CreateRidePrice(_mapper.Map<RidePriceDto>(request));

            return Ok(createdRidePriceDto);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateRidePrice(UpdateRidePriceRequest request)
        {
            await _ridePriceService.UpdateRidePrice(_mapper.Map<RidePriceDto>(request));

            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteRidePriceDto(int ridePriceId)
        {
            await _ridePriceService.DeleteRidePrice(ridePriceId);

            return NoContent();
        }
    }
}
