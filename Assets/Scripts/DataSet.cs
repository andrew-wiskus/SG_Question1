using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DataSet : MonoBehaviour
{
	private List<BirthToDeathDate> structured_dataset = new List<BirthToDeathDate>();

	public int startYear { get { return RawData.startYear; } }
	public int endYear { get { return RawData.endYear; } }

	private List<BirthToDeathDate> build_data_set()
	{
		// Get raw unstructured list of int values between 1900 and 2000;
		List<int> random_year_list = RawData.RandomYearList;

		// Seperate list by even/odd indexes
		List<int> odd_index_values = random_year_list.Where((value, index) => index % 2 != 0).ToList();
		List<int> even_index_values = random_year_list.Where((value, index) => index % 2 == 0).ToList();
		Debug.Assert(odd_index_values.Count == even_index_values.Count, "The random_year_list data count isn't even");

		// Create structured dataset using (even[i],odd[i]) where the lower value = birth year && higher value = end year;
		List<BirthToDeathDate> data_set = new List<BirthToDeathDate>();

		for (int i = 0; i < odd_index_values.Count; i++) {
			int odd = odd_index_values[i];
			int even = even_index_values[i];

			BirthToDeathDate data_entry = new BirthToDeathDate();

			data_entry.birthYear = odd < even ? odd : even;
			data_entry.endYear = odd > even ? odd : even;

			data_set.Add(data_entry);

			Debug.Assert(data_entry.birthYear <= data_entry.endYear, "Why is this birth after the end year??");
			// Debug.Log(string.Format("birth: {0}, end: {1}", data_entry.birthYear, data_entry.endYear));
		}
		return data_set;
	}

	public List<BirthToDeathDate> GetListOfBirthToDeathDates()
	{
		if (structured_dataset.Count == 0) {
			structured_dataset = build_data_set();
		}

		return structured_dataset;
	}
}

public struct BirthToDeathDate
{
	public int birthYear;
	public int endYear;
}