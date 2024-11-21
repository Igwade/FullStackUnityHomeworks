using System;
using UnityEngine;
using Zenject;

public class GameCycle : IGameCycle, IInitializable
{
    public event Action OnGameStart;
    public event Action<bool> OnGameOver;

    public void Initialize() 
        => StartGame();

    public void StartGame() 
        => OnGameStart?.Invoke();

    public void GameOver(bool win)
    {
        Time.timeScale = 0;
        OnGameOver?.Invoke(win);
    }
}