using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AttachmentManagement.Queries;

public class GetEmployeeAttachmentsHandler
{
    private readonly IAttachmentRepository _attachmentRepository;

    public GetEmployeeAttachmentsHandler(IAttachmentRepository attachmentRepository)
    {
        _attachmentRepository = attachmentRepository;
    }

    public IEnumerable<string> Handle(int employeeId)
    {

        if (employeeId <= 0)
            throw new ArgumentException("رقم الموظف غير صالح"); 

        return  _attachmentRepository.GetEmployeeAttachments(employeeId);
    }
}
