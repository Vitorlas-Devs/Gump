using MongoDB.Driver;

namespace Gump.Data.Repositories
{
	public interface IEntity
	{
		// The Id property is required for all entities, because it is used to get the next available Id in the GetId() method
		// so all models must implement this interface, sorry
		int Id { get; set; }
	}
	public class RepositoryBase<T> where T : IEntity
	{
		protected readonly MongoClient dbClient;

		protected IMongoCollection<T> Collection => dbClient.GetDatabase("gump").GetCollection<T>(typeof(T).Name.ToLowerInvariant().Replace("Repository", string.Empty));

		public RepositoryBase(string connectionString)
		{
			this.dbClient = new MongoClient(connectionString);
		}

		protected int GetId()
		{
			// Get all the Ids of the documents in the collection as a list
			var ids = Collection.AsQueryable().Select(x => x.Id).ToList();
			ids.Sort();

			// Find the first unused Id, by finding the first Id that is not equal to its index + 1
			// For example, if the Ids are [1, 2, 3, 5], then the first unused Id is 4
			var unusedId = ids.Select((id, index) => new { id, index }).FirstOrDefault(x => x.id != x.index + 1);

			// Return the first unused Id, or the maximum Id + 1 if there are no unused Ids
			return unusedId == null ? ids.Max() + 1 : unusedId.id;
		}
	}
}
