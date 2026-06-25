using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.SparePartDtos;

public record SparePartDto(string PartName , int Quantity , decimal UnitPrice);
