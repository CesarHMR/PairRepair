using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHover : MonoBehaviour
{
    public void HoverButton()
    {
        FindObjectOfType<AudioManager>().Play("Hover");
    }

    public void ClickButton()
    {
        FindObjectOfType<AudioManager>().Play("Click");
    }
}
