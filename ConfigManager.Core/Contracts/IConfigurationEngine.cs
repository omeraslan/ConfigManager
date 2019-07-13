﻿using ConfigManager.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigManager.Core.Contracts
{
    public interface IConfigurationEngine
    {
        T GetValue<T>(string key);
        bool Add(AddConfigurationDTO dto);
        Task<bool> AddAsync(AddConfigurationDTO dto);
        bool Update(UpdateConfigurationDTO dto);
        List<ConfigurationDTO> GetAll();
        List<ConfigurationDTO> SearchByName(string name);
        ConfigurationDTO GetById(string id);
        void RefreshCache();
    }
}
