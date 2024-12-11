using UnityEngine;

public class BattlePrepare : MonoBehaviour
{
    public ObjectEventSO PreparationPhaseEndEvent;

    public void OnClick()
    {
        PreparationPhaseEndEvent.RaiseEvent(this,this);
        gameObject.SetActive(false);
    }
}
