using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Order : BaseEntity
{
    public string OrderNumber { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string Problem { get; private set; }
    public string? Notes { get; private set; }
    public int CustomerId { get; private set; }
    public int DeviceId { get; private set; }
    public EOrderState OrderState { get; private set; }

    public Order(string problem, string? notes, int customerId, int deviceId)
    {
        SetProblem(problem);
        SetNotes(notes);
        SetCustomerId(customerId);
        SetDeviceId(deviceId);
        StartDate = DateTime.Now;
        OrderState = EOrderState.Pending;
    }

    public Order(int id, string orderNumber, DateTime startDate, DateTime? endDate,
        string problem, string? notes, int customerId,
        int deviceId, EOrderState orderState)
        : this(problem, notes, customerId, deviceId)
    {
        Id = id;
        OrderNumber = orderNumber;
        StartDate = startDate;
        EndDate = endDate;
        OrderState = orderState;
    }

    private void SetProblem(string problem)
    {
        if (string.IsNullOrWhiteSpace(problem))
            throw new ArgumentException("وصف المشكلة مطلوب");

        Problem = problem.Trim();
    }

    private void SetNotes(string? notes)
    {
        Notes = notes?.Trim();
    }

    private void SetCustomerId(int customerId)
    {
        if (customerId <= 0)
            throw new ArgumentException(" رقم العميل غير صالح");

        CustomerId = customerId;
    }

    private void SetDeviceId(int deviceId)
    {
        if (deviceId <= 0)
            throw new ArgumentException("رقم الجهاز غير صالح");

        DeviceId = deviceId;
    }

    public void UpdateProblem(string problem) => SetProblem(problem);
    public void UpdateNotes(string? notes) => SetNotes(notes);

    public void UpdateState(EOrderState newState)
    {
        if (!Enum.IsDefined(typeof(EOrderState), newState))
            throw new ArgumentException("حالة الطلب غير صالحة");

        if (OrderState == EOrderState.Completed || OrderState == EOrderState.Cancelled)
            throw new InvalidOperationException("لا يمكن تغيير حالة طلب منتهي");

        OrderState = newState;

        if (newState == EOrderState.Completed || newState == EOrderState.Cancelled)
            EndDate = DateTime.Now;
    }
}