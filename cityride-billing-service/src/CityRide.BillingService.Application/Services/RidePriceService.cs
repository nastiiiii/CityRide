using AutoMapper;
using CityRide.Domain.Enums;
using CityRide.BillingService.Application.Services.Interfaces;
using CityRide.BillingService.Domain.Dtos;
using CityRide.BillingService.Domain.Entities;
using CityRide.BillingService.Domain.Repositories;
using CityRide.BillingService.Application.Exceptions;

namespace CityRide.BillingService.Application.Services;

public class RidePriceService: IRidePriceService
{
    private readonly IRidePriceRepository _ridePriceRepository;
    private readonly IMapper _mapper;

    public RidePriceService(IRidePriceRepository ridePriceRepository, IMapper mapper)
    {
        _ridePriceRepository = ridePriceRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RidePriceDto?>> GetAllRidePrices()
    {
        var ridePrices = await _ridePriceRepository.GetAllAsync();

        return ridePrices.Select(r => _mapper.Map<RidePriceDto>(r));
    }

    public async Task<RidePriceDto?> GetRidePriceByCarClass(CarClass carClass)
    {
        var ridePrice = await _ridePriceRepository.GetRidePriceByCarClass(carClass);

        if (ridePrice == null)
        {
            // TODO: custom exceptions and exception handling
            throw new Exception($"Ride price not found, car class: {carClass}");
        }

        return _mapper.Map<RidePriceDto>(ridePrice);
    }
    
    public async Task<RidePriceDto?> GetRidePriceById(int ridePriceId)
    {
        var ridePrice = await _ridePriceRepository.GetByIdAsync(ridePriceId);

        if (ridePrice == null)
        {
            throw new RidePriceNotFoundException(ridePriceId);
        }

        return _mapper.Map<RidePriceDto>(ridePrice);
    }

    public async Task<RidePriceDto> CreateRidePrice(RidePriceDto ridePriceDto)
    {
        var ridePrice = _mapper.Map<RidePrice>(ridePriceDto);

        var createdRidePrice = await _ridePriceRepository.CreateAsync(ridePrice);

        return _mapper.Map<RidePriceDto>(createdRidePrice);
    }

    public async Task UpdateRidePrice(RidePriceDto ridePriceDto)
    {
        var ridePrice = await _ridePriceRepository.GetByIdAsync(ridePriceDto.Id);

        if (ridePrice == null)
        {
            throw new RidePriceNotFoundException(ridePriceDto.Id);
        }

        ridePrice.Name = ridePriceDto.Name;
        ridePrice.CarClass = ridePriceDto.CarClass;
        ridePrice.Coefficient = ridePriceDto.Coefficient;
        ridePrice.CostPerKm = ridePriceDto.CostPerKm;
        ridePrice.ExtraFees = ridePriceDto.ExtraFees;
        
        await _ridePriceRepository.UpdateAsync(ridePrice);
    }

    public async Task DeleteRidePrice(int ridePriceId) => await _ridePriceRepository.DeleteAsync(ridePriceId);
}