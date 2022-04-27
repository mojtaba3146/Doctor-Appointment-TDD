using DoctorAppointment.Entities;
using DoctorAppointment.Services.Appointments.Contracts;
using DoctorAppointment.Services.Patients.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Persistence.EF.Appointments
{
    public class EFAppointmentRepository : AppointmentRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public EFAppointmentRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public void Add(Appointment appointment)
        {
            _dbcontext.Appointments.Add(appointment);
        }

        public void Delete(Appointment appointment)
        {
            _dbcontext.Appointments.Remove(appointment);
        }

        public List<GetAllAppointmentDto> GetAll()
        {
            return _dbcontext.Appointments.Select(p => new GetAllAppointmentDto
            {
                Date = DateTime.Now.Date,
                PatientId = p.PatientId,
                DoctorId = p.DoctorId,
            }).ToList();
        }

        public Appointment GetById(int id)
        {
            return _dbcontext.Appointments.
                 FirstOrDefault(x => x.Id == id);
        }

        public PossibelityDto GetPossibelity(DateTime dateTime, int doctorId, int patientId)
        {
            var VisitsCount = _dbcontext.Appointments.Where(x => x.DoctorId == doctorId &&
             x.Date == dateTime).Count();

            var RepeatCount = _dbcontext.Appointments.Where(x => x.DoctorId == doctorId &&
              x.Date == dateTime && x.PatientId == patientId).Count();

            PossibelityDto possibelityDto = new PossibelityDto
            {
                VisitCount = VisitsCount,
                ReapetCount = RepeatCount,
            };

            return possibelityDto;
        }
    }
}
