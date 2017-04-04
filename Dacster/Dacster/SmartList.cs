using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Windows;

namespace Dacster {
    class SmartList {
        public static List<string> GetInstances() {
            LogWriter Log = new LogWriter("----- Running GetInstances -----");
            List<string> instances = new List<string>();
            SqlDataSourceEnumerator serverEnumerator = SqlDataSourceEnumerator.Instance;
            System.Data.DataTable table = serverEnumerator.GetDataSources();
            int count = 0;
            if(table.Rows.Count == 0) {
                MessageBox.Show("No servers / instances found. Are you sure you're connected?", "No servers found");
            }
            foreach (System.Data.DataRow row in table.Rows) {
                instances.Add((table.Rows[count][0]).ToString());
                count += 1;
            }
            return instances;
        }
        public static List<string> GetDatabases(string instance) {
            List<string> databases = new List<string>();
            string connectionString = "Data Source=RND-SQL2008R2; Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                connection.Open();
                DataTable dbDataTable = connection.GetSchema("Databases");
                foreach (DataRow database in dbDataTable.Rows) {
                    short dbID = database.Field<short>("dbid");
                    String databaseName = database.Field<String>("database_name");
                    if (dbID > 4) {
                        databases.Add(databaseName);
                    }
                }
            }
            return databases;
        }
    }
}
