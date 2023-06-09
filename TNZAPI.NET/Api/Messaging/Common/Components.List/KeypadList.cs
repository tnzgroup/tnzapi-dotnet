using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Helpers;
using Attachment = TNZAPI.NET.Api.Messaging.Common.Components.Attachment;

namespace TNZAPI.NET.Api.Messaging.Common.Components.List
{
    public class KeypadList
    {
        private ICollection<Keypad> Keypads { get; set; }

        public KeypadList()
        {
            Keypads = new List<Keypad>();
        }

        /// <summary>
        /// Adding keypad
        /// </summary>
        /// <param name="tone">Keypad for call connection (supports buttons 1-9)</param>
        /// <param name="keypadType">Type of keypad</param>
        /// <param name="keypadData">Keypad data - Route Number OR Play</param>
        /// <returns></returns>
        [ComVisible(false)]
        public KeypadList Add(int tone, Keypad.KeypadType keypadType, string keypadData)
        {
            Keypad keypad = new Keypad();
            keypad.Tone = tone;

            switch (keypadType)
            {
                case Keypad.KeypadType.RouteNumber:
                    keypad.RouteNumber = keypadData;
                    break;
                case Keypad.KeypadType.Play:
                    keypad.Play = keypadData;
                    break;
                case Keypad.KeypadType.PlayFile:
                    keypad.PlayFile = keypadData;
                    break;
            }

            Add(keypad);

            return this;
        }

        /// <summary>
        /// Adding keypad
        /// </summary>
        /// <param name="tone">Keypad for call connection (supports buttons 1-9)</param>
        /// <param name="routeNumber">Telephone number for call routing in dialling format</param>
        /// <param name="play">Message to play when tone is pressed</param>
        /// <returns></returns>
        [ComVisible(false)]
        public KeypadList Add(int tone, string routeNumber, string play)
        {
            Add(new Keypad(tone, routeNumber, play));

            return this;
        }

        /// <summary>
        /// Adding keypad
        /// </summary>
        /// <param name="tone">Keypad for call connection (supports buttons 1-9)</param>
        /// <param name="routeNumber">Telephone number for call routing in dialling format</param>
        /// <returns></returns>
        [ComVisible(false)]
        public KeypadList Add(int tone, string routeNumber)
        {
            Add(new Keypad(tone, routeNumber));

            return this;
        }

        /// <summary>
        /// Adding keypad
        /// </summary>
        /// <param name="keypad">Keypad</param>
        /// <returns></returns>
        public KeypadList Add(Keypad keypad)
        {
            if (keypad is null)
            {
                return this;
            }

            Keypads.Add(keypad);

            return this;
        }

        /// <summary>
        /// Adding keypad
        /// </summary>
        /// <param name="keypads">List of keypad</param>
        /// <returns></returns>
        [ComVisible(false)]
        public KeypadList Add(ICollection<Keypad> keypads)
        {
            if (keypads is null || keypads.Count() == 0)
            {
                return this;
            }

            foreach (var keypad in keypads)
            {
                Add(keypad);
            }

            return this;
        }

        public ICollection<Keypad> ToList()
        {
            if (Keypads is null)
            {
                return null;
            }

            var list = Keypads.ToList();

            foreach (var keypad in list)
            {
                // Connvert PlayFile string (file location) to Attachment object
                if (!string.IsNullOrEmpty(keypad.PlayFile))
                {
                    using (var attachment = Attachment.GetAttachment(keypad.PlayFile))
                    {
                        if (attachment is null)
                        {
                            continue;
                        }

                        keypad.PlayFileData = Mapper.Map<Attachment>(attachment);
                    }
                }
            }

            return list;
        }

        public async Task<ICollection<Keypad>> ToListAsync()
        {
            if (Keypads is null)
            {
                return null;
            }

            var list = Keypads.ToList();

            foreach (var keypad in list)
            {
                // Connvert PlayFile string (file location) to Attachment object
                if (!string.IsNullOrEmpty(keypad.PlayFile))
                {
                    using (var attachment = await Attachment.GetAttachmentAsync(keypad.PlayFile))
                    {
                        if (attachment is null)
                        {
                            continue;
                        }

                        keypad.PlayFileData = Mapper.Map<Attachment>(attachment);
                    }
                }
            }

            return list;
        }
    }
}
