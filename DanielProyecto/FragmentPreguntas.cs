using System;
using System.Collections.Generic;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using SQLite;

namespace DanielProyecto
{
    class FragmentPreguntas : Android.Support.V4.App.Fragment
    {
        public string indentificador;

        public FragmentPreguntas(string indentificador)
        {
            this.indentificador = indentificador;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View Vista = inflater.Inflate(Resource.Layout.layoutPreguntas, container, false);
            if (indentificador == "0")
            {
                var contenido = Vista.FindViewById<ListView>(Resource.Id.ListaPreguntas);
                var con = new SQLiteConnection(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "baseInterna.sqlite"));

                var consulta = con.Query<Trabajadores>("select * from trabajadores", (new Trabajadores()).id_pregunta);
                consulta.Add(new Trabajadores { id_pregunta = 0 });
                contenido.Adapter = new AdaptadorTrabajadores(consulta, this.Activity);
            }
            else
            {
                var contenido = Vista.FindViewById<ListView>(Resource.Id.ListaPreguntas);
                var con = new SQLiteConnection(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "baseInterna.sqlite"));
                var consulta = con.Query<Areas>("select * from Areas", (new Areas()).id_pregunta);
                consulta.Add(new Areas { id_pregunta = 0 });
                contenido.Adapter = new AdaptadorAreas(consulta, this.Activity);
            }
            return Vista;
        }
    }

    public class AdaptadorAreas : BaseAdapter<Areas>
    {
        List<Areas> lista;
        Activity context;
        List<string> listasdatos = new List<string>() { "0", "No", "No", "No", "No", "No" };
        

        public AdaptadorAreas(List<Areas> lista, Activity context)
        {
            this.lista = lista;
            this.context = context;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override Areas this[int position]
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
            view = context.LayoutInflater.Inflate(Resource.Layout.itemsLista, null);
            view.FindViewById<TextView>(Resource.Id.txtPregunta).Text = item.pregunta;
            var si = view.FindViewById<TextView>(Resource.Id.si);
            si.Text = item.resp_1;
            var no = view.FindViewById<TextView>(Resource.Id.no);
            no.Text = item.resp_2;
            si.Click += delegate
            {
                si.SetBackgroundColor(Color.Rgb(132, 196, 84));
                no.SetBackgroundColor(Color.White);
                listasdatos[item.id_pregunta] = item.resp_1;
                
            };
            no.Click += delegate
            {
                no.SetBackgroundColor(Color.Rgb(193, 25, 25));
                si.SetBackgroundColor(Color.White);
                listasdatos[item.id_pregunta] = item.resp_2;

            };
            if (item.id_pregunta == 0)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.ItemBoton, null);
                var enviar = view.FindViewById<TextView>(Resource.Id.btnboton);
                enviar.Click += delegate
                {
                    com.somee.servicioweb1test.Service service = new com.somee.servicioweb1test.Service();
                    try
                    {
                        var fecha = DateTime.Now;
                        string fec = fecha.ToString("dd/MM/yyyy HH:mm");
                        service.Login("insert into Cuestionario_Higiene   values ('" + fec + "','" + listasdatos[1] + "','" + listasdatos[2] + "','" + listasdatos[3] + "','" + listasdatos[4] + "','" + listasdatos[5] + "'," + new General().ConsultarDatosUsuario().id_user + ")");
                        context.StartActivity(typeof(ActivityMenu));
                    }
                    catch (System.Exception)
                    {
                        Toast.MakeText(context, "Sin internet", ToastLength.Short);
                    }

                };
            }
            return view;
        }
    }

    public class AdaptadorTrabajadores : BaseAdapter<Trabajadores>
    {
        List<Trabajadores> lista;
      
        Activity _context;
        List<string> listasdatos = new List<string>() { "0", "No", "No", "No", "No", "No" };
        public AdaptadorTrabajadores(List<Trabajadores> lista, Activity context)
        {
            this.lista = lista;
            this._context = context;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override Trabajadores this[int position]
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
            view = _context.LayoutInflater.Inflate(Resource.Layout.itemsLista, null);
            view.FindViewById<TextView>(Resource.Id.txtPregunta).Text = item.pregunta;
            var si = view.FindViewById<TextView>(Resource.Id.si);
            si.Text = item.resp_1;
            var no = view.FindViewById<TextView>(Resource.Id.no);
            no.Text = item.resp_2;
            si.Click += delegate
            {
                si.SetBackgroundColor(Color.Rgb(132, 196, 84));
                no.SetBackgroundColor(Color.White);
                listasdatos[item.id_pregunta] = item.resp_1;
            };
            no.Click += delegate
            {
                no.SetBackgroundColor(Color.Rgb(193, 25, 25));
                si.SetBackgroundColor(Color.White);
                listasdatos[item.id_pregunta] = item.resp_2;
            };

            if (item.id_pregunta == 0)
            {
                view = _context.LayoutInflater.Inflate(Resource.Layout.ItemBoton, null);
                var enviar = view.FindViewById<TextView>(Resource.Id.btnboton);
                enviar.Click += delegate 
                {
                    com.somee.servicioweb1test.Service service = new com.somee.servicioweb1test.Service();
                    try
                    {
                        var fecha = DateTime.Now;
                        string fec = fecha.ToString("dd/MM/yyyy HH:mm");
                        if (service.Login("insert into Cuestionario_Trabajadores  values ('" + fec + "','" + listasdatos[1] + "','" + listasdatos[2] + "','" + listasdatos[3] + "','" + listasdatos[4] + "','" + listasdatos[5] + "'," + new General().ConsultarDatosUsuario().id_user + ")"))
                        { 
                            _context.StartActivity(typeof(ActivityMenu));
                        }
                        
                    }
                    catch (System.Exception)
                    {
                        Toast.MakeText(_context, "Sin internet", ToastLength.Short);
                    }
              
                };
            }
            return view;
        }
    }

    public class Areas
    {
        public int id_pregunta { get; set; }
        public string pregunta { get; set; }
        public string resp_1 { get; set; }
        public string resp_2 { get; set; }
    }

    public class Trabajadores
    {
        public int id_pregunta { get; set; }
        public string pregunta { get; set; }
        public string resp_1 { get; set; }
        public string resp_2 { get; set; }
    }
    
}