using System.Windows.Controls;

/* - GENERER LA POSITION DES ELIPSES
 *  for(int i = 0; i< 10; i++)
    {
        int d = 40;//Taille du canvas - taille elispe / 2
        string a = "<Ellipse Opacity=\""+(10-i)/10.0+"\" Canvas.Left=\""+ (d * Math.Cos((Math.PI * 2 * i) / 10) + d) + "\" Canvas.Bottom=\"" +(d * Math.Sin((Math.PI * 2 * i) / 10) + d) + "\"/>";
        a = a.Replace(",", ".");
        Console.WriteLine(a);
    }

    - GENERER LES KEYFRAMES
    for (int i = 0; i < 10; i++)
    {
        double d = 0.1;
        string a = "<EasingDoubleKeyFrame KeyTime=\"0:0:" + (i * d) + "\" Value=\"" + (36 * i) + "\"/>";
        string b = "<EasingDoubleKeyFrame KeyTime=\"0:0:" + (i * d + d) + "\" Value=\"" + (36 * i) + "\"/>";
        a = a.Replace(",", ".");
        b = b.Replace(",", ".");
        Console.WriteLine(a);
        Console.WriteLine(b);
    }
 */

namespace PlanOrd.View.Resources
{
    /// <summary>
    /// Logique d'interaction pour LoadingControl.xaml
    /// </summary>
    public partial class LoadingControl : UserControl
    {
        public LoadingControl()
        {
            InitializeComponent();
        }
    }
}
