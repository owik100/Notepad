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
using Microsoft.Win32;
using System.IO;



namespace Notatnik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SolidColorBrush backColor;
        private SolidColorBrush fontColor;
        private FontFamily fontFamily;
        private double fontSize;
        private FontWeight fontWeight;
        private FontStyle fontStyle;

        private string filePatch { get; set; } = "";
        private bool textChanged { get; set; } = false;

        ApplicationData appData = new ApplicationData();


        string[] wejscie = Environment.GetCommandLineArgs();

        public MainWindow()
        {
            InitializeComponent();
            Deserialize();

            if(wejscie.Length>1)
            {
                editTextBox.Text = File.ReadAllText(wejscie[1], Encoding.Default);
                filePatch = wejscie[1];
                textChanged = false;
                mainWindow.Title = System.IO.Path.GetFileName(wejscie[1]) + " - Notepad";
            }
        }

        #region File buttons

        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (textChanged)
            {
                var result = MessageBox.Show("Save changes in file " + filePatch + "?", "Notepad", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Save_Click(sender, e);
                    editTextBox.Text = "";
                    mainWindow.Title = "Untitled - Notepad";
                    textChanged = false;
                    filePatch = "";
                    return;
                }
                else if (result == MessageBoxResult.No)
                {
                    editTextBox.Text = "";
                    mainWindow.Title = "Untitled - Notepad";
                    textChanged = false;
                    filePatch = "";
                    return;
                }
            }
            else
            {
                mainWindow.Title = "Untitled - Notepad";
                textChanged = false;
                editTextBox.Text = "";
                filePatch = "";
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (textChanged)
            {
                var result = MessageBox.Show("Save changes in file " + filePatch + "?", "Notepad", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Save_Click(sender, e);
                    OpenFile();

                }
                else if (result == MessageBoxResult.No)
                {
                    OpenFile();
                }
            }
            else
            {
                OpenFile();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (filePatch == "")
            {
                Save_as_Click(sender, e);
            }
            else
            {
                File.WriteAllText(filePatch, editTextBox.Text, Encoding.Default);
                textChanged = false;
                mainWindow.Title = System.IO.Path.GetFileName(filePatch) + " - Notepad";
            }
        }

        private void Save_as_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, editTextBox.Text, Encoding.Default);
                filePatch = saveFileDialog.FileName;
                mainWindow.Title = System.IO.Path.GetFileName(saveFileDialog.FileName) + " - Notepad";
                textChanged = false;
            }
        }


        #endregion

        #region Edit buttons

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            if (editTextBox.SelectedText != string.Empty)
            {
                Clipboard.SetText(editTextBox.SelectedText);
                editTextBox.SelectedText = string.Empty;
            }
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            if (editTextBox.SelectedText != string.Empty)
            {
                Clipboard.SetText(editTextBox.SelectedText);
            }
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            int position = editTextBox.SelectionStart;
            editTextBox.Text = editTextBox.Text.Insert(position, Clipboard.GetText());
            editTextBox.SelectionStart = Clipboard.GetText().Length + position;
        }

        #endregion

        #region Foramt buutons

        private void Wrapping_Click(object sender, RoutedEventArgs e)
        {
            if (Wrapping.IsChecked == true)
            {
                Wrapping.IsChecked = true;
                editTextBox.TextWrapping = TextWrapping.Wrap;
            }
            else
            {
                Wrapping.IsChecked = false;
                editTextBox.TextWrapping = TextWrapping.NoWrap;
            }

            Serialize();
        }

        private void FontStyleButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FontDialog fd = new System.Windows.Forms.FontDialog();
            var result = fd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {

                editTextBox.FontFamily = new FontFamily(fd.Font.Name);
                editTextBox.FontSize = fd.Font.Size * 96.0 / 72.0;
                editTextBox.FontWeight = fd.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                editTextBox.FontStyle = fd.Font.Italic ? FontStyles.Italic : FontStyles.Normal;

                TextDecorationCollection tdc = new TextDecorationCollection();
                if (fd.Font.Underline) tdc.Add(TextDecorations.Underline);
                if (fd.Font.Strikeout) tdc.Add(TextDecorations.Strikethrough);
                editTextBox.TextDecorations = tdc;

                fontFamily = editTextBox.FontFamily;
                fontSize = editTextBox.FontSize;
                fontWeight = editTextBox.FontWeight;
                fontStyle = editTextBox.FontStyle;
                Serialize();
            }
        }

        private void FontColor_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.ColorDialog cd = new System.Windows.Forms.ColorDialog();
            var result = cd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                fontColor = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                editTextBox.Foreground = fontColor;
                Serialize();
            }

        }

        #endregion

        #region Extras buttons

        private void BackgroundColor_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog cd = new System.Windows.Forms.ColorDialog();
            var result = cd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                backColor = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                editTextBox.Background = backColor;
                Serialize();
            }
        }

        private void CheckMisspellings_Click(object sender, RoutedEventArgs e)
        {
            if (CheckMisspellings.IsChecked == true)
            {
                CheckMisspellings.IsChecked = true;
                editTextBox.SpellCheck.IsEnabled = true;
            }
            else
            {
                CheckMisspellings.IsChecked = false;
                editTextBox.SpellCheck.IsEnabled = false;
            }
            Serialize();

        }

        #endregion

        #region Help button

        void HelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        #endregion

        #region Closing

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (textChanged)
            {
                var result = MessageBox.Show("Save changes in file " + filePatch + "?", "Notepad", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Save_Click(sender, e);
                    Application.Current.Shutdown();
                    Serialize();

                }
                else if (result == MessageBoxResult.No)
                {
                    textChanged = false;
                    Application.Current.Shutdown();
                    Serialize();
                }
            }
            else
            {
                Application.Current.Shutdown();
                Serialize();
            }
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RoutedEventArgs e2 = null;

            if (textChanged)
            {
                var result = MessageBox.Show("Save changes in file " + filePatch + "?", "Notepad", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Save_Click(sender, e2);
                    Serialize();

                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
            Serialize();
        }

        #endregion

        #region Commands

        void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            New_Click(sender, e);
        }

        private void OpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Open_Click(sender, e);
        }

        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Save_Click(sender, e);
        }

        #endregion

        private void editTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if(!textChanged)
            {
                textChanged = true;
                mainWindow.Title += "*";
            }
        }

        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                editTextBox.Text = File.ReadAllText(openFileDialog.FileName,Encoding.Default);
                filePatch = openFileDialog.FileName;
                mainWindow.Title = System.IO.Path.GetFileName(openFileDialog.FileName) + " - Notepad";
                textChanged = false;
            }
        }

        private void Deserialize()
        {
            appData.Deserialize(appData);

            SetSettingsFromDeserializedFile();
        }

        private void Serialize()
        {
            appData.GetTextWrappingInfo(Wrapping.IsChecked);
            appData.GetCheckMissPellingsInfo(editTextBox.SpellCheck.IsEnabled);
            appData.GetBackgroundColor(backColor.ToString());
            appData.GetFontColor(fontColor.ToString());
            appData.GetFontFamily(fontFamily.ToString());
            appData.GetFontSize(fontSize);
            appData.GetFontWeight(fontWeight.ToString());
            appData.GetFontStyle(fontStyle.ToString());
            appData.GetWindowHeight(this.Height);
            appData.GetWindowWidth(this.Width);
            appData.Serialize();
        }

        private void SetSettingsFromDeserializedFile()
        {

            Wrapping.IsChecked = appData.SetTextWrappingInfo();
            if (Wrapping.IsChecked)
            {
                editTextBox.TextWrapping = TextWrapping.Wrap;
            }
            else
            {
                editTextBox.TextWrapping = TextWrapping.NoWrap;
            }

            CheckMisspellings.IsChecked = appData.SetCheckMissPellingsInfo();
            if (CheckMisspellings.IsChecked)
            {
                editTextBox.SpellCheck.IsEnabled = true;
            }
            else
            {
                editTextBox.SpellCheck.IsEnabled = false;
            }
            editTextBox.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(appData.SetBackgroundColor());
            editTextBox.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(appData.SetFontColor());

            FontFamily mfont = new FontFamily(appData.SetFontFamily());
            editTextBox.FontFamily = mfont;

            editTextBox.FontSize = appData.SetFontSize();


            editTextBox.FontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(appData.SetFontWeight());

            editTextBox.FontStyle = (FontStyle)new FontStyleConverter().ConvertFromString(appData.SetFontStyle());

            this.Height = appData.SetWindowHeight();
            this.Width = appData.SetWindowsWidth();

            backColor = (SolidColorBrush)editTextBox.Background;
            fontColor = (SolidColorBrush)editTextBox.Foreground;
            fontFamily = editTextBox.FontFamily;
            fontSize = editTextBox.FontSize;
            fontWeight = editTextBox.FontWeight;
            fontStyle = editTextBox.FontStyle;
        }

        private void ExportSettings_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Data files (*.dat)|*.dat|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                appData.Serialize(saveFileDialog.FileName);
            }
        }

        private void ImportSettings_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Data files (*.dat)|*.dat|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                appData.Deserialize(appData,openFileDialog.FileName);

                SetSettingsFromDeserializedFile();
            }
        }

        private void DefaultSettings_Click(object sender, RoutedEventArgs e)
        {
            editTextBox.TextWrapping = TextWrapping.Wrap;
            Wrapping.IsChecked=true;

            CheckMisspellings.IsChecked = false;
            editTextBox.SpellCheck.IsEnabled = false;

            editTextBox.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");
            editTextBox.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");

            FontFamily mfont = new FontFamily("Consolas");
            editTextBox.FontFamily = mfont;

            editTextBox.FontSize = 15;

            editTextBox.FontWeight = (FontWeight)new FontWeightConverter().ConvertFromString("Regular");

            editTextBox.FontStyle = (FontStyle)new FontStyleConverter().ConvertFromString("Normal");

            this.Height = 350;
            this.Width = 525;

            backColor = (SolidColorBrush)editTextBox.Background;
            fontColor = (SolidColorBrush)editTextBox.Foreground;
            fontFamily = editTextBox.FontFamily;
            fontSize = editTextBox.FontSize;
            fontWeight = editTextBox.FontWeight;
            fontStyle = editTextBox.FontStyle;

        }

      
    }
}