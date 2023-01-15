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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAnimatedGif;

namespace WPF_PacManGame
{
    /// <summary>
    /// Interaction logic for Win.xaml
    /// </summary>
    public partial class Win : Window
    {
        public static int GameMode;

        public Win(int id)
        {
            GameMode = id;

            InitializeComponent();

            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri("C:\\Users\\jakki\\source\\repos\\WPF-PacManGame\\WPF-PacManGame\\images\\Winner.gif");
            image.EndInit();

            ImageBehavior.SetAnimatedSource(screen, image);

            // Below it needed to start animation. If not, it is only make visible but animation does not start.
            ImageBehavior.SetAutoStart(screen, true);
            ImageBehavior.SetRepeatBehavior(screen, RepeatBehavior.Forever);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            MainWindow window1 = new MainWindow(GameMode);
            window1.ShowDialog();

            window1.Close();
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
