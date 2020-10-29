using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _context;
        public BuggyController(StoreContext context)
        {
            _context = context;
        }
        
        [HttpGet("testauth")]
        [Authorize(Roles = "Member")]
        public ActionResult<string> GetSecretText()
        {
            return "secret stuff";
        }

        [HttpGet("moderate")]
        [Authorize(Policy = "ModerateProductRole")]
        public ActionResult<string> ModerateProductRolet()
        {
            return "secret stuff moderate";
        }

        [HttpGet("admin")]
        [Authorize(Policy = "RequiredAdminRole")]
        public ActionResult<string> GetSecretTextWithAdmin()
        {
            return "secret stuff admin";
        }

        [AllowAnonymous]
        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequrest()
        {
            var thing = _context.Products.Find(42);
            if (thing == null) 
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var thing = _context.Products.Find(42);
            var thingToReturn = thing.ToString();
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }

        [AllowAnonymous]
        [HttpGet("badrequest/{id}")]
        public ActionResult GetNotFoundRequest(int id)
        {
            return Ok();
        }
    }
}