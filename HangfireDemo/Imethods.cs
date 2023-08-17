using HangfireDemo.Models;
using System.Data;

namespace HangfireDemo
{
    public interface Imethods
    {
        void AddRecipe(Recipe recipe);
        List<Recipe> GetAllRecipes();

        DataTable SyncData();

        void SendEmail();

    }
}
