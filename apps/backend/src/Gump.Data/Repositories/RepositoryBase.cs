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
			// this only increments the largest id by 1
			// return Collection.AsQueryable().Select(x => x.Id).DefaultIfEmpty(0).Max() + 1;

			// search for all the unused ids and return the first one to use that
			// get all the ids
			var ids = Collection.AsQueryable().Select(x => x.Id).ToList();

			// sort them
			ids.Sort();

			// find the first unused id with linq
			var unusedId = ids.Select((id, index) => new { id, index }).FirstOrDefault(x => x.id != x.index + 1);

			// if there is no unused id, return the max id + 1
			return unusedId == null ? ids.Max() + 1 : unusedId.id;
		}
	}
}
