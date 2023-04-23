using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Proj_Turismo_ADO.Models;

namespace Proj_Turismo_ADO.Services
{
    public class AddressesService
    {
        readonly string strConn = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\Users\adm\Documents\Proj_Tourism.mdf;";
        readonly SqlConnection conn;

        public AddressesService()
        {
            conn = new SqlConnection(strConn);
            conn.Open();
        }

        public bool Insert(Address address)
        {
            bool status = false;

            try
            {
                string strInsert = "insert into Address (street, Number, Neighborhood, ZipCode, Extension, IdCity)" + "values (@Street, @Number, @Neightborhood, @ZipCode, @Extension, @IdCity)";

                SqlCommand commandInsert = new SqlCommand(strInsert, conn);

                commandInsert.Parameters.Add(new SqlParameter("@Street", address.Street));
                commandInsert.Parameters.Add(new SqlParameter("@Number", address.Number));
                commandInsert.Parameters.Add(new SqlParameter("@Neighborhood", address.Neighborhood));
                commandInsert.Parameters.Add(new SqlParameter("@ZipCode", address.ZipCode));
                commandInsert.Parameters.Add(new SqlParameter("@Extension", address.Extension));
                commandInsert.Parameters.Add(new SqlParameter("@IdCity", InsertCity(address.IdCity).Id));

                commandInsert.ExecuteScalar();
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

        public City InsertCity(City city)
        {
            bool status = false;

            try
            {
                string strInsert = "insert into City (Description)" + "values (@Description); select cast(scope_identity() as int)";

                SqlCommand commandInsert = new SqlCommand(strInsert, conn);

                commandInsert.Parameters.Add(new SqlParameter("@Description", city.Description));

                city.Id = commandInsert.ExecuteNonQuery();
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
            return city;
        }

        public List<Address> FindAll()
        {
            List<Address> addresses = new();

            StringBuilder sb = new StringBuilder(); 
            sb.Append("select a.Id, ");
            sb.Append("       a.Street, ");
            sb.Append("       a.Number, ");
            sb.Append("       a.Neighborhood, ");
            sb.Append("       a.ZipCode, ");
            sb.Append("       a.Extension, ");
            sb.Append("       c.Description AS CityDescription");
            sb.Append("  from Address a ");
            sb.Append("  JOIN City c ON a.IdCity = c.Id");

            SqlCommand commandSelect = new(sb.ToString(), conn);  
            SqlDataReader dr = commandSelect.ExecuteReader(); 

            while (dr.Read()) 
            {
                Address address = new();



                address.Id = (int)dr["Id"];
                address.Street = (string)dr["Street"];
                address.Number = (int)dr["Number"];
                address.Neighborhood = (string)dr["Neighborhood"];
                address.ZipCode = (string)dr["ZipCode"];
                address.Extension = (string)dr["Extension"];
                address.IdCity = new City() 
                {
                    Id = (int)dr["CityId"],
                    Description = (string)dr["CityDescription"] 
                };

                addresses.Add(address);
            }
            return addresses;
        }
    }
}
