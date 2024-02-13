using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int level;
    public float maxTime;
    public float time;
    public LevelData levelData;
    public string path;
    public int score;
    private bool isDone = false;
    public Text text;


    public void NextLevel()
    {
        level += 1;

    }

    public void IncreaseScore(int score)
    {
        this.score += score;
        this.score = this.score;
        this.time += 5;


    }

    private void Awake()
    {
        var a = GameObject.Find(gameObject.name);
        if (a.GetInstanceID() != gameObject.GetInstanceID())
        {
            Destroy(gameObject);

        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if (time < 0 && !isDone)
        {
            GameObject.Find("PointerManager").GetComponent<PointerManager>().ShowWin();
            text = GameObject.Find("scoreText").GetComponent<Text>();
            text.text = this.score.ToString();
            StartCoroutine(AddScore());
            isDone = true;
        }
    }

    public IEnumerator LoadLevelData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/lineconnect/configuration"))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                var text = www.downloadHandler.text;
                var config = JsonUtility.FromJson<LevelDataStructContainer>(text);
                var levelDataArray = config.data;
                levelData.Flush();
                for (int i = 0; i < levelDataArray.Length; i++)
                {
                    levelData.level.Add(levelDataArray[i].level);
                    levelData.xSize.Add(levelDataArray[i].x_grid);
                    levelData.ySize.Add(levelDataArray[i].y_grid);
                    levelData.agent.Add(levelDataArray[i].agent_count);
                    levelData.rock.Add(levelDataArray[i].rock);


                }
            }
            else
            {
                Debug.Log(www.error);
            }
        }

    

/*        string pathToJsonFile = path;
        levelData.Flush();
        if (File.Exists(pathToJsonFile))
        {
            string jsonString = System.IO.File.ReadAllText(path);

            LevelDataStructContainer levelDataArrayC = JsonUtility.FromJson<LevelDataStructContainer>(jsonString);
            var levelDataArray = levelDataArrayC.data;

            for (int i = 0; i < levelDataArray.Length; i++)
            {
                levelData.level.Add(levelDataArray[i].level);
                levelData.xSize.Add(levelDataArray[i].x_grid);
                levelData.ySize.Add(levelDataArray[i].y_grid);
                levelData.agent.Add(levelDataArray[i].agent);

            }
        }
        else
        {
            Debug.Log("JSON file not found.");
        }*/
    } 

    IEnumerator AddScore()
    {
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm("http://localhost:3000/lineconnect/" + this.score, "POST"))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Success");
                yield return new WaitForSeconds(1f);

            }
            else
            {
                Debug.Log(www.error);
            }
        }
    }

    public void PlayAgain()
    {
        time = maxTime;
        isDone = false;
        level = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
