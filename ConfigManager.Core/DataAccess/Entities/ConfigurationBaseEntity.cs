using System;

namespace ConfigManager.Core.DataAccess.Entities
{
    internal class ConfigurationBaseEntity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
        public string ApplicationName { get; set; }
        public DateTime LastModifyDate { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
