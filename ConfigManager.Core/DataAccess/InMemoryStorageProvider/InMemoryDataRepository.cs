using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConfigManager.Core.Contracts;
using ConfigManager.Core.DataAccess.Contexts;
using ConfigManager.Core.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ConfigManager.Core.DataAccess.InMemoryStorageProvider
{
    public class InMemoryDataRepository : IConfigurationDataRepository
    {
        private const string DbName = "ConfigDb";

        private DbContextOptions<InMemoryDbContext> GetConfigurationContextOptions()
        {
            return new DbContextOptionsBuilder<InMemoryDbContext>()
                .UseInMemoryDatabase(DbName)
                .Options;
        }

        public bool Add(AddConfigurationDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddAsync(AddConfigurationDTO dto)
        {
            throw new NotImplementedException();
        }

        public List<ConfigurationDTO> GetAllConfigurations()
        {
            throw new NotImplementedException();
        }

        public ConfigurationDTO GetConfigurationById(string id)
        {
            throw new NotImplementedException();
        }

        public bool IsRecordExists(string key, string applicationName)
        {
            throw new NotImplementedException();
        }

        public List<ConfigurationDTO> Search(string applicationName)
        {
            throw new NotImplementedException();
        }

        public List<ConfigurationDTO> Search(string searchKey, string applicationName)
        {
            throw new NotImplementedException();
        }

        public List<ConfigurationDTO> Search(string applicationName, DateTime lastModifyDate)
        {
            throw new NotImplementedException();
        }

        public bool Update(UpdateConfigurationDTO dto)
        {
            throw new NotImplementedException();
        }

    }
}
