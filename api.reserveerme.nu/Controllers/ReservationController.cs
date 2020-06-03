using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using api.reserveerme.nu.ViewModels;
using api.reserveerme.nu.WSControllers;
using Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Model.Enums;
using Model.Exceptions;
using Model.Interfaces;
using Model.Models;
using Model.ViewModels;
using Websocket.Client;


namespace api.reserveerme.nu.Controllers
{
    [ApiController]
    [Route("reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly IDataAccessProvider _dataAccessProvider;        
        private readonly ILogger<ReservationController> _logger;
        private readonly IExchangeLogic _exchangeLogic;

        public ReservationController(IDataAccessProvider dataAccessProvider, ILogger<ReservationController> logger, IExchangeLogic exchangeLogic)
        {
            _logger = logger;
            _dataAccessProvider = dataAccessProvider;
            _exchangeLogic = exchangeLogic;
        }

        // [HttpGet]
        // [Route("{roomId}/{reservationId}")]
        // public async Task<ActionResult<ReservationViewModel>> Get(int roomId, int reservationId)
        // {
        //     var reservation = await _dataAccessProvider.Read(roomId, reservationId);
        //     return Ok(new ReservationViewModel(reservation));
        // }
        //
        // [HttpGet]
        // [Route("room/{roomId}")]
        // public async Task<ActionResult<List<RoomViewModel>>> GetAll(int roomId)
        // {
        //     var rooms = await _dataAccessProvider.ReadAll(roomId);
        //     var models = new List<RoomViewModel>();
        //     foreach (var r in rooms)
        //     {
        //         models.Add(new RoomViewModel(r));
        //     }
        //     return Ok(models);
        // }

        [HttpGet]
        [Route("status/{roomId}")]
        public async Task<ActionResult<Status>> GetStatus(int roomId)
        {
            var nextAppointment = _exchangeLogic.GetAppointments().First();
            Status status = new Status();
            Reservation reservation = new Reservation();
            reservation.Issuer = nextAppointment.Body;
            reservation.DateStart = nextAppointment.Start;
            reservation.DateEnd = nextAppointment.End;

            status.Reservation = reservation;
            status.StatusType = StatusType.Free;
            
            if (DateTime.Now > reservation.DateStart)
            {
                switch (nextAppointment.Status)
                {
                    case "Occupied":
                        status.StatusType = StatusType.Occupied;
                        break;
                    case "Reserved":
                        status.StatusType = StatusType.Reserved;
                        break;
                }
            }
            
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
        [Route("start")]
        public async Task<ActionResult<bool>> Start([FromBody]StartMeetingViewModel viewModel)
        {
            _exchangeLogic.StartMeeting(viewModel.RoomId);
            return Ok(true);
        }

        [HttpPost]
        public async Task<ActionResult<ReservationViewModel>> Post([FromBody]InstantReservationViewModel reservationViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var reservation = new Reservation(reservationViewModel);
            
            var appointmentViewModel = new AppointmentViewModel();
            appointmentViewModel.Body = reservation.Issuer;
            appointmentViewModel.Start = reservation.DateStart;
            appointmentViewModel.End = reservation.DateEnd;
            appointmentViewModel.Subject = reservation.RoomId.ToString();
            _exchangeLogic.CreateNewAppointment(appointmentViewModel);
            
            await _dataAccessProvider.Add(reservation, reservationViewModel.RoomId);
            return Created("/reservations", reservationViewModel);
        }

        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<ReservationViewModel>> Add([FromBody]AddReservationViewModel reservationViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var reservation = new Reservation(reservationViewModel);
            
            var appointmentViewModel = new AppointmentViewModel();
            appointmentViewModel.Body = reservation.Issuer;
            appointmentViewModel.Start = reservation.DateStart;
            appointmentViewModel.End = reservation.DateEnd;
            appointmentViewModel.Subject = reservation.RoomId.ToString();
            _exchangeLogic.CreateNewAppointment(appointmentViewModel);

            await _dataAccessProvider.Add(reservation, reservationViewModel.RoomId);
            return Created("/reservations", reservationViewModel);
        }
        
        [HttpGet]
        [Route("calendar")]
        public async Task<ActionResult<List<AppointmentViewModel>>> Get()
        {
            try
            {
                var appointments = _exchangeLogic.GetAppointments();
                return Ok(appointments);
            }
            catch (CalenderEmptyException e)
            {
                return NoContent();
            }
        }
        
        [HttpPost]
        [Route("calendar")]
        public async Task<ActionResult<ReservationViewModel>> Add([FromBody]AppointmentViewModel appointmentViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                _exchangeLogic.CreateNewAppointment(appointmentViewModel);
                return Created("/reservations/calendar", appointmentViewModel);
            }
            catch (AppointmentTimeSlotNotAvailableException e)
            {
                Console.WriteLine("TIMESLOT NOT AVAILABLE");
                return Conflict("timeslot not available");
            }
            catch (CalenderEmptyException e)
            {
                return null;
            }
            
        }
    }
}