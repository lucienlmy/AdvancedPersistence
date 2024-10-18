using System;
using System.IO;
using System.Linq;

namespace AdvancedPersistence;

public static class ModSettings
{
	public static readonly string Filename = "AdvPer_SETTINGS.txt";

	public static int MaxNumberOfStreamedInCars = 16;

	public static float StreamInDistance = 150f;

	public static float StreamOutDistance = 160f;

	public static bool EnableVehicleStreamer = true;

	public static int MaxNumberOfCars = 32;

	public static bool RemovePersonalVehicles = true;

	public static int DataSavingTime = 3000;

	public static bool SpawnAllVehiclesOnStartup = true;

	public static bool SpawnAllVehiclesAtSafeSpawn = false;

	public static bool ShowVehicleOutlines = true;

	public static bool SaveDuringMissions = false;

	public static bool EnablePhoneTurnOnByController = true;

	public static bool EnableBlips = true;

	public static bool PlaySpeechOnCarDelivery = true;

	public static bool FocusOnCarDelivery = true;

	public static bool EnableRemoteSystem = true;

	public static bool EnableRemoteMovement = true;

	public static bool ReturnVehiclesToSafeLocation = true;

	public static string SaveKey = "T";

	public static string SaveModifier = "SHIFT";

	public static string PhoneKey = "Z";

	public static string PhoneModifier = "SHIFT";

	public static bool SaveCharacterPosition = true;

	public static bool SaveCharacterSkin = true;

	public static bool SaveCharacterClothes = true;

	public static bool SaveCharacterHealthArmor = true;

	public static bool SaveCharacterWeapons = true;

	public static bool SaveVehicleMods = true;

	public static bool SaveVehicleExtras = true;

	public static bool SaveVehicleDoorState = true;

	public static bool SaveVehicleWindowState = true;

	public static bool SaveVehicleWheelState = true;

	public static bool SaveVehicleLightState = true;

	public static bool SaveVehicleEngineState = true;

	public static bool SaveVehicleConvertibleState = true;

	public static bool SaveVehicleWheelTurn = true;

	public static bool SaveGameTime = true;

	public static bool SaveGameWeather = true;

	public static bool LoadSettings()
	{
		string path = ((!File.Exists("scripts/" + Filename)) ? ("scripts/AdvancedPersistence/" + Filename) : ("scripts/" + Filename));
		if (File.Exists(path))
		{
			try
			{
				string[] array = File.ReadAllLines(path);
				foreach (string text in array)
				{
					try
					{
						if (!text.Contains("/-") && !string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text))
						{
							string text2 = text;
							if (text.Contains(';'))
							{
								text2 = text.Substring(0, text.IndexOf(';'));
							}
							text2 = text2.Replace(" ", string.Empty);
							string[] array2 = text2.Split('=');
							if (array2.Length < 2)
							{
								Logging.Log("INVALID SETTING: " + array2[0]);
							}
							else
							{
								Logging.Log("Settings fetch: " + array2[0] + " -> " + array2[1]);
								if (array2[0] == "MaxNumberOfCars")
								{
									MaxNumberOfCars = int.Parse(array2[1]);
								}
								else if (array2[0] == "MaxNumberOfStreamedInCars")
								{
									MaxNumberOfStreamedInCars = int.Parse(array2[1]);
								}
								else if (array2[0] == "SaveDuringMissions")
								{
									SaveDuringMissions = bool.Parse(array2[1]);
								}
								else if (array2[0] == "EnablePhoneTurnOnByController")
								{
									EnablePhoneTurnOnByController = bool.Parse(array2[1]);
								}
								else if (array2[0] == "StreamInDistance")
								{
									StreamInDistance = float.Parse(array2[1]);
								}
								else if (array2[0] == "StreamOutDistance")
								{
									StreamOutDistance = float.Parse(array2[1]);
								}
								else if (array2[0] == "EnableBlips")
								{
									EnableBlips = bool.Parse(array2[1]);
								}
								else if (array2[0] == "EnableVehicleStreamer")
								{
									EnableVehicleStreamer = bool.Parse(array2[1]);
								}
								else if (array2[0] == "PlaySpeechOnCarDelivery")
								{
									PlaySpeechOnCarDelivery = bool.Parse(array2[1]);
								}
								else if (array2[0] == "EnableRemoteSystem")
								{
									EnableRemoteSystem = bool.Parse(array2[1]);
								}
								else if (array2[0] == "EnableRemoteMovement")
								{
									EnableRemoteMovement = bool.Parse(array2[1]);
								}
								else if (array2[0] == "FocusOnCarDelivery")
								{
									FocusOnCarDelivery = bool.Parse(array2[1]);
								}
								else if (array2[0] == "SaveCharacterWeapons")
								{
									SaveCharacterWeapons = bool.Parse(array2[1]);
								}
								else if (array2[0] == "ReturnVehiclesToSafeLocation")
								{
									ReturnVehiclesToSafeLocation = bool.Parse(array2[1]);
								}
								else if (array2[0] == "ShowVehicleOutlines")
								{
									ShowVehicleOutlines = bool.Parse(array2[1]);
								}
								else if (array2[0] == "RemovePersonalVehicles")
								{
									RemovePersonalVehicles = bool.Parse(array2[1]);
								}
								else if (array2[0] == "SpawnAllVehiclesOnStartup")
								{
									SpawnAllVehiclesOnStartup = bool.Parse(array2[1]);
								}
								else if (array2[0] == "SpawnAllVehiclesAtSafeSpawn")
								{
									SpawnAllVehiclesAtSafeSpawn = bool.Parse(array2[1]);
								}
								else if (array2[0] == "DataSavingTime")
								{
									DataSavingTime = int.Parse(array2[1]);
								}
								else if (array2[0] == "SaveKey")
								{
									SaveKey = array2[1];
								}
								else if (array2[0] == "SaveModifier")
								{
									SaveModifier = array2[1];
								}
								else if (array2[0] == "PhoneKey")
								{
									PhoneKey = array2[1];
								}
								else if (array2[0] == "PhoneModifier")
								{
									PhoneModifier = array2[1];
								}
								else if (array2[0] == "SaveCharacterPosition")
								{
									SaveCharacterPosition = bool.Parse(array2[1]);
								}
								else if (array2[0] == "SaveCharacterSkin")
								{
									SaveCharacterSkin = bool.Parse(array2[1]);
								}
								else if (array2[0] == "SaveCharacterClothes")
								{
									SaveCharacterClothes = bool.Parse(array2[1]);
								}
								else if (array2[0] == "SaveCharacterHealthArmor")
								{
									SaveCharacterHealthArmor = bool.Parse(array2[1]);
								}
								else if (array2[0] == "SaveVehicleMods")
								{
									SaveVehicleMods = bool.Parse(array2[1]);
								}
								else if (array2[0] == "SaveVehicleExtras")
								{
									SaveVehicleExtras = bool.Parse(array2[1]);
								}
								else if (array2[0] == "SaveVehicleDoorState")
								{
									SaveVehicleDoorState = bool.Parse(array2[1]);
								}
								else if (array2[0] == "SaveVehicleWindowState")
								{
									SaveVehicleWindowState = bool.Parse(array2[1]);
								}
								else if (array2[0] == "SaveVehicleLightState")
								{
									SaveVehicleLightState = bool.Parse(array2[1]);
								}
								else if (array2[0] == "SaveVehicleEngineState")
								{
									SaveVehicleEngineState = bool.Parse(array2[1]);
								}
								else if (array2[0] == "SaveVehicleConvertibleState")
								{
									SaveVehicleConvertibleState = bool.Parse(array2[1]);
								}
								else if (array2[0] == "SaveVehicleWheelTurn")
								{
									SaveVehicleWheelTurn = bool.Parse(array2[1]);
								}
								else if (array2[0] == "SaveGameTime")
								{
									SaveGameTime = bool.Parse(array2[1]);
								}
								else if (array2[0] == "SaveGameWeather")
								{
									SaveGameWeather = bool.Parse(array2[1]);
								}
							}
						}
					}
					catch (Exception ex)
					{
						Logging.Log("ERROR PARSING SETTING: (" + text + ") -> " + ex.ToString());
					}
				}
				return true;
			}
			catch (Exception ex2)
			{
				Logging.Log("ERROR PARSING SETTINGS: " + ex2.ToString());
				return false;
			}
		}
		return false;
	}
}
