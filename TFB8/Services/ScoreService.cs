namespace TFB8.Services
{
    using MySql.Data.MySqlClient;
    using System.Collections.Generic;
    using System.Configuration;
    using TFB8.Interfaces;
    using TFB8.Models;

    public class ScoreService : IScoreService
    {
        string connectionString = string.Empty;

        public ScoreService(string conString = null)
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

        public IEnumerable<Score> GetScoresById(int id)
        {
            List<Score> scores = new List<Score>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                using (MySqlCommand command = new MySqlCommand("select s.id as scoreid, d.disciplinename, d.disciplineId, s.score"
                + " from tfb8.scores s"
                + " join tfb8.semesterdisciplines sd  on sd.semesterdisciplinesid = s.semesterdisciplinesid"
                + " join tfb8.discipline d on d.disciplineId = sd.disciplineId"
                + " where s.studentid = (select studentid from tfb8.scores where id = " + id + ")"
                + " and sd.semesterid = (select sd.semesterid"
                + "   from tfb8.scores s"
                + " join tfb8.semesterdisciplines sd on sd.semesterdisciplinesid = s.semesterdisciplinesid"

                + " where s.id = " + id + ")", con))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var score = new Score();
                        int scoreId = (int)reader["scoreid"];
                        string disciplineName = reader["disciplineName"] as string;
                        int disciplineId = (int)reader["disciplineId"];
                        score.Mark = null;

                        if (reader["score"] != System.DBNull.Value)
                        {
                            score.Mark = (int)reader["score"];
                        }

                        score.ScoreId = scoreId;
                        score.Discipline = new Discipline() { DisciplineId = disciplineId, DisciplineName = disciplineName };
                        scores.Add(score);
                    }
                }

                con.Close();
            }

            return scores;
        }

        public void UpdateScores(List<DisciplineScore> disciplineScores)
        {
            foreach (var score in disciplineScores)
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    using (MySqlCommand command = new MySqlCommand(
                        "UPDATE tfb8.scores set score =  @Mark where id = @ScoreId",
                        con))
                    {
                        command.Parameters.Add(new MySqlParameter("ScoreId", score.ScoreId));
                        command.Parameters.Add(new MySqlParameter("Mark", score.Mark));
                        command.ExecuteNonQuery();
                    }

                    con.Close();
                }
            }
        }
    }
}