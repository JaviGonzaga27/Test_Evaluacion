using System;
using System.Data.SqlClient;
using TechTalk.SpecFlow;
using EvaluacionGonzagaJavier.Models;
using TechTalk.SpecFlow.Assist;
using System.Linq;
using FluentAssertions;

namespace SpectFlowTest.StepDefinitions
{
    [Binding]
    public class IngresoStepDefinitions
    {
        private readonly string connectionString = "Server=JAVI;Database=dbEvaluacion;User Id=sa;Password=sa;";
        private Order _order;

        [Given(@"Seleccionar los Datos para ingresar en la BDD")]
        public void GivenSeleccionarLosDatosParaIngresarEnLaBDD(Table table)
        {
            table.Rows.Should().HaveCountGreaterThanOrEqualTo(1, "La tabla debe contener al menos una fila de datos.");
            _order = table.CreateInstance<Order>();
        }

        [When(@"Calculamos los totales")]
        public void WhenCalculamosLosTotales()
        {
            _order.Should().NotBeNull("El objeto Order no debería ser nulo en este punto.");
            _order.CalculateTotals();
        }

        [Then(@"Los totales calculados son correctos")]
        public void ThenLosTotalesCalculadosSonCorrectos(Table table)
        {
            var expectedTotals = table.CreateInstance<Order>();

            _order.Subtotal.Should().Be(expectedTotals.Subtotal, "El subtotal calculado no es correcto");
            _order.DiscountAmount.Should().Be(expectedTotals.DiscountAmount, "El monto de descuento calculado no es correcto");
            _order.Total.Should().Be(expectedTotals.Total, "El total calculado no es correcto");
        }

        [When(@"Ingresamos el registro a la BDD")]
        public void WhenIngresamosElRegistroALaBDD()
        {
            _order.Should().NotBeNull("El objeto Order no debería ser nulo en este punto.");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "EXEC sp_AddOrder @ProductName, @UnitPrice, @Quantity, @Discount, @Subtotal, @DiscountAmount, @Total";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", _order.ProductName);
                    command.Parameters.AddWithValue("@UnitPrice", _order.UnitPrice);
                    command.Parameters.AddWithValue("@Quantity", _order.Quantity);
                    command.Parameters.AddWithValue("@Discount", _order.Discount);
                    command.Parameters.AddWithValue("@Subtotal", _order.Subtotal);
                    command.Parameters.AddWithValue("@DiscountAmount", _order.DiscountAmount);
                    command.Parameters.AddWithValue("@Total", _order.Total);

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception($"Error al ejecutar el procedimiento almacenado: {ex.Message}", ex);
                    }
                }
            }
        }

        [Then(@"Resultado del registro ingresado a la BDD")]
        public void ThenResultadoDelRegistroIngresadoALaBDD(Table table)
        {
            table.Rows.Should().HaveCountGreaterThanOrEqualTo(1, "La tabla de resultados debe contener al menos una fila.");

            var expectedOrder = table.CreateInstance<Order>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT TOP 1 * FROM Orders WHERE ProductName = @ProductName ORDER BY OrderId DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", expectedOrder.ProductName);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read().Should().BeTrue("No se encontró el registro insertado.");

                        var actualOrder = new Order
                        {
                            ProductName = reader["ProductName"].ToString(),
                            UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Discount = Convert.ToDecimal(reader["Discount"]),
                            Subtotal = Convert.ToDecimal(reader["Subtotal"]),
                            DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]),
                            Total = Convert.ToDecimal(reader["Total"])
                        };

                        actualOrder.Should().BeEquivalentTo(expectedOrder, "El registro insertado no coincide con el esperado.");
                    }
                }
            }
        }

        [Given(@"Existe una orden en la BDD")]
        public void GivenExisteUnaOrdenEnLaBDD(Table table)
        {
            _order = table.CreateInstance<Order>();
            _order.CalculateTotals();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "EXEC sp_AddOrder @ProductName, @UnitPrice, @Quantity, @Discount, @Subtotal, @DiscountAmount, @Total";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", _order.ProductName);
                    command.Parameters.AddWithValue("@UnitPrice", _order.UnitPrice);
                    command.Parameters.AddWithValue("@Quantity", _order.Quantity);
                    command.Parameters.AddWithValue("@Discount", _order.Discount);
                    command.Parameters.AddWithValue("@Subtotal", _order.Subtotal);
                    command.Parameters.AddWithValue("@DiscountAmount", _order.DiscountAmount);
                    command.Parameters.AddWithValue("@Total", _order.Total);
                    command.ExecuteNonQuery();
                }
            }
        }

        [When(@"Editamos la orden")]
        public void WhenEditamosLaOrden(Table table)
        {
            var updatedOrder = table.CreateInstance<Order>();
            updatedOrder.CalculateTotals();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "EXEC sp_UpdateOrder @ProductName, @UnitPrice, @Quantity, @Discount, @Subtotal, @DiscountAmount, @Total";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", updatedOrder.ProductName);
                    command.Parameters.AddWithValue("@UnitPrice", (decimal)updatedOrder.UnitPrice);
                    command.Parameters.AddWithValue("@Quantity", (int)updatedOrder.Quantity);
                    command.Parameters.AddWithValue("@Discount", (decimal)updatedOrder.Discount);
                    command.Parameters.AddWithValue("@Subtotal", (decimal)updatedOrder.Subtotal);
                    command.Parameters.AddWithValue("@DiscountAmount", (decimal)updatedOrder.DiscountAmount);
                    command.Parameters.AddWithValue("@Total", (decimal)updatedOrder.Total);

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"Error SQL: {ex.Message}");
                        throw;
                    }
                }
            }

            _order = updatedOrder;
        }

        [Then(@"La orden editada se refleja en la BDD")]
        public void ThenLaOrdenEditadaSeReflejaEnLaBDD(Table table)
        {
            ThenResultadoDelRegistroIngresadoALaBDD(table);
        }

        [When(@"Eliminamos la orden")]
        public void WhenEliminamosLaOrden()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "EXEC sp_DeleteOrder @ProductName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", _order.ProductName);
                    command.ExecuteNonQuery();
                }
            }
        }

        [Then(@"La orden no existe en la BDD")]
        public void ThenLaOrdenNoExisteEnLaBDD()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Orders WHERE ProductName = @ProductName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", _order.ProductName);
                    int count = (int)command.ExecuteScalar();
                    count.Should().Be(0, "La orden debería haber sido eliminada");
                }
            }
        }

        [When(@"Leemos la orden")]
        public void WhenLeemosLaOrden()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT TOP 1 * FROM Orders WHERE ProductName = @ProductName ORDER BY OrderId DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", _order.ProductName);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read().Should().BeTrue("No se encontró el registro.");

                        _order = new Order
                        {
                            ProductName = reader["ProductName"].ToString(),
                            UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Discount = Convert.ToDecimal(reader["Discount"]),
                            Subtotal = Convert.ToDecimal(reader["Subtotal"]),
                            DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]),
                            Total = Convert.ToDecimal(reader["Total"])
                        };
                    }
                }
            }
        }

        [Then(@"Los detalles de la orden son correctos")]
        public void ThenLosDetallesDeLaOrdenSonCorrectos(Table table)
        {
            var expectedOrder = table.CreateInstance<Order>();
            _order.Should().BeEquivalentTo(expectedOrder, "Los detalles leídos de la orden no coinciden con los esperados.");
        }
    }
}