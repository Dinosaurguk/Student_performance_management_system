using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Система_учёта_успеваемости
{
    public partial class FormAddUser : Form
    {
        public FormAddUser()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void FormAddUser_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Проверяем, что поля ФИО (textBox1) и Предмет (textBox2) не пустые
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!");
                return;
            }

            // 2. Строка подключения к Access и SQL-запрос к таблице Препод
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database1.accdb;";
            string query = "INSERT INTO Препод (ФИО, Предмет) VALUES (@fio, @subject)";

            // 3. Запись данных в базу
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            using (OleDbCommand command = new OleDbCommand(query, connection))
            {
                command.Parameters.AddWithValue("@fio", textBox1.Text);
                command.Parameters.AddWithValue("@subject", textBox2.Text);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Преподаватель успешно добавлен!");
                    this.DialogResult = DialogResult.OK; // Сигнал для главной формы, что добавление прошло успешно
                    this.Close(); // Закрываем форму
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }


        }
    }
}
    
