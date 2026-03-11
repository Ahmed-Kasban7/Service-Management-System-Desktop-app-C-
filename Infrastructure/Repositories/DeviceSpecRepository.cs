using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data;

public class DeviceSpecRepository:IDeviceSpecRepository
{
    public List<SpecDto> GetSpecsByTypeId(int typeId)
    {
        List<SpecDto> specDTOs = new List<SpecDto>();
        using var conn = DatabaseInitializer.GetConnection();

        string script = @"select * from Specs where TypeID = @typeId ";
        using var cmd = new SqlCommand(script, conn);
        cmd.Parameters.AddWithValue("typeId", typeId);
        conn.Open();
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            specDTOs.Add(new SpecDto(Convert.ToInt32(reader["SpecID"]), reader["SpecName"].ToString(), Convert.ToInt32(reader["TypeID"])));
        }
        return specDTOs;
    }
}
