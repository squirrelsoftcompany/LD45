using UnityEngine;

namespace Behaviour
{
    [RequireComponent(typeof(Cinemachine.CinemachineVirtualCamera))]
    public class InitializeCinemachine : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var machine = GetComponent<Cinemachine.CinemachineVirtualCamera>();
            var mask = GameObject.FindWithTag("Mask");
            machine.Follow = mask.transform;
            machine.LookAt = mask.transform;

            enabled = false;
        }
    }
}
