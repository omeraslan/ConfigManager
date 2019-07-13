using System;

namespace ConfigManager.Core.DTOs
{
    public class ConfigurationDTO : ConfigurationBaseDTO
    {
        public string Id { get; set; }
        public string ApplicationName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifyDate { get; set; }
    }
}
