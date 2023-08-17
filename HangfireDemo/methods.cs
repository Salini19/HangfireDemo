using HangfireDemo.Models;
using System.Data;
using System.Data.SqlClient;

namespace HangfireDemo
{
    public class methods : Imethods
    {
        private readonly IConfiguration _configuration;
        public methods(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendEmail()
        {
            Console.WriteLine($"SendEmail :Sending email is in process..{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }
        public void AddRecipe(Recipe recipe)
        {
            try
            {

                string connection = _configuration.GetValue<string>("ConnectionStrings:Defaultconnection");
                var query = "Insert into Recipe (RId,RName,RDesc) values (@RId,@RName,@RDesc)";
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("RId", recipe.RId);
                    cmd.Parameters.AddWithValue("RName", recipe.RName);
                    cmd.Parameters.AddWithValue("RDesc", recipe.RDesc);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
            Console.WriteLine($"UpdatedDatabase :Updating the database is in process..{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        public List<Recipe> GetAllRecipes()
        {
            DataTable recipes = SyncData();
            List<Recipe> recipeList= (from DataRow dr in recipes.Rows select new Recipe()
            {
                RId = Convert.ToInt32(dr["RId"]),
                RName = dr["RName"].ToString(),
                RDesc = dr["RDesc"].ToString()
            }).ToList();

            return recipeList;
        }

        public DataTable SyncData()
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:Defaultconnection");
            SqlConnection con = new SqlConnection(connection);
            var query = "SELECT * FROM Recipe";
            con.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            adapter.Fill(ds);

            Console.WriteLine($"SyncData :sync is going on..{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            return ds.Tables[0];
        }
    }
}
