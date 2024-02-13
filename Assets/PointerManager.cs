using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PointerManager : MonoBehaviour
{
    [SerializeField] private Color activeColor;
    [SerializeField] public bool isActive;

    [SerializeField] public List<PointerTemp> pointers = new List<PointerTemp>();
    [SerializeField] public List<PathTemp> path = new List<PathTemp>();
    [SerializeField] public GameObject win;
    [SerializeField] public GameObject start;

    public LevelSC sc;

    public Text levelText;
    public Text timeText;


    public PathTemp activePath;
    public PointerTemp activePointer;
    public int xSize;
    public int ySize;
    public int level = 1;
    public LevelData data;
    public GameManager gameManager;
    public GeneratatorTemp temp;
    public LevelGenerator generator;
    private float time;

    private void Update()
    {
        
        if (gameManager != null)
        {
            timeText.text =  ((int)(gameManager.time/60)).ToString() + "."+((int)(gameManager.time % 60)).ToString() ;
        }
    }

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        var itemGen = gameManager.levelData.getData(gameManager.level-1);
        generator.gridHeight = itemGen.ySize;
        generator.gridWidth = itemGen.xSize;
        generator.color = itemGen.agent;

        generator.Generate();
        levelText.text = gameManager.level.ToString();
        level = gameManager.level;
        if (gameManager.level > 1)
        {
            start.SetActive(false);
        }

    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2);
        start.SetActive(false);
    }

    private void Start()
    {
        foreach (var item in pointers)
        {
            item.Init(this);
        }
    }

    public void PlayAgain()
    {
        gameManager.PlayAgain();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public PathTemp GetPath(int id)
    {
        return path.Find(x => x.pathId == id);
    }
    public PathTemp GetActivePath()
    {
        return activePath;
    }

    public void Check()
    {
        foreach (var item in pointers)
        {
            if (!item.isFilled)
            {
                return;
            }
        }

        gameManager.level += 1;
        gameManager.IncreaseScore((int)(generator.color * Mathf.Lerp(1,2, (20 - time)/20)) * 10);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    public void ShowWin()
    {
        win.SetActive(true);
    }
    

    public void Restart()
    {
        gameManager.PlayAgain();   
    }

    public void SetRock(int rock)
    {
        var counter = 0;
        foreach (var item in path)
        {
            if (item.truePath.Count == 1)
            {
                item.truePath[0].Rock();
                counter += 1;
            }
        }
        if (counter > rock || path.Count != gameManager.levelData.agent[gameManager.level-1])
        {
/*            for (int i = 0; i < path.Count; i++)
            {
                Destroy(path[i]);
            }
            path.Clear();
            foreach (var item in pointers)
            {
                item.id = 0;
                item.pathId = 0;
                item.image.color = item.resetColor;

            }*/
/*            StartCoroutine(temp.RegenAgain());*/
/*            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);*/


        }
        else
        {
/*            sc.AddLevel(gameManager.level
                -1, pointers);*/
/*
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);*/
        }

    }

    public void Init()
    {
        foreach (var item in pointers)
        {
            item.Init(this);
        }
    }

    public void Log()
    {
        StartCoroutine(LogFile());
    }

    public IEnumerator LogFile()
    {

        var texture = ScreenCapture.CaptureScreenshotAsTexture();
        var imageByte = texture.EncodeToPNG();

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm("http://localhost:3000/LogLineConnect", "POST"))
        {

            // Attach the form to the request
            www.uploadHandler = new UploadHandlerRaw(imageByte);
            www.uploadHandler.contentType = "multipart/form-data";
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Berhasil");
            }
            else
            {
                Debug.Log(www.error);
            }
        }
    }

    public void SetActive(PointerTemp color)
    {
        isActive = true;
        this.activePointer = color;
    }

    public void Reset()
    {
        isActive = false;
    }
}
