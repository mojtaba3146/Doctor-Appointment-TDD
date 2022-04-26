using DoctorAppointment.Entities;
using DoctorAppointment.Services.Doctors.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Persistence.EF.Doctors
{
    public class EFDoctorRepository : DoctorRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public EFDoctorRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public void Add(Doctor doctor)
        {
            _dbcontext.Doctors.Add(doctor);
        }

        public void Delete(Doctor doctor)
        {
            _dbcontext.Doctors.Remove(doctor);
        }

        public List<GetAllDoctorsDto> GetAllDoctors()
        {
           return _dbcontext.Doctors.Select(x => new GetAllDoctorsDto
           {
               FirstName = x.FirstName,
               LastName = x.LastName,
               NationalCode = x.NationalCode,
               Field = x.Field,
           }).ToList();
        }

        public Doctor GetById(int id)
        {
            return _dbcontext.Doctors.
                FirstOrDefault(x=>x.Id == id);
        }

        public bool IsExistNationalCode(string nationalCode)
        {
            return _dbcontext.Doctors
                .Any(_ => _.NationalCode == nationalCode);
        }

        public bool IsExistNationalCodeWithId(string nationalCode, int id)
        {
            return _dbcontext.Doctors
                .Any(_ => _.NationalCode == nationalCode && _.Id != id);
        }
    }
}
