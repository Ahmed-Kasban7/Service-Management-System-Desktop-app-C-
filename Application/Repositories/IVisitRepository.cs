using Application.DTOs.SparePartDtos;
using Application.DTOs.VisitDTOs;
using Application.Features.VisitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories;

public interface  IVisitRepository
{
    public int CreateVisitWithParts(CreateVisistDto newVisit);
    public VisitDetailsDto GetVisitDetailsByAppointmentId(int appointmentId);

}
