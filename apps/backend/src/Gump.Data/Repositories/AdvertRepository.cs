using Gump.Data.Models;
using MongoDB.Driver;

namespace Gump.Data.Repositories;

public class AdvertRepository : RepositoryBase<AdvertModel>
{
	private readonly ImageRepository imageRepository;
	private readonly PartnerRepository partnerRepository;

	public AdvertRepository(string connectionString, string databaseName) : base(connectionString, databaseName)
	{
		partnerRepository = new PartnerRepository(connectionString, databaseName);
		this.imageRepository = new(connectionString, databaseName);
	}

	public AdvertModel Create(AdvertModel advert)
	{
		// check if partner exists
		partnerRepository.GetById(advert.PartnerId);

		advert.Id = GetId();

		ValidateFields(advert, "PartnerId", "Title", "ImageId");

		imageRepository.GetById(advert.ImageId);

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

		var partner = partnerRepository.GetById(advert.PartnerId);
		partner.Ads.Remove(id);
		partnerRepository.Update(partner);

		var image = imageRepository.GetById(advert.ImageId);
		imageRepository.Delete(image.Id);

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
