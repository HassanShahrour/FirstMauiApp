using BMB.Data.Models;
using BMB.DatabaseHelper.Interfaces;

namespace BMB.DatabaseHelper.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IAppDatabase _db;

        public ClientRepository(IAppDatabase db)
        {
            _db = db;
        }

        public async Task<int> AddClient(Client client)
        {
            try
            {
                return await _db.AddAsync(client);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding client", ex);
            }
        }

        public async Task<int> UpdateClient(Client client)
        {
            try
            {
                return await _db.UpdateAsync(client);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error updating client", ex);
            }
        }

        public async Task<int> DeleteClient(Client client)
        {
            try
            {
                return await _db.DeleteAsync(client);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error deleting client", ex);
            }
        }

        public async Task<List<Client>> GetAllClients()
        {
            try
            {
                return await _db.GetAllAsync<Client>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving clients", ex);
            }
        }

        public async Task<Client> GetClientById(int id)
        {
            try
            {
                return await _db.GetByIdAsync<Client>(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error retrieving client with ID {id}", ex);
            }
        }
    }
}
