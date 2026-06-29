using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AttachmentManagement.Commands;

public class AddAttachmentHandler
{
    private readonly IAttachmentRepository _attachmentRepository;

    public AddAttachmentHandler(IAttachmentRepository attachmentRepository)
    {
        _attachmentRepository = attachmentRepository;
    }

    public void Handle (int employeeId  , byte[] ImageData)
    {
        if (employeeId <= 0)
            throw new ArgumentException("رقم الموظف غير صالح");

        if (ImageData==null || ImageData.Length<=0 )
            throw new ArgumentException("الملف لا يمكن أن يكون فارغاً");

         _attachmentRepository.AddAttachment(employeeId, ImageData);
    }
}
