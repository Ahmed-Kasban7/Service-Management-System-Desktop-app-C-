using Microsoft.Data.SqlClient;

using Application.Repositories;
using System.Data;

namespace Infrastructure.Data;

public class PhoneRepository :IPhoneRepository
{
    public List<string> GetCustomerPhonesBy (int customerid)
    {
        var phones = new List<string>();

        using var conn = DatabaseInitializer.GetConnection();
        using var command = new  SqlCommand("SP_GetCustomerPhones", conn);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("customerid", customerid);
        conn.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            phones.Add(reader["PhoneNumber"].ToString());
        }
        return phones;
    }

    public bool DeletePhone(string phoneNumber)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_DeletePhone", conn);
        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
        command.CommandType =CommandType.StoredProcedure;
        conn.Open();
        var result = command.ExecuteNonQuery();
        return result > 0;
    }
    public bool UpdatePhone(string newPhone , string oldPhone)
    {
        var script = @"update Phones
                        set PhoneNumber = @newPhone
                        where PhoneNumber=@oldPhone ";
        using var conn = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand(script, conn);
        command.Parameters.AddWithValue("newPhone", newPhone);
        command.Parameters.AddWithValue("oldPhone", oldPhone);

        conn.Open();
        var result = command.ExecuteNonQuery();
        return result > 0;
    }

    public bool AddPhone(string phoneNumber, int personId)
    {

        using var conn = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_AddCustomerPhone", conn);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
        command.Parameters.AddWithValue("@personId", personId);

        conn.Open();
        var result = command.ExecuteNonQuery();
        return result > 0;
    }

    public bool PhoneExists(string phoneNumber)
    {
        var script = @"SELECT 1 FROM Phones WHERE PhoneNumber = @phoneNumber";

        using var conn = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand(script, conn);
        command.Parameters.AddWithValue("@phoneNumber", phoneNumber);

        conn.Open();

        var result = command.ExecuteScalar();

        return result != null;
    }

    public int GetPersonPhoneCount(int personId)
    {
        var script = @"select count(*) from Phones where PersonID = @personId";
        using var conn = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand(script, conn);
        command.Parameters.AddWithValue("personId", personId);
        conn.Open();

        var result = command.ExecuteScalar();

        return (result == null) ? 0 : (int)result;
    }


}
