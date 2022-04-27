using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Infrastructure.Test;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Persistence.EF.Patients;
using DoctorAppointment.Services.Patients;
using DoctorAppointment.Services.Patients.Contracts;
using DoctorAppointment.Services.Patients.Exceptions;
using DoctorAppointment.Test.tools.Patients;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DoctorAppointment.Services.Test.Unit.Patients
{
    public class PatientServiceTest
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly PatientService _sut;
        private readonly PatientRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public PatientServiceTest()
        {
            _dataContext = new EFInMemoryDatabase()
                .CreateDataContext<ApplicationDbContext>();
            _repository = new EFPatientRepository(_dataContext);
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _sut = new PatientAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_patient_properly()
        {
            AddPatientDto dto = PatientFactory.CreateAddPatientDto();

            _sut.Add(dto);

            _dataContext.Patients.Should()
                .Contain(_ => _.FirstName == dto.FirstName &&
                _.LastName == dto.LastName &&
                _.NationalCode == dto.NationalCode);
        }

        [Fact]
        public void Add_throw_patientAlreadyExistException_when_patient_with_nationalCode_exist()
        {
            Patient patient =PatientFactory.CreatePatient();
            _dataContext.Manipulate(_ => _.Patients.Add(patient));
            AddPatientDto dto = PatientFactory.CreateAddPatientDto();

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<PatientAlreadyExistException>();
        }

        [Fact]
        public void GetAll_returns_all_patients()
        {
            Patient patient = PatientFactory.CreatePatient();
            _dataContext.Manipulate(_ => _.Patients.Add(patient));

            var expected = _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.FirstName == patient.FirstName &&
            _.LastName == patient.LastName &&
            _.NationalCode == patient.NationalCode);
        }

        [Fact]
        public void Update_updates_patient_properly()
        {
            Patient patient = PatientFactory.CreatePatient();
            _dataContext.Manipulate(_ => _.Patients.Add(patient));
            UpdatePatientDto dto =PatientFactory.CreateUpdateDto();

            _sut.Update(patient.Id, dto);

            var expected = _dataContext.Patients.
                FirstOrDefault(_ => _.Id == patient.Id);
            expected.FirstName.Should().Be(dto.FirstName);
            expected.LastName.Should().Be(dto.LastName);
            expected.NationalCode.Should().Be(dto.NationalCode);
        }

        [Fact]
        public void Update_throw_patientDoesNotExsitException_when_patient_with_given_id_is_not_exist()
        {
            var patientId = 100;
            UpdatePatientDto dto = PatientFactory.CreateUpdateDto();

            Action expected = () => _sut.Update(patientId, dto);

            expected.Should().ThrowExactly<PatientDoesNotExsitException>();
        }

        [Fact]
        public void Update_throw_PatientAlreadyExistException_when_patient_with_natinalCode_exist()
        {
            List<Patient> patients = PatientFactory.CreateListOfPatients();
            _dataContext.Manipulate(_ => _.Patients.AddRange(patients));
            UpdatePatientDto dto = PatientFactory.CreateUpdateDto();
            var patientId = patients[1].Id;

            Action expected = () => _sut.Update(patientId, dto);

            expected.Should().ThrowExactly<PatientAlreadyExistException>();
        }

        [Fact]
        public void Delete_deletes_Patient_properly()
        {
            Patient patient = PatientFactory.CreatePatient();
            _dataContext.Manipulate(_ => _.Patients.Add(patient));
            var patientId = patient.Id;

            _sut.Delete(patientId);

            _dataContext.Patients.Should().
                NotContain(_ => _.Id == patientId);
        }

        [Fact]
        public void Delete_throw_PatientDoesNotExsitException_when_patient_with_given_id_is_not_exist()
        {
            var patientId = 100;

            Action expected = () => _sut.Delete(patientId);

            expected.Should().ThrowExactly<PatientDoesNotExsitException>();
        }
    }
}
