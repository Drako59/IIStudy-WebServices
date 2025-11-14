using LLStudy_Models.Models;
using System.Data;

namespace LLstudyWS.ORM
{
    public class EventCreator : IModelCreator<Event> 
    {
        public Event CreateModel(IDataReader dataReader) { 
            
            
            
            return new Event() {
                Event_name = Convert.ToString(dataReader["Event_name"]),
                Date_event = Convert.ToString(dataReader["Date_event"]),
                Details = Convert.ToString(dataReader["Details"]),
                EventID = Convert.ToString(dataReader["EventID"])


            }; 
        
        
        
        
        
        }

        
    }
}
