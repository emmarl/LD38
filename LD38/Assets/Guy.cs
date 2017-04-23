using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Guy { 

    public string Name { set; get; }
    public int TotalHealth { set; get; }
    
    public int Health { get; set; }
    public bool IsDead { get { return Health <= 0; } }

    public Attack[] Attacks { set; get; }
    public Sprite GuySprite;
    public int lvl;

    public Guy(string name, int health, Attack[] attacks, Sprite sprite) {
        Name = name;
        TotalHealth = health;
        Health = TotalHealth;
        Attacks = attacks;
        GuySprite = sprite;
    }

    public Guy(Sprite sprite) {
        int levelGen = Random.Range(0, 20);
        lvl = 0;
        if (levelGen > 4 && levelGen < 6) lvl = 1;
        if (levelGen > 5 && levelGen < 14) lvl = 2;
        if (levelGen > 13) lvl = 3;

        CustomGuy(lvl, sprite);
    }

    public Guy(int i, Sprite sprite) {
        CustomGuy(i, sprite);
    }

    void CustomGuy(int l, Sprite sprite) {
        string[] lvl1 = { "Betty", "Micky", "Chris", "Nate", };
        string[] lvl2 = { "Jane", "Michael", "Ellis", "Ray", "Rick", "Lola", "Rhonda" };
        string[] lvl3 = { "Lucky", "Fritz", "Lucas", "Samantha", "Emily", "Slick" };
        string[] lvl4 = { "a dick", "Red", "Ash", "Pikachu", "ffffff" };
        string[][] GuyNames = { lvl1, lvl2, lvl3, lvl4 };
        int[] Healths = { 20, 40, 60, 80 };

        lvl = l;

        Name = GuyNames[lvl][Random.Range(0, GuyNames[lvl].Length)];
        TotalHealth = Healths[lvl];
        Health = TotalHealth;

        List<Attack> attacks = new List<Attack>();
        attacks.AddRange(new Attack[] { Attack.Fireball, Attack.Icebeam, Attack.Rocksmash, Attack.Whirlwind });
        for (int i = 0; i < 3 - lvl; i++) {
            attacks.Remove(attacks[Random.Range(0, attacks.Count)]);
        }
        Attacks = attacks.ToArray();

        GuySprite = sprite;
    }

    public void Reset() {
        Health = TotalHealth;
    }
}
