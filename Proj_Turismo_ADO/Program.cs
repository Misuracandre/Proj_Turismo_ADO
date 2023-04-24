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

new PackagesController().FindAll().ForEach(x => Console.WriteLine(x));

Package updatedPackage = new Package();
updatedPackage.Id = 1;
updatedPackage.IdHotel = new Hotel { Id = 2};
updatedPackage.IdTicket = new Ticket { Id = 3};
updatedPackage.IdClient = new Client { Id = 2};
updatedPackage.Value = 100;

new PackagesController().UpdatePackage(updatedPackage);

if (new PackagesController().UpdatePackage(package))
{
    Console.WriteLine("Registros alterados com sucesso!");
}
else
{
    Console.WriteLine("Erro ao atualizar registros.");
}



