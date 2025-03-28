using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    int STARTING_LETTER_INTERVAL;
    [SerializeField]
    int LEVELUP_INTERVAL;
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
    [SerializeField]
    private AudioClip godClip;
    [SerializeField] 
    private AudioClip gameOverClip;
    public Action<char, bool> SpawnNewRow;
    public Action BeginLevelup;
    private int playerMovesCount = 0;
    public Action<GameState> GameStateChanged;
    public Action<char> NextLetterChanged;
    public Action<int, int> LevelupMovesChanged;
    public enum GameState { PAUSED, IN_PLAY, LEVEL_UP, LOST, GOD_MODE }
    public GameState CurrentState { get; private set; } = GameState.IN_PLAY;
    private char nextLetter;
    private int letter_interval;
    private int letterSpawnCount;

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
        letter_interval = STARTING_LETTER_INTERVAL;
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
        if (playerMovesCount % letter_interval == 0)
        {
            SpawnNextLetter();
        }
        
        if (playerMovesCount % LEVELUP_INTERVAL == 0)
        {
            BeginLevelup?.Invoke();
            TrySetGameState(GameState.LEVEL_UP);
        }
    }

    public void SpawnNextLetter()
    {
        if(CurrentState != GameState.IN_PLAY) return;
        SpawnNewRow?.Invoke(nextLetter, false);
        nextLetter = GenerateNextLetter();
        NextLetterChanged?.Invoke(nextLetter);
        letterSpawnCount++;
        letter_interval = CalculateNextInterval(letterSpawnCount);
    }

    public int GetCurrentMoves()
    {
        return playerMovesCount;
    }

    public int GetLetterMoves()
    {
        return STARTING_LETTER_INTERVAL;
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
        if (TrySetGameState(GameState.GOD_MODE) == GameState.GOD_MODE)
        {
            godModeTimer.TimeUp += OnGodModeStopped;
            SoundController.Instance.PlayAudio(godClip);
            godEffect.SetActive(true);
        }
    }

    public void OnGodModeStopped()
    {
        TrySetGameState(GameState.IN_PLAY);
        godEffect.SetActive(false);
        SoundController.Instance.PlayAudio(SoundController.Instance.gameClip, true);
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
        if (state == GameState.IN_PLAY)
        {
            timerData.ResumeTimer();
        }else if (state == GameState.PAUSED || state == GameState.LEVEL_UP || state == GameState.LOST || state == GameState.GOD_MODE)
        {
            timerData.PauseTimer();
        }
        GameStateChanged?.Invoke(CurrentState);
        if(CurrentState == GameState.LOST)
        {
            SoundController.Instance.PlayAudio(gameOverClip);
        }
        return CurrentState;
    }

    private bool IsGameOver()
    {
        bool isGameOver = false;
        if (playArea.RowCount() > playArea.MAX_ROWS)
        {
            isGameOver = true;
            Debug.Log($"Game over. row count: {playArea.RowCount()} max row: {playArea.MAX_ROWS}");
        }
        else if (timerData.IsTimeUp())
        {
            Debug.Log($"Game over. Timer ran out of time.");
            isGameOver = true;
        }
        return isGameOver;
    }

    public string GameOverCause()
    {
        if (!IsGameOver()) return "Game is not over";
        if (playArea.RowCount() > playArea.MAX_ROWS)
        {
            return "Exceeded max rows.";
        }
        else
        {
            return "Ran out of time.";
        }

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //TrySetGameState(GameState.IN_PLAY);
        //timerData.StartTimer(minute, second);
        //List<char> startingLetters = new() { LetterUtility.GenerateLetter(), LetterUtility.GenerateLetter() };
        //playArea.Restart(startingLetters);
        //playerMovesCount = 0;
    }

    public int CalculateNextInterval(int x)
    {
        return Mathf.Clamp((int)(-0.25f * x + STARTING_LETTER_INTERVAL), 2, int.MaxValue);
    }
}
