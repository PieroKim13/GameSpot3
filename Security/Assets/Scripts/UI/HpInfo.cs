using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpInfo : MonoBehaviour
{
    public Image heartBeat;
    public TextMeshProUGUI Infotext;

    private void Awake()
    {
        heartBeat = transform.GetChild(0).GetComponent<Image>();

        Infotext = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = GameManager.Inst.Player;
        Infotext.text = player.HP.ToString() + "%";

        player.onHpChange += OnValueChange;
    }

    /// <summary>
    /// 체력 값이 바뀌었을 때
    /// </summary>
    /// <param name="value">바뀐 값</param>
    private void OnValueChange(float value)
    {
        Infotext.text = Mathf.FloorToInt(value) + "%";
    }
}
