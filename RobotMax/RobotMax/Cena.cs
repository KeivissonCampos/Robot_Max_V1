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
using System.Xml.Linq;

namespace RobotMax
{
    public partial class Cena : Form
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
            public string[] listaPalavras = { "Acorda Max", "Deixa de preguiça Max", "Preparar cenário para gravação", "Feche a cortina" };

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

            public Cena(Conductor cond)
            {
                conductor = cond;
                InitializeComponent();
                Init();
                conductor.Set("Eyelids Close", true); ;
                conductor.Set("Neck Front", true);
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
            }

            // funcao para reconhecimento de voz
            async void Sre_Reconhecimento(object sender, SpeechRecognizedEventArgs e)
            {
                if (!allowRecognition)
                    return;
                allowRecognition = false;
                
                string frase = e.Result.Text;
                label1.Text = frase;

                if (frase.Equals("Acorda Max") && contador == 0)
                {
                
                    conductor.Set("Eyelids Half", true);
                    await Task.Delay(1000);
                    conductor.Set("Eyelids Blink", true);
                    conductor.Set("Eyes Half Up", true);
                    timer1.Start();
                    speak = new Speak(conductor, "Olá chefe. O que temos para hoje?", 1);
                    contador = 1;
            }
                else if (frase.Equals("Preparar cenário para gravação") && contador == 1)
                {
                    conductor.Set("Neck Half Right", true);
                    
                    speak = new Speak(conductor, "Vou preparar o cenário para o pôdkesti, mais alguma coisa?", 1);
                    await Task.Delay(3000);
                    EnviaComando("5");
                    contador = 2;
            }
                else if (frase.Equals("Feche a cortina") && contador == 2)
                {
                    speak = new Speak(conductor, "ok, fechar a cortina.", 1);
                    await Task.Delay(1500);
                    EnviaComando("3");
                    await Task.Delay(7000);
                    speak = new Speak(conductor, "Acabei de receber informações que o convidado chegou", 1);
                    await Task.Delay(7000);
                    contador = 0;
            }

                await Task.Delay(4000);
                allowRecognition = true;

            }

        void Movimentos()
            {
                long currentTimeMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                //switch (rand.Next(0, 2))
                //{
                    //case 0: conductor.Set("Eyelids Blink", true); ; break;
                    //case 1: break;
                //}

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

        void EnviaComando(String comando)
        {
            IPAddress ipAddress = IPAddress.Parse("255.255.255.255"); // IP do ESP
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 444); // Porta do ESP

            try
            {
                string message = comando; // Mensagem a ser enviada
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

        private void Cena_FormClosed(object sender, FormClosedEventArgs e)
        {
            udpClient.Close();
            timer1.Stop();
            reconhecedor.RecognizeAsyncCancel();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
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
        }

        private void button_fechar_Click(object sender, EventArgs e)
        {
            EnviaComando("3");
        }

        private void button_parar_Click(object sender, EventArgs e)
        {
            EnviaComando("7");
        }

        private void button_abrir_Click(object sender, EventArgs e)
        {
            EnviaComando("4");
        }

        private void button_ligar_Click(object sender, EventArgs e)
        {
            EnviaComando("5");
        }

        private void button_desligar_Click(object sender, EventArgs e)
        {
            EnviaComando("6");
        }

        private void button_iniciar_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            conductor.Set("Eyelids Close", true); ;
            conductor.Set("Neck Front", true);
            contador = 0;
        }
    }
}
