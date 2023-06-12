using musicoolClientWPF.model.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using musicoolClientWPF.Utilidades;

namespace musicoolClientWPF.Vistas
{
    /// <summary>
    /// Lógica de interacción para ValidarOTP.xaml
    /// </summary>
    public partial class ValidarOTP : Page
    {
        public ValidarOTP()
        {
            InitializeComponent();
        }

        private async void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            UsuarioServices usuarioServices = new UsuarioServices();
            bool esRespuestaPositiva = false;
            InfoUsuario.Instance.Usuario.Otp = TbCodigo.Text;
            try
            {
                 esRespuestaPositiva = await usuarioServices.ValidarOtp(InfoUsuario.Instance.Usuario);

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message,
                    "Algún error sucedio en la conexión a la base de datos",
                    MessageBoxButton.OK);
            }

            if (esRespuestaPositiva)
            {
                Reproductor reproductor = new Reproductor();
                NavigationService.Navigate(reproductor);
            }
        }
    }
}
