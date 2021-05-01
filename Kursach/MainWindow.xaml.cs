using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Kursach
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private const string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        private string orginalText = String.Empty;

        private void OnSaveDecryptedTextButtonClicked(object sender, RoutedEventArgs e)
        {
            if (orginalText == String.Empty)
            {
                MessageBox.Show(this, "Нет исходного текста!!!");
                return;
            }

            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.FileName = "Decrypted";
            fileDialog.DefaultExt = ".txt";
            fileDialog.Filter = "Text documents (.txt)|*.txt";
            Nullable<bool> result = fileDialog.ShowDialog();

            if (result == false)
            {
                MessageBox.Show(this, "Путь не выбран!!");
                return;
            }

            File.WriteAllText(fileDialog.FileName, TextPanel.Text, Encoding.Default);
            MessageBox.Show(this, "Файл сохранён!");
        }

        private void OnSelectNewFileButtonClicked(object sender, RoutedEventArgs e)
        {
            string key = GetKeyFromUser();
            if (String.IsNullOrWhiteSpace(key))
            {
                MessageBox.Show(this, "Ключ не был введён!!!");
                return;
            }
            key = key.Trim();

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.CheckFileExists = true;
            fileDialog.Title = "Выберите файл";
            fileDialog.DefaultExt = ".txt";
            fileDialog.Filter = "Text documents (.txt)|*.txt";
            Nullable<bool> result = fileDialog.ShowDialog();

            if (result == false)
            {
                MessageBox.Show(this, "Файл не выбран!!");
                return;
            }

            orginalText = File.ReadAllText(fileDialog.FileName, Encoding.Default);

            VigenereCipher cipher = new VigenereCipher(alphabet);
            TextPanel.Text = cipher.Decrypt(orginalText, key);
        }

        private string GetKeyFromUser()
        {
            KeyPopup keyWindow = new KeyPopup();
            keyWindow.Owner = this;
            keyWindow.ShowDialog();
            return keyWindow.GetKey();
        }

        private void OnChangeKeyButtonClicked(object sender, RoutedEventArgs e)
        {
            if (orginalText == String.Empty)
            {
                MessageBox.Show(this, "Нет исходного текста!!!");
                return;
            }

            string key = GetKeyFromUser();
            if (String.IsNullOrWhiteSpace(key))
            {
                MessageBox.Show(this, "Ключ не был введён!!!");
                return;
            }
            key = key.Trim();

            VigenereCipher cipher = new VigenereCipher(alphabet);
            TextPanel.Text = cipher.Decrypt(orginalText, key);
        }

        private void OnEncryptButtomClicked(object sender, RoutedEventArgs e)
        {
            string text = TextForEncryption.Text.Trim();

            if (String.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show(this, "Нет исходного текста!!!");
                return;
            }

            string key = GetKeyFromUser();
            if (String.IsNullOrWhiteSpace(key))
            {
                MessageBox.Show(this, "Ключ не был введён!!!");
                return;
            }
            key = key.Trim();

            VigenereCipher cipher = new VigenereCipher(alphabet);
            string encryptedText = cipher.Encrypt(text, key);

            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.FileName = "Encypted";
            fileDialog.DefaultExt = ".txt";
            fileDialog.Filter = "Text documents (.txt)|*.txt";
            Nullable<bool> result = fileDialog.ShowDialog();

            if (result == true)
            {
                File.WriteAllText(fileDialog.FileName, encryptedText, Encoding.Default);
            }
        }

        private void OnLoadDocumentButtonClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.CheckFileExists = true;
            fileDialog.Title = "Выберите документ";
            fileDialog.DefaultExt = ".docx";
            fileDialog.Filter = "Word documents (.docx)|*.docx";
            Nullable<bool> result = fileDialog.ShowDialog();

            if (result == false)
            {
                MessageBox.Show(this, "Файл не выбран!!");
                return;
            }
            DocumentText.Text = String.Empty;
            using (var document = DocX.Load(fileDialog.FileName))
            {
                document.GetSections().ToList().ForEach(x => x.SectionParagraphs.ForEach(y => DocumentText.Text += y.Text + '\n'));
            }
        }

        private void OnEncryptDocumentButtonClicked(object sender, RoutedEventArgs e)
        {
            string text = DocumentText.Text.Trim();

            if (String.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show(this, "Нет исходного текста!!!");
                return;
            }

            string key = GetKeyFromUser();
            if (String.IsNullOrWhiteSpace(key))
            {
                MessageBox.Show(this, "Ключ не был введён!!!");
                return;
            }
            key = key.Trim();

            VigenereCipher cipher = new VigenereCipher(alphabet);
            DocumentText.Text = cipher.Encrypt(text, key);
        }

        private void OnSaveDocumentButtonClicked(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.FileName = "Encypted";
            fileDialog.DefaultExt = ".docx";
            fileDialog.Filter = "Word documents (.docx)|*.docx";
            Nullable<bool> result = fileDialog.ShowDialog();

            if (result == false)
            {
                MessageBox.Show(this, "Путь не выбран!!");
                return;
            }

            using (var document = DocX.Create(fileDialog.FileName))
            {
                string[] Paragraphs = DocumentText.Text.Split('\n');
                foreach (var str in Paragraphs)
                {
                    document.InsertParagraph(str).FontSize(14d).SpacingAfter(50d).Alignment = Alignment.left;
                }
                
                document.Save();
                MessageBox.Show(this, "Файл сохранён!");
            }
        }
    }
}
