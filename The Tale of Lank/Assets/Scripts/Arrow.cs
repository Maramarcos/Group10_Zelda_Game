using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;
    public Rigidbody2D hitbox;
    public float lifetime;
    private float lifetimeCounter;
    // Start is called before the first frame update
    void Start()
    {
        lifetimeCounter = lifetime;
    }

    private void Update()
    {
        lifetimeCounter -= Time.deltaTime;
        if (lifetimeCounter <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Setup(Vector2 velocity, Vector3 direction)
    {
        hitbox.velocity = velocity.normalized * speed;
        transform.rotation = Quaternion.Euler(direction);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("EditorCollider") && !other.gameObject.CompareTag("Water") && !other.gameObject.CompareTag("AllowArrow"))
        {
            Destroy(this.gameObject);
        }
    }
}
