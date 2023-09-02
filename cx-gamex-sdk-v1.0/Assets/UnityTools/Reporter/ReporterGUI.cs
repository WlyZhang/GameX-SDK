using UnityEngine;
using System.Collections;

public class ReporterGUI : MonoBehaviour
{
	public static bool OpenLOG = false;

	Reporter reporter;
	void Awake()
	{
		reporter = gameObject.GetComponent<Reporter>();
	}


	float _time = 0;
	void OnGUI()
	{
       // if (OpenLOG)
            reporter.OnGUIDraw();

    }
}
