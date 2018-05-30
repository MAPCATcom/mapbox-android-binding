using Android.App;
using Android.Widget;
using Android.Util;
using Android.OS;
using MapcatAccountManager = Com.Mapcat.Mapcatsdk.Mapcat;
using Com.Mapcat.Mapcatsdk.Annotations;
using Com.Mapcat.Mapcatsdk.Camera;
using Com.Mapcat.Mapcatsdk.Geometry;
using Com.Mapcat.Mapcatsdk.Maps;
using Android.Support.V7.App;
using System;

namespace Naxam.MapcatQs
{
    class OnMapReadyCallback : Java.Lang.Object, IOnMapReadyCallback
    {
        public Action<MapboxMap> MapReady { get; set; }
        public OnMapReadyCallback(Action<MapboxMap> MapReady)
        {
            this.MapReady = MapReady;
        }
        public void OnMapReady(MapboxMap map)
        {
            MapReady?.Invoke(map);
        }
    }

    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@mipmap/ic_launcher", Theme = "@style/MyTheme")]
    public class MainActivity : AppCompatActivity, IMapViewInitListener
    {
        MapView mapView;

        public void OnMapViewInitSuccess()
        {
            mapView.GetMapAsync(new OnMapReadyCallback(
               (map) =>
               {
                   // set camera position
                   var position = new CameraPosition.Builder()
                           .Target(new LatLng(47.033, 18.025)) // Sets the new camera position
                           .Zoom(13) // Sets the zoom
                           .Build(); // Creates a CameraPosition from the builder
                   map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(position));

                   // add marker
                   MarkerOptions marker = new MarkerOptions();
                   marker.SetPosition(new LatLng(47.033, 18.025));
                   marker.SetTitle("Hello!");
                   marker.SetSnippet("Wellcome");
                   map.AddMarker(marker);
               }
           ));
        }

        public void OnMapViewInitError(string errorMessage)
        {
            Log.Error("MapViewInit", errorMessage);
        }

        protected override void OnCreate(Bundle bundle)
        {
            //TabLayoutResource = Resource.Layout.Tabbar;
            //ToolbarResource = Resource.Layout.Toolbar;

            Log.Info("General", "startup");

            base.OnCreate(bundle);
            MapcatAccountManager.GetInstance(this.BaseContext, "< Your Mapcat Visualization API key goes here >");

            SetContentView(Resource.Layout.Main);

            mapView = FindViewById<MapView>(Resource.Id.mapView);

            mapView.InitMapcatMap(new LayerOptions(false, false));

            mapView.OnCreate(bundle);

            MapViewInitHandler.RegisterListener(this);
        }

        protected override void OnStart()
        {
            base.OnStart();
            mapView.OnStart();
        }

        protected override void OnResume()
        {
            base.OnResume();
            mapView.OnResume();
        }

        protected override void OnPause()
        {
            mapView.OnPause();
            base.OnPause();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            mapView.OnSaveInstanceState(outState);
        }

        protected override void OnStop()
        {
            base.OnStop();
            mapView.OnStop();
        }

        protected override void OnDestroy()
        {
            mapView.OnDestroy();
            base.OnDestroy();
            MapViewInitHandler.UnregisterListener(this);
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();
            mapView.OnLowMemory();
        }
    }
}

