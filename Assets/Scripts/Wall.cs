﻿using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

    // public AudioClip chopSound1;
    // public AudioClip chopSound2;
    public Sprite dmgSprite;
    public int hp = 4;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public void DamageWall(int loss)
    {
        // SoundManager.instance.randomizeSfx(chopSound1, chopSound2);

        spriteRenderer.sprite = dmgSprite;

        hp -= loss;

        if (hp <= 0) gameObject.SetActive(false);
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}