using static TNZAPI.NET.Core.Enums;

namespace TNZAPI.NET.Api.Messaging.Common.Components
{
    public class Keypad
    {
        public enum KeypadType { RouteNumber, Play, PlayFile };

        public int Tone { get; set; } = -1;
        public string RouteNumber { get; set; } = "";
        public string Play { get; set; } = "";
        public string PlayFile { get; set; } = "";
        public KeypadPlaySection PlaySection { get; set; } = KeypadPlaySection.None;
        public Attachment PlayFileData { get; set; } = new Attachment();

        public Keypad()
        {
            Tone = -1;
            RouteNumber = "";
            Play = "";
        }

        public Keypad(int tone, string route_number)
        {
            Tone = tone;
            RouteNumber = route_number;
            Play = "";
        }

        public Keypad(int tone, string route_number, string play)
        {
            Tone = tone;
            RouteNumber = route_number;
            Play = play;
        }

        public Keypad(int? tone = null, string routeNumber = null, string play = null, string playFile = null, KeypadPlaySection? playSection = null, Attachment playFileData = null)
        {
            Tone = tone ?? -1;
            RouteNumber = routeNumber ?? "";
            Play = play ?? "";
            PlayFile = playFile ?? "";
            PlaySection = playSection ?? KeypadPlaySection.None;
            PlayFileData = playFileData ?? new Attachment();
        }
    }
}
