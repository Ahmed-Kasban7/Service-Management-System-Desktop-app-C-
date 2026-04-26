using Application.DTOs.DeviceDTOs;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.OrderDTOs;

public record OrderDetailsDto(int OrderId,string OrderNumber, string Problem,string ? Notes, DateTime StartDate , DateTime ? EndDate , string State , 
    string CustomerName, string Address , string CustomerPhones , DeviceSummaryDto CustomerDevice);

