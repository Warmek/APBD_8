
public class RepositoryService : IRepositoryService
{
    private ITripRepository _tripRepository = new TripRepository();
    public IEnumerable<TripDTO> GetTrips()
    {
        return _tripRepository.GetTrips();
    }
    public void DeleteClient(int idClient)
    {
        _tripRepository.DeleteClient(idClient);
    }

    public void AddClientToTrip(ClientDTO client)
    {
        _tripRepository.AddClientToTrip(client);
    }
}