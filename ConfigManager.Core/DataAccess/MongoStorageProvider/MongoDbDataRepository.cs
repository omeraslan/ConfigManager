using ConfigManager.Core.Contracts;
using ConfigManager.Core.DataAccess.Entities.MongoDbEntities;
using ConfigManager.Core.DTOs;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ConfigManager.Core.DataAccess.MongoStorageProvider
{

    public class MongoDbDataRepository : IConfigurationDataRepository
    {
        private const string DbName = "ConfigDb";
        private const string CollectionName = "Configuration";
        private readonly MongoClient _dbClient;

        private IMongoDatabase Db => _dbClient.GetDatabase(DbName);

        private IMongoCollection<MongoDbConfigurationEntity> Collection => Db.GetCollection<MongoDbConfigurationEntity>(CollectionName);

        public MongoDbDataRepository(string connectionString)
        {
            _dbClient = new MongoClient(connectionString);
        }

        public bool Add(AddConfigurationRepositoryDTO dto)
        {
            Collection.InsertOne(new MongoDbConfigurationEntity
            {
                Type = dto.Type,
                Value = dto.Value,
                Name = dto.Name,
                IsActive = dto.IsActive,
                CreationDate = DateTime.Now,
                LastModifyDate = DateTime.Now,
                ApplicationName = dto.ApplicationName
            });
            return true;
        }

        public async Task<bool> AddAsync(AddConfigurationRepositoryDTO dto)
        {
            await Collection.InsertOneAsync(new MongoDbConfigurationEntity
            {
                Type = dto.Type,
                Value = dto.Value,
                Name = dto.Name,
                IsActive = dto.IsActive,
                CreationDate = DateTime.Now,
                LastModifyDate = DateTime.Now,
                ApplicationName = dto.ApplicationName
            });

            return true;
        }

        public List<ConfigurationDTO> GetAllConfigurations()
        {
            return Collection.AsQueryable().Select(a => new ConfigurationDTO
            {
                Id = a.Id.ToString(),
                IsActive = a.IsActive,
                Name = a.Name,
                Type = a.Type,
                ApplicationName = a.ApplicationName,
                Value = a.Value,
                CreationDate = a.CreationDate,
                LastModifyDate = a.LastModifyDate
            }).ToList();
        }

        public ConfigurationDTO GetConfigurationById(string id)
        {
            var entity = Collection.AsQueryable().FirstOrDefault(a => a.Id == ObjectId.Parse(id));
            if (entity != null)
            {
                return new ConfigurationDTO
                {
                    Id = entity.Id.ToString(),
                    IsActive = entity.IsActive,
                    Name = entity.Name,
                    Type = entity.Type,
                    ApplicationName = entity.ApplicationName,
                    Value = entity.Value,
                    CreationDate = entity.CreationDate,
                    LastModifyDate = entity.LastModifyDate
                };
            }

            return null;
        }

        public bool IsRecordExists(string key, string applicationName)
        {
            return Collection.AsQueryable().Any(a => a.Name == key && a.ApplicationName == applicationName);
        }

        public List<ConfigurationDTO> Search(string applicationName)
        {
            return Collection.AsQueryable()
                .Where(a => a.ApplicationName == applicationName && a.IsActive).AsEnumerable()
                .Select(a => new ConfigurationDTO
                {
                    Id = a.Id.ToString(),
                    IsActive = a.IsActive,
                    Name = a.Name,
                    Type = a.Type,
                    ApplicationName = a.ApplicationName,
                    Value = a.Value,
                    CreationDate = a.CreationDate,
                    LastModifyDate = a.LastModifyDate
                }).ToList();
        }

        public List<ConfigurationDTO> Search(string searchKey, string applicationName)
        {
            return Collection.AsQueryable()
                .Where(a => a.ApplicationName == applicationName
                            && a.Name.ToLower().Contains(searchKey.ToLower()) && a.IsActive).AsEnumerable()
                .Select(a => new ConfigurationDTO
                {
                    Id = a.Id.ToString(),
                    IsActive = a.IsActive,
                    Name = a.Name,
                    Type = a.Type,
                    ApplicationName = a.ApplicationName,
                    Value = a.Value,
                    CreationDate = a.CreationDate,
                    LastModifyDate = a.LastModifyDate
                }).ToList();
        }

        public List<ConfigurationDTO> Search(string applicationName, DateTime lastModifyDate)
        {
            var result = Collection.AsQueryable()
                .Where(a => a.ApplicationName == applicationName
                            && a.LastModifyDate > lastModifyDate && a.IsActive).AsEnumerable()
                .Select(a => new ConfigurationDTO
                {
                    Id = a.Id.ToString(),
                    IsActive = a.IsActive,
                    Name = a.Name,
                    Type = a.Type,
                    ApplicationName = a.ApplicationName,
                    Value = a.Value,
                    CreationDate = a.CreationDate,
                    LastModifyDate = a.LastModifyDate
                }).ToList();

            return result;
        }

        public bool Update(UpdateConfigurationDTO dto)
        {
            var filter = Builders<MongoDbConfigurationEntity>.Filter.AnyEq("_id", new ObjectId(dto.Id));
            var update = Builders<MongoDbConfigurationEntity>.Update.Set("Value", dto.Value).Set("IsActive", dto.IsActive)
                .Set("Type", dto.Type).CurrentDate("LastModifyDate");

            var result = Collection.UpdateOne(filter, update);
            return result.IsAcknowledged ? result.ModifiedCount > 0 : result.IsAcknowledged;
        }
    }
}
