﻿using UnityEngine;
using System;
using System.Collections;

public class Enemy : MovingObject {

    public int playerDamage;

    public AudioClip enemyAttack1;
    public AudioClip enemyAttack2;

    private int enemyAttackID;

    private Animator animator;
    private Transform target;
    private bool skipMove;
    

	// Use this for initialization
	protected override void Start () {

        enemyAttackID = Animator.StringToHash("enemyAttack");

        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }

    public void MoveEnemy ()
    {
        int xDir = 0, yDir = 0;

        if(Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            yDir = target.position.y > transform.position.y ? 1 : -1;

        } else
        {
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;

        animator.SetTrigger(enemyAttackID);
        SoundManager.instance.RandomizeSoundEffects(enemyAttack1, enemyAttack2);
        hitPlayer.LoseFood(playerDamage);
    }
}
