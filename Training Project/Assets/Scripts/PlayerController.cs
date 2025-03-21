using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //Don't miss this!

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;

    private PlayerInput _input; //field to reference Player Input component
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    
    private float SprintSpeed = 1.4f;
    private float tempSpeed;

    //add this to reference a prefab that is set in the inspector
    public GameObject ball;
    //...

    //NEW remember facing direction (even after stop)
    private Vector2 _facingVector = Vector2.right;

    private bool _isKnockback = false;

    void Start()
    {
        tempSpeed = speed;
        //set reference to PlayerInput component on this object
        //Top Action Map, "Player" should be active by default
        _input = GetComponent<PlayerInput>();
        //You can switch Action Maps using _input.SwitchCurrentActionMap("UI");

        //set reference to Rigidbody2D component on this object
        _rigidbody = GetComponent<Rigidbody2D>();

        //transform.position = new Vector2(3, -1);
        //Invoke(nameof(AcceptDefeat), 10);
        _animator = GetComponent<Animator>();
    }

    public void AcceptDefeat()
    {
        Destroy(gameObject);
    }

    public void Knockback(Vector2 directionVector) 
    {
        _rigidbody.AddForce(directionVector, ForceMode2D.Impulse);
        _isKnockback = true;
        Invoke(nameof(StopKnockback), .3f);
    }

    private void StopKnockback()
    {
        _isKnockback = false;
    }

    // Update is called once per frame
    void Update()
    {
        //BEGIN NEW CODE
        if (_input.actions["Pause"].WasPressedThisFrame())
        {
            GameManager.Instance.TogglePause();
        }
        //END NEW CODE

        if (GameManager.Instance.State != Gamestate.Playing) return;


        //fire
        
        

            if (GameManager.Instance.State == Gamestate.Playing)
        {
            
                //if Fire action was performed log it to the console
                if (_input.actions["Fire"].WasPressedThisFrame())
                {
                    Debug.Log("Fire activated!");

                    //create a new object that clones ball prefab
                    // at this objects position and rotation
                    //and use a new variable ballPrefab to refer to the clone
                    var ballPrefab = Instantiate(ball, transform.position, Quaternion.identity);

                    //*CHANGE* instead of changing rigidbody velocity: 
                    //call SetDirection from BallController on new ball
                    ballPrefab.GetComponent<BallController>()?.SetDirection(_facingVector);

                }

            if (_input.actions["Sprint"].WasPerformedThisFrame())
            {
                _animator.Play("PlayerSprintEnter");

                speed = SprintSpeed;

                if(_input.actions["Sprint"].WasReleasedThisFrame() == false)
                {
                    _animator.Play("PlayerSprintActive");
                }

            }
            if (_input.actions["Sprint"].WasReleasedThisFrame())
            {
                _animator.Play("PlayerSprintExit");

                speed = tempSpeed;

            }

            return;
    }   
    }

    private void FixedUpdate()
    {
        

        if (GameManager.Instance.State == Gamestate.Playing)
        {
            if (!_isKnockback)
            {
                //set direction to the Move action's Vector2 value
                var dir = _input.actions["Move"].ReadValue<Vector2>() * speed;

                


                    //change the velocity to match the Move (every physics update)
                    _rigidbody.velocity = dir * 5;

                if (dir.magnitude > .5)
                {
                    _facingVector = _rigidbody.velocity;
                }
                return;
            }
        }
    }
}


