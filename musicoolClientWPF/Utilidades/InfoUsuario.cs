using musicoolClientWPF.model.objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicoolClientWPF.Utilidades
{
    public sealed class InfoUsuario
    {
        private static readonly object padlock = new object();
        private Usuario _usuario;
        private static InfoUsuario instance = null;



        public Usuario Usuario
        {
            get { return _usuario; }
            set { _usuario = value; }
        }

        private InfoUsuario()
        {
        }

        public static InfoUsuario Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new InfoUsuario();
                    }
                    return instance;
                }
            }
        }
    }
}
