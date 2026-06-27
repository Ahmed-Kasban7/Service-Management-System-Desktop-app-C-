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

    public void Handle(string filePath)
    {

        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("مسار الملف لا يمكن أن يكون فارغاً");

        _attachmentRepository.DeleteAttachment( filePath);
    }
}
