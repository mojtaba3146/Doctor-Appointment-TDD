using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Services.Doctors.Contracts
{
    public interface DoctorRepository : Repository
    {
        void Add(Doctor doctor);
        bool IsExistNationalCode(string nationalCode);
        List<GetAllDoctorsDto> GetAllDoctors();
        Doctor GetById(int id);
        bool IsExistNationalCodeWithId(string nationalCode, int id);
        void Delete(Doctor doctor);

    }
}
