using UnityEngine;
using System.Collections;

public enum Attack { Fireball, Icebeam, Whirlwind, Rocksmash, None }

public enum CombatState { NoobTurn, ChumpTurn, GameOver, Setup }

public class Combat : MonoBehaviour {

    Noob CurrentNoob;
    CombatState CurrentState;
    int Health;
    bool AmDead { get { return Health <= 0; } }

    bool AttackMade;

	// Use this for initialization
	void Start () {
        CurrentState = CombatState.Setup;
        Fight();
	}

    void Fight() {
        CurrentNoob = GameObject.Find("noob").GetComponent<Noob>();
        CurrentNoob.TakeTurn();
        Health = 200;
        AttackMade = false;
        CurrentState = CombatState.NoobTurn;
    }

    // Update is called once per frame
    void Update () {
	    if(CurrentState == CombatState.NoobTurn) {
            if(CurrentNoob.NextAttack != Attack.None) {
                int damage = (int)Random.Range(10, 20);
                Health -= damage;
                print("YOU take " + damage + " damage from " + CurrentNoob.NextAttack
                    + " (" + Health + " remaining)");
                if (AmDead) {
                    CurrentState = CombatState.GameOver;
                } else {
                    CurrentState = CombatState.ChumpTurn;
                    AttackMade = false;
                }
            }
        } else if(CurrentState == CombatState.ChumpTurn) {
            if (AttackMade) {
                if (CurrentNoob.IsDead) {
                    CurrentState = CombatState.GameOver;
                } else {
                    CurrentState = CombatState.NoobTurn;
                    CurrentNoob.TakeTurn();
                }
            }
        }
	}

    private void MakeAttack(Attack attack) {
        if (CurrentState == CombatState.ChumpTurn && !AttackMade) {
            int damage = (int)Random.Range(10, 20);
            CurrentNoob.Health -= damage;
            print("NOOB takes " + damage + " damage from " + attack
                + " (" + CurrentNoob.Health + " remaining)");
            if (CurrentNoob.IsDead) {
                CurrentState = CombatState.GameOver;
            }
            AttackMade = true;
        }
    }

    public void Fireball() { MakeAttack(Attack.Fireball); }
    public void Icebeam() { MakeAttack(Attack.Icebeam); }
    public void Whirlwind() { MakeAttack(Attack.Whirlwind); }
    public void Rocksmash() { MakeAttack(Attack.Rocksmash); }


}
