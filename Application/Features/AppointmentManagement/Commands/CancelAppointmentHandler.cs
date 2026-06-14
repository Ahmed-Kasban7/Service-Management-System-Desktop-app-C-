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

        public Result<bool> Handle(int appointmentId)
        {
            if (appointmentId <= 0)
                return Result<bool>.Failure("رقم الموعد غير صالح");

            try
            {
                _repository.Cancel(appointmentId);
                return Result<bool>.Success(true);
            }
            catch (InvalidOperationException ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
            catch
            {
                return Result<bool>.Failure("حدث خطأ أثناء إلغاء الموعد");
            }
        }
    }
}
