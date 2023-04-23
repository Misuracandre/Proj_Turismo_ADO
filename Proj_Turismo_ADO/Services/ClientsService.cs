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
    public class ClientsService
    {
        readonly string strConn = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\Users\adm\Documents\Proj_Tourism.mdf;";
        readonly SqlConnection conn;

        public ClientsService()
        {
            conn = new SqlConnection(strConn);
            conn.Open();
        }

        public bool Insert(Client client)
        {
            bool status = false;

            try
            {
                string strInsert = "insert into Client (Name, Phone, @IdAddress)" + "values (@Name, @Phone, @IdAddress)";

                SqlCommand commandInsert = new SqlCommand(strInsert, conn);

                commandInsert.Parameters.Add(new SqlParameter("@Name", client.Name));
                commandInsert.Parameters.Add(new SqlParameter("@Phone", client.Phone));
                commandInsert.Parameters.Add(new SqlParameter("@IdAddress", InsertAddress(client.IdAddress).Id));

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

        public Address InsertAddress(Address address)
        {
            bool status = false;

            try
            {
                string strInsert = "insert into Address (street, Number, Neighborhood, ZipCode, Extension, IdCity)" + "values (@Street, @Number, @Neightborhood, @ZipCode, @Extension, @IdCity); select cast(scope_identity() as int)";

                SqlCommand commandInsert = new SqlCommand(strInsert, conn);

                commandInsert.Parameters.Add(new SqlParameter("@Street", address.Street));
                commandInsert.Parameters.Add(new SqlParameter("@Number", address.Number));
                commandInsert.Parameters.Add(new SqlParameter("@Neighborhood", address.Neighborhood));
                commandInsert.Parameters.Add(new SqlParameter("@ZipCode", address.ZipCode));
                commandInsert.Parameters.Add(new SqlParameter("@Extension", address.Extension));
                commandInsert.Parameters.Add(new SqlParameter("@IdCity", InsertCity(address.IdCity).Id));

                address.Id = commandInsert.ExecuteNonQuery();
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
            return address;
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

        public List<Client> FindAll()
        {
            List<Client> clients = new();

            StringBuilder sb = new StringBuilder();
            sb.Append("select c.Id, ");
            sb.Append("       c.Name, ");
            sb.Append("       c.Phone, ");
            sb.Append("       a.Street AS AddressStreet,");
            sb.Append("       a.Number AS AddressNumber,");
            sb.Append("       a.Neighborhood AS AddressNeighborhood,");
            sb.Append("       a.ZipCode AS AddressZipCode,");
            sb.Append("       a.Extension AS AddressExtension,");
            sb.Append("       s.Id AS CityId");
            sb.Append("       s.Description AS CityDescription");
            sb.Append("  from Client c, ");
            sb.Append("  JOIN Address a ON c.IdAddress = a.Id");
            sb.Append("  JOIN City s ON a.IdCity = s.Id");

            SqlCommand commandSelect = new(sb.ToString(), conn);
            SqlDataReader dr = commandSelect.ExecuteReader();

            while (dr.Read())
            {
                Client client = new();

                client.Id = (int)dr["Id"];
                client.Name = (string)dr["Name"];
                client.Phone = (string)dr["Phone"];
                client.IdAddress = new Address()
                {
                    Street = (string)dr["AddressStreet"],
                    Number = (int)dr["AddressNumber"],
                    Neighborhood = (string)dr["AddressNeighborhood"],
                    ZipCode = (string)dr["AddressZipCode"],
                    Extension = (string)dr["AddressExtension"],
                    IdCity = new City()
                    {
                        Id = (int)dr["CityId"],
                        Description = (string)dr["CityDescription"]
                    }
                };

                clients.Add(client);
            }
            return clients;
        }

        public void UpdateClient(Client client)
        {
            using (SqlConnection connection = new SqlConnection(strConn))
            {
                string sql = "UPDATE Client SET Id = @Id, Name = @Name, Phone = @Phone, IdAddress = @IdAddress WHERE Id = @Id";

                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Id", client.Id);
                command.Parameters.AddWithValue("@Name", client.Name);
                command.Parameters.AddWithValue("@Phone", client.Phone);
                command.Parameters.AddWithValue("@IdAddress", client.IdAddress.Id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteClient(int id)
        {
            using (SqlConnection connection = new SqlConnection(strConn))
            {
                string citySql = "DELETE FROM Client WHERE Id = @Id";

                SqlCommand command = new SqlCommand(citySql, connection);

                command.Parameters.AddWithValue("Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
