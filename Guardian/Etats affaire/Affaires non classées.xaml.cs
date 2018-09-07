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
using DevExpress.XtraReports.UI;

namespace Guardian.Etats_affaire
{
    /// <summary>
    /// Logique d'interaction pour Affaires_non_classées.xaml
    /// </summary>
    public partial class Affaires_non_classées : Window
    {
        public Affaires_non_classées()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var rapport1 = new Liste_des_affaires_non_classées();
            documentpreview.DocumentSource = rapport1;
            rapport1.CreateDocument();
        }
    }
}
