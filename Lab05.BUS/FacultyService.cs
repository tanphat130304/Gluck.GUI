using Lab05.DAL.Eniti;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Lab05.BUS
{
    public class FacultyService
    {
        public List<Faculty> GetAll()
        {
            ModelSinhVienDB modelSinhVienDB = new ModelSinhVienDB();
            return modelSinhVienDB.Faculties.ToList();
        }
    }
}
