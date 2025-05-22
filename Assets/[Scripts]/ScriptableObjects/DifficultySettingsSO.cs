using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty", menuName = "ScriptableObjects/DifficultySetting", order = 1)]
public class DifficultySettingsSO : ScriptableObject
{
    [Header("General")]
    [Tooltip("Number of seconds before spawning a new row per number of correct answers submitted / 100.")]
    public AnimationCurve rowSpawnInterval;
    [Tooltip("Base number of 'moves' before player gets an upgrade per number of levelups already achieved/100.")]
    public AnimationCurve baseLevelupInterval;
    [Tooltip("The number of correct answers before double letters start to spawn. the 'time' (x-axis) stands for number of correct answers / 100 and the 'value' (y-axis) is the chance of double spawn between 0-1")]
    public AnimationCurve doubleLetterCurve;
    [Tooltip("Reduce the timer by this amount when submitting an incorrect word.")]
    public int incorrectTimePenalty;
    [Tooltip("Multiply the word length with this number to calculate score reward.")]
    public float scoreMultiplier;
    [Tooltip("Multiply the score gained with this (penalty) multiplier when submitting duplicate words per times duplicate word has been submitted / 100.")]
    public AnimationCurve scorePenalty;
    [Tooltip("Multiply the word length with this number to calculate time reward.")]
    public float timeMultiplier;
    [Tooltip("Multiply the time gained with this (penalty) multiplier when submitting duplicate words per times duplciate word has been submitted / 100.")]
    public AnimationCurve timePenalty;
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
