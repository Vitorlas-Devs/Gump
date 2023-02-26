using MongoDB.Bson.Serialization.Attributes;

namespace Gump.Data.Models;

public class VerificationModel
{
	[BsonElement("idPhotos")]
	public List<ulong> IdPhotos { get; set; }

	[BsonElement("professionPhotos")]
	public List<ulong> ProfessionPhotos { get; set; }
}
