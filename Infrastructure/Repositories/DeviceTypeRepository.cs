using Application.Common.Interfaces;
using Application.DTOs;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data;

public class DeviceTypeRepository:IDeviceTypeRepository
{
    public List<TypeDTO> GetAllTypes()
    {
        List<TypeDTO> types = new List<TypeDTO>();
        using var conn = DatabaseInitializer.GetConnection();

        string script = @"select * from Types ";
        using var cmd = new SqlCommand(script, conn);
        conn.Open();
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            types.Add(new TypeDTO(Convert.ToInt32(reader["TypeID"]), reader["TypeName"].ToString()));
        }

        return types;
    }

    public bool AddDeviceToCustomer(int customerId , Device device)
    {
        using var conn = DatabaseInitializer.GetConnection();

        var script = @"insert into Devices (CustomerID , BrandID , TypeID , SpecID , ModelName , SerialNumber)
                       values (@customerId , @brandId , @typeId , @specId , @modelName , @serialNumber)";
        using var cmd = new SqlCommand(script, conn);
        cmd.Parameters.AddWithValue("customerId", customerId);
        cmd.Parameters.AddWithValue("brandId", device.BrandID);
        cmd.Parameters.AddWithValue("typeId", device.TypeID);
        cmd.Parameters.AddWithValue("specId", device.SpecID);
        cmd.Parameters.AddWithValue("modelName", device.ModelName is null ? DBNull.Value : device.ModelName);
        cmd.Parameters.AddWithValue("serialNumber", device.SerialNumber is null ? DBNull.Value : device.SerialNumber);
        conn.Open();
        var affected =Convert.ToInt32(cmd.ExecuteNonQuery());

        return affected > 0;
    }
}
