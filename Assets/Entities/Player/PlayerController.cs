using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float velocityModifier;
    public Sprite[] playerSprites;
    public GameObject laser;
    public GameObject particles;
    public float firingRate = 0.3f;
    public float health;

    public static Vector3 playerPosition;
    public int initialLives = 2;
    public static int currentLives;

    private Vector3 playerVelocity;    
    private float xMax;
    private float xMin;

    // Use this for initialization
    void Start () {
        currentLives = initialLives;
        playerPosition = transform.position;
        playerVelocity = new Vector3(0f,0f,0f);
        velocityModifier = velocityModifier * Time.deltaTime;
        float shipBound = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.max.x;

        float distance = transform.position.z - Camera.main.transform.position.z; 
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1,0, distance));

        xMax = rightMost.x - shipBound;
        xMin = leftMost.x + shipBound;
    }

    // Update is called once per frame
    void Update () {
        playerVelocityChange();

        changePlayerPosition();

        shipSprite();
        
        shootLaser();

        playerPosition = transform.position;
    }

    // update ship velocity

    void playerVelocityChange() {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerVelocity += new Vector3(-velocityModifier, 0f, 0f);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            playerVelocity += new Vector3(velocityModifier, 0f, 0f);
        }
    }

    // update ship position
    void changePlayerPosition()
    {
        float xPos = Mathf.Clamp(gameObject.GetComponent<Transform>().position.x + playerVelocity.x, xMin, xMax);

        if (xPos <= xMin || xPos >= xMax)
        {
            playerVelocity = new Vector3(0f, 0f, 0f);
        }

        gameObject.GetComponent<Transform>().position = new Vector3(xPos, gameObject.GetComponent<Transform>().position.y, 0f);
    }

    void shipSprite()
    {
        if (playerVelocity.x < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = playerSprites[playerSprites.Length - 1];
        }
        else if (playerVelocity.x > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = playerSprites[playerSprites.Length - 2];
        }
        else
        {
            shipSpriteDamage();
        }
    }

    void shipSpriteDamage() {
        if (currentLives == initialLives)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = playerSprites[0];
        }
        else {
            gameObject.GetComponent<SpriteRenderer>().sprite = playerSprites[1];
        }
    }

    void shootLaser() {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("firing", 0.0000001f, firingRate);

        }
        else if (Input.GetKeyUp(KeyCode.Space)) {
            CancelInvoke("firing");
        }
    }

    void firing() {
        GameObject playerLaser = Instantiate(laser, gameObject.transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.identity) as GameObject;
    }

     void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyLaser"))
        {
            health -= collision.gameObject.GetComponent<EnemyLaser>().getDamage(); 
            if (health <= 0) {
                GameObject explosion = Instantiate(particles, transform.position, Quaternion.identity) as GameObject;
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration * 2);
                Destroy(gameObject);
            }

            // destroys the enemy laser object
            Destroy(collision.gameObject);
        }
    }
}
