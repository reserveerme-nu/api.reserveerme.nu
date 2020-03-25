using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.reserveerme.nu.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Interfaces;
using Model.Models;

namespace api.reserveerme.nu.Controllers
{
    [ApiController]
    [Route("rooms")]
    public class RoomController : ControllerBase
    {
        private readonly IDataAccessProvider _dataAccessProvider;

        public RoomController(IDataAccessProvider dataAccessProvider, ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _dataAccessProvider = dataAccessProvider;
        }
        
        private readonly ILogger<WeatherForecastController> _logger;

        [HttpGet]
        [Route("{roomId}/{reservationId}")]
        public async Task<ActionResult<ReservationViewModel>> Get(int reservationId)
        {
            var reservation = await _dataAccessProvider.Read(reservationId);
            return Ok(new ReservationViewModel(reservation));
        }

        [HttpPost]
        public async Task<ActionResult<RoomViewModel>> Post([FromBody]RoomViewModel roomViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var room = new Room(roomViewModel);
            await _dataAccessProvider.Create(room);
            return Created("/rooms", roomViewModel);
        }
    }
}