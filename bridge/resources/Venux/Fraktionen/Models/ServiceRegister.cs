namespace Venux.Service
{
    public class Service
    {
        public string name
        {
            get;
            set;
        }

        public string message
        {
            get;
            set;
        }

        public int telnr
        {
            get;
            set;
        }

        public string id
        {
            get;
            set;
        }

        public Service(string name, string message, int telnr, string id)
        {
            this.name = name;
            this.message = message;
            this.telnr = telnr;
            this.id = id;
        }
    }
}

// Copyright© Venux-Crimelife Script Mendosa // --- Angemeldet 1-0 --- //
