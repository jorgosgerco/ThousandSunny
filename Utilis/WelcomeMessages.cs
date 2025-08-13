using System;
using System.Collections.Generic;

namespace ThousandSunny.Utilis
{
    public static class WelcomeMessages
    {
        private static readonly List<string> messages = new()
        {
            "@nickname ka hyrë në Egghead… por a do dalë ndonjëherë?",
            "@nickname po luan shah me Vegapunk 🧠 dhe po fiton! ♟️",
            "⚠️ ALERT: CP0 ka hyrë në Egghead për ta kapur @nickname! 🎯",
            "Seraphim S-@nickname është jashtë kontrollit 🧬🚨",
            "“Teknologjia është fuqi!” – bërtet @nickname mes pluhurit e zjarrit 🔥",
            "Kuma e teleportoi @nickname drejt të panjohurës 🚀",
            "Vegapunk i besoi @nickname sekretin më të madh të Qeverisë Botërore 🕵️‍♂️",
            "Koka e madhe e Vegapunk? Jo më e madhe se idetë e @nickname 😂",
            "CP0 vs @nickname: lufta për të ardhmen ka filluar! ⚔️",
            "Punk Records u thye… dhe fajin e kishte @nickname 💻☠️",
            "@nickname po hakon laboratorët e Egghead me stilin shqiptar 🇦🇱💥",
            "\"Unë nuk dua të jem shkencëtar... dua të jem i LIRË!\" – @nickname 🏴‍☠️",
            "Seraphim që fluturojnë, pacifista që shpërthejnë... dhe @nickname mes tyre 🤖💫",
            "Kizaru po vjen me shpejtësi drite… por @nickname është më i shpejtë ⚡👣",
            "Saturn zbriti nga qielli, por @nickname s’përkulet para askujt ☠️🪐",
            "Tashmë, Egghead s’është ishull… është një fushëbetejë ku @nickname është mbreti! 👑",
            "🔬 Teknologjia e qeverisë? E kalon @nickname me një USB të prishur 😂",
            "Punk-01, Punk-02, Punk-03… por vetëm një është real deal: @nickname 💾",
            "@nickname është më i vështirë për t’u kapur se një Seraphim invisible 🫥",
            "Të gjithë ndjekin Vegapunk… por @nickname ndjek zemrën! ❤️‍🔥",
            "Kur satelitët e Vegapunk bien… ngrihet @nickname si dritë mes errësirës 🕯️",
            "CP0 u përpoq të ndalojë @nickname… tani po ikin me frikë 🏃💨",
            "Egghead është ishulli i së ardhmes… dhe @nickname ËshtË E ARDHMJA 🔮",
            "Luffy ka Nikan ☀️, por @nickname ka mendjen e çmendur të revolucionit! 💡💣",
            "Qendra e komandës u kap… nga @nickname me një laptop të vjedhur 😂🖥️",
            "Kuma u përplas me dritën për ta mbrojtur @nickname… ky është respekti! 🧸",
            "Vegapunk tha: “@nickname do jetë trashëgimtari i ëndrrës sime!” 🌌",
            "Ishulli po shembet… por @nickname buzëqesh! Sepse liria është afër! 🌊🔥",
            "Nëse Egghead është porta drejt së ardhmes, atëherë @nickname është çelësi! 🗝️",
            "@nickname u nis nga Foosha Village 🛶 për në ⛩️ Grand Line!",
            "“Thesari i Roger është i imi!” – tha @nickname me sy që digjeshin 🔥",
            "Me 🇦🇱 në zemër dhe D në emër, @nickname është i pandalshëm!",
            "@nickname është në kërkim nga World Government 🏛️⚖️",
            "⛩️ @nickname hyri në Wano... dhe nuk u kthye më i njëjtë!",
            "“Nëse më vrisni, mos harroni… One Piece EKZISTON!” – @nickname ☠️",
            "Emri i tij është @nickname… dhe flamuri i tij është liri! 🏴‍☠️",
            "Nëse Roger do kishte njohur @nickname, historia do ishte ndryshe 📖",
            "🐉 Kaido u rrëzua… nga @nickname me një goditje të vetme!",
            "@nickname e ka fuqinë e Nika-s ☀️ por zemrën e një shqiptari 🇦🇱",
            "Armët antike? 💣 Jo më të fuqishme se fjala e @nickname!",
            "⛵ “Unë dua të jem i lirë!” – ulëriu @nickname mbi Thousand Sunny 🌞",
            "@nickname është më i çmendur se Buggy 🤡, më i zgjuar se Vegapunk 🧠",
            "Celestial Dragons 🐉 dridhen kur dëgjojnë emrin: @nickname",
            "Në ditarët e Oden 📓, @nickname ishte përmendur fshehtas…",
            "🔓 Joyboy është kthyer… por me një tjetër emër: @nickname!",
            "Dritat e Mary Geoise ndizen sapo @nickname afrohet… ⚡",
            "“One Piece s’është një objekt, është ideali i @nickname!” 🌍",
            "Me Sabo-n 🇦🇱, Luffy-n 🍖 dhe Law-n ⚔️, tani vjen @nickname 🔥",
            "Shpirti i Shqipërisë 🇦🇱 udhëton me @nickname në New World 🌊",
            "Ata që nënvlerësojnë @nickname… nuk e kuptojnë Vullnetin e D! 🔥",
            "Nëse @nickname prek Laugh Tale… bota do të ndryshojë përherë 🌅",
            "@nickname rrëzoi një admiral dhe qeshi si Luffy 😂🖐️",
            "“Ne do të udhëtojmë bashkë!” – tha @nickname para se të hypte mbi Going Merry ⚓",
            "⛩️ @nickname ka armë të fshehta më të rrezikshme se Pluton!",
            "“Pirateria nuk është krim… është liri!” – thotë gjithmonë @nickname 🏴‍☠️",
            "@nickname kaloi Impel Down dhe doli me më shumë ndjekës 😂",
            "“Edhe nëse bie, unë do ngrihem si Zoro!” – thotë @nickname ⚔️",
            "Ata që besojnë në ëndrra, ndjekin @nickname 🌠",
            "⛩️@nickname ka shpirtin e një samurai – nderi i tij nuk thyhet kurrë!",
            "💮 “Bushi no kokoro wa tsuyoi” – @nickname",
            "✨ @nickname u trajnua në dojo-n e Wano-s për të kapur të ardhmen!",
            "🍶 Pas një gotë sake, @nickname shpalli revolucionin!",
        };

        private static readonly Random rng = new();

        public static string GetRandomMessage()
        {
            int index = rng.Next(messages.Count);
            return messages[index];
        }
    }
}
