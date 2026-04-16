using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data;

public class DeviceRepository : IDeviceRepository
{
    public List<DeviceInfoDTO> GetCustomerDevicesBy(int customerId)
    {
        var customerDevices = new List<DeviceInfoDTO>();


        using var conn = DatabaseInitializer.GetConnection();
        conn.Open();
        using var command = new SqlCommand("SP_GetCustomerDevices", conn);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@customerId", customerId);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            customerDevices.Add(new DeviceInfoDTO
            (
               Convert.ToInt32(reader["DeviceID"]),
                reader["Brand"].ToString() ?? string.Empty,

                Convert.ToInt32(reader["BrandID"]),
                                 reader["Type"]?.ToString() ?? string.Empty,

                 Convert.ToInt32(reader["TypeID"]),
                                  reader["Spec"]?.ToString() ?? string.Empty,

                Convert.ToInt32(reader["SpecID"]),


                reader["ModelName"] == DBNull.Value ? null : reader["ModelName"].ToString(),
               reader["SerialNumber"] == DBNull.Value ? null : reader["SerialNumber"].ToString()
            ));
        }

        return customerDevices;
    }
    public bool AddDeviceToCustomer(int customerId, Device device)
    {
        using var conn = DatabaseInitializer.GetConnection();

        using var cmd = new SqlCommand("SP_CreateDevice", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("customerid", customerId);
        cmd.Parameters.AddWithValue("brandid", device.BrandID);
        cmd.Parameters.AddWithValue("typeid", device.TypeID);
        cmd.Parameters.AddWithValue("specid", device.SpecID);
        cmd.Parameters.AddWithValue("serialnumber", device.SerialNumber ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("modelname", device.ModelName ?? (object)DBNull.Value);

        conn.Open();
        int affected = cmd.ExecuteNonQuery();

        return affected>0;

    }

    public bool DeleteCustomerDevice(int deviceId)
    {

        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_DeleteCustomerDevice", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@deviceId", deviceId);

        conn.Open();
        int affected = cmd.ExecuteNonQuery();

        return affected > 0;
    }
    public bool UpdateCustomerDevice(DeviceInfoDTO device)
    {

        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_UpdateDevice", conn);
        cmd.CommandType = CommandType.StoredProcedure;
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

    public bool IsDeviceExist(int deviceId)
    {
        using var conn = DatabaseInitializer.GetConnection();

        using var cmd = new SqlCommand("SP_IsDeviceExist", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@deviceId", deviceId);
        conn.Open();

        var res = (int)cmd.ExecuteScalar();

        return res == 1;
    }

    public bool IsDeviceAssignedToCustomer(int deviceId , int customerId )
    {
        using var conn = DatabaseInitializer.GetConnection();

        using var cmd = new SqlCommand("SP_IsDeviceAssignedToCustomer", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@deviceId", deviceId);
        cmd.Parameters.AddWithValue("@customerId", customerId);
        conn.Open();

        var res = (int)cmd.ExecuteScalar();

        return res == 1;
    }

}
