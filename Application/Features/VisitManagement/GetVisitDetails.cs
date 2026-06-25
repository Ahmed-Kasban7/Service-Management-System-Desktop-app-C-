using Application.DTOs.VisitDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.VisitManagement;

public class GetVisitDetailsHandler
{
    private readonly IVisitRepository _repository;

    public GetVisitDetailsHandler(IVisitRepository repository)
    {
        _repository = repository;
    }

    public VisitDetailsDto Handle(int appointmentId)
    {
        return _repository.GetVisitDetailsByAppointmentId(appointmentId);
    }

}
