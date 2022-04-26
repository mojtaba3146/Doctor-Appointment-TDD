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
    }
}
