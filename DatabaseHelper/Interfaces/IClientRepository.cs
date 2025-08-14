using BMB.Data.Models;

namespace BMB.DatabaseHelper.Interfaces
{
    public interface IClientRepository
    {
        Task<int> AddClient(Client client);
        Task<int> UpdateClient(Client client);
        Task<int> DeleteClient(Client client);
        Task<List<Client>> GetAllClients();
        Task<Client> GetClientById(int id);
    }
}
