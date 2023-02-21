using MongoDB.Driver;

namespace Gump.Data.Repositories
{
	public interface IEntity
	{
		int Id { get; set; }
	}
	public class RepositoryBase<T> where T : IEntity
	{
		protected readonly MongoClient dbClient;

		protected IMongoCollection<T> Collection => dbClient.GetDatabase("gump").GetCollection<T>(typeof(T).Name.ToLower().Replace("Repository", string.Empty));

		public RepositoryBase(MongoClient dbClient)
		{
			this.dbClient = dbClient;
		}

		protected int GetId()
		{
			var ids = Collection.AsQueryable().Select(x => x.Id).ToList();
			ids.Sort();
			var unusedId = ids.Select((id, index) => new { id, index }).FirstOrDefault(x => x.id != x.index + 1);
			return unusedId == null ? ids.Max() + 1 : unusedId.id;
		}
	}
}
