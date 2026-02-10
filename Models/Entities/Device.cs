using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public  class Device
{
    public int DeviceID { get; set ; }
    public string? SerialNumber { get;  set ; } = string.Empty;
    public string? ModelName { get;  set; } = string.Empty;
    public int BrandID { get;  set; }
    public int TypeID { get;  set; }
    public int SpecID { get;  set; }

    public int CustomerID { get; set; }
}
