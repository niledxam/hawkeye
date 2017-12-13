using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;

public class MainAnimation : MonoBehaviour {
	public GameObject trace;
	public Button firstAnim;
	private float rebondX;
	private float rebondY=0.01f;
	private float rebondZ;

	private float diffXGauche = 274.702f;
	private float diffXDroite = -282.701f;
	private float diffY = 99.99f;
	private float diffZ = 0f;

	private float thrustX=18f;
	private float thrustY=6f;
	private float thrustZ;

	private float angleZ;

	private float angleDegre;

	private float csteDeplacementAngle = 15.5f;

	private bool gauche=false;

	private GameObject target;
	private Rigidbody rb;
	private bool moveCamera;

	private int counter=0;
	private int nb_line=0;

	// Use this for initialization
	void Start () {
		Button return_firstAnim = firstAnim.GetComponent<Button>();
        return_firstAnim.onClick.AddListener(Return2Dview);

		rb = GetComponent<Rigidbody> ();

		readFile ();		
		setThrust (angleDegre);

		if(gauche){
			initVariables(diffXGauche);
			initCamera();
		} else {
			initVariables(diffXDroite);
			initCamera();
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		//Debug.Log("Counter : " + counter);
		if(gauche) {
			rb.AddForce(-thrustX,-thrustY,thrustZ);
		} else {
			rb.AddForce(thrustX,-thrustY,thrustZ);
		}

		//Block the ball 
		if (transform.position.x < 100f ||transform .position.x > 1200f) {
			transform.position = new Vector3 (100f, transform.position.y, transform.position.z);
			rb.Sleep ();
		}

		if(moveCamera) {
			if(gauche){
				if (Camera.main.transform.position.x > (rebondX+52.6) && Camera.main.transform.position.y > (rebondY+15.09) && Camera.main.transform.position.z < (rebondZ-28.7)) {
					Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x - 4f, Camera.main.transform.position.y - 0.95f, Camera.main.transform.position.z + 2.1f);
				} //else {
		//			//RotateCamera ();
		//		}
			} else {
				if (Camera.main.transform.position.x < (rebondX-52.6) && Camera.main.transform.position.y > (rebondY+15.09) && Camera.main.transform.position.z > (rebondZ+28.7)) {
					Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x + 4f, Camera.main.transform.position.y - 0.95f, Camera.main.transform.position.z - 2.1f);
				} //else {
				// 	RotateCamera ();
				// }
			}
		}
	}

	void initVariables(float k){
		angleZ = thrustZ * csteDeplacementAngle;
		transform.position = new Vector3 (rebondX + k, rebondY + diffY, rebondZ + diffZ - angleZ);
		Debug.Log ("Angle : " + getAngleDegre(rebondX, rebondZ, transform.position.x, transform.position.z) + " degrés");
		Debug.Log ("Thrust : " + thrustZ);
		Debug.Log ("");
	}

	void initCamera(){
		if (gauche) {
			Camera.main.transform.position = new Vector3(rebondX+497.4f,rebondY+121.39f,rebondZ-260.5f);
			Camera.main.transform.eulerAngles = new Vector3(15f, -70f, 0f);
		} else {
			Camera.main.transform.position = new Vector3(rebondX-497.4f,rebondY+121.39f,rebondZ+260.5f);
			Camera.main.transform.eulerAngles = new Vector3(15f, 110f, 0f);
		}
	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "Court") {

			target = Instantiate(trace, new Vector3 (transform.position.x+4f, 0.05f, transform.position.z), transform.rotation);
			target.transform.eulerAngles = new Vector3 (0f, 0f, 0f);
			moveCamera = true;
			//Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y,Camera.main.transform.position.z);
		}
	}

	void RotateCamera() {
		if (Camera.main.transform.eulerAngles.x < 89) {
			Camera.main.transform.RotateAround (target.transform.position, new Vector3 (0.0f, 0.0f, 1.0f), 50f * Time.deltaTime);
		}
	}

	void ZoomCamera() {
		if (Camera.main.transform.position.y > 8) {
			Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y - 0.1f, transform.position.z);
		}
	}

	float getAngleDegre(float x1, float y1, float x2, float y2){
		float angleRad = Mathf.Atan ((y2 - y1) / (x2 - x1));
		angleDegre = angleRad * Mathf.Rad2Deg;
		return angleDegre;
	}

	void setThrust(float angle){
		thrustZ = angle / 3.229477f;
	}

	void readFile(){
		//Read in /dev/stdin
		TextReader reader;
		TextWriter writer;

		//string fileName = "/dev/stdin";
		string fileName = "./Rebond.txt";
		string fileWriterName = "./TestPipe.log";

		writer = new StreamWriter(fileWriterName);
		writer.WriteLine("NEW ");
		writer.WriteLine("Lecture du fichier " + fileName);

		char[] delimiter = { ',' };

		reader = new StreamReader(fileName);
		string floatString = reader.ReadLine();
		string[] fields = floatString.Split(delimiter);
		rebondX = (float)Convert.ToDouble(fields [0]);
		rebondZ = (float)Convert.ToDouble(fields [1]);
		angleDegre = (float)Convert.ToDouble(fields [2]);

		writer.WriteLine("Coordonnée du rebond en X : " + fields[0]);
		writer.WriteLine("Coordonnée du rebond en Y : " + fields[1]);
		writer.WriteLine("Degré de l'angle : " + fields[2]);

		reader.Close();	
		writer.WriteLine("Fermeture du fichier " + fileName);

		File.Delete(fileName);		
		writer.WriteLine("Suppression du fichier " + fileName);
		
		writer.Close();

		if(rebondX < 648){
			gauche=true;
		}
	}

	private void Return2Dview () {
		SceneManager.LoadScene("FirstAnimation");
	}
}