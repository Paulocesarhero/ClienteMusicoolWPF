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

namespace musicoolClientWPF.Vistas
{
    /// <summary>
    /// Lógica de interacción para Registro.xaml
    /// </summary>
    public partial class Registro : Page 
    {
        public Registro()
        {
            InitializeComponent();
        }

        private async void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (!CamposVacios())
            {
                UsuarioServices usuarioServices = new UsuarioServices();
                bool result = false;
                Usuario usuario = new Usuario()
                {
                    password = TbPassword.Text,
                    username = TbUsername.Text,
                    telefono = Tbtelefono.Text
                };
                if (CbArtista.IsChecked == true)
                {
                    usuario.rol = "artista";
                }

                if (CbArtista.IsChecked == false)
                {
                    usuario.rol = "escucha";
                }
                try
                {
                    result = await usuarioServices.CrearUsuario(usuario);
                    if (result)
                    {
                        MessageBox.Show("Registro exitoso",
                            "Se ha guardado un nuevo usuario en la base de datos",
                            MessageBoxButton.OK);
                        Login login = new Login();
                        NavigationService.Navigate(login);
                    }

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message,
                        "Error en la conexión con la base de datos",
                        MessageBoxButton.OK);
                }

            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            NavigationService.Navigate(login);
        }

        private Boolean CamposVacios()
        {
            if (TbPassword.Text == "")
            {
                TbPassword.BorderBrush = Brushes.Red;
            }

            if (TbUsername.Text == "")
            {
                TbUsername.BorderBrush = Brushes.Red;
            }

            if (Tbtelefono.Text == "")
            {
                Tbtelefono.BorderBrush = Brushes.Red;
            }
            return TbPassword.Text == "" || Tbtelefono.Text == "" || TbUsername.Text == "";
        }
    }
}
