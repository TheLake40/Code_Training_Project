using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private int _score;

    // Start is called before the first frame update
    void Start()
    {
        _score = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log($"At Start {_score} enemies total.");
        
    }

    public void ReduceScore()
    {
        _score--;
        Debug.Log($"{_score} enemies total.");
        if (_score <= 0)
        {
            GameEventDispatcher.TriggerEnemiesAllDefeated();
        }
    }

    private void OnEnable()
    {
        GameEventDispatcher.EnemyDefeated += ReduceScore;
    }

    private void OnDisable()
    {
        GameEventDispatcher.EnemyDefeated -= ReduceScore;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
