using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaInfo : MonoBehaviour
{
    public Color color = Color.white;

    private Slider slider;
    private float maxValue;

    private void Awake()
    {
        //�������� �޾ƿ���
        slider = GetComponent<Slider>();
        Transform temp;

        //�ӽ� ������ ��׶��� �̹��� �޾ƿ��� �� �÷� ����
        temp = transform.GetChild(0);
        Image backgroundImage = temp.GetComponent<Image>();
        backgroundImage.color =  new Color(color.r, color.g, color.b, color.a * 0.75f);

        //�ӽ� ������ ä��� �̹��� �޾ƿ��� �� �÷� ����
        temp = transform.GetChild(1);
        Image fillImage = temp.GetComponentInChildren<Image>();
        fillImage.color = color;
    }

    private void Start()
    {
        // ���� �Ŵ������� �̱��� �޾ƿ���
        Player player = GameManager.Inst.Player;

        maxValue = player.MaxStamina;
        slider.value = player.Stamina / maxValue;
        player.onStaminaChange += OnValueChange;

    }

    /// <summary>
    /// ���׹̳� ���� �ٲ���� ��
    /// </summary>
    /// <param name="ratio">����</param>
    private void OnValueChange(float ratio)
    {
        ratio = Mathf.Clamp01(ratio);               // ratio�� 0~1�� ����
        slider.value = ratio;                       // �����̴� ����
    }
}
