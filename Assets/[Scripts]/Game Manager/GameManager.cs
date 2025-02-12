using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    int LETTER_INTERVAL = 1;
    [SerializeField]
    int LEVELUP_INTERVAL = 1;
    [SerializeField]
    private PlayAreaController playArea;
    [SerializeField]
    private TimerData timerData;
    [SerializeField]
    int minute;
    [SerializeField]
    float second;
    [SerializeField]
    private TimerData godModeTimer;
    [SerializeField]
    private GameObject godEffect;
    public Action<char, bool> SpawnNewRow;
    public Action BeginLevelup;
    private int playerMovesCount = 0;
    public Action<GameState> GameStateChanged;
    public Action<char> NextLetterChanged;
    public Action<int, int> LevelupMovesChanged;
    public enum GameState { PAUSED, IN_PLAY, LEVEL_UP, LOST, GOD_MODE }
    public GameState CurrentState { get; private set; } = GameState.IN_PLAY;
    private char nextLetter;

    private void Start()
    {
        if (!SpellChecker.IsInitialized())
        {
            Debug.Log("SpellChecker is not initialized");
        }

        if (!LetterSpriteLoader.IsInitialized())
        {
            Debug.Log("LetterSpriteLoader is not initalized");
        }
        playArea.AnswerSubmitted += OnAnswerSubmitted;
        nextLetter = GenerateNextLetter();
        NextLetterChanged?.Invoke(nextLetter);
        TrySetGameState(GameState.IN_PLAY);
        timerData.StartTimer(minute, second);
        timerData.TimeUp += OnTimeUp;
        if(CurrentState != GameState.GOD_MODE)
        {
            if (godEffect.activeSelf)
            {
                godEffect.SetActive(false);
            }
        }
    }

    private char GenerateNextLetter()
    {
        return LetterUtility.GenerateLetter();
    }

    private void OnTimeUp()
    {
        TrySetGameState(GameState.LOST);
    }

    /// <summary>
    /// Should only be called at the beginning of the game to get the starting field state
    /// </summary>
    /// <returns></returns>
    public char[] GetInitalFieldState()
    {
        return new char[] { LetterUtility.GenerateLetter(), LetterUtility.GenerateLetter()};
    }

    public void IncrementMoves()
    {
        if (CurrentState == GameState.GOD_MODE) return;
        playerMovesCount++;
        LevelupMovesChanged?.Invoke(playerMovesCount, LEVELUP_INTERVAL);

        if (playerMovesCount % LETTER_INTERVAL == 0)
        {
            SpawnNewRow?.Invoke(nextLetter, false);
            nextLetter = GenerateNextLetter();
            NextLetterChanged?.Invoke(nextLetter);
        }
        
        if (playerMovesCount % LEVELUP_INTERVAL == 0)
        {
            BeginLevelup?.Invoke();
            TrySetGameState(GameState.LEVEL_UP);
        }
    }

    public int GetCurrentMoves()
    {
        return playerMovesCount;
    }

    public int GetLetterMoves()
    {
        return LETTER_INTERVAL;
    }

    public char GetNextLetter()
    {
        return nextLetter;
    }

    public void OnAnswerSubmitted(string word)
    {
        if (SpellChecker.SpellCheck(word))
        {
            ScoreData.Instance.ChangeScore(word.Length);
            timerData.AddTime(0, word.Length * 2f);
            WordBank.Instance.AddWord(word);
        }
        else
        {
            timerData.RemoveTime(5f);
        }

        // Check if play area has no rows, if not spawn one immediately
        // Check for count of one, because we call this before destroying the the row the player submitted
        if (playArea.RowCount() == 0)
        {
            SpawnNewRow?.Invoke(LetterUtility.GenerateLetter(), false);
        }

        // For now submitting an answer won't count as a move
        //IncrementMoves();
    }

    public void StartGodMode()
    {
        godModeTimer.StartTimer(0, 15);
        print(TrySetGameState(GameState.GOD_MODE));
        godModeTimer.TimeUp += OnGodModeStopped;
        godEffect.SetActive(true);
    }

    public void OnGodModeStopped()
    {
        TrySetGameState(GameState.IN_PLAY);
        godEffect.SetActive(false);
    }

    public void CompleteLevelup()
    {
        TrySetGameState(GameState.IN_PLAY);
    }

    /// <summary>
    /// Attempts to set the game to the specified state
    /// </summary>
    /// <param name="state"></param>
    /// <returns>Will return the result state regardless if successful</returns>
    public GameState TrySetGameState(GameState state)
    {
        if (state == CurrentState) return CurrentState;
        // If already game over, we cannot change to levelup
        if(state == GameState.LEVEL_UP)
        {
            if (CurrentState == GameState.LOST) return CurrentState;
        }
        // Cannot move from swapping to paused
        if(CurrentState == GameState.LEVEL_UP)
        {
            if (state == GameState.PAUSED) return CurrentState;
        }
        if(state == GameState.LOST)
        {
            // Double check that the game state match lose conditions
            if (!IsGameOver())
            {
                return CurrentState;
            }
        }
        CurrentState = state;
        if (state == GameState.IN_PLAY || state == GameState.GOD_MODE)
        {
            timerData.ResumeTimer();
        }else if (state == GameState.PAUSED || state == GameState.LEVEL_UP || state == GameState.LOST)
        {
            timerData.PauseTimer();
        }
        GameStateChanged?.Invoke(CurrentState);
        return CurrentState;
    }

    private bool IsGameOver()
    {
        bool isGameOver = false;
        if (playArea.RowCount() > playArea.MAX_ROWS)
        {
            isGameOver = true;
        }
        else if (timerData.IsTimeUp())
        {
            isGameOver = true;
        }
        return isGameOver;
    }

    public void RestartGame()
    {
        TrySetGameState(GameState.IN_PLAY);
        timerData.StartTimer(minute, second);
        List<char> startingLetters = new() { LetterUtility.GenerateLetter(), LetterUtility.GenerateLetter() };
        playArea.Restart(startingLetters);
        playerMovesCount = 0;
    }
}
