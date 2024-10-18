using System;
using System.Drawing;
using System.Runtime.Serialization;
using GTA;
using GTA.Math;

namespace AdvancedPersistence;

[Serializable]
public class VehicleDataV1
{
	[NonSerialized]
	public Vehicle Handle;

	[NonSerialized]
	public VehicleDataMeta Meta;

	[NonSerialized]
	public bool WasUserDespawned;

	[OptionalField(VersionAdded = 2)]
	public int BlipColor;

	[OptionalField(VersionAdded = 2)]
	public string Tag = "";

	[OptionalField(VersionAdded = 2)]
	public float FrontLeftDoorAngle;

	[OptionalField(VersionAdded = 2)]
	public float FrontRightDoorAngle;

	[OptionalField(VersionAdded = 2)]
	public float BackLeftDoorAngle;

	[OptionalField(VersionAdded = 2)]
	public float BackRightDoorAngle;

	[OptionalField(VersionAdded = 2)]
	public float HoodAngle;

	[OptionalField(VersionAdded = 2)]
	public float TrunkAngle;

	[OptionalField(VersionAdded = 2)]
	public int SirenState;

	[OptionalField(VersionAdded = 2)]
	public int LightState2;

	[OptionalField(VersionAdded = 2)]
	public bool BulletProofTires;

	[OptionalField(VersionAdded = 2)]
	public Vector3 SafeSpawn;

	[OptionalField(VersionAdded = 2)]
	public bool SafeSpawnSet;

	[OptionalField(VersionAdded = 2)]
	public Vector3 SafeRotation;

	[OptionalField(VersionAdded = 2)]
	public int IndicatorState;

	public string Id { get; set; }

	public string LicensePlate { get; set; } = "";


	public VehicleHash Hash { get; set; }

	public Vector3 Position { get; set; }

	public Vector3 Rotation { get; set; }

	public bool ConvertibleState { get; set; }

	public bool EngineState { get; set; }

	public bool LockState { get; set; }

	public bool LightState { get; set; }

	public bool AlarmState { get; set; }

	public int Boost { get; set; } = -1;


	public bool Turbo { get; set; }

	public bool TireSmoke { get; set; }

	public bool XenonHeadlights { get; set; }

	public int XenonHeadlightsColor { get; set; } = -1;


	public int Spoiler { get; set; } = -1;


	public bool SpoilerVar { get; set; }

	public int FrontBumper { get; set; } = -1;


	public bool FrontBumperVar { get; set; }

	public int RearBumper { get; set; } = -1;


	public bool RearBumperVar { get; set; }

	public int SideSkirt { get; set; } = -1;


	public bool SideSkirtVar { get; set; }

	public int Exhaust { get; set; } = -1;


	public bool ExhaustVar { get; set; }

	public int Frame { get; set; } = -1;


	public bool FrameVar { get; set; }

	public int Grille { get; set; } = -1;


	public bool GrilleVar { get; set; }

	public int Hood { get; set; } = -1;


	public bool HoodVar { get; set; }

	public int Fender { get; set; } = -1;


	public bool FenderVar { get; set; }

	public int RightFender { get; set; } = -1;


	public bool RightFenderVar { get; set; }

	public int Roof { get; set; } = -1;


	public bool RoofVar { get; set; }

	public int Engine { get; set; } = -1;


	public bool EngineVar { get; set; }

	public int Brakes { get; set; } = -1;


	public bool BrakesVar { get; set; }

	public int Transmission { get; set; } = -1;


	public bool TransmissionVar { get; set; }

	public int Horns { get; set; } = -1;


	public bool HornsVar { get; set; }

	public int Suspension { get; set; } = -1;


	public bool SuspensionVar { get; set; }

	public int Armor { get; set; } = -1;


	public bool ArmorVar { get; set; }

	public int FrontWheel { get; set; } = -1;


	public bool FrontWheelVar { get; set; }

	public int RearWheel { get; set; } = -1;


	public bool RearWheelVar { get; set; }

	public int PlateHolder { get; set; } = -1;


	public bool PlateHolderVar { get; set; }

	public int VanityPlates { get; set; } = -1;


	public bool VanityPlatesVar { get; set; }

	public int TrimDesign { get; set; } = -1;


	public bool TrimDesignVar { get; set; }

	public int Ornaments { get; set; } = -1;


	public bool OrnamentsVar { get; set; }

	public int Dashboard { get; set; } = -1;


	public bool DashboardVar { get; set; }

	public int DialDesign { get; set; } = -1;


	public bool DialDesignVar { get; set; }

	public int DoorSpeakers { get; set; } = -1;


	public bool DoorSpeakersVar { get; set; }

	public int Seats { get; set; } = -1;


	public bool SeatsVar { get; set; }

	public int SteeringWheels { get; set; } = -1;


	public bool SteeringWheelsVar { get; set; }

	public int ColumnShifterLevers { get; set; } = -1;


	public bool ColumnShifterLeversVar { get; set; }

	public int Plaques { get; set; } = -1;


	public bool PlaquesVar { get; set; }

	public int Speakers { get; set; } = -1;


	public bool SpeakersVar { get; set; }

	public int Trunk { get; set; } = -1;


	public bool TrunkVar { get; set; }

	public int Hydraulics { get; set; } = -1;


	public bool HydraulicsVar { get; set; }

	public int EngineBlock { get; set; } = -1;


	public bool EngineBlockVar { get; set; }

	public int AirFilter { get; set; } = -1;


	public bool AirFilterVar { get; set; }

	public int Struts { get; set; } = -1;


	public bool StrutsVar { get; set; }

	public int ArchCover { get; set; } = -1;


	public bool ArchCoverVar { get; set; }

	public int Aerials { get; set; } = -1;


	public bool AerialsVar { get; set; }

	public int Trim { get; set; } = -1;


	public bool TrimVar { get; set; }

	public int Tank { get; set; } = -1;


	public bool TankVar { get; set; }

	public int Windows { get; set; } = -1;


	public bool WindowsVar { get; set; }

	public int Livery { get; set; } = -1;


	public bool LiveryVar { get; set; }

	public float SteeringAngle { get; set; }

	public float DirtLevel { get; set; }

	public int FrontLeftDoorState { get; set; }

	public int FrontRightDoorState { get; set; }

	public int BackLeftDoorState { get; set; }

	public int BackRightDoorState { get; set; }

	public int HoodState { get; set; }

	public int TrunkState { get; set; }

	public VehicleColor PrimaryColor { get; set; }

	public VehicleColor SecondaryColor { get; set; }

	public VehicleColor RimColor { get; set; }

	public VehicleColor PearlescentColor { get; set; }

	public VehicleColor TrimColor { get; set; }

	public VehicleColor DashboardColor { get; set; }

	public bool IsPrimaryCustom { get; set; }

	public bool IsSecondaryCustom { get; set; }

	public Color CustomPrimaryColor { get; set; }

	public Color CustomSecondaryColor { get; set; }

	public Color NeonLightColor { get; set; }

	public Color TireSmokeColor { get; set; }

	public VehicleWindowTint WindowTint { get; set; }

	public LicensePlateStyle LicensePlateStyle { get; set; }

	public VehicleWheelType WheelType { get; set; }

	public bool NeonLightLeft { get; set; }

	public bool NeonLightRight { get; set; }

	public bool NeonLightFront { get; set; }

	public bool NeonLightBack { get; set; }

	public int[] WindowStates { get; set; } = new int[8];


	public int[] WheelStates { get; set; } = new int[10];


	public bool[] Extras { get; set; } = new bool[15];


	public VehicleDataV1()
	{
		Id = Guid.NewGuid().ToString();
		Meta = new VehicleDataMeta();
		Meta.Id = Id;
	}
}
