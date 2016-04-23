using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Square : MonoBehaviour {

	public Text _number = null;

	public void hide ()
	{
		this.gameObject.SetActive (false);
	}

	
	public void show ()
	{
		this.gameObject.SetActive (true);
	}
	public int number {
		//get;
		set{
			_number.text = value.ToString();
		}
	}


}
