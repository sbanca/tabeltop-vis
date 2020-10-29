﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TableTop
{
    public class Options : MonoBehaviour
    {
        [SerializeField]
        public GameObject OptionPrefab;

        [SerializeField]
        public OptionData[] OptionList;

        private GameObject Parent;

        private Option[] optionManagersList;

        public OptionClicked optionClicked;

     

        public void Generate() {

            Parent = new GameObject("Option Container");

            Parent.transform.parent = gameObject.transform;          

            optionManagersList = new Option[OptionList.Length]; 


            for (int i= 0; i < OptionList.Length; i++) {

                //get option data item 

                OptionData optionData = OptionList[i];

                optionData.number = i;


                //instantiate option prefab

                GameObject optionObject = Instantiate(OptionPrefab);

                optionObject.name = optionData.Name;

                optionObject.transform.parent = Parent.transform;


                //set transfrom from parent object 

                setTransform(optionObject);


                //set option title and selection

                Option optionManager = optionObject.GetComponent<Option>();

                optionManager.optionData = optionData;

                // add option to the list 

                optionManagersList[i] = optionManager;

            }



        }


        private void setTransform(GameObject optionObject)
        {

            optionObject.transform.localScale = Parent.transform.parent.transform.parent.transform.localScale;

            optionObject.transform.position = Parent.transform.parent.transform.position;

            optionObject.transform.rotation = Parent.transform.parent.transform.rotation;

            optionObject.transform.parent = Parent.transform.parent.transform;

        }

        public void OptionSelector(string name) {
            
            optionClicked.Invoke(name);
        }
  
        public void DeleteAllOptions()
        {

            if (Parent == null) GetParent();

            if (Parent == null) return;

            GameObject[] childList = new GameObject[Parent.transform.childCount];

            for (int x = 0; x < Parent.transform.childCount; x++) childList[x] = Parent.transform.GetChild(x).gameObject;
                
            foreach (GameObject child in childList)
            {

#if UNITY_EDITOR
                DestroyImmediate(child);
#else
                Destroy(child);
#endif              

            }

#if UNITY_EDITOR
            DestroyImmediate(Parent);
#else
            Destroy(Parent);
#endif

        }

        private void GetParent() {

            if (Parent != null) return;

            for (int x = 0; x < gameObject.transform.childCount; x++)
            {

                var child = gameObject.transform.GetChild(x).gameObject;

                if (child.name == "Option Container") Parent = child;


            }

        }


    }


}

