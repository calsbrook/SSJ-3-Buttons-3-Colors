using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCollider : MonoBehaviour
{
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            GameObject thePlayer = GameObject.Find("Player");
            Player playerScript = thePlayer.GetComponent<Player>();
            playerScript.isAlive = false;
        }
            
    }
}
