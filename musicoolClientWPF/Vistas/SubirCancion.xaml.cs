using Microsoft.Win32;
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

namespace musicoolClientWPF.Vistas
{
    /// <summary>
    /// Lógica de interacción para SubirCancion.xaml
    /// </summary>
    public partial class SubirCancion : Page
    {
        public string ImageFilePath { get; set; }
        public string SongFilePath { get; set; }

        public SubirCancion()
        {
            InitializeComponent();
        }

        private void SeleccionarImagen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos JPG (*.jpg, *.jpeg)|*.jpg;*.jpeg";
            openFileDialog.Title = "Guardar archivo";

            if (openFileDialog.ShowDialog() == true)
            {
                ImageFilePath = openFileDialog.FileName;
            }
        }

        private void SeleccionarCancion_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos MP3 (*.mp3)|*.mp3";
            openFileDialog.Title = "Guardar archivo";

            if (openFileDialog.ShowDialog() == true)
            {
                SongFilePath = openFileDialog.FileName;
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            CancionServices cancionServices = new CancionServices();

            if (ImageFilePath != null && SongFilePath != null)
            {
                try
                {
                    if (!valoresVacios())
                    {
                        string idCancion = await cancionServices.RegistrarCancio(TbCancion.Text, TbArtista.Text);
                        bool result = cancionServices.SubirCancion(ImageFilePath, SongFilePath, idCancion);
                        if (result)
                        {
                            MessageBox.Show("Registro exitoso",
                                $"Felicidades tu canción {TbArtista.Text} ahora es cool",
                                MessageBoxButton.OK);
                            Reproductor regresarPagina = new Reproductor();
                            NavigationService.Navigate(regresarPagina);
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error de conexion",
                        "Conexion no disponible",
                        MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("Campos vacios",
                    "Seleccione una imagen y una canción primero",
                    MessageBoxButton.OK);
            }
        }

        private bool valoresVacios()
        {
            if (TbArtista.Text == "")
            {
                TbArtista.BorderBrush = new SolidColorBrush(Colors.Red);
            }

            if (TbCancion.Text == "")
            {
                TbCancion.BorderBrush = new SolidColorBrush(Colors.Red);
            }

            return TbArtista.Text == "" || TbCancion.Text == "";
        }

        private void BtnRegresar_Click(object sender, RoutedEventArgs e)
        {
            Reproductor reproductor = new Reproductor();
            NavigationService.Navigate(reproductor);
        }
    }
}