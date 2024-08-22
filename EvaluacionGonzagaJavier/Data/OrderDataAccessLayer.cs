using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using EvaluacionGonzagaJavier.Models;

namespace EvaluacionGonzagaJavier.Data
{
    public class OrderDataAccessLayer
    {
        string connectionString = "Server=JAVI;Database=dbEvaluacion;User Id=sa;Password=sa;";

        public IEnumerable<Order> GetAllOrders()
        {
            List<Order> list = new List<Order>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllOrders", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Order order = new Order
                    {
                        OrderId = Convert.ToInt32(rdr["OrderId"]),
                        ProductName = rdr["ProductName"].ToString(),
                        UnitPrice = Convert.ToDecimal(rdr["UnitPrice"]),
                        Quantity = Convert.ToInt32(rdr["Quantity"]),
                        Discount = Convert.ToDecimal(rdr["Discount"]),
                        Subtotal = Convert.ToDecimal(rdr["Subtotal"]),
                        DiscountAmount = Convert.ToDecimal(rdr["DiscountAmount"]),
                        Total = Convert.ToDecimal(rdr["Total"])
                    };

                    list.Add(order);
                }
                con.Close();
            }
            return list;
        }

        public void AddOrder(Order order)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_AddOrder", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProductName", order.ProductName);
                cmd.Parameters.AddWithValue("@UnitPrice", order.UnitPrice);
                cmd.Parameters.AddWithValue("@Quantity", order.Quantity);
                cmd.Parameters.AddWithValue("@Discount", order.Discount);

                order.CalculateTotals();
                cmd.Parameters.AddWithValue("@Subtotal", order.Subtotal);
                cmd.Parameters.AddWithValue("@DiscountAmount", order.DiscountAmount);
                cmd.Parameters.AddWithValue("@Total", order.Total);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void UpdateOrder(Order order)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateOrder", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderId", order.OrderId);
                cmd.Parameters.AddWithValue("@ProductName", order.ProductName);
                cmd.Parameters.AddWithValue("@UnitPrice", order.UnitPrice);
                cmd.Parameters.AddWithValue("@Quantity", order.Quantity);
                cmd.Parameters.AddWithValue("@Discount", order.Discount);

                order.CalculateTotals();
                cmd.Parameters.AddWithValue("@Subtotal", order.Subtotal);
                cmd.Parameters.AddWithValue("@DiscountAmount", order.DiscountAmount);
                cmd.Parameters.AddWithValue("@Total", order.Total);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public Order GetOrderData(int? id)
        {
            Order order = new Order();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetOrderData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderId", id);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    order.OrderId = Convert.ToInt32(rdr["OrderId"]);
                    order.ProductName = rdr["ProductName"].ToString();
                    order.UnitPrice = Convert.ToDecimal(rdr["UnitPrice"]);
                    order.Quantity = Convert.ToInt32(rdr["Quantity"]);
                    order.Discount = Convert.ToDecimal(rdr["Discount"]);
                    order.Subtotal = Convert.ToDecimal(rdr["Subtotal"]);
                    order.DiscountAmount = Convert.ToDecimal(rdr["DiscountAmount"]);
                    order.Total = Convert.ToDecimal(rdr["Total"]);
                }
            }
            return order;
        }

        public void DeleteOrder(int? id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteOrder", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderId", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}