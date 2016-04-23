using UnityEngine;
using System.Collections;
using GDGeek;

public class Ctrl : MonoBehaviour {
	private FSM fsm_ = new FSM();
	public View _view = null;
	public Model _model = null;
	static int score = 0;



	public void fsmPost(string msg){
		fsm_.post (msg);
	}
	State beginState ()
	{
		StateWithEventMap state = new StateWithEventMap ();
		state.onStart += delegate {
			_view.begin.gameObject.SetActive(true);
		};
		state.onOver += delegate {
			_view.begin.gameObject.SetActive(false);
		};

		state.addEvent("begin", "level");
		state.addEvent ("about", "about");
		return state;
	}

	State levelState ()
	{
		StateWithEventMap state = new StateWithEventMap ();
		state.onStart += delegate {
			_view.level.gameObject.SetActive(true);
		};
		state.onOver += delegate {
			_view.level.gameObject.SetActive(false);
		};
			
		state.addEvent("level1", "input");
		state.addEvent ("back", "begin");

		return state;
	}

	State aboutState ()
	{
		StateWithEventMap state = new StateWithEventMap ();
		state.onStart += delegate {
			_view.about.gameObject.SetActive(true);
		};
		state.onOver += delegate {
			_view.about.gameObject.SetActive(false);
		};

		state.addEvent("back", "begin");
		return state;
	}

	public void refreshModel2View (){
		for (int x =0; x <_model.width; ++x) {
			for(int y = 0; y<_model.height; ++y){
				Square s = _view.play.getSquare(x, y);
				Cube c = _model.getCube(x, y);
				if(c.isEnabled){
					s.number = c.number;
					s.show();
				}else{
					s.hide();
				}
			}		
		}
	}

	public void refreshPoints2View (){
		_model.points = Points.number;
//		_view.play.points = Points.number;

	}

	State playState ()
	{

		
		StateWithEventMap state = new StateWithEventMap ();
		state.addEvent ("back", "begin");
		state.onStart += delegate {
			refreshModel2View();
		
		};
		state.onOver += delegate {
			_view.play.gameObject.SetActive(false);
		};



		return state;
	}

	State endState ()
	{
		StateWithEventMap state = new StateWithEventMap ();
		state.addEvent("end", "begin");
		state.onStart += delegate {
			_view.end.gameObject.SetActive(true);
		};
		state.onOver += delegate {
			_view.end.gameObject.SetActive(false);
		};
		return state;
	}
	private void input(int x, int number){
		Cube c = _model.getCube(1, 0);
		c.isEnabled = false;
		c = _model.getCube(x, 0);
		c.number = number;
		c.isEnabled = true;

		refreshModel2View ();
	}

	State inputState ()
	{
		

		StateWithEventMap state = new StateWithEventMap ();
		state.addEvent ("back", "begin");

		int number = 0;

		state.onStart += delegate {
			number = Random.Range(0, 5);
			number = (int)Mathf.Pow(2,number);
			Cube c = _model.getCube(1, 0);
			c.number = number;
			c.isEnabled = true;
			refreshModel2View ();
			refreshPoints2View();
			Debug.Log("alex");
			Debug.Log (Points.number);

		};
		state.addAction("1", delegate(FSMEvent evt) {
//			Debug.Log ("I get one~");
			input(0, number);
			return "fall";
		});

		
		state.addAction("2", delegate(FSMEvent evt) {
//			Debug.Log ("I get two~");
			input(1, number);
			return "fall";
		});


		
		state.addAction("3", delegate(FSMEvent evt) {
//			Debug.Log ("I get 3~");
			input(2, number);
			return "fall";
		});


		
		state.addAction("4", delegate(FSMEvent evt) {
//			Debug.Log ("I get 4~");
			input(3, number);
			return "fall";
		});
		return state;
	}

	private Task doFallTask ()
	{

		TaskSet ts = new TaskSet ();

		for (int x =0; x < _model.width; ++x) {
			for(int y = _model.height - 1; y >= 0; --y){
				
				Cube c = _model.getCube(x, y);
				Cube end = null;
				int endY = 0;
				if(c.isEnabled){
					for(int n = y+1; n < _model.height; ++n){
						Cube fall = _model.getCube(x, n);
						if(fall == null || fall.isEnabled == true){
							break;
						}else{
							end = fall;
							endY = n;
						}
					}
					if(end != null)
					{
						end.number = c.number;
						end.isEnabled = true;
						c.isEnabled = false;
						ts.push (_view.play.moveTask(c.number, new Vector2(x, y), new Vector2(x, endY)));
						//c = end;
					}
				}

			
			}		
		}
		TaskManager.PushBack (ts, delegate() {
			refreshModel2View ();
				});
		return ts;
	}

	private State fallState ()
	{
		StateWithEventMap state = TaskState.Create(delegate {

			Task fall = doFallTask();
			/*TaskWait tw = new TaskWait();
			tw.setAllTime(0.5f);

			TaskManager.PushBack(tw, delegate {
				doFall();
			});*/
			return fall;
		}, fsm_, "remove");


		state.onStart += delegate {
			
//			Debug.LogWarning("in fall!");

		};
		return state;
	}

	bool checkAndRemove ()
	{
		bool s = false;

		for(int x =0; x <4; ++x){
			Cube c = _model.getCube (x, 2);
			if (c.isEnabled == true) {
				_view.end.gameObject.SetActive(true);
				_view.play.gameObject.SetActive(false);


				Debug.Log ("game over");
				for(int i=0; i<4; i++){
					for (int j = 0; j < 8; j++) {
						Cube endc = _model.getCube(i, j);
						endc.isEnabled = false;
						endc.number = 0;
					}
				}
			}
		}

		for (int x =0; x <_model.width; ++x) {
			for(int y = 0; y<_model.height; ++y){

				Cube c = _model.getCube(x, y);
				if(c.isEnabled == true){
					Cube up = _model.getCube (x, y-1);
					Cube down = _model.getCube (x, y+1);
					Cube left = _model.getCube (x-1, y);
					Cube right = _model.getCube (x+1, y);



					if (up != null && up.isEnabled == true && up.number + c.number == 256) {
						score += 1;
						Points.number += 1;
						c.isEnabled = false;
						up.isEnabled = false;
						s = true;
						break;
					} else if(up != null && up.isEnabled == true && up.number == c.number) {
						c.number = c.number + up.number;


						up.isEnabled = false;



//						Debug.Log ("up");

						s = true;
						continue;
					}


					if(down != null && down.isEnabled == true && down.number + c.number == 256){
						Points.number += 1;
						c.isEnabled = false;
						down.isEnabled = false;
						s = true;
						break;
					}else if(down != null && down.isEnabled == true && down.number == c.number) {


						Cube right2 = _model.getCube (x+1, y);
//
//						Debug.Log ("x,y,down");
//						Debug.Log (x);
//						Debug.Log (y);
//
//						Debug.Log ("right2");
//						Debug.Log (right2.number);
//						Debug.Log ("left2");
//						Debug.Log (left.number);
//						Debug.Log ("c");
//						Debug.Log (c.number);



						if (right2 != null && right2.isEnabled == true && c.number == right2.number) {
							Debug.Log ("left = right");

							right2.number = c.number + right2.number;
						}

						Cube left2 = _model.getCube (x-1, y);

						if (left2 != null && left2.isEnabled == true && c.number == left2.number) {
							Debug.Log ("left = right");

							left2.number = c.number + left2.number;
						}

						c.number = c.number + down.number;
						down.isEnabled = false;


						Debug.Log ("down");

						s = true;
						break;
					}




					if(left != null && left.isEnabled == true && left.number + c.number == 256){
						score += 1;
						Points.number += 1;

						c.isEnabled = false;
						left.isEnabled = false;
						s = true;
						break;
					}
					else if(left != null && left.isEnabled == true && left.number == c.number) {




						c.number = c.number + left.number;

						left.number = 0;
						left.isEnabled = false;

						Debug.Log ("left");

						Cube left2 = _model.getCube (x-2, y);

						//						Debug.Log ("right2");
						//						Debug.Log (right2.number);
						////						Debug.Log ("left");
						////						Debug.Log (left.number);
						//						Debug.Log ("c");
						//						Debug.Log (c.number);



						if (left2 != null && left2.isEnabled == true && c.number == left2.number) {
							Debug.Log ("left = right");

							left2.number = c.number + left2.number;
						}

						Cube down2 = _model.getCube (x+1, y+1);

						if (down2 != null && down2.isEnabled == true && c.number == down2.number) {
							Debug.Log ("left = right");

							down2.number = c.number + down2.number;
						}




//						s = true;
						break;
					}


					if(right != null && right.isEnabled == true && right.number + c.number == 256){
						score += 1;
						Points.number += 1;

						c.isEnabled = false;
						right.isEnabled = false;
						s = true;
						break;
					}
					else if(right != null && right.isEnabled == true && right.number == c.number) {

						Cube right2 = _model.getCube (x+2, y);

//						Debug.Log ("right2");
//						Debug.Log (right2.number);
////						Debug.Log ("left");
////						Debug.Log (left.number);
//						Debug.Log ("c");
//						Debug.Log (c.number);



						if (right2 != null && right2.isEnabled == true && c.number == right2.number) {
							Debug.Log ("left = right");

							right2.number = c.number + right2.number;
						}

						Cube down2 = _model.getCube (x+1, y+1);

						if (down2 != null && down2.isEnabled == true && c.number == down2.number) {
							Debug.Log ("left = right");

							down2.number = c.number + down2.number;
						}


						c.number = c.number + right.number;
//						TaskSet ts = new TaskSet ();
////						c.isEnabled = false;
//						ts.push (_view.play.addTask(c.number, new Vector2(x, y), new Vector2(x+1, y)));
//						c.isEnabled = false;
						right.isEnabled = false;
						right.number = 0;
//
//
//
						Debug.Log ("right");

						s = true;
						break;
					}




				}
			}		
		}
		refreshModel2View ();



		return s;
	}

	State removeState ()
	{
		bool s = false;
		StateWithEventMap state = TaskState.Create(delegate {
			Task task = new Task();
			TaskManager.PushFront(task, delegate {
				s = checkAndRemove();
			});
			return task;
		}, fsm_, 
		delegate {
			if(s){
				return "fall";
			}else{
				return "input";
			}});
		state.onStart += delegate {
//			Debug.LogWarning("in remove!");
		};
		return state;
	}



	// Use this for initialization
	void Start () {
		fsm_.addState ("begin", beginState());
		fsm_.addState ("play", playState ());
		fsm_.addState ("level", levelState ());
		fsm_.addState ("about", aboutState ());

		fsm_.addState ("input", inputState(), "play");
		fsm_.addState ("fall", fallState(), "play");
		fsm_.addState ("remove", removeState(), "play");

		fsm_.addState ("end", endState ());
		fsm_.init ("begin");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
