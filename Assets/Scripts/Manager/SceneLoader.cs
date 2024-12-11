using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public ObjectEventSO NewGmeEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnNewGame()
    {
        NewGmeEvent.RaiseEvent(null ,this);
    }
}
