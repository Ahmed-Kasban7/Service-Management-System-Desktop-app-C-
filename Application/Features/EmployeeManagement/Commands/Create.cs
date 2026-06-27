using Application.Common;
using Application.DTOs;
using Application.DTOs.EmployeeDTOs;
using Application.DTOs.PersonDTOs;
using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.EmployeeManagement.Commands;

public class CreateEmployeeHandler
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPhoneRepository _phoneRepository;
    public event Action EmployeeCreated;

    public CreateEmployeeHandler(
        IEmployeeRepository employeeRepository,
        IPhoneRepository phoneRepository)
    {
        _employeeRepository = employeeRepository;
        _phoneRepository = phoneRepository;
    }

    public Result<int> Handle(CreateEmployeeDto employeeDto)
    {
        if (employeeDto.Phones == null || !employeeDto.Phones.Any())
            return Result<int>.Failure("يجب إدخال رقم هاتف واحد على الأقل");

        List<Phone> phones;
        try
        {
            phones = employeeDto.Phones
                .Select(p => new Phone(p))
                .ToList();
        }
        catch (Exception ex)
        {
            return Result<int>.Failure(ex.Message);
        }

        // DB check phones
        var phoneNumbers = phones.Select(p => p.PhoneNumber);
        var existingPhones = _phoneRepository.GetExistingPhones(phoneNumbers);
        if (existingPhones.Any())
            return Result<int>.Failure(
                $"الأرقام التالية مسجلة بالفعل: {string.Join(", ", existingPhones)}");

      
       int employeeId = _employeeRepository.Create(employeeDto);

        EmployeeCreated?.Invoke();

        return Result<int>.Success(employeeId);
      
    }
}