using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum GameState { Intro, Dialog, Combat, GameOver, FirstCombat, FirstGameOver }

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
    bool FirstChump;

    int introProgress;

	void Start () {
        DontDestroyOnLoad(this);
        LoadDefaultChump();
        State = GameState.Intro;
        Idle = false;
        introProgress = 0;
        firstLostProgress = 0;
        FirstChump = true;
        combatsPlayed = 0;
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
                    if (combat.ChumpWon) {
                        Snark();
                        State = GameState.GameOver;
                    } else {
                        if (FirstChump) {
                            FirstChump = false;
                            State = GameState.FirstGameOver;
                            StartCoroutine(FirstChumpLostScene());
                        } else {
                            StartCoroutine(ChumpLostScene());
                            Chump = Noob;
                            Chump.Reset();
                            ChumpDayCount = 0;
                            Chump.GuySprite = NoobFrontSprites[NoobSpriteNum];
                            State = GameState.GameOver;
                        }
                    }
                }
            } else if (State == GameState.GameOver) {
                ChumpDayCount++;
                combatsPlayed++;
                Destroy(CombatManager);
                StartCoroutine(StartNewBattle());
            } else if (State == GameState.FirstGameOver) {
                Chump = Noob;
                Chump.Reset();
                ChumpDayCount = 0;
                Chump.GuySprite = NoobFrontSprites[NoobSpriteNum];
                if (firstLostProgress > firstLostText.Length - 1) {
                    State = GameState.GameOver;
                    StartCoroutine(LossPause());
                } else {
                    StartCoroutine(PlayDialog(firstLostText[firstLostProgress++]));
                }
            }
        }
    }

    void Snark() {
        string s;

        if(Chump.Health < 20) {
            s = RandFrom(new string[] { "That was close!" });
        } else if (Chump.Health >= (Chump.TotalHealth - 20)) {
            s = RandFrom(new string[] { "You haven't lost your touch.", "Barely a scratch!",
                "Now THAT'S what it means to be Elite!", "Is it over already?" });
        } else {
            if(Noob.lvl == 0) {
                s = RandFrom(new string[] {
                    "You're pretty sure that kid was 10 years old",
                    "You wonder where Melanie and Krow are going to lunch today. You could use a good sandwich...",
                    "It's not even 9 o'clock yet. And you woke up for THIS?",
                    "Good thing that was a quick fight. Doc is supposed to be fighting some real wiz kid soon..."});
            } else if(Noob.lvl == 1){
                s = RandFrom(new string[] { "They don't make wizards like they used to.",
                    "Why do you always get the boring challenges?",
                    "You wonder if Melanie would go out to the movies with you if you asked...",
                    "Another day, another win."});
            } else if(Noob.lvl == 2) {
                s = RandFrom(new string[] { "She actually did a pretty good job, considering she was 4 feet tall.",
                    "what a weird kid, so silent and brooding. The Doc would have loved them.",
                    "You hope Krow didn't see that. Not your best showing."});
            } else {
                s = RandFrom(new string[] { "That was close! What was that?",
                    "Wow, what are they feeding that kid?",
                    "Now there's a kid that'll probably have your job someday."});
            }
        }

        StartCoroutine(PlaySmallDialog(s, 1, 4));
    }

    string RandFrom(string[] strings) {
        return strings[Random.Range(0, strings.Length)];
    }

    int firstLostProgress;
    string[] firstLostText = { "This is impossible. You can't believe you just lost to this kid. After all these years... " +
            "Have you just gotten sloppy, or are these new kids getting better?",
        "Maybe You've just become arrogant. After so many years, it felt like you'd always been an Elite Wizard " +
            "and always would be. ", "When did that hopeful young duelist you used to be grow up?",
        "Maybe... maybe this is a sign. Perhaps it's time to move on with your life. There must be something out there other than this." };

    
    IEnumerator LossPause() {
        yield return new WaitForSeconds(5);
    }

    IEnumerator FirstChumpLostScene() {
        Idle = true;
        yield return new WaitForSeconds(1);
        GameObject dialog = (GameObject)Instantiate(Resources.Load("SmallDialogBox"));
        Text text = GameObject.Find("SmallText").GetComponent<Text>();
        text.text = "Wh- what just happened?";
        yield return new WaitForSeconds(2);
        Destroy(CombatManager);
        Destroy(dialog);

        Idle = false;
    }

    IEnumerator ChumpLostScene() {
        Idle = true;
        yield return new WaitForSeconds(2);
        GameObject dialog = (GameObject)Instantiate(Resources.Load("SmallDialogBox"));
        Text text = GameObject.Find("SmallText").GetComponent<Text>();
        text.text = "Wh- what just happened? This is impossible...";
        yield return new WaitForSeconds(2);
        Destroy(CombatManager);
        
        Destroy(dialog);
        yield return new WaitForSeconds(2);


        Idle = false;
    }

    IEnumerator PlaySmallDialog(string s, int i, int t) {
        Idle = true;
        yield return new WaitForSeconds(i);
        GameObject dialog = (GameObject)Instantiate(Resources.Load("SmallDialogBox"));
        Text text = GameObject.Find("SmallText").GetComponent<Text>();
        text.text = s;
        yield return new WaitForSeconds(t);
        Destroy(dialog);
        Idle = false;
    }

    IEnumerator StartNewBattle() {
        Idle = true;

        yield return new WaitForSeconds(1);
        GameObject dialog = (GameObject)Instantiate(Resources.Load("SmallDialogBox"));
        Text text = GameObject.Find("SmallText").GetComponent<Text>();
        text.text = "Day " + ChumpDayCount + "...";
        yield return new WaitForSeconds(2);
        Destroy(dialog);

        CombatManager = (GameObject)Instantiate(Resources.Load("CombatManager"));
        combat = CombatManager.GetComponent<Combat>();
        NoobSpriteNum = Random.Range(0, NoobBackSprites.Length);
        Chump.Reset();

        //// NOOB LEVEL CONTROLS
        if (combatsPlayed == 0) {
            Noob = new Guy(2, NoobBackSprites[NoobSpriteNum]);
        } else if (combatsPlayed == 1) {
            Noob = new Guy(0, NoobBackSprites[NoobSpriteNum]);
        } else if (combatsPlayed < 4) {
            Noob = new Guy(1, NoobBackSprites[NoobSpriteNum]);
        } else if (combatsPlayed > 6) {
            Noob = new Guy(NoobBackSprites[NoobSpriteNum]);
        } else {
            Noob = new Guy(NoobBackSprites[NoobSpriteNum]);
            while(Noob.Attacks.Length > 3) {
                Noob = new Guy(NoobBackSprites[NoobSpriteNum]);
            }
        }
        combat.InitializeCombat(Chump, Noob);
        
        State = GameState.Combat;
        combat.StartCombat();

        Idle = false;
    }

    int combatsPlayed;

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
        Chump.TotalHealth = 90;
        ChumpDayCount = 9989;
    }
}

public class Elite {
    public string Name { get; set; }

}
