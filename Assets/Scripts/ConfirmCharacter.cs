using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ConfirmCharacter : MonoBehaviour
{

    public ToggleGroup toggleGroup;
    private Toggle[] toggles;
    // Start is called before the first frame update
    void Start() {
        toggles = toggleGroup.GetComponentsInChildren<Toggle>();
    }
    public void SelectCharacter() {
        for (int i = 0; i < toggles.Length; i++) {
            if (toggles[i].isOn) {
                GameManager.Instance.characterObject = toggles[i].GetComponent<CharacterSelection>().characterPrefab;
                SceneManager.LoadScene("MainGame");
                break;
            }
        }

    }
}
