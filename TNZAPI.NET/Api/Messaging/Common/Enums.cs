using System.ComponentModel;

namespace TNZAPI.NET.Api.Messaging.Common
{
    public static class Enums
    {
        public enum SendModeType { Live, Test };

        public enum MessageType { Email, Text, Voice }

        public enum WebhookCallbackType { JSON, XML };

        public enum ResultCode { Failed, Success };

        public enum StatusCode { Unknown, Received, Pending, Delayed, Remote, Error, CreditHold, Completed, Transmit };

        public enum MessageDataType { MessageToPeople, MessageToAnswerPhones, CallRouteMessageToPeople, CallRouteMessageToOperators, CallRouteMessageOnWrongKey };

        public enum TTSVoiceType { 
            Female1, 
            Male1,

            [Description("Arabic@Female1")]
            ArabicFemale1,

            [Description("Arabic-Gulf@Female1")]
            ArabicGulfFemale1,

            [Description("Catalan@Female1")]
            CatalanFemale1,

            [Description("Chinese-Cantonese@Female1")]
            ChineseCantoneseFemale1,

            [Description("Chinese-Mandarin@Female1")]
            ChineseMandarinFemale1,

            [Description("Danish@Female1")]
            DanishFemale1,

            [Description("Danish@Female2")]
            DanishFemale2,

            [Description("Dutch@Female3")]
            DutchFemale3,

            [Description("English-Australian@Female1")]
            EnglishAustralianFemale1,

            [Description("English-Australian@Female2")]
            EnglishAustralianFemale2,

            [Description("English-Australian@Male1")]
            EnglishAustralianMale1,

            [Description("English-British@Female1")]
            EnglishBritishFemale1,

            [Description("English-British@Female2")]
            EnglishBritishFemale2,

            [Description("English-British@Male1")]
            EnglishBritishMale1,

            [Description("English-British@Male2")]
            EnglishBritishMale2,

            [Description("English-Indian@Female1")]
            EnglishIndianFemale1,

            [Description("English-Indian@Female2")]
            EnglishIndianFemale2,

            [Description("English-Indian@Female3")]
            EnglishIndianFemale3,

            [Description("English-NewZealand@Female1")]
            EnglishNZFemale1,

            [Description("English-SouthAfrican@Female1")]
            EnglishSouthAfricanFemale1,

            [Description("English-US@Female1")]
            EnglishUSFemale1,

            [Description("English-US@Female2")]
            EnglishUSFemale2,

            [Description("English-US@Female3")]
            EnglishUSFemale3,

            [Description("English-US@Female4")]
            EnglishUSFemale4,

            [Description("English-US@Female5")]
            EnglishUSFemale5,

            [Description("English-US@Male1")]
            EnglishUSMale1,

            [Description("English-US@Male2")]
            EnglishUSMale2,

            [Description("English-US@Male3")]
            EnglishUSMale3,

            [Description("English-US@Child1")]
            EnglishUSChild1,

            [Description("English-US@Child2")]
            EnglishUSChild2,

            [Description("English-US@Child3")]
            EnglishUSChild3,

            [Description("English-Welsh@Female1")]
            EnglishWelshFemale1,

            [Description("Finnish@Female1")]
            FinnishFemale1,

            [Description("French@Female1")]
            FrenchFemale1,

            [Description("French@Female2")]
            FrenchFemale2,

            [Description("French@Male1")]
            FrenchMale1,

            [Description("French@Male2")]
            FrenchMale2,

            [Description("French-Canadian@Female1")]
            FrenchCanadianFemale1,

            [Description("French-Canadian@Female2")]
            FrenchCanadianFemale2,

            [Description("French-Canadian@Male1")]
            FrenchCanadianMale1,

            [Description("German@Female1")]
            GermanFemale1,

            [Description("German@Female2")]
            GermanFemale2,

            [Description("German@Male1")]
            GermanMale1,

            [Description("German@Male2")]
            GermanMale2,

            [Description("German-Austrian@Female1")]
            GermanAustrianFemale1,

            [Description("Hindi@Female1")]
            HindiFemale1,

            [Description("Hindi@Female2")]
            HindiFemale2,

            [Description("Icelandic@Female1")]
            IcelandicFemale1,

            [Description("Icelandic@Male1")]
            IcelandicMale1,

            [Description("Italian@Female1")]
            ItalianFemale1,

            [Description("Italian@Female2")]
            ItalianFemale2,

            [Description("Italian@Male1")]
            ItalianMale1,

            [Description("Italian@Male2")]
            ItalianMale2,

            [Description("Japanese@Female1")]
            JapaneseFemale1,

            [Description("Japanese@Female2")]
            JapaneseFemale2,

            [Description("Korean@Female1")]
            KoreanFemale1,

            [Description("Norwegian@Female1")]
            NorwegianFemale1,

            [Description("Norwegian@Female2")]
            NorwegianFemale2,

            [Description("Polish@Female1")]
            PolishFemale1,

            [Description("Polish@Female2")]
            PolishFemale2,

            [Description("Polish@Female3")]
            PolishFemale3,

            [Description("Polish@Male1")]
            PolishMale1,

            [Description("Polish@Male2")]
            PolishMale2,

            [Description("Portuguese-Brazilian@Female1")]
            PortugueseBrazilianFemale1,

            [Description("Portuguese-Brazilian@Female2")]
            PortugueseBrazilianFemale2,

            [Description("Portuguese-Brazilian@Male1")]
            PortugueseBrazilianMale1,

            [Description("Portuguese-Brazilian@Male2")]
            PortugueseBrazilianMale2,

            [Description("Portuguese-European@Female1")]
            PortugueseEuropeanFemale1,

            [Description("Portuguese-European@Male1")]
            PortugueseEuropeanMale1,

            [Description("Romanian@Female1")]
            RomanianFemale1,

            [Description("Russian@Female1")]
            RussianFemale1,

            [Description("Russian@Male1")]
            RussianMale1,

            [Description("Spanish-European@Female1")]
            SpanishEuropeanFemale1,

            [Description("Spanish-European@Female2")]
            SpanishEuropeanFemale2,

            [Description("Spanish-European@Male1")]
            SpanishEuropeanMale1,

            [Description("Spanish-European@Male2")]
            SpanishEuropeanMale2,

            [Description("Spanish-Mexican@Female1")]
            SpanishMexicanFemale1,

            [Description("Spanish-Mexican@Male1")]
            SpanishMexicanMale1,

            [Description("Spanish-US@Female1")]
            SpanishUSFemale1,

            [Description("Spanish-US@Female2")]
            SpanishUSFemale2,

            [Description("Spanish-US@Male1")]
            SpanishUSMale1,

            [Description("Spanish-US@Male2")]
            SpanishUSMale2,

            [Description("Swedish@Female1")]
            SwedishFemale1,

            [Description("Swedish@Female2")]
            SwedishFemale2,

            [Description("Turkish@Female1")]
            TurkishFemale1,

            [Description("Welsh@Female1")]
            WelshFemale1,
        };

        public enum ViewEditByOptions { Account, SubAccount, Department, No };
    }
}
