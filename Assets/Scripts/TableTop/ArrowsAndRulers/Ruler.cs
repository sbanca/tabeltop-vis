﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace TableTop
{
    public class Ruler : MonoBehaviour
    {
        //public variables

        [SerializeField]

        private Vector2 rangeTicks = new Vector2(0f, 1f);

        public Vector2 Rangeticks
        {

            get { return rangeTicks; }
            set
            {
                if (rangeTicks != value)
                {

                    rangeTicks = value;
                }
            }

        }

        [SerializeField]

        private int ticksNumber;
        private int coordinatesNumber;
        public int Ticksnumber
        {

            get { return ticksNumber; }
            set
            {
                if (ticksNumber != value)
                {

                    ticksNumber = value;
                  
                }
            }

        }

        [SerializeField]

        private float length = 100f;

        public float Length
        {
            get { return length; }
            set
            {
                if (length != value)
                {

                    length = value;
                }
            }
        }

        [SerializeField]

        private Vector3 center = new Vector3(0f, 0f, 0f);

        public Vector3 Center
        {
            get { return center; }
            set
            {
                if (center != value)
                {

                    center = value;
                }
            }
        }

        [SerializeField]

        private Vector3 direction = Vector3.right;
        private Vector3 oppositeDirection = Vector3.forward;

        public Vector3 Direction
        {
            get { return direction; }
            set
            {
                if (direction != value)
                {

                    direction = value;
                    oppositeDirection = Vector3.Cross(direction, Vector3.up);


                }
            }
        }

        [SerializeField]
        public float thickness = 0.01f;
        [SerializeField]
        public float thickenssZ = 0.001f;
        [SerializeField]
        public float thickLength = 0.025f;

        public Rect VisibilityRectagle;

        public RulerCoordinateType type;

        public RulerCoordinatePlacement placement;

        //private variables 

        private GameObject bar;

        private List<GameObject> ticks = new List<GameObject>();

        private List<GameObject> coordinates = new List<GameObject>();


        //functions 

        public void Generate()
        {
            Createbar();

            Createticks();

            CreateCoordinates();

            AdjustCenter();

            RulerVisibilityWindow();

        }

        private void Createbar()
        {

            if (bar != null)
            {

#if UNITY_EDITOR


                DestroyImmediate(bar);

#else

            Destroy(bar);
#endif
            }

            //create cube as child
            bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bar.name = "bar";
            bar.transform.parent = gameObject.transform;

            //position cube            
            bar.transform.position = gameObject.transform.position;

            //scale
            Vector3 scale = bar.transform.localScale;
            Vector3 thickenssZVec = Vector3.up * thickenssZ;
            Vector3 thicknessVec = oppositeDirection * thickness;
            Vector3 lengthVec = direction * (length + thickness);
            Vector3 final = thicknessVec + thickenssZVec + lengthVec;
            scale.Set(final.x, final.y, final.z);
            bar.transform.localScale = scale;

            //bar material
            var r = bar.GetComponent<Renderer>();
            Material m = Resources.Load("Materials/" + name, typeof(Material)) as Material;
            r.material = m;

        }

        private void AdjustCenter()
        {
            gameObject.transform.position = center;
        }

        private void Createticks()
        {

            if (ticks.Count > 0)
            {

                foreach (GameObject t in ticks)
                {
#if UNITY_EDITOR

                    if (t != null) DestroyImmediate(t);

#else

            Destroy(t);
#endif
                }

                ticks = new List<GameObject>();

            }



            List<float> ticksarray = TicksArray();
            float step = WorldTicksStep();
            Vector3 stepvector = direction * step;
            Vector3 distance = new Vector3();

            Vector3 start = gameObject.transform.position - (direction * (length / 2));

            foreach (float tick in ticksarray)
            {

                //create cube as child 
                GameObject tickObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tickObject.name = tick.ToString();
                tickObject.transform.parent = gameObject.transform;

                //position cube            
                tickObject.transform.position = start + distance + (oppositeDirection * (thickLength / 2));
                distance += stepvector;

                //scale ticks 
                Vector3 scale = tickObject.transform.localScale;
                Vector3 thickenssZVec = Vector3.up * thickenssZ;
                Vector3 thicknessVec = direction * thickness;
                Vector3 lengthVec = oppositeDirection * thickLength;
                Vector3 final = thicknessVec + thickenssZVec + lengthVec;
                scale.Set(final.x, final.y, final.z);
                tickObject.transform.localScale = scale;

                //bar material
                var r = tickObject.GetComponent<Renderer>();
                Material m = Resources.Load("Materials/" + name, typeof(Material)) as Material;
                r.material = m;

                //add to the list 
                ticks.Add(tickObject);

            }

        }

        private void CreateCoordinates()
        {

            coordinatesNumber = RulerCoordinatePlacement.ONTHICKS == placement ? ticksNumber : ticksNumber - 1;


            string[] CoordArray = CoordinatesArray();

            if (name== "ruler-bottom" || name== "ruler-left") Array.Reverse(CoordArray);

            float step = WorldTicksStep();
            Vector3 stepvector = direction * step;
            Vector3 distance = new Vector3();

            //start
            Vector3 start = new Vector3 { };
            if (RulerCoordinatePlacement.ONTHICKS == placement) start = gameObject.transform.position - (direction * (length / 2));
            else if (RulerCoordinatePlacement.BETWEENTICKS == placement)  start = gameObject.transform.position - (direction * (length / 2)) + stepvector / 2;


            if (RulerCoordinateType.LETTERS == type || RulerCoordinateType.NUMBERS == type)
            {
                for (int i = 0; i < coordinatesNumber; i++)
                {

                    //loadprefab
                    string folder = RulerCoordinateType.LETTERS == type ? "letters/" : "numbers/";
                    GameObject prefab = loadPrefabFromResoureces(folder + CoordArray[i]);

                    //instantiate
                    GameObject coordObject = Instantiate(prefab);
                    coordObject.name = CoordArray[i];
                    coordObject.transform.parent = gameObject.transform;

                    //material
                    Renderer r = coordObject.GetComponentInChildren<MeshRenderer>();
                    Material m = Resources.Load("Materials/" + name, typeof(Material)) as Material;
                    r.material = m;

                    //rendered boundaries
                    r.gameObject.transform.position = r.transform.position - r.bounds.center;
                    //if (OrientCoordinatesToCamera.Instance == null) getOrientCoordinateInstance();
                    //OrientCoordinatesToCamera.Instance.objects.Add(r.gameObject);

                    //scale ticks 
                    Vector3 scale = coordObject.transform.localScale;
                    scale.Set(0.1f, 0.1f, 0.1f);
                    coordObject.transform.localScale = scale;
                    coordObject.transform.Rotate(new Vector3(0f, 180f, 0f));

                    //position           
                    coordObject.transform.position = start + distance + (oppositeDirection * 0.075f);
                    distance += stepvector;

                    //add to the list 
                    ticks.Add(coordObject);

                }
            }
            else if (RulerCoordinateType.VALUE == type) {

                for (int i = 0; i < coordinatesNumber; i++)
                {
                    //loadprefab            
                    GameObject prefab = loadPrefabFromResoureces("TextMeshProLabel");

                    //instantiate
                    GameObject coordObject = Instantiate(prefab);
                    coordObject.name = CoordArray[i];
                    coordObject.transform.parent = gameObject.transform;

                    TextMeshPro tmp = coordObject.GetComponent<TextMeshPro>();
                    tmp.text= CoordArray[i].ToString();

                    if (direction == Vector3.right)
                    {
                        coordObject.transform.Rotate(new Vector3(0f, 0f, -90f));
                        tmp.alignment = TextAlignmentOptions.Right;
                    }
                    else if (direction == Vector3.left)
                    {
                        coordObject.transform.Rotate(new Vector3(0f, 0f, -90f));
                        tmp.alignment = TextAlignmentOptions.Left;
                    }
                    else if (direction == Vector3.back) tmp.alignment = TextAlignmentOptions.Left;
                    else if (direction == Vector3.forward) tmp.alignment = TextAlignmentOptions.Right;

                    //position           
                    coordObject.transform.position = start + distance + (oppositeDirection * 0.075f);
                    distance += stepvector;

                    //add to the list 
                    ticks.Add(coordObject);
                }
            }



        }

        private List<float> TicksArray()
        {

            List<float> array = new List<float>();


            if (rangeTicks.x > rangeTicks.y)
            {

                var temp = rangeTicks.x;

                rangeTicks.x = rangeTicks.y;

                rangeTicks.y = temp;

            }

            var step = TicksStep();

            for (float f = rangeTicks.x; f < rangeTicks.y + (step / 2); f += step) array.Add(f);

            return array;
        }

        private string[] CoordinatesArray()
        {

            string[] Alphabet = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "Z" };

            string[] Numbers = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21" };

            string[] array = new string[coordinatesNumber];

            float count = 0f;

            for (int i = 0; i < coordinatesNumber; i++)
            {

                if (type == RulerCoordinateType.LETTERS) array[i] = Alphabet[i];
                else if (type == RulerCoordinateType.NUMBERS) array[i] = Numbers[i];
                else if (type == RulerCoordinateType.VALUE) {                  
                    array[i] = Math.Round(count,1).ToString();
                    count += TicksStep();
                }

            }

            return array;
        }

        private float TicksStep()
        {
            return (rangeTicks.y - rangeTicks.x) / (ticksNumber - 1);
        }

        private float WorldTicksStep()
        {
            return length / (ticksNumber - 1);
        }

        private GameObject loadPrefabFromResoureces(string name)
        {

            return Resources.Load("Prefabs/" + name, typeof(GameObject)) as GameObject;

        }

        private void RulerVisibilityWindow()
        {

            //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //cube.transform.position = new Vector3(VisibilityRectagle.center.x, 0f, VisibilityRectagle.center.y);

            //Vector3 scale = cube.transform.localScale;
            //scale.Set(VisibilityRectagle.width, 0f, VisibilityRectagle.height);
            //cube.transform.localScale = scale;

            Rect vr = VisibilityRectagle;

            Vector4 size = new Vector4(vr.x, vr.y, System.Math.Abs(vr.x) + System.Math.Abs(vr.width), System.Math.Abs(vr.y) + System.Math.Abs(vr.height));

#if UNITY_EDITOR

            string path = "Materials/" + name;
            Material m = Resources.Load(path, typeof(Material)) as Material;
            if (m != null) m.SetVector("_Corners", size);


#else

        Renderer[] allRenderers = UnityEngine.Object.FindObjectsOfType<Renderer>();
        //Renderer[] allRenderers = gameObject.GetComponentsInChildren<Renderer>();
        
        foreach (Renderer r in allRenderers)
        {
            Material[] materials = r.materials;
            foreach (Material m in materials)
            {
                if (m.shader.name == "Custom/ClipVolume")
                {
                    m.SetVector("_Corners", size );
                }
            }
        }

#endif


        }

        private void getOrientCoordinateInstance() {

            var MapUIParent = GameObject.Find("ArrowsAndRulers");


            if (MapUIParent == null)
            {

                MapUIParent = new GameObject();

                MapUIParent.name = "ArrowsAndRulers";

                MapUIParent.AddComponent<OrientCoordinatesToCamera>();

            }
            else {

                var orientCoord = MapUIParent.GetComponent<OrientCoordinatesToCamera>();

                if(orientCoord==null) MapUIParent.AddComponent<OrientCoordinatesToCamera>();
            }
        }

    }
}