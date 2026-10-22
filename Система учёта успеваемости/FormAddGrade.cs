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
    public partial class FormAddGrade : Form
    {
        public FormAddGrade()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Проверяем, что все 5 полей заполнены
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!");
                return;
            }

            // 2. Строка подключения и SQL-запрос к таблице Оценка
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database1.accdb;";
            string query = "INSERT INTO Оценка (id_Stud, id_Pred, id_Prepod, Значение, Дата) VALUES (@stud, @pred, @prepod, @val, @date)";

            // 3. Запись данных в файл базы данных
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            using (OleDbCommand command = new OleDbCommand(query, connection))
            {
                try
                {
                    // Конвертируем текст из полей в числа и дату под типы данных твоей БД
                    command.Parameters.AddWithValue("@stud", Convert.ToInt32(textBox1.Text));
                    command.Parameters.AddWithValue("@pred", Convert.ToInt32(textBox2.Text));
                    command.Parameters.AddWithValue("@prepod", Convert.ToInt32(textBox5.Text));
                    command.Parameters.AddWithValue("@val", Convert.ToInt32(textBox4.Text));
                    command.Parameters.AddWithValue("@date", Convert.ToDateTime(textBox3.Text).ToString("dd.MM.yyyy"));

                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Оценка успешно добавлена!");

                    this.DialogResult = DialogResult.OK; // Сигнал главной форме для мгновенного обновления таблицы
                    this.Close(); // Закрываем форму
                }
                catch (FormatException)
                {
                    MessageBox.Show("Ошибка формата! В полях ID и Оценка должны быть числа, а в поле Дата — корректная дата (например, 21.10.2026).");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка базы данных: " + ex.Message);
                }
            }

        }
    }
}
