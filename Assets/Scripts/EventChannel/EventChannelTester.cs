using eventChannel;
using UnityEngine;

public class EventChannelTester : MonoBehaviour
{
    [SerializeField] private EventChannelSO clearEventChannel;
    [SerializeField] private EventChannelSO endEventChannel;

    public void Clear()
    {
        clearEventChannel.RaiseEvent();
    }

    public void End()
    {
        endEventChannel.RaiseEvent();
    }
}
