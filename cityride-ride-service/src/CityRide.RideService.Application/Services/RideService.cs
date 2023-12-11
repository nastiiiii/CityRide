using AutoMapper;
using CityRide.Domain.Enums;
using CityRide.RideService.Application.Exceptions;
using CityRide.RideService.Application.Services.Interfaces;
using CityRide.RideService.Domain.Dtos;
using CityRide.RideService.Domain.Entities;
using CityRide.RideService.Domain.Repositories;

namespace CityRide.RideService.Application.Services;

public class RideService : IRideService
{
    private readonly IRideRepository _rideRepository;
    private readonly IMapper _mapper;

    public RideService(IRideRepository rideRepository, IMapper mapper)
    {
        _rideRepository = rideRepository;
        _mapper = mapper;
    }

    public async Task<RideDto> CreateRideAsync(RideDto rideDto)
    {
        var ride = _mapper.Map<Ride>(rideDto);

        var createdRide = await _rideRepository.CreateAsync(ride);

        return _mapper.Map<RideDto>(createdRide);
    }

    public async Task UpdateRideAsync(RideDto rideDto)
    {
        var ride = await _rideRepository.GetByIdAsync(rideDto.Id);
        if (ride == null) throw new RideNotFoundException(rideDto.Id);

        ride = _mapper.Map<Ride>(rideDto);

        await _rideRepository.UpdateAsync(ride);
    }

    public async Task DeleteRideAsync(int rideId)
    {
        await _rideRepository.DeleteAsync(rideId);
    }

    public async Task<RideDto> GetRide(int rideId)
    {
        var ride = await _rideRepository.GetByIdAsync(rideId);
        if (ride == null) throw new RideNotFoundException(rideId);
        return _mapper.Map<RideDto>(ride);
    }

    public async Task<IEnumerable<RideDto>> GetRidesByClientIdAsync(int clientId)
    {
        var ridesByClient = await _rideRepository.GetRidesByClientIdAsync(clientId);
        return ridesByClient.Select(ride => _mapper.Map<RideDto>(ride));
    }

    public async Task<IEnumerable<RideDto>> GetRidesByDriverIdAsync(int driverId)
    {
        var ridesByDriver = await _rideRepository.GetRidesByDriverIdAsync(driverId);
        return ridesByDriver.Select(ride => _mapper.Map<RideDto>(ride));
    }

    public async Task AcceptRideAndSetDriver(int rideId, int driverId)
    {
        var ride = await _rideRepository.GetByIdAsync(rideId);

        if (ride == null)
        {
            throw new RideNotFoundException(rideId);
        }

        ride.DriverId = driverId;
        ride.Status = RideStatus.Accepted;

        await _rideRepository.UpdateAsync(ride);
    }

    public async Task DeclineRide(int rideId)
    {
        var ride = await _rideRepository.GetByIdAsync(rideId);

        if (ride == null)
        {
            throw new RideNotFoundException(rideId);
        }

        ride.Status = RideStatus.Declined;

        await _rideRepository.UpdateAsync(ride);
    }

    public async Task StartRide(int rideId)
    {
        var ride = await _rideRepository.GetByIdAsync(rideId);

        if (ride == null)
        {
            throw new RideNotFoundException(rideId);
        }

        ride.Status = RideStatus.Started;

        await _rideRepository.UpdateAsync(ride);
    }

    public async Task EndRide(int rideId)
    {
        var ride = await _rideRepository.GetByIdAsync(rideId);

        if (ride == null)
        {
            throw new RideNotFoundException(rideId);
        }

        ride.Status = RideStatus.Ended;

        await _rideRepository.UpdateAsync(ride);
    }
}