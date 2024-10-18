using System;
using GTA.Math;
using GTA.Native;

namespace AdvancedPersistence;

public class MathHelper
{
	public static Vector3 RotateX(Vector3 point, float angle)
	{
		Vector3 vector = new Vector3(1f, 0f, 0f);
		Vector3 vector2 = new Vector3(0f, (float)Math.Cos(DegreesToRad(angle)), (float)(0.0 - Math.Sin(DegreesToRad(angle))));
		Vector3 vector3 = new Vector3(0f, (float)Math.Sin(DegreesToRad(angle)), (float)Math.Cos(DegreesToRad(angle)));
		Vector3 result = default(Vector3);
		result.X = vector.X * point.X + vector.Y * point.Y + vector.Z * point.Z;
		result.Y = vector2.X * point.X + vector2.Y * point.Y + vector2.Z * point.Z;
		result.Z = vector3.X * point.X + vector3.Y * point.Y + vector3.Z * point.Z;
		return result;
	}

	public static Vector3 RotateZ(Vector3 point, float angle)
	{
		Vector3 vector = new Vector3((float)Math.Cos(DegreesToRad(angle)), (float)(0.0 - Math.Sin(DegreesToRad(angle))), 0f);
		Vector3 vector2 = new Vector3((float)Math.Sin(DegreesToRad(angle)), (float)Math.Cos(DegreesToRad(angle)), 0f);
		Vector3 vector3 = new Vector3(0f, 0f, 1f);
		Vector3 result = default(Vector3);
		result.X = vector.X * point.X + vector.Y * point.Y + vector.Z * point.Z;
		result.Y = vector2.X * point.X + vector2.Y * point.Y + vector2.Z * point.Z;
		result.Z = vector3.X * point.X + vector3.Y * point.Y + vector3.Z * point.Z;
		return result;
	}

	public static Vector3 RotateY(Vector3 point, float angle)
	{
		Vector3 vector = new Vector3((float)Math.Cos(DegreesToRad(angle)), 0f, (float)Math.Sin(DegreesToRad(angle)));
		Vector3 vector2 = new Vector3(0f, 1f, 0f);
		Vector3 vector3 = new Vector3((float)(0.0 - Math.Sin(DegreesToRad(angle))), 0f, (float)Math.Cos(DegreesToRad(angle)));
		Vector3 result = default(Vector3);
		result.X = vector.X * point.X + vector.Y * point.Y + vector.Z * point.Z;
		result.Y = vector2.X * point.X + vector2.Y * point.Y + vector2.Z * point.Z;
		result.Z = vector3.X * point.X + vector3.Y * point.Y + vector3.Z * point.Z;
		return result;
	}

	public static float DegreesToRad(float deg)
	{
		return (float)Math.PI * deg / 180f;
	}

	public static float RadToDegrees(float rad)
	{
		return rad * (180f / (float)Math.PI);
	}

	public static Vector3 ScreenRelToWorld(Vector3 camPos, Vector3 camRot, Vector2 coord)
	{
		Vector3 vector = RotationToDirection(camRot) * 10f;
		Vector3 rotation = camRot;
		rotation.X += 10f;
		Vector3 rotation2 = camRot;
		rotation2.X -= 10f;
		Vector3 rotation3 = camRot;
		rotation3.Z -= 10f;
		Vector3 rotation4 = camRot;
		rotation4.Z += 10f;
		Vector3 vector2 = RotationToDirection(rotation4) - RotationToDirection(rotation3);
		Vector3 vector3 = RotationToDirection(rotation) - RotationToDirection(rotation2);
		float num = 0f - DegreesToRad(camRot.Y);
		Vector3 vector4 = vector2 * (float)Math.Cos(num) - vector3 * (float)Math.Sin(num);
		Vector3 vector5 = vector2 * (float)Math.Sin(num) + vector3 * (float)Math.Cos(num);
		if (!WorldToScreenRel(camPos + vector + vector4 + vector5, out var vec))
		{
			return camPos + vector;
		}
		if (!WorldToScreenRel(camPos + vector, out var vec2))
		{
			return camPos + vector;
		}
		if ((double)Math.Abs(vec.X - vec2.X) < 0.001 || (double)Math.Abs(vec.Y - vec2.Y) < 0.001)
		{
			return camPos + vector;
		}
		float num2 = (coord.X - vec2.X) / (vec.X - vec2.X);
		float num3 = (coord.Y - vec2.Y) / (vec.Y - vec2.Y);
		return camPos + vector + vector4 * num2 + vector5 * num3;
	}

	public unsafe static bool WorldToScreenRel(Vector3 vec, out Vector2 vec2)
	{
		float num = 0f;
		float num2 = 0f;
		bool num3 = Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, vec.X, vec.Y, vec.Z, &num2, &num);
		float x = num2;
		float y = num;
		if (num3)
		{
			vec2 = new Vector2(x, y);
			return num3;
		}
		vec2 = Vector2.Zero;
		return num3;
	}

	public static Vector3 RotationToDirection(Vector3 rotation)
	{
		float num = DegreesToRad(rotation.Z);
		float num2 = DegreesToRad(rotation.X);
		double num3 = Math.Abs(Math.Cos(num2));
		Vector3 result = default(Vector3);
		result.X = (float)((0.0 - Math.Sin(num)) * num3);
		result.Y = (float)(Math.Cos(num) * num3);
		result.Z = (float)Math.Sin(num2);
		return result;
	}
}
