using Application.Common;
using Application.DTOs.CompanySettingsDTOs;
using Application.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    public CompanySettingsDto GetSettings()
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_GetCompanySettings", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        conn.Open();
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new CompanySettingsDto
            {
                CompanyName = reader["CompanyName"].ToString(),
                CompanyLogo = reader["CompanyLogo"] == DBNull.Value ? null : (byte[])reader["CompanyLogo"]
            };
        }

        return new CompanySettingsDto { CompanyName = "Pro Fix Company" };
    }

    public void UpdateSettings(CompanySettingsDto dto)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_UpdateCompanySettings", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@CompanyName", dto.CompanyName);
        cmd.Parameters.AddWithValue("@CompanyLogo", dto.CompanyLogo != null ? dto.CompanyLogo : DBNull.Value);

        conn.Open();
        cmd.ExecuteNonQuery();
    }
}