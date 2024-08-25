using UnityEngine;

[SerializeField]
public class PropInfo
{
    public string name;
    public int difficulty;
    public float distance;
    public string place;
    public string address;
    public int normalQuest;
    public string nqText;
    public int plusQuest;
    public string pqText;
}

public class SettingPropInfo : MonoBehaviour
{
    public static SettingPropInfo instance;
    private void Awake()
    {
        instance = this;
    }

    public void Setting()
    {

    }
}
