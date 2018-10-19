using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SQLite;

namespace DanielProyecto
{
    class FragmentRespuestas : Android.Support.V4.App.Fragment
    {
        public string indentificador;

        public FragmentRespuestas(string indentificador)
        {
            this.indentificador = indentificador;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View Vista = inflater.Inflate(Resource.Layout.layoutRespuestas, container, false);
            com.somee.servicioweb1test.Service service = new com.somee.servicioweb1test.Service();
            if (indentificador == "0")
            {
                var contenido = Vista.FindViewById<ListView>(Resource.Id.ListaRespuestas);
                var xml2 = service.Consulta("select * from Cuestionario_Trabajadores where id_user = "+ new General().ConsultarDatosUsuario().id_user + ";");
                var json = JsonConvert.DeserializeObject<List<Respuestas>>(xml2);
                contenido.Adapter = new AdaptadorRespuestas(json, this.Activity, indentificador);
            }
            else
            {
                var contenido = Vista.FindViewById<ListView>(Resource.Id.ListaRespuestas);
                var xml2 = service.Consulta("select * from Cuestionario_Higiene  where id_user = " + new General().ConsultarDatosUsuario().id_user + ";");
                var json = JsonConvert.DeserializeObject<List<Respuestas>>(xml2);
                contenido.Adapter = new AdaptadorRespuestas(json, this.Activity, indentificador);
            }
            return Vista;
        }
    }
    public class AdaptadorRespuestas : BaseAdapter<Respuestas>
    {
        List<Respuestas> lista;
        Activity context;
        string identificador;

        public AdaptadorRespuestas(List<Respuestas> lista, Activity _context, string _identificador)
        {
            this.lista = lista;
            this.context = _context;
            this.identificador = _identificador;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override Respuestas this[int position]
        {
            get { return lista[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return lista.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = lista[position];
            View view = convertView;
            view = context.LayoutInflater.Inflate(Resource.Layout.layoutFechas, null);
            view.FindViewById<TextView>(Resource.Id.txtFexcha).Text = item.fecha;
            view.Click += delegate {
                Android.App.Dialog alertar = new Android.App.Dialog(context, Resource.Style.ThemeOverlay_AppCompat_Dialog);
                alertar.RequestWindowFeature(1);
                alertar.SetCancelable(true);
                alertar.SetContentView(Resource.Layout.layoutResumen);
                string concatenar = "";
                if (identificador == "0")
                {
                    var con = new SQLiteConnection(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "baseInterna.sqlite"));
                    var consulta = con.Query<Trabajadores>("select * from trabajadores", (new Trabajadores()).id_pregunta);
                    List<string> preguntas = new List<string>();
                    foreach (var itemP in consulta)
                    {
                        preguntas.Add(itemP.pregunta);
                    }
                    concatenar = preguntas[0] + "\r\n" + item.pregunta1 + "\r\n" + "\r\n"
                    + preguntas[1] + "\r\n" + item.pregunta2 + "\r\n" + "\r\n"
                    + preguntas[2] + "\r\n" + item.pregunta3 + "\r\n" + "\r\n"
                    + preguntas[3] + "\r\n" + item.pregunta4 + "\r\n" + "\r\n"
                    + preguntas[4] + "\r\n" + item.pregunta5 + "\r\n";
                }
                else
                {
                    var con = new SQLiteConnection(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "baseInterna.sqlite"));
                    var consulta = con.Query<Areas>("select * from Areas", (new Areas()).id_pregunta);
                    List<string> preguntas = new List<string>();
                    foreach (var itemP in consulta)
                    {
                        preguntas.Add(itemP.pregunta);
                    }
                    concatenar = preguntas[0] + "\r\n" + item.pregunta1 + "\r\n" + "\r\n"
                    + preguntas[1] + "\r\n" + item.pregunta2 + "\r\n" + "\r\n"
                    + preguntas[2] + "\r\n" + item.pregunta3 + "\r\n" + "\r\n"
                    + preguntas[3] + "\r\n" + item.pregunta4 + "\r\n" + "\r\n"
                    + preguntas[4] + "\r\n" + item.pregunta5 + "\r\n";
                }
                alertar.FindViewById<TextView>(Resource.Id.txtPreguntaR).Text = concatenar;
                alertar.Show();
            };
            return view;
        }
    }
    public class Respuestas
    {
        public int id_pregunta { get; set; }
        public string fecha { get; set; }
        public string pregunta1 { get; set; }
        public string pregunta2 { get; set; }
        public string pregunta3 { get; set; }
        public string pregunta4 { get; set; }
        public string pregunta5 { get; set; }
        public int id_user { get; set; }
    }
}