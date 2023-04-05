# Personal_Debts_Book
"Personal_Debts_Book" is an application where you can register people who owes you or who you owe money to by adding the date and amount. The application allows you to store the debtors contact information (currently only email and phone numbers), and save the information of the of each debtor in their seperate files.

When storing the information of the different debtors, it is highly reccomended to save the information in their own seperate folder, by creating and naming the folder yourself and specifying it in the relative textbox (if the folder does not exist the application will simply attempt to create this folder and save the information).

### Currently supported operating systems:
The application is build on "Avalonia UI" which is crossplatform supporting Linux, Windows and Mac-OS. Please visit their website for more information.

### Application Information:
This project is a XAML WPF application implemented using C# .NET with the "Avalonia UI" for crossplatform compatibility. The project implements the MVVM-pattern (Model-View-ViewModel Pattern) where "Views" uses XAML-markup language for the UI and binding to data in the "ViewModel" that implements the behaviour of the UI and "View". The "ViewModel" uses the Models "Debtor" and "Debts" which only contains data and possibly some data validations.

