namespace TFB8.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using Interfaces;
    using MySql.Data.MySqlClient;
    using TFB8.Models;

    public class StudentService : IStudentService
    {
        string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

        public void CreateStudent(Student student)
        {

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                using (MySqlCommand command = new MySqlCommand(
                    "INSERT into tfb8.student values (default, @Name, @Surname, @DateOfBirth)", con))
                {
                    command.Parameters.Add(new MySqlParameter("Name", student.Name));
                    command.Parameters.Add(new MySqlParameter("Surname", student.Surname));
                    command.Parameters.Add(new MySqlParameter("DateOfBirth", student.DateOfBirth));
                    command.ExecuteNonQuery();
                    student.StudentId = command.LastInsertedId;

                }

                con.Close();
            }

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();


                foreach (Semester semester in student.Semesters)
                {
                    try
                    {
                        using (MySqlCommand com = new MySqlCommand(
                            "INSERT into tfb8.scores (studentid, semesterdisciplinesid, score)  Select @StudentId, semesterdisciplinesid, null from tfb8.semesterdisciplines where semesterid = @SemesterId", con))
                        {
                            com.Parameters.Add(new MySqlParameter("StudentId", student.StudentId));
                            com.Parameters.Add(new MySqlParameter("SemesterId", semester.SemesterId));
                            com.ExecuteNonQuery();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                }
                con.Close();
            }
        }

        public IEnumerable<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                using (MySqlCommand command = new MySqlCommand("SELECT  s.studentId, s.name, s.surname, s.dateofbirth, sem.semesterid, sem.name as semestername "
                + "FROM tfb8.student s "
                + "join tfb8.scores sc on sc.studentid = s.studentid "
                + "join tfb8.semesterdisciplines sd on sd.semesterdisciplinesid = sc.semesterdisciplinesid "
                + "join tfb8.semester sem on sem.semesterid = sd.semesterid "
                + "group by sem.semesterid,s.name, s.surname, s.dateofbirth "
                + "order by sem.semesterid, s.name, s.surname", con))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var student = new Student();
                        int studentId = (int)reader["studentId"];
                        string name = reader["name"] as string;
                        string surname = reader["surname"] as string;
                        int semesterId = (int)reader["semesterid"];
                        string semestername = reader["semestername"] as string;
                        DateTime dateofbirth = (DateTime)reader["dateofbirth"];
                        student.StudentId = studentId;
                        student.Name = name;
                        student.Surname = surname;
                        student.DateOfBirth = dateofbirth;
                        student.CurrentSemester = new Semester() { Name = semestername, SemesterId = semesterId };
                        students.Add(student);
                    }
                }

                con.Close();
            }

            return students;

        }
    }
}