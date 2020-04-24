using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test1.Model;
using test1.Services;

namespace test1.Controllers
{
    [Route("api/teammember")]
    [ApiController]
    public class TeamMemberController : ControllerBase
    {
        private GetTeamMember task;
        public TeamMemberController(GetTeamMember task)
        {
            this.task = task;
        }

        [HttpGet]
        public IActionResult getTeamMember(int id)
        {
            var result = new TeamMember();
            try
            {
                result = task.getTeamMember(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult deleteProject(string name)
        {
            try
            {
                deleteProject(name);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}