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

namespace DanielProyecto
{
    public class ClaseDato
    {
        public int id_user { get; set; }
        public string user_name { get; set; }
        public string user_password { get; set; }
    }

    public class General
    {
        public void GuardarDatosUsuario(int id_user, string user_name, string user_password)
        {
            var serializador = new XmlSerializer(typeof(ClaseDato));
            var Escritura = new StreamWriter(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ConsultDataUsers.xml"));
            var d = new ClaseDato()
            {
                id_user = id_user,
                user_name = user_name,
                user_password = user_password
            };
            serializador.Serialize(Escritura, d);
            Escritura.Close();
        }
        public ClaseDato ConsultarDatosUsuario()
        {
            try
            {
                var serializador = new XmlSerializer(typeof(ClaseDato));
                var Lectura = new StreamReader(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ConsultDataUsers.xml"));
                var datos = (ClaseDato)serializador.Deserialize(Lectura);
                Lectura.Close();
                return datos;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }
    }
}