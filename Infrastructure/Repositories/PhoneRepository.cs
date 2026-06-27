using Microsoft.Data.SqlClient;

using Application.Repositories;
using System.Data;

namespace Infrastructure.Data;

public class PhoneRepository :IPhoneRepository
{
    public IEnumerable<string> GetCustomerPhones (int customerid)
    {
        var phones = new List<string>();

        using var conn = DatabaseInitializer.GetConnection();
        using var command = new  SqlCommand("SP_GetCustomerPhones", conn);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@customerid", customerid);
        conn.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            phones.Add(reader["PhoneNumber"].ToString());
        }
        return phones;
    }
    public IEnumerable<string> GetEmployeePhones (int employeeId)
    {
        var phones = new List<string>();

        using var conn = DatabaseInitializer.GetConnection();
        using var command = new  SqlCommand("SP_GetEmployeePhones", conn);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@employeeId", employeeId);
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

        using var conn = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_UpdatePhone", conn);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@newPhone", newPhone);
        command.Parameters.AddWithValue("@oldPhone", oldPhone);

        conn.Open();
        var result = command.ExecuteNonQuery();
        return result > 0;
    }

    public bool AddCustomerPhone(string phoneNumber, int customerId)
    {

        using var conn = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_AddCustomerPhone", conn);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
        command.Parameters.AddWithValue("@customerId", customerId);

        conn.Open();
        var result = command.ExecuteNonQuery();
        return result > 0;
    }
    public bool AddEmployeePhone(string phoneNumber, int employeeId)
    {

        using var conn = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_AddEmployeePhone", conn);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
        command.Parameters.AddWithValue("@employeeId", employeeId);

        conn.Open();
        var result = command.ExecuteNonQuery();
        return result > 0;
    }

    public List<string> GetExistingPhones(IEnumerable<string> phones)
    {
        var table = new DataTable();
        table.Columns.Add("PhoneNumber", typeof(string));

        foreach (var phone in phones)
            table.Rows.Add(phone);

        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_GetExistingPhones", conn);

        cmd.CommandType = CommandType.StoredProcedure;

        var param = cmd.Parameters.AddWithValue("@Phones", table);
        param.SqlDbType = SqlDbType.Structured;
        param.TypeName = "PhoneList";

        conn.Open();

        var result = new List<string>();

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(reader.GetString(0));
        }

        return result;
    }

    public int GetCustomerPhoneCount(int customerId)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_GetCustomerPhoneCount", conn);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@customerId", customerId);

        SqlParameter countParam = new SqlParameter("@phoneCount", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(countParam);

        conn.Open();
        command.ExecuteNonQuery(); 

        return countParam.Value != DBNull.Value ? Convert.ToInt32(countParam.Value) : 0;
    }
    public int GetEmployeePhoneCount(int employeeId)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_GetEmployeePhoneCount", conn);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@employeeId", employeeId);

        SqlParameter countParam = new SqlParameter("@phoneCount", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(countParam);

        conn.Open();
        command.ExecuteNonQuery();

        return countParam.Value != DBNull.Value ? Convert.ToInt32(countParam.Value) : 0;
    }
    public bool IsPhoneExist(string phone)
    {
        using (SqlConnection connection = DatabaseInitializer.GetConnection())
        {
            using (SqlCommand command = new SqlCommand("SP_IsPhoneExist", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@phone", phone);

                connection.Open();

                object result = command.ExecuteScalar();

                return result != null;
            }
        }
    }


}
