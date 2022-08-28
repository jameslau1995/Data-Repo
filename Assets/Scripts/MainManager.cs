using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    private GameObject InputName;
    private string playerNameDataReplace;
    
    private bool m_Started = false;
    private int m_Points;
    private int b_Points;

    private string b_Name;

    private bool m_GameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        InputName = GameObject.Find("PlayerName");
        InputName.GetComponent<PlayerName>().LoadNameData();
        LoadScoreData();
        playerNameDataReplace = InputName.GetComponent<PlayerName>().playerNameData;

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            RecordPoint();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

   
    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }
    void RecordPoint()
    {
        //if(m_Points > b_Points)
        //{
            b_Points = m_Points;
            b_Name = playerNameDataReplace;
            SaveScoreData();
        //}
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    [System.Serializable]
    class SaveData
    {
        public string b_Name;
        public int b_Points;
    }

    public void SaveScoreData()
    {
        SaveData data = new SaveData();
        data.b_Name = b_Name;
        data.b_Points = b_Points;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/scoredata.json", json);
    }

    public void LoadScoreData()
    {
        string path = Application.persistentDataPath + "/scoredata.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            b_Name = data.b_Name;
            b_Points = data.b_Points;

            BestScoreText.text = $"High Score: {b_Name} - {b_Points}";
        }
    }

}
