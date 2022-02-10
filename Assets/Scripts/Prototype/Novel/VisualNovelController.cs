using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

namespace Prototype.Novel
{
    public class VisualNovelController : MonoBehaviour
    {
        [SerializeField] private Transform _commandGameObjectsRoot;
        [SerializeField] private GameObject[] _commandGameObjects;

        private void OnValidate()
        {
            if (_commandGameObjectsRoot)
            {
                _commandGameObjects = _commandGameObjectsRoot.GetComponentsInChildren<Transform>(true)
                    .Where(g => !g.transform.Equals(_commandGameObjectsRoot))
                    .Select(t => t.gameObject)
                    .ToArray();
            }
        }

        private NovelCommandBase _currentCommand;

        private void Awake()
        {

            var commandsList = new List<NovelCommandBase>(_commandGameObjects.Length + 5);
            foreach (var go in _commandGameObjects)
            {
                go.SetActive(false);
                if (go.TryGetComponent(out VideoPlayer videoPlayer))
                {
                    commandsList.Add(new NovelVideoCommand(this, go));
                }
            }
            
            commandsList.Add(new NextSceneCommand(this, "BossFightTemplate"));
            
            _currentCommand = new MacroNovelCommandBase(this, commandsList.ToArray());
            _currentCommand.Execute();
        }
        
        

        private void Update()
        {
            _currentCommand.UpdateCommand();
        }
    }
}
