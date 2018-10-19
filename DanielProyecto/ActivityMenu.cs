using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DanielProyecto
{
    [Activity(Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class ActivityMenu : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_activityMenu);

            FindViewById<Button>(Resource.Id.btnHigieneUsuario).Click += delegate {
                Intent intent = new Intent(this, typeof(MenuActivity));
                intent.PutExtra("Indentificador", "0");
                StartActivity(intent);
            };
            FindViewById<Button>(Resource.Id.btnHigieneArea).Click += delegate {
                Intent intent = new Intent(this, typeof(MenuActivity));
                intent.PutExtra("Indentificador", "1");
                StartActivity(intent);
            };
        }
    }
}