using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using BOM.Tool;
using BOM.Model;

namespace BOM.DataAccess
{
    public static partial class DataDB
    {

        public static void UpdateMaterialListForDummies(MaterialStuck material)
        {
            OpenConnection();
            try
            {
                using (IDbConnection dbConnection = connection)
                {
                    string originalCode = material.OriginalCode;
                    string code = material.Code;
                    string name = material.Name ?? "";
                    double total = material.Total;
                    string ktl = material.Ktl ?? "";
                    string description = material.Description ?? "";
                    string provider = material.ProviderName ?? "";
                    string location = material.Location ?? "";
                    string unit = material.Unit ?? "";
                    IDbCommand dbCommand = dbConnection.CreateCommand();
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = @"dbo.sproc_UpdateWarehouseMaterialForDummies";
                    dbCommand.Parameters.Add(new SqlParameter("@Code", code));
                    dbCommand.Parameters.Add(new SqlParameter("@OriginalCode", originalCode));
                    dbCommand.Parameters.Add(new SqlParameter("@Description", description));
                    dbCommand.Parameters.Add(new SqlParameter("@ProviderName", provider));
                    dbCommand.Parameters.Add(new SqlParameter("@Total", total));
                    dbCommand.Parameters.Add(new SqlParameter("@Unit", unit));
                    dbCommand.Parameters.Add(new SqlParameter("@Location", location));
                    dbCommand.Parameters.Add(new SqlParameter("@KLT", ktl));
                    dbCommand.Parameters.Add(new SqlParameter("@MaterialName", name));
                    dbCommand.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error SQL: {exc.Message}");
                connection.Close();
                throw new Exception("Get (EnabledProducts) error: " + exc.Message, exc);
            }
        }


        public static Order GetLastAssignedOrder()
        {
            OpenConnection();
            List<MaterialOrder> list = new List<MaterialOrder>();
            Order order = new Order();
            try
            {
                using (IDbConnection dbConnection = connection)
                {
                    IDbCommand dbCommand = dbConnection.CreateCommand();
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = @"dbo.sproc_GetLastAssignedOrder";
                    using (IDataReader reader = dbCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            order.ExcelPath = reader["ExcelPath"].ToString();
                            order.Id = ulong.Parse(reader["Id"].ToString());
                            MaterialOrder material = new MaterialOrder
                            {
                                OriginalCode = reader["OriginalCode"].ToString(),
                                Code = reader["Code"].ToString(),
                                Description = reader["Description"].ToString(),
                                ChosenAmount = double.Parse(reader["ChosenAmount"].ToString()),
                                Unit = reader["Unit"].ToString()
                            };
                            list.Add(material);
                        }
                    }
                }
                order.MaterialList = list;
                connection.Close();
                return order;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error SQL: {exc.Message}");
                throw new Exception("Get (EnabledProducts) error: " + exc.Message, exc);
            }
        }

        public static void InsertMaterialListAvila(List<MaterialStuck> materialList)
        {
            OpenConnection();
            try
            {
                using (IDbConnection dbConnection = connection)
                {
                    foreach (MaterialStuck material in materialList)
                    {
                        string code = material.Code;
                        string originalCode = material.OriginalCode;
                        double total = material.Total;
                        //string ktl = material.Ktl == null ? "" : material.Ktl;
                        string ktl = material.Ktl;
                        string provider = material.Provider == null ? "" : material.Provider;
                        string location = "Material Neumático (Avila)";
                        string unit = material.Unit == null ? "" : material.Unit;
                        string description = material.Description == null ? "" : material.Description;
                        IDbCommand dbCommand = dbConnection.CreateCommand();
                        dbCommand.CommandType = CommandType.StoredProcedure;
                        dbCommand.CommandText = @"dbo.sproc_InsertMaterialAvila";
                        //dbCommand.CommandText = @"dbo.sproc_Recuperation";
                        dbCommand.Parameters.Add(new SqlParameter("@Code", code));
                        dbCommand.Parameters.Add(new SqlParameter("@OriginalCode", originalCode));
                        dbCommand.Parameters.Add(new SqlParameter("@Total", total));
                        dbCommand.Parameters.Add(new SqlParameter("@KTL", ktl));
                        dbCommand.Parameters.Add(new SqlParameter("@ProviderName", provider));
                        dbCommand.Parameters.Add(new SqlParameter("@Location", location));
                        dbCommand.Parameters.Add(new SqlParameter("@Unit", unit));
                        dbCommand.Parameters.Add(new SqlParameter("@Description", description));
                        dbCommand.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error SQL: {exc.Message}");
                connection.Close();
                throw new Exception("Get (EnabledProducts) error: " + exc.Message, exc);
            }
        }

        public static Order GetOrderById(ulong orderId)
        {
            OpenConnection();
            List<MaterialOrder> list = new List<MaterialOrder>();
            Order order = new Order();
            try
            {
                using (IDbConnection dbConnection = connection)
                {
                    IDbCommand dbCommand = dbConnection.CreateCommand();
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = @"dbo.sproc_GetOrderById";
                    dbCommand.Parameters.Add(new SqlParameter("@OrderId", orderId.ToString()));
                    using (IDataReader reader = dbCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            order.ExcelPath = reader["ExcelPath"].ToString();
                            order.Id = ulong.Parse(reader["Id"].ToString());
                            MaterialOrder material = new MaterialOrder
                            {
                                OriginalCode = reader["OriginalCode"].ToString(),
                                Code = reader["Code"].ToString(),
                                Description = reader["Description"].ToString(),
                                ChosenAmount = double.Parse(reader["ChosenAmount"].ToString()),
                                Unit = reader["Unit"].ToString(),
                                OriginName = reader["OriginName"].ToString(),
                                Ktl = reader["KLT"].ToString(),
                                Location = reader["Location"].ToString(),
                                ProviderName = reader["ProviderName"].ToString()
                            };

                            list.Add(material);
                        }
                    }
                }
                order.MaterialList = list;
                connection.Close();
                return order;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error SQL: {exc.Message}");
                throw new Exception("Get (EnabledProducts) error: " + exc.Message, exc);
            }
        }



        public static List<string> GetUnits()
        {
            OpenConnection();
            List<string> suppliersList = new List<string>();
            var dataSet = new DataSet();
            try
            {
                using (IDbConnection dbConnection = connection)
                {
                    IDbCommand dbCommand = dbConnection.CreateCommand();
                    var select = $"SELECT [Name] FROM Enum_Unit;";
                    dbCommand.CommandText = select;

                    var c = new SqlConnection(connetionString);
                    var dataAdapter = new SqlDataAdapter(select, c);

                    var commandBuilder = new SqlCommandBuilder(dataAdapter);

                    using (IDataReader reader = dbCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            suppliersList.Add(reader["Name"].ToString());
                        }
                    }
                }
                connection.Close();
                return suppliersList;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error SQL: {exc.Message}");
                connection.Close();
                throw new Exception("Get (EnabledProducts) error: " + exc.Message, exc);
            }

        }
        
        public static List<string> GetSuppliers()
        {
            OpenConnection();
            List<string> suppliersList = new List<string>();
            var dataSet = new DataSet();
            try
            {
                using (IDbConnection dbConnection = connection)
                {
                    IDbCommand dbCommand = dbConnection.CreateCommand();
                    var select = $"SELECT [Name] FROM Enum_Supplier;";
                    dbCommand.CommandText = select;

                    var c = new SqlConnection(connetionString);
                    var dataAdapter = new SqlDataAdapter(select, c);

                    var commandBuilder = new SqlCommandBuilder(dataAdapter);

                    using (IDataReader reader = dbCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            suppliersList.Add(reader["Name"].ToString());
                        }
                    }
                }
                connection.Close();
                return suppliersList;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error SQL: {exc.Message}");
                connection.Close();
                throw new Exception("Get (EnabledProducts) error: " + exc.Message, exc);
            }

        }

    }
}
