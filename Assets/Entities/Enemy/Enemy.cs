using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public GameObject laser;
    public GameObject particles;
    public float duration;

    private int frame = 0;
    private float alpha = 0;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        enemyAlpha();

        frame++;

        if (frame == 5) {
            shootLaser();
            frame = 0;
        }

    }

    void enemyAlpha() {
        //float lerp = Mathf.PingPong(Time.time, duration)/duration;

        alpha = Mathf.Lerp(0.0f, 1.0f, Time.time/duration);
        Color enemyColor = gameObject.GetComponent<SpriteRenderer>().material.color;
        enemyColor.a = alpha;
        gameObject.GetComponent<SpriteRenderer>().material.color = enemyColor;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLaser"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            GameObject explosion = Instantiate(particles, transform.position, Quaternion.identity) as GameObject;
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration * 2);
        }
      
    }

    void shootLaser() {


        if (gameObject.transform.position.x >= PlayerController.playerPosition.x - 0.5f && gameObject.transform.position.x <= PlayerController.playerPosition.x + 0.5f)
        {
           firing();
        }
    }

    void firing() {
        GameObject enemyLaser = Instantiate(laser, gameObject.transform.position - new Vector3(0f, 0.5f, 0f), Quaternion.identity) as GameObject;
    }
}
