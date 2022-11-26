using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using FontAwesome.Sharp;
using ITVMusic.Util;

namespace ITVMusic.Views {
    /// <summary>
    /// Lógica de interacción para MainView.xaml
    /// </summary>
    public partial class MainView : Window {
        public MainView() {
            InitializeComponent();
            App.CurrentSongChanged += App_CurrentSongChanged;
        }

        private void App_CurrentSongChanged(object? sender, Models.SongModel? e) {

            /*if (m_MediaButton.Content is not IconBlock icon) return;

            icon.Icon = IconChar.PlayCircle;

            if (e is null) return;

            icon.Icon = IconChar.PauseCircle;*/

            /* if (m_MediaButton.Content is not IconBlock icon) return;
            // 
            // model.CurrentSong = e;
            // 
            // icon.Icon = IconChar.PlayCircle;
            // 
            // if (e is null || e.Bytes is null) return;
            // 
            // // MessageBox.Show($"Current song: {e}");
            // 
            // Directory.CreateDirectory(App.UtilFolder);
            // 
            // string currentSongPath = Path.Combine(App.UtilFolder, $"_{e.Id}.mp3");
            // 
            // await e.Bytes.SaveAsync(currentSongPath);
            // 
            // icon.Icon = IconChar.PauseCircle;
            // 
            // m_MediaElement.SetValue(MediaElement.SourceProperty, new Uri(currentSongPath));

            // Crea tu propio MediaPlayer
            // See: https://stackoverflow.com/questions/12462261/how-to-implement-mvvm-with-a-static-class
            */
        }

        private void ControlBar_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Minimized;
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
            if (!IsVisible && IsLoaded) {

                model.NextWindow?.Show();
                Close();

                Application.Current.MainWindow = model.NextWindow;

            }
        }

        private void PopupBox_Closed(object sender, RoutedEventArgs e) {
            m_UserConfigIcon.Icon = IconChar.SortDown;
        }

        private void PopupBox_Opened(object sender, RoutedEventArgs e) {
            m_UserConfigIcon.Icon = IconChar.SortUp;
        }

        private void PlaySongButton_Click(object sender, RoutedEventArgs e) {

            /*if (sender is not Button) return;

            //if (m_MediaElement.Source is null) return;

            if (model.CurrentSong is null) return;

            if (m_MediaButton.Content is not IconBlock icon) return;

            if (icon.Icon is IconChar.CirclePause)
                icon.Icon = IconChar.PlayCircle;
            else
                icon.Icon = IconChar.CirclePause;*/

            /*var state = m_MediaElement.GetMediaState();

            //if (state is MediaState.Play) {
            //    m_MediaElement.Pause();
            //    icon.Icon = IconChar.PlayCircle;
            //    return;
            //}

            // if (state is not MediaState.Manual) {
            //     m_MediaElement.Play();
            //     icon.Icon = IconChar.CirclePause;
            //     return;
             }*/

        }

        /*private void MediaElement_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e) {
            m_MediaElement.Play();
        }

        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e) {
            m_MediaElement.Play();
        }*/
    }
}
