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
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    private GameState _state = GameState.MainMenu;
    public GameState State { get { return _state; } set { ChangeGameState(_state, value); } }

    public int Score { get; set; }

    public void ChangeGameState(GameState previous, GameState next)
    {

        _state = next;
    }

    void Start()
    {
        State = GameState.InGame;
    }

    void Update()
    {
        
    }
}
