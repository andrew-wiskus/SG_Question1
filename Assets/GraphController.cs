using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GraphController : MonoBehaviour
{
	[SerializeField] GameObject m_graph_bar_prefab;
	[SerializeField] List<Color> m_graph_colors;
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
		
	private void build_graph_bar(float bar_height)
	{
		GameObject graph_bar = GameObject.Instantiate(m_graph_bar_prefab);
		graph_bar.transform.SetParent(this.transform);
		graph_bar.GetComponent<GraphBar>().SetBarColor(m_graph_colors[bar_color_index]);

		RectTransform graph_bar_rect = graph_bar.GetComponent<RectTransform>();
		graph_bar_rect.sizeDelta = new Vector2(graph_bar_rect.sizeDelta.x, bar_height);

		update_color_index();
	}

	private float get_bar_height(int current_year)
	{
		float max_bar_height = m_rect_transform.rect.height;
		int people_alive = m_graph_data.Where(data_entry => data_entry.birthYear <= current_year && data_entry.endYear > current_year).ToList().Count;
		float percent_of_people_alive = (float)people_alive / (float)m_graph_data.Count;
		float bar_height = Mathf.Lerp(0.0f, max_bar_height, percent_of_people_alive);

		return bar_height;
	}

	private void update_color_index()
	{
		bar_color_index = bar_color_index + 1 >= m_graph_colors.Count ? 0 : bar_color_index + 1;
	}

	public void CreateGraph()
	{
		int bar_count = (m_dataset.endYear - m_dataset.startYear);

		for (int i = 0; i < bar_count; i++) {

			int current_year = RawData.startYear + i;
			float bar_height = get_bar_height(current_year);
			build_graph_bar(bar_height);
		}
	}
}