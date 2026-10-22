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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            {
                // Проверяем, какая вкладка сейчас выбрана в TabControl
                switch (tabControl1.SelectedIndex)
                {
                    case 0: // Вкладка "Преподаватель"
                        FormAddUser prepodForm = new FormAddUser();
                        if (prepodForm.ShowDialog() == DialogResult.OK)
                        {
                            // Мгновенное обновление таблицы Преподавателей
                            this.преподTableAdapter.Fill(this.database1DataSet.Препод);
                        }
                        break;

                    case 1: // Вкладка "Студент"
                        FormAddStudent studentForm = new FormAddStudent();
                        if (studentForm.ShowDialog() == DialogResult.OK)
                        {
                            // Мгновенное обновление таблицы Студентов
                            this.студентTableAdapter.Fill(this.database1DataSet.Студент);
                        }
                        break;

                    case 2: // Вкладка "Предмет"
                        FormAddSubject subjectForm = new FormAddSubject();
                        if (subjectForm.ShowDialog() == DialogResult.OK)
                        {
                            // Мгновенное обновление таблицы Предметов
                            this.предметTableAdapter.Fill(this.database1DataSet.Предмет);
                        }
                        break;

                    case 3: // Вкладка "Оценка"
                        FormAddGrade gradeForm = new FormAddGrade();
                        if (gradeForm.ShowDialog() == DialogResult.OK)
                        {
                            // Мгновенное обновление таблицы Оценок
                            this.оценкаTableAdapter.Fill(this.database1DataSet.Оценка);
                        }
                        break;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "database1DataSet.Оценка". При необходимости она может быть перемещена или удалена.
            this.оценкаTableAdapter.Fill(this.database1DataSet.Оценка);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "database1DataSet.Предмет". При необходимости она может быть перемещена или удалена.
            this.предметTableAdapter.Fill(this.database1DataSet.Предмет);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "database1DataSet.Студент". При необходимости она может быть перемещена или удалена.
            this.студентTableAdapter.Fill(this.database1DataSet.Студент);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "database1DataSet.Препод". При необходимости она может быть перемещена или удалена.
            this.преподTableAdapter.Fill(this.database1DataSet.Препод);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            {
                // 1. Проверяем, какая вкладка сейчас открыта
                if (tabControl1.SelectedTab == null) return;

                TabPage activeTab = tabControl1.SelectedTab;
                DataGridView currentGrid = null;

                // Ищем DataGridView на текущей активной вкладке
                foreach (Control control in activeTab.Controls)
                {
                    if (control is DataGridView)
                    {
                        currentGrid = (DataGridView)control;
                        break;
                    }
                }

                // Проверяем, выбрана ли строка
                if (currentGrid == null || currentGrid.CurrentRow == null || currentGrid.CurrentRow.IsNewRow)
                {
                    MessageBox.Show("Пожалуйста, выберите запись для удаления.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Подтверждение удаления
                DialogResult confirm = MessageBox.Show($"Вы уверены, что хотите удалить выбранную запись из вкладки \"{activeTab.Text}\"?",
                                                      "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm != DialogResult.Yes) return;

                try
                {
                    // 2. Определяем таблицу и ключевое поле по тексту заголовка вкладки
                    string tableName = "";
                    string primaryKeyName = "";

                    switch (activeTab.Text.Trim().ToLower())
                    {
                        case "студент":
                            tableName = "Студент";
                            primaryKeyName = "id_Stud";
                            break;
                        case "препод":
                        case "преподаватель":
                            tableName = "Препод";
                            primaryKeyName = "id_Prepod";
                            break;
                        case "предмет":
                            tableName = "Предмет";
                            primaryKeyName = "id_Pred";
                            break;
                        case "оценка":
                            tableName = "Оценка";
                            primaryKeyName = "id_Ozen";
                            break;
                        default:
                            MessageBox.Show($"Не настроено правило удаления. Программа видит заголовок: \"{activeTab.Text}\"",
                                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }

                    // 3. Получаем ID из первой ячейки выделенной строки
                    int idValue = Convert.ToInt32(currentGrid.CurrentRow.Cells[0].Value);

                    // 4. ПРЯМАЯ СТРОКА ПОДКЛЮЧЕНИЯ К ВАШЕМУ ФАЙЛУ Database1.accdb
                    string dbPath = System.IO.Path.Combine(Application.StartupPath, "Database1.accdb");
                    string connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};";


                    string query = $"DELETE FROM [{tableName}] WHERE [{primaryKeyName}] = @id";

                    using (System.Data.OleDb.OleDbConnection connection = new System.Data.OleDb.OleDbConnection(connectionString))
                    {
                        using (System.Data.OleDb.OleDbCommand command = new System.Data.OleDb.OleDbCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", idValue);
                            connection.Open();

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // 5. Визуально удаляем строку из таблицы на экране
                                currentGrid.Rows.Remove(currentGrid.CurrentRow);
                                MessageBox.Show("Запись успешно удалена из базы данных!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось выполнить удаление.\n\nВозможная причина: запись связана с другими таблицами (например, вы пытаетесь удалить Студента, у которого есть оценки).\n\nДетали ошибки: {ex.Message}",
                                    "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // Фиксируем все текущие изменения из ячеек интерфейса
                this.Validate();

                студентBindingSource.EndEdit();
                преподBindingSource.EndEdit();
                предметBindingSource.EndEdit();
                оценкаBindingSource.EndEdit();

                // 🌟 САМАЯ ВАЖНАЯ СТРОКА: Говорим DataSet, что физически удаленные 
                // через SQL строки больше не нужно пытаться повторно удалять в БД
                this.database1DataSet.AcceptChanges();

                // Теперь безопасно сохраняем новые добавленные и измененные строки
                this.студентTableAdapter.Update(this.database1DataSet.Студент);
                this.преподTableAdapter.Update(this.database1DataSet.Препод);
                this.предметTableAdapter.Update(this.database1DataSet.Предмет);
                this.оценкаTableAdapter.Update(this.database1DataSet.Оценка);

                MessageBox.Show("Изменения во всех таблицах успешно сохранены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось сохранить изменения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 1. Проверяем, введена ли строка для поиска
            string searchText = txtSearch.Text.Trim().Replace("'", "''"); // Экранируем одинарные кавычки для безопасности

            if (tabControl1.SelectedTab == null) return;
            TabPage activeTab = tabControl1.SelectedTab;

            // 2. Если поле пустое, просто сбрасываем фильтр (показываем все записи)
            if (string.IsNullOrEmpty(searchText))
            {
                СброситьВсеФильтры();
                return;
            }

            try
            {
                // 3. Фильтруем нужную таблицу в зависимости от активной вкладки
                switch (activeTab.Text.Trim().ToLower())
                {
                    case "студент":
                        // Ищем совпадения по полю ФИО или Группа (частичное совпадение)
                        студентBindingSource.Filter = $"ФИО LIKE '%{searchText}%' OR Группа LIKE '%{searchText}%'";
                        break;

                    case "препод":
                    case "преподаватель":
                        // Ищем по ФИО преподавателя или названию его Предмета
                        преподBindingSource.Filter = $"ФИО LIKE '%{searchText}%' OR Предмет LIKE '%{searchText}%'";
                        break;

                    case "предмет":
                        // Ищем по Названию предмета
                        предметBindingSource.Filter = $"Название LIKE '%{searchText}%'";
                        break;

                    case "оценка":
                        // В таблице Оценка хранятся ID. Если нужно искать точное число (например, саму оценку)
                        if (int.TryParse(searchText, out int grade))
                        {
                            оценкаBindingSource.Filter = $"Значение = {grade}";
                        }
                        else
                        {
                            MessageBox.Show("Для таблицы оценок введите числовое значение (например: 3, 4, 5).",
                                            "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        break;

                    default:
                        MessageBox.Show($"Не настроено правило поиска для вкладки: \"{activeTab.Text}\"",
                                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении поиска: {ex.Message}\nПроверьте, правильно ли указаны имена полей.",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            button3_Click(sender, e); // Поиск будет срабатывать при каждом изменении текста
        }

        // Вспомогательный метод для очистки фильтров
        private void СброситьВсеФильтры()
        {
            студентBindingSource.Filter = null;
            преподBindingSource.Filter = null;
            предметBindingSource.Filter = null;
            оценкаBindingSource.Filter = null;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
