namespace FinamDownloader
{
    internal partial class Program
    {
        /// <summary>
        /// Бумага
        /// </summary>
        private class FinamIssuer
        {
            // id - не является уникльным полем
            public string Id { get; }

            /// <summary>
            /// Имя. например: BR-1.09(BRF9)
            /// </summary>
            public string Name { get; }


            /// <summary>
            /// Код. например: BRF9
            /// </summary>
            public string Code { get; }

            /// <summary>
            /// Рынок
            /// </summary>
            public string Market { get; }

            /// <summary>
            /// Количество значимых символов после запятой?
            /// todo check cooment
            /// </summary>
            private readonly int _decp;

            /// <summary>
            /// todo add comment
            /// </summary>
            private readonly int _child;

            /// <summary>
            /// todo add comment
            /// </summary>
            public string Url { get; }

            public FinamIssuer(string id, string name, string code, string market, int decp, int child, string url)
            {
                Id = id;
                Name = name;
                Code = code;
                Market = market;
                _decp = decp;
                _child = child;
                Url = url;
            }

            public string GetDescription(bool fullDescr = false)
            {
                return !fullDescr
                    ? $"{Id}\t{Name}\t{Code}\t{Market}"
                    : $"{Id}\t{Name}\t{Code}\t{Market}\t{_decp}\t{_child}\t{Url}";
            }

            public static string GetDescriptionHead(bool fullDescr = false)
            {
                return !fullDescr
                    ? "Id\tName\tCode\tMarket"
                    : "Id\tName\tCode\tMarket\tDecp\tChild\tUrl";
            }
        }
    }
}
