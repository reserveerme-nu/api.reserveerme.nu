using api.reserveerme.nu.Controllers;
using api.reserveerme.nu.ViewModels;
using Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Enums;
using Model.Exceptions;
using Model.Models;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class ReservationTests
    {
        [TestMethod]
        public async Task GetStatus_ShouldReturnStatusTypeFree()
        {
            IExchangeLogic exchangeLogic = new ExchangeMock(GetTestAppointments());
            var controller = new ReservationController(exchangeLogic);

            var actionResult = await controller.GetStatus(5);
            var result = actionResult.Result as OkObjectResult;
            var statusObject = result.Value as Status;
            var status = statusObject.StatusType;

            Assert.AreEqual(StatusType.Free, status);
        }

        [TestMethod]
        public async Task GetStatus_ShouldReturnStatusTypeReserved()
        {
            IExchangeLogic exchangeLogic = new ExchangeMock(GetTestAppointments());
            var controller = new ReservationController(exchangeLogic);

            var actionResult = await controller.GetStatus(6);
            var result = actionResult.Result as OkObjectResult;
            var statusObject = result.Value as Status;
            var status = statusObject.StatusType;

            Assert.AreEqual(StatusType.Reserved, status);
        }

        [TestMethod]
        public async Task GetStatus_ShouldReturnStatusTypeOccupied()
        {
            IExchangeLogic exchangeLogic = new ExchangeMock(GetTestAppointments());
            var controller = new ReservationController(exchangeLogic);

            var actionResult = await controller.GetStatus(7);
            var result = actionResult.Result as OkObjectResult;
            var statusObject = result.Value as Status;
            var status = statusObject.StatusType;

            Assert.AreEqual(StatusType.Occupied, status);
        }

        [TestMethod]
        public async Task GetStatus_ShouldReturnRoomNotFoundException()
        {
            try
            {
                IExchangeLogic exchangeLogic = new ExchangeMock(GetTestAppointments());
                var controller = new ReservationController(exchangeLogic);

                var actionResult = await controller.GetStatus(337);
                var result = actionResult.Result as OkObjectResult;
                var statusObject = result.Value as Status;
                var status = statusObject.StatusType;

                Assert.Fail(); // If it gets to this line, no exception was thrown
            }
            catch (RoomNotFoundException exc)
            {
                Assert.AreEqual("Room with ID: 337 was not found", exc.Message);
            }
        }

        [TestMethod]
        public async Task StartMeeting_ShouldReturnTrue_ShouldReturnStatusTypeOccupied()
        {
            const int appointmentId = 5;

            IExchangeLogic exchangeLogic = new ExchangeMock(GetTestAppointments());
            var controller = new ReservationController(exchangeLogic);

            var actionResult = await controller.Start(new StartMeetingViewModel { RoomId = appointmentId });
            var result = actionResult.Result as OkObjectResult;
            var startedMeeting = (bool)result.Value;

            Assert.IsTrue(startedMeeting);

            var statusActionResult = await controller.GetStatus(appointmentId);
            var statusResult = statusActionResult.Result as OkObjectResult;
            var statusObject = statusResult.Value as Status;
            var status = statusObject.StatusType;

            Assert.AreEqual(StatusType.Occupied, status);
        }

        [TestMethod]
        public async Task EndCurrentAppointment_ShouldReturnFalse()
        {
            IExchangeLogic exchangeLogic = new ExchangeMock(GetTestAppointments());
            var controller = new ReservationController(exchangeLogic);

            var actionResult = await controller.Remove(new RemoveReservationViewModel { RoomId = 5 });
            var result = actionResult.Result as OkObjectResult;
            var removedAppointment = (bool)result.Value;

            Assert.IsFalse(removedAppointment);
        }

        [TestMethod]
        public async Task EndCurrentAppointment_ShouldReturnTrue()
        {
            IExchangeLogic exchangeLogic = new ExchangeMock(GetTestAppointments());
            var controller = new ReservationController(exchangeLogic);

            var actionResult = await controller.Remove(new RemoveReservationViewModel { RoomId = 6 });
            var result = actionResult.Result as OkObjectResult;
            var removedAppointment = (bool)result.Value;

            Assert.IsTrue(removedAppointment);
        }

        [TestMethod]
        public async Task AddAppointment_ShouldReturnAppointment_ShouldAddAppointment()
        {
            const int roomId = 1;

            var testAppointment = new AddReservationViewModel
            {
                RoomId = roomId,
                DateStart = DateTime.Now.AddDays(5).ToString(),
                Duration = 5000,
                Issuer = "Issuer"
            };

            IExchangeLogic exchangeLogic = new ExchangeMock(GetTestAppointments());
            var controller = new ReservationController(exchangeLogic);

            var actionResult = await controller.Add(testAppointment);
            var result = actionResult.Result as CreatedResult;

            var appointmentObject = result.Value as AddReservationViewModel;
            
            Assert.AreEqual(appointmentObject.RoomId, roomId);
        }

        [TestMethod]
        public async Task AddAppointment_ShouldThrowNoTimeSlotException()
        {
            const int roomId = 1;

            var testAppointment = new AddReservationViewModel
            {
                RoomId = roomId,
                DateStart = DateTime.Now.ToString(),
                Duration = 5000,
                Issuer = "Issuer"
            };

            IExchangeLogic exchangeLogic = new ExchangeMock(GetTestAppointments());
            var controller = new ReservationController(exchangeLogic);

            var actionResult = await controller.Add(testAppointment);
            var result = actionResult.Result as BadRequestObjectResult;

            Assert.AreEqual("Timeslot not available", result.Value);
        }

        [TestMethod]
        public async Task GetAppointments_ShouldReturnAllAppointments()
        {
            List<AppointmentViewModel> testAppointments = GetTestAppointments();

            IExchangeLogic exchangeLogic = new ExchangeMock(testAppointments);
            var controller = new ReservationController(exchangeLogic);

            var actionResult = await controller.Get();
            var result = actionResult.Result as OkObjectResult;
            var appointmentsList = result.Value as List<AppointmentViewModel>;

            Assert.AreEqual(testAppointments.Count, appointmentsList.Count);
        }

        private List<AppointmentViewModel> GetTestAppointments()
        {
            var testAppointments = new List<AppointmentViewModel>();
            testAppointments.Add(new AppointmentViewModel 
            { 
                Id = "5",
                Body = "body",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1),
                Location = "Eindhoven",
                Status = StatusType.Free.ToString(),
                Subject = "meeting"
            });
            testAppointments.Add(new AppointmentViewModel
            {
                Id = "6",
                Body = "body",
                Start = DateTime.Now.AddDays(-2),
                End = DateTime.Now.AddDays(-1),
                Location = "Eindhoven",
                Status = StatusType.Reserved.ToString(),
                Subject = "meeting"
            });
            testAppointments.Add(new AppointmentViewModel
            {
                Id = "7",
                Body = "body",
                Start = DateTime.Now.AddDays(-4),
                End = DateTime.Now.AddDays(-3),
                Location = "Eindhoven",
                Status = StatusType.Occupied.ToString(),
                Subject = "meeting"
            });
            return testAppointments;
        }
    }
}
