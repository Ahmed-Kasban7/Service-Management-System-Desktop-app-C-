using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories;

public interface  IPhoneRepository
{
    bool DeletePhone(string phoneNumber);
    List<string> GetExistingPhones(IEnumerable<string> Phones);
    bool UpdatePhone(string newPhone, string oldPhone);
    IEnumerable<string> GetCustomerPhones(int customerId);

    bool AddCustomerPhone(string phoneNumber , int customerId);
    bool AddEmployeePhone(string phoneNumber , int employeeId);
    bool IsPhoneExist(string phoneNumber);
    int GetCustomerPhoneCount(int customerId);
    IEnumerable<string> GetEmployeePhones(int employeeId);
    public int GetEmployeePhoneCount(int employeeId);



}
