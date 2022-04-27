using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Services.Patients.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Services.Appointments.Contracts
{
    public interface AppointmentRepository : Repository
    {
        void Add(Appointment appointment);
        PossibelityDto GetPossibelity(DateTime dateTime, int doctorId, int patientId);
        List<GetAllAppointmentDto> GetAll();
        Appointment GetById(int id);
        void Delete(Appointment appointment);
    }
}
