using StyleInterface;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SeaStyle
{
    public class SeaStyle : IStyle
    {
        public string Name
        {
            get
            {
                return "Sea";
            }
        }
        public ResourceDictionary ChangeStyle()
        {
            ResourceDictionary seaStyleResourses = new ResourceDictionary();

            Style firstColor = new Style();
            firstColor.Setters.Add(new Setter(Control.BackgroundProperty, new LinearGradientBrush(Color.FromRgb(22, 36, 81), Color.FromRgb(106, 115, 144), new Point(0, 0), new Point(1, 0))));
            firstColor.Setters.Add(new Setter(Control.BorderBrushProperty, new SolidColorBrush(Color.FromRgb(106, 115, 144))));
            seaStyleResourses.Add("firstColor", firstColor);

            Style secondColor = new Style();
            secondColor.Setters.Add(new Setter(Control.BackgroundProperty, new LinearGradientBrush(Color.FromRgb(0, 119, 155), Color.FromRgb(69, 156, 182), new Point(0, 0), new Point(1, 0))));
            seaStyleResourses.Add("secondColor", secondColor);

            Style thirdColor = new Style();
            thirdColor.Setters.Add(new Setter(Control.BackgroundProperty, new LinearGradientBrush(Color.FromRgb(115, 166, 173), Color.FromRgb(153, 190, 195), new Point(0, 0), new Point(1, 0))));
            seaStyleResourses.Add("thirdColor", thirdColor);

            Style fourthColor = new Style();
            fourthColor.Setters.Add(new Setter(Control.BackgroundProperty, new LinearGradientBrush(Color.FromRgb(166, 207, 213), Color.FromRgb(190, 220, 224), new Point(0, 0), new Point(1, 1))));
            fourthColor.Setters.Add(new Setter(Control.BorderBrushProperty, new SolidColorBrush(Color.FromRgb(115, 166, 173))));
            seaStyleResourses.Add("fourthColor", fourthColor);

            Style fifthColor = new Style();
            fifthColor.Setters.Add(new Setter(Control.BackgroundProperty, new LinearGradientBrush(Color.FromRgb(234, 235, 237), Color.FromRgb(253, 253, 253), new Point(0, 0), new Point(1, 0))));
            seaStyleResourses.Add("fifthColor", fifthColor);

            Style firstTextStyle = new Style();
            Style secondTextStyle = new Style();
            firstTextStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.White));
            secondTextStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.Black));
            seaStyleResourses.Add("firstTextStyle", firstTextStyle);
            seaStyleResourses.Add("secondTextStyle", secondTextStyle);
            return seaStyleResourses;
        }
    }
}
