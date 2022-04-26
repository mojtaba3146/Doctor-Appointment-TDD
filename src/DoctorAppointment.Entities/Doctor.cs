using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Entities
{
    public class Doctor : Person
    {
        public Doctor()
        {
            Appointments = new List<Appointment>();
        }
        public string Field { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}
