using SpeechLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Remoting.Messaging;

namespace RobotMax
{
    public partial class ChatGPT : Form
    {
        private const string FilePath = @"C:\Users\keivisson21\PycharmProjects\Projeto_AssistetGPT\resultado.txt";
        private CancellationTokenSource cancellationTokenSource;

        bool isRunning = true;

        Conductor conductor;
        SpVoiceClass spVoice = new SpVoiceClass();
        ISpeechObjectTokens tokens;
        int SelectedIndex = 0;

        Random rand = new Random((int)DateTime.Now.Ticks);
        long blinkEyeTime;
        long moveEyesTime;
        int moveEyesDirection;
        long moveNeckTime;
        int moveNeckDirection;

        public ChatGPT(Conductor cond)
        {
            conductor = cond;
            InitializeComponent();
        }

        private void ChatGPT_Load(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => MonitorFileChanges(cancellationTokenSource.Token));
        }

        private async Task MonitorFileChanges(CancellationToken cancellationToken)
        {
            string previousContent = null;

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // Verifica se o arquivo existe
                    if (File.Exists(FilePath))
                    {
                        // Lê o conteúdo atual do arquivo
                        string content = File.ReadAllText(FilePath, Encoding.UTF8);

                        // Verifica se o conteúdo do arquivo mudou
                        if (content != previousContent)
                        {
                            // Atualiza o TextBox na thread da interface do usuário
                            UpdateTextBox(content);

                            // Atualiza o conteúdo anterior para o novo conteúdo
                            previousContent = content;
                        }
                    }
                    else
                    {
                        // Se o arquivo não existe, limpa o TextBox
                        UpdateTextBox(string.Empty);

                        // Reseta o conteúdo anterior para null
                        previousContent = null;
                    }
                }
                catch (Exception ex)
                {
                    // Lidar com exceções aqui, se necessário
                    Console.WriteLine(ex.Message);
                }

                // Aguarda um curto intervalo antes de verificar novamente
                await Task.Delay(1000); // Aguarda 1 segundo
            }
        }

        private void UpdateTextBox(string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)(() => UpdateTextBox(text)));
            }
            else
            {
                textBox1.Text = text;
                Speak speak = new Speak(conductor, text, 1);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            cancellationTokenSource.Cancel(); // Cancela a operação de monitoramento ao fechar o formulário
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (isRunning)
            {
                if (randomEye.Checked)
                {
                    long currentTimeMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    if (blinkEyeTime < currentTimeMillis)
                    {
                        conductor.Set("Eyelids Blink", true);
                        blinkEyeTime = currentTimeMillis + rand.Next(100, 10000);
                    }

                    if (moveEyesTime < currentTimeMillis)
                    {
                        switch (moveEyesDirection)
                        {
                            case 0: conductor.Set("Eyes Half Left", true); break;
                            case 1: conductor.Set("Eyes Half Right", true); break;
                            case 2: conductor.Set("Eyes Half Up", true); break;
                            case 3: conductor.Set("Eyes Half Down", true); break;
                            case 4: conductor.Set("Eyes Center", true); break;
                        }
                        moveEyesTime = currentTimeMillis + rand.Next(100, 5000);
                        moveEyesDirection = (int)rand.Next(0, 5);
                    }

                    if (moveNeckTime < currentTimeMillis)
                    {
                        switch (moveNeckDirection)
                        {
                            case 0: conductor.Set("Neck Half Left", true); break;
                            case 1: conductor.Set("Neck Half Right", true); break;
                            case 2: conductor.Set("Neck Half Up", true); break;
                            case 3: conductor.Set("Neck Half Down", true); break;
                            case 4: conductor.Set("Neck Front", true); break;
                        }
                        moveNeckTime = currentTimeMillis + rand.Next(100, 10000);
                        moveNeckDirection = (int)rand.Next(0, 5);
                    }
                }
            }
        }
    }
}