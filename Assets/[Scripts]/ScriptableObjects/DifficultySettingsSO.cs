using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty", menuName = "ScriptableObjects/DifficultySetting", order = 1)]
public class DifficultySettingsSO : ScriptableObject
{
    [Header("General")]
    public int baseLetterInterval;
    public int baseLevelupInterval;
    public int doubleLetterSpawn;
    [Header("Play Time")]
    public int minute;
    public float second;
    [Header("God Mode")]
    public float godModeDuration;
    [Header("Powerup Spawn Rate")]
    public int middleUpgradeChance;
    public int godModeUpgradeChance;
    public int wildCardUpgradeChance;
}
