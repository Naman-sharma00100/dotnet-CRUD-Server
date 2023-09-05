using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo ClientInfo = new ClientInfo();
		public String errorMessage = "";

		public String successMessage = "";
		private readonly IConfiguration _configuration;	

		public CreateModel(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void OnGet()
        {
        }

		// Here everything wheteher name,email etc we are going to use will symbolise the name field in out Form Name attribute

		// Request.Form[] is used to access data that has been submitted to the server from an HTML form using the HTTP POST method. It allows you to retrieve values of form fields or controls that were posted back to the server as part of a form submission.
		public void OnPost()
		{
			// Actual variable values to be pushed in database when we will be saving them
            ClientInfo.name = Request.Form["name"];
			ClientInfo.email = Request.Form["email"];
			ClientInfo.phone = Request.Form["phone"];
			ClientInfo.address = Request.Form["address"];


			if (ClientInfo.name.Length == 0 || ClientInfo.email.Length == 0 || ClientInfo.phone.Length == 0 || ClientInfo.address.Length == 0)
			{
				errorMessage = "All Fields are Required";
				return;

			}

			// Save the new item into Database


			try {
				String connectionString = _configuration.GetConnectionString("db");


				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					// Writing sql query to push values into database 

					String sql = " INSERT INTO clients " +
						"(name,email,phone,address) VALUES" + "(@name,@email,@phone,@address);"; // @name,@address etc. are the field name that are to be saved with that name only ,which is Given in our Database

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@name", ClientInfo.name);
						command.Parameters.AddWithValue("@email", ClientInfo.email);
						command.Parameters.AddWithValue("@phone", ClientInfo.phone);
						command.Parameters.AddWithValue("@address", ClientInfo.address);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}
			// After adding of successful addtion of client details, we al so need to empty the client details

			ClientInfo.name = "";
			ClientInfo.email = "";
			ClientInfo.phone = "";
			ClientInfo.address = "";

			successMessage = " New Client  Added Successfully";
			Response.Redirect("/Clients/index");


		}
	}
}
