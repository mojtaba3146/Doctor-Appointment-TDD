using DoctorAppointment.Services.Appointments.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DoctorAppointment.RestAPI.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentsController : Controller
    {
        private readonly AppointmentService _service;

        public AppointmentsController(AppointmentService service)
        {
            _service = service;
        }

        [HttpPost]
        public void AddAppointment(AddApointmentDto dto)
        {
            _service.Add(dto);
        }

        [HttpGet]
        public List<GetAllAppointmentDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpDelete]
        public void DeleteById(int id)
        {
            _service.Delete(id);
        }

        [HttpPut]
        public void Update(int id, UpdateAppointmentDto dto)
        {
            _service.Update(id, dto);
        }

    }
}
