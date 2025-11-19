using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace M17A_Library.Book
{
    public partial class fBook : Form
    {
        string coverImageFile = "";
        int nBookSelected = 0;
        Database database;

        public fBook(Database database)
        {
            InitializeComponent();

            this.database = database;

            ListBooks();
        }

        private void B_OpenFile_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();

            file.Filter = "Imagens |*.jpg;*.jpeg;*.png; | Todos os ficheiros | *.*";
            file.InitialDirectory = "C:\\";
            file.Multiselect = false;

            if (file.ShowDialog() == DialogResult.OK)
            {
                string f = file.FileName;

                if (System.IO.File.Exists(f))
                {
                    PB_CoverImage.SizeMode = PictureBoxSizeMode.StretchImage;
                    PB_CoverImage.Image = Image.FromFile(f);
                    coverImageFile = f;
                }
            }
        }

        private void B_Save_Click(object sender, EventArgs e)
        {
            Book newBook = new Book(database);

            newBook.title = TB_Title.Text;
            newBook.isbn = TB_ISBN.Text;
            newBook.year = int.Parse(TB_Year.Text);
            newBook.author = TB_Author.Text;
            newBook.aquisitionDate = DTP_AquisitionDate.Value;
            newBook.price = decimal.Parse(TB_Price.Text);
            newBook.coverImage = Utilities.ProgrammFolder("M17A_Library") + @"\" + newBook.isbn;
            newBook.state = true;

            List<string> errors = newBook.Validate();

            if (errors.Count > 0)
            {
                string message = "";

                foreach (string error in errors)
                {
                    message += error + "; ";
                }

                LB_Feedback.Text = message;
                LB_Feedback.ForeColor = Color.Red;

                return;
            }

            newBook.Add();

            if (coverImageFile != "")
            {
                if (System.IO.File.Exists(coverImageFile) == true)
                {
                    System.IO.File.Copy(coverImageFile, newBook.coverImage);
                }
            }

            ListBooks();

            CleanForm();

            LB_Feedback.Text = "Livro guardado com sucesso.";
            LB_Feedback.ForeColor = Color.Black;
        }

        private void ListBooks()
        {
            DGV_Books.AllowUserToAddRows = false;
            DGV_Books.AllowUserToDeleteRows = false;
            DGV_Books.MultiSelect = false;
            DGV_Books.ReadOnly = true;
            DGV_Books.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            Book book = new Book(database);
            DGV_Books.DataSource = book.List();
        }

        private void CleanForm()
        {
            TB_Year.Text = "";
            TB_Author.Text = "";
            TB_ISBN.Text = "";
            TB_Title.Text = "";
            TB_Price.Text = "";
            PB_CoverImage.Image = null;
            coverImageFile = "";
            DTP_AquisitionDate.Value = DateTime.Now;
        }

        private void DGV_Books_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int line = DGV_Books.CurrentCell.RowIndex;

            if (line < 0)
            {
                return;
            }

            nBookSelected = int.Parse(DGV_Books.Rows[line].Cells[0].Value.ToString());
        }

        private void DeleteBook()
        {
            if (nBookSelected == 0)
            {
                MessageBox.Show("Selecione um livro para eliminar.");
                return;
            }

            if (MessageBox.Show("Tem a certeza que pretende eliminar o livro selecionado?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes )
            {
                Book delete = new Book(database);
                delete.nBook = nBookSelected;

                delete.Delete();

                nBookSelected = 0;

                ListBooks();
            }
        }

        private void BT_Delete_Click(object sender, EventArgs e)
        {
            DeleteBook();
        }
    }
}
