using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;


namespace FinamDownloader
{
    internal partial class Program
    {
        /// <summary>
        /// Эмитент
        /// </summary>
        private class FinamIssuer
        {
            public string Id { get; }

            public string Name { get; }

            public string Code { get; }

            public string Market { get; }

            public int Decp { get; }

            public int Child { get; }

            public string Url { get; }

            public FinamIssuer(string id, string name, string code, string market, int decp, int child, string url)
            {
                Id = id;
                Name = name;
                Code = code;
                Market = market;
                Decp = decp;
                Child = child;
                Url = url;
            }

            public string GetDescription(bool fullDescr = false)
            {
                return !fullDescr
                    ? $"{Id}\t{Name}\t{Code}\t{Market}"
                    : $"{Id}\t{Name}\t{Code}\t{Market}\t{Decp}\t{Child}\t{Url}";
            }

            public static string GetDescriptionHead(bool fullDescr = false)
            {
                return !fullDescr
                    ? "Id\tName\tCode\tMarket"
                    : "Id\tName\tCode\tMarket\tDecp\tChild\tUrl";
            }

            public override string ToString() {
                return $"{Name},{Id}";
            }
        }

        /// <summary>
        /// разбор файла www.finam.ru/cache/icharts/icharts.js
        /// </summary>
        private class Icharts
        {
            /// <summary>
            /// Список эмитентов
            /// </summary>
            public List<FinamIssuer> Issuers { get; }

            /// <summary>
            /// Форматы вывода данных
            /// </summary>
            public string[] DataFormats { get; }

            public Icharts(string fnIcharts)
            {
                // прочитаем все ненулевые строки в bufList
                var bufList = new List<string>();
                using (var reader = new StreamReader(fnIcharts, Encoding.Default))
                {
                    while (!reader.EndOfStream)
                    {
                        var buf = reader.ReadLine();
                        if (!string.IsNullOrWhiteSpace(buf))
                        {
                            bufList.Add(buf);
                        }
                    }
                }
                var bufArr = bufList.ToArray();


                // значимая часть каждой строки начинается с strBeg1 или strBeg2, поэтому нам нужна строка после данной подстроки
                string TruncateBgnPart(string str)
                {
                    const string templateBeg1 = " = [";
                    const string templateBeg2 = " = {";

                    // возвращаем строку, оставшуюся после вхождения подстроки template
                    string TruncateAfterTemplate(string str2, string template)
                    {
                        var idx = str2.IndexOf(template, StringComparison.Ordinal);
                        return str2.Substring(idx + template.Length);
                    }

                    return TruncateAfterTemplate(str, str.Contains(templateBeg1) ? templateBeg1 : templateBeg2);
                }

                // все строки заканчиваются на "];" или "};", поэтому обрежем в каждой строке последние 2 символа
                string TruncateEndPart(string str) => str.Substring(0, str.Length - 2);


                // обрежем не нужные части в каждой строке
                for (var i = 0; i < bufArr.Length; i++)
                {
                    var buf = bufArr[i];

                    buf = TruncateBgnPart(buf);
                    buf = TruncateEndPart(buf);

                    bufArr[i] = buf;
                }


                // делим строку по подстроке "','"
                string[] SplitSpecial(string str)
                {
                    const char ch = '~';
                    // убедимся что строка str не содержит символа ch
                    Assert.IsFalse(str.Contains(ch));

                    str = str.Replace("','", Convert.ToString(ch)); // заменим "','" на ch
                    str = str.Replace("'", ""); // удалим все одиночные апострофы
                    return str.Split(ch); // поделим по символу ch
                }


                // строки 0, 3, 4, 6, 7 можно разделить по ',' (запятая)
                // строки 1, 2, 5 можно разделить по подстроке "','"
                const char comma = ',';
                var aEmitentIds = bufArr[0].Split(comma);
                var aEmitentNames = SplitSpecial(bufArr[1]); // 1
                var aEmitentCodes = SplitSpecial(bufArr[2]); // 2
                var aEmitentMarkets = bufArr[3].Split(comma);
                var aEmitentDecp = bufArr[4].Split(comma);

                DataFormats = SplitSpecial(bufArr[5]); // 5

                var aEmitentChild = bufArr[6].Split(comma);
                var aEmitentUrls = bufArr[7].Split(comma);


                // убедимся что все строки распарсены верно
                var qty = aEmitentIds.Length;
                Assert.IsTrue(qty == aEmitentNames.Length &&
                              qty == aEmitentCodes.Length &&
                              qty == aEmitentMarkets.Length &&
                              qty == aEmitentDecp.Length &&
                              qty == aEmitentChild.Length &&
                              qty == aEmitentUrls.Length);

                Issuers = new List<FinamIssuer>();

                // осталось распарсить aEmitentDecp[i] и aEmitentUrls[i]
                for (var i = 0; i < qty; i++)
                {
                    var buf = aEmitentDecp[i].Split(':');
                    Assert.IsTrue(buf.Length == 2 && Equals(aEmitentIds[i], buf[0]));
                    var decp = Convert.ToInt32(buf[1]);

                    var child = Convert.ToInt32(aEmitentChild[i]);

                    aEmitentUrls[i] = aEmitentUrls[i].Replace(": ", ":");
                    aEmitentUrls[i] = aEmitentUrls[i].Replace("\"", "");
                    var buf2 = aEmitentUrls[i].Split(':');
                    Assert.IsTrue(buf2.Length == 2 && Equals(aEmitentIds[i], buf2[0]));
                    var url = buf2[1];

                    Issuers.Add(new FinamIssuer(aEmitentIds[i], aEmitentNames[i], aEmitentCodes[i], aEmitentMarkets[i], decp, child, url));
                }

                // tests
                //GetDublicateIds();
                //GetMarketsList();
                //GetUrlsList();
            }

            private void GetDublicateIds()
            {
                var idsList = new List<string>();
                var dublicateIdsList = new List<string>();

                foreach (var issuer in Issuers)
                {
                    if (idsList.Contains(issuer.Id))
                    {
                        dublicateIdsList.Add(issuer.Id);
                    }

                    idsList.Add(issuer.Id);
                }

                Console.WriteLine($"dublicateIdsList.Count = {dublicateIdsList.Count}");
                foreach (var id2 in dublicateIdsList)
                {
                    var issuers = Issuers.FindAll(iss => iss.Id == id2);
                    foreach (var issuer in issuers)
                    {
                        // Console.WriteLine(issuer.GetDescription());
                    }
                }
            }

            private void GetMarketsList()
            {
                var marketsList = new List<string>();

                foreach (var issuer in Issuers)
                {
                    if (!marketsList.Contains(issuer.Market))
                    {
                        marketsList.Add(issuer.Market);
                    }
                }

                Console.WriteLine($"marketsList.Count = {marketsList.Count}");
                foreach (var market in marketsList)
                {
                    //Console.WriteLine(market);
                }
            }

            private void GetUrlsList()
            {
                var urlsList = new List<string>();

                foreach (var issuer in Issuers)
                {
                    if (!urlsList.Contains(issuer.Url))
                    {
                        urlsList.Add(issuer.Url);
                    }
                }

                Console.WriteLine($"urlsList.Count = {urlsList.Count}");
                foreach (var url in urlsList)
                {
                    //Console.WriteLine(url);
                }
            }
        }
    }
}
