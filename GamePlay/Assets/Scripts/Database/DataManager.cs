using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager
{
    public static void BinarySerialize<T>(T t, string fileName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream("C:/FrozenWorld_Data/" + fileName, FileMode.Create);
        formatter.Serialize(stream, t);
        stream.Close();
    }

    public static T BinaryDeserialize<T>(string fileName)
    {
        if (File.Exists("C:/FrozenWorld_Data/" + fileName))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream("C:/FrozenWorld_Data/" + fileName, FileMode.Open);
            T t = (T)formatter.Deserialize(stream);
            stream.Close();

            return t;
        }
        return default(T);
    }
}