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
            if(TbCodigo.Text != String.Empty)
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
                    MessageBox.Show("Revisa tus codigo, por favor.",
                        "Codigo incorrecto",
                        MessageBoxButton.OK);
                }

                if (esRespuestaPositiva)
                {
                    Reproductor reproductor = new Reproductor();
                    NavigationService.Navigate(reproductor);
                }
            }
            else
            {
                MessageBox.Show(
                        "Necesito tu codigo OTP para poder continuar.", "OTP Vacio",
                        MessageBoxButton.OK);
            }

        }

        private void BtnRegresar_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            NavigationService.Navigate(login);
        }
    }
}