using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Treasury;

public record BalanceDTOs(decimal balance , DateTime LastUpdate);