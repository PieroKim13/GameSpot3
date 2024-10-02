using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Interaction : MonoBehaviour
{
    Image crossHead;
    public Sprite[] icons;

    public Action invisibleUI;
    public Action visibleUI;

    private void Awake()
    {
        Image crossHead = GetComponent<Image>();
        icons = GetComponents<Sprite>();
        invisibleUI = OnInVisible;
        visibleUI = OnVisible;
    }

    public void OnInVisible()
    {
        invisibleUI?.Invoke();
        crossHead.sprite = icons[1];
    }

    public void OnVisible()
    {
        visibleUI?.Invoke();
        crossHead.sprite = icons[0];
    }
}
