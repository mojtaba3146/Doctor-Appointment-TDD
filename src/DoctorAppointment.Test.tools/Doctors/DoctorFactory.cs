using DoctorAppointment.Entities;
using DoctorAppointment.Services.Doctors.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Test.tools.Doctors
{
    public static class DoctorFactory
    {
        public static Doctor CreateDoctor()
        {
            return new Doctor
            {
                Id = 1,
                FirstName = "mojtaba",
                LastName = "khoshnam",
                NationalCode = "230",
                Field = "brain"
            };
        }

        public static AddDoctorDto CreateAddDoctorDto()
        {
            return new AddDoctorDto
            {
                FirstName = "mojtaba",
                LastName = "khoshnam",
                NationalCode = "230",
                Field = "brain"
            };
        }

        public static UpdateDoctorDto CreateUpdateDto()
        {
            return new UpdateDoctorDto
            {
                FirstName = "moji",
                LastName = "khoshi",
                Field = "brain",
                NationalCode = "230"
            };
        }

        public static List<Doctor> CreateListOfDoctors()
        {
            List<Doctor> list = new List<Doctor>
            {
                new Doctor
                {
                Id = 1,
                FirstName = "moji",
                LastName = "khoshi",
                Field = "brain",
                NationalCode = "230"
                },
                new Doctor
                {
                Id= 2,
                FirstName = "rahil",
                LastName = "mostafavi",
                Field = "heart",
                NationalCode = "130"
                }
            };
            return list;
        }

    }
}
