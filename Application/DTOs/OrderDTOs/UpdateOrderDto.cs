using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.OrderDTOs;
public record UpdateOrderDto(int OrderId , string Problem, string? Notes);
