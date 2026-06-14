using Application.DTOs.AppointmentDTOs;
using Application.Repositories;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    public int Create(Appointment appointment)
    {
        using var conn = DatabaseInitializer.GetConnection();

        using var cmd = new SqlCommand("SP_CreateAppointment", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@OrderId", appointment.OrderId);
        cmd.Parameters.AddWithValue("@TechnicianId", appointment.TechnicianId);

        cmd.Parameters.AddWithValue("@TechnicianAssistantId", appointment.TechnicianAssistantId ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@DriverId",
    appointment.DriverId.HasValue ? appointment.DriverId : DBNull.Value);
        cmd.Parameters.AddWithValue("@Notes", appointment.Notes ?? (object)DBNull.Value);

        cmd.Parameters.AddWithValue("@ScheduledDate", appointment.ScheduledDate);

        cmd.Parameters.AddWithValue("@VisitType", (byte)appointment.VisitType);

        conn.Open();
        var result = cmd.ExecuteScalar();

        return Convert.ToInt32(result);
    }

    public IEnumerable<AppointmentSummaryDto> GetAppointmentByOrderId(int orderId)
    {
        using var connection = DatabaseInitializer.GetConnection();
        connection.Open();

        using var command = new SqlCommand("SP_GetAppointmentsByOrderId", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@OrderId", orderId);

        var result = new List<AppointmentSummaryDto>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new AppointmentSummaryDto
            {
                AppointmentId = reader.GetInt32(reader.GetOrdinal("AppointmentId")),
                ScheduledDate = reader.GetDateTime(reader.GetOrdinal("ScheduledDate")),
                AppointmentState = reader.GetByte(reader.GetOrdinal("AppointmentState")),
                VisitType = reader.GetByte(reader.GetOrdinal("VisitType")),
                TechnicianName = reader.GetString(reader.GetOrdinal("TechnicianName")),
                AssistantName = reader.IsDBNull(reader.GetOrdinal("AssistantName"))  
                        ? null
                        : reader.GetString(reader.GetOrdinal("AssistantName")),

                DriverName = reader.IsDBNull(reader.GetOrdinal("DriverName"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("DriverName")),

                Notes = reader.IsDBNull(reader.GetOrdinal("Notes"))          
                        ? null
                        : reader.GetString(reader.GetOrdinal("Notes"))
            });
        }
        return result;
    }
    public static void UpdateOverdueAppointments()
    {
        using var connection = DatabaseInitializer.GetConnection();
        connection.Open();

        string sql = @"
        UPDATE Appointments
        SET AppointmentState = 1
        WHERE AppointmentState = 0
        AND ScheduledDate < CAST(GETDATE() AS DATE)";

        using var cmd = new SqlCommand(sql, connection);
        cmd.ExecuteNonQuery();
    }
    public bool Update(UpdateAppointmentDto dto)
    {
        using var conn = DatabaseInitializer.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand("SP_UpdateAppointment", conn)
        {
            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.AddWithValue("@AppointmentId", dto.AppointmentId);
        cmd.Parameters.AddWithValue("@TechnicianId", dto.TechnicianId);
        cmd.Parameters.AddWithValue("@TechnicianAssistantId", dto.TechnicianAssistantId.HasValue ? dto.TechnicianAssistantId : DBNull.Value);
        cmd.Parameters.AddWithValue("@DriverId", dto.DriverId.HasValue ? dto.DriverId : DBNull.Value);
        cmd.Parameters.AddWithValue("@ScheduledDate", dto.ScheduledDate);
        cmd.Parameters.AddWithValue("@VisitType", dto.VisitType);
        cmd.Parameters.AddWithValue("@Notes", dto.Notes ?? (object)DBNull.Value);

        return cmd.ExecuteNonQuery() > 0;
    }
    public AppointmentDetailsDto? GetById(int appointmentId)
    {
        using var conn = DatabaseInitializer.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand("SP_GetAppointmentById", conn)
        {
            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new AppointmentDetailsDto
            {
                AppointmentId = reader.GetInt32(reader.GetOrdinal("AppointmentId")),
                OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                ScheduledDate = reader.GetDateTime(reader.GetOrdinal("ScheduledDate")),
                VisitType = reader.GetByte(reader.GetOrdinal("VisitType")),
                AppointmentState = reader.GetByte(reader.GetOrdinal("AppointmentState")),
                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                TechnicianId = reader.GetInt32(reader.GetOrdinal("TechnicianId")),
                TechnicianAssistantId = reader.IsDBNull(reader.GetOrdinal("TechnicianAssistantId")) ? null : reader.GetInt32(reader.GetOrdinal("TechnicianAssistantId")),
                DriverId = reader.IsDBNull(reader.GetOrdinal("DriverId")) ? null : reader.GetInt32(reader.GetOrdinal("DriverId")),
            };
        }
        return null;
    }
    public bool Cancel(int appointmentId)
    {
        using var conn = DatabaseInitializer.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand("SP_CancelAppointment", conn)
        {
            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);

        int rows = cmd.ExecuteNonQuery();

        if (rows == 0)
            throw new InvalidOperationException("لا يمكن إلغاء موعد مكتمل أو ملغي بالفعل");

        return true;
    }
}
