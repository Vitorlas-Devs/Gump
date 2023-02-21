using Gump.Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gump.Data.Repositories;

public class CategoryRepository : RepositoryBase<CategoryModel>
{
	public CategoryRepository(MongoClient dbClient) : base(dbClient)
	{
	}

	public CategoryModel Create(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentNullException("Category name cannot be empty");
		}

		if (GetAll().Any(x => x.Name == name))
		{
			throw new ArgumentException("Category already exists", nameof(name));
		}

		CategoryModel category = new CategoryModel
		{
			Id = GetId(),
			Name = name
		};

		try
		{
			Collection.InsertOne(category);
		}
		catch (MongoException ex)
		{
			throw new Exception("Error while creating category", ex);
		}

		return category;
	}

	public string GetById(int id)
	{
		CategoryModel category = Collection.AsQueryable().FirstOrDefault(x => x.Id == id);

		if (category == null)
		{
			throw new DocumentNotFoundException($"Document with id {id} not found.");
		}

		return category.Name;
	}

	public List<CategoryModel> GetAll()
	{
		return Collection.AsQueryable().ToList();
	}

	public void Update(CategoryModel category)
	{
		if (string.IsNullOrWhiteSpace(category.Name))
		{
			throw new ArgumentNullException("Category name cannot be empty");
		}

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
			throw new Exception("Error while updating category", ex);
		}
	}

	public void Delete(int id)
	{
		CategoryModel category = Collection.Find(x => x.Id == id).FirstOrDefault();

		if (category == null)
		{
			throw new DocumentNotFoundException($"Document with id {id} not found.");
		}

		try
		{
			Collection.DeleteOne(x => x.Id == id);
		}
		catch (MongoException ex)
		{
			throw new Exception("Error while deleting category", ex);
		}
	}
}
