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
using DevExpress.XtraPrinting.Native;

namespace Guardian
{
    /// <summary>
    /// Logique d'interaction pour Authentification_administrateur.xaml
    /// </summary>
    public partial class Authentification_administrateur : Window
    {
        public Authentification_administrateur()
        {
            InitializeComponent();
        }

        private void btcon_Click(object sender, RoutedEventArgs e)
        {
            using (var entite = new GuardianEntities1())
            {
                var verif = from id in entite.USER
                    where id.identifiant_user == tbid.Text

                    select new
                    {
                        id.identifiant_user,
                        id.psswrd_user,
                        id.actif,
                        id.profil

                    };
                if (verif.Count()==0) { MessageBox.Show("Erreur, veuillez reprendre"); }
                 foreach (var essai in verif)
                    {

                        
                         if ((essai.psswrd_user == tbpwd.Password) && (essai.actif == true) && (essai.identifiant_user==tbid.Text) && (essai.profil=="Administrateur"))
                        {MessageBox.Show("Authentification réussie","Vous avez réussi votre authentification");
                           
                            var fenAdmin = new Admin();
                             this.Hide();
                            fenAdmin.ShowDialog();
                        }
                         else if (essai.profil != "Administrateur")
                         {
                             MessageBox.Show("Vous n'êtes pas administrateur, veuillez contacter l'administrateur",
                                 "Erreur");}}

            }
        }
    }
}
