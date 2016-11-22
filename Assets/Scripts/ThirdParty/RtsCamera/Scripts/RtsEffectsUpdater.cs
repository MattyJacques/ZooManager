using UnityEngine;
using System.Collections;

/// <summary>
/// Convenience script to update various effect targets (if set on Camera).
/// 
/// The provided script only supports depth of field (both 3.4 and 4.0 versions).
/// 
/// To add a new target update, see the comments below.
/// </summary>
[AddComponentMenu("Camera-Control/RtsCamera-RtsEffectsUpdater")]
public class RtsEffectsUpdater : MonoBehaviour
{
    private RtsCamera _ohCam;
   // private DepthOfField34 _dof3;
   // private DepthOfFieldScatter _dof4;
    // ***************************
    // add private variable here
    // ***************************

	void Start ()
	{
        _ohCam = GetComponent<RtsCamera>();
        if (_ohCam == null)
            this.enabled = false;

        //_dof3 = GetComponent<DepthOfField34>();
	    //_dof4 = GetComponent<DepthOfFieldScatter>();
        // ***************************
        // get instance of component here
        // ***************************
    }
	
	void Update () {
        if (_ohCam == null)
            return;

	    var target = _ohCam.CameraTarget;

        /*
	    if(_dof3 != null)
	    {
	        _dof3.objectFocus = target;
	    }

        if(_dof4 != null)
        {
            _dof4.focalTransform = target;
        }*/

        // ***************************
        // if the script is present, update target (or whatever is needed)
        // ***************************

	}
}
