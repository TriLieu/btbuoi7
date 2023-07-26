using Lab05.BUS;
using Lab05.DAL.Entity;
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

namespace Lab05.GUI
{
    public partial class DangKiChuyenNganh : Form
    {
        private readonly FacultyService facultyService = new FacultyService();
        private readonly MajorService majorService = new MajorService();
        private readonly StudentService studentService = new StudentService();

        public DangKiChuyenNganh()
        {
            InitializeComponent();
        }

        private void DangKiChuyenNganh_Load(object sender, EventArgs e)
        {
            List <Faculty> ls = facultyService.GetAll();
            fillCmbKhoa(ls);
        }

        private void fillCmbKhoa(List<Faculty> ls)
        {
            cmb_Khoa.DataSource = ls;
            cmb_Khoa.DisplayMember = "FacultyName";
            cmb_Khoa.ValueMember = "FacultyID";
        }
        private void fillCmbMajor(List<Major> ls) 
        {
            cmb_ChuyenNganh.DataSource = ls;
            cmb_ChuyenNganh.DisplayMember = "Name";
            cmb_ChuyenNganh.ValueMember = "MajorID";
        }

        private void cmb_Khoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            Faculty selectedFaculty = cmb_Khoa.SelectedItem as Faculty;
            if (selectedFaculty!= null)
            {
                List<Major> ls1 = majorService.GetAllByFaulty(selectedFaculty.FacultyID);
                fillCmbMajor(ls1);
                List<Student> ls2 = studentService.GetAllHasNoMajor(selectedFaculty.FacultyID);
                BindData(ls2);
            }
        }
        private void BindData(List<Student> ls)
        {
            dataGridView1.Rows.Clear();
            foreach (Student s in ls)
            {
                int idx = dataGridView1.Rows.Add();
                dataGridView1.Rows[idx].Cells[1].Value = s.StudentID.ToString();
                dataGridView1.Rows[idx].Cells[2].Value = s.FullName.ToString();
                dataGridView1.Rows[idx].Cells[3].Value = s.Faculty.FacultyName;
                dataGridView1.Rows[idx].Cells[4].Value = s.AverageScore.ToString();

            }
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            bool check = false;
            Faculty selectedFaculty = cmb_Khoa.SelectedItem as Faculty;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Selected)
                {
                    string filepath = studentService.searchFilePath(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    Student student = studentService.findByID(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    Student s = new Student()
                    {
                        StudentID = student.StudentID,
                        FullName = student.FullName,
                        AverageScore = student.AverageScore,
                        FacultyID = student.Faculty.FacultyID,
                        MajorID = int.Parse(cmb_ChuyenNganh.SelectedValue.ToString()),
                        Avatar = student.Avatar,
                    };
                    check = true;
                    studentService.InsertUpdate(s);
                }
            }
            if (check)
            {
                List<Student> ls2 = studentService.GetAllHasNoMajor(selectedFaculty.FacultyID);
                BindData(ls2);
                MessageBox.Show("Dang ki chuyen nganh thanh cong !");
            }
            else
            {
                MessageBox.Show("Can chon it nhat 1 sinh vien de dang ki");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
