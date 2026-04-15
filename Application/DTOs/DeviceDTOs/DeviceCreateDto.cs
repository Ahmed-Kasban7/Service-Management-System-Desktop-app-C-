using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.DeviceDTOs;

public record DeviceCreateDto (int BrandID  , int TypeID  , int SpecID , string? Model  , string? SerialNumber);