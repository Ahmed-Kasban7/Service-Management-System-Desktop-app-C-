using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public class CustomerUpdateDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ?Age { get; set; }
    public ESex Sex { get; set; }
    public string Address { get; set; }
    public int ? Discount { get; set; } = 0;
}
