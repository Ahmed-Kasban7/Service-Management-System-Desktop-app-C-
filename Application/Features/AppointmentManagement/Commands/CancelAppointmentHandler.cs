using Application.Common;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AppointmentManagement.Commands
{
    public class CancelAppointmentHandler
    {
        private readonly IAppointmentRepository _repository;

        public CancelAppointmentHandler(IAppointmentRepository repository)
            => _repository = repository;

        public Result<bool> Handle(int appointmentId , string CancelReason )
        {
            if (appointmentId <= 0)
                return Result<bool>.Failure("رقم الموعد غير صالح");

            if (string.IsNullOrWhiteSpace(CancelReason))
                return Result<bool>.Failure("يجب تحديد سبب إلغاء الموعد.");

            _repository.Cancel(appointmentId , CancelReason);
            return Result<bool>.Success(true);
          
        }
    }
}
