using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Level Data", menuName = "Levels/Configuration")]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private ScenarioPhaseData data;
}


[System.Serializable]
public class ScenarioPhaseData
{
    //Enemy kills
    [Header("Win conditions (can be multiple)")]
    [SerializeField] private bool hasToTakeEnemies;
    public bool HasToKillEnemies() { return hasToTakeEnemies; }

    [ShowIf(EConditionOperator.Or, "HasToKillEnemies")]
    [AllowNesting]
    public KillEnemiesConfig killEnemiesConfig;

    //Survive time
    [SerializeField] private bool hasToSurviveTime;
    public bool HasToSurviveTime() { return hasToSurviveTime; }

    [ShowIf(EConditionOperator.Or, "HasToSurviveTime")]
    [AllowNesting]
    public SurviveTimeConfig surviveTimeConfig;


    //Tasks
    [SerializeField] private bool hasToCompleteTask;
    public bool HasToDoTask() { return hasToCompleteTask; }

    [ShowIf(EConditionOperator.Or, "HasToDoTask")]
    [AllowNesting]
    public TaskConfig tasksConfig;


    //Boss
    [SerializeField] private bool hasToBeatBoss;
    public bool HasToKillBoss() { return hasToBeatBoss; }

    [ShowIf(EConditionOperator.Or, "HasToDoTask")]
    [AllowNesting]
    public BossConfig BossConfig;
}

[System.Serializable]
public class BossConfig
{
    [Header("TO BE DEFINED")]
    public int asda;
}

[System.Serializable]
public class TaskConfig
{
    [Header("TO BE DEFINED")]
    public int asda;
}

[System.Serializable]
public class SurviveTimeConfig
{
    [InfoBox("Agregue ciertas opciones extras para este, si les gusta las podemos implementar ", EInfoBoxType.Normal)]
    public int secondsToSurvive;

    [SerializeField] private bool playerGainsTimeOnKill;
    public bool GainsTimeOnKill() { return playerGainsTimeOnKill; }

    [InfoBox("Este es un multiplier, lo correcto es que cada enemigo tenga su propio valor de tiempo ganado por morir, asi tenemos mas customization y control sobre esto", EInfoBoxType.Error)]
    [ShowIf(EConditionOperator.Or, "GainsTimeOnKill")]
    [Range(1, 10)]
    public float timeGainedPerKillMultiplier;
}


[System.Serializable]
public class KillEnemiesConfig
{
    [InfoBox("De momento solo tendremos una cantidad fija, si se busca que tengamos mas control y variedad de spawns en ciertos niveles, habra que juntarnos para definir las especificaciones, pero este sistema esta preparado para actualizarse ", EInfoBoxType.Normal)]
    public int AmountOfEnemiesToKill;

}
