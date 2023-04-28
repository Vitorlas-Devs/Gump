using Gump.Data.Models;
using MongoDB.Driver;
using Gump.Data;

namespace Gump.Data.Repositories;

public class AdvertRepository : RepositoryBase<AdvertModel>
{
	private readonly MongoDbConfig mongoDbConfig;
	private ImageRepository ImageRepository => new(mongoDbConfig);
	private PartnerRepository PartnerRepository => new(mongoDbConfig);

	public AdvertRepository(MongoDbConfig mongoDbConfig) : base(mongoDbConfig)
	{
		this.mongoDbConfig = mongoDbConfig;
	}

	public ulong GetRandomId()
	{
		ulong biggestId = Collection.AsQueryable().Max(x => x.Id);

		ulong randomId = 0;

		do
		{
			randomId = new Random().NextUInt64(biggestId + 1);
		}
		while (!Collection.AsQueryable().Any(x => x.Id == randomId));

		return randomId;
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
			throw new AggregateException($"Error while creating {nameof(advert)}", ex);
		}

		//Add advert to partner repository
		var partner = PartnerRepository.GetById(advert.PartnerId);
		partner.Ads.Add(advert.Id);
		PartnerRepository.Update(partner);

		return advert;

	}

	public AdvertModel Update(AdvertModel advert)
	{
		GetById(advert.Id);

		ValidateAllFields(advert);

		ImageRepository.GetById(advert.ImageId);

		try
		{
			Collection.ReplaceOne(x => x.Id == advert.Id, advert);
		}
		catch (MongoException ex)
		{
			throw new AggregateException($"Error while updating {nameof(advert)}", ex);
		}

		return advert;
	}

	public void Delete(ulong id)
	{
		var advert = GetById(id);

		var partner = PartnerRepository.GetById(advert.PartnerId);
		partner.Ads.Remove(id);
		PartnerRepository.Update(partner);

		try
		{
			Collection.DeleteOne(x => x.Id == id);
		}
		catch (MongoException ex)
		{
			throw new AggregateException($"Error while deleting {nameof(advert)}", ex);
		}
	}
}
