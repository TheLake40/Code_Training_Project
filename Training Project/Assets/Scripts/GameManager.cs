using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

//An enum of all the possible GameStates (Many are Gameplay Modes!)
[Serializable]
public enum Gamestate
{
    Starting = 1,
    Playing = 10, 
    Paused = 15,
    FailScreen = 20,
    VictoryDance = 25
}


public class GameManager : Singleton<GameManager>
{
    public static event Action<Gamestate> OnBeforeStateChange;
    public static event Action<Gamestate> OnAfterStateChange;

    public Gamestate State { get; private set; }

    private Gamestate _previousState = Gamestate.Starting;

    void Start()
    {
         ChangeState(Gamestate.Starting);
    }

    public void ChangeState(Gamestate newState)
    {
        
        OnBeforeStateChange?.Invoke(newState);

        _previousState = State; //remember for unpausing

        State = newState;
        Debug.Log("changed game state to : " + newState);

        if(_previousState == Gamestate.Paused)
        {
            Time.timeScale = 1;
        }

        //this Game Manager can do high level manager stuff itself
        switch (newState)
        {
            case Gamestate.Starting:
                StartCoroutine(HandleStarting());
                break; 
                case Gamestate.Playing:
                break;
                case Gamestate.Paused:
                Time.timeScale = 0;
                break;
                case Gamestate.FailScreen:
                break;
                default:
                Debug.Log("GameState not handled: " + nameof(newState));
                //throw new ArgumentOutOfRangeException(nameof(newState),newState, null);
                break;
        }
        OnAfterStateChange?.Invoke(newState);
    }

    private IEnumerator HandleStarting()
    {
        //Play music here?
        yield return new WaitForSeconds(2);
        ChangeState(Gamestate.Playing);
    }

    public void TogglePause()
    {
        if (State == Gamestate.Paused)
        {
            ChangeState(_previousState);
        }
        else { ChangeState(Gamestate.Paused); }
    }

}
