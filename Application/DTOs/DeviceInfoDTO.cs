using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public record  DeviceInfoDTO
{
    public int  DeviceId { get; set; }
    public string BrandName { get; set; }

    public int BrandID { get; set; }
    public string TypeName { get; set; }
    public int TypeID { get; set; }
    public string SpecName { get; set; }
    public int SpecID { get; set; }
    public string Model { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
}
