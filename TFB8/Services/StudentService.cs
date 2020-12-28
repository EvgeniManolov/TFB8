namespace TFB8.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Interfaces;
    using MySql.Data.MySqlClient;
    using TFB8.Models;

    public class StudentService : IStudentService
    {
        string connectionString = string.Empty;

        public StudentService(string conString = null)
        {
            if (conString != null)
            {
                this.connectionString = conString;
            }
            else
            {
                this.connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            }
        }

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


                try
                {
                    foreach (Semester semester in student.Semesters)
                    {
                        using (MySqlCommand com = new MySqlCommand(
                            "INSERT into tfb8.scores (studentid, semesterdisciplinesid, score)  Select @StudentId, semesterdisciplinesid, null from tfb8.semesterdisciplines where semesterid = @SemesterId", con))
                        {
                            com.Parameters.Add(new MySqlParameter("StudentId", student.StudentId));
                            com.Parameters.Add(new MySqlParameter("SemesterId", semester.SemesterId));
                            com.ExecuteNonQuery();
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
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

                using (MySqlCommand command = new MySqlCommand("SELECT  s.studentId, s.name, s.surname, s.dateofbirth, sem.semesterid, sem.name as semestername, sc.id as scoreid  "
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
                        int scoreId = (int)reader["scoreid"];
                        string semestername = reader["semestername"] as string;
                        DateTime dateofbirth = (DateTime)reader["dateofbirth"];
                        student.StudentId = studentId;
                        student.Name = name;
                        student.Surname = surname;
                        student.ScoreId = scoreId;
                        student.DateOfBirth = dateofbirth;
                        student.CurrentSemester = new Semester() { Name = semestername, SemesterId = semesterId };
                        students.Add(student);
                    }
                }

                con.Close();
            }

            return students;

        }

        public Student GetStudentById(int id)
        {
            Student student = new Student();
            student.Semesters = new List<Semester>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string sqlCommand =
                    "SELECT  s.studentid,  s.name, s.surname, s.dateofbirth, group_concat(distinct (sd.semesterid)) as semesterids"
                + " FROM tfb8.student s"
                    + " join tfb8.scores sc on sc.studentid = s.studentid"
                    + " join tfb8.semesterdisciplines sd on sd.semesterdisciplinesid = sc.semesterdisciplinesid"
                    + " where s.studentid = " + id
                    + " group by s.studentid; ";
                using (MySqlCommand command = new MySqlCommand(sqlCommand, con))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int studentId = (int)reader["studentid"];
                        string studentname = reader["name"] as string;
                        string surname = reader["surname"] as string;
                        DateTime dateofbirth = (DateTime)reader["dateofbirth"];
                        student.StudentId = studentId;
                        student.Name = studentname;
                        student.Surname = surname;
                        student.DateOfBirth = dateofbirth;

                        string semesterIds = (string)reader["semesterids"];
                        var semesterIdsArray = semesterIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToArray();

                        foreach (var semesterId in semesterIdsArray)
                        {
                            var semester = new Semester() { SemesterId = int.Parse(semesterId) };
                            student.Semesters.Add(semester);
                        }
                    }
                }

                con.Close();
            }

            return student;
        }

        public void UpdateStudent(int id, Student student)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                using (MySqlCommand command = new MySqlCommand(
                    "UPDATE  tfb8.student set name =  @Name, surname = @Surname, dateofbirth = @DateOfBirth where studentid = @StudentId", con))
                {
                    command.Parameters.Add(new MySqlParameter("Name", student.Name));
                    command.Parameters.Add(new MySqlParameter("Surname", student.Surname));
                    command.Parameters.Add(new MySqlParameter("DateOfBirth", student.DateOfBirth));
                    command.Parameters.Add(new MySqlParameter("StudentId", id));
                    command.ExecuteNonQuery();
                }

                List<int> dbSemesterIds = new List<int>();
                using (MySqlCommand command = new MySqlCommand(
                    "select distinct sd.semesterid as semesterid "
                    + " from tfb8.scores s"
                    + " join tfb8.semesterdisciplines sd on sd.semesterdisciplinesid = s.semesterdisciplinesid"
                    + " where s.studentid = " + id, con))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int semesterid = (int)reader["semesterid"];
                        dbSemesterIds.Add(semesterid);
                    }
                }

                List<int> semesterIds = new List<int>();
                foreach (var semester in student.Semesters)
                {
                    if (!dbSemesterIds.Contains(semester.SemesterId))
                    {
                        using (MySqlCommand com = new MySqlCommand(
                            "INSERT into tfb8.scores (studentid, semesterdisciplinesid, score)  Select @StudentId, semesterdisciplinesid, null from tfb8.semesterdisciplines where semesterid = @SemesterId", con))
                        {
                            com.Parameters.Add(new MySqlParameter("StudentId", id));
                            com.Parameters.Add(new MySqlParameter("SemesterId", semester.SemesterId));
                            com.ExecuteNonQuery();
                        }
                    }
                    semesterIds.Add(semester.SemesterId);
                }

                foreach (var dbsemesterid in dbSemesterIds)
                {
                    if (!semesterIds.Contains(dbsemesterid))
                    {
                        //Delete
                        //Проверка даи към съответната семестър-дисциплината няма оценки

                        using (MySqlCommand command = new MySqlCommand(
                            "select 1 from tfb8.scores s" +
                            "           join tfb8.semesterdisciplines sd on sd.semesterdisciplinesid = s.semesterdisciplinesid" +
                            "            where s.score is not null and sd.semesterid = " + dbsemesterid, con))
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                throw new Exception("Can not remove semester because there are students score on discipline assigned to semester!");
                            }
                        }

                        using (MySqlCommand command = new MySqlCommand(
                            "DELETE from tfb8.scores where semesterdisciplinesid in (select semesterdisciplinesid from tfb8.semesterdisciplines where semesterid = @semesterid)", con))
                        {
                            command.Parameters.Add(new MySqlParameter("SemesterId", dbsemesterid));
                            command.ExecuteNonQuery();
                        }

                        using (MySqlCommand command = new MySqlCommand(
                            "DELETE from tfb8.semesterdisciplines where semesterid = @Semesterid", con))
                        {
                            command.Parameters.Add(new MySqlParameter("Semesterid", dbsemesterid));
                            command.ExecuteNonQuery();
                        }
                    }
                }
                con.Close();
            }
        }
    }
}