using UnityEngine;
using UnityEngine.UI;

public class BarIndicator : MonoBehaviour 
{
	public void Set(float x_pos, int date, int people_alive)
	{
		this.gameObject.SetActive(true);
		this.transform.position = new Vector3(x_pos, this.transform.position.y, this.transform.position.z);
		this.GetComponent<Text>().text = string.Format("{0}: {1} / {2} people alive", date, people_alive, RawData.sampleSize);
	}
}
