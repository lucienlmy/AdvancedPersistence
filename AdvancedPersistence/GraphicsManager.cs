using GTA.Math;
using GTA.Native;

namespace AdvancedPersistence;

public class GraphicsManager
{
	public static GUI.RGBA HighlightColor_Edge = new GUI.RGBA(255, 255, 255);

	public static GUI.RGBA HighlightColor_Full = new GUI.RGBA(255, 255, 255, 25);

	public static void DrawSkeleton(Vector3 pos, Vector3 size, Vector3 rot)
	{
		Vector3 point = pos + new Vector3(size.X / 2f, size.Y / 2f, size.Z / 2f);
		Vector3 point2 = pos + new Vector3(size.X / 2f, (0f - size.Y) / 2f, size.Z / 2f);
		Vector3 point3 = pos + new Vector3(size.X / 2f, (0f - size.Y) / 2f, (0f - size.Z) / 2f);
		Vector3 point4 = pos + new Vector3(size.X / 2f, size.Y / 2f, (0f - size.Z) / 2f);
		Vector3 point5 = pos + new Vector3((0f - size.X) / 2f, size.Y / 2f, size.Z / 2f);
		Vector3 point6 = pos + new Vector3((0f - size.X) / 2f, (0f - size.Y) / 2f, size.Z / 2f);
		Vector3 point7 = pos + new Vector3((0f - size.X) / 2f, (0f - size.Y) / 2f, (0f - size.Z) / 2f);
		Vector3 point8 = pos + new Vector3((0f - size.X) / 2f, size.Y / 2f, (0f - size.Z) / 2f);
		point -= pos;
		point = MathHelper.RotateY(point, rot.Y);
		point = MathHelper.RotateX(point, rot.X);
		point = MathHelper.RotateZ(point, rot.Z);
		point += pos;
		point2 -= pos;
		point2 = MathHelper.RotateY(point2, rot.Y);
		point2 = MathHelper.RotateX(point2, rot.X);
		point2 = MathHelper.RotateZ(point2, rot.Z);
		point2 += pos;
		point3 -= pos;
		point3 = MathHelper.RotateY(point3, rot.Y);
		point3 = MathHelper.RotateX(point3, rot.X);
		point3 = MathHelper.RotateZ(point3, rot.Z);
		point3 += pos;
		point4 -= pos;
		point4 = MathHelper.RotateY(point4, rot.Y);
		point4 = MathHelper.RotateX(point4, rot.X);
		point4 = MathHelper.RotateZ(point4, rot.Z);
		point4 += pos;
		point5 -= pos;
		point5 = MathHelper.RotateY(point5, rot.Y);
		point5 = MathHelper.RotateX(point5, rot.X);
		point5 = MathHelper.RotateZ(point5, rot.Z);
		point5 += pos;
		point6 -= pos;
		point6 = MathHelper.RotateY(point6, rot.Y);
		point6 = MathHelper.RotateX(point6, rot.X);
		point6 = MathHelper.RotateZ(point6, rot.Z);
		point6 += pos;
		point7 -= pos;
		point7 = MathHelper.RotateY(point7, rot.Y);
		point7 = MathHelper.RotateX(point7, rot.X);
		point7 = MathHelper.RotateZ(point7, rot.Z);
		point7 += pos;
		point8 -= pos;
		point8 = MathHelper.RotateY(point8, rot.Y);
		point8 = MathHelper.RotateX(point8, rot.X);
		point8 = MathHelper.RotateZ(point8, rot.Z);
		point8 += pos;
		Function.Call(Hash.DRAW_LINE, point.X, point.Y, point.Z, point2.X, point2.Y, point2.Z, HighlightColor_Edge.Red, HighlightColor_Edge.Green, HighlightColor_Edge.Blue, 255);
		Function.Call(Hash.DRAW_LINE, point2.X, point2.Y, point2.Z, point3.X, point3.Y, point3.Z, HighlightColor_Edge.Red, HighlightColor_Edge.Green, HighlightColor_Edge.Blue, 255);
		Function.Call(Hash.DRAW_LINE, point3.X, point3.Y, point3.Z, point4.X, point4.Y, point4.Z, HighlightColor_Edge.Red, HighlightColor_Edge.Green, HighlightColor_Edge.Blue, 255);
		Function.Call(Hash.DRAW_LINE, point4.X, point4.Y, point4.Z, point.X, point.Y, point.Z, HighlightColor_Edge.Red, HighlightColor_Edge.Green, HighlightColor_Edge.Blue, 255);
		Function.Call(Hash.DRAW_POLY, point3.X, point3.Y, point3.Z, point4.X, point4.Y, point4.Z, point.X, point.Y, point.Z, HighlightColor_Full.Red, HighlightColor_Full.Green, HighlightColor_Full.Blue, HighlightColor_Full.Alpha);
		Function.Call(Hash.DRAW_POLY, point2.X, point2.Y, point2.Z, point3.X, point3.Y, point3.Z, point.X, point.Y, point.Z, HighlightColor_Full.Red, HighlightColor_Full.Green, HighlightColor_Full.Blue, HighlightColor_Full.Alpha);
		Function.Call(Hash.DRAW_LINE, point5.X, point5.Y, point5.Z, point6.X, point6.Y, point6.Z, HighlightColor_Edge.Red, HighlightColor_Edge.Green, HighlightColor_Edge.Blue, 255);
		Function.Call(Hash.DRAW_LINE, point6.X, point6.Y, point6.Z, point7.X, point7.Y, point7.Z, HighlightColor_Edge.Red, HighlightColor_Edge.Green, HighlightColor_Edge.Blue, 255);
		Function.Call(Hash.DRAW_LINE, point7.X, point7.Y, point7.Z, point8.X, point8.Y, point8.Z, HighlightColor_Edge.Red, HighlightColor_Edge.Green, HighlightColor_Edge.Blue, 255);
		Function.Call(Hash.DRAW_LINE, point8.X, point8.Y, point8.Z, point5.X, point5.Y, point5.Z, HighlightColor_Edge.Red, HighlightColor_Edge.Green, HighlightColor_Edge.Blue, 255);
		Function.Call(Hash.DRAW_POLY, point8.X, point8.Y, point8.Z, point7.X, point7.Y, point7.Z, point5.X, point5.Y, point5.Z, HighlightColor_Full.Red, HighlightColor_Full.Green, HighlightColor_Full.Blue, HighlightColor_Full.Alpha);
		Function.Call(Hash.DRAW_POLY, point7.X, point7.Y, point7.Z, point6.X, point6.Y, point6.Z, point5.X, point5.Y, point5.Z, HighlightColor_Full.Red, HighlightColor_Full.Green, HighlightColor_Full.Blue, HighlightColor_Full.Alpha);
		Function.Call(Hash.DRAW_LINE, point.X, point.Y, point.Z, point5.X, point5.Y, point5.Z, HighlightColor_Edge.Red, HighlightColor_Edge.Green, HighlightColor_Edge.Blue, 255);
		Function.Call(Hash.DRAW_LINE, point2.X, point2.Y, point2.Z, point6.X, point6.Y, point6.Z, HighlightColor_Edge.Red, HighlightColor_Edge.Green, HighlightColor_Edge.Blue, 255);
		Function.Call(Hash.DRAW_LINE, point3.X, point3.Y, point3.Z, point7.X, point7.Y, point7.Z, HighlightColor_Edge.Red, HighlightColor_Edge.Green, HighlightColor_Edge.Blue, 255);
		Function.Call(Hash.DRAW_LINE, point4.X, point4.Y, point4.Z, point8.X, point8.Y, point8.Z, HighlightColor_Edge.Red, HighlightColor_Edge.Green, HighlightColor_Edge.Blue, 255);
		Function.Call(Hash.DRAW_POLY, point.X, point.Y, point.Z, point4.X, point4.Y, point4.Z, point5.X, point5.Y, point5.Z, HighlightColor_Full.Red, HighlightColor_Full.Green, HighlightColor_Full.Blue, HighlightColor_Full.Alpha);
		Function.Call(Hash.DRAW_POLY, point5.X, point5.Y, point5.Z, point4.X, point4.Y, point4.Z, point8.X, point8.Y, point8.Z, HighlightColor_Full.Red, HighlightColor_Full.Green, HighlightColor_Full.Blue, HighlightColor_Full.Alpha);
		Function.Call(Hash.DRAW_POLY, point2.X, point2.Y, point2.Z, point5.X, point5.Y, point5.Z, point6.X, point6.Y, point6.Z, HighlightColor_Full.Red, HighlightColor_Full.Green, HighlightColor_Full.Blue, HighlightColor_Full.Alpha);
		Function.Call(Hash.DRAW_POLY, point2.X, point2.Y, point2.Z, point.X, point.Y, point.Z, point5.X, point5.Y, point5.Z, HighlightColor_Full.Red, HighlightColor_Full.Green, HighlightColor_Full.Blue, HighlightColor_Full.Alpha);
		Function.Call(Hash.DRAW_POLY, point3.X, point3.Y, point3.Z, point2.X, point2.Y, point2.Z, point6.X, point6.Y, point6.Z, HighlightColor_Full.Red, HighlightColor_Full.Green, HighlightColor_Full.Blue, HighlightColor_Full.Alpha);
		Function.Call(Hash.DRAW_POLY, point3.X, point3.Y, point3.Z, point6.X, point6.Y, point6.Z, point7.X, point7.Y, point7.Z, HighlightColor_Full.Red, HighlightColor_Full.Green, HighlightColor_Full.Blue, HighlightColor_Full.Alpha);
		Function.Call(Hash.DRAW_POLY, point3.X, point3.Y, point3.Z, point7.X, point7.Y, point7.Z, point8.X, point8.Y, point8.Z, HighlightColor_Full.Red, HighlightColor_Full.Green, HighlightColor_Full.Blue, HighlightColor_Full.Alpha);
		Function.Call(Hash.DRAW_POLY, point8.X, point8.Y, point8.Z, point4.X, point4.Y, point4.Z, point3.X, point3.Y, point3.Z, HighlightColor_Full.Red, HighlightColor_Full.Green, HighlightColor_Full.Blue, HighlightColor_Full.Alpha);
	}
}
