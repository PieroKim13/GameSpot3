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
        //게이지바 받아오기
        slider = GetComponent<Slider>();
        Transform temp;

        //임시 변수로 백그라운드 이미지 받아오기 및 컬러 설정
        temp = transform.GetChild(0);
        Image backgroundImage = temp.GetComponent<Image>();
        backgroundImage.color =  new Color(color.r, color.g, color.b, color.a * 0.75f);

        //임시 변수로 채우기 이미지 받아오기 및 컬러 설정
        temp = transform.GetChild(1);
        Image fillImage = temp.GetComponentInChildren<Image>();
        fillImage.color = color;
    }

    private void Start()
    {
        // 게임 매니저에서 싱글톤 받아오기
        Player player = GameManager.Inst.Player;

        maxValue = player.MaxStamina;
        slider.value = player.Stamina / maxValue;
        player.onStaminaChange += OnValueChange;

    }

    /// <summary>
    /// 스테미나 값이 바뀌었을 때
    /// </summary>
    /// <param name="ratio">비율</param>
    private void OnValueChange(float ratio)
    {
        ratio = Mathf.Clamp01(ratio);               // ratio를 0~1로 변경
        slider.value = ratio;                       // 슬라이더 조정
    }
}
