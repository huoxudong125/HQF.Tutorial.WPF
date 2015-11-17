Since two years I am using Text To Speech (TTS). The quality has improved a lot. The spoken text can be understood and the pronunciations have reached good levels; not perfect though. Since Windows 7 there is no need to pay for a professional voice anymore. The Microsoft Windows system voices are sufficient.
First, I wanted to build my own add-on for the Firefox browser. But I quickly realised that there are too many constraints. I am using a nice tool on my Samsung Note 4 to listen to web site texts on a daily basis. That works out very well.

Nevertheless, this tool here is for home PCs.
ClipboardTTS monitors the windows clipboard and displays the current text in a WPF TextBox. In case the CheckBox “Auto” is checked the application starts speaking the text immediately. You can also generate WAV files. Adding this feature only took a few extra lines, otherwise it would not have been worth it. There are only a few use cases.

The Clipboard class offered in .Net does not provide events to monitor clipboard changes. We therefore have to use the old fashioned 32bit Windows functions. Therefore the codes section starts with imports from the windows “user32.dll”. With these you can subscribe to updates. The event WinProc notifies the application about changes. Most of these messages are disregarded in this application. We are only interested in two types of them. The first one is the Windows clipboard chain WM_CHANGECBCHAIN. You have to store the following window of the system clipboard chain, because we must forward messages to that one. This is a weird technology, but who knows what it is good for. For sure it simplifies suppressing messages without the need for any cancellation flag.
WM_DRAWCLIPBOARD is the other type we are interested in. This message tells you that the clipboard context has changed. Have a look at the C# code, you’ll quickly understand.
You could argue that the .Net Clipboard class is not needed, after all the user32.dll can do everything we need. Well, I think we should include as much .Net as possible. This is the right way to stick to the future.

Don’t forget to reference the `System.Speech` library in Visual Studio.

The application itself is pretty short. This is due to two facts. Windows is offering good SAPI voices and acceptable .Net support to use these voices.
http://msdn.microsoft.com/en-us/library/ee125077%28v=vs.85%29.aspx

I don’t see the point to implement an add-on for Firefox to follow the multi-platform approach. Do I have to re-invent the wheel? And check out the existing add-ons. You can hardly understand the spoken text. Some of these add-ons offer multi-language support. Yeah, but come on! You cannot understand a word. We are not in the 1990s anymore. Computers have learnt speaking very well. What is the language support good for if you cannot understand anything?