using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.DeviceDTOs;

public record TypeDto(int TypeID, string TypeName) {
    public override string ToString()
    {
        return TypeName;
    }
}

