using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveUser(UserData userData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/user.txt";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, userData);
        stream.Close();
    }

    public static UserData LoadUser()
    {
        string path = Application.persistentDataPath + "/user.txt";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            stream.Position = 0;
            UserData data = formatter.Deserialize(stream) as UserData;
            stream.Close();
            return data;
        }
        return new UserData(0, "");
    }
}
