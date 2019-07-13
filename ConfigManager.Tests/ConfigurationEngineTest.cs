using System.Collections.Generic;
using ConfigManager.Core.Business;
using ConfigManager.Core.Contracts;
using ConfigManager.Core.DTOs;
using NSubstitute;
using NUnit.Framework;
namespace ConfigManager.Tests
{
    [TestFixture]
    public class ConfigurationEngineTest
    {
        private const string ApplicationName = "services";
        private ICacheRepository _mockCacheRepository;
        private IConfigurationDataRepository _dataRepository;

        [SetUp]
        public void Start()
        {
            _mockCacheRepository = Substitute.For<ICacheRepository>();
            _dataRepository = Substitute.For<IConfigurationDataRepository>();
        }

        [Test]
        public void Add_EmptyRequest_NotAdded()
        {
            var configurationReader = new ConfigurationEngine(_dataRepository, _mockCacheRepository, ApplicationName, 1);

            var result = configurationReader.Add(new AddConfigurationDTO());

            Assert.False(result);
        }

        [Test]
        public void Add_EmptyName_NotAdded()
        {
            var configurationReader = new ConfigurationEngine(_dataRepository, _mockCacheRepository, ApplicationName, 1);

            var result = configurationReader.Add(new AddConfigurationDTO
            {
                IsActive = true,
                Name = string.Empty,
                Type = "Type",
                Value = "Value"
            });

            Assert.False(result);
        }


        [Test]
        public void Add_EmptyValue_NotAdded()
        {
            var configurationReader = new ConfigurationEngine(_dataRepository, _mockCacheRepository, ApplicationName, 1);

            var result = configurationReader.Add(new AddConfigurationDTO
            {
                IsActive = true,
                Name = "Name",
                Type = "Type",
                Value = string.Empty
            });

            Assert.False(result);
        }

        [Test]
        public void Add_EmptyType_NotAdded()
        {
            var configurationReader = new ConfigurationEngine(_dataRepository, _mockCacheRepository, ApplicationName, 1);

            var result = configurationReader.Add(new AddConfigurationDTO
            {
                IsActive = true,
                Name = "Name",
                Type = string.Empty,
                Value = "Value"
            });

            Assert.False(result);
        }

        [Test]
        public void Add_ExistingName_NotAdded()
        {
            var configurationReader = new ConfigurationEngine(_dataRepository, _mockCacheRepository, ApplicationName, 1);

            _dataRepository.IsRecordExists(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

            var result = configurationReader.Add(new AddConfigurationDTO
            {
                IsActive = true,
                Name = "Name",
                Type = "Type",
                Value = "Value"
            });

            Assert.False(result);
        }

        [Test]
        public void Add_ValidRequest_Added()
        {
            var configurationReader = new ConfigurationEngine(_dataRepository, _mockCacheRepository, ApplicationName, 1);

            _dataRepository.IsRecordExists(Arg.Any<string>(), Arg.Any<string>()).Returns(false);
            _dataRepository.Add(Arg.Any<AddConfigurationRepositoryDTO>()).Returns(true);

            var result = configurationReader.Add(new AddConfigurationDTO
            {
                IsActive = true,
                Name = "Name",
                Type = "Type",
                Value = "Value"
            });

            Assert.True(result);
        }

        [Test]
        public void Update_EmptyRequest_NotUpdated()
        {
            var configurationReader = new ConfigurationEngine(_dataRepository, _mockCacheRepository, ApplicationName, 1);

            var result = configurationReader.Update(new UpdateConfigurationDTO());

            Assert.False(result);
        }

        [Test]
        public void Update_EmptyType_NotUpdated()
        {
            var configurationReader = new ConfigurationEngine(_dataRepository, _mockCacheRepository, ApplicationName, 1);

            var result = configurationReader.Update(new UpdateConfigurationDTO
            {
                Id = "1",
                IsActive = true,
                Type = string.Empty,
                Value = "Value"
            });

            Assert.False(result);
        }

        [Test]
        public void Update_EmptyValue_NotUpdated()
        {
            var configurationReader = new ConfigurationEngine(_dataRepository, _mockCacheRepository, ApplicationName, 1);

            var result = configurationReader.Update(new UpdateConfigurationDTO
            {
                Id = "1",
                IsActive = true,
                Type = "Type",
                Value = string.Empty
            });

            Assert.False(result);
        }

        [Test]
        public void Update_ValidRequest_Updated()
        {
            var configurationReader = new ConfigurationEngine(_dataRepository, _mockCacheRepository, ApplicationName, 1);

            _dataRepository.Update(Arg.Any<UpdateConfigurationDTO>()).Returns(true);

            var result = configurationReader.Update(new UpdateConfigurationDTO
            {
                Id = "1",
                IsActive = true,
                Type = "Type",
                Value = "Value"
            });

            Assert.True(result);
        }

        [Test]
        public void GetById_EmptyId_ReturnNull()
        {
            var configurationReader = new ConfigurationEngine(_dataRepository, _mockCacheRepository, ApplicationName, 1);

            var result = configurationReader.GetById(string.Empty);

            Assert.Null(result);
        }

        [Test]
        public void GetById_ValidRequest_ReturnConfiguration()
        {
            var configurationReader = new ConfigurationEngine(_dataRepository, _mockCacheRepository, ApplicationName, 1);

            _dataRepository.GetConfigurationById(Arg.Any<string>()).Returns(new ConfigurationDTO());

            var result = configurationReader.GetById("id");

            Assert.NotNull(result);
        }

        [Test]
        public void GetList_ReturnAllConfigurationList()
        {
            var configurationReader = new ConfigurationEngine(_dataRepository, _mockCacheRepository, ApplicationName, 1);

            _dataRepository.Search(Arg.Any<string>()).Returns(new List<ConfigurationDTO>
            {
                new ConfigurationDTO()
            });

            var result = configurationReader.GetAll();

            Assert.IsNotEmpty(result);
        }


        [Test]
        public void SearchByName_EmptyName_ReturnEmptyList()
        {
            var configurationReader = new ConfigurationEngine(_dataRepository, _mockCacheRepository, ApplicationName, 1);

            var result = configurationReader.SearchByName(string.Empty);

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void SearchByName_ExistingName_ReturnNotEmptyList()
        {
            var configurationReader = new ConfigurationEngine(_dataRepository, _mockCacheRepository, ApplicationName, 1);

            _dataRepository.Search(Arg.Any<string>(), Arg.Any<string>()).Returns(new List<ConfigurationDTO>
            {
                new ConfigurationDTO()
            });

            var result = configurationReader.SearchByName("name");

            Assert.Greater(result.Count, 0);
        }

        [Test]
        public void GetValue_ExistingInCache_ReturnValue()
        {
            var keyValue = "value";

            var configurationReader = new ConfigurationEngine(_dataRepository, _mockCacheRepository, ApplicationName, 1);

            _mockCacheRepository.Get<List<CacheConfigurationDTO>>(Arg.Any<string>()).Returns(
                new List<CacheConfigurationDTO>
                {
                    new CacheConfigurationDTO
                    {
                        Name = "name",
                        ApplicationName = ApplicationName,
                        Value = keyValue,
                        Type = "String",
                        IsActive = true
                    }
                });

            var result = configurationReader.GetValue<string>("name");

            Assert.AreEqual(result, keyValue);
        }

        [Test]
        public void GetValue_NotExistingInCache_ReturnDefaultValue()
        {
            var configurationReader = new ConfigurationEngine(_dataRepository, _mockCacheRepository, ApplicationName, 1);

            _mockCacheRepository.Get<List<CacheConfigurationDTO>>(Arg.Any<string>()).Returns(
                new List<CacheConfigurationDTO>
                {
                    new CacheConfigurationDTO
                    {
                        Name = "name",
                        ApplicationName = ApplicationName,
                        Value = "test",
                        Type = "String"
                    }
                });

            var result = configurationReader.GetValue<string>("name2");

            Assert.AreEqual(result, default(string));
        }

    }
}