using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
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
            UserData data = formatter.Deserialize(stream) as UserData;
            stream.Close();
            return data;
        }
        return null;
    }
}
