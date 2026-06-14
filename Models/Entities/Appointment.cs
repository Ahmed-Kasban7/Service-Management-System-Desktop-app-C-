using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Appointment:BaseEntity
{
    public int OrderId { get; private set; }
    public int TechnicianId { get; private set; }
    public int? TechnicianAssistantId { get; private set; }
    public int? DriverId { get; private set; }
    public DateTime ScheduledDate { get; private set; }
    public EAppointmentState AppointmentState { get; private set; }
    public EVisitType VisitType { get; private set; }
    public string? Notes { get; private set; }

    public Appointment(
      int orderId,
      int technicianId,
      int? technicianAssistantId,
      int? driverId,
      DateTime scheduledDate,
      EVisitType visitType,
      string? notes)
    {
        OrderId = orderId;
        TechnicianId = technicianId;
        TechnicianAssistantId = technicianAssistantId;
        DriverId = driverId;
        ScheduledDate = scheduledDate;
        AppointmentState = EAppointmentState.SCHEDULED;
        VisitType = visitType;
        Notes = notes;
    }

    protected Appointment() { }

  
}