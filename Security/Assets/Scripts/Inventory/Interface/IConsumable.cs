using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConsumable
{
    /// <summary>
    /// �������� �Һ��ϴ� �Լ�
    /// </summary>
    /// <param name="target">�������� ȿ���� ���� ���</param>
    void Consume(GameObject target);
}
