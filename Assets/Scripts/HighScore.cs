using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerData data = SaveSystem.LoadPlayer("player");
        GetComponent<Text>().text = data.highScores[0].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
