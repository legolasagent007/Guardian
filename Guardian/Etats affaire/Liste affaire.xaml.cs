﻿using System;
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
    /// Logique d'interaction pour Liste_affaire.xaml
    /// </summary>
    public partial class Liste_affaire : Window
    {
        public Liste_affaire()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var etat = new Liste_des_affaires();
            documentpreview.DocumentSource = etat;
            etat.CreateDocument();
        }
    }
}
