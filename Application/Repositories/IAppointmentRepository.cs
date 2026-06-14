using Application.Common;
using Application.DTOs.AppointmentDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories;

public interface IAppointmentRepository
{
    int Create(Appointment entity);
    bool Update(UpdateAppointmentDto entity);
    public IEnumerable<AppointmentSummaryDto> GetAppointmentByOrderId(int orderId);
    public AppointmentDetailsDto? GetById(int appointmentId);
    public bool Cancel(int appointmentId , string CancelReason);

}
