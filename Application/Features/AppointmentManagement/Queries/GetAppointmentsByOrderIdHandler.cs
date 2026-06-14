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
    public class GetAppointmentsByOrderIdHandler
    {
        private readonly IAppointmentRepository _repository;

        public GetAppointmentsByOrderIdHandler(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public Result<IEnumerable<AppointmentSummaryDto>> Handle(int orderId)
        {
            if (orderId <= 0)
                return Result<IEnumerable<AppointmentSummaryDto>>.Failure("رقم الطلب غير صالح");

           var appointments = _repository.GetAppointmentByOrderId(orderId);

            return Result < IEnumerable < AppointmentSummaryDto >>.Success(appointments);
            
        }
    }
}
