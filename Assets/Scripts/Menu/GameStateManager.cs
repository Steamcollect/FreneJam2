using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public GameState gameState = GameState.Gameplay;

    public delegate void Gameplay();
    public static event Gameplay OnGameplay;

    public delegate void Paused();
    public static event Paused OnPaused;

    public void ResumeGameState()
    {
        gameState = GameState.Gameplay;
        ApplyGameState();
    }
    public void PauseGameState()
    {
        gameState = GameState.Paused;
        ApplyGameState();
    }

    public void ApplyGameState()
    {
        switch (gameState)
        {
            case GameState.Gameplay:
                if(OnGameplay != null) OnGameplay();
                break;
            case GameState.Paused:
                if(OnPaused != null) OnPaused();
                break;
        }
    }
}

public enum GameState
{
    Gameplay,
    Paused,
}