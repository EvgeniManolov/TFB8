namespace TFB8.Models
{
    using System.Collections.Generic;

    public class AggregateData
    {
        public List<TopStudent> TopStudents { get; set; }

        public List<NoScore> NoScores { get; set; }
    }
}