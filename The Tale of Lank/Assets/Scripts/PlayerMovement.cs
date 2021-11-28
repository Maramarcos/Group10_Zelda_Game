
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement player;
    public PlayerState currentState;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    public FloatValue currentHealth;
    public Signal playerHealthSignal;
    public VectorValue startingPosition;

    // Start is called before the first frame update
    // Use this for initialization
    void Start()
    {
        Application.targetFrameRate = 60; // MUST SET FRAMERATE, FPS AFFFECTS MOTION, HIGHER FPS = SLOWER MOVEMENT
        currentState = PlayerState.walk;
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator.SetFloat("MoveX", 0);
        animator.SetFloat("MoveY", -1);
        player = this;

        transform.position = startingPosition.initialValue;
    }

    // Update is called once per frame
@@ -39,21 +39,18 @@ void Update()
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack && currentState != PlayerState.stagger)
        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack
           && currentState != PlayerState.stagger)
        {
            StartCoroutine(AttackCommand());
            StartCoroutine(AttackCo());
        }

        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAnimationAndMove();
        }

        //TODO: Interact
    }

    private IEnumerator AttackCommand()
    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
@@ -65,14 +62,14 @@ private IEnumerator AttackCommand()

    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero) // Input to move detected.
        if (change != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("MoveX", change.x);
            animator.SetFloat("MoveY", change.y);
            animator.SetBool("moving", true);
        }
        else //Not moving
        else
        {
            animator.SetBool("moving", false);
        }
@@ -81,22 +78,24 @@ void UpdateAnimationAndMove()
    void MoveCharacter()
    {
        change.Normalize();
        myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
        myRigidbody.MovePosition(
            transform.position + speed * Time.deltaTime * change
        );
    }

    public void WarpPlayer(string mapName, float x, float y)
    public void Knock(float knockTime, float damage)
    {
        if (mapName != World.activeMap)
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue > 0)
        {
            World.SetActiveMap(mapName);
        }

        this.gameObject.transform.position = new Vector3(x, y, 0);
    }

    public void Knock(float knockTime)
    {
        StartCoroutine(KnockCo(knockTime));
            StartCoroutine(KnockCo(knockTime));
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator KnockCo(float knockTime)
@@ -106,7 +105,7 @@ private IEnumerator KnockCo(float knockTime)
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;

            myRigidbody.velocity = Vector2.zero;
        }
    }
}