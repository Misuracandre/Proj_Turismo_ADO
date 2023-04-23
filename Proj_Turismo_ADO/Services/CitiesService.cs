using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proj_Turismo_ADO.Models;

namespace Proj_Turismo_ADO.Services
{
    public class CitiesService
    {
        readonly string strConn = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\Users\adm\Documents\Proj_Tourism.mdf;";
        readonly SqlConnection conn;

        public CitiesService()
        {
            conn = new SqlConnection(strConn);
            conn.Open();
        }

        public bool Insert(City city)
        {
            bool status = false;

            try
            {
                string strInsert = "insert into City (Description)" + "values (@Description)";

                SqlCommand commandInsert = new SqlCommand(strInsert, conn);

                commandInsert.Parameters.Add(new SqlParameter("@Description", city.Description));

                commandInsert.ExecuteNonQuery();
                status = true;
            }
            catch (Exception)
            {
                status = false;
                throw;
            }
            finally
            {
                conn.Close();
            }
            return status;
        }

        public List<City> FindAll()
        {
            List<City> cities = new();

            StringBuilder sb = new StringBuilder(); 
            sb.Append("select c.Id, ");
            sb.Append("       c.Description, ");
            sb.Append("  from City c ");

            SqlCommand commandSelect = new(sb.ToString(), conn); 
            SqlDataReader dr = commandSelect.ExecuteReader(); 

            while (dr.Read()) 
            {
                City city = new();

                city.Id = (int)dr["Id"];
                city.Description = (string)dr["Description"];

                cities.Add(city);
            }
            return cities;
        }

        public void UpdateCity(City city)
        {
            using (SqlConnection connection = new SqlConnection(strConn))
            {
                string sql = "UPDATE City SET Id = @Id, Description = @Description WHERE Id = @Id";

                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Id", city.Id);
                command.Parameters.AddWithValue("@Description", city.Description);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
