using System.Reactive.Linq;
using Personal_Debts_Book;
using ReactiveUI;
using System.Collections.ObjectModel;
using Avalonia.ReactiveUI;
using System.Windows.Input;
using System.Text.Json;
using System.Collections.Specialized;
using Personal_Debts_Book.Models;
using Personal_Debts_Book.Views;
using Personal_Debts_Book.Utililty;
using System.IO;
using System.Collections.Generic;
using Avalonia.Media;
using System.Text.RegularExpressions;
using System.Linq;

namespace Personal_Debts_Book.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel()
        {
            Debtor tmp1 = new Debtor("Jack", "ripper", "12/05/2001", 500);
            Debtor tmp2 = new Debtor("Jennie", "tanner", "12/05/2001", 750);
            Debtors.Add(tmp1);
            Debtors.Add(tmp2);
        }

        
        #region Variables
        private ObservableCollection<Debtor> _debtors = new ObservableCollection<Debtor>();
        public ObservableCollection<Debtor> Debtors {
            get { return _debtors; }
            set { this.RaiseAndSetIfChanged(ref _debtors, value); }
        }

        private Debtor? _selectedDebtor = null;
        public Debtor? SelectedDebtor
        {
            get { return _selectedDebtor; }
            set { this.RaiseAndSetIfChanged(ref _selectedDebtor, value); }
            
        }

        private int _selectedDebtorIndex;
        public int SelectedDebtorIndex
        {
            get { return _selectedDebtorIndex; }
            set { this.RaiseAndSetIfChanged(ref _selectedDebtorIndex, value); }
        }


        private string _debtorsListFolderLocation = "";
        public string DebtorsListFolderLocation
        {
            get { return _debtorsListFolderLocation; }
            set { this.RaiseAndSetIfChanged(ref _debtorsListFolderLocation, value); }
        }

        private string _folderLocation = "Folder Location";
        public string FolderLocation
        {
            get { return _folderLocation; }
            set { this.RaiseAndSetIfChanged(ref _folderLocation, value); }
        }

        private string _firstName = "First Name";
        public string FirstName
        {
            get { return _firstName; }
            set { this.RaiseAndSetIfChanged(ref _firstName, value); }
        }

        private string _lastName = "Last Name";
        public string LastName
        {
            get { return _lastName; }
            set { this.RaiseAndSetIfChanged(ref _lastName, value); }
        }

        private string _date = "";
        public string Date
        {
            get { return _date; }
            set { this.RaiseAndSetIfChanged(ref _date, value); }
        }

        private string _amount = "1,00";
        public string Amount
        {
            get { return _amount; }
            set { this.RaiseAndSetIfChanged(ref _amount, value); }
        }


        #endregion


        #region Methods
        #region PrivateMethods
        private Debtor? ConvertFrom_JSON_To_CSharp(string FileName)
        {
            if (System.IO.File.Exists(FileName))
            {
                string File = System.IO.File.ReadAllText(FileName);
                Debtor? debtor = JsonSerializer.Deserialize<Debtor>(File);
                return debtor;
            }
            return null;
        }

        private bool ConvertFrom_CSharp_To_JSON(string FileName, Debtor debtors)
        {
            Debtor tmpObservableCollection = debtors;
            string JSON_formatted_text = "";

            try
            {
                JSON_formatted_text = JsonSerializer.Serialize(tmpObservableCollection);
            } catch {
                return false;
            }


            if (!System.IO.File.Exists(FileName))
            {
                System.IO.File.WriteAllText(FileName, JSON_formatted_text);
            }
            else
            {
                System.IO.File.Delete(FileName);
                System.IO.File.WriteAllText(FileName, JSON_formatted_text);
            }
            return true;
        }

        #endregion
        #region PublicMethods

        #endregion
        #endregion


        #region Commads
        private void OpenWindow_DetailedDescriptionOfDebtorCommand()
        {
            if (SelectedDebtor != null)
            {
                var registerDebtsWindowViewModel = new DetailedDebtorWindowViewModel(SelectedDebtor);
                var registerDebtsWindow = new DetailedDebtorWindow()
                {
                    DataContext = registerDebtsWindowViewModel
                };
                registerDebtsWindow.Show();
            }
        }

        private void AddNewDebtorCommand()
        {
            Debtor tmpDebtor = new Debtor(FirstName, LastName);
            Debtors.Add(tmpDebtor);

            FirstName = "";
            LastName = "";
        }

        private void DeleteDebtorCommand()
        {
            if (SelectedDebtor != null) {
                Debtors.Remove(SelectedDebtor);

                if (SelectedDebtor != null)
                    SelectedDebtor = Debtors[0];
                else
                    SelectedDebtor = null;
            }
        }

        private void UpdateDebtorCommand()
        {
            if (SelectedDebtor != null)
            {
                SelectedDebtor.FirstName = FirstName;
                SelectedDebtor.LastName = LastName;
                
                if (SelectedDebtor != null)
                    SelectedDebtor = Debtors[0];
                else
                    SelectedDebtor = null;
            } else
            {
                FirstName = "ERROR: Debtor not selected!";
                LastName = "ERROR: Debtor not selected!";
            }

        }


        private void SaveOrUpdateDataCommand()
        {
            if (!Directory.Exists(FolderLocation))
            {
                try
                {
                    Directory.CreateDirectory(FolderLocation);
                }
                catch {
                    FolderLocation = "ERROR: Folder could not be created";
                    return;
                }
            }
            if (Debtors != null) {
                foreach (Debtor _debtor in Debtors)
                {
                    if (!ConvertFrom_CSharp_To_JSON(FolderLocation + "/" + "Debtor - " + _debtor.FullName, _debtor))
                    {
                        FolderLocation = "ERROR: Could not save " + _debtor.FullName + "to a file!";
                        return;
                    }
                }
            }

            DebtorsListFolderLocation = FolderLocation + "/";
        }

        private void LoadDataCommand()
        {
            if (Directory.Exists(FolderLocation))
            {
                ObservableCollection<Debtor>? tmpDebtorList = new ObservableCollection<Debtor>();
                IEnumerable<string> Files = Directory.EnumerateFiles(FolderLocation);
                string refString = "Debtor - ";

                foreach (string file in Files)
                {
                    Debtor? tmpDebtor = null;
                    if(file.Contains(refString))
                    {
                        tmpDebtor = ConvertFrom_JSON_To_CSharp(file);
                        if (tmpDebtor == null)
                        {
                            FolderLocation = "ERROR: " + file + " is not valid!";
                            return;
                        }
                        tmpDebtorList.Add(tmpDebtor);

                    }
                }

                Debtors.Clear();
                DebtorsListFolderLocation = FolderLocation + "/";

                for (int i = 0; i < tmpDebtorList.Count; i++)
                {
                    Debtors.Add(tmpDebtorList[i]);
                    Debtors[i].attachDebtsListEvent();
                }
            }
            else
            {
                FolderLocation = "ERROR: Directory does not exist!";
            }
        }
        #endregion

    }
}