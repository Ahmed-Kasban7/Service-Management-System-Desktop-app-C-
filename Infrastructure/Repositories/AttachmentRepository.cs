using Application.DTOs.EmployeeDTOs;
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
    public IEnumerable<EmployeeAttachmentDto> GetEmployeeAttachments(int employeeId)
    {
        var attachments = new List<EmployeeAttachmentDto>();

        using var conn = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_GetEmployeeAttachments", conn);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@EmployeeId", employeeId);

        conn.Open();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            attachments.Add(new EmployeeAttachmentDto
            {
                Id = Convert.ToInt32(reader["Id"]),
                AttachmentData = (byte[])reader["AttachmentData"]
            });
        }

        return attachments;
    }
    public void AddAttachment(int employeeId, byte[] attachmentData)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_AddEmployeeAttachment", conn);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@EmployeeId", employeeId);
        command.Parameters.AddWithValue("@AttachmentData", attachmentData);

        conn.Open();
        command.ExecuteNonQuery();
    }
    public void DeleteAttachment(int attachmentId)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_DeleteEmployeeAttachment", conn);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@AttachmentId", attachmentId);

        conn.Open();
        command.ExecuteNonQuery();
    }
}
