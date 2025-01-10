using UnityEngine;

namespace eventChannel
{
    [CreateAssetMenu(fileName = "EventChannel", menuName = "EventChannel/EventChannel")]
    public class EventChannelSO : ScriptableObject
    {
        public delegate void EventChannel();
        public event EventChannel OnEventRaised;

        public void RaiseEvent()
        {
            OnEventRaised?.Invoke();
        }
    }
}