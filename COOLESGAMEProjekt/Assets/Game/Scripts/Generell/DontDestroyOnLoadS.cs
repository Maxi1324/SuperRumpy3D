using UnityEngine;

namespace Generell
{
    public class DontDestroyOnLoadS : MonoBehaviour
    {
        public void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
