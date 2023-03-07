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

            Destroy(gameObject);
        }
    }
}
