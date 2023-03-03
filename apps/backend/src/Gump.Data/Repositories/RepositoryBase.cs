using MongoDB.Driver;

namespace Gump.Data.Repositories;

public interface IEntity
{
	// The Id property is required for all entities, because it is used to get the next available Id in the GetId() method
	// so all models must implement this interface, sorry
	ulong Id { get; set; }
}

public class RepositoryBase<T> where T : class, IEntity
{
	private readonly MongoClient dbClient;

	protected IMongoCollection<T> Collection { get; set; }

	public RepositoryBase(string connectionString, string databaseName)
	{
		this.dbClient = new MongoClient(connectionString);
		this.Collection = dbClient.GetDatabase(databaseName).GetCollection<T>($"{typeof(T).Name.ToLowerInvariant().Replace("model", string.Empty)}s");
	}

	protected ulong GetId()
	{
		// Get all the Ids of the documents in the collection as a list
		var ids = Collection.AsQueryable().Select(x => x.Id).ToList();
		ids.Sort();

		// Find the first unused Id, by finding the first Id that is not equal to its index + 1
		// For example, if the Ids are [1, 2, 3, 5], then the first unused Id is 4
		var unusedId = ids.Select((id, index) => new { id, index }).FirstOrDefault(x => x.id != (ulong)x.index + 1);

		// Return the first unused Id, or the maximum Id + 1 if there are no unused Ids
		return unusedId == null ? (ulong)ids.Count + 1 : unusedId.id;
	}

	// validation method that checks the given fields for null or whitespace
	// fieldsToCheck should be an array of strings or just a string
	protected void ValidateFields(T entity, params string[] fieldsToCheck)
	{
		foreach (var field in fieldsToCheck)
		{
			var value = entity.GetType().GetProperty(field).GetValue(entity);

			if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
				throw new ArgumentNullException($"{field} cannot be empty");
		}
	}
	protected void ValidateAllFields(T entity)
	{
		ValidateFields(entity, entity.GetType().GetProperties().Select(x => x.Name).ToArray());
	}

	// method to guarantee that given fields are null for the given entity
	protected void NullifyFields(T entity, params string[] fieldsToNullify)
	{
		foreach (var field in fieldsToNullify)
		{
			var prop = entity.GetType().GetProperty(field);
			if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
			{
				prop.SetValue(entity, Activator.CreateInstance(prop.PropertyType));
			}
			else
			{
				prop.SetValue(entity, null);
			}
		}
	}

	// return a model with all the fields except the given fields
	public static T2 CopyExcept<T2>(T2 source, params string[] excludedProperties) where T2 : new()
	{
		var result = new T2();
		var properties = result.GetType().GetProperties();

		foreach (var property in properties)
		{
			// If the property is not in the excluded properties, then copy the value from the source object
			if (!excludedProperties.Contains(property.Name))
			{
				property.SetValue(result, property.GetValue(source));
			}
		}
		return result;
	}

	public IEnumerable<T> GetAll()
	{
		return Collection.AsQueryable();
	}

	public T GetById(ulong id)
	{
		T entity = Collection.AsQueryable().FirstOrDefault(x => x.Id == id);

		if (entity == null)
		{
			throw new ArgumentNullException($"{typeof(T).Name.Replace("Model", string.Empty)} with id {id} does not exist");
		}

		ValidateFields(entity, "Id");

		return entity;

	}
}

