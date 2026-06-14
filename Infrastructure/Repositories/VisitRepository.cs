using System;
using System.Collections.Generic;
using System.Data;
using Application.DTOs.SparePartDtos;
using Microsoft.Data.SqlClient; // أو System.Data.SqlClient حسب مكتبتك

namespace Infrastructure.Repositories
{
    public class VisitRepository 
    {
 
        public bool CreateVisitWithParts(int appointmentId, string notes, string actionsTaken, string diagnosis, decimal totalCost, List<SparePartDTO> spareParts)
        {
            DataTable partsDataTable = CreateSparePartsDataTable(spareParts);

            using (var connection = DatabaseInitializer.GetConnection())
            {
                using (var command = new SqlCommand("sp_CreateVisitWithParts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = appointmentId;
                    command.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = (object)notes ?? DBNull.Value;
                    command.Parameters.Add("@ActionsTaken", SqlDbType.NVarChar).Value = (object)actionsTaken ?? DBNull.Value;
                    command.Parameters.Add("@Diagnosis", SqlDbType.NVarChar).Value = (object)diagnosis ?? DBNull.Value;
                    command.Parameters.Add("@TotalCost", SqlDbType.Decimal).Value = totalCost;

                    SqlParameter tvpParameter = command.Parameters.AddWithValue("@SparePartsList", partsDataTable);
                    tvpParameter.SqlDbType = SqlDbType.Structured;
                    tvpParameter.TypeName = "dbo.UsedSparePartsType"; // نفس الاسم اللي سميناه في الـ SQL Type

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("حدث خطأ أثناء حفظ تقرير الزيارة في قاعدة البيانات.", ex);
                    }
                }
            }
        }

        private DataTable CreateSparePartsDataTable(List<SparePartDTO> spareParts)
        {
            DataTable table = new DataTable();

            table.Columns.Add("PartName", typeof(string));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("UnitPrice", typeof(decimal));

            if (spareParts != null && spareParts.Count > 0)
            {
                foreach (var part in spareParts)
                {
                    if (!string.IsNullOrWhiteSpace(part.PartName))
                    {
                        table.Rows.Add(part.PartName, part.Quantity, part.UnitPrice);
                    }
                }
            }

            return table;
        }
    }
}