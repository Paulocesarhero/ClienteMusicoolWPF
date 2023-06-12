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
using ClienteGrpc;
using System.IO;
using System.Security.AccessControl;
using musicoolClientWPF.model.api;
using musicoolClientWPF.model.objetos;
using musicoolClientWPF.Utilidades;
using Path = System.IO.Path;

namespace musicoolClientWPF.Vistas
{
    /// <summary>
    /// Lógica de interacción para Reproductor.xaml
    /// </summary>
    public partial class Reproductor : Page
    {
        private CancionServices cancionServices { get; set; }
        private CancionRespuesta _cancionRespuesta { get; set; }
        public Reproductor()
        {
            InitializeComponent();
            Loaded += Reproductor_Loaded;
        }

        private void Reproductor_Loaded(object sender, RoutedEventArgs e)
        {
            string rol = InfoUsuario.Instance.Usuario.rol;
            cancionServices = new CancionServices();
            if (rol == "artista")
            {
                BtnSubirCancion.IsEnabled = true;
            }
            else
            {
                BtnSubirCancion.IsEnabled = false;
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = volumeSlider.Value;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            Cancion cancion = new Cancion()
            {
                Album = "Alisson",
                NombreArtista = "XFI",
                NombreCancion = "Alisson"
            };
            Data Bytescancion = Servicio.ObtenerCancion(cancion);
            MemoryStream stream = new MemoryStream(Bytescancion.Data_.ToByteArray());
            string tempFilePath = Path.ChangeExtension(Path.GetTempFileName(), ".mp3");
            using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
            {
                stream.WriteTo(fileStream);
            }
            mediaPlayer.Source = new Uri(tempFilePath);
        }
        private void LoadSong(string filePath)
        {
            mediaPlayer.Source = new Uri(filePath);
        }

        private  void TimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TimeSpan newPosition = TimeSpan.FromSeconds(timeSlider.Value);
            mediaPlayer.Position = newPosition;

            currentTimeText.Text = newPosition.ToString(@"mm\:ss");
        }

        private void BtnSubirCancion_Click(object sender, RoutedEventArgs e)
        {
            SubirCancion nuevaPagina = new SubirCancion();
            NavigationService.Navigate(nuevaPagina);
        }

        private async void BtnNombreCancionBuscar_Click(object sender, RoutedEventArgs e)
        { 
            _cancionRespuesta = await cancionServices.BuscarCancion(TbArtistaBucar.Text, TbCancionBuscar.Text);
            TbInfoCancion.Text = String.Format("Canción: {0} Artista: {1} Fecha de publicación: {2}", _cancionRespuesta.nombre, _cancionRespuesta.artista, _cancionRespuesta.fechaDePublicacion);
            byte[] imagenBytes = await cancionServices.ObtenerImagen(_cancionRespuesta.id);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(imagenBytes);
            bitmapImage.EndInit();
            ImgCancion.Source = bitmapImage;
            ImgCancion.MaxHeight = 50;
            ImgCancion.MaxWidth = 50;

            string cancionpath = await cancionServices.ObtenerCancion(_cancionRespuesta.id);

            mediaPlayer.Source = new Uri(cancionpath);


        }
    }
}
