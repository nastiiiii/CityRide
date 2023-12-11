using CityRide.RideService.Application.Exceptions;
using CityRide.RideService.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using RedLockNet;
using StackExchange.Redis;

namespace CityRide.RideService.Application.Services
{
    public class RedisClientService : IRedisClientService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDistributedLockFactory _distributedLockFactory;

        private readonly TimeSpan _expiryTime;
        private readonly TimeSpan _waitTime;
        private readonly TimeSpan _retryTime;

        public RedisClientService(
            IConnectionMultiplexer redis, 
            IDistributedLockFactory distributedLockFactory,
            IConfiguration configuration
            )
        {
            _redis = redis;
            _distributedLockFactory = distributedLockFactory;

            if (!double.TryParse(configuration["Redis:DistributedLock:ExpiryTime"], out var expiryTime)
                || !double.TryParse(configuration["Redis:DistributedLock:WaitTime"], out var waitTime)
                || !double.TryParse(configuration["Redis:DistributedLock:RetryTime"], out var retryTime))
            {
                throw new ArgumentException("Redis lock configuration is not set correctly");
            }
            _expiryTime = TimeSpan.FromSeconds(expiryTime);
            _waitTime = TimeSpan.FromSeconds(waitTime);
            _retryTime = TimeSpan.FromSeconds(retryTime);
        }

        public async Task SetClosestDriversListAsync(string rideId, IEnumerable<string> closestDrivers)
        {
            IDatabase db = _redis.GetDatabase();

            await using (var redisLock = await _distributedLockFactory.CreateLockAsync(rideId, _expiryTime, _waitTime, _retryTime))
            {
                if (redisLock.IsAcquired)
                {
                    if (!db.KeyExists(rideId))
                    {
                        var items = closestDrivers.Select(d => (RedisValue)d).ToArray();
                        db.SetAdd(rideId, items);
                    }
                }
            }
        }

        public async Task<bool> DeleteKeyIfDriverInListAsync(string rideId, string driverId)
        {
            IDatabase db = _redis.GetDatabase();

            await using (var redisLock = await _distributedLockFactory.CreateLockAsync(rideId, _expiryTime, _waitTime, _retryTime))
            {
                if (redisLock.IsAcquired)
                {
                    if (db.KeyExists(rideId))
                    {
                        if (db.SetContains(rideId, driverId))
                        {
                            return db.KeyDelete(rideId);
                        }
                        else
                        {
                            throw new RedisClientServiceException("The ride is not avaible anymore");
                        }
                    }
                    else
                    {
                        throw new RedisClientServiceException("Ride does not exist anymore");
                    }
                }
            }
            return false;
        }

        public async Task<bool> DeleteElementFromListAsync(string rideId, string driverId)
        {
            IDatabase db = _redis.GetDatabase();

            await using(var redisLock = await _distributedLockFactory.CreateLockAsync(rideId, _expiryTime, _waitTime, _retryTime))
            {
                if (redisLock.IsAcquired)
                {
                    if(db.KeyExists(rideId))
                    {
                        if(db.SetContains(rideId, driverId))
                        {
                            db.SetRemove(rideId, driverId);
                            return db.SetLength(rideId) != 0;
                        }
                        else
                        {
                            throw new RedisClientServiceException("The ride is not available anymore");
                        }
                    }
                    else
                    {
                        throw new RedisClientServiceException("Ride does not exist anymore");
                    }
                }
            }
            return false;
        }
    }
}
