using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler: MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 startRotation;
    Unit unit;
    private void Start() {
        this.unit = GetComponent<Unit>();
        this.startPosition = unit.transform.position;
        this.startRotation = unit.transform.rotation.eulerAngles;
    }

    public void StopAllGoToIdle()
    {
        if (!unit.isDead)
        {
        unit.anim.SetBool("Run", false);
        }
    }


    public void FaceTarget()
    {
        unit.transform.LookAt(unit.target.transform.position);
    }
        public void FaceForward()
    {
        unit.transform.position = new Vector3 (unit.transform.position.x, 0, unit.transform.position.z);
    }

        public void MoveToTarget()
        {
            StartCoroutine(MoveToTarget_Coroutine());
        }
        public void ReturnToStartPosition()
        {
            StartCoroutine(ReturnToStartPosition_Coroutine());
        }
        public IEnumerator ReturnToStartPosition_Coroutine()
        {
            unit.agent.isStopped = false;

            unit.agent.SetDestination(startPosition);

            while (unit.agent.pathPending)
                yield return null;

            unit.anim.SetBool("Run", true);
            

            while (unit.agent.remainingDistance >= 0.3f)
            {
                
                yield return null;
            }
            unit.anim.SetBool("Run",false);
            unit.transform.position = startPosition;
            unit.transform.rotation = new Quaternion(startRotation.x, startRotation.y, startRotation.z, 1f);
            unit.agent.isStopped = true;
            unit.isExecutingAction = false;
        }

        public IEnumerator MoveToTarget_Coroutine()
    {
        //Is spear
        //Is 2HS
        //Is Fists
        //Is onehand
        //Is Ranged
        unit.agent.isStopped = false;
        unit.agent.SetDestination(unit.target.transform.position);
        
        while (unit.agent.pathPending)
        {
            yield return null;
        }
    
        unit.anim.SetBool("Run",true);
     

        while (unit.agent.remainingDistance >= 2f)
        {
            //Debug.Log(unit.agent.remainingDistance);
            yield return null;
        }
        unit.anim.SetBool("Run",false);
        unit.agent.isStopped = true;

        yield return null;
        
    }
    
}
