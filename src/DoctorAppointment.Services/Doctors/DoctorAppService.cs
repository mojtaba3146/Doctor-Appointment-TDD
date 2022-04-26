using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Services.Doctors.Contracts;
using DoctorAppointment.Services.Doctors.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Services.Doctors
{
    public class DoctorAppService : DoctorService
    {
        private readonly DoctorRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public DoctorAppService(
            DoctorRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddDoctorDto dto)
        {
            var doctor = new Doctor
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Field = dto.Field,
                NationalCode = dto.NationalCode,
            };

            var isDoctorExist = _repository
                .IsExistNationalCode(doctor.NationalCode);

            if (isDoctorExist)
            {
                throw new DoctorAlreadyExistException();
            }

            _repository.Add(doctor);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var doctor = _repository.GetById(id);

            if (doctor == null)
            {
                throw new DoctorDoesNotExsitException();
            }
            _repository.Delete(doctor);
            _unitOfWork.Commit();
        }

        public List<GetAllDoctorsDto> GetAll()
        {
            return _repository.GetAllDoctors();
        }

        public void Update(int id, UpdateDoctorDto dto)
        {
           var doctor=_repository.GetById(id);

            if (doctor == null)
            {
                throw new DoctorDoesNotExsitException();
            }

            var isDoctorExist = _repository
                .IsExistNationalCodeWithId(dto.NationalCode, doctor.Id);

            if (isDoctorExist)
            {
                throw new DoctorNationalCodeAlreadyExist();
            }

            doctor.FirstName = dto.FirstName;
            doctor.LastName = dto.LastName;
            doctor.Field = dto.Field;
            doctor.NationalCode = dto.NationalCode;

            _unitOfWork.Commit();
        }
    }
}
