using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GTA;
using GTA.Math;

namespace AdvancedPersistence;

[Serializable]
public class CharacterDataV2
{
	[OptionalField(VersionAdded = 2)]
	public int PhoneBody;

	[OptionalField(VersionAdded = 2)]
	public int PhoneTone;

	public Vector3 Position { get; set; }

	public float Heading { get; set; }

	public string CarAttach { get; set; }

	public int PhoneTheme { get; set; } = 1;


	public int PhoneColor { get; set; }

	public int PhoneBackground { get; set; }

	public int PhoneBrightness { get; set; } = 5;


	public bool WearingDropHelmet { get; set; }

	public uint ActiveWeapon { get; set; } = 2725352035u;


	public int Health { get; set; } = 200;


	public int Armor { get; set; } = 100;


	public int PedSkin { get; set; } = -1692214353;


	public DateTime Date { get; set; }

	public TimeSpan Time { get; set; }

	public Weather Weather { get; set; }

	public Weather WeatherNext { get; set; }

	public int[] ClothesVariant { get; set; } = new int[12];


	public int[] ClothesTexture { get; set; } = new int[12];


	public int[] ClothesPalette { get; set; } = new int[12];


	public int[] PropsVariant { get; set; } = new int[7];


	public int[] PropsTexture { get; set; } = new int[7];


	public Dictionary<uint, List<uint>> Weapons { get; set; } = new Dictionary<uint, List<uint>>();


	public Dictionary<uint, uint> Tints { get; set; } = new Dictionary<uint, uint>();


	public Dictionary<uint, int> Ammo { get; set; } = new Dictionary<uint, int>();

}
