using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MainMenu,
    InGame,
    InGameGuessing,
    InGamePaused,
    Credits
}

public class GameStateManager : MonoBehaviour
{
    #region Singleton
    private static GameStateManager _instance;
    public static GameStateManager Instance { get => _instance; }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public delegate void TimerUpdate(int time);
    public static event TimerUpdate OnTimerUpdate;

    private GameState _state = GameState.MainMenu;
    private int _time = 0;
    private Coroutine timerRoutine = null;
    public GameState State { get { return _state; } set { ChangeGameState(_state, value); } }

    public int Score { get; set; }
    public int Time { get => _time; private set { _time = value; OnTimerUpdate?.Invoke(value); } }

    public bool TimePaused { get; set; }

    public void ChangeGameState(GameState previous, GameState next)
    {
        switch (next)
        {
            case GameState.MainMenu:
                if (timerRoutine != null)
                {
                    StopCoroutine(timerRoutine);
                    timerRoutine = null;
                    Time = 0;
                }
                break;
            case GameState.InGame:
                if (timerRoutine == null)
                {
                    timerRoutine = StartCoroutine(UpdateGameTimer());
                }
                break;
            case GameState.InGameGuessing:
                StopCoroutine(timerRoutine);
                timerRoutine = null;
                break;
            case GameState.InGamePaused:
                StopCoroutine(timerRoutine);
                timerRoutine = null;
                break;
            case GameState.Credits:
                if (timerRoutine != null)
                {
                    StopCoroutine(timerRoutine);
                    timerRoutine = null;
                    Time = 0;
                }
                break;
        }

        _state = next;
    }

    public IEnumerator UpdateGameTimer()
    {
        while (true)
        {
            while (TimePaused) yield return null;
            yield return new WaitForSeconds(1);
            Time += 1;
        }
    }

    void Start()
    {
        State = GameState.InGame;
    }

    void Update()
    {
        
    }
}
