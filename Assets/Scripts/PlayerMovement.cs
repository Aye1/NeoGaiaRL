using Assets.Scripts.Helpers;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public float speedForce = 5.0f;
    public Vector2 maxSpeed = new Vector2(1.0f, 0.0f);
    public Vector2 maxSpeedInAir = new Vector2(1.0f, 5.0f);
    //private
    private bool isCrouching = false;
    private bool isGrounded = true;
    [HideInInspector] private float lastDirectionX = 0.0f;

    [Header("Jump")]
    public float jumpForce = 750.0f;
    public int initialMaxJumpsAvailable = 1;
    public float gravityUp = 1.0f;
    public float gravityDown = 2.5f;
    public float threshold = 0.5f;
    //private
    private int maxJumpsAvailable = 1;
    private int jumpsAvailable = 1;
    private bool isJumping = false;
    private bool isGrinding = false;
    private Vector3 grindingDirection = Vector3.zero;


    private IInteractable _currentInteractable = null;

    private Rigidbody2D _body;
    private Player _player;
    private SpriteRenderer _playerSprite;

    //Getter and Setter
    public bool IsCrouching()
    {
        return isCrouching;
    }
    public float GetLatestDirectionX()
    {
        return lastDirectionX;
    }

    // Use this for initialization
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();
        _playerSprite = GetComponent<SpriteRenderer>();
        ObjectChecker.CheckNullity(_body, "RigidBody2D not found for Player");
        ObjectChecker.CheckNullity(_player, "Player not found");
    }

    // Update is called once per frame
    void Update()
    {
        ManageInputs();
        UpdateJumpsAvailable();
    }

    private void UpdateJumpsAvailable()
    {
        maxJumpsAvailable = _player.has10Jumps ? 10 : initialMaxJumpsAvailable;
    }

    private void ManageInputs()
    {
        if (_player.canMove)
        {
            //ManageMove();
            if (!isGrinding)
            {
                ManageMoveWithForces();
            } else
            {
                ManageGrindingMovement();
            }
            //ManageCrouch();
            ManageJump();
            //ManageInteraction();
            LimitVelocity();
            ChangeGravity();
        } else
        {
            _body.velocity = Vector3.zero;
            _body.gravityScale = gravityDown;
        }
    }

    private void ManageMoveWithForces()
    {
        float dirX = 0.0f;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //_body.velocity = new Vector2(actualSpeed, _body.velocity.y);
            _body.AddForce(new Vector2(speedForce, 0.0f), ForceMode2D.Impulse);
            dirX = 1.0f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            //_body.velocity = new Vector2(-actualSpeed, _body.velocity.y);
            _body.AddForce(new Vector2(-speedForce, 0.0f), ForceMode2D.Impulse);
            dirX = -1.0f;
        }
        else
        {
            _body.velocity = new Vector2(0.0f, _body.velocity.y);
        }
        //lastDirectionX = dirX;
    }

    
    private void ManageJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpsAvailable > 0)
        {
            _body.velocity = new Vector2(_body.velocity.x, 0.0f);
            _body.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
            jumpsAvailable--;
        }
    }

    public void LimitVelocity()
    {
        Vector2 actualMaxSpeed = isGrounded ? maxSpeed : maxSpeedInAir;
        float velocityX = _body.velocity.x;
        float velocityY = _body.velocity.y;

        if (Mathf.Abs(_body.velocity.y) > actualMaxSpeed.y)
        {
            float sign = Mathf.Abs(_body.velocity.y) / _body.velocity.y;
            velocityY = sign * actualMaxSpeed.y;
        }
        if (Mathf.Abs(_body.velocity.x) > actualMaxSpeed.x)
        {
            float sign = Mathf.Abs(_body.velocity.x) / _body.velocity.x;
            velocityX = sign * actualMaxSpeed.x;
        }
        _body.velocity = new Vector2(velocityX, velocityY);

    }

    private void ChangeGravity()
    {
        if (isGrinding)
        {
            _body.gravityScale = 0.0f;
        }
        else if (_body.velocity.y > threshold)
        {
            _body.gravityScale = gravityUp;
        }
        else if (_body.velocity.y < threshold)
        {
            _body.gravityScale = gravityDown;
        }
        else
        {
            _body.gravityScale = 1.0f;
        }
    }

    private void ManageCrouch()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }
    }

    private void ManageInteraction()
    {
        // Can interact
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interact button pushed");
            if (_currentInteractable != null)
            {
                Debug.Log("Interacting");
                _currentInteractable.OnInteract(_player);
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Collision");
        isGrounded = true;
        jumpsAvailable = maxJumpsAvailable;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<IInteractable>() != null)
        {
            Debug.Log("Can interact");
            _currentInteractable = col.gameObject.GetComponent<IInteractable>();
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        IInteractable interactable = col.gameObject.GetComponent<IInteractable>();
        if (interactable != null && interactable == _currentInteractable)
        {
            Debug.Log("Can't interact anymore");
            _currentInteractable = null;
        }
    }

    public void StartGrinding(float height)
    {
        Debug.Log("Start grinding");
        Vector3 debugdir = new Vector3(1.0f, 0.0f, 0.0f);
        ChangeGrindingDirection(debugdir);
        isGrinding = true;
        _playerSprite.color = Color.green;
        ForceGrindingHeight(height);
    }

    public void StopGrinding()
    {
        Debug.Log("Stop grinding");
        grindingDirection = Vector3.zero;
        isGrinding = false;
        _playerSprite.color = Color.white;
    }

    public void ChangeGrindingDirection(Vector3 dir)
    {
        grindingDirection = dir;
        CancelVerticalSpeedIfNeeded();
    }

    private void CancelVerticalSpeedIfNeeded()
    {
        if (grindingDirection.y == 0.0f)
        {
            _body.velocity = new Vector3(_body.velocity.x, 0.0f, 0.0f);
        }
    }

    private void ForceGrindingHeight(float height)
    {
        _body.velocity = new Vector3(_body.velocity.x, 0.0f, 0.0f);
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
    }

    private void ManageGrindingMovement()
    {
        _body.AddForce(grindingDirection, ForceMode2D.Impulse);
    }

}
