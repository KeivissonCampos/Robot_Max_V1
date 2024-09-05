using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using Microsoft.Speech.Recognition;
using System.Globalization;
using SpeechLib;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;

namespace RobotMax
{
    public partial class voiceRecognition : Form
    {
        UdpClient udpClient = new UdpClient();
        Speak speak;
        Timer recognitionTimer;
        //Variaveis globais
        // variaveis para voz
        static CultureInfo ci = new CultureInfo("pt-BR");// linguagem utilizada
        static SpeechRecognitionEngine reconhecedor; // reconhecedor de voz
        SpeechSynthesizer resposta = new SpeechSynthesizer();// sintetizador de voz

        // Palavras aceitas
        public string[] listaPalavras = { "do escritório","casa", "Max", "Ligar luz", "Desligar luz", "Acender luz", "Apagar luz", "da sala" };

        Conductor conductor;

        bool isRunning = false;
        bool waitForMax = false;
        Random rand = new Random((int)DateTime.Now.Ticks);
        int moveEyesDirection;
        int moveNeckDirection;

        bool comando = false;
        bool time = false;
        bool allowRecognition = true;
        int contador = 0;
        string TextFala = "";
        private Timer noCommandTimer;

        public voiceRecognition(Conductor cond)
        {
            conductor = cond;
            conductor.Set("Eyelids Close", true);
            conductor.Set("Jaw Close", true);
            InitializeComponent();
            Init();
        }

        private void voiceRecognition_FormClosed(object sender, FormClosedEventArgs e)
        {
            udpClient.Close();
            timer1.Stop();
            reconhecedor.RecognizeAsyncCancel();
        }

        public void Gramatica()
        {
            try
            {
                //reconhecedor = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-us"));
                reconhecedor = new SpeechRecognitionEngine(ci);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERRO ao integrar lingua escolhida:" + ex.Message);
            }

            // criacao da gramatica simples que o programa vai entender
            // usando um objeto Choices
            var gramatica = new Choices();
            gramatica.Add(listaPalavras); // inclui a gramatica criada

            // cria o construtor gramatical
            // e passa o objeto criado com as palavras
            var gb = new GrammarBuilder();
            gb.Append(gramatica);

            // cria a instancia e carrega a engine de reconhecimento
            // passando a gramatica construida anteriomente
            try
            {
                var g = new Grammar(gb);

                try
                {
                    // carrega o arquivo de gramatica
                    reconhecedor.RequestRecognizerUpdate();
                    reconhecedor.LoadGrammarAsync(g);

                    // registra a voz como mecanismo de entrada para o evento de reconhecimento
                    reconhecedor.SpeechRecognized += Sre_Reconhecimento;

                    reconhecedor.SetInputToDefaultAudioDevice(); // microfone padrao
                    resposta.SetOutputToDefaultAudioDevice(); // auto falante padrao
                    resposta.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult, 1, new CultureInfo("pt-BR"));
                    reconhecedor.RecognizeAsync(RecognizeMode.Multiple); // multiplo reconhecimento
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERRO ao criar reconhecedor: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERRO ao criar a gramática: " + ex.Message);
            }
        }

        public void Init()
        {
            resposta.Volume = 100; // controla volume de saida
            resposta.Rate = 3; // velocidade de fala

            Gramatica(); // inicialização da gramatica

            noCommandTimer = new Timer();
            noCommandTimer.Interval = 10000; // 10 segundos
            noCommandTimer.Tick += NoCommandTimer_Tick;
        }

        // funcao para reconhecimento de voz
        async void Sre_Reconhecimento(object sender, SpeechRecognizedEventArgs e)
        {
            if (!allowRecognition)
                return;

            allowRecognition = false;

            noCommandTimer.Stop();
            noCommandTimer.Start();

            bool setComando = false;
            string frase = e.Result.Text;
            label1.Text = frase;

            if (comando)
            {
                if (frase.Equals("do escritório"))
                {
                    TextFala = TextFala + frase;
                    setComando = true;
                }
                if (frase.Equals("da sala"))
                {
                    TextFala = TextFala + frase;
                    setComando = true;
                }
                if (setComando)
                {
                    comando = false;
                    setComando = false;
                    speak = new Speak(conductor, TextFala, 1);
                }
            }
            else if (frase.Equals("Max"))
            {
                isRunning = true;
                waitForMax = true;
                time = true;
                //Movimentos();
                conductor.Set("Eyelids Half", true);
                await Task.Delay(1000);
                conductor.Set("Eyelids Open", true);
                conductor.Set("Neck Half Left", true);
                speak = new Speak(conductor, Phrase.Speak_5, 1);
            }
            else if (waitForMax) // Se estiver no estado de espera, reconhece os novos comandos
            {
                if (frase.Equals("Ligar luz") || frase.Equals("Acender luz"))
                {
                    isRunning = false;
                    comando = true;
                    time = false;
                    TextFala = Phrase.Speak_3;
                    //Movimentos();
                }
                if (frase.Equals("Desligar luz") || frase.Equals("Apagar luz"))
                {
                    isRunning = false;
                    comando = true;
                    time = false;
                    TextFala = Phrase.Speak_2;
                    //Movimentos();
                }
            }

            await Task.Delay(1000);
            allowRecognition = true;
        }

        void Movimentos()
        {
            long currentTimeMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            switch (rand.Next(0, 2))
            {
                case 0: conductor.Set("Eyelids Blink", true); ; break;
                case 1: break;
            }

            switch (moveEyesDirection)
            {
                case 0: conductor.Set("Eyes Half Left", true); break;
                case 1: conductor.Set("Eyes Half Right", true); break;
                case 2: conductor.Set("Eyes Half Up", true); break;
                case 3: conductor.Set("Eyes Half Down", true); break;
                case 4: conductor.Set("Eyes Center", true); break;
            }
            moveEyesDirection = (int)rand.Next(0, 5);

            switch (moveNeckDirection)
            {
                case 0: conductor.Set("Neck Half Left", true); break;
                case 1: conductor.Set("Neck Half Right", true); break;
                case 2: conductor.Set("Neck Half Up", true); break;
                case 3: conductor.Set("Neck Half Down", true); break;
                case 4: conductor.Set("Neck Front", true); break;
            }
            moveNeckDirection = (int)rand.Next(0, 5);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            long currentTimeMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            switch (rand.Next(0, 2))
            {
                case 0: conductor.Set("Eyelids Blink", true); ; break;
                case 1: break;
            }
            //blinkEyeTime = currentTimeMillis + rand.Next(100, 10000);

            switch (moveEyesDirection)
            {
                case 0: conductor.Set("Eyes Half Left", true); break;
                case 1: conductor.Set("Eyes Half Right", true); break;
                case 2: conductor.Set("Eyes Half Up", true); break;
                case 3: conductor.Set("Eyes Half Down", true); break;
                case 4: conductor.Set("Eyes Center", true); break;
            }
            ///moveEyesTime = currentTimeMillis + rand.Next(100, 5000);
            moveEyesDirection = (int)rand.Next(0, 5);
        }

        void ComEsp32(string ip, string msg)
        {
            IPAddress ipAddress = IPAddress.Parse("192.168.1.76"); // IP do ESP
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 444); // Porta do ESP

            try
            {
                string message = "2"; // Mensagem a ser enviada
                byte[] bytes = Encoding.ASCII.GetBytes(message);

                udpClient.Send(bytes, bytes.Length, ipEndPoint);

                // Exiba uma mensagem de pop-up para indicar que a mensagem foi enviada com sucesso
                //MessageBox.Show("Mensagem enviada: " + message, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception p)
            {
                // Exiba uma mensagem de pop-up em caso de erro
                MessageBox.Show("Ocorreu um erro: " + p.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NoCommandTimer_Tick(object sender, EventArgs e)
        {
            waitForMax = false;
            noCommandTimer.Stop(); // Para o temporizador
        }
    }
}
