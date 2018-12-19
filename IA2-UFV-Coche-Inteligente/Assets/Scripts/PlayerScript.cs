using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
	public SpeedoMeterScript needle;
	
	public float oldVelocity;
	public float newVelocity;
	
	void Start()
	{
		needle = FindObjectOfType(typeof(SpeedoMeterScript)) as SpeedoMeterScript;	//Find the needle object in the scene.
	}
	
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
		{
			this.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 1750, ForceMode.Acceleration);	//Apply force in an acceleration state on the object
			oldVelocity = this.GetComponent<Rigidbody>().velocity.magnitude;									//Save its old velocity at the time of pressing forward. So it can lerp Towards its new velocity on the speedmeter.
		}
		
		newVelocity = this.GetComponent<Rigidbody>().velocity.magnitude;			//New velocity, has to update constantly, to lerp from old velocity to fresh newVelocity.
		
		if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			this.transform.Rotate(Vector3.down * Time.deltaTime * 100);		//Rotate towards the right
		}
		
		if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			this.transform.Rotate(Vector3.up * Time.deltaTime * 100);		//Rotate towards the left.
		}
		
		if(newVelocity >= 240)
		{
			newVelocity = 240;		//Cap its maximum speed to 240, so the speedometer wont glitch out of its position.
		}
		
		oldVelocity = Mathf.Lerp(oldVelocity, newVelocity, 10f * Time.deltaTime);	//Interpolate (smoothly) from oldVelocity towards newVelocity with t = 10.
	}
	
	void OnGUI()
	{
		GUI.Label(new Rect(115, 500,100,100), "Tap W to shoot forward, Hold A or D to rotate sideways");
	}
}
