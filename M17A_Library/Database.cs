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

            sql = $"CREATE DATABASE {this.databaseName} ON PRIMARY (NAME = {this.databaseName}, FILENAME = '{this.databasePath}')";

            command = new SqlCommand(sql, sqlConnection);
            command.ExecuteNonQuery();

            sqlConnection.ChangeDatabase(this.databaseName);

            sql = @"CREATE TABLE Livros(
                  
                      nLivro INT IDENTITY PRIMARY KEY,

                      Titulo VARCHAR(50) NOT NULL,
                      Autor VARCHAR(100) NOT NULL,
                      Isbn VARCHAR(13) NOT NULL,
                      Ano INT CHECK(ANO > 0) NOT NULL,
                      DataAquisicao DATE DEFAULT GETDATE(),
                      Preco MONEY CHECK(Preco >= 0) NOT NULL,
                      Capa VARCHAR(500),
                      Estado BIT DEFAULT 1

                    )";

            command = new SqlCommand(sql, sqlConnection);
            command.ExecuteNonQuery();
            command.Dispose();
        }
    }
}
