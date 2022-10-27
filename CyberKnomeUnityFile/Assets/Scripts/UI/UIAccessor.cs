using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAccessor : MonoBehaviour
{
    public Image img;
    public TMP_Text txt;
    public GameObject highlight;

    public void SetHighlight(bool highlightThis)
	{
        highlight.SetActive(highlightThis);
	}
}
