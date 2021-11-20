using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickUpSFX;
    [SerializeField] int scoreValue = 100;

    bool wasCollected = false;

    GameSession gameSession;

    void Awake()
    {
        gameSession = FindObjectOfType<GameSession>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            AudioSource.PlayClipAtPoint(coinPickUpSFX, Camera.main.transform.position);
            gameSession.AddToScore(scoreValue);
            // gameObject.SetActive(false);
            // the line above is in case the score is added twice, shouldn't be needed here
            Destroy(gameObject);
        }
    }
}
