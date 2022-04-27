using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Services.Patients.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Services.Appointments.Contracts
{
    public interface AppointmentService : Service
    {
        void Add(AddApointmentDto dto);
        List<GetAllAppointmentDto> GetAll();
        void Update(int id, UpdateAppointmentDto dto);
        void Delete(int appointmentId);
    }
}
