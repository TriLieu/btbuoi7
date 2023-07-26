using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Lab05.BUS;
using Lab05.DAL.Entity;

namespace Lab05.GUI
{
    public partial class Form1 : Form
    {
        private string filepath = "";
        private readonly StudentService studentService = new StudentService();
        private readonly MajorService majorService = new MajorService();
        private readonly FacultyService facultyService = new FacultyService();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dgvStudent);

                var listFacultys = facultyService.GetAll();
                var listStudents = studentService.GetAll();

                FillFacultyCombobox(listFacultys);
                BindGrid(listStudents);

                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillFacultyCombobox(List<Faculty> listFacultys)
        {
            listFacultys.Insert(0, new Faculty());
            cmbFaculty.DataSource = listFacultys;
            cmbFaculty.DisplayMember = "FacultyName";
            cmbFaculty.ValueMember = "FacultyID";
        }

        private void BindGrid(List<Student> listStudent)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = item.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = item.FullName;
                if (item.Faculty != null)
                    dgvStudent.Rows[index].Cells[2].Value = item.Faculty.FacultyName;

                dgvStudent.Rows[index].Cells[3].Value = string.Format("{0:F2}", item.AverageScore);

                if (item.MajorID != null)
                    dgvStudent.Rows[index].Cells[4].Value = item.Major.Name;

                ShowAvatar(item.Avatar);
            }
        }

        private void ShowAvatar(string ImageName)
        {
            try
            {
                if (string.IsNullOrEmpty(ImageName))
                {
                    picAvatar.Image = null;
                }
                else
                {
                    string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                    string imagePath = Path.Combine(parentDirectory, "Images", ImageName);
                    picAvatar.Image = Image.FromFile(imagePath);
                    picAvatar.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setGridViewStyle(DataGridView dgview)
        {
            dgview.BorderStyle = BorderStyle.None;
            dgview.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgview.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgview.BackgroundColor = Color.White;
            dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void chkUnregisterMajor_CheckedChanged(object sender, EventArgs e)
        {
            var listStudents = chkUnregisterMajor.Checked ? studentService.GetAllHasNoMajor() : studentService.GetAll();
            BindGrid(listStudents);
        }

        private void btnAddUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(filepath)) return;

            string fileName = $"{tbMa.Text}.jpg";
            using (var bmp = new Bitmap(picAvatar.Image))
            {
                var destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", fileName);
                bmp.Save(destinationPath, ImageFormat.Jpeg);
            }
            filepath = fileName;

            int? facultyId = cmbFaculty.SelectedValue as int?;
            Student student = new Student()
            {
                StudentID = tbMa.Text,
                FullName = tbTen.Text,
                AverageScore = float.Parse(tb_DTB.Text),
                FacultyID = facultyId ?? 0,
                Avatar = filepath
            };

            studentService.InsertUpdate(student);
            MessageBox.Show("Thành công!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            List<Student> students = studentService.GetAll();
            BindGrid(students);

            filepath = "";
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Choose Image (*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";
            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    picAvatar.Image = Image.FromFile(open.FileName);
                    picAvatar.SizeMode = PictureBoxSizeMode.StretchImage;
                    picAvatar.Show();
                    filepath = open.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading the selected image: {ex.Message}", "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    picAvatar.Image = null;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbMa.Text) && studentService.delete(tbMa.Text) == 1)
            {
                MessageBox.Show("Xóa sinh viên thành công!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                List<Student> students = studentService.GetAll();
                BindGrid(students);
            }
            else
            {
                MessageBox.Show("Không có sinh viên cần xóa!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            int idx = e.RowIndex;
            picAvatar.Refresh();
            tbMa.Text = dgvStudent.Rows[idx].Cells[0].Value.ToString();
            tbTen.Text = dgvStudent.Rows[idx].Cells[1].Value.ToString();
            cmbFaculty.Text = dgvStudent.Rows[idx].Cells[2].Value.ToString();
            tb_DTB.Text = dgvStudent.Rows[idx].Cells[3].Value.ToString();
            if (studentService.checkImage(tbMa.Text) == 1)
            {
                picAvatar.Image = null;
            }
            else
            {
                string path = tbMa.Text + ".jpg";
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", path);
                picAvatar.Image = Image.FromFile(imagePath);
                picAvatar.SizeMode = PictureBoxSizeMode.StretchImage;
                picAvatar.Show();
            }
        }

        private void đăngKíChuyênNgànhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DangKiChuyenNganh frm = new DangKiChuyenNganh();
            frm.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            List<Student> students = studentService.GetAll();
            BindGrid(students);
        }
    }
}
