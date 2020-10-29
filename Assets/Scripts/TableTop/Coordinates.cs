using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class Coordinates : Map
    {
        private Map map;

        public Vector3 WorldCoordinatesToMapLocalCoordiantes(Vector3 point)
        {
            Vector3 localCoordinate = gameObject.transform.InverseTransformPoint(point);
            return localCoordinate;
        }

        public Vector3 MapLocalCoordinatesToWorldCoordinates(Vector3 point)
        {
            Vector3 WorldCoordinates = gameObject.transform.TransformPoint(point);
            return WorldCoordinates;
        }

        public Mapzen.MercatorMeters MapLocalCoordinateToMapLocalMercatorMeters(Vector3 localCoordinate)
        {

            float XmetersLocal = localCoordinate.x / UnitsPerMeter;
            float YmetersLocal = localCoordinate.z / UnitsPerMeter; // bare in mind z is y

            Mapzen.MercatorMeters LocalMercatorMeters = new Mapzen.MercatorMeters(XmetersLocal, YmetersLocal);
            return LocalMercatorMeters;

        }

        public Mapzen.MercatorMeters MapLocalCoordinateToMapWorldMercatorMeters(Vector3 localCoordinate)
        {

            var Origin = getMapOrigin();

            Mapzen.MercatorMeters LocalMercatorMeters = MapLocalCoordinateToMapLocalMercatorMeters(localCoordinate);

            double XmetersWorld = LocalMercatorMeters.x + Origin.x;
            double YmetersWorld = LocalMercatorMeters.y + Origin.y;

            Mapzen.MercatorMeters WorldMercatorMeters = new Mapzen.MercatorMeters(XmetersWorld, YmetersWorld);
            return WorldMercatorMeters;

        }

        public Mapzen.LngLat MapLocalCoordinateToLtdLng(Vector3 localCoordinate)
        {
            Mapzen.MercatorMeters WorldMercatorMeters = MapLocalCoordinateToMapWorldMercatorMeters(localCoordinate);

            Mapzen.LngLat LtdLngcoordinate = Mapzen.Geo.Unproject(WorldMercatorMeters);

            return LtdLngcoordinate;

        }

        public Vector3 LatLngToMapLocalCoordinates(Mapzen.LngLat LngLatCoordinate)
        {
            

            var Origin = getMapOrigin();

            Mapzen.MercatorMeters mercmeters = Mapzen.Geo.Project(LngLatCoordinate);

            double XmetersLocal = ( mercmeters.x - Origin.x ) * map.UnitsPerMeter ;

            double YmetersLocal = ( mercmeters.y - Origin.y ) * map.UnitsPerMeter ; 

            return new Vector3((float)XmetersLocal, 0f, (float)YmetersLocal);

        }

        public Vector3 LatLngToMapCurrentWorldCoordinates(Mapzen.LngLat LngLatCoordinate)
        {

            Vector3 MapCurrentWorldPosition = getMapCurrentWorldPosition();

            return  LatLngToMapLocalCoordinates(LngLatCoordinate) + MapCurrentWorldPosition;

        }

        private Vector2 getMapOrigin()
        {

           

            return map.Origin;

        }

        private Vector3 getMapCurrentWorldPosition()
        {

          

            return map.gameObject.transform.position; 

        }

      
    }

}

