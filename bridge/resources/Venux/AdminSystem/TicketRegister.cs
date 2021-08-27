namespace Venux.Tickets
{
    public class Ticket
    {
        public string author
        {
            get;
            set;
        }

        public string description
        {
            get;
            set;
        }

        public int id
        {
            get;
            set;
        }

        public string bearbeiter
        {
            get;
            set;
        }

        public Ticket(string author, string description, int id, string bearbeiter)
        {
            this.author = author;
            this.description = description;
            this.id = id;
            this.bearbeiter = bearbeiter;
        }
    }
}

// Copyright© Venux-Crimelife Script Mendosa // --- Angemeldet 1-0 --- //
