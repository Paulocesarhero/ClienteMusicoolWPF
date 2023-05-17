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
        public Login()
        {
            InitializeComponent();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            Usuario usuario = new Usuario
            {
                password = PbPassword.Password,
                username = TbUsername.Text
            };
            UsuarioServices usuarioServices = new UsuarioServices();
            try
            {
                usuarioServices.EnviarToken(usuario);
                InfoUsuario.Instance.Usuario = usuario;
                ValidarOTP validarOtp = new ValidarOTP();
                NavigationService.Navigate(validarOtp);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message,
                    "Ocurrio un error",
                    MessageBoxButton.OK);
            }

        }
    } 
}
