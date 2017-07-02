using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GraphStatistics
{
	public static List<int> yearsWithMostPeopleAlive = new List<int>();
	public static int mostPeopleAliveAtOnce;

	private static List<BirthToEndDate> m_dataset;
	private static List<PopulationForYear> m_population_dataset;

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

	public struct PopulationForYear {
		public int year;
		public int population;
	}
}

