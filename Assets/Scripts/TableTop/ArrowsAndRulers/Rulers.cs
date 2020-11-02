using UnityEngine;

namespace TableTop
{
    public class Rulers : MonoBehaviour
    {

        //public variables

        public float rulerdistance = 0.05f;

        public Vector2 RangeticksX = new Vector2(0f,10f);

        public Vector2 RangeticksY = new Vector2(10f,20f);


        //priavet variables

        private Bounds TileBounds;

        private int TicksnumberX;

        private int TicksnumberY;

        public GameObject[] rulers = new GameObject[4];



        private GameObject MapUIParent;

        public Map map;

        //methods

        public void Initialize()
        {
            getMapUIParent();        

            CalculateRangeThick();

            CreateRulers();

        }

        public void CreateRulers()
        {
            DeleteRulers();

            CalculateRangeThick();

            if (MapUIParent == null) getMapUIParent();

            var mapbounds = map.MapBoundaries.MapBounds;
            var size = new Vector4(map.MapBoundaries.TableBounds.min.x, map.MapBoundaries.TableBounds.min.z, map.MapBoundaries.TableBounds.max.x, map.MapBoundaries.TableBounds.max.z);

            // X top ruler 
            var RulerCenter = new Vector3(mapbounds.center.x, 0f, mapbounds.center.z + (mapbounds.size.z / 2));
            Rect VisibilityRectagle = new Rect(mapbounds.min.x - 20, mapbounds.min.z - 20, mapbounds.size.x + 20, mapbounds.size.z + 20);
            if (map.useSlippyMap)
            {
                RulerCenter = new Vector3(mapbounds.center.x, 0f, size.z + rulerdistance);
                VisibilityRectagle = new Rect(size.x, size.z, size.z, size.w);
            }
            CreateRuler("ruler-top", 0, RangeticksX, TicksnumberX, mapbounds.size.x, Vector3.right, RulerCenter, VisibilityRectagle, RulerCoordinateType.LETTERS);

            // X Bottom ruler 
            RulerCenter = new Vector3(mapbounds.center.x, 0f, mapbounds.center.z - (mapbounds.size.z / 2));
            if (map.useSlippyMap)
            {
                RulerCenter = new Vector3(mapbounds.center.x, 0f, size.y - rulerdistance);
                VisibilityRectagle = new Rect(size.x, -size.z, size.z, size.w);
            }
            CreateRuler("ruler-bottom", 1, RangeticksX, TicksnumberX, mapbounds.size.x, Vector3.left, RulerCenter, VisibilityRectagle, RulerCoordinateType.LETTERS);


            // Z left ruler 
            RulerCenter = new Vector3(mapbounds.center.x + (mapbounds.size.x / 2), 0f, mapbounds.center.z);
            if (map.useSlippyMap)
            {
                RulerCenter = new Vector3(size.w + rulerdistance, 0f, mapbounds.center.z);
                VisibilityRectagle = new Rect(size.w, size.y, size.z, size.w);
            }
            CreateRuler("ruler-right", 2, RangeticksY, TicksnumberY, mapbounds.size.z, Vector3.back, RulerCenter, VisibilityRectagle, RulerCoordinateType.NUMBERS);

            // Z Right ruler 
            RulerCenter = new Vector3(mapbounds.center.x - (mapbounds.size.x / 2), 0f, mapbounds.center.z);
            if (map.useSlippyMap)
            {
                RulerCenter = new Vector3(size.x - rulerdistance, 0f, mapbounds.center.z);
                VisibilityRectagle = new Rect(-size.w, size.y, size.z, size.w);
            }
            CreateRuler("ruler-left", 3, RangeticksY, TicksnumberY, mapbounds.size.z, Vector3.forward, RulerCenter, VisibilityRectagle, RulerCoordinateType.NUMBERS);

        }

        public void DeleteRulers() {

            foreach (GameObject r in rulers) {

                if (r == null) return;

#if UNITY_EDITOR
                DestroyImmediate(r);
#else
                Destroy(r);
#endif
            }

            rulers = new GameObject[4];

        }

        private void CreateRuler(string name, int number, Vector2 Rangeticks, int Ticksnumber, float Length, Vector3 Direction, Vector3 Center, Rect VisibilityRect, RulerCoordinateType type)
        {

            rulers[number] = new GameObject();

            rulers[number].name = name;

            rulers[number].transform.parent = MapUIParent.transform;

            var ruler = rulers[number].AddComponent<Ruler>();

            ruler.Rangeticks = Rangeticks;

            ruler.Ticksnumber = Ticksnumber;

            ruler.Length = Length;

            ruler.Direction = Direction;

            ruler.Center = Center;

            ruler.VisibilityRectagle = VisibilityRect;

            ruler.type = type;

            ruler.Generate();

        }

        private void CalculateRangeThick() {
          

            TicksnumberX = System.Math.Abs((int)RangeticksX.y - (int)RangeticksX.x) + 2;

            TicksnumberY = System.Math.Abs((int)RangeticksY.y - (int)RangeticksY.x) + 2;

        }

        private void getMapUIParent()
        {

            MapUIParent = GameObject.Find("ArrowsAndRulers");

            if (MapUIParent == null)
            {

                MapUIParent = new GameObject();

                MapUIParent.name = "ArrowsAndRulers";

                MapUIParent.AddComponent<OrientCoordinatesToCamera>();
            }
        }

       

    }
}