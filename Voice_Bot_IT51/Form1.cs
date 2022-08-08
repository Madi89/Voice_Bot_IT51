using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;
using System.Threading;

namespace Voice_Bot_IT51
{
    public partial class IT51 : Form
    {
        SpeechRecognitionEngine h = new SpeechRecognitionEngine();
        SpeechSynthesizer s = new SpeechSynthesizer();

        Boolean hören = true;

        public Boolean suche = false;
        public IT51()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void IT51_Load(object sender, EventArgs e)
        {
            Choices commands = new Choices();
            commands.Add(File.ReadAllLines(@"C:\Voice_Bot\commands.txt")); //@"C:\Voice_Bot\search.txt"
            commands.Add(File.ReadAllLines(@"C:\Voice_Bot\search.txt"));
            GrammarBuilder grbuilder = new GrammarBuilder();
            grbuilder.Append(commands);
      
            Grammar grammar = new Grammar(grbuilder);

            h.LoadGrammar(grammar);
            h.SetInputToDefaultAudioDevice();
            h.SpeechRecognized += recEngine_SpeechRecognized;

            h.RecognizeAsync(RecognizeMode.Multiple);
            s.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
            s.SpeakAsync("wie kann ich dir helfen?");
        }

        void recEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            Random r = new Random();
            String[] hallo = new string[5] { "na du", "moin moin", "servus", "guten tag", "hi"};
            
            string target = "http://www.google.de"; //liste für webseiten erstellen
            string target1 = "http://www.wetter.com";
            string target3 = "steam://rungameid/292120"; //Steam Spiele URL mit der game ID
            string modus = e.Result.Text;

            if (modus == "sprich")
            {
                hören = true;
            }
            if (modus == "ruhe")
            {
                hören = false;
            }

            if (suche)
            {
                Process.Start("https://www.google.de/search?q=" + modus);
                suche = false;
            }

            if (hören == true && suche == false)
            {
                switch (e.Result.Text)
                {
                    case "hallo":
                        say(hallo[r.Next(5)]);
                        break;
                    case "guten morgen":
                        say("tüdelüüüü");
                        break;
                    case "wie geht es dir":
                        say("Mir geht es gut");
                        break;
                    case "wie spät ist es":
                        say(DateTime.Now.ToString("HH:mm:ss tt"));
                        break;
                    case "welcher tag ist heute":
                        say(DateTime.Now.ToString("dddd"));
                        break;
                    case "welches datum haben wir heute":
                        say(DateTime.Now.ToString("d/MMMM/yyyy"));
                        break;
                    case "öffne wordpad":
                        say("wird gemacht meister");
                        Process.Start("wordpad");
                        break;
                    case "öffne google":
                        Process.Start(target);
                        break;
                    case "wetter":
                        Process.Start(target1);
                        break;
                    case "suche nach":
                        suche = true;
                        break;
                 /* case "öffne spotify":
                        Process.Start("");
                        break;*/
                    case "öffne steam":
                        Process.Start("C:\\Program Files (x86)\\Steam\\Steam.exe");
                        break;
                    case "öffne ff":
                        Process.Start(target3);
                        break;
                    case "exit":
                        Application.Exit();
                        break;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_time.Text = DateTime.Now.ToLongTimeString();
            lbl_date.Text = DateTime.Now.ToLongDateString();
        }

        public void say(string h)
        {
            s.SpeakAsync(h);
        }
        
    }
}
