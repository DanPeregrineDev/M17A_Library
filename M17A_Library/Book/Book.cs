using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace M17A_Library.Book
{
    internal class Book : iItem
    {
        Database database;
        public int nBook { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string isbn { get; set; }
        public int year { get; set; }
        public DateTime aquisitionDate { get; set; }
        public decimal price { get; set; }
        public string coverImage { get; set; }
        public bool state { get; set; }

        public Book(Database database)
        {
            this.database = database;
        }

        public void Add()
        {
            string sql = @"INSERT INTO Books(Title, Author, Isbn, Year, AquisitionDate, Price, CoverImage)
                           VALUES(@Title, @Author, @Isbn, @Year, @AquisitionDate, @Price, @CoverImage)";

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName = "@Title",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Value = this.title
                },

                new SqlParameter()
                {
                    ParameterName = "@Author",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Value = this.author
                },

                new SqlParameter()
                {
                    ParameterName = "@Isbn",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Value = this.isbn
                },

                new SqlParameter()
                {
                    ParameterName = "@Year",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Value = this.year
                },

                new SqlParameter()
                {
                    ParameterName = "@AquisitionDate",
                    SqlDbType = System.Data.SqlDbType.DateTime,
                    Value = this.aquisitionDate
                },

                new SqlParameter()
                {
                    ParameterName = "@Price",
                    SqlDbType = System.Data.SqlDbType.Money,
                    Value = this.price
                },

                new SqlParameter()
                {
                    ParameterName = "@CoverImage",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Value = this.coverImage
                },
            };

            database.ExecuteSQL(sql, parameters);
        }

        public void Delete()
        {
            database.ExecuteSQL("DELETE FROM Books WHERE nBook = " + nBook);
        }

        public void Edit()
        {
            throw new NotImplementedException();
        }

        public List<string> Validate()
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(title) || title.Length < 3)
            {
                errors.Add("Title is required and must be more than 3 characters long.");
            }

            if (string.IsNullOrEmpty(author) || author.Length < 3)
            {
                errors.Add("Author is required and must be more than 3 characters long.");
            }

            if (year < 0 || year > DateTime.Now.Year)
            {
                errors.Add("Year must be a positive number and cannot be in the future.");
            }

            if (isbn.Length != 13)
            {
                errors.Add("ISBN must be exactly 13 characters long.");
            }

            return errors;
        }

        public DataTable List()
        {
            return database.ReturnSQL("SELECT nBook, Title, Author, ISBN FROM Books ORDER BY Title");
        }
    }
}
