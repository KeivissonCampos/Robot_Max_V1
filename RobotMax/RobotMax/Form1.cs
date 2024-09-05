using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Xml;
using System.IO;
using NAudio.Wave;
using System.Threading;
using System.Reflection;
using System.Diagnostics;

namespace RobotMax
{
    public partial class Form1 : Form
    {
        List<RobotState> robotStates = new List<RobotState>();

        KeyboardConfiguration keyboard = new KeyboardConfiguration();
        MicrophoneConfiguration mic = new MicrophoneConfiguration();

        int lastWriteAudioSize = 320000;
        int lastWriteControlSize = 0;

        String lastAudioFile = "";

        bool enableKeystrokes = true;

        Conductor conductor = new Conductor();

        String currentFilename = "";

        bool repeatPlayback = false;

        bool calibrating = false;

        bool checkDotNetVersion()
        {
            using (Microsoft.Win32.RegistryKey ndpKey = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                {
                  if (versionKeyName.StartsWith("v4")) return true;
                }
            }

            return false;
        }

        public Form1()
        {
            InitializeComponent();

            if (!checkDotNetVersion())
              MessageBox.Show("Invalid .Net Framework. You need v4.0 in\norder for this application to run\n without error!");

            keyboard.SetConductor(ref conductor);
            mic.SetConductor(ref conductor);

            recorder.PlaybackStoppedCallback += new EventHandler(recorder_PlaybackStoppedCallback);
            recorder.ClipboardCallback += new EventHandler(recorder_ClipboardCallback);
             
            conductor.ConnectionChanged += new EventHandler(conductor_ConnectionChangedCallback);

            conductor.SetControls(ref robotStates, ref recorder, ref simulator);
            conductor.SetStates(ref robotStates);

            treeView.ExpandAll();
        }

/*
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                //cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                //cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT
                return cp;
            }
        }
*/

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            conductor.SetExpression(treeView.SelectedNode.Name);
        }

        private void toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name.Equals("playStripButton") || e.ClickedItem.Name.Equals("repeatStripButton"))
            {
                ExecutePlayback(e.ClickedItem.Name.Equals("repeatStripButton"));
            }
            else
            if (e.ClickedItem.Name.Equals("stopStripButton"))
            {
              ExecuteStop();
            }
            else
            if (e.ClickedItem.Name.Equals("pauseStripButton"))
            {
              ExecutePause();
            }
            else
            if (e.ClickedItem.Name.Equals("zoomInStripButton"))
            {
                recorder.ZoomIn();
            }
            else
            if (e.ClickedItem.Name.Equals("zoomOutStripButton"))
            {
                recorder.ZoomOut();
            }
            else
            if (e.ClickedItem.Name.Equals("cutStripButton"))
            {
                ExecuteCut();
            }
            else
            if (e.ClickedItem.Name.Equals("copyStripButton"))
            {
                ExecuteCopy();
            }
            else
            if (e.ClickedItem.Name.Equals("pasteStripButton"))
            {
                ExecutePaste();
            }
            else
            if (e.ClickedItem.Name.Equals("openStripButton"))
            {
                ExecuteOpen();
            }
            else
            if (e.ClickedItem.Name.Equals("saveStripButton"))
            {
                ExecuteSave();
            }
            else
            if (e.ClickedItem.Name.Equals("saveAsToolStripMenuItem"))
            {
                ExecuteSaveAs();
            }
            else
            if (e.ClickedItem.Name.Equals("insertStripButton"))
            {
              ExecuteInsertSilence();
            }
        }

        public void WriteFloat(XmlWriter writer, String name, float value)
        {
            writer.WriteStartElement(name);
            writer.WriteString(Convert.ToString(value));
            writer.WriteEndElement();
        }

        public void WriteControlData(XmlWriter writer, ref List<RobotState> states)
        {
            int i;
            for (i = 0; i < states.Count; i++)
            {
                writer.WriteStartElement("Expression");

                writer.WriteStartElement("Position");
                writer.WriteString(Convert.ToString(states[i].position));
                writer.WriteEndElement();

                WriteFloat(writer, "leftHorizontalEye", states[i].leftHorizontalEye);
                WriteFloat(writer, "leftVerticalEye", states[i].leftVerticalEye);
                WriteFloat(writer, "rightHorizontalEye", states[i].rightHorizontalEye);
                WriteFloat(writer, "rightVerticalEye", states[i].rightVerticalEye);
                WriteFloat(writer, "leftEyebrow", states[i].leftEyebrow);
                WriteFloat(writer, "rightEyebrow", states[i].rightEyebrow);
                WriteFloat(writer, "leftEyelid", states[i].leftEyelid);
                WriteFloat(writer, "rightEyelid", states[i].rightEyelid);
                WriteFloat(writer, "leftLip", states[i].leftLip);
                WriteFloat(writer, "rightLip", states[i].rightLip);
                WriteFloat(writer, "jaw", states[i].jaw);
                WriteFloat(writer, "neckTilt", states[i].neckTilt);
                WriteFloat(writer, "neckTwist", states[i].neckTwist);

                writer.WriteEndElement();
            }
        }

        private void WriteToFile(String filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            settings.NewLineChars = "\r\n";
            settings.NewLineHandling = NewLineHandling.Replace;

            MemoryStream memStream = new MemoryStream();
            XmlWriter writer =  XmlWriter.Create(filename, settings);

            writer.WriteStartDocument();

            writer.WriteStartElement("Sequence");

            writer.WriteStartElement("Audio_Format");
            byte[] fmt = recorder.GetAudioFormat();
            writer.WriteBase64(fmt, 0, fmt.Length);
            writer.WriteEndElement();

            writer.WriteStartElement("Audio_Data");
            byte[] data = recorder.GetAudioData();
            writer.WriteBase64(data, 0, data.Length);
            writer.WriteEndElement();

            writer.WriteStartElement("Control");
            WriteControlData(writer, ref robotStates);
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();

            lastWriteAudioSize = data.Length;
            lastWriteControlSize = robotStates.Count;
        }

        private MemoryStream readBase64AsStream(XmlNodeReader reader)
        {
            MemoryStream ms = new MemoryStream();

            byte[] buf = new byte[1024];
            int numRead = 0;
            do
            {
                //numRead = reader.ReadElementContentAsBase64(buf, 0, 1024);
                numRead = reader.ReadElementContentAsBase64(buf, 0, 1024);
                ms.Write(buf, 0, numRead);
            }
            while (numRead >= 1024);

            return ms;
        }

        private void OpenFromFile(String filename)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;

            try
            {
                XmlDocument xmlFile = new XmlDocument();
                xmlFile.Load(filename);
                XmlNodeReader reader = new XmlNodeReader(xmlFile);
                //reader.WhitespaceHandling = System.Xml.WhitespaceHandling.None;

                RobotState ss = null;

                robotStates.Clear();

                String nodeName = "";

                while (reader.Read())
                {
                    /*
                    if (reader.HasAttributes)
                    {
                        xmlContent += reader.GetAttribute("ID").ToString() + "| ";
                    }
                        * */
                    if (reader.NodeType == XmlNodeType.EndElement)
                    {
                    }
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        nodeName = reader.Name;
                        if (nodeName == "Expression")
                        {
                            ss = new RobotState();
                            robotStates.Add(ss);
                        }
                    }
                    if (reader.NodeType == XmlNodeType.Text)
                    {
                        if (nodeName == "Audio_Data")
                        {
                            byte[] dat = Convert.FromBase64String(reader.Value);
                            MemoryStream ms = new MemoryStream();
                            // test increase volume digital filter
                            //int i;
                            //for (i = 1; i < dat.Length; i+=2)
                            //    dat[i] = (byte)((dat[i]+dat[i+2]));
                            ms.Write(dat, 0, dat.Length);
                            recorder.SetAudioData(ms);
                        }
                        if (nodeName == "Audio_Format")
                        {
                            byte[] dat = Convert.FromBase64String(reader.Value);
                            MemoryStream ms = new MemoryStream();
                            ms.Write(dat, 0, dat.Length);
                            recorder.SetAudioFormat(ms);
                        }
                        if (nodeName == "Position")
                            ss.position = Convert.ToInt64(reader.Value);
                        if (nodeName == "leftHorizontalEye")
                            ss.leftHorizontalEye = (float)Convert.ToDouble(reader.Value);
                        if (nodeName == "leftVerticalEye")
                            ss.leftVerticalEye = (float)Convert.ToDouble(reader.Value);
                        if (nodeName == "rightHorizontalEye")
                            ss.rightHorizontalEye = (float)Convert.ToDouble(reader.Value);
                        if (nodeName == "rightVerticalEye")
                            ss.rightVerticalEye = (float)Convert.ToDouble(reader.Value);
                        if (nodeName == "leftEyebrow")
                            ss.leftEyebrow = (float)Convert.ToDouble(reader.Value);
                        if (nodeName == "rightEyebrow")
                            ss.rightEyebrow = (float)Convert.ToDouble(reader.Value);
                        if (nodeName == "leftEyelid")
                            ss.leftEyelid = (float)Convert.ToDouble(reader.Value);
                        if (nodeName == "rightEyelid")
                            ss.rightEyelid = (float)Convert.ToDouble(reader.Value);
                        if (nodeName == "rightLip")
                            ss.rightLip = (float)Convert.ToDouble(reader.Value);
                        if (nodeName == "leftLip")
                            ss.leftLip = (float)Convert.ToDouble(reader.Value);
                        if (nodeName == "neckTilt")
                            ss.neckTilt = (float)Convert.ToDouble(reader.Value);
                        if (nodeName == "neckTwist")
                            ss.neckTwist = (float)Convert.ToDouble(reader.Value);
                        if (nodeName == "jaw")
                            ss.jaw = (float)Convert.ToDouble(reader.Value);
                    }
                }

                conductor.SetStates(ref robotStates);
                Invalidate();

                lastWriteAudioSize = recorder.GetAudioData().Length;
                lastWriteControlSize = robotStates.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An Error Occured: " + ex.Message);
            }  
        }

        private void ExecuteSaveAs()
        {
            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Sequence|*.seq";
            saveFileDialog1.Title = "Save a Sequence";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                WriteToFile(saveFileDialog1.FileName);
                currentFilename = saveFileDialog1.FileName;
            }
        }

        private void ExecuteSave()
        {
            if ((currentFilename == null) || (currentFilename.Length <= 0))
                ExecuteSaveAs();
            else
                WriteToFile(currentFilename);
        }

        private void ExecuteOpen()
        {
            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Sequence|*.seq";
            openFileDialog1.Title = "Open a Sequence";
            openFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (openFileDialog1.FileName != "")
            {
                currentFilename = openFileDialog1.FileName;
                OpenFromFile(openFileDialog1.FileName);
            }
        }

        private void calibrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //enter calibration
            conductor.calibrating = true;
            mic.calibrating = true;
            calibrating = true;

            Calibration cal = new Calibration();
            cal.SetConductor(ref conductor);
            cal.SetCalibration(conductor.GetCalibration());
            cal.ShowDialog();

            // exit calibration
            conductor.calibrating = false;
            calibrating = false;
            mic.calibrating = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
          int audioLength = recorder.GetAudioData().Length;
          int controlLength = robotStates.Count;

          if ((lastWriteAudioSize != audioLength) || (lastWriteControlSize != controlLength))
          {
            DialogResult dialogResult = MessageBox.Show("You are about to exit without saving.\nDo you want to save you work?", "Unsaved Work", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
              ExecuteSaveAs();
            }
          }

          conductor.Play();
          conductor.Stop();
          conductor.Close();
        }

        private void insertTextToSpeechToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InsertTextToSpeech iword = new InsertTextToSpeech();
            WaveFormat currentWaveFormat = recorder.GetWaveFormat();
            iword.SetWaveFormat(currentWaveFormat);
            iword.ShowDialog();
            if (iword.DialogResult == DialogResult.OK)
            {
                MemoryStream partialWave = iword.GetWaveStream();
                if (partialWave != null)
                {
                    // insert actual audio bytes
                    recorder.InsertAudio(partialWave);

                    long position = recorder.getSelectionStart();
                    if (position < 0) position = 0;
                    long adjust = (int)(partialWave.Length / (currentWaveFormat.BitsPerSample / 8));

                    // shift all simulator views past that insert over by that amount
                    int i;
                    //find the start of the insert
                    for (i = 0; (i < robotStates.Count) && (robotStates[i].position < position); i++) ;
                    // add new states for each viseme
                    int j;
                    List<Viseme> visemes = iword.GetVisemes();
                    for (j = 0; j < visemes.Count(); j++)
                    {
                        RobotState ss = conductor.CreateStateFromViseme(visemes[j].viseme);
                        ss.position = position + visemes[j].position;
                        robotStates.Insert(i++, ss);
                    }
                    while (i < robotStates.Count)
                        robotStates[i++].position += adjust;

                    conductor.SetStates(ref robotStates);
                }
                recorder.Delete();
            }
        }

        private void loadAudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lastAudioFile.EndsWith("\\"))
                openAudioDialog.FileName = lastAudioFile + "Audio.wave";
            else
                openAudioDialog.FileName = lastAudioFile;

            openAudioDialog.AddExtension = true;
            openAudioDialog.DefaultExt = ".wave";
            openAudioDialog.Filter = "Audio files (*.wav)|*.wav|All files (*.*)|*.*";
            if (openAudioDialog.ShowDialog() == DialogResult.OK)
            {
                lastAudioFile = openAudioDialog.FileName;
                recorder.LoadAudio(lastAudioFile);
                lastWriteAudioSize = recorder.GetAudioData().Length;
                //"c:\\www\\Animatronic Head\\beat1.wav"
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (enableKeystrokes&&(!calibrating))
                keyboard.ProcessKey(e, false);

            base.OnKeyDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (enableKeystrokes && (!calibrating))
                keyboard.ProcessKey(e, true);

            base.OnKeyDown(e);
        }

        public void conductor_ConnectionChangedCallback(object sender, EventArgs e)
        {
            if (((EventArgs<string>)e).Value.Equals("connected"))
            {
                this.Invoke((MethodInvoker)delegate
                {
                    ConnectionState.Text = "Connected";
                    ConnectionState.BackColor = Color.FromArgb(192, 255, 192);
                    calibrationToolStripMenuItem.Enabled = true;
                });
            }
            else
            {
                this.Invoke((MethodInvoker)delegate
                {
                    ConnectionState.Text = "Disconnected";
                    ConnectionState.BackColor = Color.FromArgb(255, 192, 192);
                    calibrationToolStripMenuItem.Enabled = false;
                });
            }
        }

        public void recorder_PlaybackStoppedCallback(object sender, EventArgs e)
        {
            if (repeatPlayback)
            {
                conductor.Restart();
            }
            else
            {
                conductor.Stop();

                stopStripButton.Enabled = false;
                playStripButton.Enabled = true;
                pauseStripButton.Enabled = false;
                repeatStripButton.Enabled = true;
            }
        }

        private void keyboardConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            keyboard.Show();
        }

        private void ExecuteNew()
        {
          int audioLength = recorder.GetAudioData().Length;
          int controlLength = robotStates.Count;

          if ((lastWriteAudioSize != audioLength) || (lastWriteControlSize != controlLength))
          {
            DialogResult dialogResult = MessageBox.Show("You are about to clear the sequencer without saving.\nDo you want to save you work?", "Unsaved Work", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
              ExecuteSaveAs();
            }
          }
          
          robotStates.Clear();
          recorder.Reset();

          lastWriteAudioSize = recorder.GetAudioData().Length;
          controlLength = 0;
        }

        private void ExecuteCut()
        {
            conductor.ExecuteCut();

            cutStripButton.Enabled = false;
            cutToolStripMenuItem.Enabled = false;
            cutMovementToolStripMenuItem.Enabled = false;
            deleteToolStripMenuItem.Enabled = false;
            
            copyStripButton.Enabled = false;
            copyToolStripMenuItem.Enabled = false;

            pasteStripButton.Enabled = true;
            pasteToolStripMenuItem.Enabled = true;
        }

        public void recorder_ClipboardCallback(object sender, EventArgs e)
        {
            cutStripButton.Enabled = true;
            cutToolStripMenuItem.Enabled = true;
            cutMovementToolStripMenuItem.Enabled = true;
            deleteToolStripMenuItem.Enabled = true;

            copyStripButton.Enabled = true;
            copyToolStripMenuItem.Enabled = true;
        }

        private void ExecuteCopy()
        {
            copyStripButton.Enabled = false;
            copyToolStripMenuItem.Enabled = false;

            pasteStripButton.Enabled = true;
            pasteToolStripMenuItem.Enabled = true;

            conductor.ExecuteCopy();
        }

        private void ExecutePaste()
        {
            conductor.ExecutePaste();
        }

        private void ExecuteInsertSilence()
        {
            // bump all states by the inserted size
            long startSelectionPosition = recorder.getSelectionStart();

            InsertSilence dlg = new InsertSilence();
            dlg.ShowDialog();
            long offset = (long)((float)dlg.GetSeconds() * recorder.getOneSecondRange());
            if (dlg.DialogResult == DialogResult.OK)
            {
                int i;
                for (i = 0; (i < robotStates.Count); i++)
                    if (robotStates[i].position >= startSelectionPosition)
                        robotStates[i].position += offset;

                // actually insert the slient space
                recorder.InsertAudioSilence(startSelectionPosition, startSelectionPosition+offset);

                conductor.CalculateTriggers(ref robotStates, recorder.getOneSecondRange());
            }
        }

        private void ExecutePlayback(bool repeat)
        {
            stopStripButton.Enabled = true;
            playStripButton.Enabled = false;
            pauseStripButton.Enabled = true;
            repeatStripButton.Enabled = false;
            conductor.Play();
            repeatPlayback = repeat;
        }

        private void ExecuteStop()
        {
            repeatPlayback = false;
            conductor.Stop();

            stopStripButton.Enabled = false;
            playStripButton.Enabled = true;
            pauseStripButton.Enabled = false;
            repeatStripButton.Enabled = true;
        }

        private void ExecutePause()
        {
          conductor.Pause();
        }

        private void newStripButton_Click(object sender, EventArgs e)
        {
            ExecuteNew();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteNew();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteOpen();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteSave();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteSaveAs();
        }

        private void insertSilenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteInsertSilence();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteCut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteCopy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecutePaste();
        }

        private void cutMovementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            conductor.ExecuteRemoveMovement();
        }

        private void basedOnTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OverlayMouthMovements moves = new OverlayMouthMovements();
            WaveFormat currentWaveFormat = recorder.GetWaveFormat();
            moves.SetWaveFormat(currentWaveFormat);
            moves.ShowDialog();
            if (moves.DialogResult == DialogResult.OK)
            {
                long position = recorder.getSelectionStart();
                if (position < 0) position = 0;

                // shift all simulator views past that insert over by that amount
                int i;
                //find the start of the insert
                for (i = 0; (i < robotStates.Count) && (robotStates[i].position < position); i++) ;
                // add new states for each viseme
                int j;
                List<Viseme> visemes = moves.GetVisemes();
                for (j = 0; j < visemes.Count(); j++)
                {
                    RobotState ss = conductor.CreateStateFromViseme(visemes[j].viseme);
                    ss.position = position + visemes[j].position;
                    robotStates.Insert(i++, ss);
                }

                conductor.SetStates(ref robotStates);
            }
        }

        private void basedOnVolumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            conductor.InsertAmpMouthMovements();
        }

        private void microphoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (microphoneToolStripMenuItem.Checked)
            {
                SetMicrophone dlg = new SetMicrophone();
                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                {
                    mic.Start(dlg.getDeviceNumber(), dlg.getSensitivity());
                }
            }
            else
                mic.Stop();
        }

        private void showMouth_CheckedChanged(object sender, EventArgs e)
        {
            conductor.SetVisibleStates(showMouth.Checked, showEyes.Checked, showNeck.Checked);
        }

        private void showEyes_CheckedChanged(object sender, EventArgs e)
        {
            conductor.SetVisibleStates(showMouth.Checked, showEyes.Checked, showNeck.Checked);
        }

        private void showNeck_CheckedChanged(object sender, EventArgs e)
        {
            conductor.SetVisibleStates(showMouth.Checked, showEyes.Checked, showNeck.Checked);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            recorder.Invalidate();
            simulator.Invalidate();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            conductor.ExecuteDelete();
        }

        private void recordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SequenceRecorder dlg = new SequenceRecorder(conductor, keyboard);
            dlg.ShowDialog();
            if (dlg.DialogResult == DialogResult.OK)
            {
                conductor.Insert(dlg.getAudio(), dlg.getFrames());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (conductor.triggerPlay)
            {
                ExecutePlayback(false);
                conductor.triggerPlay = false;
            }
        }

        private void randomMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
          Random_Messages dlg = new Random_Messages(conductor);
          dlg.ShowDialog();
        }

        private void chatServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChatServer cht = new ChatServer(conductor);
            cht.ShowDialog();
        }

        private void iAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChatGPT GPT = new ChatGPT(conductor);
            GPT.ShowDialog();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                numericUpDown1.Enabled = true;
                numericUpDown2.Enabled = true;
            }
            else
            {
                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<int> len = conductor.Upload();
            String resultado = string.Join(",", len);
            String Write;

            byte[] audioData;
            audioData = recorder.GetAudioData();

            if (checkBox1.Checked)
            {
                using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
                {
                    saveFileDialog1.Filter = "Arquivo de Áudio (*.wav)|*.wav";
                    saveFileDialog1.Title = "Salvar Arquivo de Áudio";

                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string arquivoAudio = saveFileDialog1.FileName;
                            WaveFileWriter waveFileWriter = new WaveFileWriter(arquivoAudio, recorder.GetWaveFormat());
                            waveFileWriter.Write(audioData, 0, audioData.Length);

                            waveFileWriter.Flush();
                            waveFileWriter.Dispose(); // Liberando recursos explicitamente
                            waveFileWriter.Close();

                            MessageBox.Show("Arquivo de áudio salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ocorreu um erro ao salvar o arquivo de áudio: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
            {
                saveFileDialog1.Filter = "Arquivo de Texto (*.txt)|*.txt";
                saveFileDialog1.Title = "Salvar Arquivo de Texto";
                if (checkBox1.Checked)
                {
                    Write = numericUpDown1.Value.ToString() + "," + numericUpDown2.Value.ToString() + "," + resultado;
                }
                else
                {
                    Write = "0,0," + resultado;
                }
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        StreamWriter FileText = new StreamWriter(saveFileDialog1.FileName);
                        FileText.Write(Write, 0, Write.Length);

                        FileText.Flush();
                        FileText.Dispose();
                        FileText.Close();

                        MessageBox.Show("Arquivo de texto salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ocorreu um erro ao salvar o arquivo de texto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void reconhecimentoDeVozToolStripMenuItem_Click(object sender, EventArgs e)
        {
            voiceRecognition PagVoice = new voiceRecognition(conductor);
            PagVoice.ShowDialog();
        }

        private void cenaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cena PagVoice = new Cena(conductor);
            PagVoice.ShowDialog();
        }
    }
}

