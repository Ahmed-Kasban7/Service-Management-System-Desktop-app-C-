using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.OrderDTOs;

public record class OrderSummaryDto(string OrderNumber , string CustomerName , string CustomerPhone ,string Address, string Problem , DateTime Date ,EOrderState State);
