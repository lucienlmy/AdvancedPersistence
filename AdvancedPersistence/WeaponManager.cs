using GTA;
using GTA.Native;

namespace AdvancedPersistence;

public class WeaponManager
{
	public static uint GetCurrentWeapon(int ped)
	{
		OutputArgument outputArgument = new OutputArgument();
		Function.Call<bool>(Hash.GET_CURRENT_PED_WEAPON, ped, outputArgument, true);
		return outputArgument.GetResult<uint>();
	}

	public static uint GetWeaponTintIndex(int ped, WeaponHash wep)
	{
		return Function.Call<uint>(Hash.GET_PED_WEAPON_TINT_INDEX, ped, wep);
	}

	public static uint GetWeaponTintCount(WeaponHash wep)
	{
		return Function.Call<uint>(Hash.GET_WEAPON_TINT_COUNT, wep);
	}

	public static int GetAmmoInWeapon(int ped, WeaponHash wep)
	{
		return Function.Call<int>(Hash.GET_AMMO_IN_PED_WEAPON, ped, wep);
	}

	public static bool DoesWeaponTakeComponent(WeaponHash wep, WeaponComponentHash comp)
	{
		return Function.Call<bool>(Hash.DOES_WEAPON_TAKE_WEAPON_COMPONENT, wep, comp);
	}

	public static bool HasGotWeaponComponent(int ped, WeaponHash wep, WeaponComponentHash comp)
	{
		return Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON_COMPONENT, ped, wep, comp);
	}

	public static void GiveWeaponComponent(int ped, uint wep, uint comp)
	{
		Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_PED, ped, wep, comp);
	}

	public static bool HasWeapon(int ped, WeaponHash wep)
	{
		return Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, ped, wep);
	}
}
