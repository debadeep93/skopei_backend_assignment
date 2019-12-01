using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SkopeiBackendAssignment.Entities
{
    public class User
    {
        [Key] // denotes primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // for auto increment on SQL Server (Identity(1,1))
        public long Id { get; set; }

        public string Name { get; set; }
        [Required]
        public string Email { get; set; }

        /** Audit fields **/
        [IgnoreDataMember]
        public DateTime DateModified { get; set; } = DateTime.UtcNow; // default value to current time
        [IgnoreDataMember]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow; // default value to current time
        [IgnoreDataMember]
        public bool Deleted { get; set; } = false; // defaults to false
    }
}
