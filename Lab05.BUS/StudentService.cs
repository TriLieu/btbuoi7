using Lab05.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Lab05.BUS
{
    public class StudentService
    {
        public List<Student> GetAll()
        {
            Model1 db = new Model1();
            return db.Students.ToList();
        }
        public List<Student> GetAllHasNoMajor()
        {
            Model1 db = new Model1();
            return db.Students.Where(s => s.MajorID == null).ToList();
        }
        public List<Student> GetAllHasNoMajor(int faculty)
        {
            Model1 db = new Model1();
            return db.Students.Where(s => s.FacultyID == faculty && s.MajorID == null).ToList();
        }
        public Student findByID(string id)
        {
            Model1 db = new Model1();
            return db.Students.FirstOrDefault(s => s.StudentID == id);
        }
        public void InsertUpdate(Student s)
        {
            Model1 db = new Model1();
            db.Students.AddOrUpdate(s);
            db.SaveChanges();
        }
        public int delete (string id)
        {
            Model1 db = new Model1();
            var find = db.Students.FirstOrDefault(s=> s.StudentID == id);
            if (find != null)
            {
                db.Students.Remove(find);
                db.SaveChanges();
                return 1;
            }else
            {
                return 0;
            }
        }

        public int checkImage(string id)
        {
            Model1 db = new Model1();
            var find = db.Students.FirstOrDefault(s => s.StudentID == id && string.IsNullOrEmpty(s.Avatar));
            if (find != null)
            {
                return 1;
            }
            return 0;
        }
        public string searchFilePath(string id)
        {
            Model1 db = new Model1();
            Student ss = db.Students.FirstOrDefault(s => s.StudentID == id);
            return ss.Avatar;
        }
    }
}
