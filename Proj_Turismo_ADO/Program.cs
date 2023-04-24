using Proj_Turismo_ADO.Models;
using Proj_Turismo_ADO.Controllers;

City city = new City()
{
    Description = "Teste"
};

Address address = new Address()
{
    Street = "TesteStreet",
    Number = 1,
    Neighborhood = "TesteNeighborhood",
    ZipCode = "TesteZipCode",
    Extension = "TesteExtension",
    IdCity = city
};

Client client = new Client()
{
    Name = "TesteName",
    Phone = "TestePhone",
    IdAddress = address
};

Hotel hotel = new Hotel()
{
    Name = "TesteName",
    IdAddress = address,
    Value = 50
};

Ticket ticket = new Ticket()
{
    IdOrigin = address,
    IdDestination = address,
    IdClient = client,
    Value = 50
};

Package package = new Package()
{
    IdHotel = hotel,
    IdTicket = ticket,
    IdClient = client,
    Value = 50
};
if(new PackagesController().Insert(package))
{
    Console.WriteLine("Sucesso, registro inserido!");
}
else
{
    Console.WriteLine("Erro ao inserir registro.");
}
