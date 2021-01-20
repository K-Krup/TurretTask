using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerCouterUi : MonoBehaviour
{
	Text text;

	private void Awake()
	{
		text = GetComponent<Text>();
	}
	void Update()
    {
		text.text = "Wieżyczki: " + TurretLogic.GetTurretCout();

	}
}
