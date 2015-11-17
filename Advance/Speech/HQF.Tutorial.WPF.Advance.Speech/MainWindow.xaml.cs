using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using Microsoft.Win32;

namespace HQF.Tutorial.WPF.Advance.Speech
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int WM_DRAWCLIPBOARD = 0x0308; // change notifications
        private const int WM_CHANGECBCHAIN = 0x030D; // another window is removed from the clipboard viewer chain
        private const int WM_CLIPBOARDUPDATE = 0x031D; // clipboard changed contents

        private IntPtr _HWndNextViewer; // next window
        private HwndSource _HWndSource; // this window

        private Brush _OldButtonBrush = SystemColors.ControlBrush;
        private readonly SpeechSynthesizer _SpeechSynthesizer = new SpeechSynthesizer();
        private string _Text = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetClipboardViewer(IntPtr xHWndNewViewer);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ChangeClipboardChain(IntPtr xHWndRemove, IntPtr xHWndNewNext);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SendMessage(IntPtr xHWnd, int xMessage, IntPtr xWParam, IntPtr xLParam);

        private void StartListeningToClipboard()
        {
            var lWindowInteropHelper = new WindowInteropHelper(this);
            _HWndSource = HwndSource.FromHwnd(lWindowInteropHelper.Handle);
            _HWndSource.AddHook(WinProc);
            _HWndNextViewer = SetClipboardViewer(_HWndSource.Handle); // set this window as a viewer
        } //

        private void StopListeningToClipboard()
        {
            ChangeClipboardChain(_HWndSource.Handle, _HWndNextViewer); // remove from cliboard viewer chain
            _HWndNextViewer = IntPtr.Zero;
            _HWndSource.RemoveHook(WinProc);
        } //

        private void SayIt(string xText)
        {
            if (string.IsNullOrWhiteSpace(xText)) return;
            _SpeechSynthesizer.Volume = (int) Slider_Volumne.Value;
            _SpeechSynthesizer.SpeakAsync(xText);
        } //

        private IntPtr WinProc(IntPtr xHwnd, int xMessageType, IntPtr xWParam, IntPtr xLParam, ref bool xHandled)
        {
            switch (xMessageType)
            {
                case WM_CHANGECBCHAIN:
                    if (xWParam == _HWndNextViewer) _HWndNextViewer = xLParam;
                    else if (_HWndNextViewer != IntPtr.Zero)
                        SendMessage(_HWndNextViewer, xMessageType, xWParam, xLParam);
                    break;

                case WM_DRAWCLIPBOARD:
                    SendMessage(_HWndNextViewer, xMessageType, xWParam, xLParam);

                    processWinProcMessage();
                    break;
            }

            return IntPtr.Zero;
        } //

        private void processWinProcMessage()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(processWinProcMessage);
                return;
            }

            if (!Clipboard.ContainsText()) return;
            var lPreviousText = _Text;
            _Text = Clipboard.GetText();
            if (_Text.Equals(lPreviousText)) return; // do not play the same text again
            InsertTextIntoTextBox(_Text);
            if (Checkbox_OnOff.IsChecked.Value) SayIt(_Text);
        } //

        private void InsertTextIntoTextBox(string xText)
        {
            if (!TextBox_Clipboard.Dispatcher.CheckAccess())
            {
                TextBox_Clipboard.Dispatcher.Invoke(() => InsertTextIntoTextBox(xText));
                return;
            }
            TextBox_Clipboard.Text = xText;
        } //

        private void Button_SayIt_Click(object xSender, RoutedEventArgs e)
        {
            if (_SpeechSynthesizer.State == SynthesizerState.Speaking)
            {
                _SpeechSynthesizer.SpeakAsyncCancelAll();
                return;
            }
            SayIt(TextBox_Clipboard.Text);
        } //

        private void TextBox_Clipboard_TextChanged(object xSender, TextChangedEventArgs e)
        {
            _Text = TextBox_Clipboard.Text;
        } //

        private void Window_Loaded(object xSender, RoutedEventArgs e)
        {
            var lVoices = _SpeechSynthesizer.GetInstalledVoices();
            if (lVoices.Count < 1) return;

            foreach (var xVoice in lVoices)
            {
                ComboBox_Voices.Items.Add(xVoice.VoiceInfo.Name);
            }
            ComboBox_Voices.SelectedIndex = 0;

            StartListeningToClipboard();
        } //

        private void Window_Closed(object xSender, EventArgs e)
        {
            StopListeningToClipboard();
        } //

        private void ComboBox_Voices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var xVoice = ComboBox_Voices.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(xVoice)) return;
            _SpeechSynthesizer.SelectVoice(xVoice);
        } //

        private void Slider_Volumne_ValueChanged(object xSender, RoutedPropertyChangedEventArgs<double> e)
        {
            _SpeechSynthesizer.Volume = (int) Slider_Volumne.Value;
        } //

        private void Button_Save_Click(object xSender, RoutedEventArgs e)
        {
            _OldButtonBrush = Button_Save.Background;
            Button_Save.Background = Brushes.Salmon;
            var lDialog = new SaveFileDialog();
            var lPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            lDialog.InitialDirectory = lPath;
            lDialog.FileOk += FileDialog_FileOk;
            lDialog.Filter = "All Files|*.*|WAV (*.wav)|*.wav";
            lDialog.FilterIndex = 2;
            lDialog.ShowDialog();
        } //

        private void FileDialog_FileOk(object xSender, CancelEventArgs e)
        {
            var lDialog = xSender as SaveFileDialog;
            if (lDialog == null) return;

            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => FileDialog_FileOk(xSender, e));
                return;
            }

            try
            {
                var lPathAndFile = lDialog.FileName;
                _SpeechSynthesizer.SetOutputToWaveFile(lPathAndFile);
                _SpeechSynthesizer.SpeakCompleted += SpeechSynthesizer_SpeakCompleted;
                SayIt(TextBox_Clipboard.Text);
                Button_Save.Background = _OldButtonBrush;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } //

        private void SpeechSynthesizer_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            _SpeechSynthesizer.SetOutputToDefaultAudioDevice();
            _SpeechSynthesizer.SpeakCompleted -= SpeechSynthesizer_SpeakCompleted;
        } //
    }
}