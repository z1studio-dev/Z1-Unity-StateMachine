using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateEventTriggers<Estate> : MonoBehaviour where Estate: Enum
{
    public List<StateMachineEvent<Estate>> Events;

   public void OnStateEnter(Estate estate)
   {
       Debug.Log("State Enter to " + estate);

         foreach (var stateEvent in Events)
         {
             if (EqualityComparer<Estate>.Default.Equals(stateEvent.State, estate))
              {
                  stateEvent.EventEnter.Invoke();
              }
         }
   }

    public void OnStateExit(Estate estate)
    {
         Debug.Log("State Exit to " + estate);
    
            foreach (var stateEvent in Events)
            {
                if (EqualityComparer<Estate>.Default.Equals(stateEvent.State, estate))
                  {
                      stateEvent.EventExit.Invoke();
                  }
            }
    }
}

[System.Serializable]
public struct StateMachineEvent<Estate> where Estate: Enum
{
   public Estate State;
    public UnityEvent EventEnter;
    public UnityEvent EventExit;
}
