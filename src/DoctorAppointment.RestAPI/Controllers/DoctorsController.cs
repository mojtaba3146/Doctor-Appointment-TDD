using DoctorAppointment.Services.Doctors.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DoctorAppointment.RestAPI.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly DoctorService _service;

        public DoctorsController(DoctorService service)
        {
            _service = service;
        }

        [HttpPost]
        public void AddDoctor(AddDoctorDto dto)
        {
            _service.Add(dto);
        }
        [HttpGet]
        public List<GetAllDoctorsDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpDelete]
        public void DeleteById(int id)
        {
            _service.Delete(id);
        }

        [HttpPut]
        public void Update(int id, UpdateDoctorDto dto)
        {
            _service.Update(id, dto);
        }
    }
}
