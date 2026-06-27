using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories;

public interface IAttachmentRepository
{
    IEnumerable<string> GetEmployeeAttachments(int employeeId);
    void AddAttachment(int employeeId, string filePath);
    void DeleteAttachment(string filePath);


}
