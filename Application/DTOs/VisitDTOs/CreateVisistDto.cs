using Application.DTOs.SparePartDtos;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.VisitDTOs;

public record CreateVisistDto(int AppointmentID , string Diagnosis , string ? ActionsTaken ,  string? Notes , 
    decimal TotalCostToCustomer , decimal TransportationCost , decimal AmountPaid , ETransportationBearer? TransportationBearer ,
    decimal ?  PartsTransportationCost ,int ? PaidByEmployeeID , IEnumerable<SparePartDto> SpareParts);
