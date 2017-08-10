using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraPostRender : MonoBehaviour
{
	void OnPostRender()
	{
		// Set your materials
		GL.PushMatrix();
		// yourMaterial.SetPass( );
		// Draw your stuff
		GL.PopMatrix();
	}
}

