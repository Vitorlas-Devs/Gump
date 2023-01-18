using MongoDB.Bson.Serialization.Attributes;

namespace Gump.Data.Models;

public class VerificationModel
{
	[BsonElement("idPhotos")]
	public List<Uri> IdPhotos { get; set; }

	[BsonElement("professionPhotos")]
	public List<Uri> ProfessionPhotos { get; set; }
}
