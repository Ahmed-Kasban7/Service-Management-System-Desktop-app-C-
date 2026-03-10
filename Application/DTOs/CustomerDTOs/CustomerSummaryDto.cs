using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.CustomerDTOs;

public record  CustomerSummaryDto(string ID, string Name , string Address , string Phone);