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
    public partial class AddEditStudent : Form
    {
        private string _filePath = Path.Combine(Environment.CurrentDirectory, "students.txt");
        private int _studentId;

        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Path.Combine(Environment.CurrentDirectory, "students.txt"));

        public AddEditStudent(int id = 0)
        {
            InitializeComponent();
            _studentId = id;

            if (id != 0)
            {
                Text = "Edytowanie danych ucznia";

                var students = _fileHelper.DeserializeFromFile();
                var student = students.FirstOrDefault(x => x.Id == id);

                if (student == null)
                {
                    throw new Exception("Brak użytkownika o podanym Id");
                }

                tbId.Text = student.Id.ToString();
                tbFirstName.Text = student.FirstName;
                tbLastName.Text = student.LastName;
                tbMath.Text = student.Math;
                tbPhysics.Text = student.Physics;
                tbTechnology.Text = student.Technology;
                tbPolishLang.Text = student.PolishLang;
                tbForeignLang.Text = student.ForeignLang;
                rtbComments.Text = student.Comments;
                cbActivities.Checked = student.Activities;
                cbGroupId.SelectedIndex = student.GroupID - 1;  // GroupID is greater by 1 than ComboBox index number
            }

            tbFirstName.Select();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            if (_studentId != 0)
            {
                students.RemoveAll(x => x.Id == _studentId);
            }
            else
            {
                var studentWithHighestId = students.OrderByDescending(x => x.Id).FirstOrDefault();
                _studentId = studentWithHighestId == null ? 1 : studentWithHighestId.Id + 1;
            }

            var student = new Student
            {
                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Comments = rtbComments.Text,
                Math = tbMath.Text,
                Physics = tbPhysics.Text,
                PolishLang = tbPolishLang.Text,
                ForeignLang = tbForeignLang.Text,
                Technology = tbTechnology.Text,
                GroupID = Convert.ToInt32(cbGroupId.Text),
                Activities = JoinedActivities()
            };

            students.Add(student);

            _fileHelper.SerializeToFile(students);

            Close();
        }

        // Function responsible for sending true or false to Activities property of class Student
        private bool JoinedActivities()
        {
            if (cbActivities.Checked)
                return true;
            else
                return false;
        }
    }
}
