using UnityEngine;

namespace TestForMe
{

    public sealed class Rotator : MonoBehaviour
    {
        private void Update()
        {
            transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        }

    }

}