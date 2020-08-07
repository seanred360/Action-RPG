using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PlayerState
{
    walk,attack,interact
}

public class PlayerControllerOld : MonoBehaviour
{

    #region Singleton

    public static PlayerControllerOld instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    Camera cam;
    public Interactable focus;
    CharacterCombat playerCombat;
    Animator animator;
    public PlayerState currentState;
    Vector3 change;

    public LayerMask enemyLayers;
    Transform myAttackPoint;
    float attackRange = 1f;

    #region keyboard input stuff

    public CharacterController controller;
    public float movementSpeed = 6f;
    public float turnSmoothTime = .1f;
    float turnSmoothVelocity;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        animator = GetComponentInChildren<Animator>();
        currentState = PlayerState.walk;
        playerCombat = GetComponent<CharacterCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        #region keyboard input

        if (InputManager.instance.inputEnabled)
        {
            change = Vector3.zero;
            change.x = Input.GetAxisRaw("Horizontal");
            change.y = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(change.x, 0f, change.y).normalized;
            if (Input.GetKeyDown(KeyCode.Space) && currentState != PlayerState.attack)
            {
                StartCoroutine(Attack());
            }
            else if(currentState == PlayerState.walk)
            {
                if (direction.magnitude >= .1f)
                {
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);

                    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    MoveCharacter(moveDir);
                }
            }

            Vector3 forward = transform.TransformDirection(Vector3.forward) * 1f;
            Debug.DrawRay(transform.position, forward, Color.green);

            if (Input.GetKeyDown(KeyCode.F))
            {
                Ray ray = new Ray(transform.position, forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1f))
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    if (interactable != null)
                    {
                        SetFocus(interactable);
                    }
                }
            }
        }

        #endregion
    }

    IEnumerator Attack()
    {
        currentState = PlayerState.attack;

        animator.SetTrigger("attack");
        myAttackPoint = EquipmentManager.instance.attackPoint;
        Collider[] hitEnemies = Physics.OverlapSphere(EquipmentManager.instance.attackPoint.position, attackRange, enemyLayers);
        foreach(Collider enemy in hitEnemies)
        {
            Debug.Log("we hit" + enemy.name);
            playerCombat.DoDamage(enemy.GetComponent<CharacterStats>());
        }
        yield return new WaitForSeconds(1f);

        currentState = PlayerState.walk;
    }

    private void OnDrawGizmosSelected()
    {
        if (myAttackPoint == null)
            return;

        Gizmos.DrawWireSphere(myAttackPoint.position, attackRange);
    }

    void MoveCharacter(Vector3 moveDirection)
    {
        if (change != Vector3.zero)
        {
            controller.Move(moveDirection.normalized * movementSpeed * Time.deltaTime);
        }
    }


    void SetFocus(Interactable newFocus)
    {
        if(newFocus != focus)
        {
            if(focus != null)
                focus.OnDefocused();
            focus = newFocus;
        }
        newFocus.OnFocused(transform);
    }

    void RemoveFocus()
    {
        if (focus != null)
        {
            focus.OnDefocused();
        }
        focus = null;
    }
}
