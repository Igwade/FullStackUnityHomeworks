using Objects;
using UnityEngine;

namespace Controllers
{
    public class PushController: MonoBehaviour
    {
        [SerializeField] private Character character;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                character.Push();
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                character.Toss();
            }
        }
    }
}