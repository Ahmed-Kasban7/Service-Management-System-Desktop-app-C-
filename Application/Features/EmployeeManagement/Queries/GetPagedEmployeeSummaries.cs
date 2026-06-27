using Application.Common;
using Application.DTOs.CustomerDTOs;
using Application.DTOs.EmployeeDTOs;
using Application.DTOs.PersonDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.EmployeeManagement.Queries;

public class GetPagedEmployeeSummariesHandler
{

    private readonly IEmployeeRepository _employeeRepository;

    public GetPagedEmployeeSummariesHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public PagedResult<EmployeeSummaryDto> Handle(int pageNumber, int PageSize)
    {
        if (pageNumber <= 0)
            pageNumber = 1;

        if (PageSize <= 0 || PageSize > 100)
            PageSize = 8;


        var employees = _employeeRepository.GetPagedEmployeeSummaries(pageNumber, PageSize);


        return employees;

    }
}
