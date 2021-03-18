using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace StudentDiary
{
    public partial class Main : Form
    {
        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);

        public Main()
        {
            InitializeComponent();

            RefreshDiary();
            SetColumnsHeader();
        }

        private void RefreshDiary()
        {
            var students = _fileHelper.DeserializeFromFile();
            dgvDiary.DataSource = students;
        }

        private void SetColumnsHeader()
        {
            dgvDiary.Columns[0].HeaderText = "Numer";
            dgvDiary.Columns[1].HeaderText = "Imię";
            dgvDiary.Columns[2].HeaderText = "Nazwisko";
            dgvDiary.Columns[3].HeaderText = "Uwagi";
            dgvDiary.Columns[4].HeaderText = "Matematyka";
            dgvDiary.Columns[5].HeaderText = "Technologia";
            dgvDiary.Columns[6].HeaderText = "Fizyka";
            dgvDiary.Columns[7].HeaderText = "Język polski";
            dgvDiary.Columns[8].HeaderText = "Język obcy";
            dgvDiary.Columns[9].HeaderText = "Grupa";
            dgvDiary.Columns[10].HeaderText = "Zaj. dodatkowe";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditStudent = new AddEditStudent();
            addEditStudent.StudentAdded += AddEditStudent_StudentAdded;
            addEditStudent.ShowDialog();
            addEditStudent.StudentAdded -= AddEditStudent_StudentAdded;
        }

        private void AddEditStudent_StudentAdded()
        {
            RefreshDiary();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia, którego dane chcesz edytować");
                return;
            }

            var addEditStudent = new AddEditStudent(Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));
            addEditStudent.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia, którego chcesz usunąć");
                return;
            }

            var selectedStudent = dgvDiary.SelectedRows[0];

            var confirmDelete = MessageBox.Show($"Czy na pewno chcesz usunąć ucznia {selectedStudent.Cells[1].Value.ToString() + " " + selectedStudent.Cells[2].Value.ToString().Trim()}", "Usuwanie ucznia", MessageBoxButtons.OKCancel);

            if (confirmDelete == DialogResult.OK)
            {
                var students = _fileHelper.DeserializeFromFile();
                students.RemoveAll(x => x.Id == Convert.ToInt32(selectedStudent.Cells[0].Value));
                _fileHelper.SerializeToFile(students);

                dgvDiary.DataSource = students;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        // Method responsible for filtering list of students by group parameter.
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            var studentList = _fileHelper.DeserializeFromFile();
            var filteredList = new List<Student>();

            if (cbFilter.SelectedIndex == 0)
            {
                studentList = _fileHelper.DeserializeFromFile();
                dgvDiary.DataSource = studentList;
            }
            else
            {
                filteredList = studentList.Where(x => x.GroupID == cbFilter.SelectedIndex).ToList();
                dgvDiary.DataSource = filteredList;
            }
        }
        
    }
}
