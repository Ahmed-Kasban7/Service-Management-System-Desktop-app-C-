using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common;

internal interface IAuditable
{
    public DateTime CreateAt { get; set; }
    public int CreateBy { get; set; }
}
