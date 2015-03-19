namespace _06.Xml_To_Sql_Server_Loader
{
    using System;
    using System.Data.SqlClient;
    using System.IO;
    using System.Xml;

    public class XmlSqlServerLoader
    {
        public static void XmlDataPusher(string DbConnectionString, string filePath)
        {            
            using (SqlConnection connection = new SqlConnection(DbConnectionString))
            {                
                connection.Open();                
                if (filePath != string.Empty)
                {
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            XmlSqlServerLoader.PolulateSqlTables(connection, filePath, transaction);
                            transaction.Commit();
                        }
                        catch (Exception exception)
                        {
                            transaction.Rollback();
                            throw exception;
                        }
                    }

                    connection.Close();                  
                }
            }
        }

        public static void PolulateSqlTables(SqlConnection connection, string filePath, SqlTransaction transaction)
        {
            string firstTableName = "Vendors";
            string secondTableName = "ExpensesByMonth";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            XmlElement root = xmlDoc.DocumentElement;
            string RootPath = "/expenses-by-month/vendor";
            string RootAtribute = "name";
            string childName = "expenses";
            string childAtribute = "month";
            XmlNodeList rootNodes = root.SelectNodes(RootPath);
            var nodeId = 1;

            foreach (XmlNode vendor in rootNodes)
            {
                var RootAtributeValue = vendor.Attributes[RootAtribute].Value;
                var insertStringCmd = string.Format("INSERT INTO {0} VALUES('{1}')", firstTableName, RootAtributeValue);
                SqlCommand insertVendor = new SqlCommand(insertStringCmd, connection, transaction);
                insertVendor.ExecuteNonQuery();
                XmlNodeList childNodes = vendor.SelectNodes(childName);

                for (int i = 0; i < childNodes.Count; i++)
                {
                    var expense = childNodes[i];
                    var expenceMonth = expense.Attributes[childAtribute].Value;
                    var expenceDate = DateTime.Parse(expenceMonth);
                    var expenceValue = decimal.Parse(childNodes[i].InnerText);

                    string insertChildsStr = string.Format("INSERT INTO {0} VALUES(@Date, {1}, @Money)",
                        secondTableName, nodeId);

                    SqlCommand inserChilds = new SqlCommand(insertChildsStr, connection, transaction);                    
                    inserChilds.Parameters.AddWithValue("@Money", expenceValue);
                    inserChilds.Parameters.AddWithValue("@Date", expenceDate);
                    inserChilds.ExecuteNonQuery();
                }

                nodeId++;
            }
        }
    }
}
