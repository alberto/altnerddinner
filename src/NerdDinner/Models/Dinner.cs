using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using NerdDinner.Helpers;

namespace NerdDinner.Models {
    [Bind(Include="Title,Description,EventDate,Address,Country,ContactPhone,Latitude,Longitude")]
    public class Dinner {

        public virtual bool IsHostedBy(string userName) {
            return HostedBy.Equals(userName, StringComparison.InvariantCultureIgnoreCase);
        }

        public virtual bool IsUserRegistered(string userName) {
            return Rsvps.Any(r => r.AttendeeName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
        }

        public virtual IEnumerable<RSVP> Rsvps
        {
            get { return _rsvps; }
        }

        private readonly IList<RSVP> _rsvps;

        public Dinner()
        {
            _rsvps = new List<RSVP>();
        }

        public virtual bool IsValid {
            get { return (GetRuleViolations().Count() == 0); }
        }

        public virtual DateTime EventDate { get; set; }

        public virtual int DinnerID { get; set; }

        public virtual string Title { get; set; }

        public virtual IEnumerable<RuleViolation> GetRuleViolations() {

            if (String.IsNullOrEmpty(Title))
                yield return new RuleViolation("Title is required", "Title");

            if (String.IsNullOrEmpty(Description))
                yield return new RuleViolation("Description is required", "Description");

            if (String.IsNullOrEmpty(HostedBy))
                yield return new RuleViolation("HostedBy is required", "HostedBy");

            if (String.IsNullOrEmpty(Address))
                yield return new RuleViolation("Address is required", "Address");

            if (String.IsNullOrEmpty(Country))
                yield return new RuleViolation("Country is required", "Address");

            if (String.IsNullOrEmpty(ContactPhone))
                yield return new RuleViolation("Phone# is required", "ContactPhone");

            if (!PhoneValidator.IsValidNumber(ContactPhone, Country))
                yield return new RuleViolation("Phone# does not match country", "ContactPhone");

            yield break;
        }

        public virtual string ContactPhone { get; set; }

        public virtual string Country { get; set; }

        public virtual string HostedBy { get; set; }

        public virtual string Address { get; set; }

        public virtual string Description { get; set; }

        public virtual void OnValidate(/*ChangeAction action*/) {
            if (!IsValid)
                throw new ApplicationException("Rule violations prevent saving");
        }

        public virtual double Distance(float latitude, float longitude)
        {
            var latitudeDelta = (Latitude - latitude) * Math.PI / 180.0;
            var longitudeDelta = (Longitude - longitude) * Math.PI / 180.0;
            // Intermediate result. what is this? i don't know.
            var a = Math.Sqrt(Math.Sin(latitudeDelta / 2.0)) + Math.Cos(Latitude) * Math.Cos(latitude)
                                                               * Math.Pow(Math.Sin(longitudeDelta / 2.0), 2);
            // Intermediate result c (great circle distance in Radians).
            var c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));
            const double EARTH_RADIUS_IN_KM = 6376.5;
            var distance = EARTH_RADIUS_IN_KM * c;
            return distance;
        }

        public virtual double Longitude { get; set; }

        public virtual double Latitude { get; set; }

        public virtual void AddRsvp(RSVP rsvp)
        {
            _rsvps.Add(rsvp);
            rsvp.Dinner = this;
        }
    }
}
