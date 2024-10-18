using GTA;
using GTA.Math;
using GTA.Native;

namespace AdvancedPersistence;

public class BoneHelper
{
	public static int GetBoneIndex(Vehicle veh, string bone)
	{
		return Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, veh.Handle, bone);
	}

	public static Vector3 GetBonePositionWorld(Vehicle veh, int boneIndex)
	{
		return Function.Call<Vector3>(Hash.GET_WORLD_POSITION_OF_ENTITY_BONE, veh.Handle, boneIndex);
	}
}
