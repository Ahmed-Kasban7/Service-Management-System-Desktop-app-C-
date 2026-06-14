using Application.Common;
using Application.DTOs.AppointmentDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AppointmentManagement.Commands
{
      public class UpdateAppointmentHandler
      {
          private readonly IAppointmentRepository _repository;

          public UpdateAppointmentHandler(IAppointmentRepository repository)
          {
                _repository = repository;
          }

          public Result<bool> Handle(UpdateAppointmentDto dto)
          {
              if (dto == null)
                  return Result<bool>.Failure("بيانات غير صالحة");

              if (dto.TechnicianId <= 0)
                  return Result<bool>.Failure("برجاء اختيار الفني");

            if (dto.ScheduledDate == default ||dto.ScheduledDate < DateTime.Today)
                return Result<bool>.Failure("برجاء اختيار تاريخ صحيح");

            if (dto.VisitType == null)
                return Result<bool>.Failure("برجاء اختيار نوع الزيارة");

            var success = _repository.Update(dto);

             return Result<bool>.Success(success);
          }
      }

}
