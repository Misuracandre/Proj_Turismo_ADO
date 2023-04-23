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
    public class TicketsService
    {
        readonly string strConn = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\Users\adm\Documents\Proj_Tourism.mdf;";
        readonly SqlConnection conn;

        public TicketsService()
        {
            conn = new SqlConnection(strConn);
            conn.Open();
        }

        public bool Insert(Ticket ticket)
        {
            bool status = false;

            try
            {
                string strInsert = "insert into Ticket (IdOrigin, IdDestination, IdClient, Value)" + "values (@IdOrigin, @IdDestination, @IdClient, @Value)";

                SqlCommand commandInsert = new SqlCommand(strInsert, conn);

                commandInsert.Parameters.Add(new SqlParameter("@IdOrigin", InsertAddress(ticket.Origin).Id));
                commandInsert.Parameters.Add(new SqlParameter("@IdDestination", InsertAddress(ticket.Destination).Id));
                commandInsert.Parameters.Add(new SqlParameter("@IdClient", InsertClient(ticket.Client).Id));
                commandInsert.Parameters.Add(new SqlParameter("@Value", ticket.Value));

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
                commandInsert.Parameters.Add(new SqlParameter("@IdCity", InsertCity(address.City).Id));

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

        public Client InsertClient(Client client)
        {
            bool status = false;

            try
            {
                string strInsert = "insert into Client (Name, Phone, @IdAddress)" + "values (@Name, @Phone, @IdAddress); select cast(scope_identity() as int)";

                SqlCommand commandInsert = new SqlCommand(strInsert, conn);

                commandInsert.Parameters.Add(new SqlParameter("@Name", client.Name));
                commandInsert.Parameters.Add(new SqlParameter("@Phone", client.Phone));
                commandInsert.Parameters.Add(new SqlParameter("@IdAddress", InsertAddress(client.Address).Id));

                client.Id = commandInsert.ExecuteNonQuery();
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
            return client;
        }

        public List<Ticket> FindAll()
        {
            List<Ticket> tickets = new();

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT t.Id, ");
            sb.Append("       t.IdOrigin, ");
            sb.Append("       t.IdDestination, ");
            sb.Append("       t.IdClient,");
            sb.Append("       a.Id AS OriginAddressId, a.Street AS OriginStreet, a.Number AS OriginNumber, a.Neighborhood AS OriginNeighborhood, a.ZipCode AS OriginZipCode, a.Extension AS OriginExtension, a.IdCity AS OriginCity,");
            sb.Append("       b.Id AS DestinationAddressId, b.Street AS DestinationStreet, b.Number AS DestinationNumber, b.Neighborhood AS DestinationNeighborhood, b.ZipCode AS DestinationZipCode, b.Extension AS DestinationExtension, b.IdCity AS DestinationCity,");
            sb.Append("       c.Id AS ClientId, c.Name, c.Phone,");
            sb.Append("       d.Description AS CityDescription");
            sb.Append("  FROM Ticket t ");
            sb.Append("  JOIN Address a ON t.IdOrigin = a.Id ");
            sb.Append("  JOIN Address b ON t.IdDestination = b.Id ");
            sb.Append("  JOIN Client c ON t.IdClient = c.Id ");
            sb.Append("  JOIN City d ON a.IdCity = d.Id");

            SqlCommand commandSelect = new(sb.ToString(), conn);
            SqlDataReader dr = commandSelect.ExecuteReader();

            while (dr.Read())
            {
                Ticket ticket = new();

                ticket.Id = (int)dr["Id"];
                ticket.IdOrigin = new Address()
                {
                    Id = (int)dr["OriginAddressId"],
                    Street = (string)dr["OriginStreet"],
                    Number = (int)dr["OriginNumber"],
                    Neighborhood = (string)dr["OriginNeighborhood"],
                    ZipCode = (string)dr["OriginZipCode"],
                    Extension = (string)dr["OriginExtension"],
                    IdCity = new City()
                    {
                        Id = (int)dr["OriginCityId"],
                        Description = (string)dr["CityDescription"]
                    }
                };
                ticket.IdDestination = new Address()
                {
                    Id = (int)dr["DestinationAddressId"],
                    Street = (string)dr["DestinationStreet"],
                    Number = (int)dr["DestinationNumber"],
                    Neighborhood = (string)dr["DestinationNeighborhood"],
                    ZipCode = (string)dr["DestinationZipCode"],
                    Extension = (string)dr["DestinationExtension"],
                    IdCity = new City()
                    {
                        Id = (int)dr["DestinationCityId"],
                        Description = (string)dr["CityDescription"]
                    }
                };
                ticket.IdClient = new Client()
                {
                    Id = (int)dr["ClientId"],
                    Name = (string)dr["Name"],
                    Phone = (string)dr["Phone"],
                    IdAddress = new Address()
                    {
                        Street = (string)dr["OriginStreet"],
                        Number = (int)dr["OriginNumber"],
                        Neighborhood = (string)dr["OriginNeighborhood"],
                        ZipCode = (string)dr["OriginZipCode"],
                        Extension = (string)dr["OriginExtension"],
                        IdCity = new City()
                        {
                            Id = (int)dr["OriginCityId"],
                            Description = (string)dr["CityDescription"]
                        }
                    }
                };

                tickets.Add(ticket);
            }
            return tickets;
        }

        public void UpdateTicket(Ticket ticket)
        {
            using (SqlConnection connection = new SqlConnection(strConn))
            {
                string sql = "UPDATE Ticket SET Id = @Id, IdOrigin = @IdOrigin, IdDestination = @IdDestination, IdClient = @IdClient, Value = @Value WHERE Id = @Id";

                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Id", ticket.Id);
                command.Parameters.AddWithValue("@IdOrigin", ticket.IdOrigin.IdCity);
                command.Parameters.AddWithValue("@IdDestination", ticket.IdDestination.IdCity);
                command.Parameters.AddWithValue("@IdClient", ticket.IdClient.Id);
                command.Parameters.AddWithValue("@Value", ticket.Value);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}


