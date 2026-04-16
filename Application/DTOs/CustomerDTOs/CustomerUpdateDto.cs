using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.CustomerDTOs;

public record CustomerUpdateDto(int Id , string Name , int? Age , ESex Sex , string Address , int Discount = 0);