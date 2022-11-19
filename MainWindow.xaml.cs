using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace amoghi_notes
{
    public partial class MainWindow : Window
    {
        private string prev = "";
        private string SelectedItem;
        private bool _IsItemSelected; // backing field for IsItemSelected
        private bool IsItemSelected { get { return _IsItemSelected; } set {
                _IsItemSelected = value;
                BtnAdd.Content = value ? "Exit Editing" : "Add Note";
            } }

        private readonly Func<string, string> Format = (string i) => i.Replace("<br>", "\n").Trim(); // formatting a string
        private void ClearClick(object sender, RoutedEventArgs e)
        {
            noteText.Text = ""; // clears the box
        }
        private void Refr(object sender, RoutedEventArgs e)
        {
            /* 
             * refreshes for changes from the raw persistance file
             * and researches the query for updates
             */
            notes.Items.Clear();

            Load();
            SearchFunctionality();
        }
        private void Load()
        {
            foreach (string i in Persistance.GetAll()) if (Format(i) != "") { notes.Items.Add(Format(i)); } // loading
        }

        public MainWindow()
        {
            InitializeComponent();
            Load();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string was = Alert.Text; // saving the value of Alert.Text
            Alert.Text = ""; // clearing textbox

            Persistance.Save(notes.Items); // saving to persistance

            IsItemSelected = false;
            
            if (noteText.Text == "" || was == "[EDIT]") { noteText.Text = ""; return; }


            if (notes.Items.Contains(noteText.Text))
            {
                MessageBox.Show("Duplicate not allowed.");
                return;
            }

            TextBox x = noteText;
            notes.Items.Add(Format(x.Text));
            x.Text = "";

            Persistance.Save(notes.Items);
        }

        private void Notes_DoubleClick(object sender, EventArgs e)
        {
            /*
             * editing
             */
            try
            {
                ItemCollection o = notes.Items;

                SelectedItem = notes.Items[notes.SelectedIndex].ToString();
                noteText.Text = SelectedItem;

                IsItemSelected = true;

                prev = SelectedItem;

                Alert.Text = "[EDIT]";
                Persistance.Save(notes.Items);

            } catch
            {
                Console.WriteLine($"The index is: {notes.SelectedIndex}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Persistance.Write(Persistance.GetAll(), "Recover.txt");

            Persistance.Write(Array.Empty<string>(), "Persistor.txt");
            MessageBox.Show("Deleted all notes [to recover them, go to C:\\users\\<your.name>\\Recover.txt, copy paste the contents into C:\\users\\<your.name>\\Persistor.txt, then click the refresh button].");
            notes.Items.Clear();
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            SearchFunctionality();
        }
        private void SearchFunctionality()
        {
            IEnumerable<string> x = from y in Persistance.GetAll()
                                    where (y.Contains(query.Text.ToUpper()) || y.Contains(query.Text.ToLower())) && Format(y) != ""
                                    select y;
            results.Items.Clear();
            foreach (string i in x) results.Items.Add(Format(i));
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (IsItemSelected)
            {
                notes.Items.Remove(SelectedItem);
                noteText.Text = "";
                Alert.Text = "";

                MessageBox.Show($"Deleted \"{SelectedItem}\"");

                IsItemSelected = false;
            }
        }
        private void Notes_DoubleClick2(object sender, RoutedEventArgs e) // basically notes_doubleclick but for the search results
        {
            try
            {
                ItemCollection o = notes.Items;

                SelectedItem = results.Items[results.SelectedIndex].ToString();
                noteText.Text = SelectedItem;

                IsItemSelected = true;

                prev = SelectedItem; // setting prev

                Alert.Text = "[EDIT]";
                Persistance.Save(notes.Items);

            }
            catch
            {
                Console.WriteLine($"The index is: {notes.SelectedIndex}");
            }
        }

        private void NoteText_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Alert.Text != "[EDIT]") return;

            notes.Items.Remove(prev);
            notes.Items.Add(noteText.Text);
            prev = noteText.Text;
        }
    }

}
