using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_Start : MonoBehaviour
{
    public GameObject playerNameObj;
    // Start is called before the first frame update
    void Start()
    {
        playerNameObj = GameObject.Find("PlayerName");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
        playerNameObj.GetComponent<PlayerName>().SaveNameData();
        SceneManager.LoadScene(1);
    }
}
