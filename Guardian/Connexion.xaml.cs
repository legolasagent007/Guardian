using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Controls.Internal;
using DevExpress.XtraPrinting.Native;
using MahApps.Metro;
using MahApps.Metro.Controls;
//using Xceed.Wpf.DataGrid.FilterCriteria;

namespace Guardian
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btconnexion_Click(object sender, RoutedEventArgs e)
        {
           
           
            VerifIdentifiant();
        }

        private void Connexion_Loaded(object sender, RoutedEventArgs e)
        {
            using (var x = new GuardianEntities1())
            { 
            }
           // var t = new Accueil();
           // this.Owner =new Accueil();
        }

        private  void VerifIdentifiant()
        {
            try
            {using (var entite = new GuardianEntities1())
                {
                    if (entite.Database.Exists())
                    {
                        //MessageBox.Show("Base de donnée crée");
                    }                    
                    else
                    {
                        MessageBox.Show("Base de données absente, elle va être crée sur votre seveur en local");
                        entite.Database.CreateIfNotExists();

                    }

                    var verif = from id in entite.USER
                                where id.identifiant_user==tbid.Text 
                                 
                                select new
                        {  id.identifiant_user,
                            id.psswrd_user,
                            id.actif,

                        };

                    if (verif.Count()==0) { MessageBox.Show("Erreur, veuillez reprendre"); }
                    foreach (var essai in verif)
                    {

                        
                         if ((essai.psswrd_user == TbBox.Password) && (essai.actif == true) && (essai.identifiant_user==tbid.Text))
                        {MessageBox.Show("Authentification réussie","Vous avez réussi votre authentification");
                           Videchamp();
                            Accueil fenAccueil = new Accueil();
                            this.Hide();
                            fenAccueil.Show();
                           // this.Close();
                         }
                         else if ((essai.psswrd_user == TbBox.Password) && (essai.actif == false))
                         {
                            MessageBox.Show("Votre compte n'est pas activé, veuillez contacter l'administrateur");
                             Videchamp();
                         }
                         else if ((essai.psswrd_user != TbBox.Password) && (essai.identifiant_user != tbid.Text)) { { MessageBox.Show("Erreur, veuillez reprendre"); } }   
                    }
               }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void Videchamp()
        {
            tbid.Clear();
            TbBox.Clear();
            
        }
    }
}
