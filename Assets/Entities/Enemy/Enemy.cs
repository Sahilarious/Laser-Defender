﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    public AudioClip zap;
    public AudioClip destroyed;
    public GameObject laser;
    public GameObject particles;
    public float duration;
    public float health = 200f;
    public float fireRatePerSeconds = 0.5f;
    public int scoreValue;

    private Score score;
    private float alpha = 0;
    //private float time = 0;
    private bool firstUpdate;

	// Use this for initialization
	void Start () {
        //enemyAlpha(0f);
        //firstUpdate = true;

        score = FindObjectOfType<Score>();
        
    }
	
	// Update is called once per frame
	void Update () {
        //if (firstUpdate) {
        //    time = Time.time;
        //    firstUpdate = false;
        //}
        //enemyAlphaLerp();
        if (FindObjectOfType<PlayerLife>()) {
            float probability = fireRatePerSeconds * Time.deltaTime;
            if (probability > Random.value)
            {
                shootLaser();
            }
        }
    }

    //void enemyAlphaLerp() {
    //    //float lerp = Mathf.PingPong(Time.time, duration)/duration;

    //    alpha = Mathf.Lerp(0.0f, 1.0f, (Time.time - time)/duration);

    //    enemyAlpha(alpha);
    //}

    //void enemyAlpha(float alpha) {
    //    Color enemyColor = gameObject.GetComponent<SpriteRenderer>().material.color;
    //    enemyColor.a = alpha;
    //    gameObject.GetComponent<SpriteRenderer>().material.color = enemyColor;
    //}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLaser"))
        {
      
            AudioSource.PlayClipAtPoint(zap, Camera.main.transform.position, 0.8f);
            
            // obtains the damage value from the projectile and subtracts it from the enemy's health
            health -= collision.gameObject.GetComponent<PlayerLaser>().GetDamage();

            // destroys the playerLaser
            Destroy(collision.gameObject);

            // destroys the enemy object if its health has gone to 0 or below
            // also creates an explosion effect
            if(health <= 0)
            {
                AudioSource.PlayClipAtPoint(destroyed, Camera.main.transform.position);
                Destroy(gameObject, destroyed.length/2);
                GameObject explosion = Instantiate(particles, transform.position, Quaternion.identity) as GameObject;
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration * 2);
                score.changeScore(scoreValue); 
            }
        }
    }

    void shootLaser() {
        GameObject enemyLaser = Instantiate(laser, gameObject.transform.position - new Vector3(0f, 0.5f, 0f), Quaternion.identity) as GameObject;
    }

}
