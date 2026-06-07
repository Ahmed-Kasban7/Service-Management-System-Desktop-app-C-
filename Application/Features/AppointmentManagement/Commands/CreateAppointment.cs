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
    public class CreateAppointmentHandler
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IOrderRepository _orderRepository;

        public CreateAppointmentHandler(IAppointmentRepository appointmentRepository , IOrderRepository orderRepository)
        {
            _appointmentRepository = appointmentRepository;
            _orderRepository = orderRepository;
        }
        //public Result<int> Handle(AddAppointmentDto appointmentDto)
        //{
        //    if (appointmentDto == null)
        //        return Result<int>.Failure("بيانات المعاد غير صالحة");

        //    if(!_orderRepository.IsOrderExists(appointmentDto.orderId))
        //        return Result<int>.Failure("الاوردر غير موجود");

        //}
    }
}
