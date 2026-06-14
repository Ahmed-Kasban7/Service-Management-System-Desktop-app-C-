using Application.Common;
using Application.DTOs.DeviceDTOs;
using Application.DTOs.OrderDTOs;
using Application.Repositories;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace Infrastructure.Repositories;

public class OrderRepository:IOrderRepository
{
    public int Create(Order order)
    {

        using var conn = DatabaseInitializer.GetConnection();

        using var cmd = new SqlCommand("SP_CreateOrder", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@StartDate",order.StartDate);
        cmd.Parameters.AddWithValue("@Problem", order.Problem);
        cmd.Parameters.AddWithValue("@Notes", order.Notes?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@CustomerID",order.CustomerId);
        cmd.Parameters.AddWithValue("@DeviceID", order.DeviceId);
        cmd.Parameters.AddWithValue("@OrderState", (int) order.OrderState);

        var orderId = new SqlParameter("@OrderId", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };

        cmd.Parameters.Add(orderId);

        conn.Open();

        cmd.ExecuteNonQuery();

        return (int)orderId.Value;

    }

    public int Update(Order order)
    {
        using var conn = DatabaseInitializer.GetConnection();

        using var cmd = new SqlCommand("SP_UpdateOrder", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@OrderId", order.Id);
        cmd.Parameters.AddWithValue("@Problem", order.Problem);
        cmd.Parameters.AddWithValue("@Notes", order.Notes ?? (object)DBNull.Value);
        conn.Open();

        cmd.ExecuteNonQuery();

        return order.Id;
    }

    public Order Get(int id)
    {

        using var conn = DatabaseInitializer.GetConnection();

        using var cmd = new SqlCommand("SP_GetOrderById", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@OrderId", id);

        conn.Open();

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Order(
                (int)reader["OrderId"],
                reader["OrderNumber"].ToString() ?? string.Empty,
                (DateTime)reader["StartDate"],
                reader["EndedDate"] as DateTime?, 
                reader["Problem"].ToString() ?? string.Empty,
                reader["Notes"]?.ToString(),
                (int)reader["CustomerId"],
                (int)reader["DeviceId"],
                (EOrderState)Convert.ToByte(reader["state"])
            );
        }

        return null;
    }

    public bool IsOrderExists(int orderId)
    {
        return Get(orderId) != null;
    }

    public PagedResult<OrderSummaryDto> GetPagedOrderSummaries(int pageNumber, int pageSize)
    {
        var orders = new List<OrderSummaryDto>();
        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_GetPagedOrderSummaries", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
        cmd.Parameters.AddWithValue("@PageSize", pageSize);

        var totalParam = new SqlParameter("@TotalOrderCount", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        cmd.Parameters.Add(totalParam);

        conn.Open();

        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                orders.Add(new OrderSummaryDto(
                   Convert.ToInt32( reader["OrderID"]),
                    reader["OrderNumber"].ToString(),
                    reader["CustomerName"].ToString(),
                    reader["CustomerPhone"].ToString(),
                    reader["Address"].ToString(),
                    (DateTime)reader["StartDate"],
                    reader["state"].ToString())
                );
            }
        } 

        int totalCount = totalParam.Value != DBNull.Value ? (int)totalParam.Value : 0;

        return new PagedResult<OrderSummaryDto>(orders, totalCount, pageNumber, pageSize);
    }

    public int GetOrderCount()
    {
        using var conn = DatabaseInitializer.GetConnection();

        using var cmd = new SqlCommand("SP_GetOrderCount", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        conn.Open();

        int reader =(int)cmd.ExecuteScalar();

        return reader;
    }

    public OrderDetailsDto GetOrderFullDetailsById(int orderId)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_GetOrderFullDetailsById", conn);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@OrderID", orderId);

        conn.Open();

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            var device = new DeviceSummaryDto(
                Id : Convert.ToInt32( reader["DeviceId"]) ,    
                BrandName: reader["Brand"]?.ToString(),
                TypeName: reader["Type"]?.ToString(),
                SpecName: reader["Spec"]?.ToString(),
                Model: reader["ModelName"]?.ToString(),
                SerialNumber: reader["SerialNumber"]?.ToString()
            );

            return new OrderDetailsDto(
     OrderId: reader["OrderID"] != DBNull.Value ? (int)reader["OrderID"] : 0,
     OrderNumber: reader["OrderNumber"]?.ToString(),
     Problem: reader["Problem"]?.ToString(),
     Notes: reader["Notes"] == DBNull.Value ? null : reader["Notes"]?.ToString(),
     StartDate: reader["StartDate"] != DBNull.Value
         ? (DateTime)reader["StartDate"]
         : DateTime.MinValue,
     EndDate: reader["EndedDate"] == DBNull.Value
         ? null
         : (DateTime?)reader["EndedDate"],
     OrderState: reader["OrderState"] != DBNull.Value      
         ? Convert.ToByte(reader["OrderState"])
         : (byte)0,
     State: reader["State"]?.ToString(),                     
     CustomerName: reader["Name"]?.ToString(),
     Address: reader["Address"]?.ToString(),
     CustomerPhones: reader["PhoneNumbers"]?.ToString() ?? "",
     CustomerDevice: device
 );
        }

        return null;
    }

    public PagedResult<OrderSummaryDto> SearchOrderPage(string searchWord, int pageNumber, int pageSize)
    {
        var orders = new List<OrderSummaryDto>();
        int totalCount = 0;

        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_SearchOrderPage", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@SearchWord", searchWord);
        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
        cmd.Parameters.AddWithValue("@PageSize", pageSize);

        conn.Open();

        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                if (totalCount == 0 && reader["TotalCount"] != DBNull.Value)
                {
                    totalCount = Convert.ToInt32(reader["TotalCount"]);
                }

                orders.Add(new OrderSummaryDto(
                    Convert.ToInt32(reader["OrderID"]),
                    reader["OrderNumber"].ToString(),
                    reader["CustomerName"].ToString(),
                    reader["CustomerPhone"].ToString(),
                    reader["Address"].ToString(),
                    (DateTime)reader["StartDate"],
                    reader["State"].ToString()
                ));
            }
        }

        return new PagedResult<OrderSummaryDto>(orders, totalCount, pageNumber, pageSize);
    }
    public IEnumerable<DeviceOrderHistoryDto> GetOrdersByDeviceId(int deviceId)
    {
        var orders = new List<DeviceOrderHistoryDto>();

        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_GetDeviceOrdersHistory", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@DeviceId", deviceId);

        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            orders.Add(new DeviceOrderHistoryDto
            (
                orderId: (int)reader["OrderID"],
                orderNumber: reader["OrderNumber"]?.ToString() ?? "---",
                problem: reader["Problem"] == DBNull.Value ? null : reader["Problem"].ToString(),
                startDate: (DateTime)reader["StartDate"],
                state: reader["State"]?.ToString() ?? "---",
                orderState: reader["OrderState"] == DBNull.Value ? 0 : (byte)reader["OrderState"]));
        }

        return orders;
    }

    public IEnumerable<CustomerOrderSummaryDto> GetCustomerOrders(int customerId)
    {
        var orders = new List<CustomerOrderSummaryDto>();

        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_GetCustomerOrders", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CustomerId", customerId);

        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            orders.Add(new CustomerOrderSummaryDto
            {
                OrderId = (int)reader["OrderID"],
                OrderNumber = reader["OrderNumber"].ToString(),
                StartDate = (DateTime)reader["StartDate"],
                State = reader["State"].ToString(),
                OrderState = Convert.ToByte(reader["OrderState"]),
                DeviceName = reader["DeviceName"]?.ToString()
            });
        }

        return orders;
    }
}
