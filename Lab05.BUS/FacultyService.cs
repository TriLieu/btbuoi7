using Lab05.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab05.BUS
{
    public class FacultyService
    {
        public List<Faculty> GetAll()
        {
            Model1 db = new Model1();
            return db.Faculties.ToList();
        }
        public void InsertUpdate(Faculty f)
        {
            Model1 db = new Model1();
            db.Faculties.AddOrUpdate(f);
            db.SaveChanges();
        }
        public void DropFaculty(Faculty f)
        {
            Model1 db = new Model1();
            var find = db.Faculties.Find(f);
            if (find != null)
            {
                db.Faculties.Remove(find);
                db.SaveChanges();
            }else
            {
                return;
            }
        }
    }
}
