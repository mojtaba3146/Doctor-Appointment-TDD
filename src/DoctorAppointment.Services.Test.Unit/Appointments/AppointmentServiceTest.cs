using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Infrastructure.Test;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Persistence.EF.Appointments;
using DoctorAppointment.Services.Appointments;
using DoctorAppointment.Services.Appointments.Contracts;
using DoctorAppointment.Services.Appointments.Exceptions;
using DoctorAppointment.Test.tools.Appointments;
using DoctorAppointment.Test.tools.Doctors;
using DoctorAppointment.Test.tools.Patients;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DoctorAppointment.Services.Test.Unit.Appointments
{
    public class AppointmentServiceTest
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly AppointmentService _sut;
        private readonly AppointmentRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public AppointmentServiceTest()
        {
            _dataContext = new EFInMemoryDatabase()
                .CreateDataContext<ApplicationDbContext>();
            _repository = new EFAppointmentRepository(_dataContext);
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _sut = new AppointmentAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_appointment_properly()
        {
            var doctor = DoctorFactory.CreateDoctor();
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));
            var patient = PatientFactory.CreatePatient();
            _dataContext.Manipulate(_ => _.Patients.Add(patient));
            AddApointmentDto dto = CreatAppointmentDto(doctor, patient);

            _sut.Add(dto);

            _dataContext.Appointments.Should()
                .Contain(_ => _.Date == dto.Date &&
                _.DoctorId == dto.DoctorId &&
                _.PatientId == dto.PatientId);
        }

        [Fact]
        public void GetAll_returns_all_appointments()
        {
            Appointment appointmnet = Create_doctor_patient_appointment();

            var expected = _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Date == appointmnet.Date &&
            _.PatientId == appointmnet.PatientId &&
            _.DoctorId == appointmnet.DoctorId);
        }

        [Fact]
        public void Update_updates_appointment_properly()
        {
            List<Appointment> appointmentList = Create_list_of_appointments();
            UpdateAppointmentDto dto = CreateUpdateAppointmentDto();
            var appointmentId = appointmentList[4].Id;

            _sut.Update(appointmentId, dto);

            var expected = _dataContext.Appointments.
                FirstOrDefault(_ => _.Id == appointmentId);
            expected.Date.Should().Be(dto.Date);
            expected.PatientId.Should().Be(dto.PatientId);
            expected.DoctorId.Should().Be(dto.DoctorId);
        }

        [Fact]
        public void Update_throw_AppointmentDoesNotExsitException_when_appointment_with_given_id_is_not_exist()
        {
            Create_doctor_patient();
            UpdateAppointmentDto dto = CreateUpdateAppointmentDto();
            var appointmentId = 100;

            Action expected = () => _sut.Update(appointmentId, dto);

            expected.Should().ThrowExactly<AppointmentDoesNotExsitException>();
        }

        [Fact]
        public void Delete_deletes_appointment_properly()
        {
            Appointment appointmnet=Create_doctor_patient_appointment();
            UpdateAppointmentDto dto = CreateUpdateAppointmentDto();
            var appointmentId = appointmnet.Id;

            _sut.Delete(appointmentId);

            _dataContext.Appointments.Should().
                NotContain(_ => _.Id == appointmentId);
        }

        [Fact]
        public void Delete_throw_AppointmentDoesNotExsitException_when_appointment_with_given_id_is_not_exist()
        {
            var appointmentId = 100;

            Action expected = () => _sut.Delete(appointmentId);

            expected.Should().ThrowExactly<AppointmentDoesNotExsitException>();
        }

        [Fact]
        public void Add_throw_NotEnoughSpaceException_when_doctor_has_more_than_five_appointment_in_one_day()
        {
            AddApointmentDto dto = Create_listOfAppointment_And_updateDto();

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<NotEnoughSpaceException>();
        }

        [Fact]
        public void Add_throw_AppointmentAlreadyExistException_when_doctor_has_appointment_with_patient_inOne_day()
        {
            var doctor = DoctorFactory.CreateDoctor();
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));
            var patient = PatientFactory.CreatePatient();
            _dataContext.Manipulate(_ => _.Patients.Add(patient));
            var appointment = CreateAppointment(doctor, patient);
            _dataContext.Manipulate(_ => _.Appointments.Add(appointment));
            AddApointmentDto dto = CreatAppointmentDto(doctor, patient);
            
            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<AppointmentAlreadyExistException>();
        }
        [Fact]
        public void Update_throw_NotEnoughSpaceException_when_doctor_has_more_than_five_appointment_in_one_day()
        {
            var doctor = DoctorFactory.CreateListOfDoctors();
            _dataContext.Manipulate(_ => _.Doctors.AddRange(doctor));
            var patientList = PatientFactory.CreateListOfPatients();
            _dataContext.Manipulate(_ => _.Patients.AddRange(patientList));
            var appointmentList = CreateListOfAppointment();
            _dataContext.Manipulate(_ => _.Appointments.AddRange(appointmentList));
            UpdateAppointmentDto dto = CreateUpdateDto();

            Action expected = () => _sut.Update(6, dto);

            expected.Should().ThrowExactly<NotEnoughSpaceException>();
        }

        [Fact]
        public void Update_throw_AppointmentAlreadyExistException_when_doctor_has_appointment_with_patient()
        {
            var appointment = Create_doctor_patient_appointment();
            UpdateAppointmentDto dto = CreateUpdateDto();
            var appointmentId = 1;

            Action expected = () => _sut.Update(appointmentId,dto);

            expected.Should().ThrowExactly<AppointmentAlreadyExistException>();
        }

        private static UpdateAppointmentDto CreateUpdateDto()
        {
            return new UpdateAppointmentDto
            {
                Date = DateTime.Now.Date,
                DoctorId = 1,
                PatientId = 1
            };
        }
       
        private static Appointment CreateAppointment(Doctor doctor, Patient patient)
        {
            return new Appointment
            {
                Date = DateTime.Now.Date,
                DoctorId = doctor.Id,
                PatientId = patient.Id,
            };
        }

        private static AddApointmentDto CreateAppointmnetDtoForException(Doctor doctor)
        {
            return new AddApointmentDto
            {
                Date = DateTime.Now.Date,
                DoctorId = doctor.Id,
                PatientId = 6
            };
        }

        private static UpdateAppointmentDto CreateUpdateAppointmentDto()
        {
            return new UpdateAppointmentDto
            {
                Date = DateTime.Now.Date,
                PatientId = 5,
                DoctorId = 2
            };
        }

        private static List<Appointment> CreateListOfAppointmnet()
        {
            return new List<Appointment>
            {
                new Appointment
                {
                    Date = DateTime.Now.Date,
                    DoctorId=1,
                    PatientId=1,
                },
                new Appointment
                {
                    Date = DateTime.Now.Date,
                    DoctorId=1,
                    PatientId=2,
                },
                new Appointment
                {
                    Date = DateTime.Now.Date,
                    DoctorId=1,
                    PatientId=3,
                },
                new Appointment
                {
                    Date = DateTime.Now.Date,
                    DoctorId=1,
                    PatientId=4,
                },
                new Appointment
                {
                    Date = DateTime.Now.Date,
                    DoctorId=1,
                    PatientId=5,
                }
            };
        }

        private static AddApointmentDto CreatAppointmentDto(Entities.Doctor doctor, Entities.Patient patient)
        {
            return new AddApointmentDto
            {
                Date = DateTime.Now.Date,
                DoctorId = doctor.Id,
                PatientId = patient.Id,
            };
        }
        
        private static List<Appointment> CreateListOfAppointment()
        {
            return new List<Appointment>
            {
                new Appointment
                {
                    Id = 1,
                    Date = DateTime.Now.Date,
                    DoctorId=1,
                    PatientId=1,
                },
                new Appointment
                {
                    Id=2,
                    Date = DateTime.Now.Date,
                    DoctorId=1,
                    PatientId=2,
                },
                new Appointment
                {
                    Id=3,
                    Date = DateTime.Now.Date,
                    DoctorId=1,
                    PatientId=3,
                },
                new Appointment
                {
                    Id=4,
                    Date = DateTime.Now.Date,
                    DoctorId=1,
                    PatientId=4,
                },
                new Appointment
                {
                    Id=5,
                    Date = DateTime.Now.Date,
                    DoctorId=1,
                    PatientId=5,
                },new Appointment
                {
                    Id=6,
                    Date = DateTime.Now.Date,
                    DoctorId=2,
                    PatientId=1
                }
            };
        }
        private Appointment Create_doctor_patient_appointment()
        {
            var doctor = DoctorFactory.CreateDoctor();
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));
            var patient = PatientFactory.CreatePatient();
            _dataContext.Manipulate(_ => _.Patients.Add(patient));
            Appointment appointmnet = CreateAppointment(doctor, patient);
            _dataContext.Manipulate(_ => _.Appointments.Add(appointmnet));
            return appointmnet;
        }
        private List<Appointment> Create_list_of_appointments()
        {
            var doctor = DoctorFactory.CreateListOfDoctors();
            _dataContext.Manipulate(_ => _.Doctors.AddRange(doctor));
            var patientList = PatientFactory.CreateListOfPatients();
            _dataContext.Manipulate(_ => _.Patients.AddRange(patientList));
            List<Appointment> appointmentList = CreateListOfAppointment();
            _dataContext.Manipulate(_ => _.Appointments.AddRange(appointmentList));
            return appointmentList;
        }
        private void Create_doctor_patient()
        {
            var doctor = DoctorFactory.CreateDoctor();
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));
            var patient = PatientFactory.CreatePatient();
            _dataContext.Manipulate(_ => _.Patients.Add(patient));
        }
        private AddApointmentDto Create_listOfAppointment_And_updateDto()
        {
            var doctor = DoctorFactory.CreateDoctor();
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));
            var patientList = PatientFactory.CreateListOfPatients();
            _dataContext.Manipulate(_ => _.Patients.AddRange(patientList));
            var appointmentList = CreateListOfAppointmnet();
            _dataContext.Manipulate(_ => _.Appointments.AddRange(appointmentList));
            AddApointmentDto dto = CreateAppointmnetDtoForException(doctor);
            return dto;
        }
    }
}
