using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Entities
{
    public class Patient : Person
    {
        public Patient()
        {
            Appointments = new List<Appointment>();
        }
        public List<Appointment> Appointments { get; set; }
    }
}
