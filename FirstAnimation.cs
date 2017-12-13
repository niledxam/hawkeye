using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class FirstAnimation : MonoBehaviour {
	//Buttons
	public Button review;
	public Button resume;
	public Button stop;

	//GameObjects
	public GameObject ball;
	private GameObject trace;

	//Données de InputField
	public InputField inputField;
	private String num;
	private int intNum;

	//Données lu sur le stdin
	private float pipe_rebondX;
	private float pipe_rebondY;
	private float pipe_angle;
	private float oldX;
	private float oldY;
	private float oldAngle;

	//Données aléatoires
	private float rnd_rebondX;
	private float rnd_rebondY;
	private float rnd_angle;

	//Compteur
	private int i = 0;

	//Affichage d'un maximum de rebonds
	public int canvas_nbRebonds = 5;

	//Définition des tableaux de données
	private GameObject[] tableau_rebonds;
	private float[] tableau_angle;

	//Flags
	private int counter=0;
	private bool bool_stop=false;
	private bool firstValues=false;

	// Use this for initialization
	void Start () {
		Log("Log.log","Start()");
		tableau_rebonds = new GameObject[50000];
		tableau_angle = new float[50000];

		Button btnReview = review.GetComponent<Button>();
        btnReview.onClick.AddListener(OnClickReview);
		
		Button btnStop = stop.GetComponent<Button>();
        btnStop.onClick.AddListener(OnClickStop);

		Button btnResume = resume.GetComponent<Button>();
        btnResume.onClick.AddListener(OnClickResume);

		
	}
	
	// Update is called once per frame
	void Update () {
		// counter++;
		// if (bool_stop == false && counter%100 == 0) {
			if(bool_stop == false) {
				InstantiateTrace();
		 	}

		// 	rnd_rebondX = UnityEngine.Random.Range(172, 1123);
		// 	rnd_rebondY = UnityEngine.Random.Range(-145, -584);
		// 	rnd_angle = UnityEngine.Random.Range(-40, 40);

		// 	Debug.Log(rnd_rebondX+" "+rnd_rebondY+" "+rnd_angle);

		// 	WriteToLog("./Test.txt", rnd_rebondX, rnd_rebondY, rnd_angle);
		// }
	}

    void OnClickReview()
    {
		Log("Log.log","OnClickStart()");
		float animation_rebondX = tableau_rebonds[intNum].transform.position.x;
		float animation_rebondY = tableau_rebonds[intNum].transform.position.z;
		float animation_angle = tableau_angle[intNum];
		Debug.Log("animation_rebondX : " + animation_rebondX);
		Debug.Log("animation_rebondY : " + animation_rebondY);
		Debug.Log("animation_angle : " + animation_angle);

		WriteToLog("./Rebond.txt", animation_rebondX, animation_rebondY, animation_angle);

		SceneManager.LoadScene("MainAnimation");
    }

    void OnClickStop()
    {		
		Log("Log.log","OnClickStop()");
		bool_stop=true;
		stop.gameObject.SetActive(false);
		resume.gameObject.SetActive(true);
    }

	void OnClickResume()
	{
		Log("Log.log","OnClickStop()");
		bool_stop=false;
		stop.gameObject.SetActive(true);
		resume.gameObject.SetActive(false);
	}

	void InstantiateTrace() {
		Log("Log.log","ShowText()");
		TextReader reader;
		//string fileName = "/dev/stdin";
		string fileName = "./fifo";
		char[] delimiter = { ',' };
		reader = new StreamReader(fileName);
		string floatString = reader.ReadLine();
		string[] fields = floatString.Split(delimiter);

		pipe_rebondX = (float)Convert.ToDouble(fields [0]);
		pipe_rebondY = (float)Convert.ToDouble(fields [1]);
		pipe_angle = (float)Convert.ToDouble(fields [2]);

		// Debug.Log("pipe_rebondX : " + pipe_rebondX + " -- " + "oldX : " + oldX);
		// Debug.Log("pipe_rebondY : " + pipe_rebondY + " -- " + "oldY : " + oldY);
		// Debug.Log("pipe_angle : " + pipe_angle + " -- " + "oldAngle : " + oldAngle);	

		if((pipe_rebondX != oldX) || (pipe_rebondY != oldY) || (pipe_angle != oldAngle)) {
			Debug.Log("if");
			reader.Close();	

			trace = Instantiate(ball, new Vector3 (pipe_rebondX, 1f, pipe_rebondY), Quaternion.identity);
			tableau_rebonds[i] = trace;
			tableau_angle[i] = pipe_angle;

			if (i>=canvas_nbRebonds) {
				tableau_rebonds[i-canvas_nbRebonds].SetActive(false);
			}
		
			i++;
		} else {
			Debug.Log("else");
			reader.Close();
		}

		oldX = pipe_rebondX;
		oldY = pipe_rebondY;
		oldAngle = pipe_angle;
	}

	public void Text_Changed()
    {
		Log("Log.log","Text_Changed()");
        num = inputField.text;
		if(int.TryParse(num, out intNum)) {
			Debug.Log("Num : " + num);
		} else {
			Debug.Log("no way");
		}
        
        //cube.transform.localScale = new Vector3(temp, temp, temp);
    }

	public void Log(String fn, String content) {
		TextWriter writer;	
		string fileName = fn;
		writer = new StreamWriter(fileName, true);
		writer.WriteLine(content);
		writer.Close();
	}

	public void WriteToLog(String fn, float x, float y, float a) {
		Log("Log.log","WriteToLog()");
		TextWriter writer;	
		string fileName = fn;
		writer = new StreamWriter(fileName);
		writer.WriteLine(x + "," + y + "," + a);
		writer.Close();
	}
}



		

		
		

	