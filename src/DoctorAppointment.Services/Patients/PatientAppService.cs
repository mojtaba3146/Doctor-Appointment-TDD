using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Services.Patients.Contracts;
using DoctorAppointment.Services.Patients.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Services.Patients
{
    public class PatientAppService : PatientService
    {
        private readonly PatientRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public PatientAppService(PatientRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddPatientDto dto)
        {
            var patient = new Patient
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                NationalCode = dto.NationalCode,
            };

            var isPatientExist = _repository
                .IsExistNationalCode(patient.NationalCode);

            if (isPatientExist)
            {
                throw new PatientAlreadyExistException();
            }

            _repository.Add(patient);
            _unitOfWork.Commit();
        }

        public void Delete(int patientId)
        {
            var patient = _repository.GetById(patientId);

            if (patient == null)
            {
                throw new PatientDoesNotExsitException();
            }

            _repository.Delete(patient);
            _unitOfWork.Commit();
        }

        public List<GetAllPatientDto> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(int id, UpdatePatientDto dto)
        {
            var patient = _repository.GetById(id);

            if (patient == null)
            {
                throw new PatientDoesNotExsitException();
            }

            var isPatientExist = _repository
                .IsExistNationalCodeWithId(dto.NationalCode, patient.Id);

            if (isPatientExist)
            {
                throw new PatientAlreadyExistException();
            }

            patient.FirstName = dto.FirstName;
            patient.LastName = dto.LastName;
            patient.NationalCode = dto.NationalCode;

            _unitOfWork.Commit();
        }
    }
}
