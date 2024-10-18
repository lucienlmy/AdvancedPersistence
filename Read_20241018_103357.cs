using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using GTA;

namespace AdvancedPersistence
{
    public class Constants
    {
        public const float Version = 1.61f;
        public const bool DebugMode = false;
        public const string SubFolder = "AdvancedPersistence";
    }

    public static class IniWriter
    {
        public static void WriteVehicleDataToIni(VehicleDataV1 vehicleData, string filePath)
        {
            // Ensure the directory exists
            FileInfo fileInfo = new FileInfo(filePath);
            if (!Directory.Exists(fileInfo.DirectoryName))
            {
                Directory.CreateDirectory(fileInfo.DirectoryName);
            }

            // Use StringBuilder to construct the INI content
            StringBuilder iniBuilder = new StringBuilder();
            iniBuilder.AppendLine("[VehicleData]");
            iniBuilder.AppendLine($"Id={vehicleData.Id}");
            iniBuilder.AppendLine($"LicensePlate={vehicleData.LicensePlate}");
            // Add other properties here

            // Save the file
            File.WriteAllText(filePath, iniBuilder.ToString());
        }

        public static void WriteCharacterDataToIni(CharacterDataV2 characterData, string filePath)
        {
            // Ensure the directory exists
            FileInfo fileInfo = new FileInfo(filePath);
            if (!Directory.Exists(fileInfo.DirectoryName))
            {
                Directory.CreateDirectory(fileInfo.DirectoryName);
            }

            // Use StringBuilder to construct the INI content
            StringBuilder iniBuilder = new StringBuilder();
            iniBuilder.AppendLine("[CharacterData]");
            iniBuilder.AppendLine($"Position={{{characterData.Position.X},{characterData.Position.Y},{characterData.Position.Z}}}");
            iniBuilder.AppendLine($"Heading={characterData.Heading}");
            iniBuilder.AppendLine($"CarAttach={characterData.CarAttach}");
            // Add other properties here

            // Save the file
            File.WriteAllText(filePath, iniBuilder.ToString());
        }
    }

    public class AdvancedPersistence : Script
    {
        public static readonly string VehicleFilename = "scripts/AdvancedPersistence/AdvPer_VEH_DB.bin";
        public static readonly string VehicleMetaname = "scripts/AdvancedPersistence/AdvPer_VEH_META.bin";
        public static readonly string CharacterFilename = "scripts/AdvancedPersistence/AdvPer_CHR_DB.bin";
        public static readonly string CharacterMetaname = "scripts/AdvancedPersistence/AdvPer_CHR_META.bin";

        public static readonly string VehicleFilenameIni = "scripts/AdvancedPersistence/AdvPer_VEH_DB.ini";
        public static readonly string VehicleMetanameIni = "scripts/AdvancedPersistence/AdvPer_VEH_META.ini";
        public static readonly string CharacterFilenameIni = "scripts/AdvancedPersistence/AdvPer_CHR_DB.ini";
        public static readonly string CharacterMetanameIni = "scripts/AdvancedPersistence/AdvPer_CHR_META.ini";

        public static void SaveVehicleData(Vehicle veh, VehicleDataV1 dat)
        {
            // Save as binary format
            // Your existing binary save logic here

            // Also save as INI format
            string iniFilePath = Path.Combine(Directory.GetCurrentDirectory(), VehicleFilenameIni);
            IniWriter.WriteVehicleDataToIni(dat, iniFilePath);
        }

        public static void SaveCharacterData(Ped ped, CharacterDataV2 dat)
        {
            // Save as binary format
            // Your existing binary save logic here

            // Also save as INI format
            string iniFilePath = Path.Combine(Directory.GetCurrentDirectory(), CharacterFilenameIni);
            IniWriter.WriteCharacterDataToIni(dat, iniFilePath);
        }

        // Other methods and logic...
    }

    // Your existing classes and logic...
}