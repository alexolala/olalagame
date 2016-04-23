using UnityEngine;
using System.Collections;
using GDGeek;

public class Play : MonoBehaviour {

	public Square _points;

	public Square _phototype;
	public GameObject _root = null;
	private Square[] list_ = null;
	void Awake(){
		list_ = this._root.GetComponentsInChildren<Square> ();
		foreach (Square square in list_) {
			square.hide();
		}
	}

	public Task moveTask (int number, Vector2 begin, Vector2 end)
	{
		Debug.Log ("move");
		Square s = (Square)GameObject.Instantiate (_phototype);
		Square b = this.getSquare((int)(begin.x), (int)(begin.y));
		Square e = this.getSquare((int)(end.x), (int)(end.y));
		Debug.Log (end);
		s.transform.parent = b.transform.parent;
		s.transform.localScale = b.transform.localScale;
		s.transform.localPosition = b.transform.localPosition;
		s.show ();
		s.number = number;
		b.hide ();
		TweenTask tt = new TweenTask (delegate() {
			return TweenLocalPosition.Begin(s.gameObject, 0.5f, e.transform.localPosition);
		});
	
		TaskManager.PushBack (tt, delegate {
			GameObject.DestroyObject(s.gameObject);
		});
		return tt;
	}


//	public Task addTask (int number, Vector2 begin, Vector2 end)
//	{
//		Debug.Log ("move");
//		Square s = (Square)GameObject.Instantiate (_phototype);
//		Square b = this.getSquare((int)(begin.x), (int)(begin.y));
//		Square e = this.getSquare((int)(end.x), (int)(end.y));
//		Debug.Log (end);
//		s.transform.parent = b.transform.parent;
//		s.transform.localScale = b.transform.localScale;
//		s.transform.localPosition = b.transform.localPosition;
//		s.show ();
//		s.number = number;
//		b.hide ();
//		TweenTask tt = new TweenTask (delegate() {
//			return TweenLocalPosition.Begin(s.gameObject, 0.5f, e.transform.localPosition);
//		});
//
//		TaskManager.PushBack (tt, delegate {
//			GameObject.DestroyObject(s.gameObject);
//		});
//
//		s.hide();
//		e.hide();
//
//		if (s.enabled == true) {
//			Debug.Log ("s");
//		} else {
//			Debug.Log ("no s");
//		}
//		return tt;
//	}


	/*
	public Task moveSquareTask (int number, Vector2 begin, Vector2 end)
	{
		Square s = (Square)GameObject.Instantiate (_phototype);
		Square b = this.getSquare((int)(begin.x), (int)(begin.y));
		Square e = this.getSquare((int)(end.x), (int)(end.y));
		
		s.transform.parent = b.transform.parent;
		s.transform.localScale = b.transform.localScale;
		s.transform.localPosition = b.transform.localPosition;
		s.show ();
		s.number = number;
		TweenTask tt = new TweenTask (delegate() {
			return TweenLocalPosition.Begin(s.gameObject, 3.3f, e.transform.localPosition);
		});
		TaskManager.PushFront (tt, delegate {
						Debug.Log ("aa no ~");
				});
		TaskManager.PushBack (tt, delegate {
			GameObject.DestroyObject(s.gameObject);
		});
		return tt;
	}
	*/

	
	public Square getSquare (int x, int y){

		int n = x + y * 4;
		return list_[n];
	}

}
