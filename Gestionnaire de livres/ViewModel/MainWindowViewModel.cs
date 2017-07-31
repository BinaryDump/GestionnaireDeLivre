using Gestionnaire_de_livres.Model;
using Gestionnaire_de_livres.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Gestionnaire_de_livres.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {

        public MainWindowViewModel()
        {

            FileName = Directory.GetCurrentDirectory().ToString() + "\\books.dat";

            if (!File.Exists(FileName))
            {
                File.Create(FileName).Close();
                MessageBox.Show("Un fichier \"books.dat\" a été créé dans le dossier du programme, ce fichier contient les informations permettant de générer la liste de livre que vous possédez, ne le supprimez donc pas.",
                                "Fichier créé",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                LoadList();
                BookSelected = new Book();
            }
            else
            {
                LoadList();
            }




        }

        #region ## Methods ##

        public void LoadList()
        {
            try
            {
                BookList = new ObservableCollection<Book>();
                string[] _bookArray = File.ReadAllLines(FileName);
                if (_bookArray.Any())
                {
                    foreach (string item in _bookArray)
                    {
                        // Console.WriteLine(item);
                        string[] _bookLine = item.Split(new string[] { ":!:!:" }, StringSplitOptions.None);
                        Console.WriteLine(_bookLine[0]);
                        Console.WriteLine(_bookLine[1]);
                        BookList.Add(new Book(_bookLine[0], _bookLine[1]));
                    }
                    BookSelected = new Book();
                }
            }
            catch (Exception e)
            {
               MessageBox.Show(e.ToString());
                MessageBox.Show("Une erreur s'est produite lors du chargement de la liste de livres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region ## Properties ##

        private ObservableCollection<Book> _bookList;

        public ObservableCollection<Book> BookList
        {
            get { return _bookList; }
            set
            {
                if (value.Equals(_bookList) != true)
                {
                    this._bookList = value;
                    this.OnPropertyChanged("BookList");
                }
            }
        }

        private Book _bookSelected;

        public Book BookSelected
        {
            get { return _bookSelected; }
            set
            {
                if (value != null && value.Equals(_bookSelected) != true)
                {
                    this._bookSelected = value;
                    this.OnPropertyChanged("BookSelected");
                }
            }
        }


        private string _bookTitle;

        public string BookTitle
        {
            get { return _bookTitle; }
            set
            {
                if (value.Equals(_bookTitle) != true)
                {
                    this._bookTitle = value;
                    this.OnPropertyChanged("BookTitle");
                }
            }
        }

        private string _authorName;

        public string AuthorName
        {
            get { return _authorName; }
            set
            {
                if (value.Equals(_authorName) != true)
                {
                    this._authorName = value;
                    this.OnPropertyChanged("AuthorName");
                }
            }
        }

        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set
            {
                if (value.Equals(fileName) != true)
                {
                    this.fileName = value;
                    this.OnPropertyChanged("FileName");
                }
            }
        }



        #endregion


        #region ## Commands ##

        RelayCommand _addBookCommand;
        public ICommand AddBookCommand
        {
            get
            {
                if (_addBookCommand == null)
                {
                    _addBookCommand = new RelayCommand(r => this.AddBookCommandExecute(), r => this.CanExecuteAddBookCommand());
                }
                return _addBookCommand;
            }
        }

        private void AddBookCommandExecute()
        {
            try
            {

                if (!String.IsNullOrEmpty(BookSelected.Auteur) && !String.IsNullOrEmpty(BookSelected.Titre))
                {
                    Book NewBook = new Book(BookSelected.Auteur, BookSelected.Titre);
                    if (!BookList.Where(x => x.Auteur == BookSelected.Auteur && x.Titre == BookSelected.Titre).Any())
                    {
                        BookList.Add(NewBook);
                        BookSelected = new Book();
                        File.AppendAllText(FileName, NewBook.Auteur + ":!:!:" + NewBook.Titre + Environment.NewLine);

                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Veuillez vérifier que vous avez entré un titre ainsi qu'un auteur.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private bool CanExecuteAddBookCommand()
        {
            return BookSelected != null;
        }


        RelayCommand _newBookCommand;
        public ICommand NewBookCommand
        {
            get
            {
                if (_newBookCommand == null)
                {
                    _newBookCommand = new RelayCommand(r => this.NewBookCommandExecute(), r => this.CanExecuteNewBookCommand());
                }
                return _newBookCommand;
            }
        }

        private void NewBookCommandExecute()
        {
            BookSelected = new Book(String.Empty, String.Empty);
        }

        private bool CanExecuteNewBookCommand()
        {
            return true;
        }


        RelayCommand _deleteBookCommand;
        public ICommand DeleteBookCommand
        {
            get
            {
                if (_deleteBookCommand == null)
                {
                    _deleteBookCommand = new RelayCommand(r => this.DeleteBookCommandExecute(), r => this.CanExecuteDeleteBookCommand());
                }
                return _deleteBookCommand;
            }
        }

        private void DeleteBookCommandExecute()
        {

            var _oldFile = System.IO.File.ReadAllLines(FileName);
            var _newFile = _oldFile.Where(l => !l.Contains(BookSelected.Auteur + ":!:!:" + BookSelected.Titre));
            System.IO.File.WriteAllLines(FileName, _newFile);
            BookList.Remove(BookSelected);
            BookSelected = new Book();
            LoadList();


        }

        private bool CanExecuteDeleteBookCommand()
        {
            return (BookSelected != null &&
                        (!String.IsNullOrEmpty(BookSelected.Auteur) && !String.IsNullOrEmpty(BookSelected.Titre)));
        }


        RelayCommand _exitCommand;
        public ICommand ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                {
                    _exitCommand = new RelayCommand(r => this.ExitCommandExecute(), r => this.CanExecuteExitCommand());
                }
                return _exitCommand;
            }
        }

        private void ExitCommandExecute()
        {

            Application.Current.Shutdown();


        }

        private bool CanExecuteExitCommand()
        {
            return true;
        }



        #endregion


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

