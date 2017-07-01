using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GraphController : MonoBehaviour
{
	[SerializeField] GameObject m_graph_bar_prefab;
	[SerializeField] List<Color> m_graph_colors;
	[SerializeField] GameObject m_indicator;

	private DataSet m_dataset = new DataSet();

	private List<BirthToDeathDate> m_graph_data;
	private RectTransform m_rect_transform;
	private int bar_color_index = 0;

	public void OnEnable()
	{
		set_private_variables();
		CreateGraph();
	}

	private void set_private_variables()
	{
		m_rect_transform = this.GetComponent<RectTransform>();
		m_graph_data = m_dataset.GetListOfBirthToDeathDates();
	}
		
	private GameObject build_graph_bar_game_object(int current_year, int people_alive)
	{
		GameObject graph_bar = GameObject.Instantiate(m_graph_bar_prefab);
		RectTransform graph_bar_rect = graph_bar.GetComponent<RectTransform>();
		graph_bar.transform.SetParent(this.transform);

		float bar_height = get_bar_height(current_year, people_alive);
		graph_bar_rect.sizeDelta = new Vector2(graph_bar_rect.sizeDelta.x, bar_height);

		update_color_index();
		return graph_bar;
	}

	private float get_bar_height(int current_year, int people_alive)
	{
		float max_bar_height = m_rect_transform.rect.height;
		float percent_of_people_alive = (float)people_alive / (float)m_graph_data.Count;
		float bar_height = Mathf.Lerp(0.0f, max_bar_height, percent_of_people_alive);

		return bar_height;
	}

	private void update_color_index()
	{
		bar_color_index = bar_color_index + 1 >= m_graph_colors.Count ? 0 : bar_color_index + 1;
	}

	private void set_graph_bar_data(GameObject new_bar, int current_year, int people_alive)
	{
		GraphBar graph_bar = new_bar.GetComponent<GraphBar>();
		graph_bar.SetBarColor(m_graph_colors[bar_color_index]);
		graph_bar.SetIndicatorObject(m_indicator);
		graph_bar.SetBarData(current_year, people_alive);
	}

	private void create_new_graph_bar(int i)
	{
		int current_year = RawData.startYear + i;
		int people_alive = m_graph_data.Where(data_entry => data_entry.birthYear <= current_year && data_entry.endYear > current_year).ToList().Count;

		GameObject new_bar = build_graph_bar_game_object(current_year, people_alive);
		set_graph_bar_data(new_bar, current_year, people_alive);
	}

	public void CreateGraph()
	{
		int bar_count = (m_dataset.endYear - m_dataset.startYear);

		for (int i = 0; i < bar_count; i++) {

			create_new_graph_bar(i);
		}
	}
}