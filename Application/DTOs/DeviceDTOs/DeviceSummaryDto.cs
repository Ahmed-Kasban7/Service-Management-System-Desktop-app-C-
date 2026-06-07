using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.DeviceDTOs;

public record DeviceSummaryDto(int Id,string BrandName ,string TypeName ,string SpecName  , string ? Model , string ? SerialNumber);


