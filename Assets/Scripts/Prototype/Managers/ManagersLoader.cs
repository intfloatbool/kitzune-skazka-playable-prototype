using UnityEngine;

public class ManagersLoader : MonoBehaviour
{
    private static bool IsManagersLoaded = false;
    [SerializeField] private GameObject _managersPrefab;

    private void Awake()
    {
        if (!IsManagersLoaded)
        {
            var managersObj = Instantiate(_managersPrefab);
            DontDestroyOnLoad(managersObj);
            IsManagersLoaded = true;
        }
    }
}
