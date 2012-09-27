using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShareThings.Models;

namespace ShareThings.Content
{
    public class SessionThings
    {

#region PropertySessionConsts

        private static string LOGGED_IN_USER = "loggedInUser";
       

#endregion

        // Session Properties
        public static User LoggedInUser
        {
            get
            {
                return getSession<User>(LOGGED_IN_USER, null);
            }
            set
            {
                setSession(LOGGED_IN_USER, value);
            }
        }

        public static bool IsUserAdmin
        {
            get
            {
                return (LoggedInUser != null && LoggedInUser.isAdmin);
            }
        }

        public static bool UserLoggedIn(bool requireAdmin = false)
        {
            if (requireAdmin)
                return (LoggedInUser != null && IsUserAdmin);
            else
                return (LoggedInUser != null);
        }

        private static bool _isInAdminPanel = false;
        public static bool IsInAdminPanel
        {
            get
            {
                if (IsUserAdmin)
                    return _isInAdminPanel;
                else
                    return false;
            }
            set
            {
                if (IsUserAdmin)
                    _isInAdminPanel = value;
                else
                    _isInAdminPanel = false;
            }
        }


        #region private methods

        private static T getSession<T>(string sessionSlot, T defaultValue)
        {
            if (HttpContext.Current.Session[sessionSlot] == null)
            {
                HttpContext.Current.Session.Add(sessionSlot, defaultValue);
            }
            return (T)HttpContext.Current.Session[sessionSlot];
        }

        private static void setSession(string sessionSlot, object value)
        {
            if (HttpContext.Current.Session[sessionSlot] == null)
            {
                HttpContext.Current.Session.Add(sessionSlot, value);
            }
            else
            {
                HttpContext.Current.Session[sessionSlot] = value;
            }
        }

        #endregion

    }

    public class Security{
        public static string HashString(string str)
        {
            string rethash = "";
            try
            {

                System.Security.Cryptography.SHA1 hash = System.Security.Cryptography.SHA1.Create();
                System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
                byte[] combined = encoder.GetBytes(str);
                hash.ComputeHash(combined);
                rethash = Convert.ToBase64String(hash.Hash);
            }
            catch (Exception ex)
            {
                string strerr = "Error in HashCode : " + ex.Message;
            }
            return rethash;
        }
    }
}