namespace WeatherAssistantBot
{
    /// <summary>
    /// Class to store the Name and Place in Memory during Conversation
    /// </summary>
    public class BotState : System.Collections.Generic.Dictionary<string, object>
    {
        private const string NameKey = "Name";
        private const string PlaceKey = "Place";
        private const string ServiceKey = "ServiceChoice";

        public BotState()
        {
            this[NameKey] = null;
            this[PlaceKey] = null;
            this[ServiceKey] = 0;
        }

        public string Name
        {
            get { return (string)this[NameKey]; }
            set { this[NameKey] = value; }
        }

        public string Place
        {
            get { return (string)this[PlaceKey]; }
            set { this[PlaceKey] = value; }
        }

        public int ServiceChoice
        {
            get { return (int)this[ServiceKey]; }
            set { this[ServiceKey] = value; }
        }

        //private string _Name;
        //private string _Place;
        //private int _ServiceChoice;

        //public BotState()
        //{
        //    this._Name = null;
        //    this._Place = null;
        //    this._ServiceChoice = 0;
        //}

        //public string Name
        //{
        //    get { return _Name; }
        //    set { _Name = value; }
        //}

        //public string Place
        //{
        //    get { return _Place; }
        //    set { _Place = value; }
        //}

        //public int ServiceChoice
        //{
        //    get { return _ServiceChoice; }
        //    set { _ServiceChoice = value; }
        //}
    }
}

