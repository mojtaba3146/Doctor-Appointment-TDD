using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Services.Appointments.Contracts;
using DoctorAppointment.Services.Appointments.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Services.Appointments
{
    public class AppointmentAppService : AppointmentService
    {
        private readonly AppointmentRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public AppointmentAppService(
            AppointmentRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddApointmentDto dto)
        {
            var appointment = new Appointment();
            PossibelityDto possibelity = new PossibelityDto();

            possibelity = _repository.
               GetPossibelity(dto.Date, dto.DoctorId, dto.PatientId);

            if (possibelity.VisitCount >= 5)
            {
                throw new NotEnoughSpaceException();
            }
            if (possibelity.ReapetCount > 0)
            {
                throw new AppointmentAlreadyExistException();
            }

            appointment.Date = dto.Date;
            appointment.DoctorId = dto.DoctorId;
            appointment.PatientId = dto.PatientId;

            _repository.Add(appointment);
            _unitOfWork.Commit();
        }

        public void Delete(int appointmentId)
        {
            var appointment = _repository.GetById(appointmentId);

            if (appointment == null)
            {
                throw new AppointmentDoesNotExsitException();
            }
            
            _repository.Delete(appointment);
            _unitOfWork.Commit();
        }

        public List<GetAllAppointmentDto> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(int id, UpdateAppointmentDto dto)
        {
            var appointment = _repository.GetById(id);
            PossibelityDto possibelity = new PossibelityDto();

            if (appointment == null)
            {
                throw new AppointmentDoesNotExsitException();
            }

            possibelity = _repository.
               GetPossibelity(dto.Date, dto.DoctorId, dto.PatientId);

            if (possibelity.VisitCount >= 5)
            {
                throw new NotEnoughSpaceException();
            }
            if (possibelity.ReapetCount > 0)
            {
                throw new AppointmentAlreadyExistException();
            }

            appointment.Date = dto.Date;
            appointment.DoctorId = dto.DoctorId;
            appointment.PatientId = dto.PatientId;

            _unitOfWork.Commit();
        }
    }
}
