using System.Collections;
using MongoDB.Driver;
using MongoDbGenericRepository.Attributes;

namespace Gump.Data.Repositories;

public interface IEntity
{
	// The Id property is required for all entities, because it is used to get the next available Id in the GetId() method
	// so all models must implement this interface, sorry
	ulong Id { get; set; }
}

public class RepositoryBase<T> where T : class, IEntity
{
	protected IMongoCollection<T> Collection { get; set; }

	public RepositoryBase(MongoDbConfig mongoDbConfig)
	{
		var dbClient = new MongoClient(mongoDbConfig.ConnectionString);
		var database = dbClient.GetDatabase(mongoDbConfig.Name);

		string collectionName = ((CollectionNameAttribute)Attribute
			.GetCustomAttribute(
				typeof(T),
				typeof(CollectionNameAttribute
			)
		)).Name;

		this.Collection = database.GetCollection<T>(collectionName);
	}

	protected ulong GetId()
	{
		// Get all the Ids of the documents in the collection as a list
		var ids = Collection.AsQueryable().Select(x => x.Id).ToList();
		ids.Sort();

		ulong i = 1;
		foreach (var usedId in ids)
		{
			if (usedId == i)
			{
				i++;
			}
			else if (usedId > i)
			{
				return i;
			}
		}

		return ids[^1] + 1;
	}

	// validation method that checks the given fields for null or whitespace
	// fieldsToCheck should be an array of strings or just a string
	protected void ValidateFields(T entity, params string[] fieldsToCheck)
	{
		foreach (var field in fieldsToCheck)
		{
			var property = entity.GetType().GetProperty(field);

			var value = property.GetValue(entity);

			if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
			{
				if (value == null || ((IList)value).Count == 0)
				{
					throw new RestrictedException($"{field} cannot be empty");
				}
			}
			else
			{
				if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
				{
					throw new RestrictedException($"{field} cannot be empty");
				}
			}
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
			var property = entity.GetType().GetProperty(field);

			if (property == null)
			{
				throw new NotFoundException($"Property {field} does not exist");
			}

			if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
			{
				property.SetValue(entity, Activator.CreateInstance(property.PropertyType));
			}
			else
			{
				property.SetValue(entity, null);
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
			throw new NotFoundException($"{typeof(T).Name.Replace("Model", string.Empty)} with id {id} does not exist");
		}

		ValidateFields(entity, "Id");

		return entity;
	}
}

