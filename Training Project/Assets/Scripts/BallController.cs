using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _lifeTime = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDirection(Vector2 direction)
    {
        direction = direction.normalized;

        GetComponent<Rigidbody2D>().velocity = direction * _speed;

        Invoke(nameof(Vanish), _lifeTime);
    }

    private void Vanish()
    {
        Destroy(gameObject);
    }
}
