using Application.Common;
using Application.DTOs.VisitDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.VisitManagement;

public class CreateVisitHandler
{
    private readonly IVisitRepository _repository;

    public CreateVisitHandler(IVisitRepository repository)
    {
        _repository = repository;   
    }

    public Result<int> Handle(CreateVisistDto newVisit)
    {
        if (newVisit.AppointmentID <= 0)
            return Result<int>.Failure("رقم الموعد غير صالح");

        if (string.IsNullOrWhiteSpace(newVisit.Diagnosis))
            return Result<int>.Failure("التشخيص مطلوب");

        if(newVisit.TotalCostToCustomer < 0)
            return Result<int>.Failure("المبلغ الكلى على العميل لا يمكن ان يكون سالباً");

        if(newVisit.TransportationCost < 0)
            return Result<int>.Failure("تكلف النقل لا يمكن ان تكون سالباً");

        if (newVisit.AmountPaid < 0)
            return Result<int>.Failure("المبلغ المدفوع لا يمكن ان يكون سالباً");

        if (newVisit.SpareParts.Any())
        {
            if (newVisit.PartsTransportationCost < 0)
                return Result<int>.Failure("تكلفة انتقالات القطع لا يمكن أن تكون بالسالب.");

            if (newVisit.TransportationBearer == null)
                return Result<int>.Failure("برجاء تحديد الطرف المتحمل لتكلفة انتقالات القطع.");

            foreach (var part in newVisit.SpareParts)
            {
                if (string.IsNullOrWhiteSpace(part.PartName))
                    return Result<int>.Failure("اسم القطلع مطلوب");
                if (part.Quantity <= 0)
                    return Result<int>.Failure($"الكميه يجب ان تكون اكبر من 0");
                if (part.UnitPrice < 0)
                    return Result<int>.Failure($"تكلفه الشراء لا يمكن ان تكون سالبه");
            }
        }

        int visitId = _repository.CreateVisitWithParts(newVisit);
        return Result<int>.Success(visitId);
    }

}
