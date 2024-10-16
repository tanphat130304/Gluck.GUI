﻿using Lab05.DAL.Eniti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab05.BUS
{
    public class MajorService
    {
        public List<Major> GetAllByFaculty(int facultyId)
        {
            ModelSinhVienDB modelSinhVienDB = new ModelSinhVienDB();
            return modelSinhVienDB.Majors.Where(p => p.FacultyID == facultyId).ToList();
        }
    }
}

