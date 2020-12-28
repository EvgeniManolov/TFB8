namespace TFB8.Interfaces
{
    using System.Collections.Generic;
    using TFB8.Models;

    public interface IScoreService
    {
        IEnumerable<Score> GetScoresById(int id);

        void UpdateScores(List<DisciplineScore> disciplineScores);
    }
}
