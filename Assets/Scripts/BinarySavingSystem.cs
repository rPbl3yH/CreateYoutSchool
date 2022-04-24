using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class BinarySavingSystem
{
     public static void SaveData()
     {
          BinaryFormatter formatter = new BinaryFormatter();
          string path = Application.persistentDataPath + "/game.b";
          FileStream stream = new FileStream(path, FileMode.Create);

          BuildingSystem buildingSystem = BuildingSystem.Current;

          DataSystem dataSystem = new DataSystem(buildingSystem.CountBuilding);
          
          formatter.Serialize(stream, dataSystem);
          stream.Close();
     }

     public static DataSystem LoadData()
     {
          string path = Application.persistentDataPath + "/game.b";
          if (File.Exists(path))
          {
               BinaryFormatter formatter = new BinaryFormatter();
          
               FileStream stream = new FileStream(path, FileMode.Open);
          
               DataSystem dataSystem = formatter.Deserialize(stream) as DataSystem;
               stream.Close();

               return dataSystem;
          }
          else
          {
               Debug.Log("Don't exist file in this path: " + path);
               return null;
          }
     }
}
