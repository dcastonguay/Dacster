using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dacster {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window {
        string instance = "";
        string database = "";
        string smartInstance = "";
        string smartDatabase = "";
        List<string> foundInstances = new List<string>();
        List<string> foundDatabases = new List<string>();

        public MainWindow() {
            InitializeComponent();
            foundInstances = SmartList.GetInstances();
            this.smartInstanceBox.ItemsSource = foundInstances;
        }

        private void instanceTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            var instanceText = sender as TextBox;
            instance = instanceText.Text;
        }

        private void databaseTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            var databaseText = sender as TextBox;
            database = databaseText.Text;
        }

        private void getButton_Click(object sender, RoutedEventArgs e) {
            Dacpac CurrentJob = new Dacpac(instance, database);

            SqlTools.RunSqlPackage(instance, database);
        }

        private void smartInstanceBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            string smartInstance = smartInstanceBox.SelectedItem.ToString();
            foundDatabases = SmartList.GetDatabases(smartInstance);
            this.smartDatabaseBox.ItemsSource = foundDatabases;
        }

        private void smartDatabaseBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            
        }
    }
}
