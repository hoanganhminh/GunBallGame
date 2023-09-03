using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine;
using System.IO;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace Assets.Scripts
{
    public class SaveManager : MonoBehaviour
    {
        public string directory = "/SaveData/";
        public string fileName = "saveData.txt";

        public void Save(SaveData data)
        {
            string dir = "C:/Users/hp/Desktop/";
            if (!Directory.Exists(dir))
            {
                Debug.Log("chua ton tai");
                Directory.CreateDirectory(dir);
            }
            FileInfo fi = new FileInfo(dir + fileName);
            using (TextWriter txtWriter = new StreamWriter(fi.Open(FileMode.Truncate)))
            {
                foreach (var moverData in data.Characters)
                {
                    // Serialize to json
                    var jsonData = JsonUtility.ToJson(moverData);
                    txtWriter.WriteLine(jsonData);
                }
            }
        }

        public SaveData Load()
        {
            // Retrieve json data from storage of your choice
            string fullPath = "C:/Users/hp/Desktop/" + fileName;
            SaveData saveData = new SaveData();
            saveData.Characters = new List<MoverData>();

            using (StreamReader sr = new StreamReader(fullPath))
            {
                while (sr.Peek() >= 0)
                {
                    var jsonData = sr.ReadLine();
                    MoverData moverData = JsonUtility.FromJson<MoverData>(jsonData);
                    saveData.Characters.Add(moverData);
                }
            }
            return saveData;
        }

    }
}
