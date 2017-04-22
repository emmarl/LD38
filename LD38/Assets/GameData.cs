using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour {

    public Sprite[] NoobBackSprites;
    public Sprite[] NoobFrontSprites;
    public Sprite MelanieSprite;
    public Sprite DocQuackSprite;
    public Sprite KrowSprite;
    public Sprite ChumpSprite;

    public Guy Chump;
    GameObject CombatManager;
    Combat combat;

	void Start () {
        DontDestroyOnLoad(this);

        LoadDefaultChump();

        NewCombat();
	}

    void Update() {
        if(combat.CurrentState == CombatState.GameOver) {
            Destroy(CombatManager);
            NewCombat();
        }
    }

    void NewCombat() {
        CombatManager = (GameObject)Instantiate(Resources.Load("CombatManager"));
        combat = CombatManager.GetComponent<Combat>();
        combat.InitializeCombat(Chump, new Guy(0));
        combat.StartCombat();
    }

    void LoadDefaultChump() {
        Chump = new Guy(3);
        Chump.Name = "Renaldo";
        Chump.TotalHealth = 200;
        Chump.SetSprite(Resources.Load<Sprite>("chump"));
    }
}

public class Elite {
    public string Name { get; set; }

}
