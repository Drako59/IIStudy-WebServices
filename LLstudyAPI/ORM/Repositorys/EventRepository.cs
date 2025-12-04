using LLStudy_Models.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using System.Data;
using LLstudyWS.ORM.CreatorsModels;

namespace LLstudyWS.ORM
{
    public class EventRepository : Repository<Event>, IRepository<Event>
    {
        public EventRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

        //public bool Create(Event model)
        //{


        //    string sql = "INSERT INTO Events (event_name,date_event,details) VALUES(@Name, @Date, @Detail)";
        //    this.helperOledb.AddParameter("@Name", model.Event_name);
        //    this.helperOledb.AddParameter("@Date", model.Date_event);
        //    this.helperOledb.AddParameter("@Details", model.Details);
        //    return this.helperOledb.Insert(sql) > 0;


        //}

        //public bool Delete(string id)
        //{
        //    string sql = "DELETE * From Events WHERE Event_name = @Name";
        //    this.helperOledb.AddParameter("@Name", id);
        //    return this.helperOledb.Insert(sql) > 0;
        //}

        //public List<Event> GetAll()
        //{
        //    string sql = "SELECT * FROM Events";
        //    List<Event> events = new List<Event>();
        //    using (IDataReader reader = this.helperOledb.Select(sql))
        //    {
        //        while (reader.Read())
        //        {
        //            events.Add(this.modelCreators.EventCreator.CreateModel(reader));
        //        }
        //    }
        //    return events;  
        //}

        //public Event GetByID(string ID)
        //{
        //    string sql = "SELECT * FROM Events  WHERE event_name = @ID";
        //    this.helperOledb.AddParameter("@ID", ID);
        //    using (IDataReader reader = this.helperOledb.Select(sql))
        //    {
        //        reader.Read();
        //        return this.modelCreators.EventCreator.CreateModel(reader);
        //    }
        //}

        //public bool Update(Event model)
        //{
        //    string sql = @"UPDATE Events
        //                    SET
        //                        event_name = @NAME,
        //                        date_event = @Date,
        //                        details = @Details
        //                    WHERE
        //                        event_name = @NAME";

        //    this.helperOledb.AddParameter("@NAME", model.Event_name);
        //    this.helperOledb.AddParameter("@Date", model.Date_event);
        //    this.helperOledb.AddParameter("@Details", model.Details);

        //    return this.helperOledb.Update(sql) > 0;

        //}
    }
}
