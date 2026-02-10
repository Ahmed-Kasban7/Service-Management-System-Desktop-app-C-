using Application.DTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data;

public class DeviceRepository
{
    public List<DeviceDTO> GetCustomerDevicesBy(int customerId)
    {
        var customerDevices = new List<DeviceDTO>();

        var script = @"
        SELECT 
            b.BrandName AS Brand, 
            t.TypeName AS Type, 
            s.SpecsName AS Spec, 
            d.ModelName, 
            d.SerialNumber
        FROM Devices d
        LEFT JOIN Brands b ON d.BrandID = b.BrandID
        LEFT JOIN Types t ON d.TypeID = t.TypeID
        LEFT JOIN Specs s ON d.SpecID = s.SpecID
        WHERE d.CustomerID = @customerId";

        using var conn = DatabaseInitializer.GetConnection();
        conn.Open();
        using var command = new SqlCommand(script, conn);
        command.Parameters.AddWithValue("customerId", customerId);
        using var reader =command.ExecuteReader();

        
            while (reader.Read())
            {
                customerDevices.Add(new DeviceDTO
                {
                    Brand = reader["Brand"]?.ToString() ?? string.Empty,
                    Type = reader["Type"]?.ToString() ?? string.Empty,
                    Spec = reader["Spec"]?.ToString() ?? string.Empty,
                    Model = reader["ModelName"]?.ToString() ?? "غير معين",
                    SerialNumber = reader["SerialNumber"]?.ToString() ?? "غير معين"
                });
            }
        
        return customerDevices;

    }

}
