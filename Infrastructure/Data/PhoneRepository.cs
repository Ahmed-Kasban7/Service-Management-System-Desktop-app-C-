using Domain.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data;

public class PhoneRepository
{
    public List<string> GetCustomerPhonesBy (int customerid)
    {
        var phones = new List<string>();

        var script = @"select PhoneNumber from Phones where PersonID = @customerid";
        using var conn = DatabaseInitializer.GetConnection();
        using var command = new  SqlCommand(script, conn);
        command.Parameters.AddWithValue("customerid", customerid);
        conn.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            phones.Add(reader["PhoneNumber"].ToString());
        }
        return phones;
    }
}
