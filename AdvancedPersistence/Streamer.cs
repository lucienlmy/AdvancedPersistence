using System.Collections.Generic;
using GTA;
using GTA.Native;

namespace AdvancedPersistence;

public class Streamer
{
	public static int CurrVehIndex = 0;

	public static Dictionary<Vehicle, VehicleDataV1> StreamedVehicles = new Dictionary<Vehicle, VehicleDataV1>();

	public static void HandleNextVehicle()
	{
		if (AdvancedPersistence.VehicleDatabase.Count > 0)
		{
			if (CurrVehIndex >= AdvancedPersistence.VehicleDatabase.Count)
			{
				CurrVehIndex = 0;
			}
			VehicleDataV1 vehicleDataV = AdvancedPersistence.VehicleDatabase[CurrVehIndex];
			if (vehicleDataV != null)
			{
				if (vehicleDataV.Handle != null)
				{
					if (Game.Player.Character.Position.DistanceTo(vehicleDataV.Handle.Position) > ModSettings.StreamOutDistance)
					{
						AdvancedPersistence.SaveVehicleData(vehicleDataV.Handle, vehicleDataV);
						vehicleDataV.WasUserDespawned = false;
						AdvancedPersistence.DeleteBlipsOnCar(vehicleDataV.Handle);
						if (AdvancedPersistence.AttachedVehicles.ContainsKey(vehicleDataV.Handle))
						{
							AdvancedPersistence.AttachedVehicles.Remove(vehicleDataV.Handle);
						}
						StreamedVehicles.Remove(vehicleDataV.Handle);
						vehicleDataV.Handle.IsPersistent = true;
						OutputArgument outputArgument = new OutputArgument(vehicleDataV.Handle);
						Function.Call(Hash.DELETE_VEHICLE, outputArgument);
						if (AdvancedPersistence.AttachedTasks.ContainsKey(vehicleDataV.Handle))
						{
							AdvancedPersistence.AttachedTasks[vehicleDataV.Handle].Clean();
							if (AdvancedPersistence.AttachedTasks[vehicleDataV.Handle].Ped != null)
							{
								Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, AdvancedPersistence.AttachedTasks[vehicleDataV.Handle].Ped.Handle);
								Function.Call(Hash.TASK_LEAVE_VEHICLE, AdvancedPersistence.AttachedTasks[vehicleDataV.Handle].Ped.Handle, AdvancedPersistence.AttachedTasks[vehicleDataV.Handle].Handle, 16);
								if (AdvancedPersistence.AttachedTasks[vehicleDataV.Handle].Ped.Exists())
								{
									AdvancedPersistence.AttachedTasks[vehicleDataV.Handle].Ped.IsPersistent = true;
									AdvancedPersistence.AttachedTasks[vehicleDataV.Handle].Ped.Delete();
								}
							}
							AdvancedPersistence.AttachedTasks.Remove(vehicleDataV.Handle);
						}
						vehicleDataV.Handle = null;
					}
				}
				else if (!vehicleDataV.WasUserDespawned && Game.Player.Character.Position.DistanceTo(vehicleDataV.Position) < ModSettings.StreamInDistance)
				{
					if (StreamedVehicles.Count >= ModSettings.MaxNumberOfStreamedInCars)
					{
						foreach (Vehicle key in StreamedVehicles.Keys)
						{
							if (!(Game.Player.Character.Position.DistanceTo(vehicleDataV.Position) < Game.Player.Character.Position.DistanceTo(key.Position)))
							{
								continue;
							}
							VehicleDataV1 vehicleDataV2 = StreamedVehicles[key];
							if (vehicleDataV2.Handle != null)
							{
								AdvancedPersistence.SaveVehicleData(vehicleDataV2.Handle, vehicleDataV2);
								vehicleDataV2.WasUserDespawned = false;
								AdvancedPersistence.DeleteBlipsOnCar(vehicleDataV2.Handle);
								if (AdvancedPersistence.AttachedVehicles.ContainsKey(vehicleDataV2.Handle))
								{
									AdvancedPersistence.AttachedVehicles.Remove(vehicleDataV2.Handle);
								}
								StreamedVehicles.Remove(vehicleDataV2.Handle);
								vehicleDataV2.Handle.IsPersistent = true;
								OutputArgument outputArgument2 = new OutputArgument(vehicleDataV2.Handle);
								Function.Call(Hash.DELETE_VEHICLE, outputArgument2);
								if (AdvancedPersistence.AttachedTasks.ContainsKey(vehicleDataV2.Handle))
								{
									AdvancedPersistence.AttachedTasks[vehicleDataV2.Handle].Clean();
									if (AdvancedPersistence.AttachedTasks[vehicleDataV2.Handle].Ped != null)
									{
										Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, AdvancedPersistence.AttachedTasks[vehicleDataV2.Handle].Ped.Handle);
										Function.Call(Hash.TASK_LEAVE_VEHICLE, AdvancedPersistence.AttachedTasks[vehicleDataV2.Handle].Ped.Handle, AdvancedPersistence.AttachedTasks[vehicleDataV2.Handle].Handle, 16);
										if (AdvancedPersistence.AttachedTasks[vehicleDataV2.Handle].Ped.Exists())
										{
											AdvancedPersistence.AttachedTasks[vehicleDataV2.Handle].Ped.IsPersistent = true;
											AdvancedPersistence.AttachedTasks[vehicleDataV2.Handle].Ped.Delete();
										}
									}
									AdvancedPersistence.AttachedTasks.Remove(vehicleDataV2.Handle);
								}
								vehicleDataV2.Handle = null;
							}
							Function.Call(Hash.CLEAR_AREA, vehicleDataV.Position.X, vehicleDataV.Position.Y, vehicleDataV.Position.Z, 10f, false, false, false, false);
							Vehicle vehicle = AdvancedPersistence.CreateVehicle(vehicleDataV);
							if (vehicle != null)
							{
								StreamedVehicles.Add(vehicle, vehicleDataV);
							}
							break;
						}
					}
					else
					{
						Function.Call(Hash.CLEAR_AREA, vehicleDataV.Position.X, vehicleDataV.Position.Y, vehicleDataV.Position.Z, 10f, false, false, false, false);
						Vehicle vehicle2 = AdvancedPersistence.CreateVehicle(vehicleDataV);
						if (vehicle2 != null)
						{
							StreamedVehicles.Add(vehicle2, vehicleDataV);
						}
					}
				}
			}
			CurrVehIndex++;
			if (CurrVehIndex >= AdvancedPersistence.VehicleDatabase.Count)
			{
				CurrVehIndex = 0;
			}
		}
		else
		{
			CurrVehIndex = 0;
		}
	}
}
