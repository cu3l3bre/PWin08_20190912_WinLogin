using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;


namespace WinLogin
{
    // Übungsvorschlag: Login-Fenster
    //
    //      - Authentifizierung des Benutzer via Datei 'Daten\Login.txt'
    //      - das vom Benutzer eingegebene Passwort darf nicht im "Klartext"
    //        in der 'TextBox' erscheinen (Hinweis 'KeyPress'-Ereignis)
    //      - Log-Datei, welche alle An- und Abmeldungen von Benutzers protokolliert
    //


    public partial class FormLogin : Form
    {
        private Dictionary<string, string> userPwList = new Dictionary<string, string>();
        private static readonly string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..", "Daten", "Login.txt"));

        private string password = String.Empty;


        public FormLogin()
        {
            InitializeComponent();
            readDataFromFile();
        }





        private bool readDataFromFile()
        {
            StreamReader readFromFile = null;

            try
            {

                readFromFile = new StreamReader(path, Encoding.Default);

                while (!readFromFile.EndOfStream)
                {
                    string line = readFromFile.ReadLine();
                    string[] data = line.Split(';');
                    userPwList.Add(data[0], data[1]);

                    Debug.WriteLine(data[0] + " " + data[1]);
                }

                return true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Something went wrong");
                return false;
            }
            finally
            {
                if(readFromFile != null)
                {
                    readFromFile.Close();
                }
            }
        }


        private bool writeDataToFile()
        {
            // TODO
            return true;
        }


        private void ButtonRegistrieren_Click(object sender, EventArgs e)
        {
            bool registerOK = false;
            registerOK = register();

            if(registerOK)
            {
                writeDataToFile();
            }

        }

        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            login();
        }

        private void ButtonLogout_Click(object sender, EventArgs e)
        {
            logout();
        }



        private void login()
        {

            if (!(String.IsNullOrWhiteSpace(textBoxUsername.Text) || String.IsNullOrWhiteSpace(textBoxPasswort.Text)))
            {

                if (userPwList.TryGetValue(textBoxUsername.Text, out string pw))
                {
                    //if (pw.Equals(textBoxPasswort.Text))
                    if (pw.Equals(password))
                    {
                        //Debug.WriteLine("Einloggen erfolgreich");
                        buttonLogin.Enabled = false;
                        buttonRegistrieren.Enabled = false;
                        buttonLogout.Enabled = true;

                        labelStatus.Text = "Angemeldet als " + textBoxUsername.Text;
                        textBoxUsername.Text = String.Empty;
                        textBoxPasswort.Text = String.Empty;

                    }
                    else
                    {
                        // Debug.WriteLine("Passwort falsch existiert nicht");
                        MessageBox.Show("Eingegebene Daten ungueltig");
                    }
                }
                else
                {
                    //Debug.WriteLine("User existiert nicht");
                    MessageBox.Show("Einloggen fehlgeschlagen");
                }
            }
            else
            {
                MessageBox.Show("Fuellen Sie alle Felder aus");
            }
        }



        private void logout()
        {
            buttonLogin.Enabled = true;
            buttonRegistrieren.Enabled = true;
            buttonLogout.Enabled = false;

            labelStatus.Text = "Anmelden oder Registrieren";
            textBoxUsername.Text = String.Empty;
            textBoxPasswort.Text = String.Empty;
        }


        private bool register()
        {
            if(!(String.IsNullOrWhiteSpace(textBoxUsername.Text) || String.IsNullOrWhiteSpace(textBoxPasswort.Text)))
            {
                if (!userPwList.ContainsKey(textBoxUsername.Text))
                {
                    userPwList.Add(textBoxUsername.Text, textBoxPasswort.Text);
                    textBoxUsername.Text = String.Empty;
                    textBoxPasswort.Text = String.Empty;
                    return true;
                }
                else
                {
                    MessageBox.Show("Benutzername bereits vergeben");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Nicht alle Felder ausgefuellt");
                return false;
            }
        }



        private void textBoxPasswort_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("{0}", e.KeyValue);

            if (e.KeyValue == 8 || e.KeyValue == 46 || e.KeyValue == 37 || e.KeyValue == 38 || e.KeyValue == 39 || e.KeyValue == 40)
            {
                textBoxPasswort.Text = String.Empty;
                password = String.Empty;
            }
        }


        private void textBoxPasswort_KeyPress(object sender, KeyPressEventArgs e)
        {
            Debug.WriteLine("{0}", e.KeyChar);

            if ((e.KeyChar >= (char)97) && (e.KeyChar <= (char)122)
                || (e.KeyChar >= (char)65) && (e.KeyChar <= (char)90)
                || (e.KeyChar >= (char)48) && (e.KeyChar <= (char)57)
                )
            {
                password += e.KeyChar;
                e.KeyChar = '*';
                Debug.WriteLine(password);
            }
            else
            {
                e.KeyChar = (char)0;
            }

            e.Handled = false;
        }





    }
}
