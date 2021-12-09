using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSoldier : Enemy
{
    private Rigidbody2D myRigidbody;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;
    public Animator anim;

    public SpriteRenderer spriteRenderer;
    public BoxCollider2D collider2D;
    public BoxCollider2D dmgCollider2D;
    public Rigidbody2D rb2D;
    public string targetMap;

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (World.activeMap != targetMap)
        {
            collider2D.enabled = false;
            rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
            spriteRenderer.enabled = false;
            dmgCollider2D.enabled = false;
        }
        else
        {
            collider2D.enabled = true;
            rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            spriteRenderer.enabled = true;
            dmgCollider2D.enabled = true;
        }
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        CheckDistance();
    }

    void CheckDistance() {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius ) {

            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

                changeAnim(temp - transform.position);
                myRigidbody.MovePosition(temp);
                
                ChangeState(EnemyState.walk);
            }
        }
    }

    private void SetAnimFloat(Vector2 setVector)
    {
        anim.SetFloat("moveX", setVector.x);
        anim.SetFloat("moveY", setVector.y);
    }

    private void changeAnim(Vector2 direction)
    {
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if(direction.x > 0)
            {
                SetAnimFloat(Vector2.right);
            }else if (direction.x < 0)
            {
                SetAnimFloat(Vector2.left);
            }
        }else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if(direction.y > 0)
            {
                SetAnimFloat(Vector2.up);
            }
            else if (direction.y < 0)
            {
                SetAnimFloat(Vector2.down);
            }
        }
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
            currentState = newState;
    }
}
