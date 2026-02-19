using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public record  CustomerProfileDTO
{
    public string ID { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int Discount { get; set; }
    public int? Age  { get; set; }
    public string Sex { get; set; } =  string.Empty;

    public List<DeviceDTO> Devices { get; set; } = new();
    public List<string> Phones { get; set; } = new();


}
