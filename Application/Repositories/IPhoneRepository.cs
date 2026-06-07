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

    bool AddPhone(string phoneNumber , int customerId);
    bool IsPhoneExist(string phoneNumber);
    public int GetPersonPhoneCount(int personId);

}
