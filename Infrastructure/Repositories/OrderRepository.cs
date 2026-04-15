using Application.Repositories;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        cmd.Parameters.AddWithValue("@OrderState", (int)order.OrderState);

        conn.Open();

        cmd.ExecuteNonQuery();

        return order.Id;
    }

    public Order Get(int id)
    {

        using var conn = DatabaseInitializer.GetConnection();

        using var cmd = new SqlCommand("SP_GetOrder", conn);
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
                reader["EndDate"] as DateTime?, 
                reader["Problem"].ToString() ?? string.Empty,
                reader["Notes"]?.ToString(),
                (int)reader["CustomerId"],
                (int)reader["DeviceId"],
                (EOrderState)Convert.ToByte(reader["OrderState"])
            );
        }

        return null;
    }
}
