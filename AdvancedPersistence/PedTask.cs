using GTA;
using GTA.Math;
using GTA.Native;

namespace AdvancedPersistence;

public class PedTask
{
	public int Handle;

	public Ped Ped;

	public int SequenceCount = -1;

	public PedTask(Ped ped)
	{
		Ped = ped;
	}

	public void Open()
	{
		OutputArgument outputArgument = new OutputArgument();
		Function.Call(Hash.OPEN_SEQUENCE_TASK, outputArgument);
		Handle = outputArgument.GetResult<int>();
	}

	public void Close()
	{
		Function.Call(Hash.SET_SEQUENCE_TO_REPEAT, Handle, false);
		Function.Call(Hash.CLOSE_SEQUENCE_TASK, Handle);
	}

	public void DriveTo(Vehicle veh, Vector3 target, float radius, float speed, DrivingStyle style)
	{
		SequenceCount++;
		Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, 0, veh.Handle, target.X, target.Y, target.Z, speed, style, radius);
	}

	public void WalkTo(Vector3 pos)
	{
		SequenceCount++;
		Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, 0, pos.X, pos.Y, pos.Z, 1f, 3000, 0f, 0f);
	}

	public void FaceCoord(Vector3 pos)
	{
		SequenceCount++;
		Function.Call(Hash.TASK_TURN_PED_TO_FACE_COORD, 0, pos.X, pos.Y, pos.Z, 2000);
	}

	public void PlayAnimAdv(Vector3 pos, string animDict, string animName)
	{
		SequenceCount++;
		if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, animDict))
		{
			Function.Call(Hash.REQUEST_ANIM_DICT, animDict);
		}
		Function.Call(Hash.TASK_PLAY_ANIM_ADVANCED, 0, animDict, animName, pos.X, pos.Y, pos.Z, 0f, 0f, -180f, 8f, 1f, 5000, 0, 0f, 0, 0);
	}

	public void Brake(Vehicle veh)
	{
		SequenceCount++;
		Function.Call(Hash.TASK_VEHICLE_TEMP_ACTION, 0, veh.Handle, 6, 5000);
	}

	public void ParkAt(Vehicle veh, Vector3 target, float radius)
	{
		SequenceCount++;
		Function.Call(Hash.TASK_VEHICLE_PARK, 0, veh.Handle, target.X, target.Y, target.Z, 0f, 0, radius, false);
	}

	public void ExitVehicle(Vehicle veh, bool normal = false)
	{
		SequenceCount++;
		if (normal)
		{
			Function.Call(Hash.TASK_LEAVE_VEHICLE, 0, veh.Handle, 0);
		}
		else
		{
			Function.Call(Hash.TASK_LEAVE_VEHICLE, 0, veh.Handle, 16);
		}
	}

	public void FleeCoords(Vector3 pos)
	{
		SequenceCount++;
		Function.Call(Hash.TASK_SMART_FLEE_COORD, 0, pos.X, pos.Y, pos.Z, 500f, 20000, false, false);
	}

	public void Wander()
	{
		Function.Call(Hash.TASK_WANDER_STANDARD, 0, 10f, 10);
	}

	public void Run()
	{
		Function.Call(Hash.TASK_PERFORM_SEQUENCE, Ped.Handle, Handle);
	}

	public unsafe void Clean()
	{
		int handle = Handle;
		Function.Call(Hash.CLEAR_SEQUENCE_TASK, &handle);
		Handle = 0;
	}

	public bool IsDoneSequence()
	{
		int num = Function.Call<int>(Hash.GET_SEQUENCE_PROGRESS, Ped.Handle);
		if (num != -1)
		{
			return num >= SequenceCount - 1;
		}
		return true;
	}

	public int GetSequence()
	{
		return Function.Call<int>(Hash.GET_SEQUENCE_PROGRESS, Ped.Handle);
	}
}
