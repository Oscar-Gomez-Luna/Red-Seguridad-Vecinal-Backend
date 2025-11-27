using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Backend_RSV.Config
{
    public static class FirebaseInitializer
    {
        private static bool _initialized = false;
        public static void Initialize()
        {
            if (!_initialized)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("firebase-key.json") // ruta a tu archivo JSON de credenciales
                });

                _initialized = true;
            }
        }
    }
}
