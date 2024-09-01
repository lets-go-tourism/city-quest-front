using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmpPropReveal : MonoBehaviour
{
    public enum State
    {
        UNREVEAL,       // 미탐험       :                         Quest
        ADVENTURING,    // 탐험 중      :             퀘스트 사진, Quest
        REVEAL          // 탐험 완료    : 업적 미달성, 퀘스트 사진, PlusQuest
    }

    public State state;
}
