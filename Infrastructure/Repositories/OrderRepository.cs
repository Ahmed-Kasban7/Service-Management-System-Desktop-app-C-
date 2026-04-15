using Application.Repositories;
using Domain.Entities;
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
}
