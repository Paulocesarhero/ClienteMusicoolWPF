    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                if (esNumero(Tbtelefono.Text))
                {
                    UsuarioServices usuarioServices = new UsuarioServices();
                    bool result = false;
                    Usuario usuario = new Usuario()
                    {
                        password = TbPassword.Text,
                        username = TbUsername.Text,
                        telefono = Tbtelefono.Text
                    };

                    usuario.telefono = $"+52{usuario.telefono}";


                    if (CbArtista.IsChecked == true)
                    {
                        usuario.rol = "artista";
                    }
                    else
                    {
                        usuario.rol = "escucha";
                    }

                    try
                    {
                        result = await usuarioServices.CrearUsuario(usuario);
                        if (result)
                        {
                            MessageBox.Show($"Bienvenido a Musicool! : {usuario.username}", "Registro Exitoso",
                                MessageBoxButton.OK);
                            Login login = new Login();
                            NavigationService.Navigate(login);
                        }

                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("No se ha podido establecer conexicion, intentelo mas tarde.", "Error de conexion",
                            MessageBoxButton.OK);
                    }
                }
                else
                {
                    MessageBox.Show("El numero de telefono es invalido, favor de corregirlo.", "Numero erroneo",
                            MessageBoxButton.OK);
                }
            }
        }

        private bool esNumero(string texto)
        {
            Regex regex = new Regex(@"^[0-9]+$");

            return regex.IsMatch(texto);
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
