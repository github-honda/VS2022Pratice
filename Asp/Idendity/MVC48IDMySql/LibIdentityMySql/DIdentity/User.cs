﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using Microsoft.AspNet.Identity;

namespace LibIdentityMySql.DIdentity
{
    public class User: IUser
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }

        public User(string userName)
            : this()
        {
            UserName = userName;
        }

        public string Id { get; set; }

        public string UserName { get; set; }

        public virtual string Email { get; set; }

        /// <summary>
        /// True if the email is confirmed, default is false.
        /// </summary>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        /// The salted/hashed form of the user password
        /// </summary>
        public virtual string PasswordHash { get; set; }

        /// <summary>
        /// A random value that should change whenever a users credentials have changed (password changed, login removed).
        /// </summary>
        public virtual string SecurityStamp { get; set; }

        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// True if the phone number is confirmed, default is false
        /// </summary>
        public virtual bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Is two factor enabled for the user.
        /// </summary>
        public virtual bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        public virtual DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        /// Is lockout enabled for this user.
        /// </summary>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        /// Used to record failures for the purposes of lockout.
        /// </summary>
        public virtual int AccessFailedCount { get; set; }
    }
}
