using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PlanOrd.View.Resources
{
    /// <summary>
    /// Logique d'interaction pour BadgeText.xaml
    /// </summary>
    public partial class BadgeText : Border
    {
        #region Static fields and methods
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(BadgeText), new UIPropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(BadgeText), null);
        #endregion

        /// <summary>
        /// Obtient ou définit le System.Windows.Media.Brush a appliquer au contenu textuel du badge
        /// </summary>
        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        /// <summary>
        /// Obtient ou définit le contenu textuel du badge
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public BadgeText()
        {
            InitializeComponent();
        }
    }
}
