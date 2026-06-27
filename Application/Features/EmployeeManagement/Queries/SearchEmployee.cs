using Application.Common;
using Application.DTOs.EmployeeDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.EmployeeManagement.Queries;

public class SearchEmployeeHandler
{
    private readonly IEmployeeRepository _employeeRepository;

    public SearchEmployeeHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public PagedResult<EmployeeSummaryDto> Handle(int pageNumber, int PageSize , string searchWord)
    {
        if (pageNumber <= 0)
            pageNumber = 1;

        if (PageSize <= 0 || PageSize > 100)
            PageSize = 8;

        if (string.IsNullOrWhiteSpace(searchWord))
            searchWord = "";

        var employees = _employeeRepository.SearchEmployee(pageNumber, PageSize , searchWord);


        return employees;

    }
}