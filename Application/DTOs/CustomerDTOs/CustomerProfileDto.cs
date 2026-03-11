using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public record  CustomerProfileDto(string ID , string Name , string Address , int Discount , int? Age , ESex Sex , List<DeviceInfoDTO> Devices , List<string> Phones);