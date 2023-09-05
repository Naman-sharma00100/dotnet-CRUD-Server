using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class EditModel : PageModel
    {

        public ClientInfo ClientInfo= new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";

		private readonly IConfiguration _configuration;

		public EditModel(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void OnGet()
        {
			String id = Request.Query["id"];


			try
			{
				String connectionString = _configuration.GetConnectionString("db");


				using (SqlConnection connection = new SqlConnection(connectionString))
				{

					// This block is then meant to write any businness logic acc. to our project and hence interact with our Sql Server Database
					connection.Open();

					// Sql Query to perfrom updates on id being selected 
					String sql = "SELECT * FROM clients WHERE id=@id";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id", id); // allowing us to retrieve data based on the id value passed to it.
						using (SqlDataReader reader = command.ExecuteReader())
						{
							// Here we have read the data of that pareticualr user to save them in a variables, so that w ecan use them in further business logic like Post method
							if (reader.Read())
							{
								ClientInfo.id = "" + reader.GetInt32(0);
								ClientInfo.name = reader.GetString(1);
								ClientInfo.email = reader.GetString(2);
								ClientInfo.phone = reader.GetString(3);
								ClientInfo.address = reader.GetString(4);

							}
						}

					}
				}

			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
			}
		}

        public void OnPost() {


			ClientInfo.id = Request.Form["id"];
			ClientInfo.name = Request.Form["name"];
			ClientInfo.email= Request.Form["email"];
			ClientInfo.phone = Request.Form["phone"];
			ClientInfo.address= Request.Form["address"];

			if ( ClientInfo.email.Length == 0 || ClientInfo.phone.Length == 0 || ClientInfo.address.Length == 0)
			{
				errorMessage = " All the Field are Required...";
				return;
			}

			// Again making the connection,to finally save our post method changes in our database.

			try
			{
				String connectionString = _configuration.GetConnectionString("db");

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					// Now Our Sql QUERY IN THIS CASE WILL be to put the Updated Values ,i.e ,By using UPDATE & SET sql query with values always with @
					String sql = "UPDATE clients SET name=@name ,email=@email, phone=@phone,address=@address WHERE id=@id";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@name", ClientInfo.name);
						command.Parameters.AddWithValue("@email", ClientInfo.email);
						command.Parameters.AddWithValue("@phone", ClientInfo.phone);
						command.Parameters.AddWithValue("@address", ClientInfo.address);
						command.Parameters.AddWithValue("@id", ClientInfo.id);

						command.ExecuteNonQuery();
					}
				}
				

			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}


			Response.Redirect("/Clients/Index");
       
		}
    }
}
