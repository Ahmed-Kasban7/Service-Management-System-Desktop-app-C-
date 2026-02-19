using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public record DeviceDTO
{
    public string Brand { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Spec { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
}