

using LLStudy_Models.Validation;
using System;
using System.ComponentModel.DataAnnotations;



namespace LLStudy_Models.Models
{
	public class Event: Model
	{
		string event_name;
		string date_event;
		string details;
		string eventID;

        [Required]
		public string EventID { get;set; }
        [Required]
        [StringLength(maximumLength: 40, ErrorMessage = "Max Event name Length is 40.")]
        public string Event_name { get { return this.event_name; } set{this.event_name = value; ValidateProperty(value, nameof(this.Event_name)); } }
        [Required]
        [ValidDate(ErrorMessage = "The date isn't valid.")]
        public string Date_event { get { return this.date_event; } set {this.date_event = value; ValidateProperty(value, nameof(this.Date_event)); } }
        [Required]
        [StringLength(maximumLength: 255, ErrorMessage = "Max details length is 255.")]
        public string Details { get { return this.details; } set { this.details = value; ValidateProperty(value, nameof(this.Details)); } }
    }
}
