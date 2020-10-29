using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class RayOnMap : Map
    {

        private Vector4 MapBoundaries;

        private Camera MainCam;

        private BoxCollider MapCollider;

        private RaycastHit hit;

        private Ray ray;
        

        private void Start()
        {

            MapCollider = gameObject.GetComponent<BoxCollider>();

            MainCam = Camera.main;

        }

        public Nullable<Vector3> MouseRay()
        {
            ray = MainCam.ScreenPointToRay(Input.mousePosition);

            if (MapCollider.Raycast(ray, out hit, 200f))
            {

                if (hit.point.x < MapBoundaries.x || hit.point.x > MapBoundaries.z || hit.point.z < MapBoundaries.y || hit.point.z > MapBoundaries.w)
                {

                    return null;
                }

                return hit.point;

            }

            return null;
        }

        public Nullable<Vector3> HeadRay() 
        {

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            if (MapCollider.Raycast(ray, out hit, 200f))
            {

                if (hit.point.x < MapBoundaries.x || hit.point.x > MapBoundaries.z || hit.point.z < MapBoundaries.y || hit.point.z > MapBoundaries.w)
                {

                    return null;
                }

                return hit.point;

            }

            return null;

        }


    }
}
