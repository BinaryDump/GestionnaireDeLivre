using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestionnaire_de_livres.Model
{
    class Book : INotifyPropertyChanged
    {

        public Book(string nomAuteur, string nomLivre)
        {
            this.Auteur = nomAuteur;
            this.Titre = nomLivre;
        }

        public Book()
        {
            this.Auteur = String.Empty;
            this.Titre = String.Empty;
        }

        private string _auteur;

        public string Auteur
        {
            get { return _auteur; }
            set
            {
                if (value.Equals(_auteur) != true)
                {
                    this._auteur = value;
                    this.OnPropertyChanged("Auteur");
                }
            }
        }

        private string _titre;

        public string Titre
        {
            get { return _titre; }
            set
            {
                if (value.Equals(_titre) != true)
                {
                    this._titre = value;
                    this.OnPropertyChanged("Titre");
                }
            }
        }



        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


    }
}
