using Hangfire.Logging;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using HangfireDemo.Models;

namespace HangfireDemo.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class RecipeeController : Controller
    {
        public static List<Recipe> recipes = new List<Recipe>();
        private readonly Imethods imethods;

        public RecipeeController(Imethods imethods)
        {
            this.imethods = imethods;
            
        }
        [HttpPost]
        public IActionResult AddRecipe(Recipe recipe)
        {
            if (ModelState.IsValid)
            {
               recipes.Add(recipe);
                imethods.AddRecipe(recipe);
               
                BackgroundJob.Enqueue<Imethods>(x => x.SendEmail());
                return CreatedAtAction("GetRecipe", new { recipe.RId }, recipe);
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult GetRecipe(int id)
        {
            var recipes = imethods.GetAllRecipes();
          
            var recipe = recipes.FirstOrDefault(x => x.RId == id);
            if (recipe == null)
                return NotFound();
            BackgroundJob.Enqueue<Imethods>(x => x.SyncData());
            return Ok(recipe);
        }
    }
}
