using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float patrolDelay = 1.5f;
    [SerializeField] private float patrolSpeed = 3;

    private Rigidbody2D _rigidbody;
    private Vector2 _patrolTargetPosition;
    private WaypointPath _waypointPath;
    

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _waypointPath = GetComponentInChildren<WaypointPath>();
    }

    void Start()
    {
        StartCoroutine(PatrolCoroutine());
    }

    private void FixedUpdate()
    {
        if (!_waypointPath) return;

        Vector2 _direction = _patrolTargetPosition - (Vector2)transform.position;

        if (_direction.magnitude <= 0.1)
        {
            _patrolTargetPosition = _waypointPath.GetNextWaypointPosition();


        }

        if (GameManager.Instance.State == Gamestate.Playing)
        {
            //keep resetting the velocity to the
            //direction * speed even if nudged
            _rigidbody.velocity = _direction.normalized * patrolSpeed;
        }else
        {
            _rigidbody.velocity = Vector2.zero;
        }
       
    }

    public void AcceptDefeat()
    {
        Destroy(gameObject);
    }

    //IEnumerator return type for coroutine
    //that can yield for time and come back
     private IEnumerator PatrolCoroutine()
    {
        if (_waypointPath)
        {
            _patrolTargetPosition = _waypointPath.GetNextWaypointPosition();
        }
        else
        {
            //change the direction every second

            _rigidbody.velocity = new Vector2(1 , - 1);
                yield return new WaitForSeconds(patrolDelay);
            _rigidbody.velocity = new Vector2(-1, 1);
                yield return new WaitForSeconds(patrolDelay);
            
        }
        yield break;
    }

    private void OnEnable()
    {
        GameManager.OnAfterStateChange += HandleGameStateChange;
    }
    private void OnDisable()
    {
        GameManager.OnAfterStateChange -= HandleGameStateChange;
    }

    private void HandleGameStateChange(Gamestate state)
    {
        if (state == Gamestate.Starting)
        {
            GetComponent<SpriteRenderer>().color = Color.gray;
        }
        if (state == Gamestate.Playing)
        {
            GetComponent<SpriteRenderer>().color = Color.magenta;
        }
    }
}