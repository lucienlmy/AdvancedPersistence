using System;
using System.IO;

namespace AdvancedPersistence;

public static class Logging
{
	public static readonly string Filename = "AdvPer_LOGS.txt";

	public static void Log(string s)
	{
		try
		{
			if (Directory.Exists("scripts/AdvancedPersistence"))
			{
				StreamWriter streamWriter = new StreamWriter("scripts/AdvancedPersistence/" + Filename, append: true);
				streamWriter.WriteLine($"[{DateTime.Now}] {s}");
				streamWriter.Close();
			}
			else
			{
				StreamWriter streamWriter2 = new StreamWriter("scripts/" + Filename, append: true);
				streamWriter2.WriteLine($"[{DateTime.Now}] {s}");
				streamWriter2.Close();
			}
		}
		catch (Exception)
		{
		}
	}

	public static void Reset()
	{
	}
}
