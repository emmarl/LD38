using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum Attack { Fireball, Icebeam, Whirlwind, Rocksmash, None }

public enum CombatState { NoobTurn, ChumpTurn, GameOver, Setup }

public class Combat : MonoBehaviour {

    Guy Noob;
    Guy Chump;
    public Slider ChumpHealthBar;
    public Slider NoobHealthBar;

    public GameObject ChumpSprite;
    public GameObject NoobSprite;

    public CombatState CurrentState;
    bool AttackMade;
    
    public void InitializeCombat(Guy chump, Guy noob) {
        CurrentState = CombatState.Setup;


        Chump = chump;
        Noob = noob;
        ChumpHealthBar = GameObject.Find("Chump Health").GetComponent<Slider>();
        ChumpHealthBar.maxValue = Chump.TotalHealth;
        ChumpHealthBar.value = Chump.Health;
        ChumpHealthBar.interactable = false;
        NoobHealthBar = GameObject.Find("Noob Health").GetComponent<Slider>();
        NoobHealthBar.maxValue = Noob.TotalHealth;
        NoobHealthBar.value = Noob.Health;
        NoobHealthBar.interactable = false;
        Debug.Log(ChumpSprite);
        SpriteRenderer ChumpSpriteRenderer = ChumpSprite.GetComponent<SpriteRenderer>();
        ChumpSpriteRenderer.sprite = Chump.GuySprite;
        SpriteRenderer NoobSpriteRenderer = NoobSprite.GetComponent<SpriteRenderer>();
        NoobSpriteRenderer.sprite = Noob.GuySprite;

        AttackMade = false;
    }

    public void StartCombat() {
        CurrentState = CombatState.NoobTurn;
        NoobTurn();
        print("Starting some shit");
    }
    
    void Update () {
        if (CurrentState == CombatState.GameOver) print("game over!!!");
	    if(CurrentState == CombatState.NoobTurn) {
            if(AttackMade) {
                ChumpHealthBar.value = Chump.Health;
                if (Chump.IsDead) {
                    CurrentState = CombatState.GameOver;
                } else {
                    CurrentState = CombatState.ChumpTurn;
                    AttackMade = false;
                }
            }
        } else if(CurrentState == CombatState.ChumpTurn) {
            if (AttackMade) {
                NoobHealthBar.value = Noob.Health; 
                if (Noob.IsDead) {
                    CurrentState = CombatState.GameOver;
                } else {
                    CurrentState = CombatState.NoobTurn;
                    AttackMade = false;
                    NoobTurn();
                }
            }
        }
	}

    private void MakeAttack(Guy victim, Attack attack) {
        int damage = (int)Random.Range(10, 20);
        victim.Health -= damage;

        print(victim.Name + " takes " + damage + " damage from " + attack
            + " (" + victim.Health + " remaining)");

        if (victim.IsDead) {
            CurrentState = CombatState.GameOver;
        }
        AttackMade = true;
    }

    public void Fireball() {
        if (CurrentState == CombatState.ChumpTurn) {
            MakeAttack(Noob, Attack.Fireball);
        }
    }

    public void Icebeam() {
        if (CurrentState == CombatState.ChumpTurn) {
            MakeAttack(Noob, Attack.Icebeam);
        }
    }

    public void Whirlwind() {
        if (CurrentState == CombatState.ChumpTurn) {
            MakeAttack(Noob, Attack.Whirlwind);
        }
    }

    public void Rocksmash() {
        if (CurrentState == CombatState.ChumpTurn) {
            MakeAttack(Noob, Attack.Rocksmash);
        }
    }

    public void NoobTurn() {
        StartCoroutine(Turn());
    }

    IEnumerator Turn() {
        yield return new WaitForSeconds(2);
        MakeAttack(Chump, Noob.Attacks[Random.Range(0, Noob.Attacks.Length)]);
    }
}
