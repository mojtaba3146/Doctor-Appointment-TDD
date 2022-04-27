﻿using DoctorAppointment.Entities;
using DoctorAppointment.Services.Patients.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Test.tools.Patients
{
    public static class PatientFactory
    {
        public static AddPatientDto CreateAddPatientDto()
        {
            return new AddPatientDto
            {
                FirstName = "Akbar",
                LastName = "Yousefi",
                NationalCode = "120"
            };
        }

        public static Patient CreatePatient()
        {
            return new Patient
            {
                FirstName = "Akbar",
                LastName = "Yousefi",
                NationalCode = "120"
            };
        }

        public static UpdatePatientDto CreateUpdateDto()
        {
            return new UpdatePatientDto
            {
                FirstName = "Manoch",
                LastName = "Yousefi",
                NationalCode = "120"
            };
        }

        public static List<Patient> CreateListOfPatients()
        {
            List<Patient> list = new List<Patient>
            {
                new Patient
                {
                Id = 1,
                FirstName = "moji",
                LastName = "khoshi",
                NationalCode = "120"
                },
                new Patient
                {
                Id= 2,
                FirstName = "rahil",
                LastName = "mostafavi",
                NationalCode = "130"
                }
            };
            return list;
        }
    }
}
