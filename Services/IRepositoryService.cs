public interface IRepositoryService
{
    public IEnumerable<TripDTO> GetTrips();
    public void DeleteClient(int idClient);
    public void AddClientToTrip(ClientDTO client);
}