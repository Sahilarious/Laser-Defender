using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioClip zapAudio;
    public AudioClip destroyedAudio;
    public float velocityModifier;
    public Sprite[] playerSprites;
    public GameObject laser;
    public GameObject particles;
    public float firingRate = 0.3f;
    public GameObject rightThruster;
    public GameObject leftThruster;

    public int initialLives = 2;
    public static int currentLives;

    private GameObject rThrust;
    private GameObject lThrust;
    private Vector3 playerVelocity = Vector3.zero;    
    private float xMax;
    private float xMin;

    // Use this for initialization
    void Start ()
    {
        currentLives = initialLives;

        SetShipMovementMinMax();
    }

    // Update is called once per frame
    void Update ()
    {
        UpdatePlayerVelocity();

        SetPositionLimit();

        UpdateShipSprite();

        UpdateThrusters();

        ShootLaser();


        if (!FindObjectOfType<ShieldBase>())
        {
            // changes the ship's layer back to Friendlies so that enemy lasers will interact with 
            // the player ship's collider
            gameObject.layer = 8;
        }

        if(!gameObject.GetComponent<Renderer>().enabled)
        {
            DestroyThrusters();
        }

    }

    void SetShipMovementMinMax()
    {
        float shipBound = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.max.x;

        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));

        xMax = rightMost.x - shipBound;
        xMin = leftMost.x + shipBound;
    }


    void UpdatePlayerVelocity()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(-velocityModifier * Time.deltaTime, 0f);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(velocityModifier * Time.deltaTime, 0f);
        }
    }

    void SetPositionLimit()
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

    void UpdateShipSprite()
    {
        Sprite updatedSprite;

        if (playerVelocity.x < 0)
        {
            updatedSprite = playerSprites[playerSprites.Length - 1];
        }
        else if (playerVelocity.x > 0)
        {
            updatedSprite = playerSprites[playerSprites.Length - 2];
        }
        else
        {
            updatedSprite = playerSprites[0];
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = updatedSprite;
    }

    void UpdateThrusters()
    {
        // LEFT KEY gets pressed down ->  the right thruster turns on, applying a force on the ship from the right, hence pushing the ship to the left
        // RIGHT KEY gets pressed down -> the left thruster turns on, applying a force on the ship from the left, hence pushing the ship to the right
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !rThrust)
        {
            InstantiateRightThruster();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !lThrust)
        {
            InstantiateLeftThruster();
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) && rThrust)
        {
            Destroy(rThrust);

        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) && lThrust)
        {
            Destroy(lThrust);
        }
    }

    void InstantiateLeftThruster()
    {
        lThrust = Instantiate(leftThruster, gameObject.transform.position + new Vector3(-0.3f, -0.26f, 10f), Quaternion.Euler(83.324f, -90f, -90f)) as GameObject;
        lThrust.transform.parent = gameObject.transform;
    }

    void InstantiateRightThruster()
    {
        rThrust = Instantiate(rightThruster, gameObject.transform.position + new Vector3(0.2f, -0.26f, 10f), Quaternion.Euler(95, -90, -90)) as GameObject;
        rThrust.transform.parent = gameObject.transform;
    }

    void ShootLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("Firing", 0.0000001f, firingRate);
        }
        else if (Input.GetKeyUp(KeyCode.Space)) {
            CancelInvoke("Firing");
        }
    }

    void Firing()
    {
        GameObject playerLaser = Instantiate(laser, gameObject.transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.identity) as GameObject;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Component[] shieldComponents = collider.gameObject.GetComponents(typeof(ShieldPowerUpBase));

        if (collider.gameObject.CompareTag("EnemyLaser"))
        {
            PlayZapAudio();
            LoseLives(collider.GetComponent<EnemyLaser>().GetDamage());
            
            if (gameObject && currentLives <= 0) {
                PlayDestroyedAudio();
                CreateExplosion();
                gameObject.GetComponent<Renderer>().enabled = false;
                Destroy(gameObject, destroyedAudio.length / 2);
            }
            Destroy(collider.gameObject);
        } else if (shieldComponents.Length > 0) {
            GeneratePlayerShield(collider);
        }
    }

    void PlayZapAudio()
    {
        AudioSource.PlayClipAtPoint(zapAudio, Camera.main.transform.position);
    }

    void PlayDestroyedAudio()
    {
        AudioSource.PlayClipAtPoint(destroyedAudio, Camera.main.transform.position);
    }

    void CreateExplosion()
    {
        GameObject explosion = Instantiate(particles, transform.position, Quaternion.identity) as GameObject;
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration * 2);
    }

    void LoseLives(int livesLost)
    {
        if (livesLost > currentLives)
        {
            livesLost = currentLives;
        }
        currentLives -= livesLost;
        for (int i = 0; i < livesLost; i++)
        {
            PlayerLife playerLife = FindObjectOfType<PlayerLife>();
            if (playerLife)
            {
                Destroy(playerLife.gameObject);
            }
        }
    }

    void GeneratePlayerShield(Collider2D collider)
    {
        var playerPowerUpManager = FindObjectOfType<PlayerPowerUpManager>();
        playerPowerUpManager.GenerateShield(collider.gameObject);
        gameObject.layer = 12;
    }

    public int GetInitialLives()
    {
        return initialLives;
    }

    void DestroyThrusters()
    {
        if (rThrust)
        {
            Destroy(rThrust);
        }

        if (lThrust)
        {
            Destroy(lThrust);
        }
    }
}
