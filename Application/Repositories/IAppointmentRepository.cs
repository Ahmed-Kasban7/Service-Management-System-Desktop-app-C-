using Application.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories;

public interface IAppointmentRepository:ICommandRepository<Appointment>
{
    int Create(Appointment entity);
    int Update(Appointment entity);
}
