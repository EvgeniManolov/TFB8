﻿namespace TFB8.Interfaces
{
    using System.Collections.Generic;
    using TFB8.Models;

    public interface IStudentService
    {
        IEnumerable<Student> GetAllStudents();

        void CreateStudent(Student student);

        Student GetStudentById(int id);


        void UpdateStudent(int id, Student student);
    }
}
