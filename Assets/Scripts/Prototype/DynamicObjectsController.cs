using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Prototype
{
    public class DynamicObjectsController : MonoBehaviour
    {
        [SerializeField]
        private List<DynamicGameObject> _dynamicGameObjects;

        private void OnValidate()
        {
            _dynamicGameObjects = FindObjectsOfType<DynamicGameObject>().ToList();
        }
        

        public void SetActiveAll(bool isActive)
        {
            foreach (var dyno in _dynamicGameObjects)
            {
                if(dyno)
                    dyno.SetActive(isActive);
            }
        }
    }
}