using UnityEngine;
using UnityEngine.Networking;


namespace ClinicalTools.SimEncounters.MainMenu
{
    public class RegisterUser
    {
        protected IWebAddress WebAddress { get; }
        protected MessageHandler MessageHandler { get; }
        protected ServerDataReader<string> RegisterDataReader { get; }
        public RegisterUser(IWebAddress webAddress, MessageHandler messageHandler)
        {
            WebAddress = webAddress;
            MessageHandler = messageHandler;
            var statusesParser = new StringParser();
            RegisterDataReader = new ServerDataReader<string>(statusesParser);
        }

        private const string phpFile = "Login.php";
        private const string actionVariable = "ACTION";
        private const string registerAction = "register";
        private const string usernameVariable = "username";
        private const string passwordVariable = "password";
        private const string emailVariable = "email";

        /**
         * Downloads all available and applicable menu files to display on the main manu.
         * Returns them as a MenuCase item
         */
        public void Register(string username, string password, string email)
        {
            var url = WebAddress.GetUrl(phpFile);
            var form = CreateForm(username, password, email);

            var webRequest = UnityWebRequest.Post(url, form);
            RegisterDataReader.Completed += EncounterDataReader_Completed;
            RegisterDataReader.Begin(webRequest);
        }

        public WWWForm CreateForm(string username, string password, string email)
        {
            var form = new WWWForm();

            form.AddField(actionVariable, registerAction);

            form.AddField(usernameVariable, username);
            form.AddField(passwordVariable, password);
            form.AddField(emailVariable, email);

            return form;
        }

        private void EncounterDataReader_Completed(object sender, ServerResult<string> e)
        {
            if (e.Outcome != ServerOutcome.Success) {
                MessageHandler.ShowMessage(e.Message, true);
                return;
            }
            if (e.Result.StartsWith("Connection Granted")) {
                MessageHandler.ShowMessage("Success. Please check email (or spam folder) for verification", false);
                return;
            }

            var error = e.Result.Split(new string[] { "--" }, System.StringSplitOptions.None)[0];
            MessageHandler.ShowMessage(error, true);
        }
    }
}
