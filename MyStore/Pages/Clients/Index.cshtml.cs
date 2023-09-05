using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

// This is a Model Page where we write the Logics for our project like Get /Post ,Making of Sql Connection, Making an Interface class for our Models
namespace MyStore.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<ClientInfo> listClients = new List<ClientInfo>();
        private readonly IConfiguration _configuration;

		public IndexModel(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void OnGet()
        {
            try
            {
                String conn = _configuration.GetConnectionString("db");
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    String sql = "SELECT *FROM clients";

                    using (SqlCommand command =new SqlCommand(sql, connection))
                    {
                       using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.id = "" + reader.GetInt32(0);  // Here we used an empty string to convert an integer type to string
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2); 
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address= reader.GetString(4);
                                clientInfo.created_at=  reader.GetDateTime(5).ToString(); // Here we used ToString() method to convert an integer type to string

                                listClients.Add(clientInfo);
                            }
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Exception "+ex.ToString());
            }
        }
    }

    public class ClientInfo
    {
        public String id;
        public String name;
        public String email;
        public String phone;
        public String address;
        public String created_at;

    }
}
