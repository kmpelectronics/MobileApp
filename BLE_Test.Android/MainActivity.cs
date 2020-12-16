using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using Android.Support.V4.Content;
using Android.Support.V4.App;

namespace BLE_Test.Droid
{
    [Activity(Label = "BLE_Test", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private readonly string[] Permissions =
        {
            Manifest.Permission.Bluetooth,
            Manifest.Permission.BluetoothAdmin,
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            CheckPermissions();

            LoadApplication(new App());

            //var locationPermissions = new[]
            //{
            //    Manifest.Permission.AccessCoarseLocation,
            //    Manifest.Permission.AccessFineLocation
            //};
            //// check if the app has permission to access coarse location
            //var coarseLocationPermissionGranted =
            //    ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation);
            //// check if the app has permission to access fine location
            //var fineLocationPermissionGranted =
            //     ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation);
            //// if either is denied permission, request permission from the user
            //const int locationPermissionsRequestCode = 1000;
            //if (coarseLocationPermissionGranted == Permission.Denied ||
            //    fineLocationPermissionGranted == Permission.Denied)
            //{
            //    ActivityCompat.RequestPermissions(this, locationPermissions,
            //    locationPermissionsRequestCode);
            //}


        }
        private void CheckPermissions()
        {
            bool minimumPermissionsGranted = true;

            foreach (string permission in Permissions)
            {
                if (CheckSelfPermission(permission) != Permission.Granted)
                {
                    minimumPermissionsGranted = false;
                }
            }

            // If any of the minimum permissions aren't granted, we request them from the user
            if (!minimumPermissionsGranted)
            {
                RequestPermissions(Permissions, 0);
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);


        }
    }
}