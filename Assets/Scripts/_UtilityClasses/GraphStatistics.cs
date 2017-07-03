using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GraphStatistics
{
	public static List<int> yearsWithMostPeopleAlive = new List<int>();
	public static int mostPeopleAliveAtOnce;

	private static List<BirthToEndDate> m_dataset;
	private static List<PopulationForYear> m_population_dataset;

	public static string leftColumn { get { return "LEFT_COLUMN"; }}
			public static string rightColumn { get { return "RIGHT_COLUMN"; }}

	private static List<PopulationForYear> set_population_dataset()
	{
		List<PopulationForYear> results = new List<PopulationForYear>();

		for(int i = RawData.startYear; i <= RawData.endYear; i++)
		{
			PopulationForYear data_entry = new PopulationForYear();
			data_entry.year = i;
			data_entry.population = GetAmountOfPeopleAliveDuringYear(i);
			results.Add(data_entry);
		}

		return results;
	}

	private static void set_graph_statistics() {

		m_population_dataset = set_population_dataset();
		mostPeopleAliveAtOnce = m_population_dataset.Aggregate((first, next) => next.population > first.population ? next : first).population;
		yearsWithMostPeopleAlive = m_population_dataset.Where( data_entry => data_entry.population == mostPeopleAliveAtOnce ).Select( data_entry => data_entry.year).ToList();
	}

	public static string GetStringForAllYears(string column, int year_selected)
	{
		//used in for-loop at end of method
		int index = 0;
		int max = 0;
		string result = "";

		//sets index + max depending on column
		if(column == leftColumn) {
			index = 0;
			max = m_population_dataset.Count / 2;
		} else if(column == rightColumn) {
			index = m_population_dataset.Count / 2;
			max = m_population_dataset.Count - 1;
		} else {
			Debug.Assert(false, "You need to use the pre-defined column identifiers in GetStringForAllYears(string column). Left: GraphStatistics.leftColumn, Right: GraphStatistics.rightColumn");
			return "";
		}

		for(int i = index; i < max; i++) {

			string is_bold_opening = year_selected == m_population_dataset[i].year ? "<b> * " : "";
			string is_bold_closing = year_selected == m_population_dataset[i].year ? "</b>" : "";

			result += is_bold_opening + m_population_dataset[i].year + "- pop: " + m_population_dataset[i].population + is_bold_closing + "\n";
		}

		return result;
	}
	public static void SetGraphData(List<BirthToEndDate> dataset) {
		Debug.Assert(m_dataset == null, "Why are you setting the graph data twice?");
		m_dataset = dataset;
		set_graph_statistics();

	}

	public static int GetAmountOfPeopleAliveDuringYear(int current_year)
	{
		int people_alive = m_dataset.Where(data_entry => data_entry.birthYear <= current_year && data_entry.endYear > current_year).ToList().Count;

		return people_alive;
	}
}

