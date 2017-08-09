using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBlueController : MonoBehaviour 
{
	public GameObject target = null;

	private bool m_FacingRight = true;


	void Start () 
	{
		
	}
	
	void Update () 
	{
		if (target != null) {
		
			if (transform.position.x > target.transform.position.x) {
			
				if (m_FacingRight) {
				
					Flip ();
				}

			} else {
			
				if (!m_FacingRight) {

					Flip ();
				}

			
			}

		}
	}






	private void Flip()
	{
		m_FacingRight = !m_FacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
