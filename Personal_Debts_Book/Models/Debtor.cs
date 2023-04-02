using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Collections;
using ReactiveUI;

namespace Personal_Debts_Book.Models
{
    public class Debtor : ReactiveObject
    {
        #region Variables
        private ObservableCollection<string> _phoneNumbers = new ObservableCollection<string>();
        public ObservableCollection<string> PhoneNumbers
        {
            get { return _phoneNumbers; }
            set { this.RaiseAndSetIfChanged(ref _phoneNumbers, value); }
        }

        private ObservableCollection<string> _emails = new ObservableCollection<string>();
        public ObservableCollection<string> Emails
        {
            get { return _emails; }
            set { this.RaiseAndSetIfChanged(ref _emails, value); }
        }


        private ObservableCollection<Debt> _debts = new ObservableCollection<Debt>();
        public ObservableCollection<Debt> Debts
        {
            get {return _debts;}
            set {this.RaiseAndSetIfChanged(ref _debts, value);}
        }

        private string _firstName = "";
        public string FirstName
        {
            get => _firstName;
            set => this.RaiseAndSetIfChanged(ref _firstName, value);
        }

        private string _lastName = "";
        public string LastName
        {
            get => _lastName;
            set => this.RaiseAndSetIfChanged(ref _lastName, value);
        }

        private string _fullName = "";
        public string FullName
        {
            get {
                _fullName = FirstName + " " + LastName;
                return _fullName;
            }
            set {
                _fullName = FirstName + " " + LastName;
                string thrsh = "";
                this.RaiseAndSetIfChanged(ref thrsh, value);
            }
        }

        private double _totalDebt;
        public double TotalDebt
        {
            get
            {
                _totalDebt = 0;
                if (Debts != null)
                {
                    for (int i = 0; i < Debts.Count; i++)
                    {
                        _totalDebt += Debts[i].Amount;
                    }
                }
                else
                {
                    _totalDebt = 0;
                }
                return _totalDebt;
            }
            set {
                _totalDebt = 0;
                if (Debts != null)
                {
                    for (int i = 0; i < Debts.Count; i++)
                    {
                        _totalDebt += Debts[i].Amount;
                    }
                } else
                {
                    _totalDebt = 0;
                }
                double thrsh = 0;
                this.RaiseAndSetIfChanged(ref thrsh, value);
            }
        }
        #endregion


        #region Contructers

        public Debtor() {
            _debts.CollectionChanged += new NotifyCollectionChangedEventHandler(DebtsChanged);
            TotalDebt++; }

        public Debtor(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            _debts.CollectionChanged += new NotifyCollectionChangedEventHandler(DebtsChanged);
            TotalDebt++;
        }

        public Debtor(string firstName, string lastName, string date, double amount)
        {
            FirstName = firstName;
            LastName = lastName;
            _debts.CollectionChanged += new NotifyCollectionChangedEventHandler(DebtsChanged);
            Debts.Add(new Debt(date, amount));
            TotalDebt ++;
        }

        #endregion




        #region Methods
        #region PrivateMethods
        private void DebtsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            double tmp = 0;
            if (Debts == null || Debts.Count <= 0)
            {
                TotalDebt = 0;
            }
            else {
                for (int i = 0; i < Debts.Count; i++)
                {
                    tmp += Debts[i].Amount;
                }
                TotalDebt = tmp;
            }
        }

        #endregion
        #region PublicMethods
        
        public void attachDebtsListEvent()
        {
            /*
                This function should only be used,
                if the total amount of debt is not
                updated as supposed to, because of
                the event not being attached.
            */
            _debts.CollectionChanged += new NotifyCollectionChangedEventHandler(DebtsChanged);
        }

        #endregion
        #endregion

    }
}
