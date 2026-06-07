using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.DeviceDTOs;

public record DeviceAddDTO
{
    public int BrandID { get; init; }
    public int TypeID { get; init; }
    public int SpecID { get; init; }
    public string? Model { get; init; } = string.Empty;
    public string? SerialNumber { get; init; } = string.Empty;
}
