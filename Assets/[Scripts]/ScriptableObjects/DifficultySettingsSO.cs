using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty", menuName = "ScriptableObjects/DifficultySetting", order = 1)]
public class DifficultySettingsSO : ScriptableObject
{
    [Header("General")]
    [Tooltip("Base number of seconds before spawning a new row.")]
    public int baseRowSpawnInterval;
    [Tooltip("Base number of 'moves' before player gets an upgrade.")]
    public int baseLevelupInterval;
    [Tooltip("The number of correct answers before double letters start to spawn.")]
    public int doubleLetterSpawn;
    [Tooltip("Reduce the timer by this amount when submitting an incorrect word.")]
    public int incorrectTimePenalty;
    [Tooltip("Multiply the word length with this number to calculate score reward.")]
    public float scoreMultiplier;
    [Tooltip("Multiply the score with this multiplier when submitting duplicate words.")]
    public float scorePenalty;
    [Tooltip("Multiply the word length with this number to calculate time reward.")]
    public float timeMultiplier;
    [Tooltip("Multiply the time with this multiplier when submitting duplicate words.")]
    public float timePenalty;
    [Header("Play Time")]
    [Tooltip("Amount of minutes player starts with.")]
    public int minute;
    [Tooltip("Amount of seconds player starts with.")]
    public float second;
    [Header("God Mode")]
    [Tooltip("Number of seconds per count-down.")]
    public float countdownAnimationDuration;
    [Tooltip("Number to count down.")]
    public int countdownNumber;
    [Tooltip("The total amount of seconds god mode lasts.")]
    public float godModeDuration;
    [Header("Powerup Spawn Rate")]
    [Tooltip("% chance of getting a middle letter upgrade.")]
    public int middleUpgradeChance;
    [Tooltip("Number of correct answers before spawning middleUpgrade.")]
    public int minMiddleAnsCount;
    [Tooltip("% chance of getting the god mode upgrade.")]
    public int godModeUpgradeChance;
    [Tooltip("Number of correct answers before spawning god mode.")]
    public int minGodModeAnsCount;
    [Tooltip("% chance of getting the wild card upgrade.")]
    public int wildCardUpgradeChance;
    [Tooltip("Number of valid tiles before spawning wild card.")]
    public int minSoloSysTileCount;
}
