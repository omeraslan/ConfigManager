using ConfigManager.Core.Constants;
using ConfigManager.Core.Contracts;
using ConfigManager.Core.DTOs;
using ConfigManager.Core.Utilities.Common;
using ConfigManager.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigManager.Core.Business
{
    public class ConfigurationEngine : IConfigurationEngine
    {
        private readonly IConfigurationDataRepository _storageProvider;
        private readonly ICacheRepository _cacheRepository;
        private readonly string _applicationName;
        private readonly int _refreshTimerIntervalInMs;
        public ConfigurationEngine()
        {

        }

        public ConfigurationEngine(IConfigurationDataRepository storageProvider, ICacheRepository cacheRepository,  string applicationName, int refreshTimerIntervalInMs)
        {
            _storageProvider = storageProvider;
            _applicationName = applicationName;
            _refreshTimerIntervalInMs = refreshTimerIntervalInMs;
            _cacheRepository = cacheRepository;
            FillCacheConfigurationList();
        }

        private List<CacheConfigurationDTO> CachedList => _cacheRepository.Get<List<CacheConfigurationDTO>>(string.Format(CacheKey.ApplicationConfiguration));
        public bool Add(AddConfigurationDTO dto)
        {
            var validationResult = new AddNewConfigurationValidator().Validate(dto);
            if (!validationResult.IsValid)
            {
                return false;
            }

            var isExist = _storageProvider.IsRecordExists(dto.Name, _applicationName);
            if (!isExist)
            {
                return _storageProvider.Add(new AddConfigurationDTO
                {
                    Type = dto.Type,
                    IsActive = dto.IsActive,
                    Value = dto.Value,
                    Name = dto.Name,
                    ApplicationName = _applicationName
                });
            }
            return false;
        }

        public async Task<bool> AddAsync(AddConfigurationDTO dto)
        {
            var validationResult = new AddNewConfigurationValidator().Validate(dto);
            if (!validationResult.IsValid)
            {
                return false;
            }

            var isExist = _storageProvider.IsRecordExists(dto.Name, _applicationName);
            if (!isExist)
            {
                return await _storageProvider.AddAsync(new AddConfigurationDTO
                {
                    Type = dto.Type,
                    IsActive = dto.IsActive,
                    Value = dto.Value,
                    Name = dto.Name,
                    ApplicationName = _applicationName
                });
            }
            return false;
        }

        public List<ConfigurationDTO> GetAll()
        {
            return _storageProvider.Search(_applicationName);
        }

        public ConfigurationDTO GetById(string id)
        {
            return !string.IsNullOrWhiteSpace(id) ? _storageProvider.GetConfigurationById(id) : null;
        }

        public T GetValue<T>(string key)
        {
            var cachedItem = CachedList.FirstOrDefault(a => a.Name == key && a.IsActive);

            return cachedItem != null ? cachedItem.Value.Cast<T>(cachedItem.Type) : default(T);
        }

        public void RefreshCache()
        {
            var newCachedList = CachedList != null ? new List<CacheConfigurationDTO>(CachedList) : new List<CacheConfigurationDTO>();

            var lastModifyDate = newCachedList.OrderByDescending(a => a.LastModifyDate).FirstOrDefault()?.LastModifyDate;
            var configurationList = _storageProvider.Search(_applicationName, lastModifyDate ?? DateTime.MinValue);

            if (!configurationList.Any())
                return;

            foreach (var configuration in configurationList)
            {
                var existingCacheItem = newCachedList.FirstOrDefault(a => a.Id == configuration.Id);
                if (existingCacheItem != null)
                {
                    existingCacheItem.Type = configuration.Type;
                    existingCacheItem.Value = configuration.Value;
                    existingCacheItem.LastModifyDate = configuration.LastModifyDate;
                    existingCacheItem.IsActive = configuration.IsActive;
                }
                else
                {
                    newCachedList.Add(new CacheConfigurationDTO
                    {
                        Id = configuration.Id,
                        Type = configuration.Type,
                        LastModifyDate = configuration.LastModifyDate,
                        Value = configuration.Value,
                        Name = configuration.Name,
                        ApplicationName = configuration.ApplicationName,
                        CreationDate = configuration.CreationDate,
                        IsActive = configuration.IsActive
                    });
                }
            }

            _cacheRepository.Remove(CacheKey.ApplicationConfiguration);
            _cacheRepository.Add(CacheKey.ApplicationConfiguration, newCachedList,_refreshTimerIntervalInMs);
        }

        public List<ConfigurationDTO> SearchByName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) ? _storageProvider.Search(name, _applicationName) : new List<ConfigurationDTO>();
        }

        public bool Update(UpdateConfigurationDTO dto)
        {
            var validationResult = new UpdateConfigurationValidator().Validate(dto);
            return validationResult.IsValid && _storageProvider.Update(dto);
        }

        private void FillCacheConfigurationList()
        {
            var list = _storageProvider.Search(_applicationName);
            var cacheList = list.Select(a => new CacheConfigurationDTO
            {
                ApplicationName = a.ApplicationName,
                Value = a.Value,
                Name = a.Name,
                Type = a.Type,
                Id = a.Id,
                CreationDate = a.CreationDate,
                LastModifyDate = a.LastModifyDate,
                IsActive = a.IsActive
            }).ToList();

            _cacheRepository.Add(CacheKey.ApplicationConfiguration, cacheList, _refreshTimerIntervalInMs);
        }
    }
}
