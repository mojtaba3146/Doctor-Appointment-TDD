using DoctorAppointment.Services.Patients.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DoctorAppointment.RestAPI.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly PatientService _service;

        public PatientsController(PatientService service)
        {
            _service = service;
        }

        [HttpPost]
        public void AddPatient(AddPatientDto dto)
        {
            _service.Add(dto);
        }

        [HttpGet]
        public List<GetAllPatientDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpDelete]
        public void DeleteById(int id)
        {
            _service.Delete(id);
        }

        [HttpPut]
        public void Update(int id, UpdatePatientDto dto)
        {
            _service.Update(id, dto);
        }

    }
}
