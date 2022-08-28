using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class PlayerName : MonoBehaviour
{
    public static PlayerName Instance;

    private Text playerName;
    public string playerNameData;
    // Start is called before the first frame update

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "menu")
        {
            playerName = GameObject.Find("NameText").GetComponent<Text>();
        }
        playerNameData = playerName.text;
        SaveNameData();
    }


    [System.Serializable]
    class SaveData
    {
        public string playerNameData;
    }
    public void SaveNameData()
    {
        SaveData data = new SaveData();
        data.playerNameData = playerNameData;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/namedatasave.json", json);
    }

    public void LoadNameData()
    {
        string path = Application.persistentDataPath + "/namedatasave.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            playerNameData = data.playerNameData;

        }
    }
}
