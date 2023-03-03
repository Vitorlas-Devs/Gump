using Gump.Data.Models;
using MongoDB.Driver;

namespace Gump.Data.Repositories;

public class PartnerRepository : RepositoryBase<PartnerModel>
{
	public PartnerRepository(string connectionString) : base(connectionString) { }

	public PartnerModel Create(PartnerModel partner)
	{
		if (GetAll().Any(x => x.Name == partner.Name))
		{
			throw new ArgumentException($"Partner already exists with name {partner.Name}");
		}

		partner.Id = GetId();

		ValidateFields(partner, "Name", "ContactUrl");

		try
		{
			Collection.InsertOne(partner);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while creating partner", ex);
		}

		return partner;
	}

	public PartnerModel Update(PartnerModel partner)
	{
		ValidateFields(partner, "Id", "Name", "ContactUrl");

		if (GetAll().Any(x => x.Name == partner.Name && x.Id != partner.Id))
		{
			throw new ArgumentException($"Partner already exists with name {partner.Name}");
		}

		try
		{
			Collection.ReplaceOne(x => x.Id == partner.Id, partner);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while updating partner", ex);
		}

		return partner;
	}

	public void Delete(ulong id)
	{
		try
		{
			Collection.DeleteOne(x => x.Id == id);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while deleting partner", ex);
		}
	}
}
