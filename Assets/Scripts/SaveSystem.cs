using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveSettings(Settings settings) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/settings.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        SettingsData data = new SettingsData(settings);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveSettings(SettingsData settingsData) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/settings.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        SettingsData data = new SettingsData(settingsData);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SettingsData LoadSettings() {
        string path = Application.persistentDataPath + "/settings.dat";
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SettingsData data = formatter.Deserialize(stream) as SettingsData;
            stream.Close();

            return data;
        }
        else {
            SettingsData data = new SettingsData();
            return data;
        }
    }

    public static void SaveInventory(InventoryData inventoryData) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/inventory.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        InventoryData data = new InventoryData(inventoryData);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static InventoryData LoadInventory() {
        string path = Application.persistentDataPath + "/inventory.dat";
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            InventoryData data = formatter.Deserialize(stream) as InventoryData;
            stream.Close();

            return data;
        }
        else {
            InventoryData data = new InventoryData();
            return data;
        }
    }


    public static void SavePlayer(PlayerData playerData) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = GetAppPath("player");
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(playerData);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer(string fileName) {
        string path = GetAppPath("player");
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else {
            PlayerData data = new PlayerData();
            return data;
        }
    }

    private static string GetAppPath(string fileName) {
        return Application.persistentDataPath + "/" + fileName + ".dat";
    }
}
