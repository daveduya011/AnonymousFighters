using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPrompt : MonoBehaviour
{
    private Text text;
    public void SetMessage(string message) {
        text = GetComponentInChildren<Text>();
        text.text = message;
        gameObject.SetActive(true);
    }
    public void HideMessage() {
        gameObject.SetActive(false);
    }
}
