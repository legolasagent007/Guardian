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
    /// Logique d'interaction pour Criminels_et_délits.xaml
    /// </summary>
    public partial class Criminels_et_délits : Window
    {
        public Criminels_et_délits()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Criminel_et_delits rapportCriminelEtDelits=new Criminel_et_delits();
            documentpreview2.DocumentSource = rapportCriminelEtDelits;
            rapportCriminelEtDelits.CreateDocument();
        }
    }
}
