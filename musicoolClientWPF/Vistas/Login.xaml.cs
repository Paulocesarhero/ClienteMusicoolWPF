using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
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
using musicoolClientWPF.model.api;
using musicoolClientWPF.model.objetos;
using musicoolClientWPF.Utilidades;

namespace musicoolClientWPF.Vistas
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private UsuarioServices usuarioServices = new UsuarioServices();

        public Login()
        {
            InitializeComponent();
        }

        private async void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            Usuario usuario = new Usuario
            {
                password = PbPassword.Password,
                username = TbUsername.Text
            };

            if(PbPassword.Password != String.Empty && TbUsername.Text != String.Empty) {
                try
                {
                    if (await TratarDeEnviarToken(usuario))
                    {
                        InfoUsuario.Instance.Usuario = usuario;
                        ValidarOTP validarOtp = new ValidarOTP();
                        NavigationService.Navigate(validarOtp);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Revisa tus datos, por favor.", 
                        "Datos incorrectos",
                        MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show(
                        "Necesito tu usuario y contraseña para poder continuar.", "Datos vacios",
                        MessageBoxButton.OK);

            }
        }

        private async Task<bool> TratarDeEnviarToken(Usuario usuario)
        {
            bool result = false;
            do
            {
                try
                {
                    await usuarioServices.EnviarToken(usuario);
                    result = true;
                }
                catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.InternalServerError)
                {
                    MessageBoxResult resultBox = MessageBox.Show("Error al enviar otp ¿Desea intentarlo de nuevo?", "Informacion", MessageBoxButton.YesNo);
                    if (resultBox == MessageBoxResult.Yes)
                    {
                        MessageBox.Show("Enviaremos un nuevo token", "Informacion", MessageBoxButton.OK); ;
                    }
                    if (resultBox == MessageBoxResult.No)
                    {
                        result = true;
                    }
                }
                catch (Exception exception)
                {
                    throw new Exception("Eror credenciales incorrectas", exception);
                }
            } while (result == false);
            return result;
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            Registro registro = new Registro();
            NavigationService.Navigate(registro);
        }
    }
}