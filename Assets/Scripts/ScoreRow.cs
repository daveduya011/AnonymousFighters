using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreRow : MonoBehaviour
{
    public string username;
    public long score;
    public int rank;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateScore(string username, long score, int rank) {
        gameObject.transform.Find("rank").GetComponent<Text>().text = rank.ToString();
        gameObject.transform.Find("score").GetComponent<Text>().text = score.ToString();
        gameObject.transform.Find("name").GetComponent<Text>().text = username;
    }
}
