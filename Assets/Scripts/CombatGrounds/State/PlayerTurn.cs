using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TBCMiniProject.BattleHandler;
using System;

public class PlayerTurn : State
{
    public PlayerTurn(BattleHandler _battleHandler) : base(_battleHandler)
    {
    }

#region Enter
    public override IEnumerator Enter()
    {
        Debug.Log("Entering Player Turn");
        battleHandler.inputHandler.onCurrentUnitActionHasBeenChosen += AssignPlannedActionToPlayerUnit;
        
        yield return Main();
    }
#endregion

#region Main
    public override IEnumerator Main()
    {
        Debug.Log("WaitUntilPlayerUnitActionInputAdded");

        yield return GetAllPlayerUnitActionInputs(); 
    }


    public IEnumerator GetAllPlayerUnitActionInputs()
    {
        foreach (Unit activePlayerUnit in battleHandler.playerTeam)
        {
            if (!activePlayerUnit.isDead)
            {
                ChangeCurrentPlayerUnit(activePlayerUnit);
                while (battleHandler.currentPlayerUnit.plannedAction == null)
                {
                    yield return null;
                }
                battleHandler.playerActionQueue.Add(battleHandler.currentPlayerUnit);
                continue;
            }
            continue;
        }
        yield return ExecuteAllPlayerActionQueue();
    }
    public void AssignPlannedActionToPlayerUnit(object sender, int _action)
    {
        switch (_action)
        {
            case 1:
                battleHandler.currentPlayerUnit.plannedAction = new BasicAttack(battleHandler.currentPlayerUnit);
                break;

            case 2:
                battleHandler.currentPlayerUnit.plannedAction = new CastAbility(battleHandler.currentPlayerUnit);
                break;

            default:
                break;
        }
        
    }
    public IEnumerator ExecuteAllPlayerActionQueue()
    {
        //OrganizeWhoExecutesFirstBySpeed();
        foreach (PlayerUnit playerUnit in battleHandler.playerActionQueue)
        {
            playerUnit.plannedAction.Execute();
            while (playerUnit.isExecutingAction)
            {
                yield return null;
            }
            playerUnit.plannedAction = null;
            playerUnit.target = null;
        }

        ValidateVictoryConditions();
        yield return null;
    }

    private void ValidateVictoryConditions()
    {
        foreach (Unit enemy in battleHandler.enemyTeam)
        {
            if (!enemy.isDead)
            {
                battleHandler.ChangeState(new EnemyTurn(battleHandler));
                break;
            }
        }
        Debug.Log("Enemy team has fallen you are victorious");
    }


    #endregion

    #region Exit  
    public override IEnumerator Exit()
    {
        Debug.Log("Player turn ended");
        battleHandler.inputHandler.onCurrentUnitActionHasBeenChosen -= AssignPlannedActionToPlayerUnit;
        
        
        yield return null;

    }
#endregion








#region Explanitory Variables (Player Turn)
    private void ChangeCurrentPlayerUnit(Unit playerUnit)
    {
        
        battleHandler.currentPlayerUnit = (PlayerUnit)playerUnit;
        battleHandler.inputHandler.currentPlayerUnit = battleHandler.currentPlayerUnit;
    }
#endregion





}
