using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DragDropSample
{
    public class ViewModel
    {
        private ObservableCollection<Model>? contactsInfo;
        private ObservableCollection<Model>? dragContactsInfo;
        public ViewModel() 
        {
            GenerateSource(50);
            GenerateDragItems();
        }

        public ObservableCollection<Model>? ContactsInfo
        {
            get { return contactsInfo; }
            set { this.contactsInfo = value; }
        }

        public ObservableCollection<Model>? DragContactsInfo
        {
            get { return dragContactsInfo; }
            set { this.dragContactsInfo = value; }
        }

        // Generate data for ListView.
        public void GenerateSource(int count)
        {
            ContactsInfo = new();
            int girlsCount = 0, boysCount = 0;
            for (int i = 0; i < count; i++)
            {
                var details = new Model()
                {
                    ContactImage = "people_circle" + (i % 19) + ".png",
                    IsAnimated = true
                };

                if (imagePosition.Contains(i % 19))
                    details.ContactName = CustomerNames_Boys[boysCount++ % 32];
                else
                    details.ContactName = CustomerNames_Girls[girlsCount++ % 93];

                ContactsInfo.Add(details);
            }
        }

        // Generate data used to be dragged into ListView from BindableLayout.
        public void GenerateDragItems()
        {
            DragContactsInfo = new();
            DragContactsInfo.Add(new Model() { ContactImage = "people_circle23.png", ContactName = "Michael" });
            DragContactsInfo.Add(new Model() { ContactImage = "people_circle26.png", ContactName = "Connor" });
            DragContactsInfo.Add(new Model() { ContactImage = "people_circle27.png", ContactName = "Ryan" });
            DragContactsInfo.Add(new Model() { ContactImage = "people_circle18.png", ContactName = "Logan" });
            DragContactsInfo.Add(new Model() { ContactImage = "people_circle8.png", ContactName = "Mason" });
            DragContactsInfo.Add(new Model() { ContactImage = "people_circle14.png", ContactName = "Steve" });
        }
        internal Model DraggedItem
        {
            get; set;
        }

        #region details
        readonly int[] imagePosition = new int[]
        {
            5,
            8,
            12,
            14,
            18
        };

        readonly string[] CustomerNames_Girls = new string[]
        {
            "Kyle",
            "Gina",
            "Brenda",
            "Danielle",
            "Fiona",
            "Lila",
            "Jennifer",
            "Liz",
            "Pete",
            "Katie",
            "Vince",
            "Fiona",
            "Liam   ",
            "Georgia",
            "Elijah ",
            "Alivia",
            "Evan   ",
            "Ariel",
            "Vanessa",
            "Gabriel",
            "Angelina",
            "Eli    ",
            "Remi",
            "Levi",
            "Alina",
            "Layla",
            "Ella",
            "Mia",
            "Emily",
            "Clara",
            "Lily",
            "Melanie",
            "Rose",
            "Brianna",
            "Bailey",
            "Juliana",
            "Valerie",
            "Hailey",
            "Daisy",
            "Sara",
            "Victoria",
            "Grace",
            "Layla",
            "Josephine",
            "Jade",
            "Evelyn",
            "Mila",
            "Camila",
            "Chloe",
            "Zoey",
            "Nora",
            "Ava",
            "Natalia",
            "Eden",
            "Cecilia",
            "Finley",
            "Trinity",
            "Sienna",
            "Rachel",
            "Sawyer",
            "Amy",
            "Ember",
            "Rebecca",
            "Gemma",
            "Catalina",
            "Tessa",
            "Juliet",
            "Zara",
            "Malia",
            "Samara",
            "Hayden",
            "Ruth",
            "Kamila",
            "Freya",
            "Kali",
            "Leiza",
            "Myla",
            "Daleyza",
            "Maggie",
            "Zuri",
            "Millie",
            "Lilliana",
            "Kaia",
            "Nina",
            "Paislee",
            "Raelyn",
            "Talia",
            "Cassidy",
            "Rylie",
            "Laura",
            "Gracelynn",
            "Heidi",
            "Kenzie",
        };
        readonly string[] CustomerNames_Boys = new string[]
        {
            "Irene",
            "Watson",
            "Ralph",
            "Torrey",
            "William",
            "Bill",
            "Howard",
            "Daniel",
            "Frank",
            "Jack",
            "Oscar",
            "Larry",
            "Holly",
            "Steve",
            "Zeke",
            "Aiden",
            "Jackson",
            "Mason",
            "Jacob  ",
            "Jayden ",
            "Ethan  ",
            "Noah   ",
            "Lucas  ",
            "Brayden",
            "Logan  ",
            "Caleb  ",
            "Caden  ",
            "Benjamin",
            "Xaviour",
            "Ryan   ",
            "Connor ",
            "Michael",
        };
        #endregion
    }
}
