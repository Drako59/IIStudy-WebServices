

using System;
using System.ComponentModel.DataAnnotations;



namespace LLStudy_Models.Model
{
	public class Event
	{
		string event_name;
		string date_event;
		string details;
        [Required]
        public string Event_name { get; set; }
        [Required]
        public string Date_event { get; set; }
		public string Details { get; set; }
    }
}
