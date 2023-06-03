using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class Behaviours : MonoBehaviour
{
    public static event EventHandler OnAnyAbilityStart;
    public static event EventHandler<Behaviours> OnAnyAbilityCompleted;
    protected Enemy _Enemy;
    protected bool _IsActive;
    protected Action _OnActionCompleted;
    protected virtual void Awake()
    {
        _Enemy = GetComponent<Enemy>();
    }

    protected void ActionStart(Action OnACTIONComplete)
    {
        _IsActive = true;
        this._OnActionCompleted = OnACTIONComplete;
        OnAnyAbilityStart?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete(Behaviours behaviourToGO, string sender)
    {
        _IsActive = false;
        // _OnActionCompleted();
        OnAnyAbilityCompleted?.Invoke(this, behaviourToGO);
        //Debug.Log("Existing towards behaviour :" + behaviourToGO + " -- sender is :" + sender + "-" + abilityCastAmount);
    }

    protected bool DetectIfPlayerInRange()
    {

        //cast something directly of the player, this can be used in case the boss has an attack that needs
        //the player to be in sight of him and not behind somehting, ehrew we can also play with the
        //destructable tier levels of the enviroment i placed on the boss stas, to let the boss know if he can actually attack the player even if hes behind something.
        //We can also let every behaviour decided what to do, for example on idle it can cast the raycast and be like, well hes behind something, lets do the howl attack that doesnt require me to be worried about the player hiding behind, since the attack is in area or can affect areas regardless of if theres soemthing or not
        //For now we are assuming theres nothing blocking

        //Debug.Log(Vector2.Distance(_Enemy.GetPlayerPosition.position, _Enemy.transform.position) + "-" + (Vector2.Distance(_Enemy.GetPlayerPosition.position, _Enemy.transform.position) <= _Enemy.GetRangeDetectionValue()));
        // return Vector2.Distance(_Enemy.GetPlayerPosition.position, _Enemy.transform.position) <= _Enemy.GetRangeDetectionValue();
        return false;
    }

    protected Vector2 ReturnRandomPointInScenario(int minDistance, int maxDistance, Vector2 currentPos)
    {
        Vector2 targetPos = currentPos;
        if(UnityEngine.Random.Range(0,100) < 50)
            targetPos += new Vector2(UnityEngine.Random.Range(minDistance, maxDistance), UnityEngine.Random.Range(minDistance, maxDistance));
        else
            targetPos -= new Vector2(UnityEngine.Random.Range(minDistance, maxDistance), UnityEngine.Random.Range(minDistance, maxDistance));
        return targetPos;
    }

    public abstract string GetAbilityName();

    public abstract void OnEnterBehaviour();
    public abstract void Tick();

    public abstract void OnExitBehaviour(Behaviours behaviourToGo);

    public abstract void OnDamaged();
    public abstract void OnHealed();
    public abstract void OnDead();


}
