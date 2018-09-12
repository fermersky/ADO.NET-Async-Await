using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADO.NET_async_await
{
    public partial class Form1 : Form
    {
        string cs = @"Data Source = COMP505\SQLEXPRESS; Initial Catalog = PublishingHouse; Integrated Security = true;";
        SqlConnection conn = null;
        DataTable table = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public async Task FillComboBoxAsync()
        {
            SqlConnection con1;
            using (con1 = new SqlConnection(cs))
            {
                await con1.OpenAsync();
                string sql = "select FirstName + ' ' + LastName from book.Books join book.Authors on book.Books.Id_Author = book.Authors.Id_Author;";

                SqlCommand com = new SqlCommand(sql, con1);
                SqlDataReader reader = await com.ExecuteReaderAsync();

                while(await reader.ReadAsync())
                {
                    comboBox1.Items.Add(reader[0]);
                }
            }
        }

        public async Task CountBooksWittenByAsync(string name)
        {
            SqlConnection conn1;
            using (conn1 = new SqlConnection(cs))
            {
                await conn1.OpenAsync();
                string sql = "select count(id_book) from book.Books join book.Authors on book.Books.Id_Author = book.Authors.Id_Author where (FirstName + ' ' + LastName) = @fio;";
                SqlCommand com = new SqlCommand(sql, conn1);
                com.Parameters.AddWithValue("@fio", name);

                label2.Text = Convert.ToInt32(await com.ExecuteScalarAsync()).ToString();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            using (conn = new SqlConnection(cs))
            {
                await conn.OpenAsync();
                await FillComboBoxAsync();
                string sql = "waitfor delay '00:00:03';";
                
                sql += textBox1.Text;

                SqlCommand com = new SqlCommand(sql, conn);
                SqlDataReader reader = await com.ExecuteReaderAsync();
                

                table = new DataTable();

                



                int line = 0;

                do
                {
                    while (await reader.ReadAsync())
                    {
                        if (line == 0)
                            for (int i = 0; i < reader.FieldCount; i++)
                                table.Columns.Add(reader.GetName(i));
                        DataRow row = table.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader[i];
                        }
                        table.Rows.Add(row);
                        line++;
                    }
                } while (await reader.NextResultAsync());

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = table;



               
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await CountBooksWittenByAsync(comboBox1.Items[comboBox1.SelectedIndex].ToString());

            //MessageBox.Show();
        }
    }
}
