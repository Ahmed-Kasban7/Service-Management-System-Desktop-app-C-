using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.DeviceDTOs;

public record SpecDto(int SpecID, string SpecName, int TypeID) {

    public override string ToString()
    {
        return SpecName;
    }

}

