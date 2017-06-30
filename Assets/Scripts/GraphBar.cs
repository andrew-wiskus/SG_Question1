using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphBar : MonoBehaviour
{
	[SerializeField] private Text m_bar_text;
	private Image m_bar_image;

	void OnEnable()
	{
		m_bar_image = GetComponent<Image>();
	}

	public void SetBarColor(Color color)
	{
		m_bar_image.color = color;
	}

	public void SetBarText(string text)
	{
		m_bar_text.text = text;
	}
}
