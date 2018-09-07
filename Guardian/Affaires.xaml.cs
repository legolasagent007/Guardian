using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
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
using Guardian.Etats_affaire;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;

namespace Guardian
{
    /// <summary>
    /// Logique d'interaction pour Affaires.xaml
    /// </summary>
    public partial class Affaires
    {
        public string tampnom;
        public string tamppren;
        public string tampctct;
        public string tampadr;
        public string tampprof;
        public string tampcdaff;
        public string tampdesc;
        private DataGridTextColumn _documentsColumn = new DataGridTextColumn();
        private DataGridTextColumn _doColumn = new DataGridTextColumn();

        public Affaires()
        {
            InitializeComponent();
            gboxconvoc.IsEnabled = false;
            dgridelmts.Columns.Add(_documentsColumn);
            dgridelmts.Columns.Add(_doColumn);
            _doColumn.Binding = new Binding("doc");
            _doColumn.Visibility = Visibility.Hidden;
            _documentsColumn.Binding = new Binding("nomdoc");
            _documentsColumn.Header = "Liste des documents sur l'affaire";
            Remplirgrid();
        }


        public class Docstock


        {
            public string nomdoc { get; set; }
            public byte[] doc { get; set; }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            gboxconvoc.IsEnabled = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            gboxconvoc.IsEnabled = false;
        }

        private async void dtdepot_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(dtdepot.SelectedDate > DateTime.Today)) return;
            await
                this.ShowMessageAsync("Information",
                    "La date choisie ne peut pas être supérieur à celle d'aujourd'hui",
                    MessageDialogStyle.Affirmative);
            dtdepot.SelectedDate = DateTime.Today;
        }

        private void tbcont_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbcont.MaxLength = 11;
        }



        private async void tb_val_Click(object sender, RoutedEventArgs e)
        {
            string valeur = StringFromRichTextBox(rchaff);
            if (tbcodaff.Text == "")
            {
                await
                    this.ShowMessageAsync("Attention", "Vous devez préciser le code de l'affaire",
                        MessageDialogStyle.Affirmative);
            }
            else if (tbnom.Text == "")
            {
                await this.ShowMessageAsync("Attention", "Vous devez préciser le nom du plaignant");
            }
            else if (tbpren.Text == "")
            {
                await this.ShowMessageAsync("Attention", "Le prénom du plaignant", MessageDialogStyle.Affirmative);
            }
            else if (tbcont.Text == "")
            {
                await this.ShowMessageAsync("Attention", "Le contact du plaignant", MessageDialogStyle.Affirmative);
            }
            else if (tbadre.Text == "")
            {
                await this.ShowMessageAsync("Attention", "L'adresse du plaignant doit être précisée",
                    MessageDialogStyle.Affirmative);
            }
            else if (valeur == "")
            {
                await this.ShowMessageAsync("Attention", "La description de l'affaire est obligatoire",
                    MessageDialogStyle.Affirmative);
            }
            if ((string) tb_val.Content == "Modifier")
            {
                // MessageBox.Show("idée bonne");
                CreateAffaire();
                dgridelmts.Items.Clear();
                await
                    this.ShowMessageAsync("Reussi", "L'affaire a bel et bien été créé", MessageDialogStyle.Affirmative);
                ViderChamp();
                RestorChamp();

            }
            else
            {
                // MessageBox.Show("ca ne marche pas");
                if (!ChekCodeAff())
                {
                    CreateAffaire();
                    dgridelmts.Items.Clear();
                    await
                        this.ShowMessageAsync("Reussi", "L'affaire a bel et bien été créé",
                            MessageDialogStyle.Affirmative);
                    ViderChamp();
                    RestorChamp();
                }
                else
                {
                    //  MessageBox.Show(ChekCodeAff());
                    await this.ShowMessageAsync("Attention", "Il existe déja une affaire portant le même code");
                }


            }

        }

        private bool ChekCodeAff()
        {

            string stock = tbcodaff.Text;

            using (var chkaff = new GuardianEntities1())
            {
                var testcodeaff = from d in chkaff.PLAINTE
                    where d.code_aff == stock
                    select d.code_aff;


                //MessageBox.Show(tampcodaf.ToString());
                // MessageBox.Show(stock);
                return testcodeaff.Any();
            }


        }

        public void UpdateCase()
        {
            newcase.Focus();
            VerrouChamp();

            using (var updcase = new GuardianEntities1())
            {
                bool? regle;
                var upcase = listaff.SelectedItem as Aghata;
                regle = upcase.class_plaign;
                if (regle == true)
                {
                    MessageBox.Show("Vous ne pouvez pas modifier une affaire déja close", "Attention",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else { 
                if (upcase != null)
                {
                    string p = upcase.cd_aff;
                    string desc = upcase.affdesc;
                    int idStk = upcase.id_aff;
                    var requete = (from k in updcase.PLAIGNANT where k.Id_plaignant == idStk select k).FirstOrDefault();

                    if (requete == null) return;
                    tampnom = requete.nom_plai;
                    tamppren = requete.prenom_plai;
                    tampctct = requete.contact_plai;
                    tampadr = requete.adress_plai;
                    tampcdaff = p;
                    tampdesc = desc;
                    tampprof = requete.professi_plai;
                }
                tbcodaff.Text = tampcdaff;
                tbnom.Text = tampnom;
                tbpren.Text = tamppren;
                tbadre.Text = tampadr;
                tbcont.Text = tampctct;
                tb_prof.Text = tampprof;
                rchaff.AppendText(tampdesc);
            }

            }


        }

        private string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange zoneRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);

            return zoneRange.Text;

        }



        private void CreateAffaire()
        {
            try
            {
                using (var affaire = new GuardianEntities1())
                {
                    var affairnew = new PLAIGNANT()
                    {
                        nom_plai = tbnom.Text,
                        prenom_plai = tbpren.Text,
                        contact_plai = tbcont.Text,
                        adress_plai = tbadre.Text,
                        professi_plai = tb_prof.Text,
                    };
                    affaire.PLAIGNANT.Add(affairnew);
                    affaire.SaveChanges();

                    var affid = affaire.PLAIGNANT.Max(c => c.Id_plaignant);


                    if (dtdepot.SelectedDate != null)
                    {
                        var plaintenew = new PLAINTE()
                        {
                            code_aff = tbcodaff.Text,
                            cause_aff = tbcause.Text,
                            PLAIGNANTId_plaignant = affid,
                            professconvo = tbprof.Text,
                            descrip_aff = StringFromRichTextBox(rchaff),
                            nom_convoc = tbconce.Text,
                            date_aff = dtdepot.SelectedDate.Value,
                            classe = ckaff.IsChecked,
                        };
                        affaire.PLAINTE.Add(plaintenew);
                        affaire.SaveChanges();
                    }




                    var docid = affaire.PLAINTE.Max(d => d.Id_aff);

                    foreach (var newdoc in from Docstock s in dgridelmts.Items
                        select new DOCUMENTS
                        {
                            PLAINTEId_aff = docid,
                            docname = s.nomdoc,
                            doc = s.doc,

                        })
                    {
                        affaire.DOCUMENTS.Add(newdoc);
                        affaire.SaveChanges();

                    }
                }

            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }



        }

        private void ViderChamp()
        {
            tbcodaff.Text = "";
            tbnom.Text = "";
            tbpren.Text = "";
            tbcont.Text = "";
            tbadre.Text = "";
            tbprof.Text = "";
            tb_prof.Text = "";
            tbcause.Text = "";
            tbconce.Text = "";
            chkconvoq.IsChecked = false;
            rchaff.Document.Blocks.Clear();
            dgridelmts.Items.Clear();
        }

        private void RestorChamp()
        {
            tbcodaff.IsEnabled = true;
            tbnom.IsEnabled = true;
            tbpren.IsEnabled = true;
            tbcont.IsEnabled = true;
            tbadre.IsEnabled = true;

            tb_prof.IsEnabled = true;


        }

        private void VerrouChamp()
        {
            tbcodaff.IsEnabled = false;
            tbnom.IsEnabled = false;
            tbpren.IsEnabled = false;


            tb_prof.IsEnabled = false;


        }

        private void btannulaff_Click(object sender, RoutedEventArgs e)
        {

            ViderChamp();
            RestorChamp();
            dgridelmts.Items.Clear();
            tb_val.Content = "Enregistrer affaire";
        }

        private void tbcodaff_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbcodaff.MaxLength = 50;
        }

        private void tbcause_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbcause.MaxLength = 50;
        }
        private void tbconce_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbconce.MaxLength = 25;
        }

        private void tbprof_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbprof.MaxLength = 25;
        }

        private void tbnom_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbnom.MaxLength = 25;
        }

        private void tbpren_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbpren.MaxLength = 25;
        }

        private void tbadre_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            tbadre.MaxLength = 30;
        }

        private void tb_prof_TextChanged(object sender, TextChangedEventArgs e)
        {
            tb_prof.MaxLength = 25;
        }

        private async void btimportfile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog docDialog = new OpenFileDialog();
            docDialog.Title = "Sélectionnez le document en format jpeg que vous voulez importer";

            docDialog.ShowDialog();
            if (docDialog.FileName == "")
            {
                await this.ShowMessageAsync("Attention", "Vous n'avez selectionner aucune image");
            }
            else
            {
                dgridelmts.Items.Add(new Docstock
                {
                    nomdoc = docDialog.SafeFileName,
                    doc = File.ReadAllBytes(docDialog.FileName)
                });
            }
        }

        private void btdelfile_Click(object sender, RoutedEventArgs e)
        {
            var deldoc = dgridelmts.SelectedItem as Docstock;
            dgridelmts.Items.Remove(deldoc);
            dgridelmts.Items.Refresh();
        }

        private async void dgridelmts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            await this.ShowMessageAsync("Info", "Faites un simple clic pour sélectionner une ligne");
        }


        public void Remplirgrid()
        {
            try
            {

                using (var remp = new GuardianEntities1())
                {
                    var source = from dl in remp.PLAIGNANT
                        join el in remp.PLAINTE on dl.Id_plaignant equals el.PLAIGNANTId_plaignant into temp
                        from j in temp.DefaultIfEmpty()
                        select new
                        {
                            Id = dl.Id_plaignant,
                            cdaff = j.code_aff,
                            nm = dl.nom_plai,
                            pr = dl.prenom_plai,
                            ctct = dl.contact_plai,
                            ad = dl.adress_plai,
                            dt = j.date_aff,
                            cl = j.classe,
                            dsc = j.descrip_aff,

                        };

                    foreach (var e in source)
                    {
                        listaff.Items.Add(new Aghata
                        {
                            id_aff = e.Id,
                            cd_aff = e.cdaff,
                            n_plaign = e.nm,
                            p_plaign = e.pr,
                            con_plaign = e.ctct,
                            ad_plaign = e.ad,
                            depo_plaign = e.dt,
                            class_plaign = e.cl,
                            affdesc = e.dsc
                        });
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur" + e.ToString());
            }
        }

        public class Aghata
        {
            public int id_aff { get; set; }
            public string cd_aff { get; set; }
            public string n_plaign { get; set; }
            public string p_plaign { get; set; }
            public string con_plaign { get; set; }
            public string ad_plaign { get; set; }
            public DateTime depo_plaign { get; set; }
            public bool? class_plaign { get; set; }
            public string affdesc { get; set; }
        }

        private void tbraffraichaff_Click(object sender, RoutedEventArgs e)
        {
            listaff.Items.Clear();
            Remplirgrid();
        }

        private void tbmodifaff_Click(object sender, RoutedEventArgs e)
        {
            UpdateCase();
            tb_val.Content = "Modifier";
        }

        private void listcase_GotFocus(object sender, RoutedEventArgs e)
        {


        }

        private void tbdetailaff_Click(object sender, RoutedEventArgs e)
        {
            var ligngrid = listaff.SelectedItem as Aghata;


            var f = new casedetail();
            if (ligngrid != null)
            {
                f.lbcdaff.Content = ligngrid.cd_aff;
                f.lbdtenreg.Content = ligngrid.depo_plaign;
                f.lbnom.Content = ligngrid.n_plaign;
                f.lbpren.Content = ligngrid.p_plaign;
                f.lbcont.Content = ligngrid.con_plaign;
                f.lbprof.Content = ligngrid.p_plaign;
                f.lbadr.Content = ligngrid.ad_plaign;
                f.cketat.IsChecked = ligngrid.class_plaign;
                f.rctbox.AppendText(ligngrid.affdesc);

                using (var x = new GuardianEntities1())
                {
                    var query = from s in x.PLAINTE
                                where s.PLAIGNANTId_plaignant == ligngrid.id_aff
                                select s.Id_aff;


                    foreach (var fichier in query)
                    {
                        // MessageBox.Show(fichier.ToString()); ca marche je recupère les id_plainte de toutes les aff
                        //f.listdocaff.Items.Add(fichier);
                        //now il faut les ajouter au grid en se servant de leur nom et nom de l'image elle même



                        var doc = from dl in x.DOCUMENTS
                                  where dl.PLAINTEId_aff == fichier
                                  select new
                                  {
                                      document = dl.docname,
                                      file = dl.doc

                                  };
                        foreach (var VARI in doc)
                        {

                            f.listdocaff.Items.Add(new StockDoc { nom = VARI.document, donne = VARI.file });
                            }

                    }
                }



            }
            f.ShowDialog();
        }

        public class StockDoc
        {
            public string nom { get; set; }
            public byte[] donne { get; set; }
        }

        private void btetat1_Click(object sender, RoutedEventArgs e)
        {
           Liste_affaire essai1=new Liste_affaire();
            essai1.ShowDialog();
        }

        private void btetat2_Click(object sender, RoutedEventArgs e)
        {
            Liste_affaires_classées essai2=new Liste_affaires_classées();
            essai2.ShowDialog();
        }

        private void btetat4_Click(object sender, RoutedEventArgs e)
        {
            var f = new Details_affaire();
            f.ShowDialog();
        }

        private void Affaires1_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btcancel_Click(object sender, RoutedEventArgs e)
        {
            filtcdaff.Clear();
            filnomplai.Clear();
            filnomplai.IsEnabled = true;
            filtcdaff.IsEnabled = true;
            listaff.Items.Clear();
           
            Remplirgrid();

        }

        private void filtcdaff_TextChanged(object sender, TextChangedEventArgs e)
        {
            filnomplai.IsEnabled = false;
        }

        private void filnomplai_TextChanged(object sender, TextChangedEventArgs e)
        {
            filtcdaff.IsEnabled = false;
        }

        private async void tbfiltr_Click(object sender, RoutedEventArgs e)
        {
            if ((filtcdaff.Text == "") && (filnomplai.Text == ""))
            {
                await this.ShowMessageAsync("Attention", "Vous devez précisez un paramètre de recherche");
            }
            else if (filnomplai.Text == "")
            {
                using (var gard = new GuardianEntities1())
                {
                    var filtrcod=from dl in gard.PLAIGNANT
                        join el in gard.PLAINTE on dl.Id_plaignant equals el.PLAIGNANTId_plaignant into temp
                        from j in temp.DefaultIfEmpty()
                       where j.code_aff==filtcdaff.Text
                       select new
                        {
                            Id = dl.Id_plaignant,
                            cdaff = j.code_aff,
                            nm = dl.nom_plai,
                            pr = dl.prenom_plai,
                            ctct = dl.contact_plai,
                            ad = dl.adress_plai,
                            dt = j.date_aff,
                            cl = j.classe,
                            dsc = j.descrip_aff,

                        };

                    foreach (var a in filtrcod)
                    {
                        listaff.Items.Add(new Aghata
                        {
                            id_aff = a.Id,
                            cd_aff = a.cdaff,
                            n_plaign = a.nm,
                            p_plaign = a.pr,
                            con_plaign = a.ctct,
                            ad_plaign = a.ad,
                            depo_plaign = a.dt,
                            class_plaign = a.cl,
                            affdesc = a.dsc
                        });
                    }
                }
                

            }
            else if (filtcdaff.Text == "")
            {
                using (var gard = new GuardianEntities1())
                {var filtrcod = from dl in gard.PLAIGNANT
                        join el in gard.PLAINTE on dl.Id_plaignant equals el.PLAIGNANTId_plaignant into temp
                        from j in temp.DefaultIfEmpty()
                        where dl.nom_plai==filnomplai.Text
                        select new
                        {
                            Id = dl.Id_plaignant,
                            cdaff = j.code_aff,
                            nm = dl.nom_plai,
                            pr = dl.prenom_plai,
                            ctct = dl.contact_plai,
                            ad = dl.adress_plai,
                            dt = j.date_aff,
                            cl = j.classe,
                            dsc = j.descrip_aff,

                        };

                    foreach (var x in filtrcod)
                    {
                        listaff.Items.Add(new Aghata
                        {
                            id_aff = x.Id,
                            cd_aff = x.cdaff,
                            n_plaign = x.nm,
                            p_plaign = x.pr,
                            con_plaign = x.ctct,
                            ad_plaign = x.ad,
                            depo_plaign = x.dt,
                            class_plaign = x.cl,
                            affdesc = x.dsc
                        });
                    }
                }

            }
        }

        private void Affaires1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Accueil z = new Accueil();
                this.Hide();
            z.ShowDialog();
        }
    }
}