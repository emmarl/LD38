using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Noob : MonoBehaviour {

    private int _health;
    public int Health { get { return _health; } set { _health = value; UpdateHealthBar(); } }
    public bool IsDead { get { return Health <= 0; } }
    List<Attack> Attacks;
    public Attack NextAttack;
    int NoobLevel;

    void Start() {
        
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
