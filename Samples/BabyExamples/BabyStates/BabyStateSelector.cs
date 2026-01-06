using UnityEngine;

public class BabyStateSelector : MonoBehaviour
{
    public BabyStateMachine.EBabyState BabyState;
    public BabyStateMachine BabyStateMachine;

    //this is for Ui or button usage
    public void GoToState()
    {
        BabyStateMachine.ApplyNextStateSettings(BabyState);
    }

    public void GoToState(BabyStateMachine.EBabyState state)
    {
        BabyStateMachine.ApplyNextStateSettings(state);
        BabyState = state; //just to check in runtime that we received the state data correctly
    }
    
    
}
