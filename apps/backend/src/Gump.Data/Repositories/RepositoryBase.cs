using MongoDB.Driver;

namespace Gump.Data.Repositories
{
	public interface IEntity
	{
		int Id { get; set; }
	}
	public class RepositoryBase<T> where T : IEntity
	{
		protected readonly IMongoCollection<T> Collection;

		public RepositoryBase(IMongoDatabase database)
		{
			Collection = database.GetCollection<T>(CollectionName);
		}

		public virtual string CollectionName => throw new NotImplementedException();

		public virtual void CreateIndexes()
		{
			throw new NotImplementedException();
		}

		protected int GetId()
		{
			return Collection.AsQueryable().Select(x => x.Id).DefaultIfEmpty(0).Max() + 1;
		}

	}
}
