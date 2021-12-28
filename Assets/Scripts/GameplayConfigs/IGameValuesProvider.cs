using UnityEngine;

namespace GameplayConfigs
{
    public interface IGameValuesProvider
    {
        int GetInt(string key);
        float GetFloat(string key);
        string GetString(string key);
        bool GetBool(string key);
        
        Vector2 GetVector2(string key);

        Vector3 GetVector3(string key);

        Color GetColor(string _key);
    }
}
