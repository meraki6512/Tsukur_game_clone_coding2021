using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCMove
{
    [Tooltip("NPCMove를 체크하면 NPC가 움직임")]
    public bool NPCmove;
    public string[] direction;
    [Range(1, 5)]
    public int frequency;
}

public class NPCManager : MovingObject {

    [SerializeField]
    public NPCMove npc;

    // Use this for initialization
    void Start () {
        queue = new Queue<string>(); //큐 초기화
    }

    public void SetMove()
    {
        StartCoroutine(MoveCoroutine());
    }
    public void SetNotMove()
    {
        StopAllCoroutines();
    }

    IEnumerator MoveCoroutine()
    {
        if (npc.direction.Length != 0)
        {
            for (int i = 0; i < npc.direction.Length; i++)
            {

                yield return new WaitUntil(() => queue.Count < 2); //큐에 1개의 값만 들어가고 MovingObject에서 De큐되기 때문에 걸음이 멈추는 현상 발생, 따라서 조건을 npcCanMove => queue.Count < 2로 변경
                //실질이동 구간
                base.Move(npc.direction[i], npc.frequency);

                if (i == npc.direction.Length - 1)
                    i = -1;

            }
        }
    }

}
