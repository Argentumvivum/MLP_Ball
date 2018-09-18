using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SavingSystem
{
    //streamingAssetsPath for release, persistentDataPath for testing
    public void SaveData(List<InputData> inputDatas)
    {
        if (File.Exists(Application.streamingAssetsPath + "/networkData.dat"))
        {
            File.Delete(Application.streamingAssetsPath + "/networkData.dat");
        }

        var bf = new BinaryFormatter();
        var file = File.Create(Application.streamingAssetsPath + "/networkData.dat");

        var list = new InputDataList
        {
            Data = inputDatas
        };

        bf.Serialize(file, list);
        file.Close();
    }

    public List<InputData> LoadData(List<InputData> inputDatas, bool isStandard)
    {
        if(isStandard && File.Exists(Application.streamingAssetsPath + "/standardData.dat"))
        {
            var bf = new BinaryFormatter();
            var file = File.Open(Application.streamingAssetsPath + "/standardData.dat", FileMode.Open);
            var list = (InputDataList)bf.Deserialize(file);

            file.Close();

            inputDatas = list.Data;
        }
        else if(!isStandard && File.Exists(Application.streamingAssetsPath + "/networkData.dat"))
        {
            var bf = new BinaryFormatter();
            var file = File.Open(Application.streamingAssetsPath + "/networkData.dat", FileMode.Open);
            var list = (InputDataList)bf.Deserialize(file);

            file.Close();

            inputDatas = list.Data;
        }

        return inputDatas;
    }
}
