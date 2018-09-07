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

namespace Guardian.Etats_affaire
{
    /// <summary>
    /// Logique d'interaction pour Details_affaire.xaml
    /// </summary>
    public partial class Details_affaire : Window
    {
        public Details_affaire()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var etat = new Etat_sur_une_affaire_en_particulier();
            Documentpreview.DocumentSource = etat;
            etat.CreateDocument();
        }
    }
}
