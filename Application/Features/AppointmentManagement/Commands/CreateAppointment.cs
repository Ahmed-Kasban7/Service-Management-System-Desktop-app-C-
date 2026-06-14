using Application.Common;
using Application.DTOs.AppointmentDTOs;
using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AppointmentManagement.Commands
{
    public class CreateAppointmentHandler
    {
        private readonly IAppointmentRepository _appointmentRepository;
      

        public CreateAppointmentHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public Result<int> Handle(AddAppointmentDto appointmentDto)
        {
            if (appointmentDto == null)
                return Result<int>.Failure("بيانات الموعد غير صالحة");

            if (appointmentDto.orderId <= 0)
                return Result<int>.Failure("الطلب غير صالح");

            if (appointmentDto.TachnicianId <= 0)
                return Result<int>.Failure("برجاء اختيار الفني");

            if (appointmentDto.ScheduledDate == default || appointmentDto.ScheduledDate < DateTime.Today)
                return Result<int>.Failure("برجاء اختيار تاريخ صحيح");

            if (appointmentDto.VisitType == null)
                return Result<int>.Failure("برجاء اختيار نوع الزيارة");

            var appointment = new Appointment(
                    appointmentDto.orderId,
                    appointmentDto.TachnicianId,
                    appointmentDto.AssistanId,
                    appointmentDto.DriverId,
                    appointmentDto.ScheduledDate,
                    appointmentDto.VisitType,
                    appointmentDto.Notes
            );

           int appointmentId =_appointmentRepository.Create(appointment);

            return Result<int>.Success(appointmentId);
        }
    }
}
