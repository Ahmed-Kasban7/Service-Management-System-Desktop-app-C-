using Application.DTOs.DeviceDTOs;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public record  CustomerBasicInfoDto(int ID , string Code , string Name , string Address , int Discount , int? Age , ESex Sex , DateTime CreatedDate);