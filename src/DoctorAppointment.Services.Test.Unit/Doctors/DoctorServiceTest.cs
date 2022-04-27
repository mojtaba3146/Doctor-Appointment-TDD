using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Infrastructure.Test;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Persistence.EF.Doctors;
using DoctorAppointment.Services.Doctors;
using DoctorAppointment.Services.Doctors.Contracts;
using DoctorAppointment.Services.Doctors.Exceptions;
using DoctorAppointment.Test.tools.Doctors;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DoctorAppointment.Services.Test.Unit.Doctors
{
    public class DoctorServiceTest
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly DoctorService _sut;
        private readonly DoctorRepository _repository;
        private readonly UnitOfWork _unitOfWork;


        public DoctorServiceTest()
        {
            _dataContext = new EFInMemoryDatabase()
                .CreateDataContext<ApplicationDbContext>();
            _repository = new EFDoctorRepository(_dataContext);
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _sut = new DoctorAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_doctor_properly()
        {
            AddDoctorDto dto = DoctorFactory.CreateAddDoctorDto();

            _sut.Add(dto);

            _dataContext.Doctors.Should()
                .Contain(_ => _.FirstName == dto.FirstName &&
                _.LastName == dto.LastName &&
                _.Field == dto.Field &&
                _.NationalCode == dto.NationalCode);
        }

        [Fact]
        public void Add_throw_DoctorAlreadyExistException_when_natinalCode_already_exist()
        {
            Doctor doctor = DoctorFactory.CreateDoctor();
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));
            AddDoctorDto dto = DoctorFactory.CreateAddDoctorDto();

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<DoctorAlreadyExistException>();
        }

        [Fact]
        public void GetAll_returns_all_doctors()
        {
            Doctor doctor = DoctorFactory.CreateDoctor();
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));

            var expected = _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.FirstName == doctor.FirstName &&
            _.LastName == doctor.LastName &&
            _.Field == doctor.Field &&
            _.NationalCode == doctor.NationalCode);
        }

        [Fact]
        public void Update_updates_Doctor_Properly()
        {
            Doctor doctor = DoctorFactory.CreateDoctor();
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));
            UpdateDoctorDto dto = DoctorFactory.CreateUpdateDto();

            _sut.Update(doctor.Id, dto);

            var expected = _dataContext.Doctors.
                FirstOrDefault(_ => _.Id == doctor.Id);
            expected.FirstName.Should().Be(dto.FirstName);
            expected.LastName.Should().Be(dto.LastName);
            expected.Field.Should().Be(dto.Field);
            expected.NationalCode.Should().Be(dto.NationalCode);
        }

        [Fact]
        public void Update_throw_DoctorDoesNotExsitException_When_doctor_with_given_id_is_not_exist()
        {
            var doctorId = 100;
            UpdateDoctorDto dto = DoctorFactory.CreateUpdateDto();

            Action expected = () => _sut.Update(doctorId, dto);

            expected.Should().ThrowExactly<DoctorDoesNotExsitException>();
        }

        [Fact]
        public void Update_Throw_DoctorAlreadyExistException_when_nationalCode_already_exist()
        {
            List<Doctor> doctors = DoctorFactory.CreateListOfDoctors();
            _dataContext.Manipulate(_ => _.Doctors.AddRange(doctors));
            UpdateDoctorDto dto = DoctorFactory.CreateUpdateDto();
            var doctorId = doctors[1].Id;

            Action expected = () => _sut.Update(doctorId, dto);

            expected.Should().ThrowExactly<DoctorNationalCodeAlreadyExist>();
        }

        [Fact]
        public void Delete_deletes_Doctor_properly()
        {
            Doctor doctor = DoctorFactory.CreateDoctor();
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));
            var doctorId = doctor.Id;

            _sut.Delete(doctorId);

            _dataContext.Doctors.Should().
                NotContain(_ => _.Id == doctorId);
        }

        [Fact]
        public void Delete_throw_DoctorDoesNotExsitException_When_doctor_with_given_id_is_not_exist()
        {
            var doctorId = 100;

            Action expected = () => _sut.Delete(doctorId);

            expected.Should().ThrowExactly<DoctorDoesNotExsitException>();
        }
    }
}
