using System;
using System.Threading.Tasks;
using api.reserveerme.nu.ViewModels;
using Logic;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModels;

namespace api.reserveerme.nu.Controllers
{
    [ApiController]
    [Route("config")]
    public class ConfigController : ControllerBase
    {
        private readonly IExchangeLogic _exchangeLogic;

        public ConfigController(IExchangeLogic exchangeLogic)
        {
            _exchangeLogic = exchangeLogic;
        } 
        
        [HttpPost]
        [Route("setcredentials")]
        public async Task<ActionResult<bool>> Post([FromBody]ChangeCredentialsViewModel changeCredentialsViewModel)
        {
            if (changeCredentialsViewModel.Username == String.Empty ||
                changeCredentialsViewModel.Username == String.Empty)
            {
                return BadRequest();
            }
            _exchangeLogic.SetCredentials(changeCredentialsViewModel.Username, changeCredentialsViewModel.Password);
            return Accepted(true);
        }
    }
}