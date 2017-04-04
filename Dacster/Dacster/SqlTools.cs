using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dacster {
    public static class SqlTools {
        private static string LocateSQLPackage() {
            string sqlServerFolder = @"C:\Program Files (x86)\Microsoft SQL Server";
            string[] filePath = Directory.GetFiles(sqlServerFolder, "SqlPackage.exe", SearchOption.AllDirectories);
            return filePath[0];
        }

        private static string MakeConnectionString(string instance) {
            string connectionString = $"Server={instance}";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            return builder.ConnectionString;
        }

        private static string MakeConnectionString(string instance, string database) {
            string connectionString = $"Server={instance};Database={database}";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            return builder.ConnectionString;
        }

        private static SqlConnection CreateConnection(string connectionString) {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public static List<string> GetFullDatabaseList(string instance, SqlConnection connection) {
            List<string> databaseList = new List<string>();
            string commandString = @"SELECT name FROM sys.databases where database_id >4 and state_desc = 'ONLINE' ORDER BY name";
            SqlCommand cmd = new SqlCommand(commandString, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read()) {
                string databaseName = reader.GetString(0);
                databaseList.Add(databaseName);
            }
            return databaseList;
        }

        private static string GenerateSqlPackageCommand(string sqlPackagePath, string filePath, string instance, string database) {
            string command = @"/action:Extract /targetfile:""" + filePath + @""" /SourceServerName:" + instance + @" /SourceDatabaseName:" + database;
            return command;
        }

        private static System.Diagnostics.Process CreateSqlPackageProcess(string sqlPackagePath, string sqlCommandParams) {
            LogWriter Log = new LogWriter("----- Running CreateSqlPackageProcess -----");
            System.Diagnostics.Process sqlPackageProcess = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo sqlPackage = new System.Diagnostics.ProcessStartInfo();
            sqlPackageProcess.StartInfo.CreateNoWindow = true;
            sqlPackageProcess.StartInfo.UseShellExecute = false;
            sqlPackageProcess.StartInfo.FileName = sqlPackagePath;
            sqlPackageProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            sqlPackageProcess.StartInfo.Arguments = sqlCommandParams;
            sqlPackageProcess.StartInfo.RedirectStandardOutput = true;
            sqlPackageProcess.StartInfo.RedirectStandardError = true;
            sqlPackageProcess.OutputDataReceived += (sender, args) => Log.LogWrite(args.Data);
            sqlPackageProcess.ErrorDataReceived += (sender, args) => Log.LogWrite(args.Data);
            return sqlPackageProcess;
        }

        public static void RunSqlPackage(string instance, string database) {
            StringBuilder sqlPackageProcessOutput = new StringBuilder();
            LogWriter Log = new LogWriter("----- Running RunSqlPackage -----");
            string sqlPackagePath = LocateSQLPackage();
            Log.LogWrite($"sqlpackage.exe found at {sqlPackagePath}");
            string filePath = Dacpac.GenerateFilename(instance, database, Dacpac.outputFolder, Dacpac.currentDateTime);
            Log.LogWrite($"File path is set to {filePath}");
            string sqlCommandParams = GenerateSqlPackageCommand(sqlPackagePath, filePath, instance, database);
            Log.LogWrite($"SQL Command Params: {sqlCommandParams}");
            System.Diagnostics.Process sqlPackageProcess = CreateSqlPackageProcess(sqlPackagePath, sqlCommandParams);
            sqlPackageProcess.Start();
            sqlPackageProcess.BeginOutputReadLine();
            sqlPackageProcess.BeginErrorReadLine();
            sqlPackageProcess.WaitForExit();
        }
    }
}
