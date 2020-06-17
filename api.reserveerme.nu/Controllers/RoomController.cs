using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.reserveerme.nu.ViewModels;
using Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Model.ViewModels;

namespace api.reserveerme.nu.Controllers
{
    [ApiController]
    [Route("rooms")]
    public class RoomController : ControllerBase
    {
        public RoomController(IRoomLogic roomLogic, ILogger<RoomController> logger)
        {
            _roomLogic = roomLogic;
            _logger = logger;
        }

        private readonly IRoomLogic _roomLogic;
        private readonly ILogger<RoomController> _logger;

        [HttpDelete]
        public async Task<ActionResult<bool>> Delete([FromQuery] int roomId)
        {
            _roomLogic.RemoveRoom(roomId);
            return Accepted();
        }

        [HttpGet]
        public async Task<ActionResult<RoomViewModel>> Get([FromQuery] int roomId)
        {
            try
            {
                var room = _roomLogic.GetRoomById(roomId);
                return Ok(room);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return NoContent();
            }
        }

        [HttpGet]
        [Route("getall")]
        public async Task<ActionResult<List<RoomViewModel>>> GetAll()
        {
            return Ok(_roomLogic.GetAllRooms());
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody]RoomViewModel roomViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            int roomId = _roomLogic.AddRoom(roomViewModel);
            return Created("/rooms", roomId);
        }
    }
}