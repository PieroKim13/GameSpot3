using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConsumable
{
    /// <summary>
    /// 아이템을 소비하는 함수
    /// </summary>
    /// <param name="target">아이템의 효과를 받을 대상</param>
    void Consume(GameObject target);
}
