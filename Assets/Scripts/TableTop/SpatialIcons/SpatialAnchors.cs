﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class SpatialAnchors : Map
    {

        public string JsonName = "data.json";


        public List<OptionData> spatialAnchorsList; 

        private GameObject[] spatialAnchorsGameObjectsList;


        private Coordinates MapCoordinates;
        
        private GameObject parent;


        public void Start()
        {
            

            if (parent == null)  createParent();

            if (spatialAnchorsList == null) GetSpatialAnchorsList();

            loadPrefabs();
        }

        private void GetSpatialAnchorsList()
        {
            spatialAnchorsList = new List<OptionData>();

            string panelstext = LoadResourceTextfile(JsonName);

            TasksDataGroups panelsData = JsonUtility.FromJson<TasksDataGroups>(panelstext);

            foreach (TasksData pts in panelsData.List)
            {
                foreach (TaskData pt in pts.List)
                {
                    if (pt.Options.Count > 0)
                    {
                        foreach (OptionData sa in pt.Options)
                        {
                            spatialAnchorsList.Add(sa);
                        }
                    }
                }
            }
        }

        private static string LoadResourceTextfile(string name)
        {

            string filePath = "SetupData/" + name.Replace(".json", "");

            TextAsset targetFile = Resources.Load<TextAsset>(filePath);

            return targetFile.text;
        }

        public void GenerateSpatialAnchors()
        {

#if UNITY_EDITOR

            if (spatialAnchorsList == null || spatialAnchorsList.Count==0) GetSpatialAnchorsList();

            if (parent == null) createParent();


#endif

            spatialAnchorsGameObjectsList = new GameObject[spatialAnchorsList.Count];

            for (int i=0; i< spatialAnchorsList.Count; i++)
            {
                OptionData sa = spatialAnchorsList[i];

                GameObject prefab = GetPrefabBasedOnType(sa.Type);

                GameObject SpawnedPrefab = SpawnPrefab(sa.Lat, sa.Lng, prefab);

                spatialAnchorsGameObjectsList[i]=SpawnedPrefab;
            }

        }

        public float getBuildingsElevation(Vector3 LocalPosition)
        {

            float elevation = 0f;

            var buildings = GameObject.Find("Buildings");

            var childnumber = buildings.transform.childCount;

            for (int x = 0; x < childnumber; x++)
            {

                Transform child = buildings.transform.GetChild(x);

                MeshRenderer r = child.GetComponent<MeshRenderer>();

                if (r.bounds.Contains(LocalPosition))
                {

                    MeshCollider collider = child.gameObject.AddComponent<MeshCollider>();

                    RaycastHit hit = new RaycastHit();

                    Vector3 startingPoint = LocalPosition + (Vector3.up * 2); //move up the point 


                    //this is to cast rays around to make sure surrunding buidlings or small sapce do not obscure the object 

                    var ray = new Ray(startingPoint, Vector3.down);

                    if (collider.Raycast(ray, out hit, 200f))
                    {

                        elevation = hit.point.y;

#if UNITY_EDITOR
                        Object.DestroyImmediate(collider);
#else
                        Object.Destroy(collider);
#endif

                        break;
                    }

#if UNITY_EDITOR
                    Object.DestroyImmediate(collider);
#else
                    Object.Destroy(collider);
#endif

                }
            }

            return elevation;

        }
        
        private GameObject SpawnPrefab(double Lat, double Lng, GameObject Prefab) {


            Mapzen.LngLat coordinates = new Mapzen.LngLat(Lng, Lat);

            Vector3 MapLocalCoordinates = MapCoordinates.LatLngToMapLocalCoordinates(coordinates);

            MapLocalCoordinates.y = getBuildingsElevation(MapLocalCoordinates);

            GameObject SpawnedPrefab = Instantiate(Prefab, MapLocalCoordinates, Quaternion.identity, parent.transform);

            return SpawnedPrefab;
        }

        private GameObject GetPrefabBasedOnType(SpatialAnchorType Type)
        {
            

            GameObject prefab;

            switch (Type)
            {

                case (SpatialAnchorType.ELECTRONICSHOP):
                    prefab = ElectronicShopIconPrefeab;
                    break;
                case (SpatialAnchorType.HOTEL):
                    prefab = HotelIconPrefeab;
                    break;
                case (SpatialAnchorType.PRINTSHOP):
                    prefab = PrintShopIconPrefeab;
                    break;
                case (SpatialAnchorType.RESTAURANT):
                    prefab = RestaurantIconPrefeab;
                    break;
                case (SpatialAnchorType.WORKMEETING):
                    prefab = WorkMeetingPrefeab;
                    break;
                case (SpatialAnchorType.APPLESTORE):
                    prefab = AppleIconPrefab;
                    break;
                default:
                    prefab = DefaultIconPrefab;
                    break;
            }

            return prefab;
        }

        public void DeleteAll() {

            DeleteData();

            DeleteGameObjects();

        }

        private void DeleteData()
        {
            if (spatialAnchorsList != null) spatialAnchorsList = null;

        }

        private void DeleteGameObjects() {

            getChildFromParent();

            if (spatialAnchorsGameObjectsList == null) return;

            foreach (GameObject go in spatialAnchorsGameObjectsList)
            {

#if UNITY_EDITOR
                DestroyImmediate(go);
#else
                Destroy(go);
#endif

            }

#if UNITY_EDITOR
            DestroyImmediate(parent);
#else
                Destroy(parent);
#endif

        }

        private void createParent() {

            parent = GameObject.Find("SpatialAnchors");

            if (parent != null) return;

            parent = new GameObject();

            parent.name = "SpatialAnchors";

            parent.transform.parent = gameObject.transform;
        }



        private void getChildFromParent() {

            parent = GameObject.Find("SpatialAnchors");

            if (parent == null) return;

            int childCount = parent.transform.childCount;

            if (childCount < 1) return;

            GameObject child;

            spatialAnchorsGameObjectsList = new GameObject[childCount];

            for (int i = 0; i < childCount; i++) {

                child = parent.transform.GetChild(i).gameObject;

            }

        }

        //prefabload

        [SerializeField]
        public GameObject ElectronicShopIconPrefeab;

        [SerializeField]
        public GameObject HotelIconPrefeab;

        [SerializeField]
        public GameObject PrintShopIconPrefeab;

        [SerializeField]
        public GameObject RestaurantIconPrefeab;

        [SerializeField]
        public GameObject WorkMeetingPrefeab;

        [SerializeField]
        public GameObject DefaultIconPrefab;

        [SerializeField]
        public GameObject AppleIconPrefab;

        public void loadPrefabs() {

            ElectronicShopIconPrefeab = loadPrefabFromResoureces("ElectronicShopIcon_Prefab");

            HotelIconPrefeab = loadPrefabFromResoureces("HotelIcon_Prefab");

            PrintShopIconPrefeab = loadPrefabFromResoureces("PrintShopIcon_Prefab");

            RestaurantIconPrefeab = loadPrefabFromResoureces("RestaurantIcon_Prefab");

            WorkMeetingPrefeab = loadPrefabFromResoureces("WorkMeeting_Prefab");

            AppleIconPrefab = loadPrefabFromResoureces("AppleIcon_prefab");

            DefaultIconPrefab = loadPrefabFromResoureces("annotation_prefab");

        }

        private GameObject loadPrefabFromResoureces(string name) {

            return  Resources.Load("Prefabs/"+name, typeof(GameObject)) as GameObject;

        }


    }
}
