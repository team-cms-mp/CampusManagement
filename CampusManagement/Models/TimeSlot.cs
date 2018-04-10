//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CampusManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class TimeSlot
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TimeSlot()
        {
            this.TimeSlotCourseAllocations = new HashSet<TimeSlotCourseAllocation>();
        }

        public int TimeSlotID { get; set; }
        [Display(Name = "Time Slot")]
        public string TimeSlot1 { get; set; }
        [Display(Name = "Day")]
        public string DayName { get; set; }
        [Display(Name = "Room")]
        public int RoomID { get; set; }
        [Display(Name = "Duration(Minutes)")]
        public int DurationID { get; set; }
        [Display(Name = "Created On")]
        public Nullable<System.DateTime> CreatedOn { get; set; }
        [Display(Name = "Created By")]
        public Nullable<int> CreatedBy { get; set; }
        [Display(Name = "Is Active")]
        public string IsActive { get; set; }
        [Display(Name = "Modified On")]
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        [Display(Name = "Modified By")]
        public Nullable<int> ModifiedBy { get; set; }

        [Display(Name = "Starting Hour")]
        public int StartingHour { get; set; }
        [Display(Name = "Ending Hour")]
        public int EndingHour { get; set; }
        public int DurationMunutes { get; set; }

        public virtual Duration Duration { get; set; }
        public virtual Room Room { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeSlotCourseAllocation> TimeSlotCourseAllocations { get; set; }
    }

    public class TimeSlotsViewModel
    {
        public List<TimeSlot> TimeSlots { get; set; }
        public TimeSlot SelectedTimeSlot { get; set; }
        public string DisplayMode { get; set; }
    }
}