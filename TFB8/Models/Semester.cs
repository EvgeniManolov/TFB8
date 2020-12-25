
namespace TFB8.Models
{
    using System;
    using System.Collections.Generic;

    public class Semester
    {
        public int SemesterId { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string DisciplinesAsString { get; set; }

        public int DisciplineId { get; set; }

        public List<Discipline> Disciplines { get; set; }
    }
}