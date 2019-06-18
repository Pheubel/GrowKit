using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Utilities.SceneTransition
{
    public class SceneTransitionManager : MonoBehaviour
    {
        private static SceneTransitionManager _instance;

        private Animator _animator;

        private GameMessenger _messenger;
        [SerializeField] private int _targetedSceneIndex;
        
        [SerializeField] PlantTransitionEvent _plantTransitionEvent;

        private TransitionState _currentState;

        void Start()
        {
            if (_instance == null)
                _instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            _currentState = TransitionState.NoTransition;

            _messenger = GameMessenger.Instance;
            _messenger.RegisterSubscriberToMessageTypeOf<TransitionMessage>(HandleTransitionMessage);
            _messenger.RegisterSubscriberToMessageTypeOf<PlantTransitionMessage>(HandlePlantTransitionMessage);

            _animator = GetComponent<Animator>();

            DontDestroyOnLoad(gameObject);
        }

        private void HandleTransitionMessage(TransitionMessage message)
        {
            _targetedSceneIndex = message.SceneIndex;
            _animator.SetTrigger("DoFadeOut");
            _currentState = TransitionState.FadeOut;
        }

        private void HandlePlantTransitionMessage(PlantTransitionMessage message)
        {
            _targetedSceneIndex = message.SceneIndex;
            _animator.SetTrigger("DoFadeOut");
            _currentState = TransitionState.FadeOut;
            
        }

        private void ChangeScene()
        {
            SceneManager.LoadScene(_targetedSceneIndex);
            _animator.SetTrigger("DoFadeIn");
            _currentState = TransitionState.FadeIn;
        }

        private void OnTransitionComplete()
        {
            _currentState = TransitionState.NoTransition;
        }

        private IEnumerator DoPlantTransition(PlantTransitionMessage message)
        {
            while (_currentState != TransitionState.NoTransition)
                yield return null;
            _plantTransitionEvent?.Invoke(message);
        }

        private enum TransitionState : byte
        {
            NoTransition,
            FadeOut,
            FadeIn
        }
    }
}
