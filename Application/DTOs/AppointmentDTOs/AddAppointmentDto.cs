using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.AppointmentDTOs;

public record AddAppointmentDto(int orderId , int TachnicianId,int? AssistanId, int? DriverId, DateTime ScheduledDate , string Notes , EVisitType VisitType );