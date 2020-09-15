using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    public bool isOnline;
    private Toggle toggle;
    private Animator animator;
    public LeaderboardManager leaderboardManager;
    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();
        animator = GetComponent<Animator>();

        OnToggle();
    }

    public void OnToggle() {
        if (toggle.isOn) {
            animator.SetBool("isOn", true);
            if (isOnline)
                leaderboardManager.ShowOnlineScores();
            else
                leaderboardManager.ShowOfflineScores();
        }
        else {
            animator.SetBool("isOn", false);
        }
    }
}
