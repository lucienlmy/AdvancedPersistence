using System;

namespace AdvancedPersistence;

[Serializable]
public class CharacterDataMeta
{
	public int Version { get; set; } = 2;


	public string Id { get; set; } = "one";

}
