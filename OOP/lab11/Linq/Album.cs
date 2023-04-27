using System;
using System.Collections.Generic;


namespace OopLabs.Linq
{
    internal class Album
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }

        public Album(string artist, string title, DateTime date)
        {
            Artist = artist;
            Title = title;
            Date = date;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1:dd.MM.yyyy})", Title, Date);
        }

        public static List<Album> GetAlbums()
        {
            return new List<Album>
				{
					new Album("The Beatles", "Revolver", new DateTime(1966, 08, 05)),
					new Album("The Beatles", "Please Please Me", new DateTime(1963, 03, 22)),
					new Album("The Beatles", "Abbey Road", new DateTime(1969, 09, 26)),
					new Album("The Beatles", "Help!", new DateTime(1965, 08, 06)),
					new Album("The Beatles", "Rubber Soul", new DateTime(1965, 12, 03)),
					new Album("The Beatles", "With The Beatles", new DateTime(1963, 11, 22)),

					new Album("The Rolling Stones", "Some Girls", new DateTime(1978, 06, 09)),
					new Album("The Rolling Stones", "Let It Bleed", new DateTime(1969, 12, 05)),
					new Album("The Rolling Stones", "Exile on Main St.", new DateTime(1972, 05, 12)),
					new Album("The Rolling Stones", "Aftermath", new DateTime(1966, 04, 15)),
					new Album("The Rolling Stones", "Between the Buttons", new DateTime(1967, 01, 20)),

					new Album("Elvis Presley", "How Great Thou Art", new DateTime(1967, 02, 27)),
					new Album("Elvis Presley", "Elvis Is Back!", new DateTime(1960, 04, 08)),
					new Album("Elvis Presley", "Elvis Country", new DateTime(1971, 01, 02)),
					new Album("Elvis Presley", "Elvis Presley", new DateTime(1956, 03, 23)),
				};
        }
    }
}

