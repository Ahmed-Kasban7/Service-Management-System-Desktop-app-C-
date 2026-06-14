using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums;
public enum EVisitType
{
    Diagnostic = 0,
    Repair = 1,
    PartsReplacement = 2,
    Pickup = 3,
    Delivery = 4,
    FollowingUp = 5
}