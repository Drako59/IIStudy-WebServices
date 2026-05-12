using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using LLstudyWS.ORM.CreatorsModels;
using System.Data;
using System.Globalization;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LLstudyWS.ORM
{
    public class EventRepository : Repository<Event>, IRepository<Event>
    {
        public EventRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

        public List<EventDetail> GetEventsDetails()
        {
            string sql = "SELECT * FROM Events";
            List<EventDetail> events = new List<EventDetail>();
            string Format = "yyyy-MM-dd";
            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    Event Event = this.moderlRefCreator.CreateModel<Event>(reader);
                    EventDetail eventDetail = new EventDetail() { Date_event = Event.Date_event, Details = Event.Details, EventID = Event.EventID, Event_name = Event.Event_name };
                    if(DateTime.TryParseExact(
                                            eventDetail.Date_event,
                                            Format,
                                            CultureInfo.InvariantCulture,
                                            DateTimeStyles.None,
                                            out DateTime date))
                    {
                        eventDetail.Date = date;
                    }
                    else
                    {
                        throw new FormatException("Invalid date format. Expected yyyy-MM-dd.");
                    }

                    events.Add(eventDetail);
                }
                return events;
            }
        }
    }
}
