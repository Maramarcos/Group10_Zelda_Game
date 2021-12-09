using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedHeart : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            collider.GetComponent<PlayerMovement>().currentHealth.RuntimeValue += 1;
            if (collider.GetComponent<PlayerMovement>().currentHealth.RuntimeValue > 6)
            {
                collider.GetComponent<PlayerMovement>().currentHealth.RuntimeValue = 6;
            }
            collider.GetComponent<PlayerMovement>().playerHealthSignal.Raise();
        }
    }
}
