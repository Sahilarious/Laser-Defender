using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUpManager : MonoBehaviour {
    public PlayerController player;
    public GameObject[] shieldsPowerUp;
    public GameObject[] shields;

    private Dictionary<string, int> shieldsPowerUpDict = new Dictionary<string, int>();

    // Use this for initialization
    void Start ()
    {
        player = FindObjectOfType<PlayerController>();
        CreateShieldTypeIndexDictionary();

    }

    void CreateShieldTypeIndexDictionary()
    {
        for (int i = 0; i < shieldsPowerUp.Length; i++)
        {
            shieldsPowerUpDict.Add(shieldsPowerUp[i].GetComponents<ShieldPowerUpBase>()[0].GetType().ToString(), i);
        }
    }
	
    public void GenerateShield(GameObject shieldPowerUpBase)
    {
        string shieldType = shieldPowerUpBase.GetComponents<ShieldPowerUpBase>()[0].GetType().ToString();
        GameObject shipShield = Instantiate(shields[shieldsPowerUpDict[shieldType]], 
                                            player.transform.position, 
                                            Quaternion.identity) as GameObject;
        shipShield.transform.parent = player.transform;

        Destroy(shieldPowerUpBase.gameObject);
    }
}
