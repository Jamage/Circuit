using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSelectionHighlight : MonoBehaviour
{
    SpriteRenderer highlightSprite;
    private Color transparentColor = new Color(1, 1, 1, 0);
    private Color selectedColor = new Color(1, 1, 1, .2f);
    bool isOn = false;

    private void Awake()
    {
        highlightSprite = GetComponent<SpriteRenderer>();
    }

    public void MouseEntered()
    {
        if (isOn == false)
        {
            highlightSprite.color = selectedColor;
            isOn = true;
        }
    }

    public void MouseExited()
    {
        if (isOn)
        {
            highlightSprite.color = transparentColor;
            isOn = false;
        }
    }
}
