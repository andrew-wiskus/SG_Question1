using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphBar : MonoBehaviour
{
	[SerializeField] private Text m_bar_text;
	private Image m_bar_image;
	private int m_year;
	private int m_population;
	private GameObject m_indicator_object;

	public int population { get { return m_population; } }
	public int year { get { return m_year; } }

	void OnEnable()
	{
		m_bar_image = GetComponent<Image>();
		this.GetComponent<Button>().onClick.AddListener(clicked_bar);
	}
	
	private void clicked_bar()
	{
		SetBarAsActive();
	}
	public void SetIndicatorObject(GameObject indicator)
	{
		m_indicator_object = indicator;
	}
	public void SetBarColor(Color color)
	{
		m_bar_image.color = color;
	}

	public void SetBarData(int year, int people_alive)
	{
		m_year = year;
		m_population = people_alive;
	}

	public void SetBarAsActive()
	{
		m_indicator_object.GetComponent<BarIndicator>().Set(this.transform.position.x, m_year, m_population);
	}
}
