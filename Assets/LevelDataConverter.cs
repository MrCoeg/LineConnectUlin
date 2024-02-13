using UnityEngine;

public static class LevelDataConverter
{
    public static LevelDataStruct ConvertToStruct(LevelData levelData)
    {
        LevelDataStruct convertedData = new LevelDataStruct();

        if (levelData != null)
        {
            for (int i = 0; i < Mathf.Min(levelData.level.Count, Mathf.Min(levelData.xSize.Count, Mathf.Min(levelData.ySize.Count, levelData.agent.Count))); i++)
            {
                convertedData.level = levelData.level[i];
                convertedData.x_grid = levelData.xSize[i];
                convertedData.y_grid = levelData.ySize[i];
                convertedData.agent_count = levelData.agent[i];
            }
        }

        return convertedData;
    }

}
