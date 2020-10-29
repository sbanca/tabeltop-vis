using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TableTop
{

    public class Interaction : Map
    {
      

        private Vector3? point;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {

                point = MapRayOnMap.MouseRay();

                if (point == null) return;

                Vector3 p = (Vector3)point;

                GetPointInfo(p);
                SpanAnnotation(p);

            }
        }

        private async void GetPointInfo(Vector3 point)
        {
          //implement somenthing here
     
        }

        private void SpanAnnotation(Vector3 point) {

           
            MapAnnotations.SpawnAnnotation(point);
        }

        
    }
}




