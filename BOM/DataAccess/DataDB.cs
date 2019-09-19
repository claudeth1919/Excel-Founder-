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

        private static SqlConnection connection;
        private static readonly string SplitOn = ";";
        private static string connetionString;
        private static void OpenConnection()
        {
            connetionString = $"Server={Defs.DB_HOSTNAME};Database={Defs.DB_DATABASE};User Id={Defs.DB_USER};Password={Defs.DB_PASSWORD}";
            //connetionString = @"Server= mx020038; Database= BOM; Integrated Security=True;";
            //connetionString = @"Server=localhost\smb; Database=BOM; Integrated Security=True;";
            //connetionString = @"Server=localhost\SQLEXPRESS; Database=BOM; Integrated Security=True;";
            //connetionString = @"Server=P02024375\smb;Database=MEMON;User Id=AIS_ALDS;Password=23erghiop0";

            connection = new SqlConnection(connetionString);
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                Util.ShowMessage(AlarmType.ERROR, "Por favor cheque que este conectado al internet y vuelva a intentarlo");
            }
        }

        public static List<Order> GetPendingOrders()
        {
            OpenConnection();
            List<Order> orderList = new List<Order>();
            try
            {
                using (IDbConnection dbConnection = connection)
                {
                    IDbCommand dbCommand = dbConnection.CreateCommand();
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = @"dbo.sproc_GetPendingOrders";
                    using (IDataReader reader = dbCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ulong OrderId = ulong.Parse(reader["Id"].ToString());
                            //string OrderId = reader["WhenStarted"].ToString();
                            if (OrderId !=0)
                            {
                                Order order = new Model.Order();
                                order.Id = OrderId;
                                orderList.Add(order);
                            }
                        }
                    }
                }
                connection.Close();
                return orderList;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error SQL: {exc.Message}");
                throw new Exception("Get (EnabledProducts) error: " + exc.Message, exc);
            }
        }

        public static List<Order> GetassignedOrders()
        {
            OpenConnection();
            List<Order> orderList = new List<Order>();
            try
            {
                using (IDbConnection dbConnection = connection)
                {
                    IDbCommand dbCommand = dbConnection.CreateCommand();
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = @"dbo.sproc_GetAssignedOrders";
                    using (IDataReader reader = dbCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ulong OrderId = ulong.Parse(reader["Id"].ToString());
                            //string OrderId = reader["WhenStarted"].ToString();
                            if (OrderId != 0)
                            {
                                Order order = new Model.Order();
                                order.Id = OrderId;
                                order.ExcelPath = reader["ExcelPath"].ToString();
                                orderList.Add(order);
                            }
                        }
                    }
                }
                connection.Close();
                return orderList;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error SQL: {exc.Message}");
                throw new Exception("Get (EnabledProducts) error: " + exc.Message, exc);
            }
        }

        public static Order UpdateAssignOrder(ulong orderId, List<MaterialOrder> changedList)
        {
            OpenConnection();
            List<MaterialOrder> list = new List<MaterialOrder>();
            Order order = new Order();
            Dictionary<string, string> concatDictionary = Util.ConcatMaterialList(SplitOn, changedList);
            string WarehouseIds = concatDictionary[Defs.COL_WATERHOUSE_IDS];
            string MaterialAmounts = concatDictionary[Defs.COL_TOTAL];
            try
            {
                using (IDbConnection dbConnection = connection)
                {
                    IDbCommand dbCommand = dbConnection.CreateCommand();
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = @"dbo.sproc_AssignOrder";
                    dbCommand.Parameters.Add(new SqlParameter("@WarehouseIds", WarehouseIds));
                    dbCommand.Parameters.Add(new SqlParameter("@MaterialAmounts", MaterialAmounts));
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


        public static bool IsExcelAlreadyOrdered(string excelPath)
        {
            OpenConnection();
            
            try
            {
                using (IDbConnection dbConnection = connection)
                {
                    IDbCommand dbCommand = dbConnection.CreateCommand();
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = @"dbo.sproc_CheckIfExcelExist";
                    dbCommand.Parameters.Add(new SqlParameter("@ExcelPath", excelPath));
                    using (IDataReader reader = dbCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error SQL: {exc.Message}");
                throw new Exception("Get (EnabledProducts) error: " + exc.Message, exc);
            }
        }



        public static void DeleteOrderById(ulong orderId)
        {
            OpenConnection();
            try
            {
                using (IDbConnection dbConnection = connection)
                {
                    IDbCommand dbCommand = dbConnection.CreateCommand();
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = @"dbo.sproc_DeleteOrderById";
                    dbCommand.Parameters.Add(new SqlParameter("@OrderId", Util.ConvertDynamicToString(orderId)));
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

        public static List<MaterialOrder> GetMaterialByOrderId(ulong orderId)
        {
            OpenConnection();
            List<MaterialOrder> list = new List<MaterialOrder>();
            try
            {
                using (IDbConnection dbConnection = connection)
                {
                    IDbCommand dbCommand = dbConnection.CreateCommand();
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = @"dbo.sproc_GetMaterialByOrderId";
                    dbCommand.Parameters.Add(new SqlParameter("@OrderId", orderId.ToString()));
                    using (IDataReader reader = dbCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MaterialOrder material = new MaterialOrder
                            {
                                Code = reader["Code"].ToString(),
                                OriginalCode = reader["OriginalCode"].ToString(),
                                AvailableAmount = double.Parse(reader["AvailableAmount"].ToString()),
                                StockTotal = double.Parse(reader["StockTotal"].ToString()),
                                WantedAmount = double.Parse(reader["WantedAmount"].ToString()),
                                WarehouseId = new Guid(reader["Warehouse_FK"].ToString()),
                                Location = reader["Location"].ToString(),
                                Ktl = reader["KLT"].ToString(),
                                OriginName = reader["OriginName"].ToString(),
                                BOMUnit = reader["BOMUnit"].ToString(),
                                WarehouseUnit = reader["WarehouseUnit"].ToString()
                            };

                            list.Add(material);
                        }
                    }
                }
                connection.Close();
                return list;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error SQL: {exc.Message}");
                throw new Exception("Get (EnabledProducts) error: " + exc.Message, exc);
            }
        }

        public static Order InsertBOMMaterialList(List<MaterialBOM> materialList, string excelPath)
        {
            OpenConnection();
            Dictionary<string, string> concatDictionary = Util.ConcatBOMMaterialList(SplitOn, materialList);
            string MaterialCodes = concatDictionary[Defs.COL_MATERIAL_CODE];
            string MaterialAmounts = concatDictionary[Defs.COL_AMOUNT];
            string MaterialUnits = concatDictionary[Defs.COL_UNIT];
            string MaterialExcelRow = concatDictionary[Defs.EXCEL_ROW];
            string MaterialExcelSheet = concatDictionary[Defs.SHEET_NAME];
                Order order = new Order();
            try
            {
                using (IDbConnection dbConnection = connection)
                {
                    IDbCommand dbCommand = dbConnection.CreateCommand();
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = @"dbo.sproc_InsertPreOrder";
                    dbCommand.Parameters.Add(new SqlParameter("@MaterialCodes", MaterialCodes));
                    dbCommand.Parameters.Add(new SqlParameter("@MaterialAmounts", MaterialAmounts));
                    dbCommand.Parameters.Add(new SqlParameter("@MaterialUnits", MaterialUnits));
                    dbCommand.Parameters.Add(new SqlParameter("@ExcelSheets", MaterialExcelSheet));
                    dbCommand.Parameters.Add(new SqlParameter("@ExcelRows", MaterialExcelRow));
                    dbCommand.Parameters.Add(new SqlParameter("@ExcelPath", excelPath));
                    using (IDataReader reader = dbCommand.ExecuteReader())
                    {
                        List<MaterialOrder> tempList = new List<MaterialOrder>();
                        while (reader.Read())
                        {
                            ulong OrderId = ulong.Parse(reader["Id"].ToString());
                            
                            if (OrderId != 0)
                            {
                                order.Id = OrderId;
                                MaterialOrder material = new MaterialOrder() {
                                    OriginalCode = reader["OriginalCode"].ToString(),
                                    AvailableAmount = double.Parse(reader["AvailableAmount"].ToString()),
                                    WantedAmount = double.Parse(reader["WantedAmount"].ToString()),
                                    Description = reader["Description"].ToString(),
                                    Location = reader["Location"].ToString(),
                                    Ktl = reader["KLT"].ToString(),
                                    BOMUnit = reader["BOMUnit"].ToString(),
                                    WarehouseUnit = reader["WarehouseUnit"].ToString(),
                                    OriginName = reader["OriginName"].ToString(),
                                    StockTotal = Util.ConvertDynamicToDouble(reader["StockTotal"]),
                                    ProviderName = Util.ConvertDynamicToString(reader["ProviderName"])
                                };
                                tempList.Add(material);
                            }
                        }
                        order.MaterialList = tempList;
                        order.ExcelPath = excelPath;
                    }
                }
                connection.Close();
                return order;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error SQL: {exc.Message}");
                throw new Exception("Get (EnabledProducts) error: " + exc.Message, exc);
            }
        }


        public static Order GetPreOrderById(Order order)
        {
            OpenConnection();
            try
            {
                using (IDbConnection dbConnection = connection)
                {
                    IDbCommand dbCommand = dbConnection.CreateCommand();
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = @"dbo.sproc_GetPreOrderById";
                    dbCommand.Parameters.Add(new SqlParameter("@OrderId", Util.ConvertDynamicToString(order.Id)));
                    using (IDataReader reader = dbCommand.ExecuteReader())
                    {
                        List<MaterialOrder> tempList = new List<MaterialOrder>();
                        while (reader.Read())
                        {
                            order.ExcelPath = reader["ExcelPath"].ToString();
                            MaterialOrder material = new MaterialOrder()
                            {
                                OriginalCode = reader["OriginalCode"].ToString(),
                                AvailableAmount = double.Parse(reader["AvailableAmount"].ToString()),
                                WantedAmount = double.Parse(reader["WantedAmount"].ToString()),
                                Description = reader["Description"].ToString(),
                                Location = reader["Location"].ToString(),
                                Ktl = reader["KLT"].ToString(),
                                BOMUnit = reader["BOMUnit"].ToString(),
                                WarehouseUnit = reader["WarehouseUnit"].ToString(),
                                OriginName = reader["OriginName"].ToString(),
                                StockTotal = Util.ConvertDynamicToDouble(reader["StockTotal"]),
                                ProviderName = Util.ConvertDynamicToString(reader["ProviderName"])
                            };
                            tempList.Add(material);
                        }
                        order.MaterialList = tempList;
                    }
                }
                connection.Close();
                return order;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error SQL: {exc.Message}");
                throw new Exception("Get (EnabledProducts) error: " + exc.Message, exc);
            }
        }


        public static DataSet GetMaterialFordummies()
        {
            OpenConnection();
            var dataSet = new DataSet();
            try
            {
                var select = $"SELECT * FROM MaterialList WHERE Origin_FK = {Defs.ORIGIN_PROJECT_LEFTOVERS}  ORDER BY [WhenInserted]";
                var c = new SqlConnection(connetionString); 
                var dataAdapter = new SqlDataAdapter(select, c);

                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                dataAdapter.Fill(dataSet);
                connection.Close();
                return dataSet;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error SQL: {exc.Message}");
                connection.Close();
                throw new Exception("Get (EnabledProducts) error: " + exc.Message, exc);
            }
            
        }
        
        public static DataSet GetMaterial()
        {
            OpenConnection();
            var dataSet = new DataSet();
            try
            {
                var select = $"SELECT * FROM MaterialList ORDER BY [WhenInserted]";
                var c = new SqlConnection(connetionString); 
                var dataAdapter = new SqlDataAdapter(select, c);

                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                dataAdapter.Fill(dataSet);
                connection.Close();
                return dataSet;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error SQL: {exc.Message}");
                connection.Close();
                throw new Exception("Get (EnabledProducts) error: " + exc.Message, exc);
            }
            
        }


        

        public static void UpdateMaterialList(List<MaterialStuck> materialList)
        {
            OpenConnection();
            try
            {
                using (IDbConnection dbConnection = connection)
                {
                    foreach (MaterialStuck material in materialList)
                    {
                        Guid warehouseId = material.Id;
                        string originalCode = material.OriginalCode;
                        string warehouseIdString = Util.IsEmptyGuid(warehouseId) ? String.Empty : warehouseId.ToString();
                        string code = Util.NormalizeString(material.OriginalCode);
                        
                        double total = material.Total;
                        string ktl = material.Ktl == null ? "" : material.Ktl;
                        string description = material.Description == null ? "" : material.Description;
                        string name = string.Empty;
                        string provider = material.Provider == null ? "" : material.Provider;
                        string location = material.Location == null ? "" : material.Location;
                        string unit = material.Unit == null ? "" : material.Unit;
                        bool isActive = material.IsActive;
                        IDbCommand dbCommand = dbConnection.CreateCommand();
                        dbCommand.CommandType = CommandType.StoredProcedure;
                        dbCommand.CommandText = @"dbo.sproc_UpdateWarehouseMaterial";
                        dbCommand.Parameters.Add(new SqlParameter("@WarehouseId", warehouseIdString));
                        dbCommand.Parameters.Add(new SqlParameter("@Code", code));
                        dbCommand.Parameters.Add(new SqlParameter("@OriginalCode", originalCode));
                        dbCommand.Parameters.Add(new SqlParameter("@Description", description));
                        dbCommand.Parameters.Add(new SqlParameter("@ProviderName", provider));
                        dbCommand.Parameters.Add(new SqlParameter("@Total", total));
                        dbCommand.Parameters.Add(new SqlParameter("@Unit", unit));
                        dbCommand.Parameters.Add(new SqlParameter("@Location", location));
                        dbCommand.Parameters.Add(new SqlParameter("@KLT", ktl));
                        dbCommand.Parameters.Add(new SqlParameter("@MaterialName", name));
                        dbCommand.Parameters.Add(new SqlParameter("@IsActive", isActive ? 1 : 0));
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

        public static void InsertMaterialList(List<MaterialStuck> materialList)
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
                            string ktl = material.Ktl == null ? "": material.Ktl;
                            string provider = material.Provider == null ? "" : material.Provider;
                            string location = material.Location == null ? "" : material.Location;
                            string unit = material.Unit == null ? "" : material.Unit;
                            IDbCommand dbCommand = dbConnection.CreateCommand();
                            dbCommand.CommandType = CommandType.StoredProcedure;
                            dbCommand.CommandText = @"dbo.sproc_InsertMaterial";
                            //dbCommand.CommandText = @"dbo.sproc_Recuperation";
                            dbCommand.Parameters.Add(new SqlParameter("@Code", code));
                            dbCommand.Parameters.Add(new SqlParameter("@OriginalCode", originalCode));
                            dbCommand.Parameters.Add(new SqlParameter("@Total", total));
                            dbCommand.Parameters.Add(new SqlParameter("@KTL", ktl));
                            dbCommand.Parameters.Add(new SqlParameter("@ProviderName", provider));
                            dbCommand.Parameters.Add(new SqlParameter("@Location", location));
                            dbCommand.Parameters.Add(new SqlParameter("@Unit", unit));
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

     
    }
}
