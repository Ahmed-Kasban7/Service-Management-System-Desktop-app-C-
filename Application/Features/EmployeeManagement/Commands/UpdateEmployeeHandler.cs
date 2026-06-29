using Application.Common;
using Application.DTOs.EmployeeDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.EmployeeManagement.Commands
{
    public class UpdateEmployeeHandler
    {
        private readonly IEmployeeRepository _repository;

        public UpdateEmployeeHandler(IEmployeeRepository repository)
            => _repository = repository;

        public Result<bool> Handle(UpdateEmployeeDto dto)
        {
            if (dto == null)
                return Result<bool>.Failure("بيانات غير صالحة");

            
            
              var success = _repository.Update(dto);
              return success
                  ? Result<bool>.Success(true)
                  : Result<bool>.Failure("فشل تحديث بيانات الموظف");
            

        }
    }
}
