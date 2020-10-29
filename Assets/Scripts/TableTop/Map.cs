using UnityEngine;
using Mapzen.Unity;
using UnityEditor;

namespace TableTop
{

    public class Map : MonoBehaviour
    {

        //informations from Mapzen Maps Creation process

        public bool useSlippyMap = true;

        public Vector4 SlippyMapSize; // Boundaries->TableBounds

        public float UnitsPerMeter;

        public Vector2 Origin;


        //instances of Tabletop Elements

        public Boundaries MapBoundaries;

        public Rulers MapRulers;

        public Arrows MapArrows;

        public Coordinates MapCoordinates;

        public Interaction MapInteraction;

        public Augmentations MapAugmentations;

        //public Panels MapPanels;

        public SpatialAnchors MapSpatialAnchors;

        public Annotations MapAnnotations;

        public RayOnMap MapRayOnMap;

    }



}