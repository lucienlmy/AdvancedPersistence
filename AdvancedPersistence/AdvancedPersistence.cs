using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace AdvancedPersistence;

public class AdvancedPersistence : Script
{
	private delegate ulong GetHandleAddressFuncDelegate(int handle);

	public static List<int> SoundIdBank = new List<int>();

	public static List<VehicleDataV1> VehicleDatabase = new List<VehicleDataV1>();

	public static List<VehicleDataMeta> VehicleMetabase = new List<VehicleDataMeta>();

	public static Dictionary<Vehicle, PedTask> AttachedTasks = new Dictionary<Vehicle, PedTask>();

	public static Dictionary<Vehicle, VehicleDataV1> AttachedVehicles = new Dictionary<Vehicle, VehicleDataV1>();

	public static CharacterDataV2 MainCharacter = new CharacterDataV2();

	public static CharacterDataMeta MainCharacterMeta = new CharacterDataMeta();

	public static Dictionary<int, string> SwitchedCars = new Dictionary<int, string>();

	public static readonly int[] TireIndices = new int[10] { 0, 1, 2, 3, 4, 5, 45, 46, 47, 48 };

	public static readonly string VehicleFilename = "scripts/AdvancedPersistence/AdvPer_VEH_DB.bin";

	public static readonly string VehicleMetaname = "scripts/AdvancedPersistence/AdvPer_VEH_META.bin";

	public static readonly string CharacterFilename = "scripts/AdvancedPersistence/AdvPer_CHR_DB.bin";

	public static readonly string CharacterMetaname = "scripts/AdvancedPersistence/AdvPer_CHR_META.bin";

	public bool StoppedInitial = true;

	public int StartedTime = Game.GameTime;

	public int UpdateTick;

	public int BlipTick;

	public int LoadTick;

	public int SaveTick;

	public int StreamTick;

	public bool LoadCharacter;

	public bool LoadVehicles;

	public int LoadMax;

	public int LoadCnt;

	public static bool SaveVehicle = false;

	public bool SaveOne;

	public int SaveTimed;

	public bool SaveTwo;

	public bool SaveThree;

	public bool DeletePersonalCars;

	public bool LastViewChanged;

	public bool LastFirstPerson;

	public bool Stream1;

	public bool Stream2;

	public bool Stream3;

	public bool Stream4;

	public bool DoWeapons;

	public bool DoVehicles;

	public List<VehicleDataV1> CloneList;

	public WeaponHash[] Weapons = (WeaponHash[])Enum.GetValues(typeof(WeaponHash));

	public int WeaponIndex;

	public int VehicleIndex;

	public WeaponComponentHash[] WeaponComponents = (WeaponComponentHash[])Enum.GetValues(typeof(WeaponComponentHash));

	public BinaryFormatter form = new BinaryFormatter();

	public bool debouncer;

	public bool switchDetect = true;

	public bool waitForSwitch;

	public static int AudioTick = Game.GameTime;

	public bool ActivateThings;

	public bool IsModLoading = true;

	public bool IsInitializing = true;

	public int InitTime = Game.GameTime;

	public static bool DrawTrace = false;

	public static Vector3 StartTrace = default(Vector3);

	public static Vector3 EndTrace = default(Vector3);

	public int RaycastTick = Game.GameTime;

	private Vector3 ep_priv;

	private Vector3 ep2_priv;

	private Vector3 ep3_priv;

	private Vector3 from_priv;

	private Vector3 to_priv;

	private Vector3 up_priv;

	private Vector3 right_priv;

	public int GetOutHint = Game.GameTime;

	public bool DoGetOutHint;

	public VehicleDataV1 PressToSaveVeh;

	public Vehicle LastHinted;

	public int WaitTime;

	public bool DoXThing;

	public int XTime;

	public string SaveCarString = "";

	public float SteeringAngleRemote;

	public bool CanDie = true;

	public bool CanArrest = true;

	public bool GotArrested;

	public bool HasDied;

	public int PromptTime;

	public bool ResetCars;

	public Vector3 DieArrestedAt;

	public Dictionary<Vehicle, int> DrawCarBlinkers = new Dictionary<Vehicle, int>();

	public int PressDownETime = Game.GameTime;

	public bool initialPress = true;

	public bool DidTheThing;

	public static Prop FobObj = null;

	public static bool DoingFobAnim = false;

	public WeaponHash StoredWeapon = WeaponHash.Unarmed;

	public Dictionary<uint, List<uint>> CWeapons { get; set; } = new Dictionary<uint, List<uint>>();


	public Dictionary<uint, uint> CTints { get; set; } = new Dictionary<uint, uint>();


	public Dictionary<uint, int> CAmmo { get; set; } = new Dictionary<uint, int>();


	public static void PlayFrontendAudio(string sound, int phone)
	{
		switch (phone)
		{
		case 0:
		{
			int item3 = Audio.PlaySoundFrontend(sound, "Phone_SoundSet_Michael");
			SoundIdBank.Add(item3);
			break;
		}
		case 1:
		{
			int item2 = Audio.PlaySoundFrontend(sound, "Phone_SoundSet_Trevor");
			SoundIdBank.Add(item2);
			break;
		}
		default:
		{
			int item = Audio.PlaySoundFrontend(sound, "Phone_SoundSet_Franklin");
			SoundIdBank.Add(item);
			break;
		}
		}
	}

	public AdvancedPersistence()
	{
		LoadCnt = 0;
		LoadMax = 0;
		LoadTick = Game.GameTime + 1500;
		LoadCharacter = true;
		LoadVehicles = false;
		if (Directory.Exists("scripts/AdvancedPersistence"))
		{
			File.WriteAllText("scripts/AdvancedPersistence/" + Logging.Filename, string.Empty);
		}
		else
		{
			File.WriteAllText("scripts/" + Logging.Filename, string.Empty);
		}
		Logging.Log("Booting up...");
		try
		{
			if (!Directory.Exists("scripts/AdvancedPersistence"))
			{
				Directory.CreateDirectory("scripts/AdvancedPersistence");
			}
		}
		catch (Exception ex)
		{
			Logging.Log("Couldn't create sub-folder. File permissions.");
			Logging.Log("ERROR: " + ex.ToString());
			Logging.Log("ABORTING");
			Abort();
			return;
		}
		if (ModSettings.LoadSettings())
		{
			Logging.Log("Loaded settings");
		}
		else
		{
			Logging.Log("WARN: Could not find settings, using defaults");
		}
		try
		{
			if (File.Exists(VehicleMetaname))
			{
				Logging.Log("Found vehicle metabase, loading...");
				try
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					using (Stream serializationStream = File.OpenRead(VehicleMetaname))
					{
						VehicleMetabase = (List<VehicleDataMeta>)binaryFormatter.Deserialize(serializationStream);
					}
					Logging.Log($"Meta-loaded [{VehicleMetabase.Count}] vehicles");
				}
				catch (Exception ex2)
				{
					Logging.Log("ERROR: " + ex2.ToString());
				}
			}
			else
			{
				Logging.Log("WARN: Could not find vehicle metabase");
			}
			if (File.Exists(VehicleFilename))
			{
				Logging.Log("Found vehicle database, loading...");
				try
				{
					BinaryFormatter binaryFormatter2 = new BinaryFormatter();
					using (Stream serializationStream2 = File.OpenRead(VehicleFilename))
					{
						VehicleDatabase = (List<VehicleDataV1>)binaryFormatter2.Deserialize(serializationStream2);
					}
					Notification.Show($"Loaded [{VehicleDatabase.Count}] vehicles");
					Logging.Log($"Loaded [{VehicleDatabase.Count}] vehicles");
					LoadMax = VehicleDatabase.Count;
					LoadCnt = 0;
				}
				catch (Exception ex3)
				{
					Logging.Log("ERROR: " + ex3.ToString());
				}
			}
			else
			{
				Logging.Log("WARN: Could not find vehicle database");
			}
			if (File.Exists(CharacterMetaname))
			{
				Logging.Log("Found character metabase, loading...");
				try
				{
					BinaryFormatter binaryFormatter3 = new BinaryFormatter();
					using (Stream serializationStream3 = File.OpenRead(CharacterMetaname))
					{
						MainCharacterMeta = (CharacterDataMeta)binaryFormatter3.Deserialize(serializationStream3);
					}
					Logging.Log("Loaded character meta VERSION: " + MainCharacterMeta.Version);
				}
				catch (Exception ex4)
				{
					Logging.Log("ERROR: " + ex4.ToString());
				}
			}
			else
			{
				LoadCharacter = false;
				IsModLoading = false;
				Logging.Log("WARN: Could not find character database");
			}
			if (File.Exists(CharacterFilename))
			{
				Logging.Log("Found character database, loading...");
				try
				{
					if (MainCharacterMeta != null)
					{
						if (MainCharacterMeta.Version == 1)
						{
							Logging.Log("WARN: DETECTED OLD CHARACTER DATA (Version 1)");
							Logging.Log("Converting to newer data model...");
							BinaryFormatter binaryFormatter4 = new BinaryFormatter();
							CharacterDataV1 characterDataV = new CharacterDataV1();
							using (Stream serializationStream4 = File.OpenRead(CharacterFilename))
							{
								characterDataV = (CharacterDataV1)binaryFormatter4.Deserialize(serializationStream4);
							}
							if (characterDataV != null)
							{
								MainCharacter.Position = characterDataV.Position;
								MainCharacter.Heading = characterDataV.Heading;
								MainCharacter.CarAttach = characterDataV.CarAttach;
								MainCharacter.PhoneTheme = characterDataV.PhoneTheme;
								MainCharacter.PhoneColor = characterDataV.PhoneColor;
								MainCharacter.PhoneBackground = characterDataV.PhoneBackground;
								MainCharacter.PhoneBrightness = 5;
								MainCharacter.PhoneBody = 0;
								MainCharacter.PhoneTone = 0;
								MainCharacter.Health = characterDataV.Health;
								MainCharacter.Armor = characterDataV.Armor;
								MainCharacter.PedSkin = (int)characterDataV.PedSkin;
								MainCharacter.Date = characterDataV.Date;
								MainCharacter.Time = characterDataV.Time;
								MainCharacter.Weather = characterDataV.Weather;
								MainCharacter.WeatherNext = characterDataV.WeatherNext;
								for (int i = 0; i < 12; i++)
								{
									MainCharacter.ClothesVariant[i] = characterDataV.ClothesVariant[i];
									MainCharacter.ClothesTexture[i] = characterDataV.ClothesTexture[i];
									MainCharacter.ClothesPalette[i] = characterDataV.ClothesPalette[i];
								}
								for (int j = 0; j < 7; j++)
								{
									MainCharacter.PropsVariant[j] = characterDataV.PropsVariant[j];
									MainCharacter.PropsTexture[j] = characterDataV.PropsTexture[j];
								}
								MainCharacterMeta.Version = 2;
								Logging.Log("SUCCESS: Done conversion");
							}
							else
							{
								Logging.Log("Couldn't load meta for conversion");
							}
						}
						else
						{
							BinaryFormatter binaryFormatter5 = new BinaryFormatter();
							using Stream serializationStream5 = File.OpenRead(CharacterFilename);
							MainCharacter = (CharacterDataV2)binaryFormatter5.Deserialize(serializationStream5);
						}
					}
					Logging.Log("Loaded character data");
				}
				catch (Exception ex5)
				{
					Logging.Log("ERROR: " + ex5.ToString());
				}
			}
			else
			{
				LoadCharacter = false;
				IsModLoading = false;
				Logging.Log("WARN: Could not find character database");
			}
		}
		catch (Exception ex6)
		{
			GTA.UI.Screen.ShowSubtitle("[Advanced Persistence] BOOT UP ERROR. CHECK LOGS.", 10000);
			Logging.Log("ERROR LOADING: " + ex6.ToString());
			Logging.Log("ABORTING");
			Abort();
			return;
		}
		UpdateTick = Game.GameTime + ModSettings.DataSavingTime;
		if (!LoadCharacter)
		{
			GTA.UI.Screen.FadeIn(2000);
			SaveOne = true;
			UpdateTick = Game.GameTime + 2000;
			IsModLoading = false;
			IsInitializing = false;
		}
		else
		{
			SaveOne = false;
		}
		SaveTwo = false;
		Stream1 = false;
		Stream2 = false;
		Stream3 = false;
		Stream4 = false;
		BlipTick = Game.GameTime + 3000;
		int num = Function.Call<int>(Hash.GET_CAM_ACTIVE_VIEW_MODE_CONTEXT);
		if (Function.Call<int>(Hash.GET_CAM_VIEW_MODE_FOR_CONTEXT, num) == 4)
		{
			LastFirstPerson = true;
		}
		if (ModSettings.RemovePersonalVehicles)
		{
			DeletePersonalCars = true;
		}
		Function.Call(Hash.DESTROY_MOBILE_PHONE);
		Function.Call(Hash.SCRIPT_IS_MOVING_MOBILE_PHONE_OFFSCREEN, true);
		Logging.Log("Attaching events...");
		base.Tick += OnTick;
		base.KeyDown += OnKeyDown;
		base.KeyUp += OnKeyUp;
		base.Aborted += OnAborted;
		Logging.Log("Attached");
		Logging.Log("Beginning load...");
	}

	public static void DeleteBlipsOnCar(Vehicle veh)
	{
		if (!(veh != null) || !veh.Exists())
		{
			return;
		}
		Blip[] attachedBlips = veh.AttachedBlips;
		foreach (Blip blip in attachedBlips)
		{
			if (blip != null && blip.Exists())
			{
				blip.Delete();
			}
		}
	}

	public static bool IsAmphCar(VehicleHash hash)
	{
		if (hash != VehicleHash.SeaSparrow && hash != VehicleHash.SeaSparrow2 && hash != VehicleHash.SeaSparrow3 && hash != VehicleHash.Dodo && hash != VehicleHash.Tula && hash != VehicleHash.Stromberg && hash != VehicleHash.Apc && hash != VehicleHash.Zhaba && hash != VehicleHash.Toreador)
		{
			return hash == VehicleHash.Seabreeze;
		}
		return true;
	}

	public static bool IsSubmarine(VehicleHash hash)
	{
		if (hash != VehicleHash.Submersible && hash != VehicleHash.Submersible2)
		{
			return hash == VehicleHash.Avisa;
		}
		return true;
	}

	public static Vehicle CreateVehicle(VehicleDataV1 vdata, Vehicle bypassVeh = null, bool byPassed = false, bool unfreeze = true, bool atSafeSpot = false)
	{
		if (vdata == null)
		{
			return null;
		}
		if (!atSafeSpot || !vdata.SafeSpawnSet)
		{
			Function.Call(Hash.REQUEST_COLLISION_AT_COORD, vdata.Position.X, vdata.Position.Y, vdata.Position.Z);
		}
		else
		{
			Function.Call(Hash.REQUEST_COLLISION_AT_COORD, vdata.SafeSpawn.X, vdata.SafeSpawn.Y, vdata.SafeSpawn.Z);
		}
		Vehicle vehicle = null;
		vehicle = ((bypassVeh != null) ? bypassVeh : ((atSafeSpot && vdata.SafeSpawnSet) ? World.CreateVehicle(vdata.Hash, vdata.SafeSpawn) : World.CreateVehicle(vdata.Hash, vdata.Position)));
		if (vehicle == null)
		{
			return null;
		}
		vehicle.Model.RequestCollision(1000);
		Function.Call(Hash.SET_ENTITY_LOAD_COLLISION_FLAG, vehicle.Handle, true, 1);
		Function.Call(Hash.SET_ENTITY_SHOULD_FREEZE_WAITING_ON_COLLISION, vehicle.Handle, false);
		try
		{
			vdata.WasUserDespawned = false;
			if (!byPassed)
			{
				if (!atSafeSpot || !vdata.SafeSpawnSet)
				{
					vehicle.PositionNoOffset = vdata.Position;
				}
				else
				{
					vehicle.PositionNoOffset = vdata.SafeSpawn;
				}
				vehicle.IsPositionFrozen = true;
				vehicle.IsInvincible = true;
			}
			vehicle.Mods.InstallModKit();
			vehicle.IsPersistent = true;
			if (!byPassed)
			{
				if (!atSafeSpot || !vdata.SafeSpawnSet)
				{
					vehicle.Rotation = vdata.Rotation;
				}
				else
				{
					vehicle.Rotation = vdata.SafeRotation;
				}
				vehicle.PlaceOnGround();
			}
			if (vehicle.Model.IsBoat || vehicle.Model.IsAmphibiousVehicle || vehicle.Model.IsAmphibiousQuadBike || vehicle.Model.IsAmphibiousCar || vehicle.Model.IsSubmarineCar || vehicle.Model.IsJetSki || IsSubmarine(vehicle.Model) || IsAmphCar(vehicle.Model))
			{
				Function.Call(Hash.SET_BOAT_ANCHOR, vehicle.Handle, true);
			}
			if (ModSettings.SaveVehicleExtras)
			{
				for (int i = 0; i < 15; i++)
				{
					vehicle.ToggleExtra(i, vdata.Extras[i]);
				}
			}
			if (ModSettings.SaveVehicleDoorState)
			{
				if (vdata.FrontLeftDoorState == 0)
				{
					vehicle.Doors[VehicleDoorIndex.FrontLeftDoor].Close(instantly: true);
				}
				else if (vdata.FrontLeftDoorState == 1)
				{
					vehicle.Doors[VehicleDoorIndex.FrontLeftDoor].Open();
				}
				else if (vdata.FrontLeftDoorState == 2)
				{
					vehicle.Doors[VehicleDoorIndex.FrontLeftDoor].Break(stayInTheWorld: false);
				}
				if (vdata.FrontRightDoorState == 0)
				{
					vehicle.Doors[VehicleDoorIndex.FrontRightDoor].Close(instantly: true);
				}
				else if (vdata.FrontRightDoorState == 1)
				{
					vehicle.Doors[VehicleDoorIndex.FrontRightDoor].Open();
				}
				else if (vdata.FrontRightDoorState == 2)
				{
					vehicle.Doors[VehicleDoorIndex.FrontRightDoor].Break(stayInTheWorld: false);
				}
				if (vdata.BackLeftDoorState == 0)
				{
					vehicle.Doors[VehicleDoorIndex.BackLeftDoor].Close(instantly: true);
				}
				else if (vdata.BackLeftDoorState == 1)
				{
					vehicle.Doors[VehicleDoorIndex.BackLeftDoor].Open();
				}
				else if (vdata.BackLeftDoorState == 2)
				{
					vehicle.Doors[VehicleDoorIndex.BackLeftDoor].Break(stayInTheWorld: false);
				}
				if (vdata.BackRightDoorState == 0)
				{
					vehicle.Doors[VehicleDoorIndex.BackRightDoor].Close(instantly: true);
				}
				else if (vdata.BackRightDoorState == 1)
				{
					vehicle.Doors[VehicleDoorIndex.BackRightDoor].Open();
				}
				else if (vdata.BackRightDoorState == 2)
				{
					vehicle.Doors[VehicleDoorIndex.BackRightDoor].Break(stayInTheWorld: false);
				}
				if (vdata.TrunkState == 0)
				{
					vehicle.Doors[VehicleDoorIndex.Trunk].Close(instantly: true);
				}
				else if (vdata.TrunkState == 1)
				{
					vehicle.Doors[VehicleDoorIndex.Trunk].Open();
				}
				else if (vdata.TrunkState == 2)
				{
					vehicle.Doors[VehicleDoorIndex.Trunk].Break(stayInTheWorld: false);
				}
				if (vdata.HoodState == 0)
				{
					vehicle.Doors[VehicleDoorIndex.Hood].Close(instantly: true);
				}
				else if (vdata.HoodState == 1)
				{
					vehicle.Doors[VehicleDoorIndex.Hood].Open();
				}
				else if (vdata.HoodState == 2)
				{
					vehicle.Doors[VehicleDoorIndex.Hood].Break(stayInTheWorld: false);
				}
			}
			if (ModSettings.SaveVehicleWindowState)
			{
				for (int j = 0; j < 10; j++)
				{
					if (vdata.WheelStates[j] == 1)
					{
						Function.Call(Hash.SET_VEHICLE_TYRE_BURST, vehicle.Handle, TireIndices[j], false, 100f);
					}
					else if (vdata.WheelStates[j] == 2)
					{
						Function.Call(Hash.SET_VEHICLE_TYRE_BURST, vehicle.Handle, TireIndices[j], true, 1000f);
					}
				}
			}
			if (ModSettings.SaveVehicleConvertibleState && vehicle.IsConvertible)
			{
				Function.Call(Hash.ROLL_DOWN_WINDOWS, vehicle.Handle);
				if (vdata.ConvertibleState)
				{
					Function.Call(Hash.RAISE_CONVERTIBLE_ROOF, vehicle.Handle, true);
				}
				else
				{
					Function.Call(Hash.LOWER_CONVERTIBLE_ROOF, vehicle.Handle, true);
				}
			}
			if (!byPassed)
			{
				Script.Wait(1);
			}
			if (ModSettings.SaveVehicleWindowState)
			{
				for (int k = 0; k < 8; k++)
				{
					if (vdata.WindowStates[k] == 1)
					{
						Function.Call(Hash.SMASH_VEHICLE_WINDOW, vehicle.Handle, k);
						continue;
					}
					if (!vehicle.IsConvertible)
					{
						Function.Call(Hash.ROLL_UP_WINDOW, vehicle.Handle, k);
					}
					Function.Call(Hash.FIX_VEHICLE_WINDOW, vehicle.Handle, k);
				}
			}
			if (!byPassed && ModSettings.SaveCharacterPosition && vdata.Id == MainCharacter.CarAttach)
			{
				Game.Player.Character.SetIntoVehicle(vehicle, VehicleSeat.Driver);
			}
			vehicle.DirtLevel = vdata.DirtLevel;
			if (!vehicle.Model.IsPlane)
			{
				Function.Call(Hash.SET_VEHICLE_ENGINE_ON, vehicle.Handle, false, true, false);
				if (ModSettings.SaveVehicleEngineState)
				{
					Function.Call(Hash.SET_VEHICLE_ENGINE_ON, vehicle.Handle, vdata.EngineState, true, false);
				}
			}
			if (ModSettings.SaveVehicleLightState)
			{
				if (vdata.LightState2 == 0)
				{
					vehicle.AreHighBeamsOn = false;
					vehicle.AreLightsOn = false;
				}
				else if (vdata.LightState2 == 1)
				{
					vehicle.AreHighBeamsOn = false;
					vehicle.AreLightsOn = true;
				}
				else
				{
					vehicle.AreHighBeamsOn = true;
					vehicle.AreLightsOn = true;
				}
				if (vdata.SirenState == 1)
				{
					vehicle.IsSirenActive = true;
				}
			}
			vehicle.IsAlarmSet = vdata.AlarmState;
			if (vdata.LockState)
			{
				vehicle.LockStatus = VehicleLockStatus.CannotEnter;
			}
			AttachedVehicles[vehicle] = vdata;
			vdata.Handle = vehicle;
			if (ModSettings.SaveVehicleMods)
			{
				vehicle.CanTiresBurst = !vdata.BulletProofTires;
				vehicle.Mods.NeonLightsColor = vdata.NeonLightColor;
				vehicle.Mods.WindowTint = vdata.WindowTint;
				vehicle.Mods.LicensePlateStyle = vdata.LicensePlateStyle;
				vehicle.Mods.Livery = vdata.Livery;
				vehicle.Mods.WheelType = vdata.WheelType;
				vehicle.Mods.TireSmokeColor = vdata.TireSmokeColor;
				if (vdata.Turbo)
				{
					vehicle.Mods[VehicleToggleModType.Turbo].IsInstalled = true;
				}
				if (vdata.XenonHeadlights)
				{
					vehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled = true;
				}
				if (vdata.TireSmoke)
				{
					vehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled = true;
				}
				vehicle.Mods[VehicleModType.Spoilers].Index = vdata.Spoiler;
				vehicle.Mods[VehicleModType.Spoilers].Variation = vdata.SpoilerVar;
				vehicle.Mods[VehicleModType.FrontBumper].Index = vdata.FrontBumper;
				vehicle.Mods[VehicleModType.FrontBumper].Variation = vdata.FrontBumperVar;
				vehicle.Mods[VehicleModType.RearBumper].Index = vdata.RearBumper;
				vehicle.Mods[VehicleModType.RearBumper].Variation = vdata.RearBumperVar;
				vehicle.Mods[VehicleModType.SideSkirt].Index = vdata.SideSkirt;
				vehicle.Mods[VehicleModType.SideSkirt].Variation = vdata.SideSkirtVar;
				vehicle.Mods[VehicleModType.Exhaust].Index = vdata.Exhaust;
				vehicle.Mods[VehicleModType.Exhaust].Variation = vdata.ExhaustVar;
				vehicle.Mods[VehicleModType.Frame].Index = vdata.Frame;
				vehicle.Mods[VehicleModType.Frame].Variation = vdata.FrameVar;
				vehicle.Mods[VehicleModType.Grille].Index = vdata.Grille;
				vehicle.Mods[VehicleModType.Grille].Variation = vdata.GrilleVar;
				vehicle.Mods[VehicleModType.Hood].Index = vdata.Hood;
				vehicle.Mods[VehicleModType.Hood].Variation = vdata.HoodVar;
				vehicle.Mods[VehicleModType.Fender].Index = vdata.Fender;
				vehicle.Mods[VehicleModType.Fender].Variation = vdata.FenderVar;
				vehicle.Mods[VehicleModType.RightFender].Index = vdata.RightFender;
				vehicle.Mods[VehicleModType.RightFender].Variation = vdata.RightFenderVar;
				vehicle.Mods[VehicleModType.Roof].Index = vdata.Roof;
				vehicle.Mods[VehicleModType.Roof].Variation = vdata.RoofVar;
				vehicle.Mods[VehicleModType.Engine].Index = vdata.Engine;
				vehicle.Mods[VehicleModType.Engine].Variation = vdata.EngineVar;
				vehicle.Mods[VehicleModType.Brakes].Index = vdata.Brakes;
				vehicle.Mods[VehicleModType.Brakes].Variation = vdata.BrakesVar;
				vehicle.Mods[VehicleModType.Transmission].Index = vdata.Transmission;
				vehicle.Mods[VehicleModType.Transmission].Variation = vdata.TransmissionVar;
				vehicle.Mods[VehicleModType.Horns].Index = vdata.Horns;
				vehicle.Mods[VehicleModType.Horns].Variation = vdata.HornsVar;
				vehicle.Mods[VehicleModType.Suspension].Index = vdata.Suspension;
				vehicle.Mods[VehicleModType.Suspension].Variation = vdata.SuspensionVar;
				vehicle.Mods[VehicleModType.Armor].Index = vdata.Armor;
				vehicle.Mods[VehicleModType.Armor].Variation = vdata.ArmorVar;
				vehicle.Mods[VehicleModType.FrontWheel].Index = vdata.FrontWheel;
				vehicle.Mods[VehicleModType.FrontWheel].Variation = vdata.FrontWheelVar;
				vehicle.Mods[VehicleModType.RearWheel].Index = vdata.RearWheel;
				vehicle.Mods[VehicleModType.RearWheel].Variation = vdata.RearWheelVar;
				vehicle.Mods[VehicleModType.PlateHolder].Index = vdata.PlateHolder;
				vehicle.Mods[VehicleModType.PlateHolder].Variation = vdata.PlateHolderVar;
				vehicle.Mods[VehicleModType.VanityPlates].Index = vdata.VanityPlates;
				vehicle.Mods[VehicleModType.VanityPlates].Variation = vdata.VanityPlatesVar;
				vehicle.Mods[VehicleModType.TrimDesign].Index = vdata.TrimDesign;
				vehicle.Mods[VehicleModType.TrimDesign].Variation = vdata.TrimDesignVar;
				vehicle.Mods[VehicleModType.Ornaments].Index = vdata.Ornaments;
				vehicle.Mods[VehicleModType.Ornaments].Variation = vdata.OrnamentsVar;
				vehicle.Mods[VehicleModType.Dashboard].Index = vdata.Dashboard;
				vehicle.Mods[VehicleModType.Dashboard].Variation = vdata.DashboardVar;
				vehicle.Mods[VehicleModType.DialDesign].Index = vdata.DialDesign;
				vehicle.Mods[VehicleModType.DialDesign].Variation = vdata.DialDesignVar;
				vehicle.Mods[VehicleModType.DoorSpeakers].Index = vdata.DoorSpeakers;
				vehicle.Mods[VehicleModType.DoorSpeakers].Variation = vdata.DoorSpeakersVar;
				vehicle.Mods[VehicleModType.Seats].Index = vdata.Seats;
				vehicle.Mods[VehicleModType.Seats].Variation = vdata.SeatsVar;
				vehicle.Mods[VehicleModType.SteeringWheels].Index = vdata.SteeringWheels;
				vehicle.Mods[VehicleModType.SteeringWheels].Variation = vdata.SteeringWheelsVar;
				vehicle.Mods[VehicleModType.ColumnShifterLevers].Index = vdata.ColumnShifterLevers;
				vehicle.Mods[VehicleModType.ColumnShifterLevers].Variation = vdata.ColumnShifterLeversVar;
				vehicle.Mods[VehicleModType.Plaques].Index = vdata.Plaques;
				vehicle.Mods[VehicleModType.Plaques].Variation = vdata.PlaquesVar;
				vehicle.Mods[VehicleModType.Speakers].Index = vdata.Speakers;
				vehicle.Mods[VehicleModType.Speakers].Variation = vdata.SpeakersVar;
				vehicle.Mods[VehicleModType.Trunk].Index = vdata.Trunk;
				vehicle.Mods[VehicleModType.Trunk].Variation = vdata.TrunkVar;
				vehicle.Mods[VehicleModType.Hydraulics].Index = vdata.Hydraulics;
				vehicle.Mods[VehicleModType.Hydraulics].Variation = vdata.HydraulicsVar;
				vehicle.Mods[VehicleModType.EngineBlock].Index = vdata.EngineBlock;
				vehicle.Mods[VehicleModType.EngineBlock].Variation = vdata.EngineBlockVar;
				vehicle.Mods[VehicleModType.AirFilter].Index = vdata.AirFilter;
				vehicle.Mods[VehicleModType.AirFilter].Variation = vdata.AirFilterVar;
				vehicle.Mods[VehicleModType.Struts].Index = vdata.Struts;
				vehicle.Mods[VehicleModType.Struts].Variation = vdata.StrutsVar;
				vehicle.Mods[VehicleModType.ArchCover].Index = vdata.ArchCover;
				vehicle.Mods[VehicleModType.ArchCover].Variation = vdata.ArchCoverVar;
				vehicle.Mods[VehicleModType.Aerials].Index = vdata.Aerials;
				vehicle.Mods[VehicleModType.Aerials].Variation = vdata.AerialsVar;
				vehicle.Mods[VehicleModType.Trim].Index = vdata.Trim;
				vehicle.Mods[VehicleModType.Trim].Variation = vdata.TrimVar;
				vehicle.Mods[VehicleModType.Tank].Index = vdata.Tank;
				vehicle.Mods[VehicleModType.Tank].Variation = vdata.TankVar;
				vehicle.Mods[VehicleModType.Windows].Index = vdata.Windows;
				vehicle.Mods[VehicleModType.Windows].Variation = vdata.WindowsVar;
				vehicle.Mods[VehicleModType.Livery].Index = vdata.Livery;
				vehicle.Mods[VehicleModType.Livery].Variation = vdata.LiveryVar;
				vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, vdata.NeonLightBack);
				vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, vdata.NeonLightFront);
				vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, vdata.NeonLightLeft);
				vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, vdata.NeonLightRight);
				vehicle.Mods.PrimaryColor = vdata.PrimaryColor;
				vehicle.Mods.SecondaryColor = vdata.SecondaryColor;
				vehicle.Mods.PearlescentColor = vdata.PearlescentColor;
				vehicle.Mods.DashboardColor = vdata.DashboardColor;
				if (vdata.IsPrimaryCustom)
				{
					vehicle.Mods.CustomPrimaryColor = vdata.CustomPrimaryColor;
				}
				if (vdata.IsSecondaryCustom)
				{
					vehicle.Mods.CustomSecondaryColor = vdata.CustomSecondaryColor;
				}
				vehicle.Mods.LicensePlate = vdata.LicensePlate;
				vehicle.Mods.TrimColor = vdata.TrimColor;
				vehicle.Mods.RimColor = vdata.RimColor;
				Function.Call(Hash.SET_VEHICLE_MOD, vehicle.Handle, 40, vdata.Boost, vdata.RearWheelVar);
				Function.Call(Hash.SET_VEHICLE_XENON_LIGHT_COLOR_INDEX, vehicle.Handle, vdata.XenonHeadlightsColor);
			}
			if (ModSettings.SaveVehicleWheelTurn && (!(vdata.SteeringAngle >= -10f) || !(vdata.SteeringAngle <= 10f)))
			{
				if (vdata.SteeringAngle > 0f)
				{
					vehicle.SteeringAngle = vdata.SteeringAngle - 10f;
				}
				else
				{
					vehicle.SteeringAngle = vdata.SteeringAngle + 10f;
				}
			}
			if (!byPassed)
			{
				vehicle.IsHandbrakeForcedOn = true;
				Script.Wait(1);
				vehicle.IsHandbrakeForcedOn = false;
			}
			if (ModSettings.EnableBlips)
			{
				if (vehicle.AttachedBlip == null)
				{
					vehicle.AddBlip();
				}
				if (vehicle.Model.IsHelicopter)
				{
					vehicle.AttachedBlip.Sprite = BlipSprite.PoliceHelicopter;
				}
				else if (vehicle.Model.IsAmphibiousQuadBike || vehicle.Model.IsBicycle || vehicle.Model.IsBike || vehicle.Model.IsQuadBike)
				{
					vehicle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleBike;
				}
				else if (vehicle.Model.IsJetSki)
				{
					vehicle.AttachedBlip.Sprite = BlipSprite.Seashark;
				}
				else if (vehicle.Model.IsBoat)
				{
					vehicle.AttachedBlip.Sprite = BlipSprite.Boat;
				}
				else if (vehicle.Model.IsPlane)
				{
					vehicle.AttachedBlip.Sprite = BlipSprite.Plane;
				}
				else
				{
					vehicle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleCar;
				}
				vehicle.AttachedBlip.IsShortRange = true;
				vehicle.AttachedBlip.Scale = 0.75f;
				vehicle.AttachedBlip.Alpha = 255;
				vehicle.AttachedBlip.Name = "Saved Vehicle";
				vehicle.AttachedBlip.Priority = 0;
				Function.Call(Hash.SHOW_TICK_ON_BLIP, vehicle.AttachedBlip.Handle, false);
				vehicle.AttachedBlip.Color = (BlipColor)vdata.BlipColor;
				Function.Call(Hash.SHOW_HEADING_INDICATOR_ON_BLIP, vehicle.AttachedBlip.Handle, false);
			}
			if (!byPassed)
			{
				vehicle.IsInvincible = false;
				if (!vehicle.Model.IsHelicopter && unfreeze)
				{
					vehicle.IsPositionFrozen = false;
				}
			}
			return vehicle;
		}
		catch (Exception ex)
		{
			Logging.Log("ERROR VEH: " + ex.ToString());
			return null;
		}
	}

	private void OnAborted(object sender, EventArgs e)
	{
		Logging.Log("ABORTED Cleaning up...");
		try
		{
			if (GTA.UI.Screen.IsFadedOut)
			{
				GTA.UI.Screen.FadeIn(2000);
			}
			int num = 0;
			foreach (int item in SoundIdBank)
			{
				Audio.StopSound(item);
				Audio.ReleaseSound(item);
			}
			if (FobObj != null)
			{
				FobObj.IsPersistent = true;
				FobObj.Delete();
				FobObj = null;
			}
			if (Function.Call<bool>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, 69696969))
			{
				Function.Call(Hash.REMOVE_DOOR_FROM_SYSTEM, 69696969);
			}
			if (Function.Call<bool>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, 696969))
			{
				Function.Call(Hash.REMOVE_DOOR_FROM_SYSTEM, 696969);
			}
			if (Function.Call<bool>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, 6969))
			{
				Function.Call(Hash.REMOVE_DOOR_FROM_SYSTEM, 6969);
			}
			foreach (VehicleDataV1 item2 in VehicleDatabase)
			{
				if (item2.Handle != null)
				{
					DeleteBlipsOnCar(item2.Handle);
					if (item2.Handle.Exists())
					{
						OutputArgument outputArgument = new OutputArgument(item2.Handle);
						Function.Call(Hash.DELETE_VEHICLE, outputArgument);
					}
					num++;
					item2.Handle = null;
				}
			}
			foreach (KeyValuePair<Vehicle, PedTask> attachedTask in AttachedTasks)
			{
				attachedTask.Value.Clean();
				if (attachedTask.Value.Ped != null)
				{
					Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, attachedTask.Value.Ped.Handle);
					Function.Call(Hash.TASK_LEAVE_VEHICLE, attachedTask.Value.Ped.Handle, attachedTask.Key.Handle, 16);
					if (attachedTask.Value.Ped.Exists())
					{
						attachedTask.Value.Ped.IsPersistent = true;
						attachedTask.Value.Ped.Delete();
					}
				}
			}
			Logging.Log($"Cleaned up [{num}] vehicles");
		}
		catch (Exception ex)
		{
			Logging.Log("ABORT FAILED: " + ex.ToString());
		}
	}

	public static void SaveVehicleData(Vehicle veh, VehicleDataV1 dat)
	{
		dat.Hash = veh.Model;
		if (!veh.IsDead)
		{
			dat.Position = veh.Position;
		}
		dat.Rotation = veh.Rotation;
		dat.SteeringAngle = veh.SteeringAngle;
		dat.Handle = veh;
		veh.IsPersistent = true;
		if (dat.Handle.Doors[VehicleDoorIndex.FrontLeftDoor].IsBroken)
		{
			dat.FrontLeftDoorState = 2;
		}
		else if (dat.Handle.Doors[VehicleDoorIndex.FrontLeftDoor].AngleRatio > 0.1f)
		{
			dat.FrontLeftDoorState = 1;
		}
		else
		{
			dat.FrontLeftDoorState = 0;
		}
		if (dat.Handle.Doors[VehicleDoorIndex.FrontRightDoor].IsBroken)
		{
			dat.FrontRightDoorState = 2;
		}
		else if (dat.Handle.Doors[VehicleDoorIndex.FrontRightDoor].AngleRatio > 0.1f)
		{
			dat.FrontRightDoorState = 1;
		}
		else
		{
			dat.FrontRightDoorState = 0;
		}
		if (dat.Handle.Doors[VehicleDoorIndex.BackLeftDoor].IsBroken)
		{
			dat.BackLeftDoorState = 2;
		}
		else if (dat.Handle.Doors[VehicleDoorIndex.BackLeftDoor].AngleRatio > 0.1f)
		{
			dat.BackLeftDoorState = 1;
		}
		else
		{
			dat.BackLeftDoorState = 0;
		}
		if (dat.Handle.Doors[VehicleDoorIndex.BackRightDoor].IsBroken)
		{
			dat.BackRightDoorState = 2;
		}
		else if (dat.Handle.Doors[VehicleDoorIndex.BackRightDoor].AngleRatio > 0.1f)
		{
			dat.BackRightDoorState = 1;
		}
		else
		{
			dat.BackRightDoorState = 0;
		}
		if (dat.Handle.Doors[VehicleDoorIndex.Trunk].IsBroken)
		{
			dat.TrunkState = 2;
		}
		else if (dat.Handle.Doors[VehicleDoorIndex.Trunk].AngleRatio > 0.1f)
		{
			dat.TrunkState = 1;
		}
		else
		{
			dat.TrunkState = 0;
		}
		if (dat.Handle.Doors[VehicleDoorIndex.Hood].IsBroken)
		{
			dat.HoodState = 2;
		}
		else if (dat.Handle.Doors[VehicleDoorIndex.Hood].AngleRatio > 0.1f)
		{
			dat.HoodState = 1;
		}
		else
		{
			dat.HoodState = 0;
		}
		for (int i = 0; i < 10; i++)
		{
			if (!Function.Call<bool>(Hash.IS_VEHICLE_TYRE_BURST, veh.Handle, TireIndices[i], true))
			{
				if (!Function.Call<bool>(Hash.IS_VEHICLE_TYRE_BURST, veh.Handle, TireIndices[i], false))
				{
					dat.WheelStates[i] = 0;
				}
				else
				{
					dat.WheelStates[i] = 1;
				}
			}
			else
			{
				dat.WheelStates[i] = 2;
			}
		}
		for (int j = 0; j < 8; j++)
		{
			if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, veh.Handle, j))
			{
				dat.WindowStates[j] = 0;
			}
			else
			{
				dat.WindowStates[j] = 1;
			}
		}
		if (Function.Call<int>(Hash.GET_CONVERTIBLE_ROOF_STATE, veh.Handle) == 0)
		{
			dat.ConvertibleState = true;
		}
		else
		{
			dat.ConvertibleState = false;
		}
		dat.DirtLevel = veh.DirtLevel;
		dat.EngineState = veh.IsEngineRunning;
		dat.LockState = veh.LockStatus == VehicleLockStatus.CannotEnter;
		dat.AlarmState = veh.IsAlarmSet;
		dat.LightState = veh.AreLightsOn;
		dat.PrimaryColor = veh.Mods.PrimaryColor;
		dat.SecondaryColor = veh.Mods.SecondaryColor;
		dat.PearlescentColor = veh.Mods.PearlescentColor;
		dat.DashboardColor = veh.Mods.DashboardColor;
		dat.IsPrimaryCustom = veh.Mods.IsPrimaryColorCustom;
		dat.IsSecondaryCustom = veh.Mods.IsSecondaryColorCustom;
		dat.CustomPrimaryColor = veh.Mods.CustomPrimaryColor;
		dat.CustomSecondaryColor = veh.Mods.CustomSecondaryColor;
		dat.LicensePlate = veh.Mods.LicensePlate;
		dat.LightState2 = 0;
		if (veh.IsEngineRunning && veh.AreLightsOn)
		{
			if (veh.AreHighBeamsOn)
			{
				dat.LightState2 = 2;
			}
			else
			{
				dat.LightState2 = 1;
			}
		}
		dat.SirenState = 0;
		if (veh.IsSirenActive)
		{
			dat.SirenState = 1;
		}
		dat.BulletProofTires = !veh.CanTiresBurst;
		if (veh.Mods[VehicleToggleModType.Turbo].IsInstalled)
		{
			dat.Turbo = true;
		}
		else
		{
			dat.Turbo = false;
		}
		if (veh.Mods[VehicleToggleModType.TireSmoke].IsInstalled)
		{
			dat.TireSmoke = true;
		}
		else
		{
			dat.TireSmoke = false;
		}
		if (veh.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled)
		{
			dat.XenonHeadlights = true;
		}
		else
		{
			dat.XenonHeadlights = false;
		}
		dat.SpoilerVar = veh.Mods[VehicleModType.Spoilers].Variation;
		dat.Spoiler = veh.Mods[VehicleModType.Spoilers].Index;
		dat.FrontBumperVar = veh.Mods[VehicleModType.FrontBumper].Variation;
		dat.FrontBumper = veh.Mods[VehicleModType.FrontBumper].Index;
		dat.RearBumperVar = veh.Mods[VehicleModType.RearBumper].Variation;
		dat.RearBumper = veh.Mods[VehicleModType.RearBumper].Index;
		dat.SideSkirtVar = veh.Mods[VehicleModType.SideSkirt].Variation;
		dat.SideSkirt = veh.Mods[VehicleModType.SideSkirt].Index;
		dat.ExhaustVar = veh.Mods[VehicleModType.Exhaust].Variation;
		dat.Exhaust = veh.Mods[VehicleModType.Exhaust].Index;
		dat.FrameVar = veh.Mods[VehicleModType.Frame].Variation;
		dat.Frame = veh.Mods[VehicleModType.Frame].Index;
		dat.GrilleVar = veh.Mods[VehicleModType.Grille].Variation;
		dat.Grille = veh.Mods[VehicleModType.Grille].Index;
		dat.HoodVar = veh.Mods[VehicleModType.Hood].Variation;
		dat.Hood = veh.Mods[VehicleModType.Hood].Index;
		dat.FenderVar = veh.Mods[VehicleModType.Fender].Variation;
		dat.Fender = veh.Mods[VehicleModType.Fender].Index;
		dat.RightFenderVar = veh.Mods[VehicleModType.RightFender].Variation;
		dat.RightFender = veh.Mods[VehicleModType.RightFender].Index;
		dat.RoofVar = veh.Mods[VehicleModType.Roof].Variation;
		dat.Roof = veh.Mods[VehicleModType.Roof].Index;
		dat.EngineVar = veh.Mods[VehicleModType.Engine].Variation;
		dat.Engine = veh.Mods[VehicleModType.Engine].Index;
		dat.BrakesVar = veh.Mods[VehicleModType.Brakes].Variation;
		dat.Brakes = veh.Mods[VehicleModType.Brakes].Index;
		dat.TransmissionVar = veh.Mods[VehicleModType.Transmission].Variation;
		dat.Transmission = veh.Mods[VehicleModType.Transmission].Index;
		dat.HornsVar = veh.Mods[VehicleModType.Horns].Variation;
		dat.Horns = veh.Mods[VehicleModType.Horns].Index;
		dat.SuspensionVar = veh.Mods[VehicleModType.Suspension].Variation;
		dat.Suspension = veh.Mods[VehicleModType.Suspension].Index;
		dat.ArmorVar = veh.Mods[VehicleModType.Armor].Variation;
		dat.Armor = veh.Mods[VehicleModType.Armor].Index;
		dat.FrontWheelVar = veh.Mods[VehicleModType.FrontWheel].Variation;
		dat.FrontWheel = veh.Mods[VehicleModType.FrontWheel].Index;
		dat.RearWheelVar = veh.Mods[VehicleModType.RearWheel].Variation;
		dat.RearWheel = veh.Mods[VehicleModType.RearWheel].Index;
		dat.PlateHolderVar = veh.Mods[VehicleModType.PlateHolder].Variation;
		dat.PlateHolder = veh.Mods[VehicleModType.PlateHolder].Index;
		dat.VanityPlatesVar = veh.Mods[VehicleModType.VanityPlates].Variation;
		dat.VanityPlates = veh.Mods[VehicleModType.VanityPlates].Index;
		dat.TrimDesignVar = veh.Mods[VehicleModType.TrimDesign].Variation;
		dat.TrimDesign = veh.Mods[VehicleModType.TrimDesign].Index;
		dat.OrnamentsVar = veh.Mods[VehicleModType.Ornaments].Variation;
		dat.Ornaments = veh.Mods[VehicleModType.Ornaments].Index;
		dat.DashboardVar = veh.Mods[VehicleModType.Dashboard].Variation;
		dat.Dashboard = veh.Mods[VehicleModType.Dashboard].Index;
		dat.DialDesignVar = veh.Mods[VehicleModType.DialDesign].Variation;
		dat.DialDesign = veh.Mods[VehicleModType.DialDesign].Index;
		dat.DoorSpeakersVar = veh.Mods[VehicleModType.DoorSpeakers].Variation;
		dat.DoorSpeakers = veh.Mods[VehicleModType.DoorSpeakers].Index;
		dat.SeatsVar = veh.Mods[VehicleModType.Seats].Variation;
		dat.Seats = veh.Mods[VehicleModType.Seats].Index;
		dat.SteeringWheelsVar = veh.Mods[VehicleModType.SteeringWheels].Variation;
		dat.SteeringWheels = veh.Mods[VehicleModType.SteeringWheels].Index;
		dat.ColumnShifterLeversVar = veh.Mods[VehicleModType.ColumnShifterLevers].Variation;
		dat.ColumnShifterLevers = veh.Mods[VehicleModType.ColumnShifterLevers].Index;
		dat.PlaquesVar = veh.Mods[VehicleModType.Plaques].Variation;
		dat.Plaques = veh.Mods[VehicleModType.Plaques].Index;
		dat.SpeakersVar = veh.Mods[VehicleModType.Speakers].Variation;
		dat.Speakers = veh.Mods[VehicleModType.Speakers].Index;
		dat.TrunkVar = veh.Mods[VehicleModType.Trunk].Variation;
		dat.Trunk = veh.Mods[VehicleModType.Trunk].Index;
		dat.HydraulicsVar = veh.Mods[VehicleModType.Hydraulics].Variation;
		dat.Hydraulics = veh.Mods[VehicleModType.Hydraulics].Index;
		dat.EngineBlockVar = veh.Mods[VehicleModType.EngineBlock].Variation;
		dat.EngineBlock = veh.Mods[VehicleModType.EngineBlock].Index;
		dat.AirFilterVar = veh.Mods[VehicleModType.AirFilter].Variation;
		dat.AirFilter = veh.Mods[VehicleModType.AirFilter].Index;
		dat.StrutsVar = veh.Mods[VehicleModType.Struts].Variation;
		dat.Struts = veh.Mods[VehicleModType.Struts].Index;
		dat.ArchCoverVar = veh.Mods[VehicleModType.ArchCover].Variation;
		dat.ArchCover = veh.Mods[VehicleModType.ArchCover].Index;
		dat.AerialsVar = veh.Mods[VehicleModType.Aerials].Variation;
		dat.Aerials = veh.Mods[VehicleModType.Aerials].Index;
		dat.TrimVar = veh.Mods[VehicleModType.Trim].Variation;
		dat.Trim = veh.Mods[VehicleModType.Trim].Index;
		dat.TankVar = veh.Mods[VehicleModType.Tank].Variation;
		dat.Tank = veh.Mods[VehicleModType.Tank].Index;
		dat.WindowsVar = veh.Mods[VehicleModType.Windows].Variation;
		dat.Windows = veh.Mods[VehicleModType.Windows].Index;
		dat.LiveryVar = veh.Mods[VehicleModType.Livery].Variation;
		dat.Livery = veh.Mods[VehicleModType.Livery].Index;
		dat.Boost = Function.Call<int>(Hash.GET_VEHICLE_MOD, veh.Handle, 40);
		dat.NeonLightBack = veh.Mods.IsNeonLightsOn(VehicleNeonLight.Back);
		dat.NeonLightFront = veh.Mods.IsNeonLightsOn(VehicleNeonLight.Front);
		dat.NeonLightLeft = veh.Mods.IsNeonLightsOn(VehicleNeonLight.Left);
		dat.NeonLightRight = veh.Mods.IsNeonLightsOn(VehicleNeonLight.Right);
		dat.NeonLightColor = veh.Mods.NeonLightsColor;
		dat.WindowTint = veh.Mods.WindowTint;
		dat.LicensePlateStyle = veh.Mods.LicensePlateStyle;
		dat.Livery = veh.Mods.Livery;
		dat.WheelType = veh.Mods.WheelType;
		dat.TireSmokeColor = veh.Mods.TireSmokeColor;
		dat.XenonHeadlightsColor = Function.Call<int>(Hash.GET_VEHICLE_XENON_LIGHT_COLOR_INDEX, veh.Handle);
		dat.RimColor = veh.Mods.RimColor;
		dat.TrimColor = veh.Mods.TrimColor;
		for (int k = 0; k < 15; k++)
		{
			dat.Extras[k] = veh.IsExtraOn(k);
		}
	}

	public bool IsControlJustReleased(int num, GTA.Control ctrl)
	{
		return Function.Call<bool>(Hash.IS_CONTROL_JUST_RELEASED, num, (int)ctrl);
	}

	public bool IsControlPressed(int num, GTA.Control ctrl)
	{
		return Function.Call<bool>(Hash.IS_CONTROL_PRESSED, num, (int)ctrl);
	}

	public bool IsControlJustPressed(int num, GTA.Control ctrl)
	{
		return Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, num, (int)ctrl);
	}

	public bool IsDisabledControlJustReleased(int num, GTA.Control ctrl)
	{
		return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, num, (int)ctrl);
	}

	public int TimeDiffToSec(int before, int now)
	{
		return Math.Max(0, (int)(4f - (float)(now - before) / 1000f));
	}

	private void OnTick(object sender, EventArgs e)
	{
		if (Game.IsLoading)
		{
			return;
		}
		if (IsInitializing)
		{
			if (Game.GameTime - InitTime < 4500)
			{
				return;
			}
			IsInitializing = false;
			GTA.UI.Screen.FadeOut(100);
		}
		foreach (Vehicle item2 in DrawCarBlinkers.Keys.ToList())
		{
			if (Game.GameTime - DrawCarBlinkers[item2] > 500)
			{
				DrawCarBlinkers.Remove(item2);
				continue;
			}
			int boneIndex = BoneHelper.GetBoneIndex(item2, "indicator_lf");
			int boneIndex2 = BoneHelper.GetBoneIndex(item2, "indicator_rf");
			int boneIndex3 = BoneHelper.GetBoneIndex(item2, "indicator_lr");
			int boneIndex4 = BoneHelper.GetBoneIndex(item2, "indicator_rr");
			int boneIndex5 = BoneHelper.GetBoneIndex(item2, "headlight_l");
			int boneIndex6 = BoneHelper.GetBoneIndex(item2, "headlight_r");
			int boneIndex7 = BoneHelper.GetBoneIndex(item2, "taillight_l");
			int boneIndex8 = BoneHelper.GetBoneIndex(item2, "taillight_r");
			if (boneIndex != -1)
			{
				Vector3 bonePositionWorld = BoneHelper.GetBonePositionWorld(item2, boneIndex);
				Function.Call(Hash.DRAW_LIGHT_WITH_RANGEEX, bonePositionWorld.X, bonePositionWorld.Y, bonePositionWorld.Z, Color.Orange.R, Color.Orange.G, Color.Orange.B, 1f, 1f, 5f);
			}
			else if (boneIndex5 != -1)
			{
				Vector3 bonePositionWorld2 = BoneHelper.GetBonePositionWorld(item2, boneIndex5);
				Function.Call(Hash.DRAW_LIGHT_WITH_RANGEEX, bonePositionWorld2.X, bonePositionWorld2.Y, bonePositionWorld2.Z, Color.Orange.R, Color.Orange.G, Color.Orange.B, 1f, 1f, 5f);
			}
			if (boneIndex2 != -1)
			{
				Vector3 bonePositionWorld3 = BoneHelper.GetBonePositionWorld(item2, boneIndex2);
				Function.Call(Hash.DRAW_LIGHT_WITH_RANGEEX, bonePositionWorld3.X, bonePositionWorld3.Y, bonePositionWorld3.Z, Color.Orange.R, Color.Orange.G, Color.Orange.B, 1f, 1f, 5f);
			}
			else if (boneIndex6 != -1)
			{
				Vector3 bonePositionWorld4 = BoneHelper.GetBonePositionWorld(item2, boneIndex6);
				Function.Call(Hash.DRAW_LIGHT_WITH_RANGEEX, bonePositionWorld4.X, bonePositionWorld4.Y, bonePositionWorld4.Z, Color.Orange.R, Color.Orange.G, Color.Orange.B, 1f, 1f, 5f);
			}
			if (boneIndex3 != -1)
			{
				Vector3 bonePositionWorld5 = BoneHelper.GetBonePositionWorld(item2, boneIndex3);
				Function.Call(Hash.DRAW_LIGHT_WITH_RANGEEX, bonePositionWorld5.X, bonePositionWorld5.Y, bonePositionWorld5.Z, Color.Orange.R, Color.Orange.G, Color.Orange.B, 1f, 1f, 5f);
			}
			else if (boneIndex7 != -1)
			{
				Vector3 bonePositionWorld6 = BoneHelper.GetBonePositionWorld(item2, boneIndex7);
				Function.Call(Hash.DRAW_LIGHT_WITH_RANGEEX, bonePositionWorld6.X, bonePositionWorld6.Y, bonePositionWorld6.Z, Color.Orange.R, Color.Orange.G, Color.Orange.B, 1f, 1f, 5f);
			}
			if (boneIndex4 != -1)
			{
				Vector3 bonePositionWorld7 = BoneHelper.GetBonePositionWorld(item2, boneIndex4);
				Function.Call(Hash.DRAW_LIGHT_WITH_RANGEEX, bonePositionWorld7.X, bonePositionWorld7.Y, bonePositionWorld7.Z, Color.Orange.R, Color.Orange.G, Color.Orange.B, 1f, 1f, 5f);
			}
			else if (boneIndex8 != -1)
			{
				Vector3 bonePositionWorld8 = BoneHelper.GetBonePositionWorld(item2, boneIndex8);
				Function.Call(Hash.DRAW_LIGHT_WITH_RANGEEX, bonePositionWorld8.X, bonePositionWorld8.Y, bonePositionWorld8.Z, Color.Orange.R, Color.Orange.G, Color.Orange.B, 1f, 1f, 5f);
			}
		}
		if (ModSettings.ReturnVehiclesToSafeLocation)
		{
			if (Game.Player.Character.IsDead && CanDie)
			{
				CanDie = false;
				HasDied = true;
				PromptTime = Game.GameTime;
				ResetCars = true;
				DieArrestedAt = Game.Player.Character.Position;
			}
			if (Function.Call<bool>(Hash.IS_PLAYER_BEING_ARRESTED, Game.Player, false) && CanArrest)
			{
				CanArrest = false;
				GotArrested = true;
				PromptTime = Game.GameTime;
				ResetCars = true;
				DieArrestedAt = Game.Player.Character.Position;
			}
			if (Game.Player.Character.IsAlive)
			{
				CanDie = true;
				if (HasDied)
				{
					if (Game.GameTime - PromptTime > 5000 && ResetCars)
					{
						Vehicle[] nearbyVehicles = World.GetNearbyVehicles(DieArrestedAt, 100f);
						foreach (Vehicle vehicle in nearbyVehicles)
						{
							if (!AttachedVehicles.ContainsKey(vehicle))
							{
								continue;
							}
							if (vehicle.IsAlive && !vehicle.IsDead)
							{
								if (AttachedVehicles[vehicle].SafeSpawnSet)
								{
									vehicle.Position = AttachedVehicles[vehicle].SafeSpawn;
									vehicle.Rotation = AttachedVehicles[vehicle].SafeRotation;
								}
							}
							else
							{
								VehicleDataV1 vdata = AttachedVehicles[vehicle];
								AttachedVehicles.Remove(vehicle);
								vehicle.Delete();
								CreateVehicle(vdata, null, byPassed: false, unfreeze: true, atSafeSpot: true);
							}
						}
						ResetCars = false;
					}
					if (Game.GameTime - PromptTime > 5000)
					{
						GTA.UI.Screen.ShowHelpTextThisFrame("Your vehicles have been returned to their set spawn.", beep: false);
					}
					if (Game.GameTime - PromptTime > 10000)
					{
						HasDied = false;
					}
				}
			}
			if (!Function.Call<bool>(Hash.IS_PLAYER_BEING_ARRESTED, Game.Player, false))
			{
				CanArrest = true;
				if (GotArrested)
				{
					if (Game.GameTime - PromptTime > 5000 && ResetCars)
					{
						Vehicle[] nearbyVehicles = World.GetNearbyVehicles(DieArrestedAt, 100f);
						foreach (Vehicle vehicle2 in nearbyVehicles)
						{
							if (!AttachedVehicles.ContainsKey(vehicle2))
							{
								continue;
							}
							if (vehicle2.IsAlive && !vehicle2.IsDead)
							{
								if (AttachedVehicles[vehicle2].SafeSpawnSet)
								{
									vehicle2.Position = AttachedVehicles[vehicle2].SafeSpawn;
									vehicle2.Rotation = AttachedVehicles[vehicle2].SafeRotation;
								}
							}
							else
							{
								VehicleDataV1 vdata2 = AttachedVehicles[vehicle2];
								AttachedVehicles.Remove(vehicle2);
								vehicle2.Delete();
								CreateVehicle(vdata2, null, byPassed: false, unfreeze: true, atSafeSpot: true);
							}
						}
						ResetCars = false;
					}
					if (Game.GameTime - PromptTime > 5000)
					{
						GTA.UI.Screen.ShowHelpTextThisFrame("Your vehicles have been returned to their set spawn.", beep: false);
					}
					if (Game.GameTime - PromptTime > 10000)
					{
						GotArrested = false;
					}
				}
			}
		}
		if (ModSettings.EnableRemoteSystem && DoingFobAnim && Game.GameTime - WaitTime > 1750)
		{
			if (FobObj != null)
			{
				FobObj.IsPersistent = true;
				FobObj.Delete();
				FobObj = null;
			}
			if (StoredWeapon != WeaponHash.Unarmed)
			{
				Function.Call(Hash.SET_CURRENT_PED_WEAPON, Game.Player.Character.Handle, StoredWeapon, true);
			}
			DoingFobAnim = false;
		}
		bool flag = Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 0);
		if (ModSettings.EnableRemoteSystem && !Game.IsCutsceneActive && !DoingFobAnim && Game.Player.Character.CurrentVehicle == null && PressToSaveVeh != null)
		{
			if (!flag)
			{
				if (Game.IsControlPressed(GTA.Control.ScriptPadRight) && !GUI.Phone.IsOn && !DoXThing)
				{
					DoXThing = true;
					XTime = Game.GameTime;
				}
				if (Game.IsControlJustReleased(GTA.Control.ScriptPadRight) && !GUI.Phone.IsOn)
				{
					DoXThing = false;
					if (PressToSaveVeh != null && PressToSaveVeh.Handle != null)
					{
						if (PressToSaveVeh.Handle.LockStatus == VehicleLockStatus.CannotEnter)
						{
							PressToSaveVeh.Handle.LockStatus = VehicleLockStatus.Unlocked;
						}
						else
						{
							PressToSaveVeh.Handle.LockStatus = VehicleLockStatus.CannotEnter;
							PressToSaveVeh.SafeRotation = PressToSaveVeh.Handle.Rotation;
							PressToSaveVeh.SafeSpawn = PressToSaveVeh.Handle.Position;
							PressToSaveVeh.SafeSpawnSet = true;
						}
						Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "CONFIRM_BEEP", PressToSaveVeh.Handle.Handle, "HUD_MINI_GAME_SOUNDSET", 1, 0);
						if (!DoingFobAnim)
						{
							int num = Function.Call<int>(Hash.GET_CAM_ACTIVE_VIEW_MODE_CONTEXT);
							if (Function.Call<int>(Hash.GET_CAM_VIEW_MODE_FOR_CONTEXT, num) == 4)
							{
								Game.Player.Character.Task.PlayAnimation("ANIM@MP_PLAYER_INTMENU@KEY_FOB@", "FOB_CLICK_FP", 8f, -2f, -1, AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary, 0f);
							}
							else
							{
								Game.Player.Character.Task.PlayAnimation("ANIM@MP_PLAYER_INTMENU@KEY_FOB@", "FOB_CLICK", 8f, -2f, -1, AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary, 0f);
							}
							Model model = new Model("lr_prop_carkey_fob");
							model.Request(5000);
							FobObj = World.CreateProp(model, Game.Player.Character.Position, dynamic: false, placeOnGround: false);
							int num2 = Function.Call<int>(Hash.GET_PED_BONE_INDEX, Game.Player.Character.Handle, Bone.PHRightHand);
							if (FobObj != null)
							{
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, FobObj.Handle, Game.Player.Character.Handle, num2, 0f, 0f, 0f, 0f, 0f, 0f, true, false, false, false, 2, true);
							}
							WaitTime = Game.GameTime;
							StoredWeapon = Game.Player.Character.Weapons.Current.Hash;
							Function.Call(Hash.SET_CURRENT_PED_WEAPON, Game.Player.Character.Handle, WeaponHash.Unarmed, true);
							DoingFobAnim = true;
						}
						DrawCarBlinkers[PressToSaveVeh.Handle] = Game.GameTime;
					}
				}
			}
			else
			{
				if (Game.IsControlPressed(GTA.Control.Context) && !GUI.Phone.IsOn && !DoXThing)
				{
					DoXThing = true;
					XTime = Game.GameTime;
				}
				if (Game.IsControlJustReleased(GTA.Control.Context) && !GUI.Phone.IsOn)
				{
					DoXThing = false;
					if (PressToSaveVeh != null && PressToSaveVeh.Handle != null)
					{
						if (PressToSaveVeh.Handle.LockStatus == VehicleLockStatus.CannotEnter)
						{
							PressToSaveVeh.Handle.LockStatus = VehicleLockStatus.Unlocked;
						}
						else
						{
							PressToSaveVeh.Handle.LockStatus = VehicleLockStatus.CannotEnter;
							PressToSaveVeh.SafeRotation = PressToSaveVeh.Handle.Rotation;
							PressToSaveVeh.SafeSpawn = PressToSaveVeh.Handle.Position;
							PressToSaveVeh.SafeSpawnSet = true;
						}
						Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "CONFIRM_BEEP", PressToSaveVeh.Handle.Handle, "HUD_MINI_GAME_SOUNDSET", 1, 0);
						if (!DoingFobAnim)
						{
							int num3 = Function.Call<int>(Hash.GET_CAM_ACTIVE_VIEW_MODE_CONTEXT);
							if (Function.Call<int>(Hash.GET_CAM_VIEW_MODE_FOR_CONTEXT, num3) == 4)
							{
								Game.Player.Character.Task.PlayAnimation("ANIM@MP_PLAYER_INTMENU@KEY_FOB@", "FOB_CLICK_FP", 8f, -2f, -1, AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary, 0f);
							}
							else
							{
								Game.Player.Character.Task.PlayAnimation("ANIM@MP_PLAYER_INTMENU@KEY_FOB@", "FOB_CLICK", 8f, -2f, -1, AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary, 0f);
							}
							Model model2 = new Model("lr_prop_carkey_fob");
							model2.Request(5000);
							FobObj = World.CreateProp(model2, Game.Player.Character.Position, dynamic: false, placeOnGround: false);
							int num4 = Function.Call<int>(Hash.GET_PED_BONE_INDEX, Game.Player.Character.Handle, Bone.PHRightHand);
							if (FobObj != null)
							{
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, FobObj.Handle, Game.Player.Character.Handle, num4, 0f, 0f, 0f, 0f, 0f, 0f, true, false, false, false, 2, true);
							}
							WaitTime = Game.GameTime;
							StoredWeapon = Game.Player.Character.Weapons.Current.Hash;
							Function.Call(Hash.SET_CURRENT_PED_WEAPON, Game.Player.Character.Handle, WeaponHash.Unarmed, true);
							DoingFobAnim = true;
						}
						DrawCarBlinkers[PressToSaveVeh.Handle] = Game.GameTime;
					}
				}
			}
			if (DoXThing && Game.GameTime - XTime > 1250)
			{
				if (PressToSaveVeh != null && PressToSaveVeh.Handle != null)
				{
					if (PressToSaveVeh.Handle.IsEngineRunning)
					{
						Function.Call(Hash.SET_VEHICLE_ENGINE_ON, PressToSaveVeh.Handle.Handle, false, true, false);
					}
					else
					{
						Function.Call(Hash.SET_VEHICLE_ENGINE_ON, PressToSaveVeh.Handle.Handle, true, true, false);
					}
					Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "CONFIRM_BEEP", PressToSaveVeh.Handle.Handle, "HUD_MINI_GAME_SOUNDSET", 1, 0);
					if (!DoingFobAnim)
					{
						int num5 = Function.Call<int>(Hash.GET_CAM_ACTIVE_VIEW_MODE_CONTEXT);
						if (Function.Call<int>(Hash.GET_CAM_VIEW_MODE_FOR_CONTEXT, num5) == 4)
						{
							Game.Player.Character.Task.PlayAnimation("ANIM@MP_PLAYER_INTMENU@KEY_FOB@", "FOB_CLICK_FP", 8f, -2f, -1, AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary, 0f);
						}
						else
						{
							Game.Player.Character.Task.PlayAnimation("ANIM@MP_PLAYER_INTMENU@KEY_FOB@", "FOB_CLICK", 8f, -2f, -1, AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary, 0f);
						}
						Model model3 = new Model("lr_prop_carkey_fob");
						model3.Request(5000);
						FobObj = World.CreateProp(model3, Game.Player.Character.Position, dynamic: false, placeOnGround: false);
						int num6 = Function.Call<int>(Hash.GET_PED_BONE_INDEX, Game.Player.Character.Handle, Bone.PHRightHand);
						if (FobObj != null)
						{
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, FobObj.Handle, Game.Player.Character.Handle, num6, 0f, 0f, 0f, 0f, 0f, 0f, true, false, false, false, 2, true);
						}
						WaitTime = Game.GameTime;
						StoredWeapon = Game.Player.Character.Weapons.Current.Hash;
						Function.Call(Hash.SET_CURRENT_PED_WEAPON, Game.Player.Character.Handle, WeaponHash.Unarmed, true);
						DoingFobAnim = true;
					}
					DrawCarBlinkers[PressToSaveVeh.Handle] = Game.GameTime;
				}
				DoXThing = false;
			}
		}
		if (!Game.IsCutsceneActive && !GUI.Phone.IsOn)
		{
			if (Game.GameTime - RaycastTick > 450)
			{
				PressToSaveVeh = null;
				if (Game.Player.Character.CurrentVehicle == null)
				{
					ep_priv = GameplayCamera.Position;
					ep2_priv = MathHelper.ScreenRelToWorld(GameplayCamera.Position, GameplayCamera.Rotation, new Vector2(0.5f, 0.5f));
					ep3_priv = ep2_priv - ep_priv;
					from_priv = ep_priv + ep3_priv * 0.05f;
					to_priv = ep_priv + ep3_priv * 4f;
					up_priv = GameplayCamera.UpVector.Normalized;
					right_priv = GameplayCamera.RightVector.Normalized;
					Ped character = Game.Player.Character;
					RaycastResult raycastResult = World.Raycast(from_priv, to_priv, IntersectFlags.Map | IntersectFlags.Vehicles, character);
					RaycastResult raycastResult2 = World.Raycast(from_priv + up_priv * 0.5f, to_priv + up_priv * 0.2f, IntersectFlags.Map | IntersectFlags.Vehicles, character);
					RaycastResult raycastResult3 = World.Raycast(from_priv + up_priv * -0.5f, to_priv + up_priv * -0.2f, IntersectFlags.Map | IntersectFlags.Vehicles, character);
					RaycastResult raycastResult4 = World.Raycast(from_priv + right_priv * 0.5f, to_priv + right_priv * 0.2f, IntersectFlags.Map | IntersectFlags.Vehicles, character);
					RaycastResult raycastResult5 = World.Raycast(from_priv + right_priv * -0.5f, to_priv + right_priv * -0.2f, IntersectFlags.Map | IntersectFlags.Vehicles, character);
					Vehicle vehicle3 = null;
					if (raycastResult.HitEntity != null)
					{
						foreach (VehicleDataV1 item3 in VehicleDatabase)
						{
							if (!(item3.Handle == raycastResult.HitEntity))
							{
								continue;
							}
							if (vehicle3 == null)
							{
								if (Game.Player.Character.Position.DistanceTo(item3.Handle.Position) < 50f)
								{
									vehicle3 = (Vehicle)raycastResult.HitEntity;
								}
							}
							else if (Game.Player.Character.Position.DistanceTo(item3.Handle.Position) < Game.Player.Character.Position.DistanceTo(vehicle3.Position))
							{
								vehicle3 = (Vehicle)raycastResult.HitEntity;
							}
							break;
						}
					}
					if (raycastResult2.HitEntity != null)
					{
						foreach (VehicleDataV1 item4 in VehicleDatabase)
						{
							if (!(item4.Handle == raycastResult2.HitEntity))
							{
								continue;
							}
							if (vehicle3 == null)
							{
								if (Game.Player.Character.Position.DistanceTo(item4.Handle.Position) < 50f)
								{
									vehicle3 = (Vehicle)raycastResult2.HitEntity;
								}
							}
							else if (Game.Player.Character.Position.DistanceTo(item4.Handle.Position) < Game.Player.Character.Position.DistanceTo(vehicle3.Position))
							{
								vehicle3 = (Vehicle)raycastResult2.HitEntity;
							}
							break;
						}
					}
					if (raycastResult3.HitEntity != null)
					{
						foreach (VehicleDataV1 item5 in VehicleDatabase)
						{
							if (!(item5.Handle == raycastResult3.HitEntity))
							{
								continue;
							}
							if (vehicle3 == null)
							{
								if (Game.Player.Character.Position.DistanceTo(item5.Handle.Position) < 50f)
								{
									vehicle3 = (Vehicle)raycastResult3.HitEntity;
								}
							}
							else if (Game.Player.Character.Position.DistanceTo(item5.Handle.Position) < Game.Player.Character.Position.DistanceTo(vehicle3.Position))
							{
								vehicle3 = (Vehicle)raycastResult3.HitEntity;
							}
							break;
						}
					}
					if (raycastResult4.HitEntity != null)
					{
						foreach (VehicleDataV1 item6 in VehicleDatabase)
						{
							if (!(item6.Handle == raycastResult4.HitEntity))
							{
								continue;
							}
							if (vehicle3 == null)
							{
								if (Game.Player.Character.Position.DistanceTo(item6.Handle.Position) < 50f)
								{
									vehicle3 = (Vehicle)raycastResult4.HitEntity;
								}
							}
							else if (Game.Player.Character.Position.DistanceTo(item6.Handle.Position) < Game.Player.Character.Position.DistanceTo(vehicle3.Position))
							{
								vehicle3 = (Vehicle)raycastResult4.HitEntity;
							}
							break;
						}
					}
					if (raycastResult5.HitEntity != null)
					{
						foreach (VehicleDataV1 item7 in VehicleDatabase)
						{
							if (!(item7.Handle == raycastResult5.HitEntity))
							{
								continue;
							}
							if (vehicle3 == null)
							{
								if (Game.Player.Character.Position.DistanceTo(item7.Handle.Position) < 50f)
								{
									vehicle3 = (Vehicle)raycastResult5.HitEntity;
								}
							}
							else if (Game.Player.Character.Position.DistanceTo(item7.Handle.Position) < Game.Player.Character.Position.DistanceTo(vehicle3.Position))
							{
								vehicle3 = (Vehicle)raycastResult5.HitEntity;
							}
							break;
						}
					}
					if (vehicle3 != null)
					{
						PressToSaveVeh = AttachedVehicles[vehicle3];
						string text = Function.Call<string>(Hash.GET_DISPLAY_NAME_FROM_VEHICLE_MODEL, PressToSaveVeh.Hash);
						SaveCarString = Function.Call<string>(Hash.GET_FILENAME_FOR_AUDIO_CONVERSATION, text);
						if (SaveCarString == "NULL")
						{
							SaveCarString = PressToSaveVeh.Handle.Mods.LicensePlate;
						}
					}
					else
					{
						SteeringAngleRemote = 0f;
					}
					if (PressToSaveVeh == null && Function.Call<bool>(Hash.IS_GAMEPLAY_HINT_ACTIVE))
					{
						Function.Call(Hash.STOP_GAMEPLAY_HINT, false);
					}
					RaycastTick = Game.GameTime;
				}
				else
				{
					PressToSaveVeh = null;
					if (Function.Call<bool>(Hash.IS_GAMEPLAY_HINT_ACTIVE))
					{
						Function.Call(Hash.STOP_GAMEPLAY_HINT, false);
					}
				}
			}
			if (DoGetOutHint && Game.GameTime - GetOutHint > 1500)
			{
				if (Function.Call<bool>(Hash.IS_GAMEPLAY_HINT_ACTIVE))
				{
					Function.Call(Hash.STOP_GAMEPLAY_HINT, false);
				}
				DoGetOutHint = false;
			}
			if (ModSettings.EnableRemoteSystem && PressToSaveVeh != null && PressToSaveVeh.Handle != null)
			{
				Game.DisableControlThisFrame(GTA.Control.Context);
				Game.DisableControlThisFrame(GTA.Control.ContextSecondary);
				Game.DisableControlThisFrame(GTA.Control.Phone);
				Game.DisableControlThisFrame(GTA.Control.PhoneDown);
				Game.DisableControlThisFrame(GTA.Control.FrontendDown);
				Game.DisableControlThisFrame(GTA.Control.FrontendUp);
				Game.DisableControlThisFrame(GTA.Control.FrontendLeft);
				Game.DisableControlThisFrame(GTA.Control.FrontendRight);
				Game.DisableControlThisFrame(GTA.Control.CharacterWheel);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 288, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 289, true);
				if (ModSettings.EnableRemoteMovement)
				{
					if (!flag)
					{
						GTA.UI.Screen.ShowHelpTextThisFrame("~b~[" + SaveCarString + "]~s~~n~Press ~INPUT_SCRIPT_PAD_RIGHT~ to " + ((PressToSaveVeh.Handle.LockStatus == VehicleLockStatus.CannotEnter) ? "~g~unlock~s~" : "~r~lock~s~") + "~n~Hold  ~INPUT_SCRIPT_PAD_RIGHT~ to remote " + (PressToSaveVeh.Handle.IsEngineRunning ? "~r~stop~s~" : "~g~start~s~") + "~n~Hold ~INPUTGROUP_FRONTEND_DPAD_UD~ ~INPUT_SPRINT~ ~INPUT_JUMP~ to move", beep: false);
					}
					else
					{
						GTA.UI.Screen.ShowHelpTextThisFrame("~b~[" + SaveCarString + "]~s~~n~Press ~INPUT_CONTEXT~ to " + ((PressToSaveVeh.Handle.LockStatus == VehicleLockStatus.CannotEnter) ? "~g~unlock~s~" : "~r~lock~s~") + "~n~Hold  ~INPUT_CONTEXT~ to remote " + (PressToSaveVeh.Handle.IsEngineRunning ? "~r~stop~s~" : "~g~start~s~") + "~n~Hold ~INPUTGROUP_FRONTEND_DPAD_ALL~ to move", beep: false);
					}
				}
				else if (!flag)
				{
					GTA.UI.Screen.ShowHelpTextThisFrame("~b~[" + SaveCarString + "]~s~~n~Press ~INPUT_SCRIPT_PAD_RIGHT~ to " + ((PressToSaveVeh.Handle.LockStatus == VehicleLockStatus.CannotEnter) ? "~g~unlock~s~" : "~r~lock~s~") + "~n~Hold  ~INPUT_SCRIPT_PAD_RIGHT~ to remote " + (PressToSaveVeh.Handle.IsEngineRunning ? "~r~stop~s~" : "~g~start~s~"), beep: false);
				}
				else
				{
					GTA.UI.Screen.ShowHelpTextThisFrame("~b~[" + SaveCarString + "]~s~~n~Press ~INPUT_CONTEXT~ to " + ((PressToSaveVeh.Handle.LockStatus == VehicleLockStatus.CannotEnter) ? "~g~unlock~s~" : "~r~lock~s~") + "~n~Hold  ~INPUT_CONTEXT~ to remote " + (PressToSaveVeh.Handle.IsEngineRunning ? "~r~stop~s~" : "~g~start~s~"), beep: false);
				}
				if (ModSettings.EnableRemoteMovement)
				{
					if (PressToSaveVeh.Handle.IsEngineRunning)
					{
						if (Game.IsControlPressed(GTA.Control.PhoneDown))
						{
							if (!flag)
							{
								if (SteeringAngleRemote < 20f)
								{
									SteeringAngleRemote += 1f;
								}
								PressToSaveVeh.Handle.SteeringAngle = SteeringAngleRemote;
								if (PressToSaveVeh.Handle != LastHinted)
								{
									Function.Call(Hash.STOP_GAMEPLAY_HINT, false);
								}
								Function.Call(Hash.SET_GAMEPLAY_VEHICLE_HINT, PressToSaveVeh.Handle, 0f, 0f, 0.5f, true, -1, 1000, 1000);
								GetOutHint = Game.GameTime;
								DoGetOutHint = true;
							}
							else
							{
								PressToSaveVeh.Handle.ForwardSpeed = -1f;
								if (PressToSaveVeh.Handle != LastHinted)
								{
									Function.Call(Hash.STOP_GAMEPLAY_HINT, false);
								}
								Function.Call(Hash.SET_GAMEPLAY_VEHICLE_HINT, PressToSaveVeh.Handle, 0f, 0f, 0.5f, true, -1, 1000, 1000);
								GetOutHint = Game.GameTime;
								DoGetOutHint = true;
							}
						}
						if (Game.IsControlPressed(GTA.Control.PhoneLeft))
						{
							if (!flag)
							{
								if (SteeringAngleRemote > -100f)
								{
									SteeringAngleRemote -= 10f;
								}
								PressToSaveVeh.Handle.SteeringAngle = SteeringAngleRemote;
								if (PressToSaveVeh.Handle != LastHinted)
								{
									Function.Call(Hash.STOP_GAMEPLAY_HINT, false);
								}
								Function.Call(Hash.SET_GAMEPLAY_VEHICLE_HINT, PressToSaveVeh.Handle, 10f, 30f, 44f, true, -1, 1000, 1000);
								GetOutHint = Game.GameTime;
								DoGetOutHint = true;
							}
							else
							{
								PressToSaveVeh.Handle.ForwardSpeed = 10f;
								if (PressToSaveVeh.Handle != LastHinted)
								{
									Function.Call(Hash.STOP_GAMEPLAY_HINT, false);
								}
								Function.Call(Hash.SET_GAMEPLAY_VEHICLE_HINT, PressToSaveVeh.Handle, 100f, 100f, 50f, true, -1, 1000, 1000);
								GetOutHint = Game.GameTime;
								DoGetOutHint = true;
							}
						}
						if (Game.IsControlPressed(GTA.Control.PhoneUp) && flag)
						{
							if (SteeringAngleRemote < 50f)
							{
								SteeringAngleRemote += 40f;
							}
							PressToSaveVeh.Handle.SteeringAngle = SteeringAngleRemote;
							if (PressToSaveVeh.Handle != LastHinted)
							{
								Function.Call(Hash.STOP_GAMEPLAY_HINT, false);
							}
							Function.Call(Hash.SET_GAMEPLAY_VEHICLE_HINT, PressToSaveVeh.Handle, 0f, 0f, 0.5f, true, -1, 1000, 1000);
							GetOutHint = Game.GameTime;
							DoGetOutHint = true;
						}
						if (Game.IsControlPressed(GTA.Control.FrontendRight) && flag)
						{
							if (SteeringAngleRemote > -20f)
							{
								SteeringAngleRemote -= 5f;
							}
							PressToSaveVeh.Handle.SteeringAngle = SteeringAngleRemote;
							if (PressToSaveVeh.Handle != LastHinted)
							{
								Function.Call(Hash.STOP_GAMEPLAY_HINT, false);
							}
							Function.Call(Hash.SET_GAMEPLAY_VEHICLE_HINT, PressToSaveVeh.Handle, 0f, 0f, 0.5f, true, -1, 1000, 1000);
							GetOutHint = Game.GameTime;
							DoGetOutHint = true;
						}
						if (Game.IsControlPressed(GTA.Control.Sprint) && !flag)
						{
							PressToSaveVeh.Handle.ForwardSpeed = 100f;
							if (PressToSaveVeh.Handle != LastHinted)
							{
								Function.Call(Hash.STOP_GAMEPLAY_HINT, false);
							}
							Function.Call(Hash.SET_GAMEPLAY_VEHICLE_HINT, PressToSaveVeh.Handle, 0f, 0f, 0.5f, true, -1, 1000, 1000);
							GetOutHint = Game.GameTime;
							DoGetOutHint = true;
						}
						if (Game.IsControlPressed(GTA.Control.Jump) && !flag)
						{
							PressToSaveVeh.Handle.ForwardSpeed = -100f;
							if (PressToSaveVeh.Handle != LastHinted)
							{
								Function.Call(Hash.STOP_GAMEPLAY_HINT, false);
							}
							Function.Call(Hash.SET_GAMEPLAY_VEHICLE_HINT, PressToSaveVeh.Handle, 0f, 0f, 0.5f, true, -1, 1000, 1000);
							GetOutHint = Game.GameTime;
							DoGetOutHint = true;
						}
					}
					if (PressToSaveVeh.Handle != LastHinted)
					{
						Function.Call(Hash.STOP_GAMEPLAY_HINT, false);
					}
					LastHinted = PressToSaveVeh.Handle;
				}
			}
		}
		if (DrawTrace && ModSettings.ShowVehicleOutlines && GUI.Phone.activatedCar != null && GUI.Phone.activatedCar.Handle != null && GUI.Phone.activatedCar.Handle != Game.Player.Character.CurrentVehicle)
		{
			StartTrace = Game.Player.Character.Position;
			new OutputArgument();
			new OutputArgument();
			(Vector3 rearBottomLeft, Vector3 frontTopRight) dimensions = GUI.Phone.activatedCar.Handle.Model.Dimensions;
			Vector3 item = dimensions.rearBottomLeft;
			Vector3 size = dimensions.frontTopRight - item;
			EndTrace = GUI.Phone.activatedCar.Handle.Position;
			Vector3 position = Game.Player.Character.Position;
			int num7 = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 3f, Game.GenerateHash("prop_phone_ing"), false, false, false);
			if (Function.Call<bool>(Hash.DOES_ENTITY_EXIST, num7))
			{
				StartTrace = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, num7, false);
				Function.Call(Hash.DRAW_LINE, StartTrace.X, StartTrace.Y, StartTrace.Z, EndTrace.X, EndTrace.Y, EndTrace.Z, 255, 255, 255, 255);
			}
			else
			{
				num7 = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 3f, Game.GenerateHash("prop_phone_ing_02"), false, false, false);
				if (Function.Call<bool>(Hash.DOES_ENTITY_EXIST, num7))
				{
					StartTrace = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, num7, false);
					Function.Call(Hash.DRAW_LINE, StartTrace.X, StartTrace.Y, StartTrace.Z, EndTrace.X, EndTrace.Y, EndTrace.Z, 255, 255, 255, 255);
				}
				else
				{
					num7 = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 3f, Game.GenerateHash("prop_phone_ing_03"), false, false, false);
					if (Function.Call<bool>(Hash.DOES_ENTITY_EXIST, num7))
					{
						StartTrace = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, num7, false);
						Function.Call(Hash.DRAW_LINE, StartTrace.X, StartTrace.Y, StartTrace.Z, EndTrace.X, EndTrace.Y, EndTrace.Z, 255, 255, 255, 255);
					}
				}
			}
			GraphicsManager.DrawSkeleton(EndTrace, size, GUI.Phone.activatedCar.Handle.Rotation);
		}
		if (IsModLoading)
		{
			GTA.UI.Screen.ShowSubtitle($"Loading Advanced Persistence... [{Game.GameTime - StartedTime}ms]", 10000);
		}
		if (!Game.IsMissionActive)
		{
			Function.Call(Hash.SET_THIS_SCRIPT_CAN_REMOVE_BLIPS_CREATED_BY_ANY_SCRIPT, true);
		}
		if (Game.GameTime - AudioTick > 1000)
		{
			if (SoundIdBank.Count > 0)
			{
				foreach (int x in SoundIdBank.ToList())
				{
					if (Audio.HasSoundFinished(x))
					{
						Audio.ReleaseSound(x);
						SoundIdBank.RemoveAll((int y) => y == x);
					}
				}
			}
			AudioTick = Game.GameTime;
		}
		if (switchDetect && Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS))
		{
			Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;
			if (currentVehicle != null && AttachedVehicles.ContainsKey(currentVehicle))
			{
				if (Game.Player.Character.Model == PedHash.Franklin)
				{
					SwitchedCars[1] = AttachedVehicles[currentVehicle].Id;
				}
				else if (Game.Player.Character.Model == PedHash.Michael)
				{
					SwitchedCars[2] = AttachedVehicles[currentVehicle].Id;
				}
				else if (Game.Player.Character.Model == PedHash.Trevor)
				{
					SwitchedCars[3] = AttachedVehicles[currentVehicle].Id;
				}
			}
			waitForSwitch = true;
			switchDetect = false;
		}
		if (waitForSwitch && !Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS))
		{
			Vehicle currentVehicle2 = Game.Player.Character.CurrentVehicle;
			if (currentVehicle2 != null)
			{
				string id = "null";
				if (Game.Player.Character.Model == PedHash.Franklin)
				{
					if (SwitchedCars.ContainsKey(1))
					{
						id = SwitchedCars[1];
						SwitchedCars.Remove(1);
						VehicleDataV1 vehicleDataV = VehicleDatabase.FirstOrDefault((VehicleDataV1 x) => x.Id == id);
						if (vehicleDataV != null)
						{
							if (currentVehicle2 == vehicleDataV.Handle)
							{
								CreateVehicle(vehicleDataV, currentVehicle2, byPassed: true);
							}
							else if (vehicleDataV.Handle != null)
							{
								if (vehicleDataV.Handle.Model != currentVehicle2.Model)
								{
									AttachedVehicles.Remove(vehicleDataV.Handle);
									if (vehicleDataV.Handle.Exists())
									{
										DeleteBlipsOnCar(vehicleDataV.Handle);
										vehicleDataV.Handle.Delete();
									}
									CreateVehicle(vehicleDataV, currentVehicle2);
								}
								else
								{
									AttachedVehicles.Remove(vehicleDataV.Handle);
									if (vehicleDataV.Handle.Exists())
									{
										DeleteBlipsOnCar(vehicleDataV.Handle);
										vehicleDataV.Handle.Delete();
									}
									CreateVehicle(vehicleDataV, currentVehicle2, byPassed: true);
								}
							}
							else
							{
								CreateVehicle(vehicleDataV, currentVehicle2);
							}
						}
					}
				}
				else if (Game.Player.Character.Model == PedHash.Michael)
				{
					if (SwitchedCars.ContainsKey(2))
					{
						id = SwitchedCars[2];
						SwitchedCars.Remove(2);
						VehicleDataV1 vehicleDataV2 = VehicleDatabase.FirstOrDefault((VehicleDataV1 x) => x.Id == id);
						if (vehicleDataV2 != null)
						{
							if (currentVehicle2 == vehicleDataV2.Handle)
							{
								CreateVehicle(vehicleDataV2, currentVehicle2, byPassed: true);
							}
							else if (vehicleDataV2.Handle != null)
							{
								if (vehicleDataV2.Handle.Model != currentVehicle2.Model)
								{
									AttachedVehicles.Remove(vehicleDataV2.Handle);
									if (vehicleDataV2.Handle.Exists())
									{
										DeleteBlipsOnCar(vehicleDataV2.Handle);
										vehicleDataV2.Handle.Delete();
									}
									CreateVehicle(vehicleDataV2, currentVehicle2);
								}
								else
								{
									AttachedVehicles.Remove(vehicleDataV2.Handle);
									if (vehicleDataV2.Handle.Exists())
									{
										DeleteBlipsOnCar(vehicleDataV2.Handle);
										vehicleDataV2.Handle.Delete();
									}
									CreateVehicle(vehicleDataV2, currentVehicle2, byPassed: true);
								}
							}
							else
							{
								CreateVehicle(vehicleDataV2, currentVehicle2);
							}
						}
					}
				}
				else if (Game.Player.Character.Model == PedHash.Trevor && SwitchedCars.ContainsKey(3))
				{
					id = SwitchedCars[3];
					SwitchedCars.Remove(3);
					VehicleDataV1 vehicleDataV3 = VehicleDatabase.FirstOrDefault((VehicleDataV1 x) => x.Id == id);
					if (vehicleDataV3 != null)
					{
						if (currentVehicle2 == vehicleDataV3.Handle)
						{
							CreateVehicle(vehicleDataV3, currentVehicle2, byPassed: true);
						}
						else if (vehicleDataV3.Handle != null)
						{
							if (vehicleDataV3.Handle.Model != currentVehicle2.Model)
							{
								AttachedVehicles.Remove(vehicleDataV3.Handle);
								if (vehicleDataV3.Handle.Exists())
								{
									DeleteBlipsOnCar(vehicleDataV3.Handle);
									vehicleDataV3.Handle.Delete();
								}
								CreateVehicle(vehicleDataV3, currentVehicle2);
							}
							else
							{
								AttachedVehicles.Remove(vehicleDataV3.Handle);
								if (vehicleDataV3.Handle.Exists())
								{
									DeleteBlipsOnCar(vehicleDataV3.Handle);
									vehicleDataV3.Handle.Delete();
								}
								CreateVehicle(vehicleDataV3, currentVehicle2, byPassed: true);
							}
						}
						else
						{
							CreateVehicle(vehicleDataV3, currentVehicle2);
						}
					}
				}
			}
			else
			{
				if (Game.Player.Character.Model == PedHash.Franklin)
				{
					if (SwitchedCars.ContainsKey(1))
					{
						string id = SwitchedCars[1];
						SwitchedCars.Remove(1);
						VehicleDataV1 vehicleDataV4 = VehicleDatabase.FirstOrDefault((VehicleDataV1 x) => x.Id == id);
						if (vehicleDataV4 != null)
						{
							if (vehicleDataV4.Handle == null)
							{
								CreateVehicle(vehicleDataV4);
							}
							else if (!vehicleDataV4.Handle.Exists())
							{
								CreateVehicle(vehicleDataV4);
							}
						}
					}
				}
				else if (Game.Player.Character.Model == PedHash.Michael)
				{
					if (SwitchedCars.ContainsKey(2))
					{
						string id = SwitchedCars[2];
						SwitchedCars.Remove(2);
						VehicleDataV1 vehicleDataV5 = VehicleDatabase.FirstOrDefault((VehicleDataV1 x) => x.Id == id);
						if (vehicleDataV5 != null)
						{
							if (vehicleDataV5.Handle == null)
							{
								CreateVehicle(vehicleDataV5);
							}
							else if (!vehicleDataV5.Handle.Exists())
							{
								CreateVehicle(vehicleDataV5);
							}
						}
					}
				}
				else if (Game.Player.Character.Model == PedHash.Trevor && SwitchedCars.ContainsKey(3))
				{
					string id = SwitchedCars[3];
					SwitchedCars.Remove(3);
					VehicleDataV1 vehicleDataV6 = VehicleDatabase.FirstOrDefault((VehicleDataV1 x) => x.Id == id);
					if (vehicleDataV6 != null)
					{
						if (vehicleDataV6.Handle == null)
						{
							CreateVehicle(vehicleDataV6);
						}
						else if (!vehicleDataV6.Handle.Exists())
						{
							CreateVehicle(vehicleDataV6);
						}
					}
				}
				foreach (VehicleDataV1 item8 in VehicleDatabase)
				{
					if (item8.Handle != null)
					{
						if (!item8.Handle.Exists() && !item8.WasUserDespawned)
						{
							CreateVehicle(item8);
							if (item8.SafeSpawnSet && item8.Handle != null)
							{
								item8.Position = item8.SafeSpawn;
								item8.Rotation = item8.SafeRotation;
							}
						}
					}
					else if (!item8.WasUserDespawned)
					{
						CreateVehicle(item8);
						if (item8.SafeSpawnSet && item8.Handle != null)
						{
							item8.Position = item8.SafeSpawn;
							item8.Rotation = item8.SafeRotation;
						}
					}
				}
			}
			switchDetect = true;
			waitForSwitch = false;
		}
		if (StoppedInitial && Game.GameTime - StartedTime > 4000)
		{
			StoppedInitial = false;
			if (!Function.Call<bool>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, 6969))
			{
				Function.Call(Hash.ADD_DOOR_TO_SYSTEM, 6969, Game.GenerateHash("prop_ch_025c_g_door_01"), 18.65038f, 546.3401f, 176.3448f, true, false, true);
			}
			Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_DISTANCE, 6969, 10f, false, true);
			Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_RATE, 6969, 1f, false, true);
			if (!Function.Call<bool>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, 696969))
			{
				Function.Call(Hash.ADD_DOOR_TO_SYSTEM, 696969, Game.GenerateHash("prop_ld_garaged_01"), -815.2816f, 185.975f, 72.99993f, true, false, true);
			}
			Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_DISTANCE, 696969, 10f, false, true);
			Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_RATE, 696969, 1f, false, true);
			if (!Function.Call<bool>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, 69696969))
			{
				Function.Call(Hash.ADD_DOOR_TO_SYSTEM, 69696969, Game.GenerateHash("prop_cs4_10_tr_gd_01"), 1972.787f, 3824.554f, 32.65174f, true, false, true);
			}
			Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_DISTANCE, 69696969, 10f, false, true);
			Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_RATE, 69696969, 1f, false, true);
			Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, 6969, 0, true, true);
			Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, 696969, 0, true, true);
			Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, 69696969, 0, true, true);
		}
		if (!SaveVehicle)
		{
			if (ModSettings.EnablePhoneTurnOnByController && IsControlJustReleased(2, GTA.Control.PhoneDown) && !flag && Function.Call<bool>(Hash.IS_CONTROL_ENABLED, 0, GTA.Control.PhoneDown) && Function.Call<bool>(Hash.IS_CONTROL_ENABLED, 1, GTA.Control.PhoneDown) && Function.Call<bool>(Hash.IS_CONTROL_ENABLED, 2, GTA.Control.PhoneDown) && Function.Call<bool>(Hash.IS_CONTROL_ENABLED, 0, GTA.Control.ScriptPadDown) && Function.Call<bool>(Hash.IS_CONTROL_ENABLED, 1, GTA.Control.ScriptPadDown) && Function.Call<bool>(Hash.IS_CONTROL_ENABLED, 2, GTA.Control.ScriptPadDown) && !GUI.Phone.IsOn && !GUI.Phone.TriggerLoaded)
			{
				OutputArgument outputArgument = new OutputArgument();
				Function.Call(Hash.GET_MOBILE_PHONE_POSITION, outputArgument);
				Vector3 result = outputArgument.GetResult<Vector3>();
				if (result.Y < -70f || result.Y > 0f)
				{
					GUI.Phone.TriggerLoaded = true;
				}
			}
			if (IsControlPressed(2, GTA.Control.ScriptLS) && IsControlPressed(2, GTA.Control.VehicleDuck) && !debouncer)
			{
				debouncer = true;
				if (!GUI.Phone.IsOn)
				{
					SaveVehicle = true;
				}
				else
				{
					GTA.UI.Screen.ShowSubtitle("Please close your phone first.", 3000);
				}
			}
			if (IsControlJustReleased(2, GTA.Control.ScriptLS))
			{
				debouncer = false;
			}
			if (IsControlJustReleased(2, GTA.Control.VehicleDuck))
			{
				debouncer = false;
			}
		}
		if (AttachedTasks.Count > 0)
		{
			foreach (KeyValuePair<Vehicle, PedTask> item9 in new Dictionary<Vehicle, PedTask>(AttachedTasks))
			{
				if (item9.Value.IsDoneSequence())
				{
					item9.Value.Clean();
					item9.Value.Ped.IsPersistent = false;
					item9.Value.Ped.IsInvincible = false;
					item9.Key.IsInvincible = false;
					item9.Key.CanBeVisiblyDamaged = true;
					AttachedTasks.Remove(item9.Key);
					if (ModSettings.PlaySpeechOnCarDelivery)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Game.Player.Character, "GENERIC_THANKS", "SPEECH_PARAMS_FORCE_SHOUTED", 0);
					}
					if (ModSettings.FocusOnCarDelivery)
					{
						Function.Call(Hash.SET_GAMEPLAY_VEHICLE_HINT, item9.Key.Handle, 0f, 0f, 0.5f, true, 3000, 1000, 1000);
					}
					GTA.UI.Screen.ShowSubtitle("Your vehicle has arrived!", 3000);
				}
			}
		}
		int num8 = Function.Call<int>(Hash.GET_CAM_ACTIVE_VIEW_MODE_CONTEXT);
		if (Function.Call<int>(Hash.GET_CAM_VIEW_MODE_FOR_CONTEXT, num8) == 4)
		{
			if (!LastFirstPerson)
			{
				LastFirstPerson = true;
				LastViewChanged = true;
			}
		}
		else if (LastFirstPerson)
		{
			LastFirstPerson = false;
			LastViewChanged = true;
		}
		if (LastViewChanged)
		{
			if (GUI.Phone.IsOn)
			{
				if (LastFirstPerson)
				{
					GUI.Phone.BringDown();
				}
				else
				{
					GUI.Phone.BringUp();
				}
			}
			LastViewChanged = false;
		}
		if (ModSettings.RemovePersonalVehicles && !Game.IsMissionActive)
		{
			Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "vehicle_gen_controller");
		}
		if (GUI.Phone.TriggerLoaded)
		{
			if (GUI.Phone.Interface == 0)
			{
				GUI.Phone.PhoneScaleform = new Scaleform("cellphone_ifruit");
			}
			else if (GUI.Phone.Interface == 1)
			{
				GUI.Phone.PhoneScaleform = new Scaleform("cellphone_facade");
			}
			else
			{
				GUI.Phone.PhoneScaleform = new Scaleform("cellphone_badger");
			}
			int gameTime = Game.GameTime;
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, GUI.Phone.PhoneScaleform.Handle))
			{
				Script.Yield();
				if (Game.GameTime - gameTime > 1000)
				{
					GTA.UI.Screen.ShowSubtitle("Failed to load phone. Try again.");
					return;
				}
			}
			GUI.Phone.TurnOn();
			GUI.Phone.TriggerLoaded = false;
		}
		if (GUI.Phone.RawOn)
		{
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 27, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 85, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 74, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 80, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 24, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 25, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 73, true);
			if (!flag)
			{
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 21, true);
			}
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 51, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 19, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 106, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 99, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 100, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 68, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 69, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 70, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 114, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 331, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 122, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 235, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 288, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 289, true);
		}
		if (GUI.Phone.IsOn)
		{
			if (Game.IsControlJustPressed(GTA.Control.PhoneRight) && GUI.Phone.IsOnHomeScreen())
			{
				GUI.Phone.SetInputEvent(GUI.Phone.Direction.Right);
			}
			if (Game.IsControlJustPressed(GTA.Control.PhoneLeft) && GUI.Phone.IsOnHomeScreen())
			{
				GUI.Phone.SetInputEvent(GUI.Phone.Direction.Left);
			}
			if (GUI.Phone.CurrentApp != null)
			{
				if (GUI.Phone.CurrentApp.Name == "Changelog")
				{
					if (Game.IsControlPressed(GTA.Control.PhoneDown))
					{
						GUI.Phone.SetInputEvent(GUI.Phone.Direction.Down);
					}
					if (Game.IsControlPressed(GTA.Control.PhoneUp))
					{
						GUI.Phone.SetInputEvent(GUI.Phone.Direction.Up);
					}
				}
				else
				{
					if (Game.IsControlJustPressed(GTA.Control.PhoneDown))
					{
						if (GUI.Phone.IsOnHomeScreen())
						{
							GUI.Phone.SetInputEvent(GUI.Phone.Direction.Down);
						}
						else
						{
							GUI.Phone.SetInputEvent(GUI.Phone.Direction.Down);
						}
					}
					if (Game.IsControlJustPressed(GTA.Control.PhoneUp))
					{
						if (GUI.Phone.IsOnHomeScreen())
						{
							GUI.Phone.SetInputEvent(GUI.Phone.Direction.Up);
						}
						else
						{
							GUI.Phone.SetInputEvent(GUI.Phone.Direction.Up);
						}
					}
				}
			}
			else
			{
				if (Game.IsControlJustPressed(GTA.Control.PhoneDown))
				{
					if (GUI.Phone.IsOnHomeScreen())
					{
						GUI.Phone.SetInputEvent(GUI.Phone.Direction.Down);
					}
					else
					{
						GUI.Phone.SetInputEvent(GUI.Phone.Direction.Down);
					}
				}
				if (Game.IsControlJustPressed(GTA.Control.PhoneUp))
				{
					if (GUI.Phone.IsOnHomeScreen())
					{
						GUI.Phone.SetInputEvent(GUI.Phone.Direction.Up);
					}
					else
					{
						GUI.Phone.SetInputEvent(GUI.Phone.Direction.Up);
					}
				}
			}
			if (Game.IsControlJustPressed(GTA.Control.CursorScrollDown))
			{
				if (GUI.Phone.IsOnHomeScreen())
				{
					GUI.Phone.SetInputEvent(GUI.Phone.Direction.Right);
				}
				else
				{
					GUI.Phone.SetInputEvent(GUI.Phone.Direction.Down);
				}
			}
			if (Game.IsControlJustPressed(GTA.Control.CursorScrollUp))
			{
				if (GUI.Phone.IsOnHomeScreen())
				{
					GUI.Phone.SetInputEvent(GUI.Phone.Direction.Left);
				}
				else
				{
					GUI.Phone.SetInputEvent(GUI.Phone.Direction.Up);
				}
			}
			if (Game.IsControlJustPressed(GTA.Control.PhoneSelect) && GUI.Phone.IsOn)
			{
				GUI.Phone.Click();
			}
			if (Game.IsControlJustPressed(GTA.Control.PhoneCancel) && GUI.Phone.IsOn)
			{
				GUI.Phone.Back();
			}
		}
		GUI.Phone.Draw();
		if (DeletePersonalCars)
		{
			Vehicle[] nearbyVehicles = World.GetAllVehicles();
			foreach (Vehicle vehicle4 in nearbyVehicles)
			{
				if (vehicle4 != null && vehicle4.Exists() && (vehicle4.Model == VehicleHash.Buffalo2 || vehicle4.Model == VehicleHash.Bagger || vehicle4.Model == VehicleHash.Tailgater || vehicle4.Model == VehicleHash.Bodhi2 || vehicle4.Model == VehicleHash.Issi2 || vehicle4.Model == VehicleHash.Sentinel2 || vehicle4.Model == VehicleHash.Blazer3))
				{
					while (vehicle4.Exists())
					{
						vehicle4.Delete();
					}
				}
			}
			DeletePersonalCars = false;
			return;
		}
		if (SaveVehicle)
		{
			if (Game.Player.Character.CurrentVehicle != null)
			{
				if (AttachedVehicles.ContainsKey(Game.Player.Character.CurrentVehicle))
				{
					Vehicle currentVehicle3 = Game.Player.Character.CurrentVehicle;
					VehicleDataV1 vehicleDataV7 = AttachedVehicles[currentVehicle3];
					AttachedVehicles.Remove(currentVehicle3);
					VehicleDatabase.Remove(vehicleDataV7);
					VehicleMetabase.Remove(vehicleDataV7.Meta);
					if (currentVehicle3.AttachedBlip != null)
					{
						currentVehicle3.AttachedBlip.Delete();
					}
					Blip[] attachedBlips = currentVehicle3.AttachedBlips;
					for (int i = 0; i < attachedBlips.Length; i++)
					{
						attachedBlips[i].Delete();
					}
					currentVehicle3.IsPersistent = false;
					Notification.Show($"Vehicle Removed [{VehicleDatabase.Count}]");
					SaveVehicle = false;
					return;
				}
				if (VehicleDatabase.Count >= ModSettings.MaxNumberOfCars)
				{
					Notification.Show($"ERROR: Max Vehicles Reached [{ModSettings.MaxNumberOfCars}]");
				}
				else
				{
					Vehicle currentVehicle4 = Game.Player.Character.CurrentVehicle;
					VehicleDataV1 vehicleDataV8 = new VehicleDataV1();
					SaveVehicleData(currentVehicle4, vehicleDataV8);
					vehicleDataV8.SafeSpawn = currentVehicle4.Position;
					vehicleDataV8.SafeSpawnSet = true;
					vehicleDataV8.SafeRotation = currentVehicle4.Rotation;
					if (ModSettings.EnableBlips)
					{
						if (currentVehicle4.AttachedBlip == null)
						{
							currentVehicle4.AddBlip();
						}
						if (currentVehicle4.Model.IsHelicopter)
						{
							currentVehicle4.AttachedBlip.Sprite = BlipSprite.PoliceHelicopter;
						}
						else if (currentVehicle4.Model.IsAmphibiousQuadBike || currentVehicle4.Model.IsBicycle || currentVehicle4.Model.IsBike || currentVehicle4.Model.IsQuadBike)
						{
							currentVehicle4.AttachedBlip.Sprite = BlipSprite.PersonalVehicleBike;
						}
						else if (currentVehicle4.Model.IsJetSki)
						{
							currentVehicle4.AttachedBlip.Sprite = BlipSprite.Seashark;
						}
						else if (currentVehicle4.Model.IsBoat)
						{
							currentVehicle4.AttachedBlip.Sprite = BlipSprite.Boat;
						}
						else if (currentVehicle4.Model.IsPlane)
						{
							currentVehicle4.AttachedBlip.Sprite = BlipSprite.Plane;
						}
						else
						{
							currentVehicle4.AttachedBlip.Sprite = BlipSprite.PersonalVehicleCar;
						}
						currentVehicle4.AttachedBlip.IsShortRange = true;
						currentVehicle4.AttachedBlip.Scale = 0.75f;
						currentVehicle4.AttachedBlip.Name = "Saved Vehicle";
						currentVehicle4.AttachedBlip.Alpha = 255;
						Function.Call(Hash.SHOW_TICK_ON_BLIP, currentVehicle4.AttachedBlip.Handle, false);
						currentVehicle4.AttachedBlip.Priority = 0;
						currentVehicle4.AttachedBlip.Color = (BlipColor)vehicleDataV8.BlipColor;
						Function.Call(Hash.SHOW_HEADING_INDICATOR_ON_BLIP, currentVehicle4.AttachedBlip.Handle, false);
					}
					MainCharacter.CarAttach = vehicleDataV8.Id;
					AttachedVehicles[currentVehicle4] = vehicleDataV8;
					VehicleDatabase.Add(vehicleDataV8);
					VehicleMetabase.Add(vehicleDataV8.Meta);
					Notification.Show($"Vehicle Added [{VehicleDatabase.Count}]");
				}
				SaveVehicle = false;
			}
			else
			{
				Notification.Show("ERROR: Not in vehicle");
				SaveVehicle = false;
			}
			return;
		}
		if (Game.GameTime >= LoadTick && LoadCharacter && !Game.IsMissionActive && !Game.IsCutsceneActive)
		{
			if (ModSettings.SaveCharacterSkin)
			{
				Model model4 = new Model(MainCharacter.PedSkin);
				if (model4.IsInCdImage)
				{
					model4.Request(10000);
					Game.Player.ChangeModel(model4);
					Game.Player.ChangeModel(model4);
					Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, Game.Player.Character.Handle);
				}
			}
			if (ModSettings.SaveGameTime)
			{
				World.CurrentTimeOfDay = MainCharacter.Time;
				World.CurrentDate = MainCharacter.Date;
			}
			if (ModSettings.SaveGameWeather)
			{
				World.Weather = MainCharacter.Weather;
				World.NextWeather = MainCharacter.WeatherNext;
			}
			if (ModSettings.SaveCharacterPosition)
			{
				Game.Player.Character.PositionNoOffset = MainCharacter.Position;
				Game.Player.Character.Heading = MainCharacter.Heading;
			}
			if (ModSettings.SaveCharacterHealthArmor)
			{
				Game.Player.Character.Health = MainCharacter.Health;
				Game.Player.Character.Armor = MainCharacter.Armor;
			}
			if (ModSettings.SaveCharacterClothes && ModSettings.SaveCharacterSkin)
			{
				for (int j = 0; j < 12; j++)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character.Handle, j, MainCharacter.ClothesVariant[j], MainCharacter.ClothesTexture[j], MainCharacter.ClothesPalette[j]);
				}
				for (int k = 0; k < 7; k++)
				{
					Function.Call(Hash.SET_PED_PROP_INDEX, Game.Player.Character.Handle, k, MainCharacter.PropsVariant[k], MainCharacter.PropsTexture[k], false);
				}
			}
			GUI.Phone.HomescreenImage = (GUI.Phone.BackgroundImage)MainCharacter.PhoneBackground;
			GUI.Phone.PhoneColor = MainCharacter.PhoneColor;
			GUI.Phone.PhoneBrightness = MainCharacter.PhoneBrightness;
			GUI.Phone.ActiveTheme = (GUI.Phone.Theme)MainCharacter.PhoneTheme;
			GUI.Phone.PhoneTone = MainCharacter.PhoneTone;
			GUI.Phone.PhoneModel = MainCharacter.PhoneBody;
			if (ModSettings.SaveCharacterWeapons)
			{
				Game.Player.Character.Weapons.RemoveAll();
				foreach (KeyValuePair<uint, List<uint>> weapon in MainCharacter.Weapons)
				{
					Function.Call(Hash.GIVE_WEAPON_TO_PED, Game.Player.Character.Handle, weapon.Key, MainCharacter.Ammo[weapon.Key], false, false);
				}
				foreach (KeyValuePair<uint, List<uint>> weapon2 in MainCharacter.Weapons)
				{
					if (!WeaponManager.HasWeapon(Game.Player.Character.Handle, (WeaponHash)weapon2.Key))
					{
						continue;
					}
					foreach (uint item10 in weapon2.Value)
					{
						if (WeaponManager.DoesWeaponTakeComponent((WeaponHash)weapon2.Key, (WeaponComponentHash)item10))
						{
							WeaponManager.GiveWeaponComponent(Game.Player.Character.Handle, weapon2.Key, item10);
						}
					}
					Function.Call(Hash.SET_AMMO_IN_CLIP, Game.Player.Character.Handle, weapon2.Key, MainCharacter.Ammo[weapon2.Key]);
					Function.Call(Hash.SET_PED_AMMO, Game.Player.Character.Handle, weapon2.Key, MainCharacter.Ammo[weapon2.Key], true);
					if (WeaponManager.GetWeaponTintCount((WeaponHash)weapon2.Key) >= MainCharacter.Tints[weapon2.Key])
					{
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character.Handle, weapon2.Key, MainCharacter.Tints[weapon2.Key]);
					}
				}
			}
			Script.Wait(500);
			if (ModSettings.SaveCharacterWeapons && MainCharacter.ActiveWeapon != 2725352035u)
			{
				Function.Call(Hash.SET_CURRENT_PED_WEAPON, Game.Player.Character.Handle, MainCharacter.ActiveWeapon, true);
			}
			if (LoadMax > 0)
			{
				if (ModSettings.SpawnAllVehiclesOnStartup && !ModSettings.EnableVehicleStreamer)
				{
					LoadVehicles = true;
				}
				else
				{
					if (!ModSettings.EnableVehicleStreamer)
					{
						foreach (VehicleDataV1 item11 in VehicleDatabase)
						{
							item11.WasUserDespawned = true;
						}
					}
					SaveOne = true;
					UpdateTick = Game.GameTime + 2000;
					IsModLoading = false;
					GTA.UI.Screen.FadeIn(2000);
					GTA.UI.Screen.ShowSubtitle("Advanced Persistence Loaded!", 3000);
					Logging.Log("Advanced Persistence Loaded! (" + (Game.GameTime - StartedTime) + "ms)");
				}
			}
			else
			{
				SaveOne = true;
				UpdateTick = Game.GameTime + 2000;
				IsModLoading = false;
				GTA.UI.Screen.FadeIn(2000);
				GTA.UI.Screen.ShowSubtitle("Advanced Persistence Loaded!", 3000);
				Logging.Log("Advanced Persistence Loaded! (" + (Game.GameTime - StartedTime) + "ms)");
			}
			LoadCharacter = false;
			return;
		}
		if (LoadVehicles)
		{
			CreateVehicle(VehicleDatabase[LoadCnt], null, byPassed: false, unfreeze: false, ModSettings.SpawnAllVehiclesAtSafeSpawn);
			if (LoadCnt == LoadMax - 1)
			{
				foreach (Vehicle key in AttachedVehicles.Keys)
				{
					if (key != null && key.Exists() && !key.Model.IsHelicopter)
					{
						key.IsPositionFrozen = false;
					}
				}
				IsModLoading = false;
				GTA.UI.Screen.FadeIn(2000);
				GTA.UI.Screen.ShowSubtitle("Advanced Persistence Loaded!", 3000);
				LoadVehicles = false;
				SaveOne = true;
				UpdateTick = Game.GameTime + 2000;
				Logging.Log("Advanced Persistence Loaded! (" + (Game.GameTime - StartedTime) + "ms)");
			}
			else
			{
				LoadCnt++;
			}
			return;
		}
		if (SaveTwo)
		{
			if (SaveThree && Game.GameTime >= SaveTick)
			{
				StreamTick = Game.GameTime;
				Stream1 = true;
				SaveThree = false;
			}
			if (Stream1 && Game.GameTime - StreamTick > 50)
			{
				using (Stream serializationStream = new FileStream(CharacterMetaname, FileMode.Create, FileAccess.Write, FileShare.None))
				{
					form.Serialize(serializationStream, MainCharacterMeta);
				}
				Stream1 = false;
				Stream2 = true;
				StreamTick = Game.GameTime;
			}
			else if (Stream2 && Game.GameTime - StreamTick > 50)
			{
				using (Stream serializationStream2 = new FileStream(CharacterFilename, FileMode.Create, FileAccess.Write, FileShare.None))
				{
					form.Serialize(serializationStream2, MainCharacter);
				}
				Stream2 = false;
				Stream3 = true;
				StreamTick = Game.GameTime;
			}
			else if (Stream3 && Game.GameTime - StreamTick > 50)
			{
				using (Stream serializationStream3 = new FileStream(VehicleMetaname, FileMode.Create, FileAccess.Write, FileShare.None))
				{
					form.Serialize(serializationStream3, VehicleMetabase);
				}
				Stream3 = false;
				Stream4 = true;
				StreamTick = Game.GameTime;
			}
			else if (Stream4 && Game.GameTime - StreamTick > 50)
			{
				using (Stream serializationStream4 = new FileStream(VehicleFilename, FileMode.Create, FileAccess.Write, FileShare.None))
				{
					form.Serialize(serializationStream4, VehicleDatabase);
				}
				Stream4 = false;
				SaveTwo = false;
				SaveOne = true;
				SaveThree = false;
				UpdateTick = Game.GameTime + ModSettings.DataSavingTime;
			}
		}
		bool flag2 = SaveOne;
		if (!ModSettings.SaveDuringMissions)
		{
			flag2 = flag2 && !Game.IsMissionActive;
		}
		if (flag2 && Game.GameTime >= UpdateTick)
		{
			SaveTimed = Game.GameTime;
			Ped character2 = Game.Player.Character;
			MainCharacter.Position = character2.Position;
			MainCharacter.Heading = character2.Heading;
			MainCharacter.PedSkin = character2.Model.Hash;
			MainCharacter.Health = character2.Health;
			MainCharacter.Armor = character2.Armor;
			MainCharacter.PhoneBackground = (int)GUI.Phone.HomescreenImage;
			MainCharacter.PhoneTheme = (int)GUI.Phone.ActiveTheme;
			MainCharacter.PhoneColor = GUI.Phone.PhoneColor;
			MainCharacter.PhoneBrightness = GUI.Phone.PhoneBrightness;
			MainCharacter.Time = World.CurrentTimeOfDay;
			MainCharacter.Date = World.CurrentDate;
			MainCharacter.Weather = World.Weather;
			MainCharacter.WeatherNext = World.NextWeather;
			MainCharacter.PhoneBody = GUI.Phone.PhoneModel;
			MainCharacter.PhoneTone = GUI.Phone.PhoneTone;
			if (ModSettings.SaveCharacterClothes)
			{
				for (int l = 0; l < 12; l++)
				{
					MainCharacter.ClothesVariant[l] = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character2.Handle, l);
					MainCharacter.ClothesTexture[l] = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character2.Handle, l);
					MainCharacter.ClothesPalette[l] = Function.Call<int>(Hash.GET_PED_PALETTE_VARIATION, character2.Handle, l);
				}
				for (int m = 0; m < 7; m++)
				{
					MainCharacter.PropsVariant[m] = Function.Call<int>(Hash.GET_PED_PROP_INDEX, character2.Handle, m);
					MainCharacter.PropsTexture[m] = Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, character2.Handle, m);
				}
			}
			MainCharacter.CarAttach = null;
			if (ModSettings.SaveCharacterPosition && character2.CurrentVehicle != null && AttachedVehicles.ContainsKey(character2.CurrentVehicle))
			{
				MainCharacter.CarAttach = AttachedVehicles[character2.CurrentVehicle].Id;
			}
			if (ModSettings.SaveCharacterWeapons)
			{
				CWeapons = new Dictionary<uint, List<uint>>();
				CTints = new Dictionary<uint, uint>();
				CAmmo = new Dictionary<uint, int>();
			}
			SaveOne = false;
			DoWeapons = true;
		}
		if (DoVehicles)
		{
			if (ModSettings.SaveCharacterWeapons)
			{
				MainCharacter.ActiveWeapon = WeaponManager.GetCurrentWeapon(Game.Player.Character.Handle);
			}
			if (CloneList != null && CloneList.Count > 0)
			{
				VehicleDataV1 vehicleDataV9 = CloneList[VehicleIndex];
				if (vehicleDataV9.Handle != null)
				{
					if (vehicleDataV9.Handle.Exists())
					{
						SaveVehicleData(vehicleDataV9.Handle, vehicleDataV9);
					}
					else if (!vehicleDataV9.WasUserDespawned && !ModSettings.EnableVehicleStreamer)
					{
						CreateVehicle(vehicleDataV9);
					}
					if (vehicleDataV9.Handle.Model.IsHelicopter)
					{
						vehicleDataV9.Handle.IsPositionFrozen = false;
					}
				}
				else
				{
					if (!vehicleDataV9.WasUserDespawned && !ModSettings.EnableVehicleStreamer)
					{
						CreateVehicle(vehicleDataV9);
					}
					if (vehicleDataV9.Handle != null && vehicleDataV9.Handle.Model.IsHelicopter)
					{
						vehicleDataV9.Handle.IsPositionFrozen = false;
					}
				}
			}
			VehicleIndex++;
			if (CloneList != null)
			{
				if (VehicleIndex >= CloneList.Count)
				{
					DoVehicles = false;
					SaveThree = true;
					SaveTwo = true;
					SaveTick = Game.GameTime + 500;
				}
			}
			else
			{
				DoVehicles = false;
				SaveThree = true;
				SaveTwo = true;
				SaveTick = Game.GameTime + 500;
			}
		}
		if (DoWeapons)
		{
			if (ModSettings.SaveCharacterWeapons)
			{
				int handle = Game.Player.Character.Handle;
				WeaponHash weaponHash = Weapons[WeaponIndex];
				if (weaponHash != WeaponHash.Unarmed && WeaponManager.HasWeapon(handle, weaponHash))
				{
					CAmmo[(uint)weaponHash] = WeaponManager.GetAmmoInWeapon(handle, weaponHash);
					CTints[(uint)weaponHash] = WeaponManager.GetWeaponTintIndex(handle, weaponHash);
					CWeapons[(uint)weaponHash] = new List<uint>();
					WeaponComponentHash[] weaponComponents = WeaponComponents;
					foreach (WeaponComponentHash weaponComponentHash in weaponComponents)
					{
						if (WeaponManager.DoesWeaponTakeComponent(weaponHash, weaponComponentHash) && WeaponManager.HasGotWeaponComponent(handle, weaponHash, weaponComponentHash))
						{
							CWeapons[(uint)weaponHash].Add((uint)weaponComponentHash);
						}
					}
				}
			}
			WeaponIndex++;
			if (WeaponIndex >= Weapons.Length)
			{
				if (ModSettings.SaveCharacterWeapons)
				{
					MainCharacter.Weapons = CWeapons;
					MainCharacter.Ammo = CAmmo;
					MainCharacter.Tints = CTints;
				}
				WeaponIndex = 0;
				DoWeapons = false;
				DoVehicles = true;
				VehicleIndex = 0;
				CloneList = VehicleDatabase.ToList();
			}
		}
		if (ModSettings.EnableVehicleStreamer)
		{
			Streamer.HandleNextVehicle();
		}
	}

	private void OnKeyDown(object sender, KeyEventArgs e)
	{
	}

	private void OnKeyUp(object sender, KeyEventArgs e)
	{
		_ = e.KeyCode;
		_ = 74;
		if (SaveVehicle)
		{
			return;
		}
		if (e.Modifiers == (Keys)Enum.Parse(typeof(Keys), ModSettings.SaveModifier, ignoreCase: true) && e.KeyCode == (Keys)Enum.Parse(typeof(Keys), ModSettings.SaveKey, ignoreCase: true))
		{
			if (!GUI.Phone.IsOn)
			{
				SaveVehicle = true;
			}
			else
			{
				GTA.UI.Screen.ShowSubtitle("Please close your phone first.", 3000);
			}
		}
		if (e.Modifiers == (Keys)Enum.Parse(typeof(Keys), ModSettings.PhoneModifier, ignoreCase: true) && e.KeyCode == (Keys)Enum.Parse(typeof(Keys), ModSettings.PhoneKey, ignoreCase: true) && !GUI.Phone.IsOn && !GUI.Phone.TriggerLoaded)
		{
			OutputArgument outputArgument = new OutputArgument();
			Function.Call(Hash.GET_MOBILE_PHONE_POSITION, outputArgument);
			Vector3 result = outputArgument.GetResult<Vector3>();
			if (result.Y < -70f || result.Y > 0f)
			{
				GUI.Phone.TriggerLoaded = true;
			}
		}
	}
}
