using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Hands;



public struct AttractingData
{
    public bool isAttractingLeft;
    public bool isAttractingRight;
    public Handedness ActiveHande;

    public AttractingData(bool isAttractingLeft, bool isAttractingRight, Handedness ActiveHande)
    {
        this.isAttractingLeft = isAttractingLeft;
        this.isAttractingRight = isAttractingRight;
        this.ActiveHande = ActiveHande;
    }
}

public class AttractGameObjectToHand : MonoBehaviour
{
    public bool enable = true;
    
    private Vector3 attractor;
    public Rigidbody rigidBodyToAttract;
    public Transform referencesBonesAttracted;
    public bool enableRotation = false;
    public bool followRotX = true;
    public bool followRotY = true;
    public bool followRotZ = true;
    [SerializeField]
    private float fallOffDistance = 0.5f;
    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private float rotationSpeed = 1.0f;
    
    [SerializeField]
    private bool isLeftAttracting = false;
    [SerializeField]
    private bool isRightAttracting = false;
    
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private Vector3 offsetRotation;

    //[SerializeField]
   // private Handedness handSelection;

    private HandsEventManager handsEventManager;
    private AttractingData attractingData;
    
    [SerializeField]
    [Tooltip("The hand shape or pose that must be detected for the gesture to be performed.")]
    ScriptableObject m_HandShapeOrPose;
    
    public UnityEvent OnAttracting;
    public UnityEvent OnNotAttracting;

    void Start(){
        handsEventManager = HandsEventManager.GetInstance();
    }
    //Can be refactor with global hands event manager instead of doing it locally here
    void OnEnable(){
        HandsEventManager.OnGesturePerformed += GesturePerformed;
        HandsEventManager.OnGestureEnded += GestureEnded;
    }

    void OnDisable(){
        HandsEventManager.OnGesturePerformed -= GesturePerformed;
        HandsEventManager.OnGestureEnded -= GestureEnded;
    }

    public void GesturePerformed(XRHandJointsUpdatedEventArgs eventArgs, ScriptableObject handShapeOrPose, Transform targetTransform)
    {
      //  if(eventArgs.hand.handedness != Handedness.Invalid || eventArgs.hand.handedness != handSelection) return;
        if(handShapeOrPose == m_HandShapeOrPose){
            //this will get the last hand that was active
            attractingData.ActiveHande = eventArgs.hand.handedness;
            HandleGesturePerformed(eventArgs);
            OnAttracting?.Invoke();
            //Log.LogMe(this.name, "Gesture Performed: " + handShapeOrPose.name);

        }
    }

    public void GestureEnded(XRHandJointsUpdatedEventArgs eventArgs, ScriptableObject handShapeOrPose, Transform targetTransform)
    {
       // if(eventArgs.hand.handedness != Handedness.Invalid || eventArgs.hand.handedness != handSelection) return;
        if(handShapeOrPose == m_HandShapeOrPose){
            HandleGestureEnded(eventArgs);
            OnNotAttracting?.Invoke();
            //Log.LogMe(this.name, "Gesture Ended: " + handShapeOrPose.name);
        }
    }

    public void ExternalStopTrigger(){
        attractingData.isAttractingRight = false;
        attractingData.isAttractingLeft = false;
    }

    private void HandleGesturePerformed(XRHandJointsUpdatedEventArgs eventArgs){
       // _handedness = eventArgs.hand.handedness;

        if (eventArgs.hand.handedness == Handedness.Left)
        {
            attractingData.isAttractingLeft = true;
            isLeftAttracting = true;
        }

        if (eventArgs.hand.handedness == Handedness.Right)
        {
            attractingData.isAttractingRight = true;
            isRightAttracting = true;       
        }
    }

    private void HandleGestureEnded(XRHandJointsUpdatedEventArgs eventArgs){

        if (eventArgs.hand.handedness == Handedness.Left)
        {
            attractingData.isAttractingLeft = false;
            isLeftAttracting = false;
        }

        if (eventArgs.hand.handedness == Handedness.Right)
        {
            attractingData.isAttractingRight = false;
            isRightAttracting = false;
        }
    }

    void Update()
    {
        if(!enable)
            return;
        
        if (attractingData.isAttractingRight || attractingData.isAttractingLeft)
        {
            AttractObjectToHand();  
        }
    }

    public void AttractObjectToHand()
    {
        Rigidbody rb = rigidBodyToAttract;
        if (rb != null)
        {
            if (attractingData.isAttractingLeft && attractingData.isAttractingRight)
            {
                Vector3 attractorLeft = handsEventManager.GetHandPose(Handedness.Left).position + 
                            handsEventManager.GetHandPose(Handedness.Left).right * offset.x + 
                            handsEventManager.GetHandPose(Handedness.Left).up * offset.y + 
                            handsEventManager.GetHandPose(Handedness.Left).forward * offset.z;
                
                Vector3 attractorRight = handsEventManager.GetHandPose(Handedness.Right).position + 
                            handsEventManager.GetHandPose(Handedness.Right).right * offset.x + 
                            handsEventManager.GetHandPose(Handedness.Right).up * offset.y + 
                            handsEventManager.GetHandPose(Handedness.Right).forward * offset.z;

                attractor = (attractorLeft + attractorRight) / 2;
            }
            else
            {
                attractor = handsEventManager.GetHandPose(attractingData.ActiveHande).position + 
                            handsEventManager.GetHandPose(attractingData.ActiveHande).right * offset.x + 
                            handsEventManager.GetHandPose(attractingData.ActiveHande).up * offset.y + 
                            handsEventManager.GetHandPose(attractingData.ActiveHande).forward * offset.z;
            }

            float distance = Vector3.Distance(attractor, referencesBonesAttracted.transform.position);
            if (distance > fallOffDistance)
            {
                float speedDivider = 0.5f;
                // Calculate new position by interpolating between current position and target position
                Vector3 newPosition = Vector3.Lerp(referencesBonesAttracted.transform.position, attractor, speed * Time.deltaTime * speedDivider);
                // Calculate linear velocity based on position difference over time
                Vector3 linearVelocity = (newPosition - referencesBonesAttracted.transform.position) / Time.deltaTime * speedDivider;
                // Smooth the velocity transition by interpolating between current and target velocity
                Vector3 smoothLinearVelocity = Vector3.Lerp(rb.linearVelocity, linearVelocity, speed * Time.deltaTime * speedDivider);
                rb.linearVelocity = smoothLinearVelocity;
                
                if (enableRotation)
                {
                    // 1) compute the rotation that makes the object look at the camera
                    Quaternion targetRot = Quaternion.LookRotation(
                        Camera.main.transform.position - referencesBonesAttracted.position);

                    // 2) add your designer-defined offset
                    targetRot *= Quaternion.Euler(offsetRotation);

                    // 3) strip out any Euler component we don’t want to follow
                    Vector3 targetEuler = targetRot.eulerAngles;
                    Vector3 currentEuler = rb.rotation.eulerAngles;

                    if (!followRotX) targetEuler.x = currentEuler.x;
                    if (!followRotY) targetEuler.y = currentEuler.y;
                    if (!followRotZ) targetEuler.z = currentEuler.z;

                    // 4) apply the constrained rotation smoothly
                    Quaternion constrained = Quaternion.Euler(targetEuler);
                    rb.MoveRotation(Quaternion.Slerp(rb.rotation, constrained, rotationSpeed * Time.deltaTime));
                }
            }
        }       
        else
        {
            Debug.LogWarning("No Rigidbody attached to the object to attract.");
        }        
    }

}
