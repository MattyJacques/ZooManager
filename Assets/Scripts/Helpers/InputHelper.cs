// Title        : InputHelper.cs
// Purpose      : Helper method for input related stuff
// Author       : Eivind Andreassen
// Date         : 21.01.2017
// Note         : This class will be deprecated after the prototype, and
//                the methods should be reorganized into another class

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class InputHelper
    {
        public static bool GetMousePositionInWorldSpace(out Vector3 mousePosition,
            int layerMask = ~0,
            float maximumRangeToCheck = Mathf.Infinity,
            QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            Ray ray = Camera.main.ScreenPointToRay (UnityEngine.Input.mousePosition);
            RaycastHit rayHit = new RaycastHit ();
            if (Physics.Raycast (ray, out rayHit, maximumRangeToCheck, layerMask, QueryTriggerInteraction.Ignore))
            {
                mousePosition = rayHit.point;
                return true;
            }

            mousePosition = Vector3.zero;
            return false;
        }
    }
}
