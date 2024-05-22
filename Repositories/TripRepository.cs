
using System.Linq;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Zadanie7.Models;

public class TripRepository : ITripRepository
{
    private MasterContext _context = new MasterContext();
    public TripRepository()
    {
    }

    public void AddClientToTrip(ClientDTO client)
    {
        using (var transaction = new TransactionScope())
        {
            try
            {
                if (!_context.Clients.Where(c => c.Pesel == client.Pesel).Any())
                {
                    int id_max = _context.Clients.Max(c => c.IdClient) + 1;

                    Client toAdd = new Client
                    {
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        Email = client.Email,
                        Telephone = client.Telephone,
                        Pesel = client.Pesel,
                        IdClient = id_max
                    };
                    _context.Clients.Add(toAdd);
                    _context.SaveChanges();
                }

                Client test = _context.Clients.Where(c => c.Pesel == client.Pesel).FirstOrDefault();

                int id = _context.Clients.Where(c => c.Pesel == client.Pesel).FirstOrDefault().IdClient;

                if (_context.ClientTrips.Where(t => t.IdTrip == client.IdTrip && t.IdClient == id).Any())
                {
                    throw new Exception("Ten klient jest już przypisany do tej wycieczki");
                }


                if (!_context.Trips.Where(t => t.IdTrip == client.IdTrip).Any())
                {
                    throw new Exception("Nie ma wycieczki o podanym id");
                }

                _context.ClientTrips.Add(new ClientTrip
                {
                    IdTrip = client.IdTrip,
                    IdClient = id,
                    PaymentDate = client.PaymentDate,
                    RegisteredAt = DateTime.Now
                });

                _context.SaveChanges();

                transaction.Complete();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

    }

    public void DeleteClient(int idClient)
    {
        if (_context.ClientTrips.Any(c => c.IdClient == idClient))
        {
            throw new Exception("Ten klient ma przypisane wycieczki");
        }

        _context.Clients.Where(c => c.IdClient == idClient).ExecuteDelete();
        _context.SaveChanges();
    }

    public IEnumerable<TripDTO> GetTrips()
    {
        List<TripDTO> ans = new List<TripDTO>();

        IEnumerable<Trip> trips = _context.Trips.Select(t => t);

        foreach (Trip trip in trips)
        {
            TripDTO tripDTO = new TripDTO();
            tripDTO.Name = trip.Name;
            tripDTO.Description = trip.Description;
            tripDTO.DateFrom = trip.DateFrom;
            tripDTO.DateTo = trip.DateTo;
            tripDTO.MaxPeople = trip.MaxPeople;

            tripDTO.Countries = _context.Countries.Where(c => c.IdTrips.Contains(trip)).Select(c => c.Name);

            IEnumerable<int> client_ids = _context.ClientTrips.Where(c => c.IdTrip == trip.IdTrip).Select(c => c.IdClient);


            tripDTO.Clients = _context.Clients.Where(c => client_ids.Contains(c.IdClient)).Select(c => new { FirstName = c.FirstName, LastName = c.LastName });

            

            ans.Add(tripDTO);
        }

        return ans.OrderByDescending(t => t.DateFrom);
    }

}