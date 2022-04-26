using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Infrastructure.Test;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Persistence.EF.Doctors;
using DoctorAppointment.Services.Doctors;
using DoctorAppointment.Services.Doctors.Contracts;
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
        public void Add_adds_Doctor_Properly()
        {
            AddDoctorDto dto = CreateAddDoctorDto();

            _sut.Add(dto);

            _dataContext.Doctors.Should()
                .Contain(_ => _.FirstName == dto.FirstName &&
                _.LastName == dto.LastName &&
                _.Field == dto.Field &&
                _.NationalCode == dto.NationalCode);
        }

        private static AddDoctorDto CreateAddDoctorDto()
        {
            return new AddDoctorDto
            {
                FirstName = "mojtaba",
                LastName = "khoshnam",
                NationalCode = "230",
                Field = "brain"
            };
        }
    }
}
