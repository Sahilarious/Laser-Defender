using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLives : MonoBehaviour {
    public GameObject playerShip;
    public GameObject life;
    public Canvas loseScreen;
    public float xPos = 15.5f;
    public float yPos = 11.5f;

    // Use this for initialization
    void Start () {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            loseScreen.GetComponent<Canvas>().enabled = false;
            for (int i = 0; i < playerShip.GetComponent<PlayerController>().health / 100; i++)
            {
                Instantiate(life, new Vector3(xPos - i, yPos, 0), Quaternion.identity);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!FindObjectOfType<PlayerLife>() && SceneManager.GetActiveScene().name == "Game")
        {
            loseScreen.GetComponent<Canvas>().enabled = true;
        }
    }
}
