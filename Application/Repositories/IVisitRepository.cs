using Application.DTOs.SparePartDtos;
using Application.Features.VisitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    interface  IVisitRepository
    {
        public bool CreateVisitWithParts(int appointmentId, string notes, string actionsTaken, string diagnosis, decimal totalCost, List<SparePartDTO> spareParts);
    }
}
