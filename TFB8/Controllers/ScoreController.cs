namespace TFB8.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using TFB8.Interfaces;
    using TFB8.Models;
    using TFB8.Services;

    public class ScoreController : ApiController
    {

        IScoreService scoreService = new ScoreService();

        public ScoreController(IScoreService scoreService)
        {
            this.scoreService = scoreService;
        }

        // GET api/<controller>/id
        [HttpGet()]
        public IHttpActionResult Get(int id)
        {
            IHttpActionResult res;
            List<Score> scores = new List<Score>();

            try
            {
                scores = this.scoreService.GetScoresById(id).ToList();
                res = Ok(scores);
            }
            catch (Exception e)
            {
                res = BadRequest();
            }

            return res;
        }

        [HttpPost()]
        public IHttpActionResult Post(List<DisciplineScore> disciplineScore)
        {
            IHttpActionResult res = null;


            try
            {
                this.scoreService.UpdateScores(disciplineScore);
                res = Content(HttpStatusCode.Created, "Successfully created scores!");
            }
            catch (Exception e)
            {
                res = BadRequest();
            }

            return res;
        }

    }
}
