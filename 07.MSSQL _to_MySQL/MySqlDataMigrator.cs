namespace _07.MSSQL__to_MySQL
{
    using System.Linq;

    using SagittaDB.Models.MySQL;
    using SagittaDB.Models;

    public class MySqlDataMigrator
    {
        public void MigrateDataSqlServerToFromMySql()
        {
            using (var contextSqlServer = new SagittaDBEntities())
            {
                using (var contextMySqlServer = new sagittadb_mysqlEntities())
                {

                    foreach (var vendorSqlServer in contextSqlServer.Vendors)
                    {
                        if (!contextMySqlServer.vendors
                            .Any(v => v.Name == vendorSqlServer.Vendor_Name))
                        {
                            contextMySqlServer.vendors.Add(new vendor()
                            {
                                Name = vendorSqlServer.Vendor_Name
                            });
                        }
                    }
                }
            }
        }
    }
}
