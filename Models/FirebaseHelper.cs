using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza.Models
{
    public class FirebaseHelper
    {
        static FirebaseClient firebase;
        static FirebaseHelper instance;

        public static FirebaseHelper Instance
        {
            get
            {
                if (instance == null) instance = new FirebaseHelper();
                return instance;
            }
        }

        protected FirebaseHelper()
        {
        }

        public static async Task<FirebaseClient> Init()
        {
            firebase = new FirebaseClient(
              "https://lapizza-d38a3-default-rtdb.firebaseio.com/",
              new FirebaseOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult("mvz0mi2ODKEY5r6Qx83EN4QgFJkLxwXJhLawDgbc")
              });

            return await Task.FromResult(firebase);
        }

        internal static async Task<FirebaseClient> GetFirebase()
        {
            if (firebase == null)
            {
                firebase =  await Init();
            }
            return firebase;
        }
    }
}
