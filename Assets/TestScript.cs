using System.IO;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public string path;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void TestLoad()
    {
        string pathToJsonFile = path;

        if (File.Exists(pathToJsonFile))
        {
            string jsonString = File.ReadAllText(path);

            LevelDataStructContainer levelDataArrayC = JsonUtility.FromJson<LevelDataStructContainer>(jsonString);
            var levelDataArray = levelDataArrayC.data;
            foreach (var data in levelDataArray)
            {
                Debug.Log($"Level: {data.level}, X_Grid: {data.x_grid}, Y_Grid: {data.y_grid}, Agent: {data.agent_count}");
            }
        }
        else
        {
            Debug.Log("JSON file not found.");
        }
    }

}

[System.Serializable]
public struct LevelDataStructContainer
{
    public LevelDataStruct[] data;
}

[System.Serializable]
public struct LevelDataStruct
{
    public int level;
    public int x_grid;
    public int y_grid;
    public int agent_count;
    public int rock;

}
