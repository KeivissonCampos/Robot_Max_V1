﻿
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using NAudio.Wave;
using System.IO;
using SpeechLib;

namespace RobotMax
{
    public partial class Random_Messages : Form
    {
        bool isRunning=false;
        Conductor conductor;
        SpVoiceClass spVoice = new SpVoiceClass();
        ISpeechObjectTokens tokens;
        int SelectedIndex = 0;
        string [] messages = null;
        int triggerMessage = 0;

        Random rand = new Random((int)DateTime.Now.Ticks);
        long blinkEyeTime;
        long moveEyesTime;
        int moveEyesDirection;
        long moveNeckTime;
        int moveNeckDirection;

        public Random_Messages(Conductor cond)
        {
            conductor = cond;

            InitializeComponent();

            String savedText = RobotMax.Properties.Settings.Default.Random_Messages_Text;
            if (savedText.Length>0)
              messagesTextBox.Text = savedText;
            else
              messagesTextBox.Text = "Good morning! How are you?\r\nMy bring is an Arduino Microcontroller.\r\nIt is a nice day!\r\nDid you have your coffee yet?\r\nMy name is RobotMax!\r\nI am a robot? Are you a robot also?\r\nI am made by X Y Z bot.\r\nI have 13 movable parts.\r\nI am one year old. How old are you?\r\n";

            randomEye.Checked = RobotMax.Properties.Settings.Default.Random_Messages_Eyes_On;
            randomNeck.Checked = RobotMax.Properties.Settings.Default.Random_Messages_Neck_On;
        }

        private void Random_Messages_Load(object sender, EventArgs e)
        {
            tokens = spVoice.GetVoices("", "");

            for (int i = 0; i < tokens.Count; i++)
                comboBoxVoice.Items.Add(tokens.Item(i).GetAttribute("Name"));

            if (tokens.Count > 0)
                comboBoxVoice.SelectedIndex = 0;
        }

        private void Activate_Click(object sender, EventArgs e)
        {
          isRunning = true;

          Disable.Enabled = true;
          Activate.Enabled = false;
          messagesTextBox.Enabled = false;

          // save in case things go wrong
          Random_Messages_FormClosing(null, null);

          messages = messagesTextBox.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

          long currentTimeMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
          blinkEyeTime = currentTimeMillis + rand.Next(100, 10000);
          moveEyesTime = currentTimeMillis + rand.Next(100, 5000);
          moveEyesDirection = (int)rand.Next(0, 5);
          moveNeckTime = currentTimeMillis + rand.Next(100, 10000);
          moveNeckDirection = (int)rand.Next(0, 5);
        }

        private void Disable_Click(object sender, EventArgs e)
        {
          isRunning = false;

          Disable.Enabled = false;
          Activate.Enabled = true;
          messagesTextBox.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void comboBoxVoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedIndex = comboBoxVoice.SelectedIndex;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
          if (isRunning)
          {
            triggerMessage--;
            if (triggerMessage<=0)
            {
              triggerMessage = 30;
              if (messages != null)
              {
                int rand = new Random((int)DateTime.Now.Ticks).Next(0, messages.Length);
                Speak speak = new Speak(conductor, messages[rand], SelectedIndex);
              }
            }
            
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

        private void Random_Messages_FormClosing(object sender, FormClosingEventArgs e)
        {
          RobotMax.Properties.Settings.Default.Random_Messages_Text = messagesTextBox.Text;
          RobotMax.Properties.Settings.Default.Random_Messages_Eyes_On = randomEye.Checked;
          RobotMax.Properties.Settings.Default.Random_Messages_Neck_On = randomNeck.Checked;
          RobotMax.Properties.Settings.Default.Save();
        }

        private void Falar_Click(object sender, EventArgs e)
        {
            Speak speak = new Speak(conductor, messagesTextBox.Text, SelectedIndex);
        }

        private void Movimentos_Click(object sender, EventArgs e)
        {
            long currentTimeMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            Serial serial = new Serial();
            byte[] buffer;

            switch (rand.Next(0, 2))
            {
                case 0: conductor.Set("Eyelids Blink", true); ; break;
                case 1: break;
            }
            blinkEyeTime = currentTimeMillis + rand.Next(100, 10000);

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

        private void checkBox_teste_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_teste.Checked)
            {
                Falar.Visible = true;
                Movimentos.Visible = true;
            }
            else
            {
                Falar.Visible = false;
                Movimentos.Visible = false;
            }
        }
    }
}
