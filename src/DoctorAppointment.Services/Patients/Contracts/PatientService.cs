using DoctorAppointment.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Services.Patients.Contracts
{
    public interface PatientService : Service
    {
        void Add(AddPatientDto dto);
        List<GetAllPatientDto> GetAll();
        void Update(int id, UpdatePatientDto dto);
        void Delete(int patientId);
    }
}
