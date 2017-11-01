using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public GameObject laser;
    public GameObject particles;
    public float duration;
    public float health = 200f;
    public float fireRatePerSeconds = 0.5f;

    private float alpha = 0;
    private float time = 0;
    private bool firstUpdate;

	// Use this for initialization
	void Start () {       
        enemyAlpha(0f);
        firstUpdate = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (firstUpdate == true) {
            time = Time.time;
            firstUpdate = false;
        }
        enemyAlphaLerp();

        float probability =  fireRatePerSeconds * Time.deltaTime;

        if (probability > Random.value) {
            shootLaser();
        }
    }

    void enemyAlphaLerp() {
        //float lerp = Mathf.PingPong(Time.time, duration)/duration;

        alpha = Mathf.Lerp(0.0f, 1.0f, (Time.time - time)/duration);

        enemyAlpha(alpha);
    }

    void enemyAlpha(float alpha) {
        Color enemyColor = gameObject.GetComponent<SpriteRenderer>().material.color;
        enemyColor.a = alpha;
        gameObject.GetComponent<SpriteRenderer>().material.color = enemyColor;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLaser"))
        {
            // obtains the damage value from the projectile and subtracts it from the enemy's health
            health -= collision.gameObject.GetComponent<PlayerLaser>().getDamage();

            // destroys the playerLaser
            Destroy(collision.gameObject);

            // destroys the enemy object if its health has gone to 0 or below
            if(health <= 0)
            {
                Destroy(gameObject);
                GameObject explosion = Instantiate(particles, transform.position, Quaternion.identity) as GameObject;
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration * 2);
            }
        }
    }

    void shootLaser() {
        GameObject enemyLaser = Instantiate(laser, gameObject.transform.position - new Vector3(0f, 0.5f, 0f), Quaternion.identity) as GameObject;
    }

    public void resetTime()
    {
        time = Time.time;
    }

}
