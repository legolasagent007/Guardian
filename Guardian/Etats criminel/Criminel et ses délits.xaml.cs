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
    /// Logique d'interaction pour Criminel_et_ses_délits.xaml
    /// </summary>
    public partial class Criminel_et_ses_délits : Window
    {
        public Criminel_et_ses_délits()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Fiche_descriptive_du_criminel rapportFiche=new Fiche_descriptive_du_criminel();
            documentpreview3.DocumentSource = rapportFiche;
            rapportFiche.CreateDocument();
        }
    }
}
