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
    public class HotelsService
    {
        readonly string strConn = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\Users\adm\Documents\Proj_Tourism.mdf;";
        readonly SqlConnection conn;

        public HotelsService()
        {
            conn = new SqlConnection(strConn);
            conn.Open();
        }

        public bool Insert(Hotel hotel)
        {
            bool status = false;

            try
            {
                string strInsert = "insert into Hotel (Name, IdAddress, Value)" + "values (@Name, @IdAddress, @Value)";

                SqlCommand commandInsert = new SqlCommand(strInsert, conn);

                commandInsert.Parameters.Add(new SqlParameter("@Name", hotel.Name));
                commandInsert.Parameters.Add(new SqlParameter("@IdAddress", InsertAddress(hotel.IdAddress).Id));
                commandInsert.Parameters.Add(new SqlParameter("@Value", hotel.Value));

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

        public List<Hotel> FindAll()
        {
            List<Hotel> hotels = new();

            StringBuilder sb = new StringBuilder();
            sb.Append("select h.Id, ");
            sb.Append("       h.Name, ");
            sb.Append("       h.Value, ");
            sb.Append("       a.Street AS AddressStreet,");
            sb.Append("       a.Number AS AddressNumber,");
            sb.Append("       a.Neighborhood AS AddressNeighborhood,");
            sb.Append("       a.ZipCode AS AddressZipCode,");
            sb.Append("       a.Extension  AS AddressExtension,");
            sb.Append("       c.Id AS CityId");
            sb.Append("       c.Description AS CityDescription");
            sb.Append("  from Hotel h, ");
            sb.Append("  JOIN Address a ON h.IdAddress = a.Id");
            sb.Append("  JOIN City c ON a.IdCity = c.Id");

            SqlCommand commandSelect = new(sb.ToString(), conn);
            SqlDataReader dr = commandSelect.ExecuteReader();

            while (dr.Read())
            {
                Hotel hotel = new();

                hotel.Id = (int)dr["Id"];
                hotel.Name = (string)dr["Name"];
                hotel.Value = (int)dr["Value"];
                hotel.IdAddress = new Address()
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

                hotels.Add(hotel);
            }
            return hotels;
        }
    }
}


