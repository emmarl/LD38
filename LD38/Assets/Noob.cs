using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Noob : MonoBehaviour {

    private int _health;
    public int Health { get { return _health; } set { _health = value; UpdateHealthBar(); } }
    public bool IsDead { get { return Health <= 0; } }
    List<Attack> Attacks;
    public Attack NextAttack;
    int NoobLevel;

    void Start() {
        int levelGen = Random.Range(0, 10);
        if(levelGen < 3) {
            NoobLevel = 1;
            Health = 20;
        } else if(levelGen < 9) {
            NoobLevel = 2;
            Health = 80;
        } else {
            NoobLevel = 3;
            Health = 140;
        }
        Attacks = new List<Attack>();
        Attacks.AddRange(new Attack[] {Attack.Fireball, Attack.Icebeam, Attack.Rocksmash, Attack.Whirlwind });
        for(int i = 0; i < 4 - NoobLevel; i++) {
            Attacks.Remove(Attacks[Random.Range(0, Attacks.Count)]);
        }
        print("NoobLevel: " + NoobLevel + ", Health: " + Health
            + ", Attacks: " + AttacksToString());
        NextAttack = Attack.None;
    }
    
    string AttacksToString() {
        string s = "";
        foreach (Attack a in Attacks) {
            s += a + ", ";
        }
        return s;
    }

    public void TakeTurn() {
        NextAttack = Attack.None;
        StartCoroutine(Turn());
    }

    IEnumerator Turn() {
        yield return new WaitForSeconds(3);
        NextAttack = Attacks[Random.Range(0, Attacks.Count)];
    }

    void UpdateHealthBar() {

    }
}
