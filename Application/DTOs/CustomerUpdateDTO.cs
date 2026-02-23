using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public class CustomerUpdateDTO
{
    public string Name { get; set; }
    public int ?Age { get; set; }
    public string Sex { get; set; }
    public string Address { get; set; }
    public int ? Discount { get; set; } = 0;
}
