using Gump.Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gump.Data.Repositories;

public class CategoryRepository : RepositoryBase<CategoryModel>
{
	public CategoryRepository(string connectionString) : base(connectionString) { }

	public CategoryModel Create(string name)
	{
		if (GetAll().Any(x => x.Name == name))
		{
			throw new ArgumentException("Category already exists", nameof(name));
		}

		CategoryModel category = new CategoryModel
		{
			Id = GetId(),
			Name = name
		};

		ValidateFields(category, "Name");

		try
		{
			Collection.InsertOne(category);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while creating category", ex);
		}

		return category;
	}

	public string GetById(ulong id)
	{
		CategoryModel category = Collection.AsQueryable().FirstOrDefault(x => x.Id == id);

		if (category == null)
		{
			throw new ArgumentNullException($"Category with id {id} does not exist");
		}

		ValidateFields(category, "Id");

		return category.Name;
	}

	public List<CategoryModel> GetAll()
	{
		return Collection.AsQueryable().ToList();
	}

	public void Update(CategoryModel category)
	{
		ValidateAllFields(category);

		if (GetAll().Any(x => x.Name == category.Name))
		{
			throw new ArgumentException("Category already exists", nameof(category.Name));
		}

		try
		{
			Collection.ReplaceOne(x => x.Id == category.Id, category);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while updating category", ex);
		}
	}

	public void Delete(ulong id)
	{
		CategoryModel category = Collection.Find(x => x.Id == id).FirstOrDefault();

		ValidateFields(category, "Id");

		try
		{
			Collection.DeleteOne(x => x.Id == id);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while deleting category", ex);
		}
	}
}
