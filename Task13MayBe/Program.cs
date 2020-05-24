using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab10Library;

namespace Task13MayBe
{
    class Program
    {
        static void Main(string[] args)
        {
            MyNewCollection myFirstCollection = new MyNewCollection("First");
            MyNewCollection mySecondCollection = new MyNewCollection("Second");
            Journal jounFirst = new Journal();
            Journal jounSecond = new Journal();
            //Подписки
            myFirstCollection.CollectionCountChanged += jounFirst.CollectionCountChanged;
            myFirstCollection.CollectionReferenceChanged += jounFirst.CollectionReferenceChanged;
            myFirstCollection.CollectionReferenceChanged += jounSecond.CollectionReferenceChanged;
            mySecondCollection.CollectionReferenceChanged += jounSecond.CollectionReferenceChanged;
            //Добавление в 1
            Administer adm = new Administer("For","The",31,"Republic");
            myFirstCollection.Add(adm);
            myFirstCollection.Add(new Administer("q", "ad", 22, "frin"));
            myFirstCollection.Add(new Administer("Press", "F", 22, "To"));
            myFirstCollection.Add(new Administer("Pay", "Respect", 22, "Yeah"));
            myFirstCollection.Add(new Administer("America", "Fk", 22, "Yeah"));
            myFirstCollection.Add(new Worker("Pusk", "Analiz", 90, "Task"));
            myFirstCollection.Add(new Administer("Sborka", "Otladka", 22, "Test"));
            Console.WriteLine("First Collection");
            myFirstCollection.Remove(1);
            myFirstCollection.Remove(3);
            Engineer Eng = new Engineer("Jenni", "Stronger", 29, "Federation", "Fast");
            myFirstCollection[0] = Eng;
            myFirstCollection[1] = new Worker("Done","Fstrin",29,"Key");
            Console.WriteLine(jounFirst.ToString());
            Console.WriteLine("\nВторая коллекция \n ");
            //Добавление во 2
            mySecondCollection.Add(new Worker("Rammstein","Deutschland",23,"Austrong"));
            mySecondCollection.Add(new Worker("Diamant", "Frau", 31, "Das"));
            mySecondCollection.Add(new Administer("Auf", "Wieldersahen", 39, "Der"));
            mySecondCollection.Add(new Engineer("Und", "Katze", 32, "Hug", "Apfel"));
            mySecondCollection.Add(new Engineer("Jerry", "Dimka", 20, "Open", "Tuberkulez"));
            mySecondCollection.Remove(2);
            mySecondCollection[3] = new Administer("Done","Frau",29,"Ice");
            Console.WriteLine(jounSecond.ToString());
            Console.ReadLine();
        }
    }

    class MyCollection : List<Person>
    {
        
    }

    class CollectionHandlerEventArgs : System.EventArgs
    {
        public string CollectionId;
        public string Changes;
        public Person ChangedPerson;

        public CollectionHandlerEventArgs(string collectionId, string changes, Person p)
        {
            this.CollectionId = collectionId;
            this.Changes = changes;
            ChangedPerson = p;
        }

        public override string ToString()
        {
            return "Param: " + CollectionId + " // " + Changes + " // " + ChangedPerson.Show();
        }
    }

    
    class MyNewCollection : MyCollection
    {
        public delegate void CollectionHandler(object source, CollectionHandlerEventArgs args);//делегат
        public event CollectionHandler CollectionCountChanged;    //Добавление-удаление элементов
        public event CollectionHandler CollectionReferenceChanged;   //Новое значение

        public string CollectionID { get; set; }
        public MyCollection myCollection = new MyCollection();

        public MyNewCollection(string collectionId = "Unnamed collection")
        {
            myCollection = new MyCollection();
            this.CollectionID = collectionId;
        }

        public Person this[int index]
        {
            get
            {
                return myCollection[index];
            }
            set
            {
                OnCollectionReferenceChanged(this, new CollectionHandlerEventArgs(this.CollectionID, "changed",myCollection[index]));
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Changed from ");
                Console.ResetColor();
                Console.Write(value.Show());
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" to ");
                Console.ResetColor();
                Console.Write(myCollection[index].Show() + "\n");
                myCollection[index] = value;
            }
        }


        public bool Remove(int position)
        {
            OnCollectionCountChanged(this, new CollectionHandlerEventArgs(this.CollectionID, "delete", myCollection[position]));
            myCollection.RemoveAt(position);
            return true;
        }

        public bool Add(Person p)
        {
            myCollection.Add(p);
            OnCollectionCountChanged(this, new CollectionHandlerEventArgs(CollectionID, "add", myCollection[myCollection.Count-1]));
            return true;
        }

        //обработчик события CollectionCountChanged
        public virtual void OnCollectionCountChanged(object source, CollectionHandlerEventArgs args)
        {
            if (CollectionCountChanged != null)
                CollectionCountChanged(source, args);
        }
        //обработчик события OnCollectionReferenceChanged
        public virtual void OnCollectionReferenceChanged(object source, CollectionHandlerEventArgs args)
        {
            if (CollectionReferenceChanged != null)
                CollectionReferenceChanged(source, args);
        }

    }

    class JournalEntry
    {
        public string CollectionId;
        public string Changes;
        public Person ChangePerson;

        
        public JournalEntry(string collectionId, string changes, Person p)
        {
            this.CollectionId = collectionId;
            this.Changes = changes;
            ChangePerson = p;
        }

        public JournalEntry(CollectionHandlerEventArgs args)
        {
            CollectionId = args.CollectionId;
            Changes = args.Changes;
            ChangePerson = args.ChangedPerson;
        }

        public override string ToString()
        {
            return "JournalEntry: " + CollectionId + " // " + Changes + " // " + ChangePerson.Show();
        }
    }

    class Journal
    {
        private List<JournalEntry> journallList = new List<JournalEntry>();
        
        public Journal()
        {
            journallList = new List<JournalEntry>();
        }

        public void CollectionCountChanged(object sourse, CollectionHandlerEventArgs e)
        {
            JournalEntry je = new JournalEntry(e.CollectionId, e.Changes, e.ChangedPerson);
            journallList.Add(je);
        }
        public void CollectionReferenceChanged(object sourse, CollectionHandlerEventArgs e)
        {
            JournalEntry je = new JournalEntry(e.CollectionId, e.Changes, e.ChangedPerson);
            journallList.Add(je);
        }

        public string Shows(int i)
        {
            string str = "";
            str = journallList[i].CollectionId + " " + journallList[i].Changes + " " + journallList[i].ChangePerson +
                  "\n";
            return str;
        }

        public override string ToString()
        {
            string str = "";
            foreach (JournalEntry item in journallList)
            {
                str += item.CollectionId + " " + item.Changes + " " + item.ChangePerson.Show() + "\n";
            }

            return str;
        }
    }
}
