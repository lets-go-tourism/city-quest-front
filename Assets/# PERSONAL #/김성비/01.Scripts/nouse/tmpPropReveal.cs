using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmpPropReveal : MonoBehaviour
{
    public enum State
    {
        UNREVEAL,       // ��Ž��       :                         Quest
        ADVENTURING,    // Ž�� ��      :             ����Ʈ ����, Quest
        REVEAL          // Ž�� �Ϸ�    : ���� �̴޼�, ����Ʈ ����, PlusQuest
    }

    public State state;
}
