using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private PlayAreaController playArea;
    [SerializeField]
    private TimerData timerData;
    [SerializeField]
    private TimerData godModeTimer;
    [SerializeField]
    private TimerData rowSpawnTimer;
    [SerializeField]
    private GameObject godEffect;
    [SerializeField]
    private AudioClip godClip;
    [SerializeField] 
    private AudioClip gameOverClip;
    [SerializeField]
    private SimpleAnimator animator;
    [SerializeField]
    private FloatingText floatingTimeText;
    public Action<string, bool> SpawnNewRow;
    public Action BeginLevelup;
    private int playerMovesCount = 0;
    public Action<GameState> GameStateChanged;
    public Action<string> NextLetterChanged;
    public Action<int, int> LevelupMovesChanged;
    public Action<float> TimeToNextRowSpawn;
    public enum GameState { PAUSED, IN_PLAY, LEVEL_UP, LOST, GOD_MODE }
    public GameState CurrentState { get; private set; } = GameState.IN_PLAY;
    private string nextLetter;
    public int CorrectWordsSubmitted { get; private set; }
    public int LevelupCount { get; private set; }
    public int IncorrectWords { get; private set; }

    DifficultySettingsSO difficultySetting;
    private void Start()
    {
        difficultySetting = DifficultyManager.Instance.difficultySettings;
        CorrectWordsSubmitted = 0;
        if (!SpellChecker.IsInitialized())
        {
            Debug.Log("SpellChecker is not initialized");
        }

        if (!LetterSpriteLoader.IsInitialized())
        {
            Debug.Log("LetterSpriteLoader is not initalized");
        }
        playArea.AnswerSubmitted += OnAnswerSubmitted;
        nextLetter = GenerateNextSubstring();
        NextLetterChanged?.Invoke(nextLetter);
        TrySetGameState(GameState.IN_PLAY);
        timerData.StartTimer(difficultySetting.minute, difficultySetting.second);
        timerData.TimeUp += OnTimeUp;
        if(CurrentState != GameState.GOD_MODE)
        {
            if (godEffect.activeSelf)
            {
                godEffect.SetActive(false);
            }
        }
        rowSpawnTimer.StartTimer(0, difficultySetting.rowSpawnInterval.Evaluate(CorrectWordsSubmitted/100f));
        rowSpawnTimer.TimeUp += OnRowSpawnTimerUp;
        rowSpawnTimer.TimeChanged += OnRowTimeChanged;
    }

    private void OnRowTimeChanged()
    {
        float second = rowSpawnTimer.second;
        second += 60f * rowSpawnTimer.minute;
        float percent = 1f - (second / difficultySetting.rowSpawnInterval.Evaluate(CorrectWordsSubmitted / 100f));
        TimeToNextRowSpawn?.Invoke(percent);
    }

    private void OnRowSpawnTimerUp()
    {
        rowSpawnTimer.StartTimer(0, difficultySetting.rowSpawnInterval.Evaluate(CorrectWordsSubmitted/100f));
        SpawnNextLetter();
    }

    private string GenerateNextSubstring()
    {
        string spawn = string.Empty;
        List<char> used = new();
        spawn += LetterUtility.GenerateLetter();
        float chanceSpawn = difficultySetting.doubleLetterCurve.Evaluate(Mathf.Clamp(CorrectWordsSubmitted, 0, 100)/100);
        float roll = UnityEngine.Random.Range(0f, 1f);
        if (roll > chanceSpawn) return spawn;

        if (LetterUtility.IsVowel(spawn[0]))
        {
            spawn += LetterUtility.GenerateLetter();
        }
        else
        {
            // double consonant spawn protection
            spawn += LetterUtility.GenerateUniqueVowel(used);
        }
        return spawn;
    }

    private void OnTimeUp()
    {
        TrySetGameState(GameState.LOST);
    }

    /// <summary>
    /// Should only be called at the beginning of the game to get the starting field state
    /// </summary>
    /// <returns></returns>
    public string[] GetInitalFieldState()
    {
        return new string[] { GenerateNextSubstring(), GenerateNextSubstring()};
    }

    public void IncrementMoves()
    {
        if (CurrentState == GameState.GOD_MODE) return;
        playerMovesCount++;
        int newLevelupCount = (int)Mathf.Round(difficultySetting.baseLevelupInterval.Evaluate(LevelupCount / 100f));
        LevelupMovesChanged?.Invoke(playerMovesCount, newLevelupCount);
        
        if (playerMovesCount % newLevelupCount == 0)
        {
            BeginLevelup?.Invoke();
            LevelupCount++;
        }
    }

    public void SpawnNextLetter()
    {
        if(CurrentState != GameState.IN_PLAY && CurrentState != GameState.GOD_MODE) return;
        SpawnNewRow?.Invoke(nextLetter, false);
        nextLetter = GenerateNextSubstring();
        NextLetterChanged?.Invoke(nextLetter);
        rowSpawnTimer.StartTimer(0, difficultySetting.rowSpawnInterval.Evaluate(CorrectWordsSubmitted / 100f));
        if (CurrentState == GameState.GOD_MODE) rowSpawnTimer.PauseTimer();
        TimeToNextRowSpawn?.Invoke(0);
    }

    public int GetCurrentMoves()
    {
        return playerMovesCount;
    }

    public string GetNextLetter()
    {
        return nextLetter;
    }

    public void OnAnswerSubmitted(string word, Vector2 rowPos)
    {
        if (SpellChecker.SpellCheck(word))
        {
            float scoreValue = word.Length;
            float timeValue = word.Length;
            int wordCount = WordBank.Instance.GetWordCount(word);
            if (wordCount > 0)
            {
                scoreValue *= difficultySetting.scorePenalty.Evaluate(wordCount / 100f);
                timeValue *= difficultySetting.timePenalty.Evaluate(wordCount / 100f);
            }
            ScoreData.Instance.ChangeScore(scoreValue * difficultySetting.scoreMultiplier);
            timerData.AddTime(0, timeValue * difficultySetting.timeMultiplier);
            WordBank.Instance.AddWord(word);
            // for difficulty, we only count words correctly submitted.
            CorrectWordsSubmitted++;
            floatingTimeText.PlayText($"+{timeValue}s", Color.green, rowPos);
        }
        else
        {
            timerData.RemoveTime(difficultySetting.incorrectTimePenalty);
            floatingTimeText.PlayText($"-{difficultySetting.incorrectTimePenalty}s", Color.red, rowPos);
            IncorrectWords++;
        }
        // For now submitting an answer won't count as a move
        //IncrementMoves();
    }

    public void StartGodMode()
    {
        godModeTimer.StartTimer(0, difficultySetting.godModeDuration);
        if (TrySetGameState(GameState.GOD_MODE) == GameState.GOD_MODE)
        {
            godModeTimer.TimeUp += OnGodModeStopped;
            godModeTimer.TimeChanged += OnGodModeTimerTick;
            SoundController.Instance.PlayAudio(godClip);
            godEffect.SetActive(true);
        }
    }


    private void OnGodModeTimerTick()
    {
        int num = difficultySetting.countdownNumber;
        float anim = difficultySetting.countdownAnimationDuration;
        // This should only happen once per god mode activation
        if (godModeTimer.minute == 0 && godModeTimer.second <= (anim * num) && godModeTimer.second > 1f)
        {
            // Ensure that animation hasn't already been added
            if (!animator.IsAnimating())
            {
                // Pop in the numbers 3, 2, 1 on screen
                for (int i = num; i > 0; i--)
                {
                    Sprite sprite = LetterSpriteLoader.GetSprite((char)(48 + i));
                    AnimationData popIn = new(SimpleAnimator.AnimationType.POP_IN, anim, sprite);
                    animator.QueueAnimation(popIn);

                }
            }
        }
    }

    private void OnGodModeStopped()
    {
        godEffect.SetActive(false);
        playArea.RemoveEmptyRows();
        // If game is over then no need to do anything else.
        if (CurrentState == GameState.LOST) return;
        TrySetGameState(GameState.IN_PLAY);
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
            rowSpawnTimer.ResumeTimer();
        }else if (state == GameState.PAUSED || state == GameState.LEVEL_UP || state == GameState.LOST || state == GameState.GOD_MODE)
        {
            timerData.PauseTimer();
            rowSpawnTimer.PauseTimer();
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
}
