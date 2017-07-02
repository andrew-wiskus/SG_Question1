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
	[SerializeField] Text m_results_text;

	private DataSet m_dataset = new DataSet();
	private List<BirthToEndDate> m_graph_data;
	private List<GameObject> m_graph_bars = new List<GameObject>();
	private RectTransform m_container_transform;
	private int bar_color_index = 0;

	void OnEnable()
	{
		set_graph_data();
		create_graph();
		set_results_text();
		//This method has to be in a coroutine because we need to wait for the bars position (being controller by horizontal layout group) to be set onto the canvas.
		StartCoroutine(set_initial_bar());
	}

	private IEnumerator set_initial_bar()
	{
		//Goes through all graph bars and finds the first object with a population == mostPeopleAlive;
		GraphBar bar = m_graph_bars.Where(obj => obj.GetComponent<GraphBar>().population == GraphStatistics.mostPeopleAliveAtOnce).ToList()[0].GetComponent<GraphBar>();

		yield return new WaitForEndOfFrame(); //waiting for bars position to be set on canvas;

		bar.SetBarAsActive();
		yield break;
	}

	private void set_graph_data()
	{
		m_graph_data = m_dataset.GetListOfBirthToEndDates();
		GraphStatistics.SetGraphData(m_graph_data);
	}

	private GameObject build_graph_bar_game_object(int current_year, int people_alive)
	{
		//setting parent of new graphbar to container with horizontal layoutgroup
		GameObject graph_bar = GameObject.Instantiate(m_graph_bar_prefab);
		RectTransform graph_bar_rect = graph_bar.GetComponent<RectTransform>();
		graph_bar.transform.SetParent(this.transform);

		//setting height of bar depending on graph_data;
		float bar_height = get_bar_height(current_year, people_alive);
		graph_bar_rect.sizeDelta = new Vector2(graph_bar_rect.sizeDelta.x, bar_height);

		//alternating colors;
		update_color_index();

		return graph_bar;
	}

	private float get_bar_height(int current_year, int people_alive)
	{
		float max_bar_height = m_container_transform.rect.height;
		float percent_of_people_alive = (float)people_alive / (float)m_graph_data.Count;
		float bar_height = Mathf.Lerp(0.0f, max_bar_height, percent_of_people_alive);

		return bar_height;
	}

	private void update_color_index()
	{
		//used to traverse a List<Color> to give graph bars visibile definition from eachother;
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
		int people_alive = GraphStatistics.GetAmountOfPeopleAliveDuringYear(current_year);

		GameObject new_bar = build_graph_bar_game_object(current_year, people_alive);
		m_graph_bars.Add(new_bar);
		set_graph_bar_data(new_bar, current_year, people_alive);
	}

	private void create_graph()
	{
		int bar_count = (m_dataset.endYear - m_dataset.startYear);
		m_container_transform = this.GetComponent<RectTransform>();
		for (int i = 0; i < bar_count; i++) {

			create_new_graph_bar(i);
		}
	}

	private void set_results_text()
	{
		List<int> years = GraphStatistics.yearsWithMostPeopleAlive;
		int amount_of_people_alive = GraphStatistics.mostPeopleAliveAtOnce;

		string year_string = years.Count == 1 ? years[0].ToString() : get_comma_seperated_year_string(years);
		string results_string = "In " + year_string + " there was the most people alive with " + amount_of_people_alive + " living";
		m_results_text.text = results_string;
	}

	private string get_comma_seperated_year_string(List<int> years)
	{
		string result = years[0].ToString();
		for (int i = 1; i < years.Count; i++) {
			result += i + 1 < years.Count ? ", " + years[i] : " and " + years[i];
		}

		return result;
	}
}
