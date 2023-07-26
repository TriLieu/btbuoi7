using Lab05.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab05.BUS
{
    public class MajorService
    {
        public List<Major> GetAllByFaulty(int facultyID)
        {
            Model1 db = new Model1();
            return db.Majors.Where(s=> s.FacultyID == facultyID).ToList();
        }
    }
}
