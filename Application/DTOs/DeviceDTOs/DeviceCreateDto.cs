using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.DeviceDTOs;

public record DeviceCreateDto (int BrandID  ,string BrandName , int TypeID , string TypeName   , int SpecID ,string SpecName, string? Model  , string? SerialNumber);