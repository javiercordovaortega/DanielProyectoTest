using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Media;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DanielProyecto
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, MediaPlayer.IOnPreparedListener
    {
        EditText txtUsuario, txtPassword;
        Button btnLogin;
        VideoView video;
        ProgressBar progressBar;

        public void OnPrepared(MediaPlayer mp)
        {
            mp.Looping = true;
            mp.SetVolume(0, 0);
            mp.SetVideoScalingMode(VideoScalingMode.ScaleToFitWithCropping);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.activity_main);
            
            CopyDocuments("baseInterna.sqlite", "LeonaliDB.db");
            txtUsuario = (EditText)FindViewById(Resource.Id.txtUsuario);
            txtPassword = (EditText)FindViewById(Resource.Id.txtContrasena);
            btnLogin = (Button)FindViewById(Resource.Id.btnLogin);
            var ln = (LinearLayout)FindViewById(Resource.Id.lnPreguntas);

            btnLogin.Click += async delegate {

                progressBar = new ProgressBar(this, null, Android.Resource.Attribute.ProgressBarStyleLarge);
                RelativeLayout.LayoutParams p = new RelativeLayout.LayoutParams(200, 200);
                p.AddRule(LayoutRules.CenterInParent);
                progressBar.IndeterminateDrawable.SetColorFilter(Android.Graphics.Color.Rgb(255, 255, 255), Android.Graphics.PorterDuff.Mode.Multiply);

                FindViewById<RelativeLayout>(Resource.Id.FondoLogin).AddView(progressBar, p);
                await Task.Delay(10000);
                progressBar.Visibility = Android.Views.ViewStates.Visible;
                Window.AddFlags(Android.Views.WindowManagerFlags.NotTouchable);
                
                try
                {
                    com.somee.servicioweb1test.Service service = new com.somee.servicioweb1test.Service();
                    var xml = service.Consulta("select * from usuarios where user_name = '" + txtUsuario.Text + "' and user_password = '" + txtPassword.Text + "';");
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ClaseDato));
                    var claseDato = new ClaseDato();

                    var jsonLimpio = "";
                    var bandera = false;

                    foreach (var item in xml)
                    {
                        if (item == '[')
                            bandera = true;
                        if (bandera)
                            jsonLimpio += item;
                        if (item == ']')
                            break;
                    }

                    var results = JsonConvert.DeserializeObject<List<ClaseDato>>(jsonLimpio);

                    new General().GuardarDatosUsuario(results[0].id_user, results[0].user_name, results[0].user_password);

                    StartActivity(typeof(ActivityMenu));
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "Error al iniciar \r\n Verifica tu conexion a internet o tu Usuario y/o contraseña", ToastLength.Short).Show();
                    progressBar.Visibility = Android.Views.ViewStates.Invisible;
                    Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                }

            };

            video = (VideoView)FindViewById(Resource.Id.videoPlay);

            video.SetOnPreparedListener(this);
            string videoPaht = "android.resource://DanielProyecto.DanielProyecto/" + Resource.Raw.agri;
            Android.Net.Uri uri = Android.Net.Uri.Parse(videoPaht);
            video.SetVideoURI(uri);
            video.Start();
        }

        public void CopyDocuments(string FileName, string AssetsFileName)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string dbPath = System.IO.Path.Combine(path, FileName);

            try
            {
                if (!File.Exists(dbPath))
                {
                    using (var br = new BinaryReader(Application.Context.Assets.Open(AssetsFileName)))
                    {
                        using (var bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
                        {
                            byte[] buffer = new byte[2048];
                            int length = 0;
                            while ((length = br.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                bw.Write(buffer, 0, length);
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }
    }
}

