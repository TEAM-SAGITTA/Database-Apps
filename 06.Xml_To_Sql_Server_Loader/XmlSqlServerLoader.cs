﻿namespace _06.Xml_To_Sql_Server_Loader
{
    using System;
    using System.Data.SqlClient;
    using System.IO;
    using System.Xml;

    public class XmlSqlServerLoader
    {
        private const string firstTableName = "CompanyNames";
        private const string secondTableName = "ExpensesByMonth";
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
                            XmlSqlServerLoader.DropCreateSqlServerTables(
                                connection, transaction);

                            XmlSqlServerLoader.PolulateSqlTables(
                                connection, filePath, transaction);

                            transaction.Commit();
                        }
                        catch (Exception exeption)
                        {
                            transaction.Rollback();
                            throw exeption;
                        }
                    }

                    connection.Close();                  
                }
            }
        }


        public static void DropCreateSqlServerTables(
             SqlConnection connection,
             SqlTransaction transaction)
        {
            string dropConstraintStr = string.Format("IF OBJECT_ID('{0}') IS NOT NULL ALTER TABLE {0} " +
                "DROP CONSTRAINT FK_CompanyNameId;", secondTableName);

            string createFirstTableStr = string.Format(
                "IF OBJECT_ID('{0}') IS NOT NULL DROP TABLE {0} CREATE TABLE {0}" +
                "(CompanyNameId  int IDENTITY(1,1) PRIMARY KEY, CompanyName NVARCHAR(MAX) NOT NULL)",
                firstTableName);

            string createSecondTableStr = string.Format(
                "IF OBJECT_ID('{0}') IS NOT NULL DROP TABLE {0} CREATE TABLE {0}" +
                "(Id  int IDENTITY(1,1) PRIMARY KEY, ExpenseMonth NVARCHAR(50) NULL," +
                "CompanyNameId int, Expenses MONEY NULL," + 
                "CONSTRAINT FK_CompanyNameId FOREIGN KEY (CompanyNameId) REFERENCES {1}(CompanyNameId) ON DELETE SET NULL)",
                secondTableName, firstTableName);

            SqlCommand dropConstraint = new SqlCommand(dropConstraintStr, connection, transaction);
            dropConstraint.ExecuteNonQuery();

            SqlCommand createFirstTable = new SqlCommand(createFirstTableStr, connection, transaction);
            createFirstTable.ExecuteNonQuery();

            SqlCommand createSecondTable = new SqlCommand(createSecondTableStr, connection, transaction);
            createSecondTable.ExecuteNonQuery();
        }

        public static void PolulateSqlTables(
             SqlConnection connection,
             string filePath,
             SqlTransaction transaction)
        {
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
                var insertStringCmd = string.Format("INSERT INTO {0} VALUES('{1}')", firstTableName, RootAtributeValue); // pravq prostotiq da grymne za test
                SqlCommand insertVendor = new SqlCommand(insertStringCmd, connection, transaction);
                insertVendor.ExecuteNonQuery();
                XmlNodeList childNodes = vendor.SelectNodes(childName);

                for (int i = 0; i < childNodes.Count; i++)
                {
                    var expense = childNodes[i];
                    var expenceMonth = expense.Attributes[childAtribute].Value;
                    var expenceValue = decimal.Parse(childNodes[i].InnerText);

                    string insertChildsStr = string.Format("INSERT INTO {0} VALUES('{1}', {2}, @Money)",
                        secondTableName, expenceMonth, nodeId);

                    SqlCommand inserChilds = new SqlCommand(insertChildsStr, connection, transaction);                    
                    inserChilds.Parameters.AddWithValue("@Money", expenceValue);
                    inserChilds.ExecuteNonQuery();
                }

                nodeId++;
            }
        }
    }
}