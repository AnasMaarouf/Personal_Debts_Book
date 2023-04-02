using Personal_Debts_Book.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.VisualStudio.PlatformUI;
using Personal_Debts_Book.Views;
using System.Collections.Specialized;
using DynamicData.Binding;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using Avalonia.Collections;
using System.Text.RegularExpressions;
using System.Configuration;
using Personal_Debts_Book.Utililty;

namespace Personal_Debts_Book.ViewModels
{
    public class DetailedDebtorWindowViewModel : ViewModelBase
    {

        #region De-/Con-structers
        public DetailedDebtorWindowViewModel()
        {
        }
        public DetailedDebtorWindowViewModel(Debtor debtor)
        {
            SelectedDebtor = debtor;
            FullName = SelectedDebtor.FullName;
            Debts = SelectedDebtor.Debts;
            Emails = SelectedDebtor.Emails;
            PhoneNumbers = SelectedDebtor.PhoneNumbers;

            DateTime tmp = DateTime.Today;
            Date = tmp.ToString();
        }
        #endregion



        #region Properties
        private Debtor SelectedDebtor = new Debtor();

        private ObservableCollection<Debt> _debts = new ObservableCollection<Debt>();
        public ObservableCollection<Debt> Debts
        {
            get { return _debts; }
            set { this.RaiseAndSetIfChanged(ref _debts, value); }
        }

        private Debt? _selectedDebt;
        public Debt? SelectedDebt
        {
            get { return _selectedDebt; }
            set { this.RaiseAndSetIfChanged(ref _selectedDebt, value); }
        }

        private ObservableCollection<string> _emails = new ObservableCollection<string>();
        public ObservableCollection<string> Emails
        {
            get { return _emails; }
            set { this.RaiseAndSetIfChanged(ref _emails, value); }
        }

        private string? _selectedEmail = null;
        public string? SelectedEmail
        {
            get { return _selectedEmail; }
            set { this.RaiseAndSetIfChanged(ref _selectedEmail, value); }
        }

        private string _email = "";
        public string Email
        {
            get { return _email; }
            set { this.RaiseAndSetIfChanged(ref _email, value); }
        }

        private ObservableCollection<string> _phoneNumbers = new ObservableCollection<string>();
        public ObservableCollection<string> PhoneNumbers
        {
            get { return _phoneNumbers; }
            set { this.RaiseAndSetIfChanged(ref _phoneNumbers, value); }
        }

        private string? _selectedPhoneNumber = null;
        public string? SelectedPhoneNumber
        {
            get { return _selectedPhoneNumber; }
            set { this.RaiseAndSetIfChanged(ref _selectedPhoneNumber, value); }
        }

        private string _phoneNumber = "";
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { this.RaiseAndSetIfChanged(ref _phoneNumber, value); }
        }

        private string? _fullName;
        public string? FullName
        {
            get { return _fullName; }
            set { this.RaiseAndSetIfChanged(ref _fullName, value); }
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

        #endregion
        #region PublicMethods

        #endregion
        #endregion


        #region Commands
        void AddNewDebt()
        {
            if (Date == null)   //Error handler: Date is not selected or is null.
            {
                //_logger.LogMessage("ERROR: Date is null!");
                Amount = "ERROR: Date is null!";
                return;
            }

            
            double tmpDouble = 0;
            if(!double.TryParse(Amount, out tmpDouble)) //Error handler: Convert string to double.
            {
                //_logger.LogMessage("ERROR: Cannot convert string to double!");
                Amount = "ERROR: Not a number!";
                return;
            }

            string tmpDate = "";
            for (int i = 0; i < 10; i++)
                tmpDate += _date[i];
            

            Debt tmpDebt = new Debt(tmpDate, tmpDouble);
            Debts.Add(tmpDebt);
            SelectedDebtor.TotalDebt++; //Update total debt of the user.

            Amount = ""; //Making the textbox empty on success, to help the user quickly add another debt.
        }

        void EditSelectedDebt()
        {
            if (SelectedDebt == null)   //Error handler: Selected debt is not selected or is null.
            {
                //_logger.LogMessage("ERROR: Debt is null!");
                Amount = "ERROR: Debt not selected!";
                return;
            }

            if (Date == null)   //Error handler: Date is not selected or is null.
            {
                //_logger.LogMessage("ERROR: Date is null!");
                Amount = "ERROR: Date is null!";
                return;
            }

            double tmpDouble = 0;
            if (!double.TryParse(Amount, out tmpDouble))    //Error handler: Convert string to double.
            {
                //_logger.LogMessage("ERROR: Cannot convert string to double!");
                Amount = "ERROR: Not a number!";
                return;
            }


            string tmpDate = "";
            for (int i = 0; i < 10; i++)
                tmpDate += _date[i];

            SelectedDebt.Amount = tmpDouble;
            SelectedDebt.Date = tmpDate;
            SelectedDebtor.TotalDebt++; //Update total debt of the user.

            Amount = ""; //Making the textbox empty on success, to help the user quickly add another debt with not much work.
        }

        void DeleteDebt()
        {
            if (SelectedDebt == null)   //Error handler: Selected debt is not selected or is null.
            {
               // _logger.LogMessage("ERROR: Debt is null!");
                Amount = "ERROR: Debt not selected!";
                return;
            }

            if (!Debts.Remove(SelectedDebt))    //Error handler: Unsuccessful removal if selected debt.
            {
                //_logger.LogMessage("ERROR: Debt could not be removed! --> " + SelectedDebt);
                Amount = "ERROR: Debt could not be removed!";
                return;
            }

            if (Debts.Count() < 1)  //Automatically select a debt for the user on successfull deletion of debt.
                SelectedDebt = null;
            else
                SelectedDebt = Debts[0];

            SelectedDebtor.TotalDebt++; //Update total debt of the user.
        }



        void AddEmail()
        {
            //Check wheather input is an email.
            if (Regex.IsMatch(Email, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")) {
                Emails.Add(Email);
            } else {   //Error hander: If not an email.
                //_logger.LogMessage("ERROR: Entered string is not a valid email! --> " + Email);
                Email = "ERROR: " + Email + " is not an email!";
                return;
            }
        }
        void DeleteEmail()
        {
            if (SelectedEmail == null)
            {
                //_logger.LogMessage("Error: Selected email is null!");
                Email = "Error: Email not selected!";
                return;
            }

            if (!Emails.Remove(SelectedEmail))
            {
                //_logger.LogMessage("Error: Email could not be removed! --> " + SelectedEmail);
                Email = "Error: Email could not be removed!";
                return;
            }
        }

        void AddPhoneNumber()
        {
            PhoneNumbers.Add(PhoneNumber);
            PhoneNumber = "";
        }
        void DeletePhoneNumber()
        {
            if (SelectedPhoneNumber == null)
            {
                //_logger.LogMessage("Error: PhoneNumber is null!");
                Email = "Error: PhoneNumber not selected!";
                return;
            }
            if (!PhoneNumbers.Remove(SelectedPhoneNumber))
            {
                //_logger.LogMessage("Error: PhoneNumber could not be removed! --> " + SelectedPhoneNumber);
                PhoneNumber = "Error: PhoneNumber could not be removed!";
                return;
            }
        }

        #endregion


    }
}
