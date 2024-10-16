﻿using Lab05.BUS;
using Lab05.DAL.Eniti;
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

namespace Lab05.GUI
{
    public partial class Form1 : Form
    {
        public readonly StudentService studentService = new StudentService();
        public readonly FacultyService facultyService = new FacultyService();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dgvTableSV);
                var listFacultys = facultyService.GetAll();
                var listStudents = studentService.GetAll();
                FillFalcultyCombobox(listFacultys);
                BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillFalcultyCombobox(List<Faculty> listFacultys)
        {
            listFacultys.Insert(0, new Faculty()); // Đảm bảo có dòng chọn đầu tiên
            this.cbbChuyenNganh.DataSource = listFacultys;
            this.cbbChuyenNganh.DisplayMember = "FacultyName";
            this.cbbChuyenNganh.ValueMember = "FacultyID";
        }

        private void BindGrid(List<Student> listStudent)
        {
            dgvTableSV.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvTableSV.Rows.Add();
                dgvTableSV.Rows[index].Cells[0].Value = item.StudentID;
                dgvTableSV.Rows[index].Cells[1].Value = item.FullName;

                if (item.Faculty != null)
                    dgvTableSV.Rows[index].Cells[2].Value = item.Faculty.FacultyName;

                dgvTableSV.Rows[index].Cells[3].Value = item.AverageScore.ToString();
                if (item.MajorID != null && item.Major != null)
                    dgvTableSV.Rows[index].Cells[4].Value = item.Major.Name;

                ShowAvatar(item.Avatar);
            }
        }

        private void ShowAvatar(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
            {
                pictureAvatar.Image = null;
            }
            else
            {
                string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string imagePath = Path.Combine(parentDirectory, "Images", imageName);
                pictureAvatar.Image = Image.FromFile(imagePath);
                pictureAvatar.Refresh();
            }
        }

        public void setGridViewStyle(DataGridView dgview)
        {
            dgview.BorderStyle = BorderStyle.None;
            dgview.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgview.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgview.BackgroundColor = Color.White;
            dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvTableSV.SelectedRows.Count > 0)
                {
                    var selectedRow = dgvTableSV.SelectedRows[0];
                    var studentID = selectedRow.Cells[0].Value.ToString();

                    // Tìm sinh viên theo StudentID
                    var student = studentService.FindById(studentID);

                    if (student != null)
                    {
                        var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo);
                        if (confirmResult == DialogResult.Yes)
                        {
                            // Gọi phương thức Remove từ đối tượng studentService
                            studentService.Remove(student); // Đảm bảo gọi từ đối tượng đã khởi tạo
                            BindGrid(studentService.GetAll());
                            MessageBox.Show("Xóa sinh viên thành công!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên để xóa.");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn sinh viên để xóa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (dgvTableSV.SelectedRows.Count > 0)
                {
                    var selectedRow = dgvTableSV.SelectedRows[0];
                    var studentID = selectedRow.Cells[0].Value.ToString();

                    // Tìm sinh viên theo StudentID
                    var student = studentService.FindById(studentID);

                    if (student != null)
                    {
                        var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo);
                        if (confirmResult == DialogResult.Yes)
                        {
                            // Gọi phương thức Remove từ đối tượng studentService
                            studentService.Remove(student); // Đảm bảo gọi từ đối tượng đã khởi tạo
                            BindGrid(studentService.GetAll());
                            MessageBox.Show("Xóa sinh viên thành công!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên để xóa.");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn sinh viên để xóa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void chkUnregisterMajor_CheckedChanged(object sender, EventArgs e)
        {
            var listStudents = chkUnregisterMajor.Checked
               ? studentService.GetAllHasNoMajor()
               : studentService.GetAll();

            BindGrid(listStudents);

        }

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Tạo một đối tượng sinh viên mới
                var student = new Student
                {
                    StudentID = txtMSSV.Text,
                    FullName = txtHoten.Text,
                    AverageScore = double.Parse(txtDTB.Text),
                    FacultyID = (int)cbbChuyenNganh.SelectedValue,
                    MajorID = null 
                };

                // Gọi service để thêm sinh viên
                studentService.InsertUpdate(student);

                
                BindGrid(studentService.GetAll());

                // Thông báo thành công
                MessageBox.Show("Thêm sinh viên thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btSsua_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra nếu có dòng được chọn trong DataGridView
                if (dgvTableSV.SelectedRows.Count > 0)
                {
                    // Lấy sinh viên hiện tại từ DataGridView
                    var selectedRow = dgvTableSV.SelectedRows[0];
                    var studentID = selectedRow.Cells[0].Value.ToString();

                    // Tìm sinh viên theo StudentID
                    var student = studentService.FindById(studentID);

                    if (student != null)
                    {
                        // Cập nhật các thông tin sinh viên từ giao diện
                        student.FullName = txtHoten.Text;             
                        student.AverageScore = double.Parse(txtDTB.Text);  
                        student.FacultyID = (int)cbbChuyenNganh.SelectedValue;  
                        student.MajorID = null; 

                        // Gọi phương thức InsertUpdate() để cập nhật thông tin sinh viên
                        studentService.InsertUpdate(student);

                        // Cập nhật lại danh sách sinh viên trên DataGridView
                        BindGrid(studentService.GetAll());

                        // Thông báo thành công
                        MessageBox.Show("Cập nhật sinh viên thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên để sửa.");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn sinh viên để sửa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void pictureAvatar_Click(object sender, EventArgs e)
        {
            try
            {
                
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Chọn ảnh đại diện";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    
                    string selectedImagePath = openFileDialog.FileName;

                    pictureAvatar.Image = Image.FromFile(selectedImagePath);

                    
                    if (dgvTableSV.SelectedRows.Count > 0)
                    {
                        var selectedRow = dgvTableSV.SelectedRows[0];
                        var studentID = selectedRow.Cells[0].Value.ToString();


                        var student = studentService.FindById(studentID);

                        if (student != null)
                        {
                            
                            string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                            string imagesDirectory = Path.Combine(parentDirectory, "Images");
                            if (!Directory.Exists(imagesDirectory))
                            {
                                Directory.CreateDirectory(imagesDirectory);
                            }
                            string newImageName = $"{student.StudentID}{Path.GetExtension(selectedImagePath)}";
                            string destinationPath = Path.Combine(imagesDirectory, newImageName);
                            File.Copy(selectedImagePath, destinationPath, true);

                           
                            student.Avatar = newImageName;
                            studentService.InsertUpdate(student);
                       
                            MessageBox.Show("Cập nhật ảnh đại diện thành công!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật ảnh đại diện: " + ex.Message);
            }
        }
    }
}
