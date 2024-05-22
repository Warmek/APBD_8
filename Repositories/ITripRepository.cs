using Zadanie7.Models;

public interface ITripRepository
{
    public IEnumerable<TripDTO> GetTrips();
    public void DeleteClient(int idClient);
    public void AddClientToTrip(ClientDTO client);
}