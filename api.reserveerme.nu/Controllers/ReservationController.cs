using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using api.reserveerme.nu.Tasks;
using api.reserveerme.nu.ViewModels;
using api.reserveerme.nu.WSControllers;
using Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Exchange.WebServices.Data;
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

        public ReservationController(IExchangeLogic exchangeLogic)
        {
            _exchangeLogic = exchangeLogic;
        }

        [HttpGet]
        [Route("status/{roomId}")]
        public async Task<ActionResult<Status>> GetStatus(int roomId)
        {
            try
            {
                var nextAppointment = _exchangeLogic.GetCurrentAppointment(roomId);
                Status status = new Status();
                Reservation reservation = new Reservation();
                reservation.Issuer = nextAppointment.Body;
                reservation.DateStart = nextAppointment.Start;
                reservation.DateEnd = nextAppointment.End;
                
                status.StatusType = StatusType.Free;
                
                if (DateTime.Now > reservation.DateStart)
                {
                    switch (nextAppointment.Status)
                    {
                        case "Occupied":
                            status.Reservation = reservation;
                            status.StatusType = StatusType.Occupied;
                            break;
                        case "Reserved":
                            status.Reservation = reservation;
                            status.StatusType = StatusType.Reserved;
                            break;
                    }
                }
            
                return Ok(status);
            }
            catch (AppointmentNotExistantException e)
            {
                Status status = new Status();
                status.StatusType = StatusType.Free;
                return Ok(status);
            }
        }

        [HttpPost]
        [Route("start")]
        public async Task<ActionResult<bool>> Start([FromBody]StartMeetingViewModel viewModel)
        {
            _exchangeLogic.StartMeeting(viewModel.RoomId);
            return Ok(true);
        }
        
        [HttpPost]
        [Route("remove")]
        public async Task<ActionResult<bool>> Remove([FromBody]RemoveReservationViewModel viewModel)
        {
            var didDeleteAppointment = _exchangeLogic.EndCurrentAppointment(viewModel.RoomId);
            return Ok(didDeleteAppointment);
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
            
            return Created("/reservations", reservationViewModel);
        }

        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<ReservationViewModel>> Add([FromBody]AddReservationViewModel reservationViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            var reservation = new Reservation(reservationViewModel);
            
            var appointmentViewModel = new AppointmentViewModel();
            appointmentViewModel.Body = reservation.Issuer;
            appointmentViewModel.Start = reservation.DateStart;
            appointmentViewModel.End = reservation.DateEnd;
            appointmentViewModel.Subject = reservation.RoomId.ToString();
            try
            {
                _exchangeLogic.CreateNewAppointment(appointmentViewModel);

            }
            catch (AppointmentTimeSlotNotAvailableException e)
            {
                return BadRequest("Timeslot not available");
            }

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
    }
}