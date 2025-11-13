using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace M17A_Library
{
    internal class Database
    {
        string databaseName;
        string connectionString;
        string databasePath;

        public Database(string databaseName) 
        {
            this.databaseName = databaseName;

            connectionString += ConfigurationManager.ConnectionStrings["SQL"].ToString();

            databasePath = Utilities.ProgrammFolder("M17A_Library");
            databasePath += @"\" + databaseName + ".mdf";

            if (System.IO.File.Exists(databasePath) == false)
            {
                createDatabase();
            }
        }

        void createDatabase()
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            sqlConnection.Open();

            string sql = $@"IF EXISTS(SELECT * FROM Master.Sys.Databases
                            WHERE Name = '{this.databaseName}')
                            BEGIN
                                USE [Master];
                                EXEC sp_detach_db {this.databaseName}
                            END";

            SqlCommand command = new SqlCommand(sql, sqlConnection);
            command.ExecuteNonQuery();
        }
    }
}
