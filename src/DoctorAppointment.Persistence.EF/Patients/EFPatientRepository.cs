using DoctorAppointment.Entities;
using DoctorAppointment.Services.Patients.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Persistence.EF.Patients
{
    public class EFPatientRepository : PatientRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public EFPatientRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public void Add(Patient patient)
        {
            _dbcontext.Patients.Add(patient);
        }

        public void Delete(Patient patient)
        {
            _dbcontext.Patients.Remove(patient);
        }

        public List<GetAllPatientDto> GetAll()
        {
            return _dbcontext.Patients.Select(p => new GetAllPatientDto
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                NationalCode = p.NationalCode,
            }).ToList();
        }

        public Patient GetById(int id)
        {
            return _dbcontext.Patients.
                FirstOrDefault(p => p.Id == id);
        }

        public bool IsExistNationalCode(string nationalCode)
        {
            return _dbcontext.Patients
                .Any(p => p.NationalCode == nationalCode);
        }

        public bool IsExistNationalCodeWithId(string nationalCode, int id)
        {
            return _dbcontext.Patients
               .Any(p => p.NationalCode == nationalCode && p.Id != id);
        }
    }
}
