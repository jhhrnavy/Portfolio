using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : PersistentSingleton<MissionManager>
{
    public MissionData[] missionDatas;
    public int selectMission;

    private void Start()
    {
        selectMission = -1;
    }
    public void SelectMission(int num)
    {
        selectMission = num;
    }

    public void LoadLevel()
    {
        if(selectMission == -1)
        {
            Debug.Log("Select Mission");
            return;
        }
        SceneManager.LoadScene(missionDatas[selectMission].sceneIndex, LoadSceneMode.Single);
        SceneManager.LoadSceneAsync(1,LoadSceneMode.Additive); // UI Scene
    }

    public void SetCurrentMissionClearTime(float time)
    {
        missionDatas[selectMission].cleartime = time;
    }

    public MissionData GetCurrentMissionData()
    {
        return missionDatas[selectMission];
    }
}

[System.Serializable]
public struct MissionData
{
    [Min(2)] public int sceneIndex;
    public int reward;
    public float cleartime;

}
