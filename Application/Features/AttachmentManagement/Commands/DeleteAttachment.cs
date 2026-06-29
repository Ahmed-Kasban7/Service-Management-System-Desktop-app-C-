using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AttachmentManagement.Commands;

public class DeleteAttachmentHandler
{

    private readonly IAttachmentRepository _attachmentRepository;

    public DeleteAttachmentHandler(IAttachmentRepository attachmentRepository)
    {
        _attachmentRepository = attachmentRepository;
    }

    public void Handle(int id)
    {

        if (id <=0)
            throw new ArgumentException("رقم الملف غير صالح");

        _attachmentRepository.DeleteAttachment(id);
    }
}
