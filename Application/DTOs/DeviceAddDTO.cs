using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public record DeviceAddDTO
{
    public int BrandID { get; set; }
    public int TypeID { get; set; }
    public int SpecID { get; set; }
    public string? Model { get; set; } = string.Empty;
    public string? SerialNumber { get; set; } = string.Empty;
}
