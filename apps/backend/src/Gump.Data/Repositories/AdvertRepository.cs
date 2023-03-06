using Gump.Data.Models;
using MongoDB.Driver;

namespace Gump.Data.Repositories;

public class AdvertRepository : RepositoryBase<AdvertModel>
{
	private readonly string connectionString;
	private readonly string databaseName;

	private ImageRepository ImageRepository => new(connectionString, databaseName);
	private PartnerRepository PartnerRepository => new(connectionString, databaseName);

	public AdvertRepository(string connectionString, string databaseName) : base(connectionString, databaseName)
	{
		this.connectionString = connectionString;
		this.databaseName = databaseName;
	}

	public AdvertModel Create(AdvertModel advert)
	{
		// check if partner exists
		PartnerRepository.GetById(advert.PartnerId);

		advert.Id = GetId();

		ValidateFields(advert, "PartnerId", "Title", "ImageId");

		ImageRepository.GetById(advert.ImageId);

		try
		{
			Collection.InsertOne(advert);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while creating advert", ex);
		}

		return advert;

	}

	public AdvertModel Update(AdvertModel advert)
	{
		ValidateAllFields(advert);

		try
		{
			Collection.ReplaceOne(x => x.Id == advert.Id, advert);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while updating advert", ex);
		}

		return advert;
	}

	public void Delete(ulong id)
	{
		var advert = GetById(id);

		var partner = PartnerRepository.GetById(advert.PartnerId);
		partner.Ads.Remove(id);
		PartnerRepository.Update(partner);

		var image = ImageRepository.GetById(advert.ImageId);
		ImageRepository.Delete(image.Id);

		try
		{
			Collection.DeleteOne(x => x.Id == id);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while deleting advert", ex);
		}
	}
}
