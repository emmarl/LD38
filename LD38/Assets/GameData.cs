using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum GameState { Intro, Dialog, Combat, Scoring, FirstCombat }

public class GameData : MonoBehaviour {

    public Sprite[] NoobBackSprites;
    public Sprite[] NoobFrontSprites;
    public Sprite MelanieSprite;
    public Sprite DocQuackSprite;
    public Sprite KrowSprite;
    public Sprite ChumpSprite;

    public Guy Chump;
    public Guy Noob;
    int NoobSpriteNum;
    int ChumpDayCount;

    GameObject CombatManager;
    Combat combat;
    GameState State;
    bool Idle;

    int introProgress;

	void Start () {
        DontDestroyOnLoad(this);
        LoadDefaultChump();
        State = GameState.Intro;
        Idle = false;
        introProgress = 0;
	}

    void Update() {
        if (!Idle) {
            if (State == GameState.Intro) {
                if (introProgress > introText.Length - 1) {
                    State = GameState.FirstCombat;
                } else {
                    StartCoroutine(PlayDialog(introText[introProgress++]));
                }
            } else if (State == GameState.FirstCombat) {
                StartCoroutine(StartNewBattle());
            } else if (State == GameState.Combat) {
                if (combat.CurrentState == CombatState.GameOver) {
                    Destroy(CombatManager);
                    State = GameState.Scoring;
                    Debug.Log("Game over, scoring mode");             
                }
            }
        }
    }

    IEnumerator StartNewBattle() {
        Idle = true;

        GameObject dialog = (GameObject)Instantiate(Resources.Load("SmallDialogBox"));
        Text text = GameObject.Find("SmallText").GetComponent<Text>();
        text.text = "Day " + ChumpDayCount + "...";
        Debug.Log(text.text);
        Debug.Log(dialog);
        yield return new WaitForSeconds(2);
        Destroy(dialog);

        CombatManager = (GameObject)Instantiate(Resources.Load("CombatManager"));
        combat = CombatManager.GetComponent<Combat>();
        NoobSpriteNum = Random.Range(0, NoobBackSprites.Length);
        Noob = new Guy(0, NoobBackSprites[NoobSpriteNum]);
        combat.InitializeCombat(Chump, Noob);
        
        State = GameState.Combat;
        combat.StartCombat();
        Idle = false;
        yield return null;
    }

    string[] introText =
        {"You were 12 years old when you swore on your grandfather's grave" +
            " that you'd become the greatest wizard of all time.",
        "He'd been a famous wizard duelist himself, " +
            "known for his fabulous fire magic and generous personality.",
        "It took 5 years for you to become a wizard master, and 2 more for " +
            "you to accomplish that most difficult and farfetched of childhood " +
            "dreams:",  "To beat one of the 4 Elite Wizards and become an Elite Wizard yourself.",
        "It's an incredible honor, and you're known throughout the land for " +
            "your magical prowess. However, the day to day of it is less glamorous" +
            " than you would expect.", "After all, most of your time is spent battling " +
            "those walking in your footsteps, those bright-eyed ambitious youths chasing fame, power, and glory..."};

    IEnumerator PlayDialog(string s) {
        Idle = true;
        GameObject dialog = (GameObject)Instantiate(Resources.Load("DialogBox"));
        Text text = GameObject.Find("Text").GetComponent<Text>();
        text.text = s;
        while (!Input.GetMouseButtonDown(0)) {
            yield return null;
        }
        Destroy(dialog);
        Idle = false; 
    }

    void LoadDefaultChump() {
        Chump = new Guy(3, ChumpSprite);
        Chump.Name = "Renaldo";
        Chump.TotalHealth = 200;
        ChumpDayCount = 1523;
    }
}

public class Elite {
    public string Name { get; set; }

}
