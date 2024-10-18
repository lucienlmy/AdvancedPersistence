using System;
using GTA;
using GTA.Math;

namespace AdvancedPersistence;

[Serializable]
public class CharacterDataV1
{
	public Vector3 Position { get; set; }

	public float Heading { get; set; }

	public string CarAttach { get; set; }

	public int PhoneTheme { get; set; } = 1;


	public int PhoneColor { get; set; }

	public int PhoneBackground { get; set; }

	public int Health { get; set; } = 200;


	public int Armor { get; set; } = 100;


	public PedHash PedSkin { get; set; } = PedHash.Franklin;


	public DateTime Date { get; set; }

	public TimeSpan Time { get; set; }

	public Weather Weather { get; set; }

	public Weather WeatherNext { get; set; }

	public int[] ClothesVariant { get; set; } = new int[12];


	public int[] ClothesTexture { get; set; } = new int[12];


	public int[] ClothesPalette { get; set; } = new int[12];


	public int[] PropsVariant { get; set; } = new int[7];


	public int[] PropsTexture { get; set; } = new int[7];

}
