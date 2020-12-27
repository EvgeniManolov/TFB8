using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFB8.Models
{
    public class Student
    {
        public long StudentId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Semester CurrentSemester { get; set; }

        public List<Semester> Semesters { get; set; }
    }
}