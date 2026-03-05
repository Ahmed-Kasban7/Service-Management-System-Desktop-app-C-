using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories;

public interface  IPhoneRepository
{
    bool DeletePhone(string phoneNumber);
    bool PhoneExists(string phoneNumber);
    bool UpdatePhone(string newPhone, string oldPhone);

    bool AddPhone(string phoneNumber , int customerId);
    public int GetPersonPhoneCount(int personId);

}
