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

    public bool ChumpWon;
    
    public void InitializeCombat(Guy chump, Guy noob) {
        CurrentState = CombatState.Setup;

        ChumpWon = true;
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
        MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    
    void Update () {
	    if(CurrentState == CombatState.NoobTurn) {
            if(AttackMade) {
                ChumpHealthBar.value = Chump.Health;
                if (Chump.IsDead) {
                    ChumpWon = false;
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

    public Camera MainCamera;

    IEnumerator ShakeAttack(Guy victim, Attack attack) {
        MainCamera.GetComponent<Shake>().shake(10, 15);
        yield return new WaitForSeconds(1);
        int damage = (int)Random.Range(10, 20);
        victim.Health -= damage;

        AttackMade = true;
    }

    public GameObject ChumpFireball;
    public GameObject ChumpIcebeam;
    public GameObject ChumpRocksmash;
    public GameObject ChumpWhirlwind;
    public GameObject NoobFireball;
    public GameObject NoobIcebeam;
    public GameObject NoobRocksmash;
    public GameObject NoobWhirlwind;


    public void Fireball() {
        if (CurrentState == CombatState.ChumpTurn) {
            StartCoroutine(ShakeAttack(Noob, Attack.Fireball));
            StartCoroutine(AttackAnim(ChumpFireball, new Vector3(-130, -50, 0), 1));
        }
    }

    IEnumerator AttackAnim(GameObject attack, Vector3 change, float time) {
        GameObject attackInstance = Instantiate(attack);
        Vector3 curr = attackInstance.transform.position;
        Vector3 pos = curr + change;
        float t = 0;
        while(t < 1) {
            t += Time.deltaTime / time;
            attackInstance.transform.position = Vector3.Lerp(curr, pos, t);
            yield return null;
        }
        Destroy(attackInstance);
    }

    public void Icebeam() {
        if (CurrentState == CombatState.ChumpTurn) {
            StartCoroutine(ShakeAttack(Noob, Attack.Icebeam));
            StartCoroutine(AttackAnim(ChumpIcebeam, new Vector3(0, 0, 0), 1));
        }
    }

    public void Whirlwind() {
        if (CurrentState == CombatState.ChumpTurn) {
            StartCoroutine(ShakeAttack(Noob, Attack.Whirlwind));
            StartCoroutine(AttackAnim(ChumpWhirlwind, new Vector3(-200, -50, 0), 1));
        }
    }

    public void Rocksmash() {
        if (CurrentState == CombatState.ChumpTurn) {
            StartCoroutine(ShakeAttack(Noob, Attack.Rocksmash));
            StartCoroutine(AttackAnim(ChumpRocksmash, new Vector3(0, -200, 0), 1));
        }
    }

    public void SkipTurn() {
        if (CurrentState == CombatState.ChumpTurn) {
            AttackMade = true;
        }
    }

    public void NoobTurn() {
        StartCoroutine(Turn());
    }

    IEnumerator Turn() {
        yield return new WaitForSeconds(Random.Range(1,2));
        Attack attack = Noob.Attacks[Random.Range(0, Noob.Attacks.Length)];
        StartCoroutine(ShakeAttack(Chump,attack));
        if(attack == Attack.Fireball) StartCoroutine(AttackAnim(NoobFireball, new Vector3(130, 50, 0), 1));
        if (attack == Attack.Rocksmash) StartCoroutine(AttackAnim(NoobRocksmash, new Vector3(0, -200, 0), 1));
        if (attack == Attack.Icebeam) StartCoroutine(AttackAnim(NoobIcebeam, new Vector3(0, 0, 0), 1));
        if (attack == Attack.Whirlwind) StartCoroutine(AttackAnim(NoobWhirlwind, new Vector3(200, 50, 0), 1));
    }
}
