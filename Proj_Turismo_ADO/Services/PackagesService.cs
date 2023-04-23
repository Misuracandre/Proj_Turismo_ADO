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
    public class PackagesService
    {
        readonly string strConn = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\Users\adm\Documents\Proj_Tourism.mdf;";
        readonly SqlConnection conn;

        public PackagesService()
        {
            conn = new SqlConnection(strConn);
            conn.Open();
        }

        public bool Insert(Package package)
        {
            bool status = false;

            try
            {
                string strInsert = "insert into Package (IdHotel, IdTicket, IdClient, Value)" + "values (@IdHotel, @IdTicket, @IdClient, @Value)";

                SqlCommand commandInsert = new SqlCommand(strInsert, conn);

                commandInsert.Parameters.Add(new SqlParameter("@IdHotel", InsertHotel(package.IdHotel).Id));
                commandInsert.Parameters.Add(new SqlParameter("@IdTicket", InsertTicket(package.IdTicket).Id));
                commandInsert.Parameters.Add(new SqlParameter("@IdClient", InsertClient(package.IdClient).Id));
                commandInsert.Parameters.Add(new SqlParameter("@Value", package.Value));

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

        public Hotel InsertHotel(Hotel hotel)
        {
            bool status = false;

            try
            {
                string strInsert = "insert into Hotel (Name, IdAddress, Value)" + "values (@Name, @IdAddress, @Value); select cast (scope_identity() as int)";

                SqlCommand commandInsert = new SqlCommand(strInsert, conn);

                commandInsert.Parameters.Add(new SqlParameter("@Name", hotel.Name));
                commandInsert.Parameters.Add(new SqlParameter("@IdAddress", InsertAddress(hotel.IdAddress).Id));
                commandInsert.Parameters.Add(new SqlParameter("@Value", hotel.Value));

                hotel.Id = commandInsert.ExecuteNonQuery();
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
            return hotel;
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

        public Client InsertClient(Client client)
        {
            bool status = false;

            try
            {
                string strInsert = "insert into Client (Name, Phone, @IdAddress)" + "values (@Name, @Phone, @IdAddress); select cast(scope_identity() as int)";

                SqlCommand commandInsert = new SqlCommand(strInsert, conn);

                commandInsert.Parameters.Add(new SqlParameter("@Name", client.Name));
                commandInsert.Parameters.Add(new SqlParameter("@Phone", client.Phone));
                commandInsert.Parameters.Add(new SqlParameter("@IdAddress", InsertAddress(client.IdAddress).Id));

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

        public Ticket InsertTicket(Ticket ticket)
        {
            bool status = false;

            try
            {
                string strInsert = "insert into Ticket (IdOrigin, IdDestination, IdClient, Value)" + "values (@IdOrigin, @IdDestination, @IdClient, @Value); select cast (scope_identity() as int)";

                SqlCommand commandInsert = new SqlCommand(strInsert, conn);

                commandInsert.Parameters.Add(new SqlParameter("@IdOrigin", InsertAddress(ticket.IdOrigin).Id));
                commandInsert.Parameters.Add(new SqlParameter("@IdDestination", InsertAddress(ticket.IdDestination).Id));
                commandInsert.Parameters.Add(new SqlParameter("@IdClient", InsertClient(ticket.IdClient).Id));
                commandInsert.Parameters.Add(new SqlParameter("@Value", ticket.Value));

                ticket.Id = commandInsert.ExecuteNonQuery();
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
            return ticket;
        }

        public List<Package> FindAll()
        {
            List<Package> packages = new();

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT p.Id, ");
            sb.Append("       p.IdHotel, ");
            sb.Append("       p.IdTicket, ");
            sb.Append("       p.IdClient,");
            sb.Append("       p.Value,");
            sb.Append("       a.Id AS OriginAddressId, a.Street AS OriginStreet, a.Number AS OriginNumber, a.Neighborhood AS OriginNeighborhood, a.ZipCode AS OriginZipCode, a.Extension AS OriginExtension, a.IdCity AS OriginCityId,");
            sb.Append("       b.Id AS DestinationAddressId, b.Street AS DestinationStreet, b.Number AS DestinationNumber, b.Neighborhood AS DestinationNeighborhood, b.ZipCode AS DestinationZipCode, b.Extension AS DestinationExtension, b.IdCity AS DestinationCityId,");
            sb.Append("       c.Id AS ClientId, c.Name, c.Phone,");
            sb.Append("       d.Id AS CityId");
            sb.Append("       d.Description AS CityDescription");
            sb.Append("  FROM Package p ");
            sb.Append("  JOIN Address a ON p.OriginAddressId = a.Id ");
            sb.Append("  JOIN Address b ON p.DestinationAddressId = b.Id ");
            sb.Append("  JOIN City d ON a.IdCity = d.Id ");
            sb.Append("  JOIN Hotel h ON p.IdHotel = h.Id ");
            sb.Append("  JOIN Ticket t ON p.IdTicket = t.Id ");
            sb.Append("  JOIN Client c ON p.IdClient = c.Id ");

            SqlCommand commandSelect = new(sb.ToString(), conn);
            SqlDataReader dr = commandSelect.ExecuteReader();

            while (dr.Read())
            {
                Package package = new();

                package.Id = (int)dr["Id"];
                package.Value = (decimal)dr["Value"];

                package.IdHotel = new Hotel()
                {
                    Id = (int)dr["OriginAddressId"],
                    Name = (string)dr["HotelName"],
                    Value = (int)dr["HotelValue"],
                    IdAddress = new Address()
                    {
                        Id = (int)dr["HotelAddressId"],
                        Street = (string)dr["HotelStreet"],
                        Number = (int)dr["HotelNumber"],
                        Neighborhood = (string)dr["HotelNeighborhood"],
                        ZipCode = (string)dr["HotelZipCode"],
                        Extension = (string)dr["HotelExtension"],
                        IdCity = new City()
                        {
                            Id = (int)dr["HotelCityId"],
                            Description = (string)dr["CityDescription"]
                        }

                    }

                };

                package.IdTicket = new Ticket()
                {
                    Id = (int)dr["OriginAddressId"],
                    IdOrigin = new Address()
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
                    },
                    IdDestination = new Address()
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
                    },
                    IdClient = new Client()
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
                    }
                };

                package.IdClient = new Client()
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

                packages.Add(package);
            }
            return packages;
        }
    }
}
