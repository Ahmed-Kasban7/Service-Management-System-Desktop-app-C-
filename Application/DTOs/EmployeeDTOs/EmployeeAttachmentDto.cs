using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.EmployeeDTOs;

public class EmployeeAttachmentDto
{
    public int Id { get; set; }
    public byte[] AttachmentData { get; set; } 
}