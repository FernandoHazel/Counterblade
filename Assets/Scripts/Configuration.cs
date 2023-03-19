using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Task Configuration", menuName ="Tasks/Configuration")]
public class Configuration : ScriptableObject
{
    //All the relevant variables that i came up with for the tasks are on this script
    //and can be configured  from this scriptable object
    [Header("Player")]
    [SerializeField]
    private float _MaxHP;
    [SerializeField]
    private float _BlinkSpeedAfterDamage;

    [Header("Jump Paddle configuration")]
    [SerializeField]
    private float _MaxLaunchTime;
    [SerializeField]
    private float _LaunchVelocity;
    [Header("Bouncing")]
   
    [SerializeField]
    private float _MaxBounceTime;
    [SerializeField]
    private float _BounceVelocity;
    [SerializeField]
    [Range(1,2)]
    private float _BounceOnLaunchMultiplier;
    [SerializeField]
    private int _MaxAmountOfBounces;
    //para mi, hacer multiples boucnes dinamicos
    [Header("ScreenShaking")]
    [SerializeField]
    private float _ShakeForceIncreaseSpeed;
    [SerializeField]
    [Range(1, 2)]
    private float _OnLaunchShakeMultiplier;
    [SerializeField]
    private float _MaxShakeForce;

    [Header("Spikes")]
    [SerializeField]
    private float _DamagePerHit;
    [SerializeField]
    private float _MaxInvulnerableTime;
    [SerializeField]
    private float _KnockBackForce;
    [SerializeField]
    [Range(0, 1)]
    private float _SpeedPercentDebuff;

    public float BlinkRateAfterDamage { get { return _BlinkSpeedAfterDamage; } }
    public float SpeedPercentDebuff { get { return _SpeedPercentDebuff; } }
    public float KnockBackForce { get { return _KnockBackForce; } }
    public float MaxInvulnerableTime{ get { return _MaxInvulnerableTime; } }

    public float MaxBounceTime { get { return _MaxBounceTime; } }
    public float MaxLaunchTime { get { return _MaxLaunchTime; } }
    public float LaunchVelocity { get { return _LaunchVelocity; } }
    public float BounceVelocity { get { return _BounceVelocity; } }
    public float BounceOnLaunchMultiplier { get { return _BounceOnLaunchMultiplier; } }
    public float OnLaunchShakeMultiplier { get { return _OnLaunchShakeMultiplier; } }
    public float MaxShakeForce { get { return _MaxShakeForce; } }
    public float ShakeForceIncreaseSpeed { get { return _ShakeForceIncreaseSpeed; } }
    public float MaxHP { get { return _MaxHP; } }
    public float DamagePerHit { get { return _DamagePerHit; } }
    public int MaxAmountOfBounces { get { return _MaxAmountOfBounces; } }
}
