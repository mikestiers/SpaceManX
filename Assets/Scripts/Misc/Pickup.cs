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
            PlayerController temp = collision.gameObject.GetComponent<PlayerController>();

            switch (currentPickup)
            {
                case PickupType.Health:
                    temp.StartJumpForceChange();
                    break;
                case PickupType.Life:
                    temp.lives++;
                    break;
                case PickupType.Charge:
                    temp.StartSpeedChange();
                    break;
            }

            Destroy(gameObject);
        }
    }
}
