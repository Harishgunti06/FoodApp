using FoodOrderingDataAccessLayer;
using FoodOrderingWebServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Models;

namespace FoodOrderingWebServices.Controllers
{
    [EnableCors("AllowAngularApp")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SupportController : ControllerBase
    {

        private readonly IFoodRepository foodrepo;

        public SupportController(IFoodRepository repo)
        {
            foodrepo = repo;

        }

        [Authorize]
        [HttpGet]
        public ActionResult<List<Issue>> GetIssues()
        {
            try
            {
                var issues = foodrepo.GetAllIssues();
                if (issues == null || issues.Count == 0)
                    return NotFound("No issues found.");
                return Ok(issues);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving issues.");
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult<List<Issue>> GetAllIssues(string email)
        {
            try
            {
                var issues = foodrepo.GetAllIssuesByEmail(email);
                if (issues == null || issues.Count == 0)
                    return NotFound("No issues found.");

                return Ok(issues);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving issues.");
            }
        }

        [Authorize(Roles ="3")]
        [HttpPut]
        public IActionResult UpdateIssueStatus(Models.Issue issue)
        {
            try
            {
                if (issue == null || issue.IssueId <= 0)
                    return BadRequest("Invalid issue data.");
                FoodOrderingDataAccessLayer.Models.Issue issueToUpdate = new FoodOrderingDataAccessLayer.Models.Issue
                {
                    IssueId = issue.IssueId,
                    IssueStatus = issue.IssueStatus
                };
                bool status = foodrepo.UpdateIssueStatus(issueToUpdate.IssueId, issueToUpdate.IssueStatus);
                if (status)
                    return Ok(true);
                else
                    return NotFound("Issue not found.");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while updating issue status: {ex.Message}");
            }
        }

    }
}
