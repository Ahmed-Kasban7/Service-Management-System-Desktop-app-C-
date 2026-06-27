using Application.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class AttachmentRepository:IAttachmentRepository
{
    public IEnumerable<string> GetEmployeeAttachments(int employeeId)
    {
        var attachments = new List<string>();

        using var conn = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_GetEmployeeAttachments", conn);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@employeeId", employeeId);

        conn.Open();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            attachments.Add(reader["FilePath"].ToString());
        }

        return attachments;
    }
    public void AddAttachment(int employeeId, string filePath)
    {
      
        using var conn = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_AddEmployeeAttachment", conn);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@employeeId", employeeId);
        command.Parameters.AddWithValue("@filePath", filePath);

        conn.Open();

        command.ExecuteNonQuery();
        
    }
    public void DeleteAttachment(string filePath)
    {

            using var conn = DatabaseInitializer.GetConnection();
            using var command = new SqlCommand("SP_DeleteEmployeeAttachment", conn);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@filePath", filePath);

            conn.Open();
            command.ExecuteNonQuery();

    }
}
