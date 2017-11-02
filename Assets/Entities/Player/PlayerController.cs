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
    public GameObject rightThruster;
    public GameObject leftThruster;

    //public static Vector3 playerPosition;
    public int initialLives = 2;
    public static int currentLives;

    private GameObject rThrust;
    private GameObject lThrust;
    private Vector3 playerVelocity;    
    private float xMax;
    private float xMin;

    // Use this for initialization
    void Start () {
        currentLives = initialLives;

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

        positionLimit();

        shipSprite();
        
        shootLaser();

        thrusters();
    }

    // update ship velocity

    void playerVelocityChange() {
        // LEFT KEY pressed -> velocity increases along NEGATIVE x-axis
        // RIGHT KEY pressed -> velocity increases along the POSITIVE x-axis 
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(-velocityModifier, 0f);
            //playerVelocity += new Vector3(-velocityModifier, 0f, 0f);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(velocityModifier, 0f);
            //playerVelocity += new Vector3(velocityModifier, 0f, 0f);
        }
    }

    void thrusters()
    {
        // LEFT KEY gets pressed down ->  the right thruster turns on, applying a force on the ship from the right, hence pushing the ship to the left
        // RIGHT KEY gets pressed down -> the left thruster turns on, applying a force on the ship from the left, hence pushing the ship to the right
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!rThrust) {
                rThrust = Instantiate(rightThruster, gameObject.transform.position + new Vector3(0.2f, -0.26f, 10f), Quaternion.Euler(95, -90, -90)) as GameObject;
                rThrust.transform.parent = gameObject.transform;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!lThrust)
            {
                lThrust = Instantiate(leftThruster, gameObject.transform.position + new Vector3(-0.3f, -0.26f, 10f), Quaternion.Euler(83.324f, -90f, -90f)) as GameObject;
                lThrust.transform.parent = gameObject.transform;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Destroy(rThrust);

        }
        else if (Input.GetKeyUp(KeyCode.RightArrow)) {
            Destroy(lThrust);
        }
    }

    // update ship position
    void positionLimit()
    {
        float xPos = Mathf.Clamp(gameObject.GetComponent<Transform>().position.x, xMin, xMax);

        if (gameObject.GetComponent<Transform>().position.x < xMin)
        {
            gameObject.transform.position = new Vector3(xMin, gameObject.GetComponent<Transform>().position.y, 0f);
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
        } else if (gameObject.GetComponent<Transform>().position.x > xMax) {
            gameObject.transform.position = new Vector3(xMax, gameObject.GetComponent<Transform>().position.y, 0f);
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }
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
