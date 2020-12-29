namespace TFB8.Services
{
    using MySql.Data.MySqlClient;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using TFB8.Interfaces;
    using TFB8.Models;

    public class AggregateDataService : IAggregateDataService
    {
        string connectionString = string.Empty;

        public AggregateDataService(string conString = null)
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
        public AggregateData GetAllAggregateData()
        {

            AggregateData aggregateData = new AggregateData();
            aggregateData.TopStudents = new List<TopStudent>();
            aggregateData.NoScores = new List<NoScore>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                using (MySqlCommand command = new MySqlCommand("SELECT sem.name as semestername,concat(st.name, ' ', st.surname) as studentname,"
                + " cast(sum(sc.score) / count(sc.semesterdisciplinesid) as double) as averageScore"
                + " from tfb8.student st"
                + " join tfb8.scores sc on sc.studentid = st.studentid"
                + " join tfb8.semesterdisciplines sd on sd.semesterdisciplinesid = sc.semesterdisciplinesid"
                + " join tfb8.semester sem on sem.semesterid = sd.semesterid"
                + " where sc.score is not null"
                + " group by sd.semesterId, st.studentid, concat(st.name, '', st.surname)"
                + " order by sd.semesterId, sum(sc.score) / count(sc.semesterdisciplinesid) desc"
                + " limit 10 ", con))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var topStudent = new TopStudent();
                        string studentname = reader["studentname"] as string;
                        string semestername = reader["semestername"] as string;
                        double averageScore = (double)reader["averageScore"];
                        topStudent.StudentName = studentname;
                        topStudent.AverageScore = averageScore;
                        topStudent.SemesterName = semestername;
                        aggregateData.TopStudents.Add(topStudent);
                    }
                }

                using (MySqlCommand command = new MySqlCommand("SELECT"
                + " s.name as semestername,"
                + " concat(st.name, ' ', st.surname) as studentname,"
                + " group_concat(d.disciplinename) as disciplines"
                + " from tfb8.student st"
                + " join tfb8.scores sc on sc.studentid = st.studentid"
                + " join tfb8.semesterdisciplines sd on sd.semesterdisciplinesid = sc.semesterdisciplinesid"
                + " join tfb8.discipline d on d.disciplineid = sd.disciplineid"
                + " join tfb8.semester s on s.semesterid = sd.semesterid"
                + " where sc.score is null"
                + " group by s.semesterid, st.studentid"
                + " order by st.name, d.disciplinename", con))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var noScoreStudent = new NoScore();
                        string semestername = reader["semestername"] as string;
                        string studentname = reader["studentname"] as string;
                        string disciplines = reader["disciplines"] as string;
                        noScoreStudent.SemesterName = semestername;
                        noScoreStudent.StudentName = studentname;
                        noScoreStudent.DisciplinesName = disciplines;
                        aggregateData.NoScores.Add(noScoreStudent);
                    }
                }

                con.Close();
            }

            return aggregateData;
        }
    }
}