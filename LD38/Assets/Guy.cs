using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Guy { 

    public string Name { set; get; }
    public int TotalHealth { set; get; }
    
    public int Health { get; set; }
    public bool IsDead { get { return Health <= 0; } }

    public Attack[] Attacks { set; get; }
    Sprite GuySprite;

    SpriteRenderer GuySpriteRenderer;

    public Guy(string name, int health, Attack[] attacks, Sprite sprite) {
        Name = name;
        TotalHealth = health;
        Health = TotalHealth;
        Attacks = attacks;
        GuySpriteRenderer = new SpriteRenderer();
        SetSprite(sprite);
    }

    public Guy() {
        int levelGen = Random.Range(0, 20);
        int lvl = 0;
        if (levelGen > 4 && levelGen < 13) lvl = 1;
        if (levelGen > 12 && levelGen < 19) lvl = 2;
        if (levelGen > 19) lvl = 3;

        CustomGuy(lvl);
    }

    public Guy(int i) {
        CustomGuy(i);
    }

    void CustomGuy(int lvl) {
        string[] lvl1 = { "Betty", "Micky", "Chris", "Nate", };
        string[] lvl2 = { "Jane", "Michael", "Ellis", "Ray", "Rick", "Lola", "Rhonda" };
        string[] lvl3 = { "Lucky", "Fritz", "Lucas", "Samantha", "Emily", "Slick" };
        string[] lvl4 = { "a dick", "Red", "Ash", "Pikachu", "ffffff" };
        string[][] GuyNames = { lvl1, lvl2, lvl3, lvl4 };
        int[] Healths = { 20, 80, 140, 200 };

        Name = GuyNames[lvl][Random.Range(0, GuyNames[lvl].Length)];
        TotalHealth = Healths[lvl];
        Health = TotalHealth;

        List<Attack> attacks = new List<Attack>();
        attacks.AddRange(new Attack[] { Attack.Fireball, Attack.Icebeam, Attack.Rocksmash, Attack.Whirlwind });
        for (int i = 0; i < 3 - lvl; i++) {
            attacks.Remove(attacks[Random.Range(0, attacks.Count)]);
        }
        Attacks = attacks.ToArray();

        //Sprite sprite = Instantiate(Resources.Load<Sprite>("noob"));
        //GuySpriteRenderer = new SpriteRenderer();
        //SetSprite(sprite);
    }

    public void Reset() {
        Health = TotalHealth;
    }

    public void SetSprite(Sprite sprite) {
        //GuySprite = sprite;
        //GuySpriteRenderer.sprite = GuySprite;
    }
}
