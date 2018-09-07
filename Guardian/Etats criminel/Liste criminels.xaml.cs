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
using System.Windows.Shapes;

namespace Guardian.Etats
{
    /// <summary>
    /// Logique d'interaction pour Liste_criminels.xaml
    /// </summary>
    public partial class Liste_criminels : Window
    {
        public Liste_criminels()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Liste_de_criminels rapport1=new Liste_de_criminels();
            documentpreview1.DocumentSource = rapport1;
            rapport1.CreateDocument();

        }
    }
}
