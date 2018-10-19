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
            if (indentificador == "1")
            {

            }
            else
            {

            }
            return Vista;
        }
    }
    public class AdaptadorRespuestas : BaseAdapter<Respuestas>
    {
        List<Respuestas> lista;
        Activity context;

        public AdaptadorRespuestas(List<Respuestas> lista, Activity context)
        {
            this.lista = lista;
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
            view = context.LayoutInflater.Inflate(Resource.Layout.itemsLista, null);

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