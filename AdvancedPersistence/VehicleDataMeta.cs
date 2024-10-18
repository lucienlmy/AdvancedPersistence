using System;

namespace AdvancedPersistence;

[Serializable]
public class VehicleDataMeta
{
	public int Version { get; set; } = 2;


	public string Id { get; set; }
}
