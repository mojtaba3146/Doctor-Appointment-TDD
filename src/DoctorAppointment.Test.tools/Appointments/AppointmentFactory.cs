using DoctorAppointment.Infrastructure.Test;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Test.tools.Doctors;
using DoctorAppointment.Test.tools.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Test.tools.Appointments
{
    public class AppointmentFactory
    {
        private readonly ApplicationDbContext _dataContext;

        public AppointmentFactory()
        {
            _dataContext = new EFInMemoryDatabase()
                .CreateDataContext<ApplicationDbContext>();

        }
        public void CreateDoctorAndPatient()
        {
            var doctor = DoctorFactory.CreateDoctor();
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));
            var patient = PatientFactory.CreatePatient();
            _dataContext.Manipulate(_ => _.Patients.Add(patient));
        }

    }
}
