using ReactiveUI;

namespace Personal_Debts_Book.Models
{
    public class Debt : ReactiveObject
    {
        private string _date = "";
        public string Date {
            get {return _date;}
            set {this.RaiseAndSetIfChanged(ref _date, value);}
        }


        private double _amount = 0;
        public double Amount
        {
            get { return _amount; }
            set { this.RaiseAndSetIfChanged(ref _amount, value); }
        }


        public Debt(string date, double amount)
        {
            Amount = amount;
            Date = date;
        }

    }
}
