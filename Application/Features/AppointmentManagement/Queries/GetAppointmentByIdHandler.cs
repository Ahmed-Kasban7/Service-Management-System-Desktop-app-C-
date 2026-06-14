using Application.Common;
using Application.DTOs.AppointmentDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AppointmentManagement.Queries
{
    public class GetAppointmentByIdHandler
    {
        private readonly IAppointmentRepository _repository;

        public GetAppointmentByIdHandler(IAppointmentRepository repository)
            => _repository = repository;

        public Result<AppointmentDetailsDto> Handle(int appointmentId)
        {
            if (appointmentId <= 0)
                return Result<AppointmentDetailsDto>.Failure("رقم الموعد غير صالح");

          var appointment = _repository.GetById(appointmentId);
          return Result<AppointmentDetailsDto>.Success(appointment);
        }
    }
}
