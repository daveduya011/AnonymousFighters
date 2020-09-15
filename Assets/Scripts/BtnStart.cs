using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnStart : MonoBehaviour
{
    public void PressStart() {
        GameManager.Instance.ResetValues();
        SceneManager.LoadScene("CharacterSelection");
    }
}
