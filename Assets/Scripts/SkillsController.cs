using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillsController : MonoBehaviour
{
    private PlayerController player;
    private Skill[] skills;
    public Button[] skillButtons;
    private Image[] timerSliders;
    private Image[] timerIcons;
    private Text[] timerTexts;
    public Image basicAttackImage;
    private Color hiddenColor;
    private Color initialColor;

    private bool isAttacking;
    // Start is called before the first frame update
    void Start() {
        hiddenColor = new Color(0,0,0,0);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        skills = player.skillData.skills;
        timerSliders = new Image[skillButtons.Length];
        timerIcons = new Image[skillButtons.Length];
        timerTexts = new Text[skillButtons.Length];
        for (int i = 0; i < skillButtons.Length; i++) {
            timerSliders[i] = skillButtons[i].transform.Find("timerImage").GetComponent<Image>();
            timerIcons[i] = skillButtons[i].transform.Find("icon").GetComponent<Image>();
            timerTexts[i] = skillButtons[i].transform.Find("timer").GetComponent<Text>();
            skills[i].currentTime = 0;

            timerIcons[i].sprite = player.skillData.skills[i].icon;
        }
        initialColor = timerTexts[0].color;
        basicAttackImage.sprite = player.attacksData.icon;
    }
    void Update() {
        if (isAttacking) {
            player.Attack();
        }
        for (int i = 0; i < skillButtons.Length; i++) {
            Skill skill = skills[i];
            if (skill.currentTime > 0) {
                skill.currentTime -= Time.deltaTime;
                skill.isOn = false;
                skillButtons[i].interactable = false;
                timerTexts[i].color = initialColor;
                timerTexts[i].text = ((int)skill.currentTime).ToString();
            }
            else {
                skill.isOn = true;
                skillButtons[i].interactable = true;
                timerTexts[i].color = hiddenColor;
            }
            timerSliders[i].fillAmount = 1 / (skill.waitTime / skill.currentTime);
        }
    }

    public void ExecuteSkill(int skillNumber) {
        if (!player.isPerformingSkill && !player.isStunned && skills[skillNumber].isOn) {
            player.ExecuteSkill(skills[skillNumber].animationName, skillNumber);
        }
    }

    public void Attack() {
        isAttacking = true;
    }

    public void EndAttack() {
        isAttacking = false;
    }
}
