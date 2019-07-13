using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConfigManager.Core.Contracts;
using ConfigManager.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConfigManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigManagerController : ControllerBase
    {
        private readonly IConfigurationEngine _configurationEngine;

        public ConfigManagerController(IConfigurationEngine configurationEngine)
        {
            _configurationEngine = configurationEngine;
        }

        [Route("/Add")]
        [HttpPost]
        public ActionResult<bool> Add([FromBody] AddConfigurationDTO dto)
        {
            return _configurationEngine.Add(dto);
        }
        [Route("/GetAll")]
        [HttpGet]
        public List<ConfigurationDTO> GetAll()
        {
            return _configurationEngine.GetAll();
        }

        [Route("/Update")]
        [HttpPut]
        public ActionResult<bool> Update([FromBody] UpdateConfigurationDTO dto)
        {
            return _configurationEngine.Update(dto);
        }
        [Route("/GetItem")]
        [HttpGet]
        public string GetItem(string key)
        {
            return _configurationEngine.GetValue<string>(key);
        }
    }
}