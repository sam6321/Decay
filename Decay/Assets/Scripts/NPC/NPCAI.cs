using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.StateMachine;

public class NPCAI : MonoBehaviour
{
    enum States
    {
        Start,
        Stop
    }

    StateMachine<States> fsm;

    // Start is called before the first frame update
    void Start()
    {
        fsm = StateMachine<States>.Initialize(this);
        fsm.ChangeState(States.Start);
    }

    IEnumerator Start_Enter()
    {
        Debug.Log("Start_Enter");
        yield return new WaitForSeconds(2.0f);
        fsm.ChangeState(States.Stop);
    }

    void Start_Exit()
    {
        Debug.Log("Start_Exit");
    }

    IEnumerator Stop_Enter()
    {
        Debug.Log("Stop_Enter");
        yield return new WaitForSeconds(2.0f);
        fsm.ChangeState(States.Start);
    }

    void Stop_Exit()
    {
        Debug.Log("Stop_Exit");
    }
}
