using Application.DTOs.EmployeeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories;

public interface IAttachmentRepository
{
    IEnumerable<EmployeeAttachmentDto> GetEmployeeAttachments(int employeeId);
    void AddAttachment(int employeeId, byte[] attachmentData);
    public void DeleteAttachment(int attachmentId);


}
