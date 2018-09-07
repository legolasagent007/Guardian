using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Guardian
{
    /// <summary>
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class Accueil : MetroWindow
    {
        public Accueil()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            tbheur.Text = DateTime.Now.ToLongDateString();
            HeurBox.Text = DateTime.UtcNow.ToShortTimeString();
        }

        private void tlaffa_Click(object sender, RoutedEventArgs e)
        {
            
            Affaires fenAffaires=new Affaires();
            this.Hide();
            fenAffaires.ShowDialog();
        }

        private void tlcrim_Click(object sender, RoutedEventArgs e)
        {
           
            Criminel fenCriminel=new Criminel();
            this.Hide();
            fenCriminel.ShowDialog();
        }

        private void tladmin_Click(object sender, RoutedEventArgs e)
        {
           
          var fenAdmin=new Authentification_administrateur();
            this.Hide();
            fenAdmin.ShowDialog();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
           // Application.Exit();
          // Environment.Exit()
         
            
        }

        private void WindowsFormsHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
    }
}
