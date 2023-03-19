using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType
    {
        Health,
        Life,
        Charge
    }

    public PickupType currentPickup;
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (currentPickup)
            {
                case PickupType.Health:
                    collision.gameObject.GetComponent<PlayerController>().StartJumpForceChange();
                    break;
                case PickupType.Life:
                    GameManager.instance.lives++;
                    break;
                case PickupType.Charge:
                    collision.gameObject.GetComponent<PlayerController>().StartSpeedChange();
                    break;
            }


            if (pickupSound)
                collision.gameObject.GetComponent<AudioSourceManager>().PlayOneShot(pickupSound, false);

            Destroy(gameObject);
        }
    }
}
