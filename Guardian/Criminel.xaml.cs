using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Guardian.Etats;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using DevExpress.XtraMap;
using DevExpress.Map;
using DevExpress.Xpf.Map;
using Guardian;
namespace Guardian
{
    /// <summary>
    /// Logique d'interaction pour Criminel.xaml
    /// </summary>
    public partial class Criminel : MetroWindow
    {
      //  public List<Bob> BobList;
        private DataGridTextColumn deliColumn = new DataGridTextColumn();

       public string phtocrim;
        public string tampmatri;
        public string tampnom;
        public string tamppren;
        public DateTime tampdatenaiss;
        public string tampprofess;
        public string tampcont;
        public Boolean tampdeff;
        public byte[] tampimg; 
        public Decimal? tamptaill;
        public Decimal? tamppoid;
        public string tampnatio;

        //Initialisation du module de recherche
             DevExpress.Xpf.Map.BingSearchDataProvider searchprovier = new DevExpress.Xpf.Map.BingSearchDataProvider();
            
        

        public Criminel()
        {
            InitializeComponent();

            //définition de la grille liste des délits
            dgriddelit.Columns.Add(deliColumn);
            deliColumn.Binding = new Binding("delit");
            deliColumn.Header = "Liste des délits du criminel";
            FillistCrim();

            //Initialisation du module de recherche (configuration)
            recherche.DataProvider = searchprovier;
            searchprovier.BingKey = "cQYFuSValWnct8kgz0WU~yyovoyehqz97kWCdmbrUwg~AsRlS1Da-P1JSecYHKgOlnt1NKjM8DxJeCiLp5zBtEdDV5bZQPyjqvJsrIufdTNu";
            searchprovier.Search("6.150100, 1.230601");

        }

        private void newcrim_Loaded(object sender, RoutedEventArgs e)
        {
            dtenreg.Text = DateTime.Today.ToLongDateString();
        }

        private void btvalider_Click(object sender, RoutedEventArgs e)
        {

            dgriddelit.Items.Add(new crim {delit = cbdelit.Text});




        }

        public class crim
        {

            public string delit { get; set; }
        }
        public class adr
        {
            public double longi { get; set; }
            public double lati { get; set; }
            public double alti { get; set; }
            public string pays { get; set; }
            public string ville { get; set; }
            public string precision { get; set; }
            public string quartier { get; set; }
        }


        public void ViderChamp()
        {

            tbnom.Clear();
            tbnumpc.Clear();
            tbcontact.Clear();
            tbpren.Clear();
            tbjob.Clear();
            tbmatricul.Clear();
            numtaille.Value = 0;
            numpoid.Value = 0;
            deffere.IsChecked = false;
            tblieudeli.Text = "";
            dgriddelit.Items.Clear();
            // listcrim.Items.Clear();
            crimimg.Source=new BitmapImage();
        }

        private void btannulcrim_Click(object sender, RoutedEventArgs e)
        {
            ViderChamp();
            RestorChanm();
        }

        private async void imgbt_Click(object sender, RoutedEventArgs e)
        {
           var envoimage = new OpenFileDialog();
            envoimage.Title = "Sélectionnez l'image correspondant au criminel";

            try
            {
                envoimage.ShowDialog();

                if (envoimage.FileName == "")
                {
                    crimimg.Source = new BitmapImage();
                    phtocrim = "";
                    await this.ShowMessageAsync("Info", "aucune image sélectonné", MessageDialogStyle.Affirmative);
                }
                else
                {
                    crimimg.Source = new BitmapImage(new Uri(envoimage.FileName));
                    phtocrim = envoimage.FileName;

                }
            }
            catch 
            {

                MessageBox.Show("Erreur pendant l'envoie, veuillez réessayer");
            }
            
        }

        private void btsuppdeli_Click(object sender, RoutedEventArgs e)
        {
            crim pr = dgriddelit.SelectedItem as crim;
            dgriddelit.Items.Remove(pr);

        }

        private async void dtDOB_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtDOB.SelectedDate > DateTime.Today)
            {

                await this.ShowMessageAsync("Erreur", "La date que vous choisissez n'est pas la bonne",
                    MessageDialogStyle.Affirmative);
                dtDOB.SelectedDate = DateTime.Today;
            }
        }

        private async void dtenreg_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtenreg.SelectedDate > DateTime.Today)
            {
                await this.ShowMessageAsync("Erreur", "La date que vous choisissez n'est pas la bonne",
                    MessageDialogStyle.Affirmative);
                dtenreg.SelectedDate = DateTime.Today;
            }
        }

        private async void btenregcrim_Click(object sender, RoutedEventArgs e)
        {
            if (!ChekMatricul())
            {
               
           
                if (tbnom.Text == "")
                {
                    if (tbpren.Text == "")
                    {
                        if (cbnatio.Text == "")
                        {
                            
                                await
                                    this.ShowMessageAsync("Attention",
                                        "Tous les champs marqués d'un ' * ' doivent être obligatoirement remplis",
                                        MessageDialogStyle.Affirmative);
                            }
                        }
                    }
                
            else 
            {
                CreateCriminel();
                RestorChanm();
                dgriddelit.Items.Clear();
                dgridlocalisation.Items.Clear();

            }
            }
            else
            {
                await
                 this.ShowMessageAsync("Info", "Il existe déja un criminel avec ce numéro matricule dans la base",
                     MessageDialogStyle.Affirmative);
            }


        }

        private void btmodif_Click(object sender, RoutedEventArgs e)
        {
            
            UpdateCriminel();
            newcrim.Focus();
            tbmatricul.Text = tampmatri;
            tbnom.Text = tampnom;
            tbpren.Text = tamppren;
            dtDOB.SelectedDate = tampdatenaiss;
            tbjob.Text = tampprofess;
            crimimg.Source =new BitmapImage(new Uri(phtocrim));
            cbnatio.Text = tampnatio;
            numpoid.Value = (Double)tamppoid;
            numtaille.Value = (Double) tamptaill;
            tbcontact.Text = tampcont;
            VerrouChamp();



        }

        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
        //Convert BitmapImage to byte[] array
        public static byte[] ConvertToBytes(BitmapImage bitmapImage)
        {
            byte[] data = null;
            using (MemoryStream stream = new MemoryStream())
            {
                WriteableBitmap wBitmap = new WriteableBitmap(bitmapImage);
              //  wBitmap.CopyPixels(stream, wBitmap.PixelWidth, wBitmap.PixelHeight, 0, 100);
                stream.Seek(0, SeekOrigin.Begin);
                data = stream.GetBuffer();
            }
            return data;
        }

        private void dgriddelit_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
        }

        private void btrafraichi_Click(object sender, RoutedEventArgs e)
        {
            listcrim.Items.Clear();
            FillistCrim();

        }

        public void FillistCrim()
        {
           
            using (var list = new GuardianEntities1())
            {

                var source = from cr in list.CRIMINEL
                             join dl in list.DELIT on cr.Id_crim equals dl.CRIMINELId_crim into temp
                             from j in temp.DefaultIfEmpty()

                             select new 
                             {
                                 Id = cr.Id_crim,
                                 matricul = cr.matricul_crim,
                                 nom = cr.nom_crim,
                                 prenom = cr.prenom_crim,
                                 datenaiss = cr.datenaiss_crim,
                                 contact = cr.contatct_crim,
                                 nationa = cr.crim_nationa,
                                 deffere_crim = cr.deffere,
                                 nomdelit = j.nom_delit,
                                 lieudelit = j.lieu_delit,
                                 photocrim = cr.photo_crim,
                                 taill=cr.crim_taill,
                                 poid=cr.crim_poid,
                             };

                foreach (var e in source)
                {
                 
                    listcrim.Items.Add(new Bob { id = e.Id, matricul = e.matricul, nom = e.nom, prenom = e.prenom, datenaiss = e.datenaiss, contact = e.contact,nation = e.nationa,
                        deffere_crim = e.deffere_crim, nom_deli = e.nomdelit, lieu_delit = e.lieudelit, photo = e.photocrim,taille = e.taill,poids = e.poid});  
            }
              
            }

        }

        public void FiltrCrimMatri()
        {
            using (var filtre = new GuardianEntities1())
            {
                var reqfiltr = from cr in filtre.CRIMINEL
                    join dl in filtre.DELIT on cr.Id_crim equals dl.CRIMINELId_crim into temp
                    from j in temp.DefaultIfEmpty()
                    where cr.crim_nationa== tbfiltmatr.Text
                    select new
                    {
                        Id = cr.Id_crim,
                        matricul = cr.matricul_crim,
                        nom = cr.nom_crim,
                        prenom = cr.prenom_crim,
                        datenaiss = cr.datenaiss_crim,
                        contact = cr.contatct_crim,
                        nationa = cr.crim_nationa,
                        deffere_crim = cr.deffere,
                        nomdelit = j.nom_delit,
                        lieudelit = j.lieu_delit,
                        photocrim = cr.photo_crim,
                        taill = cr.crim_taill,
                        poid = cr.crim_poid,

                    };
                foreach (var e in reqfiltr)
                {

                    listcrim.Items.Add(new Bob { id = e.Id, matricul = e.matricul, nom = e.nom, prenom = e.prenom, datenaiss = e.datenaiss, contact = e.contact, nation = e.nationa, deffere_crim = e.deffere_crim, nom_deli = e.nomdelit, lieu_delit = e.lieudelit, photo = e.photocrim, taille = e.taill, poids = e.poid });
                }
            }
            
        }

        public void FiltrCrimNom()
        {
            using (var nomfiltr = new GuardianEntities1())
            {
                var reqnom = from cr in nomfiltr.CRIMINEL
                    join dl in nomfiltr.DELIT on cr.Id_crim equals dl.CRIMINELId_crim 
                    join ab in nomfiltr.AdresseSet on cr.Id_crim equals ab.CRIMINELId_crim into temp
                    from j in temp.DefaultIfEmpty()
                    where cr.nom_crim == tbfiltrnom.Text
                    select new
                    {
                        Id = cr.Id_crim,
                        matricul = cr.matricul_crim,
                        nom = cr.nom_crim,
                        prenom = cr.prenom_crim,
                        datenaiss = cr.datenaiss_crim,
                        contact = cr.contatct_crim,
                        nationa = cr.crim_nationa,
                        deffere_crim = cr.deffere,
                        nomdelit =dl.nom_delit,
                        lieudelit = dl.lieu_delit,
                        photocrim = cr.photo_crim,
                        taill = cr.crim_taill,
                        poid = cr.crim_poid,
                        longi=j.longitude,
                        lati=j.latitude,

                    };
                foreach (var e in reqnom)
                {

                    listcrim.Items.Add(new Bob { id = e.Id, matricul = e.matricul, nom = e.nom, prenom = e.prenom, datenaiss = e.datenaiss, contact = e.contact, nation = e.nationa,
                        deffere_crim = e.deffere_crim, nom_deli = e.nomdelit, lieu_delit = e.lieudelit, photo = e.photocrim, taille = e.taill, poids = e.poid,
                    lati=e.lati, longi=e.longi});
                }
            }
        }
       

        public void UpdateCriminel()
        {       
            
            using (GuardianEntities1 updte = new GuardianEntities1())
            {
                var crimetat = listcrim.SelectedItem as Bob;
                if (crimetat != null)
                {
                    string m = crimetat.matricul;
                   // MessageBox.Show(m);
                var query = (from p in updte.CRIMINEL where p.matricul_crim == m select p).FirstOrDefault();
                tampmatri = query.matricul_crim;
                tampnom = query.nom_crim;
                tamppren = query.prenom_crim;
                tampdatenaiss = query.datenaiss_crim;
                tampprofess = query.crim_profes;
                tampcont = query.contatct_crim;
                tampimg = query.photo_crim;
                tamppoid = query.crim_poid;
                tamptaill = query.crim_taill;
                tampnatio = query.crim_nationa;
                tampprofess = query.crim_profes;


                }
                else
                {
                    MessageBox.Show("error");
                }


            }
             ArrayToImage(tampimg);
            
            OpenFileDialog test=new OpenFileDialog();
            test.FileName =@"C:\Temp\img.jpg";
            phtocrim = test.FileName;



        }

        public async void CreateCriminel() 
        {
           
            try
            {
                using (var criminel = new GuardianEntities1()){
                    if (dtDOB.SelectedDate != null)
                    {
                        if (dtenreg.SelectedDate != null)
                        {
                            if (phtocrim != null)
                            {
                                var crimnew = new CRIMINEL()
                                {
                                    matricul_crim = tbmatricul.Text,
                                    crim_nationa = cbnatio.Text,
                                    nom_crim = tbnom.Text,
                                    crim_poid = (decimal) numpoid.Value,
                                    crim_taill = (decimal) numtaille.Value,
                                    prenom_crim = tbpren.Text,
                                    photo_crim = File.ReadAllBytes(phtocrim),
                                    crim_profes = tbjob.Text,
                                    datenaiss_crim = dtDOB.SelectedDate.Value,
                                    date_enreg = dtenreg.SelectedDate.Value,
                                    typ_piece = cbtyppc.Text,
                                    num_piece = tbnumpc.Text,
                                    deffere = deffere.IsChecked,
                                    contatct_crim = tbcontact.Text,
                                };

                                criminel.CRIMINEL.Add(crimnew);
                                criminel.SaveChanges();
                            }
                        }
                    }
                    
                    phtocrim = "";
                    var crimid = criminel.CRIMINEL.Max(p => p.Id_crim);
                    // MessageBox.Show(crimid.ToString());

                    //ajoute chaque ligne du datagrid a la table delit en prenant comme crimid le dernier id de criminel

                    foreach (var delitnew in from crim c in dgriddelit.Items
                                             select new DELIT()
                                             {
                                                 CRIMINELId_crim = crimid,
                                                 nom_delit = c.delit,
                                                 lieu_delit = tblieudeli.Text,

                                             })
                    {
                        criminel.DELIT.Add(delitnew);
                        criminel.SaveChanges();
                    }

                    dgriddelit.Items.Clear();

                    //ajoute chaque ligne du datagrid a la table adresse en prenant comme crimid le dernier id de criminel
                    foreach (var adressnew in from adr b in dgridlocalisation.Items
                                              select new Adresse()
                                              {
                                                  CRIMINELId_crim = crimid,
                                                  altitude = b.alti,
                                                  latitude = b.lati,
                                                  longitude = b.longi,
                                                  pays = b.pays,
                                                  precision = b.precision,
                                                  quartier = b.quartier,
                                                  ville = b.ville,
                                                  

                                              })
                    {
                        criminel.AdresseSet.Add(adressnew);
                        criminel.SaveChanges();
                    }

                    dgridlocalisation.Items.Clear();
                            
                  
                    await this.ShowMessageAsync("Réussi", "Le nouveau criminel a bien été enregistré",
                        MessageDialogStyle.Affirmative);

                    ViderChamp();

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Attention Erreur \n" +e.ToString());
               
            }
            


        }


       public void VerrouChamp()
       {
           tbmatricul.IsEnabled = false;
           tbnom.IsEnabled = false;
           tbpren.IsEnabled = false;
           dtDOB.IsEnabled = false;
           tbjob.IsEnabled = false;
           cbnatio.IsEnabled = false;
           numpoid.IsEnabled = false;
           numtaille.IsEnabled = false;


       } 
       public void RestorChanm()
       {
           tbmatricul.IsEnabled = true;
           tbnom.IsEnabled = true;
           tbpren.IsEnabled = true;
           dtDOB.IsEnabled = true;
           tbjob.IsEnabled = true;
           cbnatio.IsEnabled = true;
           numpoid.IsEnabled = true;
           numtaille.IsEnabled =true;
    
}

       private async void listcrim_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
       { 
           await
               this.ShowMessageAsync("Attention",
                   "Pour modifier un criminel veuillez faire un clique sur sa ligne et cliquer ensuite sur modifier",
                   MessageDialogStyle.Affirmative); 
           
       }

       private async void listcrim_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
       {
           

               await
                   this.ShowMessageAsync("Attention",
                       "Pour modifier un criminel veuillez faire un clique sur sa ligne et cliquer ensuite sur modifier",
                       MessageDialogStyle.Affirmative);
         
       }

        public void ArrayToImage(byte[] tampimage)
       {
            using (FileStream file = new FileStream(@"C:\Temp\img.jpg",FileMode.Create))
            {
                file.Write(tampimage, 0, tampimage.Count());
                
            }
        }

        private void tbmatricul_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbmatricul.MaxLength = 5;
        }

        private void tbcontact_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbcontact.MaxLength = 11;
        }

        private bool ChekMatricul()
        {
            //IQueryable tampon;
            using (var ckmatri = new GuardianEntities1())
            {
                var matest = from c in ckmatri.CRIMINEL
                    where c.matricul_crim == tbmatricul.Text
                    select c.matricul_crim;
              //  tampon = matest;
                return matest.Any();
            }
           
        }

        private void tbfiltmatr_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbfiltmatr.Text == "")
            {
                tbfiltrnom.IsEnabled = true;
            }
            else tbfiltrnom.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            listcrim.Items.Clear();
            FillistCrim();
            tbfiltmatr.Clear();
            tbfiltrnom.Clear();
        }

        private void btrech_Click(object sender, RoutedEventArgs e)
        {
            if (tbfiltmatr.Text != "")
            {
                listcrim.Items.Clear();
                FiltrCrimMatri();
            }
            else
            {
                listcrim.Items.Clear();
                FiltrCrimNom();
            }
        }

        private void tbfiltrnom_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbfiltrnom.Text == "")
            {
                tbfiltmatr.IsEnabled = true;
            }
            else tbfiltmatr.IsEnabled = false;
        }

        private void btetat1_Click(object sender, RoutedEventArgs e)
        {
            Liste_criminels f=new Liste_criminels();
            f.ShowDialog();
        }

        private void btetat3_Click(object sender, RoutedEventArgs e)
        {
            Criminels_et_délits g=new Criminels_et_délits();
            g.ShowDialog();
        }

        private void btetat2_Click(object sender, RoutedEventArgs e)
        {
            Criminel_et_ses_délits fcrim=new Criminel_et_ses_délits();
            fcrim.ShowDialog();}

        private void cbbox_pays_Loaded(object sender, RoutedEventArgs e)
        {
            cbbox_pays.Items.Add("Togo");
            cbbox_pays.Items.Add("Bénin");
            cbbox_pays.Items.Add("Ghana");
            cbbox_pays.Items.Add("Côte d'Ivoire");
            cbbox_pays.Items.Add("Nigéria");
            cbbox_pays.Items.Add("Burkina Faso");
            cbbox_pays.Items.Add("Niger");
            cbbox_pays.Items.Add("Sénégal");
            cbbox_pays.Items.Add("Mali");
            cbbox_pays.Items.Add("Gambie");
        }

        private void tbajout_Click(object sender, RoutedEventArgs e)
        {
            dgridlocalisation.Items.Add(new adr
            {
                alti = (double)tb_altit.Value,
                lati = (double)tb_latit.Value,
                longi = (double)tb_longit.Value,
                pays = cbbox_pays.Text,
                ville = tbville.Text,
                quartier = tbquartier.Text,
                precision = tb_precision.Text
            });
        }

        private void tb_retire_Click(object sender, RoutedEventArgs e)
        {
            adr pr = dgridlocalisation.SelectedItem as adr;
            dgridlocalisation.Items.Remove(pr);
        }

        private void dgridlocalisation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listcrim_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dgridlocalisation_Loaded(object sender, RoutedEventArgs e)
        {
            //définition de la grille des adresses

            //dgridlocalisation.Columns.Add(latcolumn);
            //latcolumn.Binding = new Binding("lati");
            //latcolumn.Header = "Latitude";

            //dgridlocalisation.Columns.Add(longcolumn);
            //latcolumn.Binding = new Binding("longi");
            //latcolumn.Header = "Longitude";

            //dgridlocalisation.Columns.Add(altitud);
            //latcolumn.Binding = new Binding("alti");
            //latcolumn.Header = "Altitude";

            //dgridlocalisation.Columns.Add(pays);
            //latcolumn.Binding = new Binding("pays");
            //latcolumn.Header = "Pays";

            //dgridlocalisation.Columns.Add(quartier);
            //latcolumn.Binding = new Binding("quartier");
            //latcolumn.Header = "Quartier";

            //dgridlocalisation.Columns.Add(ville);
            //latcolumn.Binding = new Binding("ville");
            //latcolumn.Header = "Ville";

            //dgridlocalisation.Columns.Add(precision);
            //latcolumn.Binding = new Binding("precision");
            //latcolumn.Header = "Précisions";
        }

        private void btprintmaps_Click(object sender, RoutedEventArgs e)
        {
            //carte_criminels.ShowRibbonPrintPreview(this);
            
        }

        private void carte_criminels_Loaded(object sender, RoutedEventArgs e)
        {
            //var pushpinLayer = new MapLayer();
            //pushpinLayer.Name = "PushPinLayer";
            //carte_criminels.Children.Add(pushpinLayer);

            //var location = new Location(GetLattitude(), GetLongitude);
            //var pushpin = new Pushpin();
            //pushpin.Name = "My New Pushpin";
            //pushpin.Background = new SolidColorBrush(Colors.Blue);
            //pushPinLayer.AddChild(pushpin);

            //on met un marquer par défaut sur le DCPJ lors du chargement de la page d'accueil


            //VectorItemsLayer vlayer = (VectorItemsLayer)this.carte_criminels.Layers[0];

            //  CoordPoint point = this.carte_criminels.ScreenPointToCoordPoint(new MapPoint(e.X, e.Y));

            //if (vlayer.Data == null)
            //    vlayer.Data = storage;
            //else
            // storage = vlayer.Data as MapItemStorage;

          
            


        }

        private void btprintcrims_Click(object sender, RoutedEventArgs e)
        {

           


        }

        private void mapresearch_SearchCompleted(object sender, DevExpress.Xpf.Map.BingSearchCompletedEventArgs e)
        {
            

            //foreach (DevExpress.Xpf.Map.BingLocationInformation resultInfo in e.RequestResult.SearchResults)
            //{
            //    var point = new DevExpress.Xpf.Map.MapPushpin();

            //    point.Location = resultInfo.Location;
            //    point.Information = "DCPJ Localisation de test";
            //    point.IsHighlighted = true;
            //    point.Text = "Localisation de la direction centrale de la police";

            //    mapItems.Items.Add(point);

            //}



            //carte_criminels.ZoomToFitLayerItems();
        }

        private void searchprovier_SearchCompleted(object sender, DevExpress.Xpf.Map.BingSearchCompletedEventArgs e)
        {


            foreach (DevExpress.Xpf.Map.BingLocationInformation resultInfo in e.RequestResult.SearchResults)
            {
                var point = new DevExpress.Xpf.Map.MapPushpin();

                point.Location = resultInfo.Location;
                point.Information = "DCPJ Localisation de test";
                point.IsHighlighted = true;
                point.Text = "Localisation de la direction centrale de la police";
                point.Visible = true;
                mapItems.Items.Add(point);

            }



            carte_criminels.ZoomToFitLayerItems();
        }
        
        private void VectorLayer_Loaded(object sender, RoutedEventArgs e)
        {
           

        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Accueil p = new Accueil();
            this.Hide();
            p.ShowDialog();
        }
    }


    public class Bob
    {
        public int id { get; set; }
        public string matricul { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public DateTime datenaiss { get; set; }
        public string contact { get; set; }
        public string nation { get; set; }
        public bool? deffere_crim { get; set; }
        public string nom_deli { get; set; }
        public string lieu_delit { get; set; }
        public byte[] photo { get; set; }
        public decimal? taille { get; set; }
        public decimal? poids { get; set; }
        public double? lati { get; set; }
        public double? longi { get; set; }

    }
}