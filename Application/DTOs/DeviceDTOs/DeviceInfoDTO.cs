using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.DeviceDTOs;

public record  DeviceInfoDTO
{
    public DeviceInfoDTO(int deviceId, string brandName, int brandID, string typeName, int typeID, string specName, int specID, string model, string serialNumber)
    {
        DeviceId = deviceId;
        BrandName = brandName;
        BrandID = brandID;
        TypeName = typeName;
        TypeID = typeID;
        SpecName = specName;
        SpecID = specID;
        Model = model;
        SerialNumber = serialNumber;
    }

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
