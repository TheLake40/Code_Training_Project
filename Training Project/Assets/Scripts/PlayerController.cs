using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //Don't miss this!

public class PlayerController : MonoBehaviour
{
    private PlayerInput _input; //field to reference Player Input component
    private Rigidbody2D _rigidbody;

    //add this to reference a prefab that is set in the inspector
    public GameObject ball;
    //...

    //NEW remember facing direction (even after stop)
    private Vector2 _facingVector = Vector2.right;

    // Start is called before the first frame update
    void Start()
    {
        //set reference to PlayerInput component on this object
        //Top Action Map, "Player" should be active by default
        _input = GetComponent<PlayerInput>();
        //You can switch Action Maps using _input.SwitchCurrentActionMap("UI");

        //set reference to Rigidbody2D component on this object
        _rigidbody = GetComponent<Rigidbody2D>();

        //transform.position = new Vector2(3, -1);
        //Invoke(nameof(AcceptDefeat), 10);
    }

    void AcceptDefeat()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
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
    }

    private void FixedUpdate()
    {
        //set direction to the Move action's Vector2 value
        var dir = _input.actions["Move"].ReadValue<Vector2>();
        

        //change the velocity to match the Move (every physics update)
        _rigidbody.velocity = dir * 5;

        if (dir.magnitude > .5)
        {
            _facingVector = _rigidbody.velocity;
        }
    }
}


