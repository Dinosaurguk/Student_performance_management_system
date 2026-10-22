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
    public partial class FormAddStudent : Form
    {
        public FormAddStudent()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Проверяем, что поля ФИО (textBox1) и Группа (textBox2) не пустые
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!");
                return;
            }

            // 2. Строка подключения к твоей базе данных Access и SQL-запрос к таблице Студент
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database1.accdb;";
            string query = "INSERT INTO Студент (ФИО, Группа) VALUES (@fio, @group)";

            // 3. Запись данных в файл базы данных
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            using (OleDbCommand command = new OleDbCommand(query, connection))
            {
                command.Parameters.AddWithValue("@fio", textBox1.Text);
                command.Parameters.AddWithValue("@group", textBox2.Text);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Студент успешно добавлен!");

                    this.DialogResult = DialogResult.OK; // Передаем сигнал главной форме на мгновенное обновление
                    this.Close(); // Закрываем форму добавления
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка базы данных: " + ex.Message);
                }
            }

        }
    }
}
