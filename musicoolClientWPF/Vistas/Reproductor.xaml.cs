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

        private void LoadSong(string filePath)
        {
            mediaPlayer.Source = new Uri(filePath);
        }

        private void TimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
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
            if (imagenBytes != null)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(imagenBytes);
                bitmapImage.EndInit();
                ImgCancion.Source = bitmapImage;
                ImgCancion.MaxHeight = 300;
                ImgCancion.MaxWidth = 300;
            }

            string cancionpath = await cancionServices.ObtenerCancion(_cancionRespuesta.id);
            if (cancionpath != "")
            {
                mediaPlayer.Source = new Uri(cancionpath);
            }

            List<CancionServices.Comentario> comentarios = await cancionServices.ObtenerComentarios(_cancionRespuesta.id);
            StringBuilder sb = new StringBuilder();
            if (comentarios != null)
            {
                foreach (var comentario in comentarios)
                {
                    sb.AppendLine($"Autor: {comentario.autor}");
                    sb.AppendLine($"Comentario: {comentario.comentario}");
                    sb.AppendLine();
                }

                TbForo.Text = sb.ToString();
            }
        }

        private async void BtnComentar_Click(object sender, RoutedEventArgs e)
        {
            TbComentario.BorderBrush = Brushes.White;

            if (TbComentario.Text != "" && _cancionRespuesta.id != null)
            {
                try
                {
                    if (await cancionServices.ComentarCancion(_cancionRespuesta.id, TbComentario.Text))
                    {
                        List<CancionServices.Comentario> comentarios = await cancionServices.ObtenerComentarios(_cancionRespuesta.id);
                        StringBuilder sb = new StringBuilder();

                        foreach (var comentario in comentarios)
                        {
                            sb.AppendLine($"Autor: {comentario.autor}");
                            sb.AppendLine($"Comentario: {comentario.comentario}");
                            sb.AppendLine();
                        }

                        TbForo.Text = sb.ToString();
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message,
                        "Error en la conexión con la base de datos",
                        MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("Para ser cool tienes que llenar los comentarios",
                    "Campos vacions",
                    MessageBoxButton.OK);
                TbComentario.BorderBrush = Brushes.Red;
            }
        }
    }
}