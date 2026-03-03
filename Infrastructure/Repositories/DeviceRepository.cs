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

public class DeviceRepository : IDevcieRepository
{
    public List<DeviceInfoDTO> GetCustomerDevicesBy(int customerId)
    {
        var customerDevices = new List<DeviceInfoDTO>();

        // تم إضافة d.BrandID, d.TypeID, d.SpecID للاستعلام
        var script = @"
    SELECT 
        d.DeviceID,
        d.BrandID,  
        d.TypeID,   
        d.SpecID,   
        b.BrandName AS Brand, 
        t.TypeName AS Type, 
        s.SpecName AS Spec, 
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
        command.Parameters.AddWithValue("@customerId", customerId);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            customerDevices.Add(new DeviceInfoDTO
            {
                DeviceId = Convert.ToInt32(reader["DeviceID"]),

                BrandID = Convert.ToInt32(reader["BrandID"]),
                TypeID = Convert.ToInt32(reader["TypeID"]),
                SpecID = Convert.ToInt32(reader["SpecID"]),

                BrandName = reader["Brand"].ToString() ?? string.Empty,
                TypeName = reader["Type"]?.ToString() ?? string.Empty,
                SpecName = reader["Spec"]?.ToString() ?? string.Empty,

                Model = reader["ModelName"] == DBNull.Value ? null : reader["ModelName"].ToString(),
                SerialNumber = reader["SerialNumber"] == DBNull.Value ? null : reader["SerialNumber"].ToString(),
            });
        }

        return customerDevices;
    }
    public bool AddDeviceToCustomer(int customerId, Device device)
    {
        var script = @"insert into devices (customerid ,brandid , typeid , specid ,serialnumber , modelname) 
            values (@customerid , @brandid , @typeid , @specid , @serialnumber , @modelname)";

        using var conn = DatabaseInitializer.GetConnection();

        using var cmd = new SqlCommand(script, conn);

        cmd.Parameters.AddWithValue("customerid", customerId);
        cmd.Parameters.AddWithValue("brandid", device.BrandID);
        cmd.Parameters.AddWithValue("typeid", device.TypeID);
        cmd.Parameters.AddWithValue("specid", device.SpecID);
        cmd.Parameters.AddWithValue("serialnumber", device.SerialNumber is null ? (object)DBNull.Value : device.SerialNumber);
        cmd.Parameters.AddWithValue("modelname", device.ModelName is null? (object)DBNull.Value : device.ModelName);
        conn.Open();
        int affected = cmd.ExecuteNonQuery();
        return affected>0;

    }

    public bool DeleteCustomerDevice(int deviceId)
    {
        var script = @"
DELETE FROM Devices
WHERE DeviceID = @deviceId
AND (
    SELECT COUNT(*) 
    FROM Devices AS d
    WHERE d.CustomerID = Devices.CustomerID
) > 1;";

        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand(script, conn);
        cmd.Parameters.AddWithValue("@deviceId", deviceId);

        conn.Open();
        int affected = cmd.ExecuteNonQuery();

        return affected > 0;
    }
    public bool UpdateCustomerDevice(DeviceInfoDTO device)
    {
        var script = @"
        UPDATE Devices 
        SET BrandID = @brandId, 
            TypeID = @typeId, 
            SpecID = @specId, 
            ModelName = @model, 
            SerialNumber = @serial
        WHERE DeviceID = @deviceId";

        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand(script, conn);

        cmd.Parameters.AddWithValue("@brandId", device.BrandID);
        cmd.Parameters.AddWithValue("@typeId", device.TypeID);
        cmd.Parameters.AddWithValue("@specId", device.SpecID);
        cmd.Parameters.AddWithValue("@model", string.IsNullOrEmpty(device.Model) ? (object)DBNull.Value : device.Model);
        cmd.Parameters.AddWithValue("@serial", string.IsNullOrEmpty(device.SerialNumber) ? (object)DBNull.Value : device.SerialNumber);
        cmd.Parameters.AddWithValue("@deviceId", device.DeviceId);

        try
        {
            conn.Open();
            int affected = cmd.ExecuteNonQuery();
            return affected > 0; 
        }
        catch (Exception)
        {
            return false;
        }
    }
}
