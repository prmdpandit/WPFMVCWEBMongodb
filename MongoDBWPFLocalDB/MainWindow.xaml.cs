using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Shared;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Builders;

namespace MongoDBWPFLocalDB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Developer pramod kumar pandit
    /// blog prmdpandit.jbko.in
    /// prmdpandit@gmail.com
    /// </summary>
    public partial class MainWindow : Window
    {
        public class info
        {
            [BsonId]
            public MongoDB.Bson.BsonObjectId Id { get; set; }
            public int info_id { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public int age { get; set; }
        }
        MongoClient mongo = new MongoClient("mongodb://localhost");
        MongoServer server ;
        MongoDatabase database;
        MongoCollection<info> _infos;
        info _info;
        public MainWindow()
        {
            InitializeComponent();
            server = mongo.GetServer();
            server.Connect(); 
            database= server.GetDatabase("mydb");
            bindgrid();
        }
        public void reversBind(info _info)
        {
             
            textBox1.Text = _info.firstname;
            textBox2.Text = _info.lastname;
            textBox3.Text =_info.age.ToString();

        }
        public info Getinfo(int id)
        {
            IMongoQuery query = Query.EQ("info_id", id);
            return _infos.Find(query).FirstOrDefault();
           // IMongoQuery query = Query.EQ("info_id", id);
            //return _infos.Find(query).FirstOrDefault();
        }

        private void ConnectMongodb_Click(object sender, RoutedEventArgs e)
        {
           // insert the record in to the mydb info collaction<table> 
            _info = new info { info_id = (int)database.GetCollection("info").Count() + 1, firstname = textBox1.Text, lastname = textBox2.Text, age = Convert.ToInt16(textBox3.Text) };
            addinfo(_info);
            bindgrid();
        }

        public void addinfo(info _info)
        {
             
            _info.Id = ObjectId.GenerateNewId();
            
            _infos.Insert(_info);
            //return item;
        }

        public void bindgrid()
        {
            // bind the existing info collection<table> record in grid
            _infos = database.GetCollection<info>("info");
            infogrid.DataContext = _infos.FindAll();
            infogrid.ItemsSource = _infos.FindAll();
        }

        private void infogrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                info val = (info)e.AddedItems[0];
                _info = Getinfo(val.info_id);
                reversBind(_info);
            }
        }

        public void updateInfo(info _info)
       {
           IMongoQuery query = Query.EQ("info_id", _info.info_id);
           IMongoUpdate update = Update
               
               .Set("firstname", _info.firstname)

              .Set("lastname", _info.lastname)
              .Set("age", _info.age);
           SafeModeResult result = _infos.Update(query, update);
          // return result.UpdatedExisting;
            
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
           // _info = new info { info_id = (int)database.GetCollection("info").Count() + 1, firstname = textBox1.Text, lastname = textBox2.Text, age = Convert.ToInt16(textBox3.Text) };
            infogrid.Focusable = false;
            _info.firstname = textBox1.Text;
            _info.lastname = textBox2.Text;
            _info.age =Convert.ToInt16(textBox3.Text);
            updateInfo(_info);
            bindgrid();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Clear(); textBox2.Clear(); textBox3.Clear(); textBox1.Focus();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
             IMongoQuery query = Query.EQ("info_id", _info.info_id);
             SafeModeResult result = _infos.Remove(query); bindgrid();

        }
       

      
    }
}
