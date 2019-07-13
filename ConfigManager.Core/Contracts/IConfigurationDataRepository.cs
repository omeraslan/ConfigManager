using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ConfigManager.Core.DTOs;

namespace ConfigManager.Core.Contracts
{
    public interface IConfigurationDataRepository
    {
        bool IsRecordExists(string key, string applicationName);
        bool Add(AddConfigurationRepositoryDTO dto);
        Task<bool> AddAsync(AddConfigurationRepositoryDTO dto);
        bool Update(UpdateConfigurationDTO dto);
        ConfigurationDTO GetConfigurationById(string id);
        List<ConfigurationDTO> Search(string applicationName);
        List<ConfigurationDTO> Search(string searchKey, string applicationName);
        List<ConfigurationDTO> Search(string applicationName,DateTime lastModifyDate);
        List<ConfigurationDTO> GetAllConfigurations();
    }
}
