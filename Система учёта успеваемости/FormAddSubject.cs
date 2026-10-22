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
    public partial class FormAddSubject : Form
    {
        public FormAddSubject()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Проверяем, что поле Название предмета (textBox1) не пустое
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Пожалуйста, введите название предмета!");
                return;
            }

            // 2. Строка подключения к твоей базе данных Access и SQL-запрос к таблице Предмет
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database1.accdb;";
            string query = "INSERT INTO Предмет (Название) VALUES (@name)";

            // 3. Запись данных в файл базы данных
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            using (OleDbCommand command = new OleDbCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", textBox1.Text);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Предмет успешно добавлен!");

                    this.DialogResult = DialogResult.OK; // Сигнал главной форме для мгновенного обновления таблицы
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
