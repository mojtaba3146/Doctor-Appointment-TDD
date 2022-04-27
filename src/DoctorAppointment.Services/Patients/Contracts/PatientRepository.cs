using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Services.Patients.Contracts
{
    public interface PatientRepository : Repository
    {
        void Add(Patient patient);
        bool IsExistNationalCode(string nationalCode);
        List<GetAllPatientDto> GetAll();
        Patient GetById(int id);
        bool IsExistNationalCodeWithId(string nationalCode, int id);
        void Delete(Patient patient);
    }
}
