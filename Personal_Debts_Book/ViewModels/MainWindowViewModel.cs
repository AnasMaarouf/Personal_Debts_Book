using ReactiveUI;
using System.Collections.ObjectModel;
using System.Text.Json;
using Personal_Debts_Book.Models;
using Personal_Debts_Book.Views;
using System.IO;
using System.Collections.Generic;
using Avalonia;

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


        //private string _debtorsListFolderLocation = ""; //Use if folder contains other information such as pictures, configuration files and etc.
        //public string DebtorsListFolderLocation     //Use if folder contains other information such as pictures, configuration files and etc.
        //{
        //    get { return _debtorsListFolderLocation; }
        //    set { this.RaiseAndSetIfChanged(ref _debtorsListFolderLocation, value); }
        //}

        private string _folderLocation = "Folder Location";
        public string FolderLocation
        {
            get { return _folderLocation; }
            set {this.RaiseAndSetIfChanged(ref _folderLocation, value);}
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


        private bool _saveLoadSingleFile_RadioButton = true;
        public bool SaveLoadSingleFile_RadioButton
        {
            get { return _saveLoadSingleFile_RadioButton; }
            set { this.RaiseAndSetIfChanged(ref _saveLoadSingleFile_RadioButton, value); }
        }

        private bool _saveLoadMultipleFiles_RadioButton = false;
        public bool SaveLoadMultipleFiles_RadioButton
        {
            get { return _saveLoadMultipleFiles_RadioButton; }
            set { this.RaiseAndSetIfChanged(ref _saveLoadMultipleFiles_RadioButton, value); }
        }

        #endregion


        #region Methods
        #region PrivateMethods
        private Debtor? ConvertFrom_JSONToCSharp_Multifile(string FileName)
        {
            if (System.IO.File.Exists(FileName))
            {
                Debtor? debtor;
                string File = System.IO.File.ReadAllText(FileName);
                try
                {
                    debtor = JsonSerializer.Deserialize<Debtor>(File);
                }
                catch
                {
                    return null;
                }
                return debtor;
            }
            return null;
        }

        private bool ConvertFrom_CSharpToJSON_Multifile(string FileName, Debtor debtors)
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
                try
                {
                    System.IO.File.WriteAllText(FileName, JSON_formatted_text);
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    System.IO.File.Delete(FileName);
                }
                catch
                {
                    return false;
                }

                try
                {
                    System.IO.File.WriteAllText(FileName, JSON_formatted_text);
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }


        private ObservableCollection<Debtor>? ConvertFrom_JSONToCSharp_Singlefile(string FileName)
        {
            if (System.IO.File.Exists(FileName))
            {
                string File = System.IO.File.ReadAllText(FileName);
                ObservableCollection<Debtor>? debtor;
                try
                {
                    debtor = JsonSerializer.Deserialize<ObservableCollection<Debtor>>(File);
                } catch { return null; }
                return debtor;
            }
            return null;
        }

        private bool ConvertFrom_CSharpToJSON_Singlefile(string FileName, ObservableCollection<Debtor> debtors)
        {
            ObservableCollection<Debtor> tmpObservableCollection = debtors;
            string JSON_formatted_text = "";

            try
            {
                JSON_formatted_text = JsonSerializer.Serialize(tmpObservableCollection);
            }
            catch
            {
                return false;
            }

            
            if (!System.IO.File.Exists(FileName))
            {
                try
                {
                    System.IO.File.WriteAllText(FileName, JSON_formatted_text);
                } catch
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    System.IO.File.Delete(FileName);
                } catch {
                    return false;
                }

                try
                {
                    System.IO.File.WriteAllText(FileName, JSON_formatted_text);
                }
                catch
                {
                    return false;
                }
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


        private void SaveOrUpdateDataCommand()
        {
            
            if (Debtors != null) {
                if (SaveLoadMultipleFiles_RadioButton) {
                    if (!Directory.Exists(FolderLocation))
                    {
                        try
                        {
                            Directory.CreateDirectory(FolderLocation);
                        }
                        catch
                        {
                            FolderLocation = "ERROR: Folder could not be created";
                            return;
                        }
                    }
                    foreach (Debtor _debtor in Debtors)
                    {
                        if (!ConvertFrom_CSharpToJSON_Multifile(FolderLocation + "/" + "Debtor - " + _debtor.FullName, _debtor))
                        {
                            FolderLocation = "ERROR: Could not save " + _debtor.FullName + "to a file!";
                            return;
                        }
                    }
                } else if (SaveLoadSingleFile_RadioButton)
                {
                    if (FolderLocation.Contains("/"))
                    {
                        int index = FolderLocation.LastIndexOf('/');
                        string dir = FolderLocation.Substring(0, index + 1);
                        if (!Directory.Exists(dir))
                        {
                            try
                            {
                                Directory.CreateDirectory(dir);
                            } catch
                            {
                                FolderLocation = "ERROR: Could not create directory \"" + dir + "\"!";
                            }
                        }
                    }

                    if (!ConvertFrom_CSharpToJSON_Singlefile(FolderLocation, Debtors))
                    {
                        FolderLocation = "ERROR: Could not save list to a file!";
                        return;
                    }
                }
            } else
            {
                FolderLocation = "ERROR: No debtors!";
            }

            //DebtorsListFolderLocation = FolderLocation + "/";     //Use if folder contains other information such as pictures, configuration files and etc.
        }

        private void LoadDataCommand()
        {
            if (Directory.Exists(FolderLocation))
            {
                ObservableCollection<Debtor>? tmpDebtorList = new ObservableCollection<Debtor>();
                IEnumerable<string> Files = Directory.EnumerateFiles(FolderLocation);
                string refString = "Debtor - ";

                if (SaveLoadMultipleFiles_RadioButton)
                {
                    foreach (string file in Files)
                    {
                        Debtor? tmpDebtor = null;
                        if (file.Contains(refString))
                        {
                            tmpDebtor = ConvertFrom_JSONToCSharp_Multifile(file);
                            if (tmpDebtor == null)
                            {
                                FolderLocation = FolderLocation + ", \"" + file + "\" is Invalid Invalid input!";
                                return;
                            }
                            tmpDebtorList.Add(tmpDebtor);

                        }
                    }
                }
                else if (SaveLoadSingleFile_RadioButton)
                {
                    tmpDebtorList = ConvertFrom_JSONToCSharp_Singlefile(FolderLocation);
                    if (tmpDebtorList == null)
                    {
                        FolderLocation = "ERROR: Invalid input!";
                        return;
                    }
                }

                 Debtors.Clear();
                //DebtorsListFolderLocation = FolderLocation + "/";     //Use if folder contains other information such as pictures, configuration files and etc.
                if (tmpDebtorList != null) {
                    for (int i = 0; i < tmpDebtorList.Count; i++)
                    {
                        Debtors.Add(tmpDebtorList[i]);
                        Debtors[i].attachDebtsListEvent();
                    }
                } else
                {
                    FolderLocation = "ERROR: Folder contains no debtors!";
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