using UnityEngine.SceneManagement;

namespace Utilities.SceneTransition
{
    readonly struct TransitionMessage
    {
        public readonly int SceneIndex;

        public TransitionMessage(int sceneIndex)
        {
            SceneIndex = sceneIndex;
        }
    }
}