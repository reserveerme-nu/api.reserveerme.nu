using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.reserveerme.nu.ViewModels;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.Logging;
using Model.Interfaces;
using Model.Models;
using ExchangeService = Logic.ExchangeService;

namespace api.reserveerme.nu.Controllers
{
    [ApiController]
    [Route("reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly IDataAccessProvider _dataAccessProvider;
        private readonly ExchangeService exchangeService;

        public ReservationController(IDataAccessProvider dataAccessProvider, ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _dataAccessProvider = dataAccessProvider;
            exchangeService = new ExchangeService();
            
        }
        
        private readonly ILogger<WeatherForecastController> _logger;

        [HttpGet]
        [Route("{roomId}/{reservationId}")]
        public async Task<ActionResult<ReservationViewModel>> Get(int roomId, int reservationId)
        {
            var reservation = await _dataAccessProvider.Read(roomId, reservationId);
            return Ok(new ReservationViewModel(reservation));
        }

        [HttpGet]
        [Route("room/{roomId}")]
        public async Task<ActionResult<List<RoomViewModel>>> GetAll(int roomId)
        {
            var rooms = await _dataAccessProvider.ReadAll(roomId);
            var models = new List<RoomViewModel>();
            foreach (var r in rooms)
            {
                models.Add(new RoomViewModel(r));
            }
            return Ok(models);
        }

        [HttpGet]
        [Route("status/{roomId}")]
        public async Task<ActionResult<List<RoomViewModel>>> GetStatus(int roomId)
        {
            var status= await _dataAccessProvider.GetStatus(roomId);
            return Ok(status);
        }

        [HttpPost]
        [Route("remove")]
        public async Task<ActionResult<bool>> Remove([FromBody]RemoveReservationViewModel viewModel)
        {
            var status= await _dataAccessProvider.RemoveCurrentReservation(viewModel.RoomId);
            return Ok(status);
        }

        [HttpPost]
        public async Task<ActionResult<ReservationViewModel>> Post([FromBody]ReservationViewModel reservationViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var reservation = new Reservation(reservationViewModel);
            await _dataAccessProvider.Add(reservation, reservationViewModel.RoomId);
            return Created("/reservations", reservationViewModel);
        }

        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<ReservationViewModel>> Add([FromBody]InstantReservationViewModel reservationViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var reservation = new Reservation(reservationViewModel);
            await _dataAccessProvider.Add(reservation, reservationViewModel.RoomId);
            return Created("/reservations", reservationViewModel);
        }
        
        [HttpGet]
        [Route("calendar")]
        public async Task<ActionResult<List<AppointmentViewModel>>> Get()
        {
            var appointments = exchangeService.GetAppointments();
            return Ok(appointments);
        }
        
        [HttpPost]
        [Route("calendar")]
        public async Task<ActionResult<ReservationViewModel>> Add([FromBody]AppointmentViewModel appointmentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            exchangeService.CreateNewAppointment(appointmentViewModel);
            return Created("/reservations/calendar", appointmentViewModel);
        }
    }
}