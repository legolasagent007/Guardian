using System;
using System.Collections.Generic;
using System.IO;
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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace Guardian
{
    /// <summary>
    /// Logique d'interaction pour Admin.xaml
    /// </summary>
    public partial class Admin : MetroWindow
    {
        public byte[] tampimg;
        public string adimg;
        public Admin()
        {
            InitializeComponent();
            dob_user.SelectedDate = DateTime.Today; InitializeComponent();
           FillDglistuer();
         //   Filloglist();
        }

        private void btannuler_Click(object sender, RoutedEventArgs e)
        {
            Videchamp();
            

        }

        private async void bt_img_Click(object sender, RoutedEventArgs e)
        {
             OpenFileDialog envoimage=new OpenFileDialog();
          envoimage.Title = "Sélectionnez l'image correspondant au criminel";
          
          envoimage.InitialDirectory="c:\\users";
          envoimage.ShowDialog();
            adimg = envoimage.FileName;
          if (envoimage.FileName=="")
          {
              await this.ShowMessageAsync("Info", "aucune image sélectonné", MessageDialogStyle.Affirmative);
          }
          else
          {
             
            user_img.Source = new BitmapImage(new Uri(envoimage.FileName));
              tampimg = File.ReadAllBytes(envoimage.FileName);

          }
        }

        private void FillDglistuer()
        {
            try
            {
                using (var e = new GuardianEntities1())
                {
                    var req = from d in e.USER
                        select new
                        {name= d.nom_user,
                          surname=d.prenom_user,
                          email=d.mail_user,
                          tel=d.contact,
                          id=d.identifiant_user,
                          psswd=d.psswrd_user,
                          state=d.actif,
                          img=d.photo_user,
                          profil_user=d.profil,
                          
                        };
                    foreach (var bs in req)
                    {
                        dglistuser.Items.Add(new StockUser
                        {
                        nom=bs.name,
                        prenom = bs.surname,
                        mail = bs.email,
                        ctct = bs.tel,
                        iduser = bs.id,
                        pwd = bs.psswd,
                        account = bs.state,
                        view = bs.img,
                        prfl=bs.profil_user,
    
                        });
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            


        }
        private class StockUser

        {
            public string nom { get; set; }
            public string prenom { get; set; }
            public string mail { get; set; }
            public string ctct { get; set; }
            public string iduser { get; set; }
            public string pwd { get; set; }
            public bool? account { get; set; }
            public byte[] view { get; set; }
            public string prfl{ get; set; }


        }

        private void Filloglist()
        {
            String ConString =
               ConfigurationManager.ConnectionStrings["Guardian.Properties.Settings.GuardianConnectionString"]
                   .ConnectionString;
            string cmdstring = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                cmdstring = "SELECT [identifiant_log],[nom_log],[grade],[date_log],[user_photo] FROM [Guardian].[dbo].[LOG]";
                SqlCommand cmd = new SqlCommand(cmdstring, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("LOG");
                sda.Fill(dt);
                loglist.ItemsSource = dt.DefaultView;
            }
        }

        private void Videchamp()
        {
            tb_contact.Clear();
            tb_grade.Clear();
            tb_id.Clear();
            tb_pwd.Clear();
            tb_mail.Clear();
            tb_prenm.Clear();
            tb_usernom.Clear();
            dob_user.Text = "Sélectionnez une date";
            cbprofil.SelectedItem = "";



            user_img.Source = new BitmapImage();

            ckactive.IsEnabled = false;
        }

        private void VerrouChamp()
        {

            tb_grade.IsEnabled = false;
            
           
            tb_mail.IsEnabled = false;
            tb_prenm.IsEnabled = false;
            tb_usernom.IsEnabled = false;
            dob_user.IsEnabled = false;
           
            
        }

        private void RestaurChamp()
        {
            tb_grade.IsEnabled = true;
            tb_prenm.IsEnabled = true;
            tb_usernom.IsEnabled = true;
            dob_user.Text = "Sélectionnez une date";

        }

        public void CreateAdmin()
        {
            try
            {
                using (var adm = new GuardianEntities1())
                {
                    if (dob_user.SelectedDate != null)
                    {
                        var ad = new USER()
                        { nom_user = tb_usernom.Text,
                            prenom_user = tb_prenm.Text,
                            dob_user = dob_user.SelectedDate.Value,
                            contact = tb_contact.Text,
                            grade_user = tb_grade.Text,
                            identifiant_user = tb_id.Text,
                            psswrd_user = tb_pwd.Password,
                            mail_user = tb_mail.Text,
                            photo_user = tampimg,
                            actif =  (bool) ckactive.IsChecked,
                            profil = cbprofil.Text,
                        };
                        adm.USER.Add(ad);
                       
                    }
                    adm.SaveChanges();

                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private async void bt_valid_Click(object sender, RoutedEventArgs e)
        {
            if (dob_user.SelectedDate > DateTime.Today)
            {
                await this.ShowMessageAsync("Erreur", "la date de naissance est incorrecte");
                dob_user.SelectedDate = DateTime.Today;
            }
            else if (tb_usernom.Text == "")
            {
                await this.ShowMessageAsync("info", "Veuillez préciser le nom du futur utilisateur");
            }
            else if (tb_prenm.Text == "")
            {
                await this.ShowMessageAsync("info", "veuilez préciser le prénom du futur utilisateur");
            }
            else if (tb_contact.Text == "")
            {
                await this.ShowMessageAsync("info", "Veuillez préciser le contact de l'utilisateur");
            }
            else   if (tb_mail.Text == "")
            {
                await this.ShowMessageAsync("info", "Vous devez préciser le mail");

            }
            else if (tb_id.Text == "")
            {
                await this.ShowMessageAsync("info", "Veuillez préciser l'identifiant utilisé par l'utilisateur");
            }
            else if (tb_pwd.Password == "")
            {
                await this.ShowMessageAsync("info", "vous devez préciser le mot de passe de l'utilisateur");
            }
            else
            {
                CreateAdmin();
                Videchamp();
                await this.ShowMessageAsync("Info", "utilisateur enregistré", MessageDialogStyle.Affirmative);
            }
            
        }

        private void tb_usernom_TextChanged(object sender, TextChangedEventArgs e)
        {
            tb_usernom.MaxLength = 10;
        }

        private void tb_prenm_TextChanged(object sender, TextChangedEventArgs e)
        {
            tb_prenm.MaxLength = 15;
        }

        private void tb_contact_TextChanged(object sender, TextChangedEventArgs e)
        {
            tb_contact.MaxLength = 11;
        }

        private void tb_grade_TextChanged(object sender, TextChangedEventArgs e)
        {
            tb_grade.MaxLength = 15;
        }

        private void tb_mail_TextChanged(object sender, TextChangedEventArgs e)
        {
            tb_mail.MaxLength = 25;
        }

        private void tb_id_TextChanged(object sender, TextChangedEventArgs e)
        {
            tb_id.MaxLength = 15;
        }

        private void tb_pwd_TextInput(object sender, TextCompositionEventArgs e)
        {
            tb_pwd.MaxLength = 15;
        }

        private void cbprofil_TextInput(object sender, TextCompositionEventArgs e)
        {
            
        }

        private void tbrafrai_Click(object sender, RoutedEventArgs e)
        {  dglistuser.Items.Clear();
            FillDglistuer();
        }

        private void admin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {   
            Accueil form = new Accueil();
            this.Hide();
            form.ShowDialog();
        }
    }

}
