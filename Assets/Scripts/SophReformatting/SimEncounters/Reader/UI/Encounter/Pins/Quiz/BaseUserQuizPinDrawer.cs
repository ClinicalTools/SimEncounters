using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseUserQuizPinDrawer : MonoBehaviour
    {
        public abstract void Display(UserQuizPin quizPin);

        public class Pool : SceneMonoMemoryPool<BaseUserQuizPinDrawer>
        {
            public Pool(SignalBus signalBus) : base(signalBus) { }
        }
    }


    public class Hmmmm
    {
        public static int X = 0;
    }

    // Zero parameters
    // NOTE: For this to work, the given component must be at the root game object of the thing
    // you want to use in a pool
    public class SceneMonoMemoryPool<TValue> : MonoMemoryPool<TValue>
        where TValue : Component
    {
        private readonly SignalBus _signalBus;
        private bool _sceneChanged = false;

        [Inject]
        public SceneMonoMemoryPool(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<SceneChangedSignal>(SceneChanged);
        }

        Transform _originalParent;
        protected override void OnCreated(TValue item)
        {
            _originalParent = item.transform.parent;
            base.OnCreated(item);
        }
        protected virtual void SceneChanged() => _sceneChanged = true;

        protected override void OnDespawned(TValue item)
        {
            if (!_sceneChanged && _originalParent && _originalParent.transform)
                base.OnDespawned(item);
        }
    }
}